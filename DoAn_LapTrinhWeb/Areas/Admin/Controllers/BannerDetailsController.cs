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
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class BannerDetailsController : BaseController
    {
        private DbContext db = new DbContext();

        //View list sản phẩm của banner
        public ActionResult BannerDIndex(string search, string show, int? size, int? page, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<Banner> bannerlist = db.Banners.Where(m => m.status != "2")
                .OrderByDescending(m => m.banner_id)
                .ThenBy(m => m.banner_name).ToList();
                ViewBag.bannerlist = bannerlist;
                List<Product> productlist = db.Products.OrderBy(m => m.product_name)
                    .Where(m => m.status == "1").ToList();
                ViewBag.productlist = productlist;
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true ||
                User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = size ?? 10;
                    var pageNumber = page ?? 1;
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.ResetSort = "";
                    ViewBag.BannerNameSortParm = sortOrder == "bannername_asc" ? "bannername_desc" : "bannername_asc";
                    ViewBag.ProductNameSortParm = sortOrder == "productname_asc" ? "productname_desc" : "productname_asc";
                    var list = from bd in db.Banner_Detail
                               join b in db.Banners on bd.banner_id equals b.banner_id
                               join p in db.Products on bd.product_id equals p.product_id
                               where (bd.status == "1")
                               orderby bd.product_id descending // giảm dần
                               select new BannerDTOs
                               {
                                   product_slug = p.slug,
                                   banner_id = bd.banner_id,
                                   banner_name = b.banner_name,
                                   banner_detail_id = bd.id,
                                   image_thumbnail = b.image_thumbnail,
                                   product_img = p.image,
                                   product_id = p.product_id,
                                   product_name = p.product_name,
                                   bannerdetail_create_at = bd.create_at,
                                   bannerdetail_status = bd.status,
                                   bannerdetail_create_by = bd.create_by,
                                   banner_start = b.banner_start,
                                   banner_slug = b.slug,
                                   banner_end = b.banner_end
                               };
                    //sắp xếp
                    switch (sortOrder)
                    {
                        case "bannername_asc":
                            list = from bd in db.Banner_Detail
                                   join b in db.Banners on bd.banner_id equals b.banner_id
                                   join p in db.Products on bd.product_id equals p.product_id
                                   where (bd.status == "1")
                                   orderby bd.banner_id
                                   select new BannerDTOs
                                   {
                                       product_slug = p.slug,
                                       banner_id = bd.banner_id,
                                       banner_name = b.banner_name,
                                       banner_detail_id = bd.id,
                                       image_thumbnail = b.image_thumbnail,
                                       product_img = p.image,
                                       product_id = p.product_id,
                                       product_name = p.product_name,
                                       bannerdetail_create_at = bd.create_at,
                                       bannerdetail_status = bd.status,
                                       bannerdetail_create_by = bd.create_by,
                                       banner_start = b.banner_start,
                                       banner_slug = b.slug,
                                       banner_end = b.banner_end
                                   };
                            break;
                        case "bannername_desc":
                            list = from bd in db.Banner_Detail
                                   join b in db.Banners on bd.banner_id equals b.banner_id
                                   join p in db.Products on bd.product_id equals p.product_id
                                   where (bd.status == "1")
                                   orderby bd.banner_id descending
                                   select new BannerDTOs
                                   {
                                       product_slug = p.slug,
                                       banner_id = bd.banner_id,
                                       banner_name = b.banner_name,
                                       banner_detail_id = bd.id,
                                       image_thumbnail = b.image_thumbnail,
                                       product_img = p.image,
                                       product_id = p.product_id,
                                       product_name = p.product_name,
                                       bannerdetail_create_at = bd.create_at,
                                       bannerdetail_status = bd.status,
                                       bannerdetail_create_by = bd.create_by,
                                       banner_start = b.banner_start,
                                       banner_slug = b.slug,
                                       banner_end = b.banner_end
                                   };
                            break;
                        case "productname_asc":
                            list = from bd in db.Banner_Detail
                                   join b in db.Banners on bd.banner_id equals b.banner_id
                                   join p in db.Products on bd.product_id equals p.product_id
                                   where (bd.status == "1")
                                   orderby bd.product_id
                                   select new BannerDTOs
                                   {
                                       product_slug = p.slug,
                                       banner_id = bd.banner_id,
                                       banner_name = b.banner_name,
                                       banner_detail_id = bd.id,
                                       image_thumbnail = b.image_thumbnail,
                                       product_img = p.image,
                                       product_id = p.product_id,
                                       product_name = p.product_name,
                                       bannerdetail_create_at = bd.create_at,
                                       bannerdetail_status = bd.status,
                                       bannerdetail_create_by = bd.create_by,
                                       banner_start = b.banner_start,
                                       banner_slug = b.slug,
                                       banner_end = b.banner_end
                                   };
                            break;
                        case "productname_desc":
                            list = from bd in db.Banner_Detail
                                   join b in db.Banners on bd.banner_id equals b.banner_id
                                   join p in db.Products on bd.product_id equals p.product_id
                                   where (bd.status == "1")
                                   orderby bd.product_id descending
                                   select new BannerDTOs
                                   {
                                       product_slug = p.slug,
                                       banner_id = bd.banner_id,
                                       banner_name = b.banner_name,
                                       banner_detail_id = bd.id,
                                       image_thumbnail = b.image_thumbnail,
                                       product_img = p.image,
                                       product_id = p.product_id,
                                       product_name = p.product_name,
                                       bannerdetail_create_at = bd.create_at,
                                       bannerdetail_status = bd.status,
                                       bannerdetail_create_by = bd.create_by,
                                       banner_start = b.banner_start,
                                       banner_slug = b.slug,
                                       banner_end = b.banner_end
                                   };
                            break;
                    }
                    //search banner
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1")) //tìm kiếm tất cả
                            list = list.Where(s =>
                                s.banner_name.Contains(search) || s.banner_id.ToString().Contains(search) ||
                                s.product_id.ToString().Contains(search) || s.product_name.Contains(search));
                        else if (show.Equals("2")) //theo tên sản phẩm
                            list = list.Where(s => s.banner_name.Contains(search));
                        else if (show.Equals("3")) //theo giá sản phẩm
                            list = list.Where(s => s.product_name.Contains(search));
                        return View("BannerDIndex", list.ToPagedList(pageNumber, 50));
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

        [HttpPost]
        public JsonResult GetBannerDetailSearch(string Prefix)
        {
            var search = (from c in db.Banner_Detail
                where c.Product.product_name.Contains(Prefix)
                orderby c.Product.product_name ascending
                select new { c.Product.product_name, c.Product.slug, c.Product.image });
            return Json(search, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateProductBanner(Banner_Detail banner_Detail)
        {
            bool result;
            try
            {
                if (User.Identity.Permiss_Create() == true)
                {
                    var product = db.Products.Where(m => banner_Detail.product_id == m.product_id).SingleOrDefault();
                    banner_Detail.banner_id = banner_Detail.banner_id;
                    banner_Detail.product_id = banner_Detail.product_id;
                    banner_Detail.genre_id = product.genre_id;
                    banner_Detail.status = "1";
                    banner_Detail.create_by = User.Identity.GetEmail();
                    banner_Detail.create_at = DateTime.Now;
                    db.Banner_Detail.Add(banner_Detail);
                    db.SaveChanges();
                    result = true;
                    Notification.set_flash("Thêm mới thành công", "success");
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //xoá tag
        public JsonResult DeleteProductBanner(int id)
        {
            Boolean result;
            try
            {
                if(User.Identity.Permiss_Delete() == true) { 
                var banner_Detail = db.Banner_Detail.FirstOrDefault(m => m.id == id);
                db.Banner_Detail.Remove(banner_Detail);
                db.SaveChanges();
                result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch
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