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
    public class GenresController : BaseController
    {
        private readonly DbContext _db = new DbContext();
        //View list thể loại
        public ActionResult GenreIndex(string search, string show, int? size, int? page, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 9);
                    var pageNumber = (page ?? 1);
                    var list = from a in _db.Genres
                               where (a.status == "1" || a.status == "0")
                               orderby a.genre_id descending
                               select a;

                    ViewBag.countTrash = _db.Genres.Count(a => a.status == "2");
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.ResetSort = "";
                    ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_asc" : "date_desc";
                    ViewBag.NameSort = sortOrder == "name_desc" ? "name_asc" : "name_desc";
                    //Sort
                    switch (sortOrder)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Mới nhất";
                            list = from a in _db.Genres
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.genre_id descending
                                   select a;
                            break;
                        case "date_asc":
                            ViewBag.sortname = "Cũ nhất";
                            list = from a in _db.Genres
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.genre_id
                                   select a;
                            break;
                        case "name_desc":
                            ViewBag.sortname = "Tên thể loại(Z-A)";
                            list = from a in _db.Genres
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.genre_id descending
                                   select a;
                            break;

                        case "name_asc":
                            ViewBag.sortname = "Tên thể loại(A-Z)";
                            list = from a in _db.Genres
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.genre_id
                                   select a;
                            break;
                    }
                    if (string.IsNullOrEmpty(search)) return View(list.ToPagedList(pageNumber, pageSize));
                    switch (show)
                    {
                        //tìm kiếm tất cả
                        case "1":
                            list = (IOrderedQueryable<Genre>)list.Where(s => s.genre_id.ToString().Contains(search)
                                                                             || s.genre_name.Contains(search)
                                                                             || s.create_by.Contains(search)
                                                                             || s.ParentGenres.name.ToString()
                                                                                 .Contains(search));
                            break;
                        //theo id
                        case "2":
                            list = (IOrderedQueryable<Genre>)list.Where(s => s.genre_id.ToString().Contains(search));
                            break;
                        //theo tên thể loại
                        case "3":
                            list = (IOrderedQueryable<Genre>)list.Where(s => s.genre_name.ToString().Contains(search)
                                                                             || s.ParentGenres.name.ToString()
                                                                                 .Contains(search));
                            break;
                    }

                    return View("GenreIndex", list.ToPagedList(pageNumber, 50));
                }
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //View list trash thể loại con
        public ActionResult GenreTrash(string search, string show, int? size, int? page, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 9);
                    var pageNumber = (page ?? 1);
                    var list = from a in _db.Genres
                               where a.status == "2"
                               orderby a.update_at descending
                               select a;
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.ResetSort = "";
                    ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.NameSort = sortOrder == "name_asc" ? "name_desc" : "name_asc";

                    //Sort
                    switch (sortOrder)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Mới nhất";
                            list = from a in _db.Genres
                                   where a.status == "2"
                                   orderby a.genre_id descending
                                   select a;
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Cũ nhất";
                            list = from a in _db.Genres
                                   where a.status == "2"
                                   orderby a.genre_id
                                   select a;
                            break;

                        case "name_desc":
                            ViewBag.sortname = "Tên thể loại(Z-A)";
                            list = from a in _db.Genres
                                   where a.status == "2"
                                   orderby a.genre_id descending
                                   select a;
                            break;

                        case "name_asc":
                            ViewBag.sortname = "Tên thể loại(A-Z)";
                            list = from a in _db.Genres
                                   where a.status == "2"
                                   orderby a.genre_id
                                   select a;
                            break;
                    }

                    if (string.IsNullOrEmpty(search)) return View(list.ToPagedList(pageNumber, pageSize));
                    switch (show)
                    {
                        //tìm kiếm tất cả
                        case "1":
                            list = (IOrderedQueryable<Genre>)list.Where(s => s.genre_id.ToString().Contains(search) ||
                                                                             s.genre_name.Contains(search)
                                                                             || s.create_by.Contains(search));
                            break;
                        //theo id
                        case "2":
                            list = (IOrderedQueryable<Genre>)list.Where(s => s.genre_id.ToString().Contains(search));
                            break;
                        //theo tên thể loại
                        case "3":
                            list = (IOrderedQueryable<Genre>)list.Where(s => s.genre_name.ToString().Contains(search));
                            break;
                    }

                    return View("GenreTrash", list.ToPagedList(pageNumber, 50));
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("GenreTrash");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Thông tin thể loại con
        public ActionResult GenreDetails(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Read() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var genre = _db.Genres.SingleOrDefault(a => a.genre_id == id);
                    if (genre == null || id == null)
                    {
                        Notification.set_flash("Không tồn tại thể loại: " + genre.genre_name + "", "warning");
                        return RedirectToAction("GenreIndex");
                    }
                    return View(genre);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("GenreIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //View thêm thể loại con
        public ActionResult GenreCreate()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true)
                {
                    ViewBag.ListParentGenre =
                        new SelectList(_db.ParentGenres.Where(m => m.status == "1").OrderBy(m => m.name), "id", "name", 0);
                    return View();
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("GenreIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Code xử lý thêm thể loại con
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult GenreCreate(Genre genre)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.ListParentGenre = new SelectList(_db.ParentGenres.Where(m => m.status == "1").OrderBy(m => m.name), "id", "name", 0);
                string slug = SlugGenerator.SlugGenerator.GenerateSlug(genre.genre_name);
                try
                {
                    var checkslug = _db.Genres.Any(m => m.slug == slug);
                    if (checkslug)
                    {
                        genre.slug = slug + "-" + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + new Random().Next(1, 1000);
                    }
                    else
                    {
                        genre.slug = SlugGenerator.SlugGenerator.GenerateSlug(genre.genre_name);
                    }
                    genre.parent_genre_id = genre.parent_genre_id;
                    genre.status = genre.status;
                    genre.create_at = DateTime.Now;
                    genre.create_by = User.Identity.GetEmail();
                    genre.update_at = DateTime.Now;
                    genre.update_by = User.Identity.GetEmail();
                    if (genre.genre_image != null)
                    {
                        genre.genre_image = genre.genre_image;
                    }
                    else
                    {
                        genre.genre_image = "/Images/ImagesCollection/no-image-available.png";
                    }
                    genre.description = genre.description;
                    _db.Genres.Add(genre);
                    _db.SaveChanges();
                    Notification.set_flash("Thêm mới thể loại thành công thể loại: " + genre.genre_name + "", "success");
                    return RedirectToAction("GenreIndex");
                }
                catch
                {
                    Notification.set_flash("Lỗi", "danger");
                }
                return View(genre);
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //View chỉnh sửa thể loại con
        public ActionResult GenreEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null &&
                Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("GenreEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var genre = _db.Genres.SingleOrDefault(a => a.genre_id == id);
                if ((User.Identity.Permiss_Modify() == true) &&
                    (genre.status == "1" || genre.status == "0"))
                {
                    if (genre == null || id == null)
                    {
                        Notification.set_flash("Không tồn tại thể loại: " + genre.genre_name + "", "warning");
                        return RedirectToAction("GenreIndex");
                    }
                    ViewBag.ListParentGenre =
                        new SelectList(_db.ParentGenres.Where(m => m.status == "1").OrderBy(m => m.name), "id", "name", 0);
                    return View(genre);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("GenreIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Code xử lý chỉnh sửa thể loại con
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult GenreEdit(Genre genre, string returnUrl)
        {
            ViewBag.ListParentGenre = new SelectList(_db.ParentGenres.Where(m => m.status == "1").OrderBy(m => m.name),"id", "name", 0);
            try
            {
                genre.update_at = DateTime.Now;
                genre.parent_genre_id = genre.parent_genre_id;
                genre.update_by = User.Identity.GetEmail();
                genre.status = genre.status;
                _db.Entry(genre).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Đã cập nhật lại thông tin thể loại: " + genre.genre_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("GenreIndex");
            }
            catch
            {
                Notification.set_flash("404!", "warning");
            }

            return View(genre);
        }
        //Vô hiệu hóa thể loại con
        public ActionResult DelTrash(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                var genre = _db.Genres.SingleOrDefault(a => a.genre_id == id);
                if (genre == null || id == null)
                {
                    Notification.set_flash("Không tồn tại thể loại: " + genre.genre_name + "", "warning");
                    return RedirectToAction("GenreIndex");
                }
                genre.status = "2";
                genre.update_at = DateTime.Now;
                genre.update_by = User.Identity.GetEmail();
                _db.Entry(genre).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Đã chuyển thể loại: " + genre.genre_name + " vào thùng rác!", "success");
                return RedirectToAction("GenreIndex");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("GenreIndex");
            }
        }
        //Khôi phục thể loại con
        public ActionResult Undo(int? id)
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                var genre = _db.Genres.SingleOrDefault(a => a.genre_id == id);
                //select genre_id,parent_genre_id from ParentGenres,Genre where ParentGenres.id = Genre.parent_genre_id and genre_id = 1
                var genre_id_parent = (from a in _db.Genres
                                       join b in _db.ParentGenres on a.parent_genre_id equals b.id
                                       where a.genre_id == id
                                       select b.status).SingleOrDefault();
                if (genre == null || id == null)
                {
                    Notification.set_flash("Không tồn tại thể loại: " + genre.genre_name + "", "warning");
                    return RedirectToAction("GenreIndex");
                }
                if (genre_id_parent == "2")
                {
                    Notification.set_flash("Thể loại cha: đang nằm trong thùng rác", "warning");
                }
                else
                {
                    genre.status = "1";
                    genre.update_at = DateTime.Now;
                    genre.update_by = User.Identity.GetEmail();
                    _db.Entry(genre).State = EntityState.Modified;
                    _db.SaveChanges();
                    Notification.set_flash("Khôi phục thành công thể loại: " + genre.genre_name + "", "success");
                }
                return RedirectToAction("GenreTrash");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("GenreTrash");
            }
        }
        //Xóa thể loại con
        public ActionResult GenreDelete(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null &&
                Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("GenreDelete", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                if (User.Identity.Permiss_Delete() == true)
                {
                    var genre = _db.Genres.SingleOrDefault(a => a.genre_id == id);
                    if (genre == null)
                    {
                        Notification.set_flash("Không tồn tại thể loại: " + genre.genre_name + "", "warning");
                        return RedirectToAction("GenreTrash");
                    }
                    return View(genre);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("GenreTrash");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Xác nhận xóa thể loại con
        [HttpPost]
        [ActionName("GenreDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            var genre = _db.Genres.SingleOrDefault(a => a.genre_id == id);
            _db.Genres.Remove(genre);
            _db.SaveChanges();
            Notification.set_flash("Đã xoá vĩnh viễn thể loại: " + genre.genre_name + "", "success");
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("GenreTrash");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}