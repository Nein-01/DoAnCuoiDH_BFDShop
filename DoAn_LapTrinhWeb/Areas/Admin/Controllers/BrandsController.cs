using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Models;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class BrandsController : BaseController
    {
        private readonly DbContext _db = new DbContext();
        //View list thương hiệu
        public ActionResult BrandIndex(string search,string show,int? size, int? page, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = size ?? 10;
                    var pageNumber = (page ?? 1);
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.ResetSort = "";
                    ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.NameSort = sortOrder == "name_asc" ? "name_desc" : "name_asc";
                    ViewBag.countTrash = _db.Brands.Count(a => a.status == "0" || a.status == "2");
                    var list = from a in _db.Brands
                               where (a.status == "1")
                               orderby a.brand_id descending
                               select a;
                    switch (sortOrder)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Xếp theo: Mới nhất";
                            list = (from a in _db.Brands
                                    where (a.status == "1")
                                    orderby a.brand_id descending
                                    select a);
                            break;

                        case "date_asc":
                            ViewBag.sortname = "Xếp theo: Cũ nhất";
                            list = from a in _db.Brands
                                   where (a.status == "1")
                                   orderby a.brand_id ascending
                                   select a;
                            break;
                        case "name_desc":
                            ViewBag.sortname = "Xếp theo: Z-A";
                            list = from a in _db.Brands
                                   where (a.status == "1")
                                   orderby a.brand_name descending
                                   select a;
                            break;
                        case "name_asc":
                            ViewBag.sortname = "Xếp theo: A-Z";
                            list = from a in _db.Brands
                                   where (a.status == "1")
                                   orderby a.brand_name
                                   select a;
                            break;
                    }

                    if (string.IsNullOrEmpty(search)) return View(list.ToPagedList(pageNumber, pageSize));
                    switch (show)
                    {
                        case "1":
                            ViewBag.searchName = "Tìm kiếm tất cả";
                            list = (IOrderedQueryable<Brand>)list.Where(s => s.brand_id.ToString().Contains(search)
                                                                             || s.brand_name.Contains(search)); //tìm kiếm tất cả
                            break;
                        case "2":
                            ViewBag.searchName = "Tìm kiếm ID";
                            list = (IOrderedQueryable<Brand>)list.Where(s => s.brand_id.ToString().Contains(search)); //theo id
                            break;
                        case "3":
                            ViewBag.searchName = "Tìm kiếm theo tên ";
                            list = (IOrderedQueryable<Brand>)list.Where(s => s.brand_name.Contains(search)); //theo tên
                            break;
                    }

                    return View("BrandIndex", list.ToPagedList(pageNumber, 50));
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
        //gợi ý search thương hiệu
        [HttpPost]
        public JsonResult GetBrandSearch(string Prefix)
        {
            var search = (from c in _db.Brands
                          where c.brand_name.Contains(Prefix)
                          orderby c.brand_name ascending
                          select new { c.brand_name, c.brand_image});
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //View list Trash thương hiệu
        public ActionResult BrandTrash(string search,string show,int? size, int? page,string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 10);
                    var pageNumber = (page ?? 1);
                    ViewBag.countTrash = _db.Brands.Count(a => a.status == "2");
                    ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.NameSort = sortOrder == "name_asc" ? "name_desc" : "name_asc";
                    var list = from a in _db.Brands
                               where a.status == "2"
                               orderby a.update_at descending
                               select a;

                    switch (sortOrder)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Xếp theo: Mới nhất";
                            list = (from a in _db.Brands
                                    where (a.status == "2")
                                    orderby a.brand_id descending
                                    select a);
                            break;
                        case "date_asc":
                            ViewBag.sortname = "Xếp theo: Cũ nhất";
                            list = from a in _db.Brands
                                   where (a.status == "2")
                                   orderby a.brand_id
                                   select a;
                            break;
                        case "name_desc":
                            ViewBag.sortname = "Xếp theo: Z-A";
                            list = from a in _db.Brands
                                   where (a.status == "2")
                                   orderby a.brand_name descending
                                   select a;
                            break;
                        case "name_asc":
                            ViewBag.sortname = "Xếp theo: A-Z";
                            list = from a in _db.Brands
                                   where (a.status == "2")
                                   orderby a.brand_name
                                   select a;
                            break;
                    }
                    if (string.IsNullOrEmpty(search)) return View(list.ToPagedList(pageNumber, pageSize));
                    switch (show)
                    {
                        //tìm kiếm tất cả
                        case "1":
                            list = (IOrderedQueryable<Brand>)list.Where(s => s.brand_id.ToString().Contains(search) || s.brand_name.Contains(search)
                                || s.create_by.Contains(search));
                            break;
                        //theo id
                        case "2":
                            list = (IOrderedQueryable<Brand>)list.Where(s => s.brand_id.ToString().Contains(search));
                            break;
                        //theo tên thể loại
                        case "3":
                            list = (IOrderedQueryable<Brand>)list.Where(s => s.brand_name.ToString().Contains(search));
                            break;
                    }
                    return View("BrandTrash", list.ToPagedList(pageNumber, 50));
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
        //Thông tin chi tiết thương hiệu
        public ActionResult BrandDetails(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Read() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var brand = _db.Brands.SingleOrDefault(a => a.brand_id == id);
                    if (brand != null && id != null) return View(brand);
                    Notification.set_flash("Không tồn tại thương hiệu: " + brand.brand_name + "", "warning");
                    return RedirectToAction("BrandIndex");
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
        //View thêm thương hiệu
        public ActionResult BrandCreate()
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
                    return RedirectToAction("BrandIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Code xử lý thêm thương hiệu
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult BrandCreate(Brand brand)
        {
            string slug = SlugGenerator.SlugGenerator.GenerateSlug(brand.brand_name);
            try
            {
                var checkslug = _db.Brands.Any(m => m.slug == slug);
                if (checkslug)
                {
                    brand.slug = slug + "-" + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + new Random().Next(1, 1000);
                }
                else
                {
                    brand.slug = SlugGenerator.SlugGenerator.GenerateSlug(brand.brand_name);
                }
                brand.create_at = DateTime.Now;
                brand.create_by = User.Identity.GetEmail();
                if (brand.brand_image!=null)
                {
                    brand.brand_image = brand.brand_image;
                }
                else
                {
                    brand.brand_image = "/Images/ImagesCollection/no-image-available.png";
                }
                brand.Web_directory = brand.Web_directory;
                brand.description = brand.description;
                brand.update_at = DateTime.Now; 
                brand.status = brand.status;
                brand.update_by = User.Identity.GetEmail();
                _db.Brands.Add(brand);
                _db.SaveChanges();
                Notification.set_flash("Đã thêm mới thương hiệu: " + brand.brand_name + "", "success");
                return RedirectToAction("BrandIndex");
            }
            catch
            {
                Notification.set_flash("Lỗi", "danger");
            }
            return View(brand);
        }
        //View chỉnh sửa thông tin thương hiệu
        public ActionResult BrandEdit(int? id,string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("BrandEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                var brand = _db.Brands.SingleOrDefault(pro => pro.brand_id == id);
                if ((User.Identity.Permiss_Modify() == true) && (brand.status == "1" || brand.status == "0"))
                {
                    if (brand != null)
                    {
                        return View(brand);
                    }
                    else
                    {
                        Notification.set_flash("Không tồn tại thương hiệu: " + brand.brand_name + "", "warning");
                        return RedirectToAction("BrandIndex");
                    }
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("BrandIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Code xử lý chỉnh sửa thông tin thương hiệu
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult BrandEdit(Brand brand, string returnUrl)
        {
            try
            {
                brand.Web_directory = brand.Web_directory;
                brand.update_at = DateTime.Now;
                brand.brand_image = brand.brand_image;
                brand.description = brand.description;
                brand.status = brand.status ;
                brand.update_by = User.Identity.GetEmail();
                _db.Entry(brand).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Đã cập nhật lại thông tin thương hiệu: " + brand.brand_name + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("BrandIndex");
            }
            catch
            {
                Notification.set_flash("Lỗi", "danger");
            }
            return View(brand);
        }
        //Vô hiệu hóa thương hiệu
        public ActionResult DelTrash(int? id) //bỏ sp vào thùng rác
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                var brand = _db.Brands.SingleOrDefault(pro => pro.brand_id == id);
                if (brand == null || id == null)
                {
                    Notification.set_flash("Không tồn tại thương hiệu: " + brand.brand_name + "", "warning");
                    return RedirectToAction("BrandIndex");
                }
                brand.status = "2";
                brand.update_at = DateTime.Now;
                brand.update_by = User.Identity.GetEmail();
                _db.Entry(brand).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Đã chuyển thương hiệu: " + brand.brand_name + " vào thùng rác", "success");
                return RedirectToAction("BrandIndex");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("BrandIndex");
            }
        }
        //Khôi phục thương hiệu từ thùng rác
        public ActionResult Undo(int? id) // khôi phục từ thùng rác
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                var brand = _db.Brands.SingleOrDefault(pro => pro.brand_id == id);
                if (brand == null || id == null)
                {
                    Notification.set_flash("Không tồn tại thương hiệu: " + brand.brand_name + "", "warning");
                    return RedirectToAction("BrandIndex");
                }
                brand.status = "1";
                brand.update_at = DateTime.Now;
                brand.update_by = User.Identity.GetEmail();
                _db.Entry(brand).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Khôi phục thành công thương hiệu: " + brand.brand_name + "", "success");
                return RedirectToAction("BrandTrash");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("BrandTrash");
            }
        }
        //Xóa thương hiệu
        [HttpPost]
        public JsonResult BrandDelete(int id)
        {
            Boolean result;
            if (User.Identity.Permiss_Delete() == true)
            {
                Brand brand = _db.Brands.Find(id);
                if (brand.Products.Count > 0)
                {
                    result = false;
                }
                else
                {
                    _db.Brands.Remove(brand);
                    _db.SaveChanges();
                    result = true;
                    Notification.set_flash("Xóa thành công id " + id + "", "success");
                }
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