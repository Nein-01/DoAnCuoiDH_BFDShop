using DoAn_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
// using DoAn_LapTrinhWeb.Models;
namespace DoAn_LapTrinhWeb.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private DbContext db = new DbContext();
        public ActionResult Index()
        {
           
            BannerGlobal();
            return View();
        }
        //Error 404 hiện khi sai URL
        public ActionResult PageNotFound()
        {
            BannerGlobal();
            return View();
        }
        //View Gửi yêu cầu hỗ trợ
        public ActionResult SentRequest()
        {
            BannerGlobal();
            return View();
        }
        //Code cử lý Gửi yêu càu hỗ trợ
       
        //hiển thị banner toàn layout
        public void BannerGlobal()
        {
            ViewBag.BannerTopHorizontal = db.Banners.OrderByDescending(m => Guid.NewGuid()).Where(m => m.banner_start < DateTime.Now && m.banner_end > DateTime.Now && m.status == "1" && m.banner_type == 3).Take(8).ToList();
        }
    }
}