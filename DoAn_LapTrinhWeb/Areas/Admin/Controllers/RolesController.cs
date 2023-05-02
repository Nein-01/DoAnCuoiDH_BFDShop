using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Mvc;
using DoAn_LapTrinhWeb;
using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Models;
using PagedList;
using static DoAn_LapTrinhWeb.DTOs.RolesDTO;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class RolesController : BaseController
    {
        private readonly DbContext db = new DbContext();
        //View list người dùng
        public ActionResult RolesIndex(string search, string role, string show, int? size, int? page, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = size ?? 10;
                    var pageNumber = page ?? 1;
                    ViewBag.ListRoles = db.Roles.ToList();
                    ViewBag.countTrash = db.Accounts.Count(a => a.status == "2"); // đếm user status 0 và role 1
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.ResetSort = "";
                    ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
                    ViewBag.EmailSortParm = sortOrder == "email_asc" ? "email_desc" : "email_asc";
                    ViewBag.PhoneSortParm = sortOrder == "phone_asc" ? "phone_desc" : "phone_asc";
                    var user = from a in db.Accounts
                               where (a.status == "1" || a.status == "0")
                               orderby a.account_id descending // giảm dần
                               select a;
                    switch (sortOrder)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Xếp theo: Mới nhất";
                            user = from a in db.Accounts
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.account_id descending // giảm dần
                                   select a;
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Xếp theo: Cũ nhất";
                            user = from a in db.Accounts
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.account_id
                                   select a;
                            break;
                        case "name_desc":
                            ViewBag.sortname = "Xếp theo: Tên người dùng (Z-A)";
                            user = from a in db.Accounts
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.Name descending // giảm dần
                                   select a;
                            break;
                        case "name_asc":
                            ViewBag.sortname = "Xếp theo: Tên người dùng (A-Z)";
                            user = from a in db.Accounts
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.Name
                                   select a;
                            break;
                        case "email_desc":
                            ViewBag.sortname = "Xếp theo: Tên email (Z-A)";
                            user = from a in db.Accounts
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.Email descending // giảm dần
                                   select a;
                            break;

                        case "email_asc":
                            ViewBag.sortname = "Xếp theo: Tên email (A-Z)";
                            user = from a in db.Accounts
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.Email
                                   select a;
                            break;
                        case "phone_desc":
                            ViewBag.sortname = "Xếp theo: Số điện thoại (9-0)";
                            user = from a in db.Accounts
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.Phone descending // giảm dần
                                   select a;
                            break;
                        case "phone_asc":
                            ViewBag.sortname = "Xếp theo: Số điện thoại (0-9)";
                            user = from a in db.Accounts
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.Phone
                                   select a;
                            break;
                    }
                    
                    if (string.IsNullOrEmpty(search)) return View(user.ToPagedList(pageNumber, pageSize));
                    switch (show)
                    {
                        //tìm kiếm tất cả
                        case "1":
                            user = (IOrderedQueryable<Account>)user.Where(s =>
                                s.account_id.ToString().Contains(search) || s.Email.Contains(search) ||
                                s.Name.Contains(search) || s.Phone.ToString().Contains(search)
                                || s.status.Contains(search));
                            break;
                        //theo id
                        case "2":
                            user = (IOrderedQueryable<Account>)user.Where(s => s.account_id.ToString().Contains(search));
                            break;
                        //theo email
                        case "3":
                            user = (IOrderedQueryable<Account>)user.Where(s => s.Email.Contains(search));
                            break;
                        //theo tên
                        case "4":
                            user = (IOrderedQueryable<Account>)user.Where(s => s.Name.Contains(search));
                            break;
                        //theo số điện thoại
                        case "5":
                            user = (IOrderedQueryable<Account>)user.Where(s => s.Phone.ToString().Contains(search));
                            break;
                        default:
                            ViewBag.search = search;
                            user = (IOrderedQueryable<Account>)user.Where(s => s.role_id.ToString().Contains(search));
                            break;
                    }
                    return View("RolesIndex", user.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //View list trash người dùng
        public ActionResult RolesTrash(string search, string show, int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = size ?? 5;
                    var pageNumber = page ?? 1;
                    var user = from a in db.Accounts
                               where a.status == "2"
                               orderby a.update_at descending // giảm dần
                               select a;
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1")) //tìm kiếm tất cả
                            user = (IOrderedQueryable<Account>)user.Where(s =>
                                s.account_id.ToString().Contains(search) || s.Email.Contains(search) ||
                                s.Name.Contains(search) || s.Phone.ToString().Contains(search)
                                || s.status.Contains(search));
                        else if (show.Equals("2")) //theo id
                            user = (IOrderedQueryable<Account>)user.Where(s => s.account_id.ToString().Contains(search));
                        else if (show.Equals("3")) //theo email
                            user = (IOrderedQueryable<Account>)user.Where(s => s.Email.Contains(search));
                        else if (show.Equals("4")) //theo tên
                            user = (IOrderedQueryable<Account>)user.Where(s => s.Name.Contains(search));
                        else if (show.Equals("5")) //theo số điện thoại
                            user = (IOrderedQueryable<Account>)user.Where(s => s.Phone.ToString().Contains(search));
                        else if (show.Equals("6")) //theo trạng thái
                            user = (IOrderedQueryable<Account>)user.Where(s => s.status.Contains(search));
                        return View("RolesTrash", user.ToPagedList(pageNumber, 50));
                    }
                    return View(user.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //gợi ý tìm kiếm
        [HttpPost]
        public JsonResult GeUserSearch(string Prefix)
        {
            var search = (from c in db.Accounts
                          where c.status != "2" && c.Email.StartsWith(Prefix)
                          orderby c.Email ascending
                          select new { c.Email });
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //Thông tin chi tiết tài khoản
        public ActionResult RolesDetails(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Read() == true)
                {
                    var user = (from a in db.Accounts
                                where a.account_id == id
                                orderby a.create_at descending // giảm dần
                                select a).FirstOrDefault();
                    if (user != null && id != null) return View(user);
                    Notification.set_flash("Không tồn tại tài khoản: " + user.Email + "", "warning");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("RolesIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Vô hiệu hóa tài khoản
        public ActionResult DelTrash(int? id, string returnUrl) // chuyển status về 0
        {
            if (User.Identity.GetRole()!=4)
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("RolesIndex");
            }
            else
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("DelTrash", new { returnUrl = Request.UrlReferrer.ToString() });
                }

                var account = db.Accounts.SingleOrDefault(a => a.account_id == id);
                if (account == null || id == null)
                {
                    Notification.set_flash("Không tồn tại tài khoản: " + account.Email + "", "warning");
                    return RedirectToAction("RolesIndex");
                }

                account.status = "2";
                account.update_at = DateTime.Now;
                account.update_by = User.Identity.GetEmail();
                db.Configuration.ValidateOnSaveEnabled = false;
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                Notification.set_flash("Đã vô hiệu hóa tài khoản: " + account.Email + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Dashboard");
            }
        }

        //Khôi phục tài khoản
        public ActionResult Undo(int? id, string returnUrl) // khôi phục từ thùng rác
        {
            if (User.Identity.GetRole() != 4)
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("RolesTrash");
            }
            else
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null
                                                    && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("Undo", new { returnUrl = Request.UrlReferrer.ToString() });
                }

                var account = db.Accounts.SingleOrDefault(a => a.account_id == id);
                if (account == null || id == null)
                {
                    Notification.set_flash("Không tồn tại tài khoản: " + account.Email + "", "warning");
                    return RedirectToAction("RolesTrash");
                }

                account.status = "1";
                account.update_at = DateTime.Now;
                account.update_by = User.Identity.GetEmail();
                db.Configuration.ValidateOnSaveEnabled = false;
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                Notification.set_flash("Khôi phục thành công tài khoản: " + account.Email + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("RolesTrash");
            }
        }

        public ActionResult OrderHistory(int id, int? size, int? page, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                var pageSize = size ?? 10;
                var pageNumber = page ?? 1;
                ViewBag.CurrentSort = sortOrder;
                ViewBag.ResetSort = "";
                ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_asc" : "date_desc";
                ViewBag.WaitingSortParm = "order_waiting";
                ViewBag.ProcessingortParm = "order_processing";
                ViewBag.CompleteSortParm = "order_complete";
                ViewBag.CancleSortParm = sortOrder == "order_cancle" ? "order_cancle" : "order_cancle";
                ViewBag.SumTotalOrder = db.Orders.Where(m => m.account_id == id).Sum(m => (int?)m.total) ?? 0;
                ViewBag.CountOrder = db.Orders.Where(m => m.account_id == id).Count();
                var list = from a in db.Orders
                           orderby a.order_id descending // giảm dần
                           where a.account_id == id && a.Account.status == "1"
                           select new OrderHistoryDTOs
                           {
                               order_status = a.status,
                               order_id = a.order_id,
                               total_price = a.total,
                               create_at = a.create_at,
                               update_at = a.update_at,
                               payment_id = a.payment_id,
                               payment_transaction = a.payment_transaction
                           };
                switch (sortOrder)
                {
                    case "date_desc":
                        list = from a in db.Orders
                               orderby a.order_id descending // giảm dần
                               where a.account_id == id && a.Account.status == "1"
                               select new OrderHistoryDTOs
                               {
                                   order_status = a.status,
                                   order_id = a.order_id,
                                   total_price = a.total,
                                   create_at = a.create_at,
                                   update_at = a.update_at,
                                   payment_id = a.payment_id,
                                   payment_transaction = a.payment_transaction
                               };
                        break;
                    case "date_asc":
                        list = from a in db.Orders
                               orderby a.order_id ascending // giảm dần
                               where a.account_id == id && a.Account.status == "1"
                               select new OrderHistoryDTOs
                               {
                                   order_status = a.status,
                                   order_id = a.order_id,
                                   total_price = a.total,
                                   create_at = a.create_at,
                                   update_at = a.update_at,
                                   payment_id = a.payment_id,
                                   payment_transaction = a.payment_transaction
                               };
                        break;
                    case "order_waiting":
                        ViewBag.SumTotalOrder =
                            db.Orders.Where(m => m.account_id == id && m.status == "1").Sum(m => (int?)m.total) ?? 0;
                        ViewBag.CountOrder = db.Orders.Where(m => m.account_id == id && m.status == "1").Count();
                        list = from a in db.Orders
                               orderby a.order_id descending // giảm dần
                               where a.account_id == id && a.Account.status == "1" && a.status == "1"
                               select new OrderHistoryDTOs
                               {
                                   order_status = a.status,
                                   order_id = a.order_id,
                                   total_price = a.total,
                                   create_at = a.create_at,
                                   update_at = a.update_at,
                                   payment_id = a.payment_id,
                                   payment_transaction = a.payment_transaction
                               };
                        break;
                    case "order_processing":
                        ViewBag.SumTotalOrder =
                            db.Orders.Where(m => m.account_id == id && m.status == "2").Sum(m => (int?)m.total) ?? 0;
                        ViewBag.CountOrder = db.Orders.Where(m => m.account_id == id && m.status == "2").Count();
                        list = from a in db.Orders
                               orderby a.order_id descending // giảm dần
                               where a.account_id == id && a.Account.status == "1" && a.status == "2"
                               select new OrderHistoryDTOs
                               {
                                   order_status = a.status,
                                   order_id = a.order_id,
                                   total_price = a.total,
                                   create_at = a.create_at,
                                   update_at = a.update_at,
                                   payment_id = a.payment_id,
                                   payment_transaction = a.payment_transaction
                               };
                        break;
                    case "order_complete":
                        ViewBag.SumTotalOrder =
                            db.Orders.Where(m => m.account_id == id && m.status == "3").Sum(m => (int?)m.total) ?? 0;
                        ViewBag.CountOrder = db.Orders.Where(m => m.account_id == id && m.status == "3").Count();
                        list = from a in db.Orders
                               orderby a.order_id descending // giảm dần
                               where a.account_id == id && a.Account.status == "1" && a.status == "3"
                               select new OrderHistoryDTOs
                               {
                                   order_status = a.status,
                                   order_id = a.order_id,
                                   total_price = a.total,
                                   create_at = a.create_at,
                                   update_at = a.update_at,
                                   payment_id = a.payment_id,
                                   payment_transaction = a.payment_transaction
                               };
                        break;
                    case "order_cancle":
                        ViewBag.SumTotalOrder =
                            db.Orders.Where(m => m.account_id == id && m.status == "0").Sum(m => (int?)m.total) ?? 0;
                        ViewBag.CountOrder = db.Orders.Where(m => m.account_id == id && m.status == "0").Count();
                        list = from a in db.Orders
                               orderby a.order_id descending // giảm dần
                               where a.account_id == id && a.Account.status == "1" && a.status == "0"
                               select new OrderHistoryDTOs
                               {
                                   order_status = a.status,
                                   order_id = a.order_id,
                                   total_price = a.total,
                                   create_at = a.create_at,
                                   update_at = a.update_at,
                                   payment_id = a.payment_id,
                                   payment_transaction = a.payment_transaction
                               };
                        break;
                    default: // Name ascending
                        list = from a in db.Orders
                               orderby a.order_id descending // giảm dần
                               where a.account_id == id && a.Account.status == "1"
                               select new OrderHistoryDTOs
                               {
                                   order_status = a.status,
                                   order_id = a.order_id,
                                   total_price = a.total,
                                   create_at = a.create_at,
                                   update_at = a.update_at,
                                   payment_id = a.payment_id,
                                   payment_transaction = a.payment_transaction
                               };
                        break;
                }
                return View(list.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("RolesIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //View chỉnh sửa thông tin tài khoản
        public ActionResult RolesEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null &&
                Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("RolesEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }

                var account = db.Accounts.Where(x => x.account_id == id).SingleOrDefault();
                if (User.Identity.GetRole() != 4)
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("RolesIndex");
                }
                else
                {
                    //check xem có tồn tại id người dùng không
                    if (account == null || id == null)
                    {
                        Notification.set_flash("Không tồn tại: " + account.Email + "", "warning");
                        return RedirectToAction("RolesIndex");
                    }
                    else
                    {
                        return View(account);
                    }
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Code xử lý chỉnh sửa thông tin tài khoản
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RolesEdit(Account model, string returnUrl, int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var account = db.Accounts.SingleOrDefault(m => m.account_id == id);
                try
                {
                    account.Avatar = model.Avatar;
                    account.Name = model.Name;
                    account.Email = model.Email;
                    account.Phone = model.Phone;
                    account.Dateofbirth = model.Dateofbirth;
                    account.Gender = model.Gender;
                    account.status = model.status;
                    account.update_by = User.Identity.GetEmail();
                    account.update_at = DateTime.Now;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    Notification.set_flash("Cập nhật thành công: " + account.Email + "", "success");
                    //quay về trang trước đó khi ấn nút lưu nếu thông tin cập nhật thành công
                    if (!String.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Dashboard");
                }
                catch
                {
                    Notification.set_flash("Lỗi", "danger");
                }

                return View(model);
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Xóa sản phẩm
        [HttpPost]
        public JsonResult RolesDelete(int id)
        {
            string result;
            if (User.Identity.GetRole() == 4)
            {
                var account = db.Accounts.FirstOrDefault(m => m.account_id == id);
                var newsProducts = db.NewsProducts.FirstOrDefault(a => a.product_id == id);
                var product_image = db.Product_Images.FirstOrDefault(m => m.product_id == id);
                if (account.Orders.Count > 0)
                {
                    result = "ExitOrder";
                }
                else if (account.NewsComments.Count > 0)
                {
                    result = "ExistComment";
                }
                else if (account.News.Count > 0)
                {
                    result = "ExistPost";
                }              
                else if (account.Feedbacks.Count > 0)
                {
                    result = "ExistFeedback";
                }
                else
                {
                    db.Accounts.Remove(account);
                    db.SaveChanges();
                    result = "Success";
                    Notification.set_flash("Xóa thành công", "success");
                }
            }
            else
            {              
                result = "false";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //View list quyền
        public ActionResult RolesList(int ?page,int ? size)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.GetRole() == 4)
                {
                    var pageSize = size ?? 11;
                    var pageNumber = page ?? 1;
                    ViewBag.rolePermission = db.RolesPermissions.ToList();
                    ViewBag.roles = db.Roles.OrderBy(m => m.role_id).ToPagedList(pageNumber, pageSize);
                    var rolepermisscheck = from p in db.Permissions
                                           orderby p.permission_id ascending
                                           select new
                                           {
                                               p.permission_id,
                                               p.permission_name,
                                               Checked = ((from rp in db.RolesPermissions where (rp.permission_id == p.permission_id) select rp).Count() > 0)
                                           };
                    var MyrolepermissCheckBoxList = new List<RolePermissCheckbox>();
                    foreach (var item in rolepermisscheck)
                    {
                        MyrolepermissCheckBoxList.Add(new RolePermissCheckbox { permiss_id = item.permission_id, permiss_name = item.permission_name });
                    }
                    RolesDTO rolesDTO = new RolesDTO();
                    rolesDTO.RolePermissions = MyrolepermissCheckBoxList;
                    return View(rolesDTO);
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        public JsonResult CreateRole(RolesDTO rolesDTO,Roles roles)
        {
            bool result;
            if(User.Identity.GetRole() == 4)
            {
                roles.create_at = DateTime.Now;
                roles.update_at = DateTime.Now;
                roles.role_name = rolesDTO.role_name;
                db.Roles.Add(roles);
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                foreach (var item in rolesDTO.RolePermissions)
                {
                    if (item.Checked)
                    {
                        db.RolesPermissions.Add(new RolesPermissions() { role_id = roles.role_id, permission_id = item.permiss_id });
                        db.SaveChanges();
                    }
                }
                Notification.set_flash("Thêm thành công '" + roles.role_name + "'", "success");
                result = true;
            }
            else 
            {
                result = false;
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        public JsonResult RoleEdit(RolesDTO rolesDTO)
        {
            bool result;
            if (User.Identity.GetRole() == 4)
            {
                Roles roles = db.Roles.FirstOrDefault(m => m.role_id == rolesDTO.role_id);
                roles.update_at = DateTime.Now;
                roles.role_name = rolesDTO.role_name;
                foreach (var item in db.RolesPermissions)
                {
                    if (item.role_id == rolesDTO.role_id)
                    {
                        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }
                }
                foreach (var item in rolesDTO.RolePermissions)
                {
                    if (item.Checked)
                    {
                        db.RolesPermissions.Add(new RolesPermissions() { role_id = roles.role_id, permission_id = item.permiss_id });
                    }
                }
                db.SaveChanges();
                Notification.set_flash("Sửa thành công '" + roles.role_name + "'", "success");
                result = true;
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ChangeStatus(int id, int role_id)
        {
            bool result;
            if (User.Identity.GetRole() == 4)
            {
                Account account = db.Accounts.SingleOrDefault(m => m.account_id == id);
                account.update_at = DateTime.Now;
                account.role_id = role_id;
                account.update_by = User.Identity.GetEmail();
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}