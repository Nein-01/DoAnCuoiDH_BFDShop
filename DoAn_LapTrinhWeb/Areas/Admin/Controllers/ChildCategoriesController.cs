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
    public class ChildCategoriesController : BaseController
    {
        private DbContext db = new DbContext();

        //VIew danh sách danh mục con
        public ActionResult ChildCateIndex(string search, string show, int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
               User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 10);
                    var pageNumber = (page ?? 1);
                    ViewBag.countTrash = db.ChildCategory.Count(a => a.status == "2");
                    var list = from a in db.ChildCategory
                               where (a.status == "1" || a.status == "0")
                               orderby a.childcategory_id descending
                               select a;
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))//tìm kiếm tất cả
                            list = (IOrderedQueryable<ChildCategory>)list.Where(s => s.childcategory_id.ToString().Contains(search) || s.name.Contains(search));
                        else if (show.Equals("2"))//theo id
                            list = (IOrderedQueryable<ChildCategory>)list.Where(s => s.childcategory_id.ToString().Contains(search));
                        else if (show.Equals("3"))//theo tên thể loại
                            list = (IOrderedQueryable<ChildCategory>)list.Where(s => s.name.ToString().Contains(search));
                        return View("ChildCateIndex", list.ToPagedList(pageNumber, 50));
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
        //View danh sách thùng rác danh mục con
        public ActionResult ChildCateTrash(string search, string show, int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 10);
                    var pageNumber = (page ?? 1);
                    var list = from a in db.ChildCategory
                               where (a.status == "2")
                               orderby a.childcategory_id descending
                               select a;
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))//tìm kiếm tất cả
                            list = (IOrderedQueryable<ChildCategory>)list.Where(s => s.childcategory_id.ToString().Contains(search) || s.name.Contains(search));
                        else if (show.Equals("2"))//theo id
                            list = (IOrderedQueryable<ChildCategory>)list.Where(s => s.childcategory_id.ToString().Contains(search));
                        else if (show.Equals("3"))//theo tên thể loại
                            list = (IOrderedQueryable<ChildCategory>)list.Where(s => s.name.ToString().Contains(search));
                        return View("ChildCateTrash", list.ToPagedList(pageNumber, 50));
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
        //Xem chi tiết danh mục con
        public ActionResult ChildCateDetails(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Read() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    ChildCategory newsCategory = db.ChildCategory.Find(id);
                    if (newsCategory == null)
                    {
                        return HttpNotFound();
                    }
                    return View(newsCategory);
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
        //tạo mới danh mục con
        public ActionResult ChildCateCreate()
        {
            if (User.Identity.Permiss_Create() == true)
            {
                ViewBag.ListCategory = new SelectList(db.ParentCategory.Where(m => (m.status == "1")).OrderBy(m => m.name), "parentcategory_id", "name", 0);
                return View();
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("ChildCateIndex");
            }
        }
        //Code xử lý tạo mới danh mục con
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChildCateCreate(ChildCategory ChillCategory)
        {
            ViewBag.ListCategory = new SelectList(db.ParentCategory.Where(m => (m.status == "1")).OrderBy(m => m.name), "parentcategory_id", "name", 0);
            string slug = SlugGenerator.SlugGenerator.GenerateSlug(ChillCategory.slug);
            try
            {
                var checkslug = db.ChildCategory.Any(m => m.slug == slug);
                if (checkslug)
                {
                    ChillCategory.slug = slug + "-" + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + new Random().Next(1, 100);
                }
                else
                {
                    ChillCategory.slug = SlugGenerator.SlugGenerator.GenerateSlug(ChillCategory.slug);
                }
                ChillCategory.parentcategory_id = ChillCategory.parentcategory_id;
                ChillCategory.name = ChillCategory.name;
                ChillCategory.image = ChillCategory.image;
                ChillCategory.image2 = ChillCategory.image2;
                ChillCategory.create_at = DateTime.Now;
                ChillCategory.status = ChillCategory.status;
                ChillCategory.create_by = User.Identity.GetEmail();
                ChillCategory.update_at = DateTime.Now;
                ChillCategory.update_by = User.Identity.GetEmail();
                db.ChildCategory.Add(ChillCategory);
                db.SaveChanges();
                Notification.set_flash("Thêm thành công: " + ChillCategory.name + "", "success");                
                return RedirectToAction("ChildCateIndex");
            }
            catch
            {
                Notification.set_flash("Lỗi", "danger");
            }
            return View(ChillCategory);
        }
        //Chỉnh sửa danh mục con
        public ActionResult ChildCateEdit(int? id,string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("ChildCateEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var newscategory = db.ChildCategory.SingleOrDefault(a => a.childcategory_id == id);
                if ((User.Identity.Permiss_Modify() == true) && (newscategory.status == "1" || newscategory.status == "0"))
                {
                    if (newscategory == null || id == null)
                    {
                        Notification.set_flash("Không tồn tại: " + newscategory.name + "", "warning");
                        return RedirectToAction("ChildCateIndex");
                    }
                    ViewBag.ListCategory = new SelectList(db.ParentCategory.Where(m => (m.status == "1")).OrderBy(m => m.name), "parentcategory_id", "name", 0);
                    return View(newscategory);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("ChildCateIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //code xử lý chỉnh sửa danh mục con
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChildCateEdit(ChildCategory chillCategory,string returnUrl)
        {
            ViewBag.ListCategory = new SelectList(db.ParentCategory.Where(m => (m.status == "1")).OrderBy(m => m.name), "parentcategory_id", "name", 0);
            try
            {
                chillCategory.parentcategory_id = chillCategory.parentcategory_id;
                chillCategory.name = chillCategory.name;
                chillCategory.image = chillCategory.image;
                chillCategory.image2 = chillCategory.image2;
                chillCategory.description = chillCategory.description;
                chillCategory.update_at = DateTime.Now;
                chillCategory.update_by = User.Identity.GetEmail();
                chillCategory.status = chillCategory.status;
                db.Entry(chillCategory).State = EntityState.Modified;
                db.SaveChanges();
                Notification.set_flash("Cập nhật thành thông: " + chillCategory.name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("ChildCateIndex");
            }
            catch
            {
                Notification.set_flash("404!", "warning");
            }
            return View(chillCategory);
        }
        //vô hiệu hoá danh mục con
        public ActionResult DelTrash(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                var newscategory = db.ChildCategory.SingleOrDefault(a => a.childcategory_id == id);
                if (newscategory == null || id == null)
                {
                    Notification.set_flash("Không tồn tại: " + newscategory.name + "", "warning");
                    return RedirectToAction("ChildCateIndex");
                }
                newscategory.status = "2";
                newscategory.update_at = DateTime.Now;
                newscategory.update_by = User.Identity.GetEmail();
                db.Entry(newscategory).State = EntityState.Modified;
                db.SaveChanges();
                Notification.set_flash("Đã chuyển: " + newscategory.name + " vào thùng rác!", "success");
                return RedirectToAction("ChildCateIndex");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("ChildCateIndex");
            }
        }
        //Khôi phục danh mục con
        public ActionResult Undo(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                var newscategory = db.ChildCategory.SingleOrDefault(a => a.childcategory_id == id);
                if (newscategory == null || id == null)
                {
                    Notification.set_flash("Không tồn tạii: " + newscategory.name + "", "warning");
                }
                newscategory.status = "1";
                newscategory.update_at = DateTime.Now;
                newscategory.update_by = User.Identity.GetEmail();
                db.Entry(newscategory).State = EntityState.Modified;
                db.SaveChanges();
                Notification.set_flash("Khôi phục thành công: " + newscategory.name + "", "success");
                return RedirectToAction("ChildCateTrash");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("ChildCateTrash");
            }
        }
        //Xoá danh mục con
        public ActionResult ChildCateDelete(int? id,string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("ChildCateDelete", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                if (User.Identity.Permiss_Delete() == true)
                {
                    var chillCategory = db.ChildCategory.SingleOrDefault(a => a.childcategory_id == id);
                    if (chillCategory == null)
                    {
                        Notification.set_flash("Không tồn tại: " + chillCategory.name + "", "warning");
                        return RedirectToAction("ChildCateTrash");
                    }
                    return View(chillCategory);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("ChildCateTrash");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Xác nhận xoá
        [HttpPost, ActionName("ChildCateDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,string returnUrl)
        {
            var chillCategory = db.ChildCategory.SingleOrDefault(a => a.childcategory_id == id);
            db.ChildCategory.Remove(chillCategory);
            db.SaveChanges();
            Notification.set_flash("Đã xoá vĩnh viễn: " + chillCategory.name + "", "success");
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("ChildCateTrash");
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
