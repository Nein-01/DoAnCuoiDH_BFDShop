using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class ProductsController : Controller
    {
        private DbContext db = new DbContext();
        //Danh danh sản phẩm
        public ActionResult ListProduct(int? page, string slug, string sortOrder)
        {
           
            BannerGlobal();
            return View("Product");
        }
   
        //hiển thị banner trên cùng khi chuyển page
        public void BannerGlobal()
        {
            ViewBag.BannerTopHorizontal = db.Banners.OrderByDescending(m => Guid.NewGuid()).Where(m => m.banner_start < DateTime.Now && m.banner_end > DateTime.Now && m.status == "1" && m.banner_type == 3).Take(8).ToList();
        }
    }
}