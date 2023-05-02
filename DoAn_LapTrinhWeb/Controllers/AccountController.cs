using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class AccountController : Controller
    {
        //gọi DbContext để sử dụng các Model
        private readonly DbContext db = new DbContext();
        //View Đăng nhập
        public ActionResult SignIn(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("SignIn", new { returnUrl = Request.UrlReferrer.ToString() });
            }
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            BannerGlobal();
            return View();
        }
        // POST: /Account/ExternalLogin

        //Code xử lý đăng nhập
        [HttpPost]
        //script xử lý SignIn | path: Scripts/my_js/checkuseraccount.js"
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(Sigin model,string returnUrl)
        {
            //Mã hóa password dùng sha 256
            model.password = Crypto.Hash(model.password);
            //model.password = model.password;
            

            //nếu trùng username,password và status ="1 tức là đang hoạt dộng và 0 là vô hiệu hóa" thì đăng nhập thành công
            var DataItem = db.Accounts.Where(m => m.status == "1" && m.Email.ToLower() == model.Email && m.password == model.password).SingleOrDefault();
            var checkdisable = db.Accounts.Where(m => m.status == "2" && m.Email.ToLower() == model.Email && m.password == model.password).SingleOrDefault();
            var checkactivate = db.Accounts.Where(m => m.status == "0" && m.Email.ToLower() == model.Email && m.password == model.password).SingleOrDefault();
            bool create = false; ; bool read =false; ; bool update = false; bool delete = false; bool access = false; bool modify = false;

            if (checkdisable !=null)
            {
                Notification.set_flash1s("Tài khoản đã bị vô hiệu hoá", "danger");
            }
            else
            if (checkactivate != null)//kiểm tra tài khoản đã kích hoạt hay chưa
            {
                TempData["AccountID"] = checkactivate.account_id;
                TempData["EmailID"] = checkactivate.Email;
                String Activation_code = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(checkactivate.Email, Activation_code, "VerifyAccount");
                return RedirectToAction("EmailverificationRequired", "Account");
            }
            else if (DataItem != null)//check đúng tài khoản và mật khẩu chưa
            {
                List<RolesPermissions> permissions = DataItem.Roles.RolesPermissions.ToList();
                foreach (var permiss in permissions)
                {
                    if (permiss.permission_id == 1)
                    { create = true; }
                    if (permiss.permission_id == 2)
                    { read = true; }
                    if (permiss.permission_id == 3)
                    { modify = true; }
                    if (permiss.permission_id == 7)
                    { update = true; }
                    if (permiss.permission_id == 4)
                    { delete = true; }
                    if (permiss.permission_id == 5)
                    { access = true; }
                }
                //lưu thông tin khi sau khi đăng nhập
                var userData = new LoggedUserData
                {
                    UserId = DataItem.account_id,
                    Name = DataItem.Name,
                    Email = DataItem.Email,
                    Permission_create = create,
                    Permission_read = read,
                    Permission_update = update,
                    Permission_modify = modify,
                    Permission_delete = delete,
                    Permission_access = access,
                    RoleCode = DataItem.Roles.role_id,
                    Rolename= DataItem.Roles.role_name,
                    Avatar = DataItem.Avatar,
                    PhoneNumber = DataItem.Phone,
                };            
                FormsAuthentication.SetAuthCookie(JsonConvert.SerializeObject(userData), false);//tạo chuỗi json, lúc này userData sẽ có kiểu dữ liệu là {"UserId":1418,"Username":null,"Name":"Nguyễn Văn T","Email":"vn13012000@gmail.com","RoleCode":"1","Avatar":"/Images/svg/avatars/001-boy.svg","PhoneNumber":"0665656566","Address":null}
                Notification.set_flash1s("Đăng nhập thành công", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                return RedirectToAction("Index", "Home");
            }
            else//trường hợp cuối thì cho nó fase sai tài khoản hoặc mật khẩu
            {
                //fail thì thông báo cho người dùng 
                Notification.set_flash1s("Sai tài khoản hoặc mật khẩu", "danger");
            }
            BannerGlobal();
            return View(model);
        }
        //Đăng xuất và xóa cookie
        public ActionResult Logout(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("Logout", new { returnUrl = Request.UrlReferrer.ToString()});//tạo url khi đăng xuất, đăng xuất thành công thì quay lại trang trước đó
            }
            FormsAuthentication.SignOut();//đăng xuất
            Notification.set_flash1s("Đăng xuất thành công", "success");
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
        //View Đăng ký
        public ActionResult Register()
        {
           //Nếu đã đăng nhập thì không vô được trang đăng ký
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            BannerGlobal();
            return View();
        }
        //Code Xử lý đăng ký
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Exclude = "status,Requestcode")] Account accounts,Register model)
        {
            string fail = "";
            string success = "";
            //check email đã có trong hệ database chưa
            var checkemail = db.Accounts.Any(m => m.Email == model.Email);
            //check số điện thoại đã có trong hệ database chưa
            var checkphone = db.Accounts.Any(m => m.Phone == model.Phone);
            if (checkemail)
            {
                fail = "Email đã được sử dụng";
            }
            else if (checkphone)
            {
                fail = "SĐT đã được sử dụng";
            }
            else
            {
                accounts.role_id = Const.ROLE_MEMBER_CODE; //admin quyền là 0: thành viên quyền là 1,biên tập viên là 2, người kiểm duyệt là 3             
                accounts.Email = model.Email;
                accounts.create_by = model.Email;
                accounts.update_by = model.Email;
                accounts.Name = model.Name;
                accounts.Gender = "3";
                accounts.Avatar = "/Images/svg/avatars/001-boy.svg";
                if (model.Phone.StartsWith("84"))//convert nếu user nhập số điện thoại bắt đầu = 84 => 0
                {
                    model.Phone = Regex.Replace(model.Phone, @"84", "0");
                    accounts.Phone = model.Phone;
                }
                else
                {
                    accounts.Phone = model.Phone;
                }
                accounts.update_at = DateTime.Now;
                accounts.Dateofbirth = DateTime.Now;
                accounts.expired_at = DateTime.Now.AddMinutes(10);
                //tạo chuỗi code kích hoạt tài khoản
                accounts.status = "0";
                model.Requestcode = Guid.NewGuid().ToString();
                accounts.Requestcode = model.Requestcode;
                //Gửi request code đến email bạn đăng ký tài khoản, nếu không muốn gửi request code thì chuyển status ="1", comment  model.Requestcode = Guid.NewGuid().ToString(); và SendVerificationLinkEmail(model.Email, model.Requestcode, "VerifyAccount");
                SendVerificationLinkEmail(model.Email, accounts.Requestcode, "VerifyAccount");
                //do password có nhiều ràng buộc "validdation nên phải thêm" không thêm sẽ báo lõi "Validation failed for one or more entities" 
                db.Configuration.ValidateOnSaveEnabled = false;
                //hash password và không cho khoảng trắng
                accounts.password = Crypto.Hash(model.password.Trim());
                accounts.create_at = DateTime.Now;
                db.Accounts.Add(accounts);
                //add dữ liệu vào database
                db.SaveChanges();
                TempData["AccountID"] = accounts.account_id;//truyền sang EmailverificationRequired
                TempData["EmailID"] = accounts.Email;//truyền sang EmailverificationRequired
                return RedirectToAction("EmailverificationRequired", "Account");
            }
            ViewBag.Success = success; 
            ViewBag.Fail = fail;
            BannerGlobal();
            return View(model);
        }
        //Gửi email xác thựctrang này chỉ tồn tại nếu bạn vừa dk xong, nếu bạn refesh lại trang thì trang này sẽ không hiện nữa
        //nếu bạn muuốn xác thực lại tk thì có thể dùng cách quên mật khẩu lúc đó tài khoản của bạn sẽ được kích hoạt khi bạn thay đổi mk thành công
        public ActionResult EmailverificationRequired()
        {
            ViewBag.AccountID = TempData["AccountID"];
            ViewBag.Email = TempData["EmailID"];
            if (ViewBag.AccountID == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                BannerGlobal();
                return View();
            }
        }
        //Gửi lại email xác thực
        public ActionResult ReSendVerification(Account model)
        {
            var account = db.Accounts.FirstOrDefault(m=>m.account_id==model.account_id);
            if (account != null)
            {
                string Activation_code = Guid.NewGuid().ToString();
                string EmailID = account.Email;
                SendVerificationLinkEmail(EmailID, Activation_code, "VerifyAccount");
                account.Email = account.Email;
                account.Requestcode = Activation_code;
                account.update_at = DateTime.Now;
                account.expired_at = DateTime.Now.AddMinutes(10);
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
            }
            return Json( JsonRequestBehavior.AllowGet);
        }
        //lấy danh sách quận huyện
        public JsonResult GetDistrictsList(int province_id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Districts> districtslist = db.Districts.Where(m => m.province_id == province_id).OrderBy(m => m.type).ThenBy(m=>m.district_name).ToList();
            return Json(districtslist,JsonRequestBehavior.AllowGet);
        }
        //lấy danh sách phường xã
        public JsonResult GetWardsList(int district_id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Wards> wardslist = db.Wards.Where(m => m.district_id == district_id).OrderBy(m => m.type).ThenBy(m=>m.ward_name).ToList();
            return Json(wardslist, JsonRequestBehavior.AllowGet);
        }
        //Code xử lý kích hoạt tài khoản sau khi đăng ký
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                bool activate = false;
                {
                    db.Configuration.ValidateOnSaveEnabled = false; 
                    var verify = db.Accounts.Where(a => a.Requestcode == new Guid(id).ToString()).FirstOrDefault();
                    if (verify != null && verify.expired_at>DateTime.Now)
                    {
                        verify.update_at = DateTime.Now;
                        verify.expired_at = DateTime.Now;
                        verify.status = "1";
                        verify.Requestcode = "";
                        db.SaveChanges();
                        activate = true;
                    }
                    else
                    {
                        ViewBag.Message = "Yêu cầu không hợp lệ";
                    }
                }
                ViewBag.Status = activate;
            }
            BannerGlobal();
            return View();
        }
        //View quên mật khẩu
        public ActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated)//nếu dã đăng nhập thì không thể gọi action "ForgotPassword"
            {
                return RedirectToAction("Index", "Home");//quay về trang chủ
            }
            BannerGlobal();
            return View();
        }
        //Code xử lý quên mật khẩu
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string fail = "";
            string success = "";
            var account = db.Accounts.Where(m => m.Email == EmailID && m.status!="2").FirstOrDefault(); // kiểm tra email đã trùng với email đăng ký tài khoản chưa, nếu chưa đăng ký sẽ trả về fail
            if (account != null)
            {
                string resetCode = Guid.NewGuid().ToString();
                //Gửi code reset đến mail đã nhập ở form quên mật khẩu 
                SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword"); 
                string sendmail = account.Email;
                account.Requestcode = resetCode; //request code phải giống reset code
                account.expired_at = DateTime.Now.AddMinutes(10);//email quên mật khẩu hết hạn sau 10p
                db.Configuration.ValidateOnSaveEnabled = false;//khi chạy action "ForgotPassword" và bị báo lỗi "Validation failed for one or more entities. See 'EntityValidationErrors'" thì thêm dòng này vô. Vì có quá nhiều validation trong một funtion nên báo lỗi.
                db.SaveChanges();
                success = "Đường dẫn reset password đã được gửi đến "+EmailID+" vui lòng kiểm tra email";
            }
            else
            {
                fail = "Email chưa tồn tại trong hệ thống hoặc tài khoản đã bị vô hiệu hóa"; // tài khoản không có trong hệ thống sẽ báo fail
            }
            
            ViewBag.Message1 = success;//truyền viewbag qua view của "ForgotPassword"
            ViewBag.Message2 = fail;//truyền viewbag qua view của "ForgotPassword"
            BannerGlobal();
            return View();
        }
        //View ResetPassword
        public ActionResult Resetpassword(string id)
        {
            var user = db.Accounts.Where(a => a.Requestcode == id).FirstOrDefault();
            if (user != null)
            {
                ResetPassword model = new ResetPassword();
                model.ResetCode = id;
                BannerGlobal();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //Code xử lý resetpassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Accounts.Where(m => m.Requestcode == model.ResetCode).FirstOrDefault();
                if (user != null && user.expired_at>DateTime.Now)
                {
                    user.password = Crypto.Hash(model.NewPassword);
                    //sau khi đổi mật khẩu thành công khi quay lại link cũ thì sẽ không đôi được mật khẩu nữa 
                    user.Requestcode = "";
                    user.update_by = user.Email;
                    user.update_at = DateTime.Now;
                    user.status = "1";
                    user.expired_at = DateTime.Now;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    Notification.set_flash("Cập nhật mật khẩu thành công", "success");
                    return RedirectToAction("SignIn");
                }
            }
            else
            {
                return HttpNotFound();
            }
            BannerGlobal();
            return View(model);
        }
        //Gửi Email xác nhận đăng ký, quên mật khẩu
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor)
        {
            // đường dẫn mail gồm có controller "Account"  +"emailfor" +  "code reset đã được mã hóa(mội lần gửi email quên mật khẩu sẽ random 1 code reset mới"
            var verifyUrl = "/Account/" + emailFor + "/" + activationCode;
            ///để dùng google email gửi email reset cho người khác bạn cần phải vô đây "https://www.google.com/settings/security/lesssecureapps" Cho phép ứng dụng kém an toàn: Bật
            var fromEmail = new MailAddress(AccountEmail.UserEmail, AccountEmail.Name); // "username email-vd: vn123@gmail.com" ,"tên hiển thị mail khi gửi"
            var toEmail = new MailAddress(emailID);
            //nhập password của bạn
            var fromEmailPassword = AccountEmail.Password; 
            string subject = "";
            //dùng body mail html , file template nằm trong thư mục "EmailTemplate/Text.cshtml"
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "MailConfirm" + ".cshtml");
            if (emailFor == "VerifyAccount")
            {
                subject = "Xác thực tài khoản " + emailID;            
                body = body.Replace("{{ViewBag.Sendmail}}", "Xác thực tài khoản");
                body = body.Replace("{{ViewBag.confirmtext}}", "Kích hoạt tài khoản");
                body = body.Replace("{{ViewBag.bodytext}}", "Email này có hiệu lực trong vòng <span style='font-weight:600;'>10 phút</span>, Vui lòng nhấn vào nút bên dưới để xác thực hoàn tất đăng ký cho tài khoản của bạn");
                body = body.Replace("{{viewBag.Confirmlink}}", Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl));//hiển thị nội dung lên form html
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Khôi phục mật khẩu cho "+emailID;
                body = body.Replace("{{ViewBag.Sendmail}}", "Khôi phục mật khẩu");
                body = body.Replace("{{ViewBag.confirmtext}}", "Thiết lập mật khẩu mới");
                body = body.Replace("{{ViewBag.bodytext}}", "Email này có hiệu lực trong vòng <span style='font-weight:600;'>10 phút</span>, Vui lòng nhấn vào nút bên dưới để đặt lại mật khẩu tài khoản của bạn. Nếu bạn không yêu cầu đặt lại mật khẩu mới, vui lòng bỏ qua email này");
                body = body.Replace("{{viewBag.Confirmlink}}", Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl));//hiển thị nội dung lên form html
            }
            var smtp = new SmtpClient
            {
                Host = AccountEmail.Host, //tên mấy chủ nếu bạn dùng gmail thì đổi  "Host = "smtp.gmail.com"
                Port = 587,
                EnableSsl = true, //bật ssl
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            smtp.Send(message);
        }
        //Quản lý địa chỉ cá nhân
        public ActionResult AddressManager()
        {
            ViewBag.ProvincesList = db.Provinces.OrderBy(m=>m.province_name).ToList();
            ViewBag.DistrictsList = db.Districts.OrderBy(m => m.type).ThenBy(m=>m.district_name).ToList();
            ViewBag.WardsList = db.Wards.OrderBy(m => m.type).ThenBy(m=>m.ward_name).ToList();
            if (User.Identity.IsAuthenticated)
            {
                int userid = User.Identity.GetUserId();
                ViewBag.Check_address = db.Account_Address.Where(m=>m.account_id==userid).Count();
                var account = db.Accounts.Where(m => m.account_id == userid).FirstOrDefault();
                ViewBag.Avatar = account.Avatar;
                ViewBag.AccountName = account.Name;
                var listaddress = (from a in db.Account_Address
                                  where a.account_id==userid
                                  orderby a.account_address_default descending 
                                  select new AddressManagerDTO 
                                    {
                                      phonenumber = a.account_address_phonenumber,
                                      username = a.account_address_username,
                                      province_name = a.Provinces.province_name,
                                      province_type = a.Provinces.type,
                                      district_name = a.Districts.district_name,
                                      district_type = a.Districts.type,
                                      ward_name = a.Wards.ward_name,
                                      ward_type = a.Wards.type,
                                      address_content = a.account_address_content,
                                      address_default=a.account_address_default,
                                      address_id=a.account_address_id,
                                      ward_id=a.ward_id,
                                      district_id=a.district_id,
                                      province_id=a.province_id
                                  }).ToList();
                BannerGlobal();
                return View(listaddress);
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }
        //Thay đổi địa chỉ mặc định
        public ActionResult DefaultAddress(int id)
        {
            if (User.Identity.IsAuthenticated) {
            var userid = User.Identity.GetUserId();
            var address = db.Account_Address.FirstOrDefault(m => m.account_address_id == id);
            var checkdefault = db.Account_Address.ToList();
            foreach (var item in checkdefault)
            {
                if (item.account_address_default == true && item.account_id == userid)
                {
                    item.account_address_default = false;
                    db.SaveChanges();
                }
            }
            address.account_address_default = true;
            db.SaveChanges();
            Notification.set_flash("Cập nhật địa chỉ mặc định thành công", "success");
            return RedirectToAction("AddressManager", "Account");
            }
            else
            {
                return RedirectToAction("Index", "Homde");
            }
        }
        //Thêm mới địa chỉ 
        public ActionResult AddressCreate(Account_Address address)
        {
            bool result;
            var userid = User.Identity.GetUserId();
            var checkdefault = db.Account_Address.Where(m=>m.account_id==userid).ToList();
            var limit_address = db.Account_Address.Where(m => m.account_id == userid).ToList();
            try
            {
                if (limit_address.Count() == 4 )//tối đa 4 ký tự
                {
                    result = false;
                }
                else
                {
                    foreach (var item in checkdefault)
                    {
                        if (item.account_address_default == true && address.account_address_default == true)
                        {
                            item.account_address_default = false;
                            db.SaveChanges();
                        }
                    }
                    address.account_id = userid;
                    db.Account_Address.Add(address);
                    db.SaveChanges();
                    result = true;
                    Notification.set_flash("Thêm thành công", "success");
                }
            }
            catch
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Sửa địa chỉ
        [HttpPost]
        public JsonResult AddressEdit(int id,string username,string phonenumber,int province_id,int district_id,int ward_id,string address_content)
        {
            var address = db.Account_Address.FirstOrDefault(m=>m.account_address_id==id);
            bool result;
            if (address != null)
            {
                address.province_id = province_id;
                address.account_address_username = username;
                address.account_address_phonenumber = phonenumber;
                address.district_id = district_id;
                address.ward_id = ward_id;
                address.account_address_content = address_content;
                address.account_id = User.Identity.GetUserId();
                db.SaveChanges();
                result = true;
                Notification.set_flash("Cập nhật thành công", "success");
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Xóa địa chỉ
        [HttpPost]
        public JsonResult AddressDelete(int id)
        {
            bool result;
            try
            {
                var address = db.Account_Address.FirstOrDefault(m => m.account_address_id == id);
                db.Account_Address.Remove(address);
                db.SaveChanges();
                result = true;
                Notification.set_flash("Xóa thành công", "success");
            }
            catch
            {
                result = false;
                Notification.set_flash("Lỗi", "danger");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //View chỉnh sửa thông tin cá nhân
        public ActionResult Editprofile()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var account = db.Accounts.Where(m => m.account_id == userId).FirstOrDefault();
                ViewBag.Avatar = account.Avatar;
                ViewBag.AccountName = account.Name;
                BannerGlobal();
                return View(account);
            }
            else
            {
                Notification.set_flash("Vui lòng đăng nhập", "danger");
                return RedirectToAction("SignIn", "Account");
            }
        }
        //Code chỉnh sửa thông tin cá nhân
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editprofile(Account model)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var account = db.Accounts.Where(m => m.account_id == userId).FirstOrDefault();
                if (model.ImageUpload == null)
                {
                    account.Avatar = account.Avatar;
                }
                else
                {
                    string fileName = Path.GetFileNameWithoutExtension(model.ImageUpload.FileName);
                    string extension = Path.GetExtension(model.ImageUpload.FileName);
                    fileName=  SlugGenerator.SlugGenerator.GenerateSlug(fileName) + "-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + extension;
                    account.Avatar = "/Images/ImagesAvatar/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/ImagesAvatar/"), fileName);
                    model.ImageUpload.SaveAs(fileName);
                }
                account.account_id = userId;
                account.Name = model.Name;
                //nếu user nhập sdt là 84, thì chuyển về số 0
                if (model.Phone.StartsWith("84"))
                {
                    model.Phone = Regex.Replace(model.Phone, @"84", "0");
                    account.Phone = model.Phone;
                }
                else
                {
                    account.Phone = model.Phone;
                }
                account.Dateofbirth = model.Dateofbirth;
                account.Gender = model.Gender;
                account.status = "1";
                account.update_by = User.Identity.GetEmail();
                account.update_at = DateTime.Now;
                //check nhiều validation thì phải cho nó false nếu không sẽ bị lỗi khi chạy đến đây
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                Notification.set_flash("Cập nhật thành công", "success");
                return RedirectToAction("Editprofile");
            }
            catch { 
             Notification.set_flash("Lỗi", "danger");
            }
            return View();
        }
        //View đổi mật khẩu
        public ActionResult ChangePassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var account= db.Accounts.Where(m => m.account_id == userId).FirstOrDefault();
                ViewBag.Avatar = account.Avatar;
                ViewBag.AccountName = account.Name;
                BannerGlobal();
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }
        //Thay đổi mật khẩu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword model)
        {
            var userId = User.Identity.GetUserId();
            model.OldPassword = Crypto.Hash(model.OldPassword);
            //model.OldPassword = model.OldPassword;
            
            string ckequal_newpass = Crypto.Hash(model.NewPassword);
            var account = db.Accounts.Where(m => m.account_id == userId).FirstOrDefault();
            var checkpassword = db.Accounts.Any(m => m.password == model.OldPassword && m.account_id == userId);
            if (checkpassword)
            {
                if (ckequal_newpass == account.password)
                {
                    Notification.set_flash("Mật khẩu mới và mật khẩu cũ không được trùng", "danger");
                }
                else
                {
                    if (account != null)
                    {
                        db.Configuration.ValidateOnSaveEnabled = false;
                        account.update_by = User.Identity.GetEmail();
                        account.password = Crypto.Hash(model.NewPassword);
                        account.update_at = DateTime.Now;
                        db.SaveChanges();
                        Notification.set_flash("Cập nhật mật khẩu thành công", "success");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Notification.set_flash("Lỗi", "danger");
                    }
                }

            }
            else
            {
                Notification.set_flash("Mật khẩu cũ không đúng", "danger");
            }
            BannerGlobal();
            return View();
        }
        //Kiểm tra danh sách đơn hàng của user
        public ActionResult TrackingOrder(int? page,int? size,string search,string sortOrder)//kiểm tra đơn hàng đã đặt và có phân trang cho phần đơn hàng dã đặt
        {
            if (User.Identity.IsAuthenticated)
            {
                var pageSize = size ?? 3;
                var pageNumber = page ?? 1;
                ViewBag.CurrentSort = sortOrder;
                ViewBag.ResetSort = String.IsNullOrEmpty(sortOrder) ? "" : "";
                ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_asc" : "date_desc";
                ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
                ViewBag.WaitingSortParm = sortOrder == "order_waiting" ? "order_waiting" : "order_waiting";
                ViewBag.ProcessingortParm = sortOrder == "order_processing" ? "order_processing" : "order_processing";
                ViewBag.CompleteSortParm = sortOrder == "order_complete" ? "order_complete" : "order_complete";
                ViewBag.CancleSortParm = sortOrder == "order_cancle" ? "order_cancle" : "order_cancle";
                //truyền viewbag của Deliveries qua view "TrackingOrder"
                ViewBag.Deli = db.Deliveries;
                //truyền view bag của payment qua view "TrackingOrder"
                ViewBag.Payment = db.Payments;
                ViewBag.itemOrder = db.Order_Detail.OrderByDescending(m => m.order_id);
                ViewBag.productOrder = db.Products;
                int userid = User.Identity.GetUserId();
                var account = db.Accounts.Where(m => m.account_id == userid).FirstOrDefault();
                ViewBag.Avatar = account.Avatar;
                ViewBag.AccountName = account.Name;
                BannerGlobal();
                var list = from a in db.Orders
                           where (a.account_id== userid)
                           orderby a.order_id descending
                           select a;

                if (!string.IsNullOrEmpty(search))
                {
                    list = (IOrderedQueryable<Order>)list.Where(s => s.order_id.ToString().Contains(search));
                    return View("TrackingOrder", list.ToPagedList(pageNumber, pageSize));
                }
                switch (sortOrder)
                {
                    case "date_asc":
                        ViewBag.sortname = "Xếp theo: Cũ nhất";
                        list = from a in db.Orders
                               where (a.account_id == userid)
                               orderby a.order_id ascending
                               select a;
                        break;
                    case "date_desc":
                        ViewBag.sortname = "Xếp theo: Mới nhất";
                        list = from a in db.Orders
                               where (a.account_id == userid)
                               orderby a.order_id descending
                               select a;
                        break;
                    case "price_asc":
                        ViewBag.sortname = "Xếp theo: Trị giá thấp - cao";
                        list = from a in db.Orders
                               where (a.account_id == userid)
                               orderby a.total ascending
                               select a;
                        break;
                    case "price_desc":
                        ViewBag.sortname = "Xếp theo: Trị giá cao - thấp";
                        list = from a in db.Orders
                               where (a.account_id == userid)
                               orderby a.total descending
                               select a;
                        break;
                    case "order_waiting":
                        ViewBag.sortname = "Xếp theo: Chờ xử lý";
                        list = from a in db.Orders
                               where (a.account_id == userid && a.status=="1")
                               orderby a.order_id descending
                               select a;
                        break;
                    case "order_processing":
                        ViewBag.sortname = "Xếp theo: Đang xử lý";
                        list = from a in db.Orders
                               where (a.account_id == userid && a.status == "2")
                               orderby a.order_id descending
                               select a;
                        break;
                    case "order_complete":
                        ViewBag.sortname = "Xếp theo: Hoàn thành";
                        list = from a in db.Orders
                               where (a.account_id == userid && a.status == "3")
                               orderby a.order_id descending
                               select a;
                        break;
                    case "order_cancle":
                        ViewBag.sortname = "Xếp theo: Bị huỷ";
                        list = from a in db.Orders
                               where (a.account_id == userid && a.status == "0")
                               orderby a.order_id descending
                               select a;
                        break;
                    default:
                        list = from a in db.Orders
                               where (a.account_id == userid)
                               orderby a.order_id descending
                               select a;
                        break;
                }
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }
        //view chi tiết đơn hàng
        [HttpPost]
        public ActionResult TrackingOrderDetail()
        {
          
                return View();
        }
        public ActionResult TrackingOrderDetail(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                int user_id = User.Identity.GetUserId();
                ViewBag.CheckExistFb = db.Feedbacks.Where(m => m.account_id == user_id).ToList();
                ViewBag.CheckEditOrder = db.Order_Detail.Where(m => m.Order.account_id == user_id && m.Order.status == "3").ToList();
                int userid = User.Identity.GetUserId();
                var orderdetail = db.Orders.FirstOrDefault(m => m.order_id == id && m.account_id == userid);
                if (orderdetail != null)
                {
                    var discountpriceorder = db.Order_Detail.FirstOrDefault(m => m.order_id == id);
                    var discount = db.Discounts.Where(m => m.discounts_code == discountpriceorder.discount_code).FirstOrDefault();
                    if (discount == null)
                    {
                        discount.discount_price = 0;
                    }
                    OrderDTOs order = new OrderDTOs
                    {
                        order_date = orderdetail.oder_date,
                        order_id = orderdetail.order_id,
                        payment_transaction = orderdetail.payment_transaction,
                        order_note = orderdetail.order_note,
                        payment_id = orderdetail.payment_id,
                        order_address_id = orderdetail.OrderAddress.order_address_id,
                        payment_name = orderdetail.Payment.payment_name,
                        Phone = orderdetail.Account.Phone,
                        Name = orderdetail.Account.Name,
                        order_address_content = orderdetail.OrderAddress.order_address_content,
                        order_address_phonenumber = orderdetail.OrderAddress.order_address_phonenumber,
                        order_address_times_edit = orderdetail.OrderAddress.times_edit_adress,
                        order_address_username = orderdetail.OrderAddress.order_address_username,
                        order_address_ward = orderdetail.OrderAddress.order_adress_wards,
                        order_address_district = orderdetail.OrderAddress.order_adress_district,
                        order_address_province = orderdetail.OrderAddress.order_adress_province,
                        status = orderdetail.status,
                        create_at = orderdetail.create_at,
                        total_price = orderdetail.total,
                        temporary = orderdetail.Order_Detail.Sum(m => m.price * m.quantity),
                        discount_price = discount.discount_price,
                        discount_type = discount.discounts_type,
                        discount_max = discount.discount_max
                    };
                    var account = db.Accounts.Where(m => m.account_id == userid).FirstOrDefault();
                    List<Order_Detail> product_orderdetail = db.Order_Detail.Where(m => m.order_id == id).ToList();
                    ViewBag.product_orderdetail = product_orderdetail;
                    ViewBag.Avatar = account.Avatar;
                    ViewBag.AccountName = account.Name;
                    TempData["OrderId"] = id;
                    BannerGlobal();
                    return View(order);
                }
                else
                {
                    return RedirectToAction("TrackingOrder", "Account");
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }
        //đổi địa chỉ nhận hàng trong chi tiết đơn hàng
        [HttpPost]
        public ActionResult ChangeOrderAddress(OrderDTOs model, int id, string address_content, string phonenumber, string username, string ward,string district,string province)
        {
            bool result;
            try
            {
                var order_adress = db.OrderAddress.FirstOrDefault(m => m.order_address_id == id);
                order_adress.order_address_content = address_content;
                order_adress.order_adress_wards = ward;
                order_adress.order_adress_district = district;
                order_adress.order_adress_province = province;
                order_adress.times_edit_adress = 1;
                order_adress.order_address_phonenumber = phonenumber;
                order_adress.order_address_username = username;
                db.SaveChanges();
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                result = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //Huỷ đơn hàng
        [HttpPost]
        public ActionResult CancleOrder(OrderDTOs model)
        {
            bool result = false;
            var order = db.Orders.FirstOrDefault(m => m.order_id == model.order_id);
            try
            {
                if (order != null && order.status == "1")
                {
                        order.status = "0";
                        order.update_by = User.Identity.GetName();
                        order.update_at = DateTime.Now;
                        db.SaveChanges();
                        result = true;
                }
                return Json(result,JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result,JsonRequestBehavior.AllowGet);
            }
        }
        //gợi ý search đơn hàng
        [HttpPost]
        public JsonResult GetOrderSearch(string Prefix)
        {
            var userid = User.Identity.GetUserId();
            var search = (from c in db.Orders
                          where c.account_id== userid && c.order_id.ToString().Contains(Prefix)
                          orderby c.order_id ascending
                          select new { c.order_id });
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //Lịch sử bình luận
        public ActionResult CommentHistory(int? page, int? size)
        {
            if (User.Identity.IsAuthenticated)
            {
            int userid = User.Identity.GetUserId();
            ViewBag.Check_address = db.Account_Address.Where(m => m.account_id == userid).Count();
            var account = db.Accounts.Where(m => m.account_id == userid).FirstOrDefault();
            ViewBag.Avatar = account.Avatar;
            ViewBag.AccountName = account.Name;
            var pageNumber = page ?? 1;
            var list = (from nc in db.NewsComments
                        join ns in db.News on nc.news_id equals ns.news_id
                        where ns.status == "1" && nc.account_id==userid && nc.status=="2"
                        orderby nc.comment_id descending
                        select new CommentHistoryDTOs
                        {
                            news_title = ns.news_title,
                            news_slug = ns.slug,
                            comment_content = nc.comment_content,
                            comment_at = nc.create_at,
                        });
            BannerGlobal();
            return View(list.ToPagedList(pageNumber, 5));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //Lịch sử đánh giá
        public ActionResult ReplyHistory(int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                int userid = User.Identity.GetUserId();
                ViewBag.Check_address = db.Account_Address.Where(m => m.account_id == userid).Count();
                var account = db.Accounts.Where(m => m.account_id == userid).FirstOrDefault();
                ViewBag.Avatar = account.Avatar;
                ViewBag.AccountName = account.Name;
                var pageNumber = page ?? 1;
                var list = (from rc in db.Reply_Comments
                            join nc in db.NewsComments on rc.comment_id equals nc.comment_id
                            join ns in db.News on nc.news_id equals ns.news_id
                            where ns.status == "1" && rc.account_id == userid && nc.status == "2" && rc.status == "2"
                            orderby nc.comment_id descending
                            select new CommentHistoryDTOs
                            {
                                news_title = ns.news_title,
                                news_slug = ns.slug,
                                reply_comment_content = rc.reply_comment_content,
                                reply_at = rc.create_at
                            });
                BannerGlobal();
                return View(list.ToPagedList(pageNumber, 5));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //Lịch sử phản hồi bình luận
        public ActionResult FeedbackHistory(int? page,string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.ResetSort = String.IsNullOrEmpty(sortOrder) ? "" : "";
                ViewBag.WaitingSortParm = sortOrder == "waiting" ? "waiting" : "waiting";
                ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
                ViewBag.CompleteSortParm = sortOrder == "complete" ? "complete" : "complete";
                ViewBag.CancleSortParm = sortOrder == "cancle" ? "cancle" : "cancle";
                int userid = User.Identity.GetUserId();
                var account = db.Accounts.Where(m => m.account_id == userid).FirstOrDefault();
                ViewBag.fb_img = db.Feedback_Image.ToList();
                ViewBag.Avatar = account.Avatar;
                ViewBag.AccountName = account.Name;
                var pageNumber = page ?? 1;
                var list = from fb in db.Feedbacks
                           join p in db.Products on fb.product_id equals p.product_id
                           join a in db.Accounts on fb.account_id equals a.account_id
                           where fb.parent_feedback_id == 0 && fb.account_id == userid
                           orderby fb.feedback_id descending
                           select new FeedbackDTOs
                           {
                               product_name = p.product_name,
                               product_slug = p.slug,
                               feedback_id = fb.feedback_id,
                               genre_id = p.genre_id,
                               discount_id = p.disscount_id,
                               description = fb.description,
                               rating_star = fb.rate_star,
                               status = fb.status,
                               create_at = fb.create_at,
                               Name = a.Name,
                               account_id = a.account_id,
                               product_id = fb.product_id,
                           };
                switch (sortOrder)
                {
                    case "date_asc":
                        ViewBag.sortname = "Xếp theo: Cũ nhất";
                        list = from fb in db.Feedbacks
                               join p in db.Products on fb.product_id equals p.product_id
                               join a in db.Accounts on fb.account_id equals a.account_id
                               where fb.parent_feedback_id == 0 && fb.account_id == userid
                               orderby fb.create_at ascending
                               select new FeedbackDTOs
                               {
                                   product_name = p.product_name,
                                   product_slug = p.slug,
                                   feedback_id = fb.feedback_id,
                                   genre_id = p.genre_id,
                                   discount_id = p.disscount_id,
                                   description = fb.description,
                                   rating_star = fb.rate_star,
                                   status = fb.status,
                                   create_at = fb.create_at,
                                   Name = a.Name,
                                   account_id = a.account_id,
                                   product_id = fb.product_id,
                               };
                        break;
                    case "date_desc":
                        ViewBag.sortname = "Xếp theo: Mới hất";
                        list = from fb in db.Feedbacks
                               join p in db.Products on fb.product_id equals p.product_id
                               join a in db.Accounts on fb.account_id equals a.account_id
                               where fb.parent_feedback_id == 0 && fb.account_id == userid
                               orderby fb.create_at descending
                               select new FeedbackDTOs
                               {
                                   product_name = p.product_name,
                                   product_slug = p.slug,
                                   feedback_id = fb.feedback_id,
                                   genre_id = p.genre_id,
                                   discount_id = p.disscount_id,
                                   description = fb.description,
                                   rating_star = fb.rate_star,
                                   status = fb.status,
                                   create_at = fb.create_at,
                                   Name = a.Name,
                                   account_id = a.account_id,
                                   product_id = fb.product_id,
                               };
                        break;
                    case "cancle":
                        ViewBag.sortname = "Xếp theo: Hủy bỏ";
                         list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.parent_feedback_id == 0 && fb.account_id == userid && fb.status=="0"
                                   orderby fb.feedback_id descending
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       Name = a.Name,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                        break;
                    case "complete":
                        ViewBag.sortname = "Xếp theo: Đã duyệt";
                         list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.parent_feedback_id == 0 && fb.account_id == userid && fb.status == "2"
                                   orderby fb.feedback_id descending
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       Name = a.Name,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                        break;
                    case "waiting":
                        ViewBag.sortname = "Xếp theo: Chờ duyệt";
                         list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.parent_feedback_id == 0 && fb.account_id == userid && fb.status=="1"
                                   orderby fb.feedback_id descending
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       Name = a.Name,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                        break;
                }
                BannerGlobal();
                return View(list.ToPagedList(pageNumber, 5));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //Kiểm tra user đã đăng nhập hay chưa
        public ActionResult UserLogged()
        {
            // Được gọi khi nhấn [Thanh toán]
            return Json(User.Identity.IsAuthenticated, JsonRequestBehavior.AllowGet);
        }
        //banner trên cùng, phải có để hiển thị toàn bộ layout
        public void BannerGlobal()
        {
            ViewBag.BannerTopHorizontal = db.Banners.OrderByDescending(m => Guid.NewGuid()).Where(m => m.banner_start < DateTime.Now && m.banner_end > DateTime.Now && m.status == "1" && m.banner_type == 3).Take(8).ToList();
        }

    }
}