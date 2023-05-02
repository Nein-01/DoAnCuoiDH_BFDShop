using System;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly DbContext db = new DbContext();
        //View thống kê 
        public ActionResult Index(string sortOrder, DateTime? picker1, DateTime? picker2)
        {
            if (User.Identity.IsAuthenticated)
            {
                DateTime today = DateTime.Today;
                ViewBag.CurrentSort = sortOrder;
                ViewBag.ResetSort = String.IsNullOrEmpty(sortOrder) ? "" : "";
                ViewBag.TodaySortParm = sortOrder == "today" ? "today" : "today";
                ViewBag.DaysAgoSortParm = sortOrder == "7daysago" ? "7daysago" : "7daysago";
                ViewBag.MonthSortParm = sortOrder == "month" ? "month" : "month";
                ViewBag.YearSortParm = sortOrder == "year" ? "year" : "year";
                switch (sortOrder)
                {
                    case "today"://sort ngày hiện tại
                        DateTime before_day = DateTime.Now.AddDays(-1);
                        ViewBag.CountTotalOrder = db.Orders.Where(m => m.status != "0" && m.oder_date.Day == today.Day && m.oder_date.Month == today.Month && m.oder_date.Year == today.Year).Count();
                        ViewBag.CountTotalOrderbefore = db.Orders.Where(m => m.oder_date.Day == before_day.Day && m.oder_date.Month == today.Month).Count();
                        ViewBag.CountquantitySales = db.Order_Detail.Where(m => m.Order.status != "0" && m.Order.oder_date.Day == today.Day && m.Order.oder_date.Month == today.Month).Sum(m => (int?)m.quantity)??0;
                        ViewBag.CountRevenue = db.Orders.Where(m =>m.status=="3" && m.oder_date.Day == today.Day && m.oder_date.Month == today.Month).Sum(m => (int?)m.total)??0;
                        ViewBag.CountFeedback = db.Feedbacks.Where(m => m.create_at.Day == today.Day && m.parent_feedback_id == 0 && m.create_at.Month == today.Month).Count();
                        break;
                    case "7daysago":
                        //tính 7 ngày trước, bỏ ra ngày hiện tại nên addday(-8) 
                        DateTime start_date = DateTime.Now.AddDays(-8);
                        DateTime end_date = DateTime.Now.AddDays(-1);
                        ViewBag.CountTotalOrder = db.Orders.Where(m => m.status != "0" && m.oder_date > start_date && m.oder_date < end_date).Count();
                        ViewBag.CountquantitySales = db.Order_Detail.Where(m => m.Order.status != "0" &&m.Order.oder_date > start_date && m.Order.oder_date < end_date).Sum(m => (int?)m.quantity)??0;
                        ViewBag.CountRevenue = db.Orders.Where(m => m.status == "3"  && m.oder_date > start_date && m.oder_date < end_date).Sum(m => (int?)m.total)??0;
                        ViewBag.CountFeedback = db.Feedbacks.Where(m => m.parent_feedback_id == 0 && (m.create_at > start_date && m.create_at < end_date)).Count();
                        break;
                    case "month":
                        ViewBag.CountTotalOrder = db.Orders.Where(m => m.status != "0" && m.oder_date.Month == today.Month && m.oder_date.Year == today.Year).Count();
                        ViewBag.CountquantitySales = db.Order_Detail.Where(m => m.Order.status != "0" && m.Order.oder_date.Month == today.Month && m.Order.oder_date.Year == today.Year).Sum(m => (int?)m.quantity) ?? 0;
                        ViewBag.CountRevenue = db.Orders.Where(m => m.status == "3" && m.oder_date.Month == today.Month && m.oder_date.Year == today.Year).Sum(m => (int?)m.total) ?? 0;
                        ViewBag.CountFeedback = db.Feedbacks.Where(m => m.parent_feedback_id == 0 && m.create_at.Month == today.Month && m.create_at.Year == today.Year).Count();
                        break;
                    case "year":
                        ViewBag.CountTotalOrder = db.Orders.Where(m => m.status != "0" && m.oder_date.Year == today.Year).Count();
                        ViewBag.CountquantitySales = db.Order_Detail.Where(m => m.Order.status != "0" && m.Order.oder_date.Year == today.Year).Sum(m => (int?)m.quantity)??0;
                        ViewBag.CountRevenue = db.Orders.Where(m => m.status == "3" && m.oder_date.Year == today.Year).Sum(m => (int?)m.total)??0;
                        ViewBag.CountFeedback = db.Feedbacks.Where(m => m.parent_feedback_id == 0 && m.create_at.Year == today.Year).Count();
                        break;
                    default:
                        DateTime before_day1 = DateTime.Now.AddDays(-1);
                        ViewBag.CountTotalOrder = db.Orders.Where(m => m.status != "0" && m.oder_date.Day == today.Day && m.oder_date.Month == today.Month && m.oder_date.Year == today.Year).Count();
                        ViewBag.CountquantitySales = db.Order_Detail.Where(m => m.Order.status != "0" && m.Order.oder_date.Day == today.Day && m.Order.oder_date.Month == today.Month && m.Order.oder_date.Year == today.Year).Sum(m => (int?)m.quantity) ?? 0;
                        ViewBag.CountRevenue = db.Orders.Where(m => m.status == "3" && m.oder_date.Day == today.Day && m.oder_date.Month == today.Month && m.oder_date.Year == today.Year).Sum(m => (int?)m.total) ?? 0;
                        ViewBag.CountFeedback = db.Feedbacks.Where(m =>  m.create_at.Day == today.Day && m.parent_feedback_id == 0 && m.create_at.Month == today.Month && m.create_at.Year == today.Year).Count();
                        break;
                }
                return View();
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Biểu đồ tổng doanh thu
        public ActionResult Revenue(int id)
        {
            DateTime today = DateTime.Today;
            DateTime fiveyearago = DateTime.Today.AddYears(-5);
            var query = from o in db.Orders
                        where o.status == "3" && o.oder_date.Month == today.Month && o.oder_date.Year == today.Year
                        orderby o.oder_date ascending
                        group o by o.oder_date.Day into g
                        select new
                        {
                            days = g.Key,
                            total = g.Sum(w => w.total),
                        }; 
            switch (id)
            {
                case 1:
                     query = from o in db.Orders
                                where o.status == "3" && o.oder_date.Month == today.Month && o.oder_date.Year == today.Year
                                orderby o.oder_date ascending
                                group o by o.oder_date.Day into g
                                select new
                                {
                                    days = g.Key,
                                    total = g.Sum(w => w.total),
                                };
                    break;
                case 2:
                    query = from o in db.Orders
                                where o.status == "3" && o.oder_date.Year == today.Year
                                orderby o.oder_date ascending
                                group o by o.oder_date.Month into g
                                select new
                                {
                                    days = g.Key,
                                    total = g.Sum(w => w.total),
                                };
                    break;
                case 3:
                    query = (from o in db.Orders
                            where o.status == "3" && o.oder_date.Year > fiveyearago.Year && o.oder_date.Year <= today.Year
                             orderby o.oder_date ascending
                            group o by o.oder_date.Year into g
                            select new
                            {
                                days = g.Key,
                                total = g.Sum(w => w.total),
                            }).Take(5);
                    break;
            }
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        //Số đơn hàng 
        public ActionResult CountTotalOrder(int id)
        {
            DateTime today = DateTime.Today;
            DateTime fiveyearago = DateTime.Today.AddYears(-5);
            var query = from o in db.Orders
                        where o.oder_date.Month == today.Month && o.status != "0" && o.oder_date.Year == today.Year
                        orderby o.oder_date ascending
                        group o by o.oder_date.Day into g
                        select new
                        {
                            time = g.Key,
                            counttotal = g.Count(),
                        };
            switch (id)
            {
                case 1:
                    query = from o in db.Orders
                            where o.oder_date.Month == today.Month && o.status != "0" && o.oder_date.Year == today.Year
                            orderby o.oder_date ascending
                            group o by o.oder_date.Day into g
                            select new
                            {
                                time = g.Key,
                                counttotal = g.Count(),
                            };
                    break;
                case 2:
                    query = from o in db.Orders
                            where o.oder_date.Year == today.Year && o.status != "0"
                            orderby o.oder_date ascending
                            group o by o.oder_date.Month into g
                            select new
                            {
                                time = g.Key,
                                counttotal = g.Count(),
                            };
                    break;
                case 3:
                    query = (from o in db.Orders
                             where o.oder_date.Year > fiveyearago.Year && o.oder_date.Year <= today.Year && o.status != "0"
                             orderby o.oder_date ascending
                             group o by o.oder_date.Year into g
                             select new
                             {
                                 time = g.Key,
                                 counttotal = g.Count(),
                             }).Take(5);
                    break;
            }
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        //Số lượng sản phẩm bán ra
        public ActionResult CountQuantityProductlOrder(int? id)
        {
            DateTime today = DateTime.Today;
            DateTime sevendayago = DateTime.Now.AddDays(-8);
            var query = db.Order_Detail.Include("Product")
                   .Where(m => m.Order.status != "0" && m.Order.oder_date.Day  == today.Day && m.Order.oder_date.Month == today.Month && m.Order.oder_date.Year == today.Year)
                   .GroupBy(p => p.Product.product_name)
                   .Select(g => new { name = g.Key, count = g.Sum(w => w.quantity)}).OrderByDescending(m => m.count).Take(5).ToList();
            switch (id)
            {
                case 1:
                    query = db.Order_Detail.Include("Product")
                   .Where(m => m.Order.status != "0" && m.Order.oder_date.Day == today.Day && m.Order.oder_date.Month == today.Month && m.Order.oder_date.Year == today.Year)
                   .GroupBy(p => p.Product.product_name)
                   .Select(g => new { name = g.Key, count = g.Sum(w => w.quantity)}).OrderByDescending(m => m.count).Take(5).ToList();
                    break;
                case 2:
                    query = db.Order_Detail.Include("Product")
                   .Where(m => m.Order.status != "0"&& m.Order.oder_date > sevendayago)
                   .GroupBy(p => p.Product.product_name)
                   .Select(g => new { name = g.Key, count = g.Sum(w => w.quantity) }).OrderByDescending(m => m.count).Take(5).ToList();
                    break;
                case 3:
                    query = db.Order_Detail.Include("Product")
                   .Where(m => m.Order.status != "0" && m.Order.oder_date.Month == today.Month && m.Order.oder_date.Year == today.Year)
                   .GroupBy(p => p.Product.product_name)
                   .Select(g => new { name = g.Key, count = g.Sum(w => w.quantity)}).OrderByDescending(m => m.count).Take(5).ToList();
                    break;
                case 4:
                    query = db.Order_Detail.Include("Product")
                   .Where(m => m.Order.status != "0" && m.Order.oder_date.Year== today.Year)
                   .GroupBy(p => p.Product.product_name)
                   .Select(g => new { name = g.Key, count = g.Sum(w => w.quantity)}).OrderByDescending(m => m.count).Take(5).ToList();
                    break;
            }
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        //Khu vực đặt hàng nhiều
        public ActionResult CountAddressOrder(int? id)
        {
            DateTime today = DateTime.Today;
            DateTime sevendayago = DateTime.Now.AddDays(-8);
            var query = db.Orders.Include("Product")
                   .Where(m => m.status != "0" && m.oder_date.Day == today.Day && m.oder_date.Month == today.Month && m.oder_date.Year == today.Year)
                   .GroupBy(p => p.OrderAddress.order_adress_province)
                   .Select(g => new { name = g.Key, count = g.Count()}).OrderByDescending(m => m.count).Take(5).ToList();
            switch (id)
            {
                case 1:
                    query = db.Orders.Include("Product")
                   .Where(m => m.status != "0" && m.oder_date.Day == today.Day && m.oder_date.Month == today.Month && m.oder_date.Year == today.Year)
                   .GroupBy(p => p.OrderAddress.order_adress_province)
                   .Select(g => new { name = g.Key, count = g.Count()}).OrderByDescending(m => m.count).Take(5).ToList();
                    break;
                case 2:
                    query = db.Orders.Include("Product")
                   .Where(m => m.status != "0" && m.oder_date > sevendayago)
                   .GroupBy(p => p.OrderAddress.order_adress_province)
                   .Select(g => new { name = g.Key, count = g.Count()}).OrderByDescending(m => m.count).Take(5).ToList();
                    break;
                case 3:
                    query = db.Orders.Include("Product")
                   .Where(m => m.status != "0" && m.oder_date.Month == today.Month && m.oder_date.Year == today.Year)
                   .GroupBy(p => p.OrderAddress.order_adress_province)
                   .Select(g => new { name = g.Key, count = g.Count()}).OrderByDescending(m => m.count).Take(5).ToList();
                    break;
                case 4:
                    query = db.Orders.Include("Product")
                   .Where(m => m.status != "0" && m.oder_date.Year == today.Year)
                   .GroupBy(p => p.OrderAddress.order_adress_province)
                   .Select(g => new { name = g.Key, count = g.Count()}).OrderByDescending(m => m.count).Take(5).ToList();
                    break;
            }
            return Json(query, JsonRequestBehavior.AllowGet);
        }
    }
}