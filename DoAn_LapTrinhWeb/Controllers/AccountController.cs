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
                //SendVerificationLinkEmail(checkactivate.Email, Activation_code, "VerifyAccount");
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
                //SendVerificationLinkEmail(model.Email, accounts.Requestcode, "VerifyAccount");
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
       //banner trên cùng, phải có để hiển thị toàn bộ layout
        public void BannerGlobal()
        {
            ViewBag.BannerTopHorizontal = db.Banners.OrderByDescending(m => Guid.NewGuid()).Where(m => m.banner_start < DateTime.Now && m.banner_end > DateTime.Now && m.status == "1" && m.banner_type == 3).Take(8).ToList();
        }

    }
}