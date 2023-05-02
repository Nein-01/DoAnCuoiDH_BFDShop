using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn_LapTrinhWeb;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Models;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        private DbContext db = new DbContext();
        //View danh sách danh mục bài viết cha
        public ActionResult CategoriesIndex(string search, string show, int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 10);
                    var pageNumber = (page ?? 1);
                    ViewBag.countTrash = db.ParentCategory.Count(a => a.status == "2");
                    var list = from a in db.ParentCategory
                               where (a.status == "1" || a.status == "0")
                               orderby a.parentcategory_id descending
                               select a;
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))//tìm kiếm tất cả
                            list = (IOrderedQueryable<ParentCategory>)list.Where(s => s.parentcategory_id.ToString().Contains(search) || s.name.Contains(search));
                        else if (show.Equals("2"))//theo id
                            list = (IOrderedQueryable<ParentCategory>)list.Where(s => s.parentcategory_id.ToString().Contains(search));
                        else if (show.Equals("3"))//theo tên thể loại
                            list = (IOrderedQueryable<ParentCategory>)list.Where(s => s.name.ToString().Contains(search));
                        return View("CategoriesIndex", list.ToPagedList(pageNumber, 50));
                    }
                    return View(list.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //View thùng rác danh mục bài viết cha
        public ActionResult CategoriesTrash(string search, string show, int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 10);
                    var pageNumber = (page ?? 1);
                    var list = from a in db.ParentCategory
                               where (a.status == "2")
                               orderby a.parentcategory_id descending
                               select a;
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))//tìm kiếm tất cả
                            list = (IOrderedQueryable<ParentCategory>)list.Where(s => s.parentcategory_id.ToString().Contains(search) || s.name.Contains(search));
                        else if (show.Equals("2"))//theo id
                            list = (IOrderedQueryable<ParentCategory>)list.Where(s => s.parentcategory_id.ToString().Contains(search));
                        else if (show.Equals("3"))//theo tên thể loại
                            list = (IOrderedQueryable<ParentCategory>)list.Where(s => s.name.ToString().Contains(search));
                        return View("CategoriesTrash", list.ToPagedList(pageNumber, 50));
                    }
                    return View(list.ToPagedList(pageNumber, pageSize));
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
        //Xem chi tiết danh mục cha 
        public ActionResult CategoriesDetails(int ? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Read() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var category = db.ParentCategory.Where(m => m.parentcategory_id == id).FirstOrDefault();
                    return View(category);
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
        //Tạo mới danh mục cha
        public ActionResult CategoriesCreate()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true)
                {
                    return View();
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("CategoriesIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Code xử lý tạo mới danh mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoriesCreate(ParentCategory category)
        {
            string slug = SlugGenerator.SlugGenerator.GenerateSlug(category.name);
            try
            {
                var checkslug = db.ParentCategory.Any(m => m.slug == category.slug);
                if (checkslug)
                {
                    category.slug = slug + "-" + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + new Random().Next(1, 100);
                }
                else
                {
                    category.slug = SlugGenerator.SlugGenerator.GenerateSlug(category.name);
                }
                category.name = category.name;
                category.image = category.image;
                category.image2 = category.image2;
                category.status = category.status;
                category.create_at = DateTime.Now;
                category.create_by = User.Identity.GetEmail();
                category.update_at = DateTime.Now;
                category.update_by = User.Identity.GetEmail();
                category.category_description = category.category_description;
                db.ParentCategory.Add(category);
                db.SaveChanges();
                Notification.set_flash("Thêm thành công: " + category.name + "", "success");
                return RedirectToAction("CategoriesIndex");
            }
            catch
            {
                Notification.set_flash("Lỗi", "danger");
            }

            return View(category);
        }
        //Chỉnh sửa danh mục cha
        public ActionResult CategoriesEdit(int? id,string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("CategoriesEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var category = db.ParentCategory.SingleOrDefault(a => a.parentcategory_id == id);
                if ((User.Identity.Permiss_Modify() == true) && (category.status == "1" || category.status == "0"))
                {
                    if (category == null || id == null)
                    {
                        Notification.set_flash("Không tồn tại: " + category.name + "", "warning");
                        return RedirectToAction("CategoriesIndex");
                    }
                    return View(category);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("CategoriesIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Code xử lý chỉnh sửa danh mục cha
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoriesEdit(ParentCategory category,string returnUrl)
        {
            try
            {
                category.name = category.name;
                category.image = category.image;
                category.image2 = category.image2;
                category.category_description = category.category_description;
                category.update_at = DateTime.Now;
                category.update_by = User.Identity.GetEmail();
                category.status = category.status;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                Notification.set_flash("Cập nhật thành thông: " + category.name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("CategoriesIndex");
            }
            catch
            {
                Notification.set_flash("404!", "warning");
            }
            return View(category);
        }
        //Vô hiệu hoá danh mục cha
        public ActionResult DelTrash(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                var category = db.ParentCategory.SingleOrDefault(a => a.parentcategory_id == id);
                if (category == null || id == null)
                {
                    Notification.set_flash("Không tồn tại: " + category.name + "", "warning");
                    return RedirectToAction("CategoriesIndex");
                }
                category.status = "2";
                category.update_at = DateTime.Now;
                category.update_by = User.Identity.GetEmail();
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                Notification.set_flash("Đã chuyển: " + category.name + " vào thùng rác!", "success");
                return RedirectToAction("CategoriesIndex");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("CategoriesIndex");
            }
        }
        //Khôi phục danh mục cha
        public ActionResult Undo(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                var category = db.ParentCategory.SingleOrDefault(a => a.parentcategory_id == id);
                if (category == null || id == null)
                {
                    Notification.set_flash("Không tồn tạii: " + category.name + "", "warning");
                }
                category.status = "1";
                category.update_at = DateTime.Now;
                category.update_by = User.Identity.GetEmail();
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                Notification.set_flash("Khôi phục thành công: " + category.name + "", "success");
                return RedirectToAction("CategoriesTrash");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("CategoriesIndex");
            }
        }
        //Xoá danh mục cha
        public ActionResult CategoriesDelete(int? id,string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("CategoriesDelete", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                if (User.Identity.Permiss_Delete() == true)
                {
                    var category = db.ParentCategory.SingleOrDefault(a => a.parentcategory_id == id);
                    if (category == null)
                    {
                        Notification.set_flash("Không tồn tại: " + category.name + "", "warning");
                        return RedirectToAction("CategoriesTrash");
                    }

                    return View(category);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("CategoriesTrash");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Xác nhận xoá danh mục cha
        [HttpPost, ActionName("CategoriesDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,string returnUrl)
        {
            var category = db.ParentCategory.SingleOrDefault(a => a.parentcategory_id == id);
            db.ParentCategory.Remove(category);
            db.SaveChanges();
            Notification.set_flash("Đã xoá vĩnh viễn: " + category.name + "", "success");
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("CategoryTrash");
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
