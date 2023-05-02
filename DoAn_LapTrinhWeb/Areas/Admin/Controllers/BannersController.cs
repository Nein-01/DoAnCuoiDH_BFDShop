using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Model;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class BannersController : BaseController
    {
        private readonly DbContext db = new DbContext();

        //View List Banner
        public ActionResult BannerIndex(int? size, int? page, string search, string show, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true)
                {
                    var pageSize = (size ?? 9);
                    var pageNumber = (page ?? 1);
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.ResetSort = "";
                    ViewBag.BannerNameSortParm = sortOrder == "bannername_asc" ? "bannername_desc" : "bannername_asc";
                    ViewBag.BannerIdSortParm = sortOrder == "bannerid_asc" ? "bannerid_desc" : "bannerid_asc";
                    ViewBag.countTrash = db.Banners.Count(a => a.status == "2");

                    var list = from a in db.Banners
                               where (a.status == "1" || a.status == "0")
                               orderby a.banner_id descending
                               select a;

                    switch (sortOrder)
                    {
                        case "bannername_desc":
                            ViewBag.sortname = "Xếp theo: Tên Khuyễn Mãi (Z - A)";
                            list = from a in db.Banners
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.banner_name descending
                                   select a;
                            break;

                        case "bannername_asc":
                            ViewBag.sortname = "Xếp theo: Tên Khuyễn Mãi (A - Z)";
                            list = from a in db.Banners
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.banner_name
                                   select a;
                            break;

                        case "bannerid_desc":
                            ViewBag.sortname = "Xếp theo: Mã Khuyễn Mãi (9 - 0)";
                            list = from a in db.Banners
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.banner_id descending
                                   select a;
                            break;

                        case "bannerid_asc":
                            ViewBag.sortname = "Xếp theo: Mã Khuyễn Mãi (0 - 9)";
                            list = from a in db.Banners
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.banner_id
                                   select a;
                            break;
                    }

                    if (!string.IsNullOrEmpty(search))
                    {
                        switch (show)
                        {
                            //tìm kiếm tất cả
                            case "1":
                                ViewBag.fillter = "Tất cả";
                                list = (IOrderedQueryable<Banner>)list.Where(s => s.banner_id.ToString().Contains(search) ||
                                                                                  s.banner_name.Contains(search)
                                                                                  || s.create_by.Contains(search));
                                break;
                            //theo id
                            case "2":
                                ViewBag.fillter = "Theo ID";
                                list = (IOrderedQueryable<Banner>)list.Where(s => s.banner_id.ToString().Contains(search));
                                break;
                            //theo tên thể loại
                            case "3":
                                ViewBag.fillter = "Theo tên chương trình";
                                list = (IOrderedQueryable<Banner>)list.Where(s => s.banner_name.Contains(search));
                                break;
                        }

                        return View("BannerIndex", list.ToPagedList(pageNumber, 50));
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

        public JsonResult GetBannerSearch(string Prefix)
        {
            var search = (from c in db.Banners
                where c.banner_name.Contains(Prefix) || c.banner_id.ToString().Contains(Prefix)
                orderby c.banner_name, c.banner_id
                select new { c.banner_id, c.banner_name, c.image_thumbnail });
            return Json(search, JsonRequestBehavior.AllowGet);
        }

        //View list Banner Trash
        public ActionResult BannerTrash(int? size, int? page, string search, string show, string sortOrder)
        {
            if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true ||
            User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
            {
                var pageSize = (size ?? 9);
                var pageNumber = (page ?? 1);
                ViewBag.CurrentSort = sortOrder;
                ViewBag.ResetSort = "";
                ViewBag.BannerNameSortParm = sortOrder == "bannername_asc" ? "bannername_desc" : "bannername_asc";
                ViewBag.BannerIdSortParm = sortOrder == "bannerid_asc" ? "bannerid_desc" : "bannerid_asc";

                var list = from a in db.Banners
                    where (a.status == "2")
                    orderby a.update_at descending
                    select a;

                switch (sortOrder)
                {
                    case "bannername_desc":
                        ViewBag.sortname = "Xếp theo: Tên Khuyễn Mãi (Z - A)";
                        list = from a in db.Banners
                            where (a.status == "2")
                            orderby a.banner_name descending
                            select a;
                        break;

                    case "bannername_asc":
                        ViewBag.sortname = "Xếp theo: Tên Khuyễn Mãi (A - Z)";
                        list = from a in db.Banners
                            where (a.status == "2")
                            orderby a.banner_name
                            select a;
                        break;

                    case "bannerid_desc":
                        ViewBag.sortname = "Xếp theo: Mã Khuyễn Mãi (9 - 0)";
                        list = from a in db.Banners
                            where (a.status == "2")
                            orderby a.banner_id descending
                            select a;
                        break;

                    case "bannerid_asc":
                        ViewBag.sortname = "Xếp theo: Mã Khuyễn Mãi (0 - 9)";
                        list = from a in db.Banners
                            where (a.status == "2")
                            orderby a.banner_id
                            select a;
                        break;
                }

                if (!string.IsNullOrEmpty(search))
                {
                    switch (show)
                    {
                        //tìm kiếm tất cả
                        case "1":
                            list = (IOrderedQueryable<Banner>)list.Where(s => s.banner_id.ToString().Contains(search) ||
                                                                              s.banner_name.Contains(search)
                                                                              || s.create_by.Contains(search));
                            break;
                        //theo id
                        case "2":
                            list = (IOrderedQueryable<Banner>)list.Where(s => s.banner_id.ToString().Contains(search));
                            break;
                        //theo tên thể loại
                        case "3":
                            list = (IOrderedQueryable<Banner>)list.Where(s => s.banner_name.Contains(search));
                            break;
                    }

                    return View("BannerTrash", list.ToPagedList(pageNumber, 50));
                }

                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //nếu không phải là admin hoặc biên tập viên thì sẽ back về trang chủ bảng điều khiển
                return RedirectToAction("Index", "Dashboard");
            }
        }

        //Thông tin chi tiết Banner
        public ActionResult BannerDetails(int? id)
        {
            if (User.Identity.IsAuthenticated )
            {
                if(User.Identity.Permiss_Read() == true || User.Identity.Permiss_Create() == true ||User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
                    if (banner == null && User.Identity.Permiss_Read() == true)
                    {
                        Notification.set_flash("Không tồn tại: " + banner.banner_name, "warning");
                        return RedirectToAction("BannerIndex");
                    }
                    else
                    {
                        return View(banner);
                    }
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
        //View Thêm khuyến mãi
        public ActionResult BannerCreate()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true)
                {
                    return View();
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
        //Code xử lý thêm khuyến mãi
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult BannerCreate(Banner banner)
        {
            string slug = SlugGenerator.SlugGenerator.GenerateSlug(banner.banner_name);
            try
            {
                var checkslug = db.Banners.Any(m => m.slug == slug);
                if (checkslug)
                {
                    banner.slug = slug + "-" + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") +
                                  new Random().Next(1, 1000);
                }
                else
                {
                    banner.slug = SlugGenerator.SlugGenerator.GenerateSlug(banner.banner_name);
                }

                banner.banner_name = banner.banner_name;
                banner.image_thumbnail = banner.image_thumbnail;
                banner.banner_type = banner.banner_type;
                banner.description = banner.description;
                banner.banner_start = banner.banner_start;
                banner.banner_end = banner.banner_end;
                banner.create_by = User.Identity.GetEmail();
                banner.update_by = User.Identity.GetEmail();
                banner.update_at = DateTime.Now;
                banner.create_at = DateTime.Now;
                db.Banners.Add(banner);
                db.SaveChanges();

                Notification.set_flash("Thêm mới thành công!", "success");
                return RedirectToAction("BannerIndex");
            }
            catch
            {
                Notification.set_flash("Lỗi", "danger");
            }

            return View(banner);
        }
        //View Chỉnh sửa khuyến mãi
        public ActionResult BannerEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
                if ((User.Identity.Permiss_Modify() == true) && (banner.status == "1" || banner.status == "0"))
                {
                    if (banner == null || id == null)
                    {
                        Notification.set_flash("Không tồn tại: " + banner.banner_name + "", "warning");
                        return RedirectToAction("BannerIndex");
                    }
                    return View(banner);
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
        //Code xử lý Chỉnh sửa khuyến mãi
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult BannerEdit(Banner banner)
        {
            try
            {
                banner.description = banner.description;
                banner.update_at = DateTime.Now;
                banner.update_by = User.Identity.GetEmail();
                db.Entry(banner).State = EntityState.Modified;
                db.SaveChanges();
                Notification.set_flash("Cập nhật thành công: " + banner.banner_name + "", "success");
                return RedirectToAction("BannerIndex");
            }
            catch
            {
                Notification.set_flash("cập nhật thất bại!", "danger");
            }

            return View(banner);
        }

        //Vô hiệu xóa khuyến mãi
        public ActionResult DelTrash(int? id) //bỏ sp vào thùng rác
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
                if (banner == null || id == null)
                {
                    Notification.set_flash("Không tồn tại: " + banner.banner_name, "warning");
                    return RedirectToAction("BannerIndex");
                }

                banner.status = "2";
                banner.update_at = DateTime.Now;
                banner.update_by = User.Identity.GetEmail();
                db.Configuration.ValidateOnSaveEnabled = false;
                db.Entry(banner).State = EntityState.Modified;
                db.SaveChanges();
                Notification.set_flash("Đã chuyển: " + banner.banner_name + " vào thùng rác", "success");
                return RedirectToAction("BannerIndex");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("BannerIndex");
            }
        }
        //Khôi phục khuyến mãi từ thùng rác
        public ActionResult Undo(int? id) // khôi phục từ thùng rác
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
                if (banner == null || id == null)
                {
                    Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                    return RedirectToAction("BannerIndex");
                }
                banner.status = "1";
                banner.update_at = DateTime.Now;
                banner.update_by = User.Identity.GetEmail();
                db.Entry(banner).State = EntityState.Modified;
                db.SaveChanges();
                Notification.set_flash("Khôi phục thành công: " + banner.banner_name, "success");
                return RedirectToAction("BannerTrash");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("BannerTrash");
            }
        }
        //Xóa khuyến mãi
        public ActionResult BannerDelete(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                //tạo url back về trang trước
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null &&Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("BannerDelete", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                //=> 
                if (User.Identity.Permiss_Delete() == true)//delete
                {
                    var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
                    if (banner == null)
                    {
                        Notification.set_flash("Không tồn tại: " + banner.banner_name + "", "warning");
                        return RedirectToAction("BannerTrash");
                    }
                    return View(banner);
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
        //Xác nhận xóa vĩnh viễn
        [HttpPost]
        [ActionName("BannerDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            var banner = db.Banners.SingleOrDefault(a => a.banner_id == id);
            db.Banners.Remove(banner);
            db.SaveChanges();
            Notification.set_flash("Xóa thành công: " + banner.banner_name + "", "success");
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("BannerTrash");
        }
        //đổi status banner
        [HttpPost]
        public JsonResult ChangeStatus(int id, int state = 0)
        {
            Banner banner = db.Banners.Where(m => m.banner_id == id).FirstOrDefault();
            int title = banner.banner_id;
            banner.status = state.ToString();
            string prefix = state.ToString() == "1" ? "Hiển thị" : "Không hiển thị";
            banner.update_at = DateTime.Now;
            banner.update_by = User.Identity.GetEmail();
            db.SaveChanges();
            return Json(new { Message = "Đã chuyển " + "ID" + title + " sang " + prefix },JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}