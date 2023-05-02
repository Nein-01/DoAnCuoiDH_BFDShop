using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class CampaignController : Controller
    {
        private readonly DbContext db = new DbContext();
        //Show List banner
        public ActionResult Listbanner(int ? page)
        {
            int pagesize = 10;
            int cpage = page ?? 1;
            var Listbanner = db.Banners.Where(m => m.status == "1" && m.banner_type==1).ToList();
            ViewBag.BannerDetail = db.Banner_Detail.ToList();
            BannerGlobal();
            return View(Listbanner.ToPagedList(cpage, pagesize));
        }
        //Show sản phẩm của banner khuyến mãi
        public ActionResult Bannerdetail(string slug,int?page)
        {
            ViewBag.Banner = db.Banner_Detail.Where(m => m.Banner.slug == slug).FirstOrDefault();
            ViewBag.AvgFeedback = db.Feedbacks.ToList();
            var bannerimage = db.Banner_Detail.Where(m => m.Banner.slug == slug).FirstOrDefault();
            if (bannerimage.Banner.banner_type == 1)
            {
                ViewBag.Bannerimage = bannerimage.Banner.image_thumbnail;
            }
            else
            {
                ViewBag.Bannernamebreadcumb = db.Banner_Detail.Where(m => m.Banner.slug == slug).FirstOrDefault().Banner.banner_name;
            }
            int pagesize = 12;//cho phép hiện 12 sản phẩm trên mỗi loại sản phẩm
            int cpage = page ?? 1;
            var list = from a in db.Products
                       join c in db.Banner_Detail on a.product_id equals c.product_id
                       join d in db.Banners on c.banner_id equals d.banner_id
                       join e in db.Discounts on a.disscount_id equals e.disscount_id
                       group d by new { d.banner_id, c, e.disscount_id, a } into g
                       where (g.Key.c.status == "1" && g.Key.c.Banner.slug == slug && g.Key.c.Product.quantity != "0")
                       orderby g.Key.c.create_at descending // giảm dần
                       select new BannerDTOs
                       {
                           product_slug = g.Key.c.Product.slug,
                           product_id=g.Key.c.product_id,
                           banner_id = g.Key.c.banner_id,
                           banner_name = g.Key.c.Banner.banner_name,
                           discount_end = g.Key.a.Discount.discount_end,
                           discount_start = g.Key.a.Discount.discount_start,
                           price = g.Key.a.price,
                           discount_price = g.Key.a.Discount.discount_price,
                           product_img = g.Key.a.image,
                           product_name = g.Key.a.product_name,
                           seo_title = g.Key.a.title_seo,
                           discount_status = g.Key.a.Discount.status
                       };
            BannerGlobal();
            return View(list.ToPagedList(cpage, pagesize));
        }
        //hiện thị banner top toàn layout
        public void BannerGlobal()
        {
            ViewBag.BannerTopHorizontal = db.Banners.OrderByDescending(m => Guid.NewGuid()).ThenBy(m=>m.banner_id).Where(m => m.status == "1" && m.banner_type == 3).Take(8).ToList();
        }
    }
}