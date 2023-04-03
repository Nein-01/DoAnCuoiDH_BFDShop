using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using DoAn_LapTrinhWeb.PaymentLibrary;
using DoAn_LapTrinhWeb.ZaloPay;
using DoAn_LapTrinhWeb.ZaloPay.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
namespace DoAn_LapTrinhWeb.Controllers
{
    public class CartController : Controller
    {        
        private DbContext db = new DbContext();
        //View Giỏ hàng
        public ActionResult ViewCart()
        {
            //var cart = this.GetCart();
            //ViewBag.Quans = cart.Item2;
            //var listProduct = cart.Item1.ToList();
            List<Product> listProduct = new List<Product>();
            double discount =0d;
           /* if (Session["Discount"] != null && Session["Discountcode"] != null)
            {
                var code = Session["Discountcode"].ToString();
                var discountupdatequan = db.Discounts.Where(d => d.discounts_code == code).FirstOrDefault();

                if (discountupdatequan.quantity == "0")
                {
                    Notification.set_flash("Mã giảm giá đã hết lượt sử dụng", "danger");
                }
                else if (discountupdatequan.discount_start >= DateTime.Now || discountupdatequan.discount_end <= DateTime.Now)
                {
                    Notification.set_flash("Mã giảm giá không thể sử dụng trong thời gian này", "danger");
                }
                else
                {
                    discount = Convert.ToDouble(Session["Discount"].ToString());
                }
                Session.Remove("Discount");
                Session.Remove("Discountcode");
            }*/
            BannerGlobal();
            ViewBag.Discount = discount;
            return View(listProduct);
        }
        //Xóa sản phẩm khỏi giỏ hàng
       
        //hiển thị banner top toàn layout
        public void BannerGlobal()
        {
            ViewBag.BannerTopHorizontal = db.Banners.OrderByDescending(m => Guid.NewGuid()).Where(m => m.banner_start < DateTime.Now && m.banner_end > DateTime.Now && m.status == "1" && m.banner_type == 3).Take(8).ToList();
        }
    }
}