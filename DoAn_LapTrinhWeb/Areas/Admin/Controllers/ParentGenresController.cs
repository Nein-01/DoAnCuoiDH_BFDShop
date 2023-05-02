using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Model;
using PagedList;
using System.Web.Helpers;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class ParentGenresController : BaseController
    {
        private readonly DbContext _db = new DbContext();
        //View list thể loại cha
        public ActionResult ParentGIndex(string search, string show, int? size, int? page, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 9);
                    var pageNumber = (page ?? 1);
                    ViewBag.countTrash = _db.ParentGenres.Count(a => a.status == "2");
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.ResetSort = "";
                    ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.NameSort = sortOrder == "name_asc" ? "name_desc" : "name_asc";
                    var list = from a in _db.ParentGenres
                               where (a.status == "1" || a.status == "0")
                               orderby a.id descending
                               select a;
                    //Sort
                    switch (sortOrder)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Mới nhất";
                            list = from a in _db.ParentGenres
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.id descending
                                   select a;
                            break;
                        case "date_asc":
                            ViewBag.sortname = "Cũ nhất";
                            list = from a in _db.ParentGenres
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.id
                                   select a;
                            break;
                        case "name_desc":
                            ViewBag.sortname = "Tên thể loại(Z-A)";
                            list = from a in _db.ParentGenres
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.name descending
                                   select a;
                            break;
                        case "name_asc":
                            ViewBag.sortname = "Tên thể loại(A-Z)";
                            list = from a in _db.ParentGenres
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.name
                                   select a;
                            break;
                    }
                    //filter & search
                    if (string.IsNullOrEmpty(search)) return View(list.ToPagedList(pageNumber, pageSize));
                    switch (show)
                    {
                        //tìm kiếm tất cả
                        case "1":
                            list = (IOrderedQueryable<Model.ParentGenres>)list.Where(s =>
                                s.id.ToString().Contains(search) || s.name.Contains(search));
                            break;
                        //theo id
                        case "2":
                            list = (IOrderedQueryable<Model.ParentGenres>)list.Where(s => s.id.ToString().Contains(search));
                            break;
                        //theo tên
                        case "3":
                            list = (IOrderedQueryable<Model.ParentGenres>)list.Where(
                                s => s.name.ToString().Contains(search));
                            break;
                    }
                    return View("ParentGIndex", list.ToPagedList(pageNumber, 50));
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
        //gợi ý search thể loại cha 
        [HttpPost]
        public JsonResult GetParentGenresSearch(string Prefix)
        {
            var search = (from c in _db.ParentGenres
                where c.name.Contains(Prefix)
                orderby c.name ascending
                select new { c.name, c.image });
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //View list trash thể loại cha
        public ActionResult ParentGTrash(string search, string show, int? size, int? page, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 9);
                    var pageNumber = (page ?? 1);
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.ResetSort = "";
                    ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.NameSort = sortOrder == "name_asc" ? "name_desc" : "name_asc";
                    var list = from a in _db.ParentGenres where a.status == "2" orderby a.update_at descending select a;
                    //Sort
                    switch (sortOrder)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Mới nhất";
                            list = from a in _db.ParentGenres where (a.status == "2") orderby a.id descending select a;
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Cũ nhất";
                            list = from a in _db.ParentGenres
                                   where (a.status == "2") orderby a.id select a;
                            break;
                        case "name_desc":
                            ViewBag.sortname = "Tên thể loại(Z-A)";
                            list = from a in _db.ParentGenres where (a.status == "2") orderby a.name descending
                                   select a;
                            break;
                        case "name_asc":
                            ViewBag.sortname = "Tên thể loại(A-Z)";
                            list = from a in _db.ParentGenres
                                   where (a.status == "2")
                                   orderby a.name
                                   select a;
                            break;
                    }
                    //filter & search
                    if (string.IsNullOrEmpty(search)) return View(list.ToPagedList(pageNumber, pageSize));
                    switch (show)
                    {
                        //tìm kiếm tất cả
                        case "1":
                            list = (IOrderedQueryable<Model.ParentGenres>)list.Where(s =>
                                s.id.ToString().Contains(search) || s.name.Contains(search)
                                                                 || s.create_by.Contains(search));
                            break;
                        //theo id
                        case "2":
                            list = (IOrderedQueryable<Model.ParentGenres>)list.Where(s => s.id.ToString().Contains(search));
                            break;
                        //theo tên thể loại
                        case "3":
                            list = (IOrderedQueryable<Model.ParentGenres>)list.Where(
                                s => s.name.ToString().Contains(search));
                            break;
                    }
                    return View("ParentGTrash", list.ToPagedList(pageNumber, 50));
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
        //Thông tin thể loại cha
        public ActionResult ParentGDetails(int? id)
        {
            if (User.Identity.Permiss_Read() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
            User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
            {
                var genre = _db.ParentGenres.SingleOrDefault(a => a.id == id);
                if (genre != null && id != null) return View(genre);
                Notification.set_flash("Không tồn tại: " + genre.name + "", "warning");
                return RedirectToAction("ParentGIndex");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("ParentGIndex");
            }
        }
        //View thêm thể loại cha
        public ActionResult ParentGCreate()
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
                    return RedirectToAction("ParentGIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Code xử lý thêm thể loại cha
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ParentGCreate(ParentGenres genre)
        {
            string slug = SlugGenerator.SlugGenerator.GenerateSlug(genre.name);
            try
            {
                var checkslug = _db.ParentGenres.Any(m => m.slug == slug);
                if (checkslug)
                {
                    genre.slug = slug + "-" + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") +
                                 new Random().Next(1, 1000);
                }
                else
                {
                    genre.slug = SlugGenerator.SlugGenerator.GenerateSlug(genre.name);
                }
                genre.status = genre.status;
                genre.create_at = DateTime.Now;
                genre.create_by = User.Identity.GetEmail();
                genre.update_at = DateTime.Now;
                genre.update_by = User.Identity.GetEmail();
                if (genre.image != null)
                {
                    genre.image = genre.image;
                }
                else
                {
                    genre.image = "/Images/ImagesCollection/no-image-available.png";
                }
                genre.description = genre.description;
                _db.ParentGenres.Add(genre);
                _db.SaveChanges();
                Notification.set_flash("Thêm thành công: " + genre.name + "", "success");
                return RedirectToAction("ParentGIndex");
            }
            catch
            {
                Notification.set_flash("Lỗi", "danger");
            }
            return View(genre);
        }
        //View chỉnh sửa thể loại cha
        public ActionResult ParentGEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null &&
                Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("ParentGEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var genre = _db.ParentGenres.SingleOrDefault(a => a.id == id);
                if ((User.Identity.Permiss_Modify() == true) && (genre.status == "1" || genre.status == "0"))
                {
                    if (genre == null || id == null)
                    {
                        Notification.set_flash("Không tồn tại: " + genre.name + "", "warning");
                        return RedirectToAction("ParentGIndex");
                    }
                    return View(genre);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("ParentGIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Code xử lý chỉnh sửa thể loại cha
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ParentGEdit(ParentGenres genre, string returnUrl)
        {
            try
            {
                genre.update_at = DateTime.Now;
                genre.update_by = User.Identity.GetEmail();
                genre.status = genre.status;
                _db.Entry(genre).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Cập nhật thành công: " + genre.name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("ParentGIndex");
            }
            catch
            {
                Notification.set_flash("404!", "warning");
            }
            return View(genre);
        }
        //Vô hiệu hóa thể loại cha
        public ActionResult DelTrash(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                var genre = _db.ParentGenres.SingleOrDefault(a => a.id == id);
                var list = from a in _db.ParentGenres
                           join b in _db.Genres on a.id equals b.parent_genre_id
                           where b.parent_genre_id == id
                           select b;
                if (genre == null || id == null)
                {
                    Notification.set_flash("Không tồn tại: " + genre.name + "", "warning");
                    return RedirectToAction("ParentGIndex");
                }
                genre.status = "2";
                genre.update_at = DateTime.Now;
                genre.update_by = User.Identity.GetEmail();
                _db.Entry(genre).State = EntityState.Modified;
                foreach (var listchildgenre in list)
                {
                    listchildgenre.status = "2";
                }
                _db.SaveChanges();
                Notification.set_flash("Đã chuyển: " + genre.name + " vào thùng rác!", "success");
                return RedirectToAction("ParentGIndex");
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }
        //Khôi phục thể loại cha
        public ActionResult Undo(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                var genre = _db.ParentGenres.SingleOrDefault(a => a.id == id);
                var list = from a in _db.ParentGenres
                    join b in _db.Genres on a.id equals b.parent_genre_id
                    where b.parent_genre_id == id
                    select b;
                if (genre == null || id == null)
                {
                    Notification.set_flash("Không tồn tại thể loại: " + genre.name + "", "warning");
                    return RedirectToAction("ParentGIndex");
                }
                genre.status = "1";
                genre.update_at = DateTime.Now;
                genre.update_by = User.Identity.GetEmail();
                _db.Entry(genre).State = EntityState.Modified;
                foreach (var listchildgenre in list)
                {
                    listchildgenre.status = "0";
                }
                _db.SaveChanges();
                Notification.set_flash("Khôi phục thành công thể loại: " + genre.name + "", "success");
                return RedirectToAction("ParentGTrash");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("ParentGTrash");
            }
        }
        //Xóa thể loại cha
        public ActionResult ParentGDelete(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null &&
                Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("ParentGDelete", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                if (User.Identity.Permiss_Delete() == true)
                {
                    var genre = _db.ParentGenres.SingleOrDefault(a => a.id == id);
                    if (genre == null)
                    {
                        Notification.set_flash("Không tồn tại: " + genre.name + "", "warning");
                        return RedirectToAction("ParentGTrash");
                    }
                    return View(genre);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("ParentGTrash");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Xác nhận xóa thể loại cha
        [HttpPost]
        [ActionName("ParentGDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            var genre = _db.ParentGenres.SingleOrDefault(a => a.id == id);
            _db.ParentGenres.Remove(genre);
            _db.SaveChanges();
            Notification.set_flash("Đã xoá vĩnh viễn: " + genre.name + "", "success");
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("ParentGTrash");
        }
        //thay đổi trạng thái thể loại cha
        [HttpPost]
        public JsonResult ChangeStatus(int id, int state = 0)
        {
            bool result;
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                ParentGenres parent = _db.ParentGenres.Where(m => m.id == id).FirstOrDefault();
                string title = parent.name;
                parent.status = state.ToString();
                string prefix = state.ToString() == "1" ? "Hiển thị" : "Không hiển thị";
                parent.update_at = DateTime.Now;
                parent.update_by = User.Identity.GetEmail();
                _db.SaveChanges();
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
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}