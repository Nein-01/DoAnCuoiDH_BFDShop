using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Hosting;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using PagedList;
using Syncfusion.XlsIO;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly DbContext db = new DbContext();
        //List view đơn hàng
        public ActionResult OrderIndex(int?page,int?size, string search, string show,string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = size ?? 10;
                    var pageNumber = page ?? 1;
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
                    ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
                    ViewBag.PhoneNumberSortParm = sortOrder == "phone_asc" ? "phone_desc" : "phone_asc";
                    ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_asc" : "date_desc";
                    ViewBag.WaitingSortParm = sortOrder == "waiting" ? "waiting" : "waiting";
                    ViewBag.ProcessingSortParm = sortOrder == "processing" ? "processing" : "processing";
                    ViewBag.CompletegSortParm = sortOrder == "complete" ? "complete" : "complete";
                    ViewBag.countTrash = db.Orders.Count(a => a.status == "0"); //  đếm tổng sp có trong thùng rác
                    var list = from a in db.Order_Detail
                               join b in db.Orders on a.order_id equals b.order_id
                               group a by new { a.order_id, b } into g
                               where g.Key.b.status != "0"
                               orderby g.Key.b.order_id descending
                               select new OrderDTOs
                               {
                                   order_id = g.Key.order_id,
                                   total_price = g.Key.b.total,
                                   status = g.Key.b.status,
                                   order_date = g.Key.b.oder_date,
                                   update_at = g.Key.b.update_at,
                                   update_by = g.Key.b.update_by,
                                   Name = g.Key.b.Account.Name,
                                   Email = g.Key.b.Account.Email,
                                   Phone = g.Key.b.Account.Phone,
                                   payment_id = g.Key.b.payment_id,
                                   payment_transaction = g.Key.b.payment_transaction
                               };
                    switch (sortOrder)
                    {
                        case "name_asc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.order_id equals b.order_id
                                   group a by new { a.order_id, b } into g
                                   where g.Key.b.status != "0"
                                   orderby g.Key.b.Account.Name ascending
                                   select new OrderDTOs
                                   {
                                       order_id = g.Key.order_id,
                                       total_price = g.Key.b.total,
                                       status = g.Key.b.status,
                                       order_date = g.Key.b.oder_date,
                                       update_at = g.Key.b.update_at,
                                       update_by = g.Key.b.update_by,
                                       Name = g.Key.b.Account.Name,
                                       Email = g.Key.b.Account.Email,
                                       Phone = g.Key.b.Account.Phone,
                                       payment_id = g.Key.b.payment_id,
                                       payment_transaction = g.Key.b.payment_transaction
                                   };
                            break;
                        case "name_desc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.order_id equals b.order_id
                                   group a by new { a.order_id, b } into g
                                   where g.Key.b.status != "0"
                                   orderby g.Key.b.Account.Name descending
                                   select new OrderDTOs
                                   {
                                       order_id = g.Key.order_id,
                                       total_price = g.Key.b.total,
                                       status = g.Key.b.status,
                                       order_date = g.Key.b.oder_date,
                                       update_at = g.Key.b.update_at,
                                       update_by = g.Key.b.update_by,
                                       Name = g.Key.b.Account.Name,
                                       Email = g.Key.b.Account.Email,
                                       Phone = g.Key.b.Account.Phone,
                                       payment_id = g.Key.b.payment_id,
                                       payment_transaction = g.Key.b.payment_transaction
                                   };
                            break;
                        case "price_asc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.order_id equals b.order_id
                                   group a by new { a.order_id, b } into g
                                   where g.Key.b.status != "0"
                                   orderby g.Key.b.total ascending
                                   select new OrderDTOs
                                   {
                                       order_id = g.Key.order_id,
                                       total_price = g.Key.b.total,
                                       status = g.Key.b.status,
                                       order_date = g.Key.b.oder_date,
                                       update_at = g.Key.b.update_at,
                                       update_by = g.Key.b.update_by,
                                       Name = g.Key.b.Account.Name,
                                       Email = g.Key.b.Account.Email,
                                       Phone = g.Key.b.Account.Phone,
                                       payment_id = g.Key.b.payment_id,
                                       payment_transaction = g.Key.b.payment_transaction
                                   };
                            break;
                        case "price_desc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.order_id equals b.order_id
                                   group a by new { a.order_id, b } into g
                                   where g.Key.b.status != "0"
                                   orderby g.Key.b.total descending
                                   select new OrderDTOs
                                   {
                                       order_id = g.Key.order_id,
                                       total_price = g.Key.b.total,
                                       status = g.Key.b.status,
                                       order_date = g.Key.b.oder_date,
                                       update_at = g.Key.b.update_at,
                                       update_by = g.Key.b.update_by,
                                       Name = g.Key.b.Account.Name,
                                       Email = g.Key.b.Account.Email,
                                       Phone = g.Key.b.Account.Phone,
                                       payment_id = g.Key.b.payment_id,
                                       payment_transaction = g.Key.b.payment_transaction
                                   };
                            break;
                        case "waiting":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.order_id equals b.order_id
                                   group a by new { a.order_id, b } into g
                                   where g.Key.b.status == "1"
                                   orderby g.Key.b.order_id descending
                                   select new OrderDTOs
                                   {
                                       order_id = g.Key.order_id,
                                       total_price = g.Key.b.total,
                                       status = g.Key.b.status,
                                       order_date = g.Key.b.oder_date,
                                       update_at = g.Key.b.update_at,
                                       update_by = g.Key.b.update_by,
                                       Name = g.Key.b.Account.Name,
                                       Email = g.Key.b.Account.Email,
                                       Phone = g.Key.b.Account.Phone,
                                       payment_id = g.Key.b.payment_id,
                                       payment_transaction = g.Key.b.payment_transaction
                                   };
                            break;
                        case "processing":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.order_id equals b.order_id
                                   group a by new { a.order_id, b } into g
                                   where g.Key.b.status == "2"
                                   orderby g.Key.b.order_id descending
                                   select new OrderDTOs
                                   {
                                       order_id = g.Key.order_id,
                                       total_price = g.Key.b.total,
                                       status = g.Key.b.status,
                                       order_date = g.Key.b.oder_date,
                                       update_at = g.Key.b.update_at,
                                       update_by = g.Key.b.update_by,
                                       Name = g.Key.b.Account.Name,
                                       Email = g.Key.b.Account.Email,
                                       Phone = g.Key.b.Account.Phone,
                                       payment_id = g.Key.b.payment_id,
                                       payment_transaction = g.Key.b.payment_transaction
                                   };
                            break;
                        case "complete":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.order_id equals b.order_id
                                   group a by new { a.order_id, b } into g
                                   where g.Key.b.status == "3"
                                   orderby g.Key.b.order_id descending
                                   select new OrderDTOs
                                   {
                                       order_id = g.Key.order_id,
                                       total_price = g.Key.b.total,
                                       status = g.Key.b.status,
                                       order_date = g.Key.b.oder_date,
                                       update_at = g.Key.b.update_at,
                                       update_by = g.Key.b.update_by,
                                       Name = g.Key.b.Account.Name,
                                       Email = g.Key.b.Account.Email,
                                       Phone = g.Key.b.Account.Phone,
                                       payment_id = g.Key.b.payment_id,
                                       payment_transaction = g.Key.b.payment_transaction
                                   };
                            break;
                        case "phone_asc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.order_id equals b.order_id
                                   group a by new { a.order_id, b } into g
                                   where g.Key.b.status != "0"
                                   orderby g.Key.b.Account.Phone ascending
                                   select new OrderDTOs
                                   {
                                       order_id = g.Key.order_id,
                                       total_price = g.Key.b.total,
                                       status = g.Key.b.status,
                                       order_date = g.Key.b.oder_date,
                                       update_at = g.Key.b.update_at,
                                       update_by = g.Key.b.update_by,
                                       Name = g.Key.b.Account.Name,
                                       Email = g.Key.b.Account.Email,
                                       Phone = g.Key.b.Account.Phone,
                                       payment_id = g.Key.b.payment_id,
                                       payment_transaction = g.Key.b.payment_transaction
                                   };
                            break;
                        case "phone_desc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.order_id equals b.order_id
                                   group a by new { a.order_id, b } into g
                                   where g.Key.b.status != "0"
                                   orderby g.Key.b.Account.Phone descending
                                   select new OrderDTOs
                                   {
                                       order_id = g.Key.order_id,
                                       total_price = g.Key.b.total,
                                       status = g.Key.b.status,
                                       order_date = g.Key.b.oder_date,
                                       update_at = g.Key.b.update_at,
                                       update_by = g.Key.b.update_by,
                                       Name = g.Key.b.Account.Name,
                                       Email = g.Key.b.Account.Email,
                                       Phone = g.Key.b.Account.Phone,
                                       payment_id = g.Key.b.payment_id,
                                       payment_transaction = g.Key.b.payment_transaction
                                   };
                            break;
                        case "date_asc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.order_id equals b.order_id
                                   group a by new { a.order_id, b } into g
                                   where g.Key.b.status != "0"
                                   orderby g.Key.b.oder_date ascending
                                   select new OrderDTOs
                                   {
                                       order_id = g.Key.order_id,
                                       total_price = g.Key.b.total,
                                       status = g.Key.b.status,
                                       order_date = g.Key.b.oder_date,
                                       update_at = g.Key.b.update_at,
                                       update_by = g.Key.b.update_by,
                                       Name = g.Key.b.Account.Name,
                                       Email = g.Key.b.Account.Email,
                                       Phone = g.Key.b.Account.Phone,
                                       payment_id = g.Key.b.payment_id,
                                       payment_transaction = g.Key.b.payment_transaction
                                   };
                            break;
                        case "date_desc":
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.order_id equals b.order_id
                                   group a by new { a.order_id, b } into g
                                   where g.Key.b.status != "0"
                                   orderby g.Key.b.oder_date descending
                                   select new OrderDTOs
                                   {
                                       order_id = g.Key.order_id,
                                       total_price = g.Key.b.total,
                                       status = g.Key.b.status,
                                       order_date = g.Key.b.oder_date,
                                       update_at = g.Key.b.update_at,
                                       update_by = g.Key.b.update_by,
                                       Name = g.Key.b.Account.Name,
                                       Email = g.Key.b.Account.Email,
                                       Phone = g.Key.b.Account.Phone,
                                       payment_id = g.Key.b.payment_id,
                                       payment_transaction = g.Key.b.payment_transaction
                                   };
                            break;
                        default:
                            list = from a in db.Order_Detail
                                   join b in db.Orders on a.order_id equals b.order_id
                                   group a by new { a.order_id, b } into g
                                   where g.Key.b.status != "0"
                                   orderby g.Key.b.order_id descending
                                   select new OrderDTOs
                                   {
                                       order_id = g.Key.order_id,
                                       total_price = g.Key.b.total,
                                       status = g.Key.b.status,
                                       order_date = g.Key.b.oder_date,
                                       update_at = g.Key.b.update_at,
                                       update_by = g.Key.b.update_by,
                                       Name = g.Key.b.Account.Name,
                                       Email = g.Key.b.Account.Email,
                                       Phone = g.Key.b.Account.Phone,
                                       payment_id = g.Key.b.payment_id,
                                       payment_transaction = g.Key.b.payment_transaction
                                   };
                            break;
                    }
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))//tìm kiếm tất cả
                            list = list.Where(s => s.order_id.ToString().Contains(search) || s.Name.ToString().Trim().Contains(search) || s.status.Trim().Contains(search));
                        else if (show.Equals("2"))//theo id
                            list = list.Where(s => s.order_id.ToString().Contains(search));
                        else if (show.Equals("3"))//theo tên khách hàng
                            list = list.Where(s => s.Name.ToString().Contains(search));
                        else if (show.Equals("4"))//theo số điện thoại
                            list = list.Where(s => s.Phone.ToString().Contains(search));
                        else if (show.Equals("5"))// trạng thái
                            list = list.Where(s => s.status.Contains(search));
                        return View("OrderIndex", list.ToPagedList(pageNumber, 50));
                    }
                    return View(list.ToPagedList(pageNumber, pageSize));
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
        //List view trash đơn hàng
        public ActionResult OrderTrash(int? page, int? size, string search, string show)
        {
            if (User.Identity.IsAuthenticated)
            {
                var pageSize = size ?? 10;
                var pageNumber = page ?? 1;
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var list = from a in db.Order_Detail
                               join b in db.Orders on a.order_id equals b.order_id
                               group a by new { a.order_id, b } into g
                               where g.Key.b.status == "0"
                               orderby g.Key.b.update_at descending
                               select new OrderDTOs
                               {
                                   order_id = g.Key.order_id,
                                   total_price = g.Key.b.total,
                                   status = g.Key.b.status,
                                   update_at = g.Key.b.update_at,
                                   update_by = g.Key.b.update_by,
                                   Name = g.Key.b.Account.Name,
                                   Phone = g.Key.b.Account.Phone
                               };
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))//tìm kiếm tất cả
                            list = list.Where(s => s.order_id.ToString().Contains(search) || s.Name.ToString().Trim().Contains(search) || s.Phone.ToString().Contains(search) || s.status.Trim().Contains(search));
                        else if (show.Equals("2"))//theo id
                            list = list.Where(s => s.order_id.ToString().Contains(search));
                        else if (show.Equals("3"))//theo tên khách hàng
                            list = list.Where(s => s.Name.ToString().Contains(search));
                        else if (show.Equals("4"))//theo số điện thoại
                            list = list.Where(s => s.Phone.ToString().Contains(search));
                        else if (show.Equals("5"))// trạng thái
                            list = list.Where(s => s.status.Contains(search));
                        return View("OrderTrash", list.ToPagedList(pageNumber, 50));
                    }
                    return View(list.ToPagedList(pageNumber, pageSize));
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
        //gợi ý tìm kiếm
        [HttpPost]
        public JsonResult GetOrderSearch(string Prefix)
        {
            var search = (from c in db.Orders
                          where c.status!="0"&& c.order_id.ToString().StartsWith(Prefix)
                          orderby c.order_id descending
                          select new { c.order_id });
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //Thông tin đơn hàng
        public ActionResult OrderDetails(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Read() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var order_detail = db.Order_Detail.FirstOrDefault(m => m.order_id == id);
                    var check_discount = db.Discounts.FirstOrDefault(m => m.discounts_code == order_detail.discount_code);
                    if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    // var order = db.Orders.Find(id);
                    var discountpriceorder = db.Order_Detail.FirstOrDefault(m => m.order_id == id);
                    var discount = db.Discounts.Where(m => m.discounts_code == discountpriceorder.discount_code).FirstOrDefault();
                    var order = (from a in db.Order_Detail
                                 join b in db.Orders on a.order_id equals id
                                 join p in db.Products on a.product_id equals p.product_id
                                 group a by new { a.order_id, b, a.product_id, p } into g
                                 where g.Key.b.order_id == id
                                 select new OrderDTOs
                                 {
                                     discount_price = discount.discount_price,
                                     account_id = g.Key.b.account_id,
                                     order_id = g.Key.order_id,
                                     status = g.Key.b.status,
                                     total_price = g.Key.b.total,
                                     create_at = g.Key.b.create_at,
                                     order_date = g.Key.b.oder_date,
                                     create_by = g.Key.b.create_by,
                                     payment_id = g.Key.b.payment_id,
                                     payment_name = g.Key.b.Payment.payment_name,
                                     delivery_name = g.Key.b.Delivery.delivery_name,
                                     product_name = g.Key.p.product_name,
                                     warranty_time = g.Key.p.warranty_time,
                                     Email = g.Key.b.Account.Email,
                                     payment_transaction = g.Key.b.payment_transaction,
                                     order_note = g.Key.b.order_note,
                                     update_at = g.Key.b.update_at,
                                     update_by = g.Key.b.update_by,
                                     Name = g.Key.b.OrderAddress.order_address_username,
                                     Phone = g.Key.b.OrderAddress.order_address_phonenumber,
                                     Address = g.Key.b.OrderAddress.order_address_content,
                                     discount_type = check_discount.discounts_type,
                                     discount_max = check_discount.discount_max
                                 }).FirstOrDefault();
                    if (order == null || id == null)
                    {
                        Notification.set_flash("Không tồn tại đơn hàng: " + "BFD" + id + ")", "warning");
                        return RedirectToAction("OrderIndex");
                    }
                    ViewBag.orderDetails = db.Order_Detail.Where(m => m.order_id == id).ToList();
                    ViewBag.orderProduct = db.Products.ToList();
                    return View(order);
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
        //hủy đơn hàng
        [HttpPost]
        public JsonResult CancleOrder(string ButtonConfirmlink,int? id, string ProductOrder, string SubOrderTotal, string Discount_Price, string OrderTotal)
        {
            Boolean result;
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            var order = db.Orders.Where(m => m.order_id == id).FirstOrDefault();
            var orderdetail = db.Order_Detail.Where(m => m.order_id == order.order_id).ToList();
            var discountpriceorder = db.Order_Detail.FirstOrDefault(m => m.order_id == id);
            var discount = db.Discounts.Where(m => m.discounts_code == discountpriceorder.discount_code).FirstOrDefault();
            var product = db.Products.ToList();
            if (order.status == "3")
            {
                result = false;
            }
            else
            {
                order.status = "0";
                order.update_at = DateTime.Now;
                order.update_by = User.Identity.GetName();
                string emailID = order.Account.Email;
                string OrderID = order.order_id.ToString();
                string OrderPhone = order.OrderAddress.order_address_phonenumber;
                string OrderName = order.OrderAddress.order_address_username;
                string OrderAddress = order.OrderAddress.order_address_content;
                string OrderDelivery = order.Delivery.delivery_name;
                string DeliveryId = OrderID;
                double pricesum = 0;
                foreach (var item in orderdetail)
                {
                    pricesum += (item.price * item.quantity);
                    ProductOrder += "<tr style='border-bottom: 1px solid rgba(0,0,0,.05);' class='product_item'>" +
                            "<td valign='middle' width ='80%' style='text-align:left; padding:0 2.5em;'> " +
                                "<div class='product-entry'>" +
                                    "<img src='https://bfd.vn/" + item.Product.image + "' style = 'width: 100px; max-width: 600px; height: auto; padding-bottom:5px; display: block;'>" +
                                    "<div class='text'>" +
                                        "<a href='" + Request.Url.Scheme + "://" + Request.Url.Authority + "/product/" + item.Product.slug + "'> <div class='product_name'>" + item.Product.product_name + "</div></a>" +
                                        "<span class='product_quantity'>Số lượng: " + item.quantity + "</span>" +
                                    "</div>" +
                                "</div>" +
                            "</td>" +
                            "<td valign='middle' width='20%' style='text-align:right; padding-right: 2.5em;'>" +
                                "<span class='price' style='color: #005f8f; font-size: 14px; font-weight: 500;'>" + item.price.ToString("#,0", cul.NumberFormat) + "₫" + "</span>" +
                            "</td>" +
                        "</tr>";
                    foreach (var pd in product)
                    {
                        if (item.product_id == pd.product_id)
                        {
                            pd.quantity = (Convert.ToInt32(pd.quantity) + Convert.ToInt32(item.quantity)).ToString();
                        }
                    }
                }
                SubOrderTotal = pricesum.ToString("#,0", cul.NumberFormat) + "₫";
                double discount_price = discount.discount_price;
                if (discount_price == 0)
                {
                    Discount_Price = "0₫";
                }
                else
                {
                    if (discount.discount_price <= 100)
                    {
                        double discount_max_calc = ((discount.discount_price * pricesum) / 100);
                        if (discount_max_calc >= discount.discount_max)
                        {

                            Discount_Price = "<span style='color: #28a745'>" + discount.discount_max.ToString("-#,0", cul.NumberFormat) + "₫" + "</span>";
                        }
                        else
                        {
                            Discount_Price = "<span style='color: #28a745'>" + discount_max_calc.ToString("-#,0", cul.NumberFormat) + "₫" + "</span>";
                        }
                    }
                    else
                    {
                        Discount_Price = "<span style='color: #28a745'>" + discount.discount_price.ToString("-#,0", cul.NumberFormat) + "₫" + "</span>";
                    }
                }
                if (order.payment_id != 1 && order.payment_transaction == "2")
                {
                    OrderTotal = "0₫";
                }
                else
                {
                    OrderTotal = order.total.ToString("#,0", cul.NumberFormat) + "₫";
                }
                string OrderPayment = db.Payments.Where(m => m.payment_id == order.payment_id).FirstOrDefault().payment_name;
                string OrderStatus = "<span style='color:#dc3545;'>Bị huỷ</span>";
                ButtonConfirmlink = Request.Url.Scheme + "://" + Request.Url.Authority + "/account/order_detail/" + order.order_id;
                SendEmailOrders(ButtonConfirmlink,Discount_Price, SubOrderTotal, ProductOrder, OrderPayment, OrderStatus, emailID, OrderID, OrderTotal,
                OrderPhone, OrderName, OrderAddress, OrderDelivery,DeliveryId, "CancleOrders");
                if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
                {
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                    Notification.set_flash("Hủy đơn hàng id " + id + " thành công", "success");
                }
                else
                {
                    result = false;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Chuyển trạng thái chờ 
        public ActionResult ChangeWaitting(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
                {
                    var order = db.Orders.SingleOrDefault(pro => pro.order_id == id && pro.status != "3");
                    if (order != null)
                    {
                        order.status = "1";
                        order.update_at = DateTime.Now;
                        order.update_by = User.Identity.GetName();
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                        Notification.set_flash("Đã chuyển trạng thái đơn hàng: " + "BFD" + id + " sang chờ xử lý!", "success");
                    }
                    else
                    {
                        Notification.set_flash("Đơn hàng: " + "#" + id + " đã được hoàn thành, không thể thay đổi trạng thái khác!", "warning");
                    }
                    return RedirectToAction("OrderIndex");
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("OrderIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Chuyển trạng thái sang đang xử lý
        public ActionResult ChangeProcessing(string ButtonConfirmlink,int? id, string ProductOrder, string SubOrderTotal, string Discount_Price, string OrderTotal,string OrderAddress)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
                {
                    CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                    var order = db.Orders.SingleOrDefault(pro => pro.order_id == id);
                    var orderdetail = db.Order_Detail.Where(m => m.order_id == order.order_id).ToList();
                    var discountpriceorder = db.Order_Detail.FirstOrDefault(m => m.order_id == id);
                    var discount = db.Discounts.Where(m => m.discounts_code == discountpriceorder.discount_code).FirstOrDefault();
                    if (order != null)
                    {
                        order.status = "2";
                        order.update_at = DateTime.Now;
                        order.update_by = User.Identity.GetName();
                        db.Entry(order).State = EntityState.Modified;
                    }
                    string emailID = order.Account.Email;
                    string OrderID = order.order_id.ToString();
                    string OrderPhone = order.OrderAddress.order_address_phonenumber.ToString();
                    string OrderName = order.OrderAddress.order_address_username;
                    string OrderDelivery = order.Delivery.delivery_name;
                    string DeliveryId = OrderID;
                    double pricesum = 0;
                    OrderAddress = order.OrderAddress.order_address_content;
                    foreach (var item in orderdetail)
                    {
                        pricesum += (item.price * item.quantity);
                        ProductOrder += "<tr style='border-bottom: 1px solid rgba(0,0,0,.05);' class='product_item'>" +
                                "<td valign='middle' width ='80%' style='text-align:left; padding:0 2.5em;'> " +
                                    "<div class='product-entry'>" +
                                        "<img src='https://bfd.vn/" + item.Product.image + "' style = 'width: 100px; max-width: 600px; height: auto; padding-bottom:5px; display: block;'>" +
                                        "<div class='text'>" +
                                            "<a href='" + Request.Url.Scheme + "://" + Request.Url.Authority + "/product/" + item.Product.slug + "'> <div class='product_name'>" + item.Product.product_name + "</div></a>" +
                                            "<span class='product_quantity'>Số lượng: " + item.quantity + "</span>" +
                                        "</div>" +
                                    "</div>" +
                                "</td>" +
                                "<td valign='middle' width='20%' style='text-align:right; padding-right: 2.5em;'>" +
                                    "<span class='price' style='color: #005f8f; font-size: 14px; font-weight: 500;'>" + item.price.ToString("#,0", cul.NumberFormat) + "₫" + "</span>" +
                                "</td>" +
                            "</tr>";
                    }
                    SubOrderTotal = pricesum.ToString("#,0", cul.NumberFormat) + "₫";
                    double discount_price = discount.discount_price;
                    if (discount_price == 0)
                    {
                        Discount_Price = "0₫";
                    }
                    else
                    {
                        if (discount.discount_price <= 100)
                        {
                            double discount_max_calc = ((discount.discount_price * pricesum) / 100);
                            if (discount_max_calc >= discount.discount_max)
                            {

                                Discount_Price = "<span style='color: #28a745'>" + discount.discount_max.ToString("-#,0", cul.NumberFormat) + "₫" + "</span>";
                            }
                            else
                            {
                                Discount_Price = "<span style='color: #28a745'>" + discount_max_calc.ToString("-#,0", cul.NumberFormat) + "₫" + "</span>";
                            }
                        }
                        else
                        {
                            Discount_Price = "<span style='color: #28a745'>" + discount.discount_price.ToString("-#,0", cul.NumberFormat) + "₫" + "</span>";
                        }
                    }
                    if (order.payment_id != 1 && order.payment_transaction == "2")
                    {
                        OrderTotal = "0₫";
                    }
                    else
                    {
                        OrderTotal = order.total.ToString("#,0", cul.NumberFormat) + "₫";
                    }
                    string OrderPayment = db.Payments.Where(m => m.payment_id == order.payment_id).FirstOrDefault().payment_name;
                    string OrderStatus = "<span style='color:#17a2b8;'>Đang xử lý</span>";
                    ButtonConfirmlink = Request.Url.Scheme + "://" + Request.Url.Authority + "/account/order_detail/" + order.order_id;
                    SendEmailOrders(ButtonConfirmlink, Discount_Price, SubOrderTotal, ProductOrder, OrderPayment, OrderStatus, emailID, OrderID, OrderTotal,
                    OrderPhone, OrderName, OrderAddress, OrderDelivery, DeliveryId, "ChangeProcessing");
                    db.SaveChanges();
                    Notification.set_flash("Đã chuyển trạng thái đơn hàng: " + "#" + id + " sang đang xử lý!", "success");
                    return RedirectToAction("OrderIndex");
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
        //Chuyển trạng thái đơn hàng sang hoàn thành
        public ActionResult ChangeComplete(string ButtonConfirmlink,int? id, string ProductOrder, string SubOrderTotal, string Discount_Price, string OrderTotal)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
                {
                    CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                    var order = db.Orders.SingleOrDefault(pro => pro.order_id == id);
                    var orderdetail = db.Order_Detail.Where(m => m.order_id == order.order_id).ToList();
                    var discountpriceorder = db.Order_Detail.FirstOrDefault(m => m.order_id == id);
                    var discount = db.Discounts.Where(m => m.discounts_code == discountpriceorder.discount_code).FirstOrDefault();
                    if (order != null)
                    {
                        order.status = "3";
                        order.update_at = DateTime.Now;
                        order.update_by = User.Identity.GetName();
                        db.Entry(order).State = EntityState.Modified;
                    }

                    string emailID = order.Account.Email;
                    string OrderID = order.order_id.ToString();
                    string OrderPhone = order.OrderAddress.order_address_phonenumber;
                    string OrderName = order.OrderAddress.order_address_username;
                    string OrderAddress = order.OrderAddress.order_address_content;
                    string OrderDelivery = order.Delivery.delivery_name;
                    string DeliveryId = OrderID;
                    double pricesum = 0;
                    foreach (var item in orderdetail)
                    {
                        pricesum += (item.price * item.quantity);
                        ProductOrder += "<tr style='border-bottom: 1px solid rgba(0,0,0,.05);' class='product_item'>" +
                                "<td valign='middle' width ='80%' style='text-align:left; padding:0 2.5em;'> " +
                                    "<div class='product-entry'>" +
                                        "<img src='https://bfd.vn/" + item.Product.image + "' style = 'width: 100px; max-width: 600px; height: auto; padding-bottom:5px; display: block;'>" +
                                        "<div class='text'>" +
                                            "<a href='" + Request.Url.Scheme + "://" + Request.Url.Authority + "/product/" + item.Product.slug + "'> <div class='product_name'>" + item.Product.product_name + "</div></a>" +
                                            "<span class='product_quantity'>Số lượng: " + item.quantity + "</span>" +
                                        "</div>" +
                                    "</div>" +
                                "</td>" +
                                "<td valign='middle' width='20%' style='text-align:right; padding-right: 2.5em;'>" +
                                    "<span class='price' style='color: #005f8f; font-size: 14px; font-weight: 500;'>" + item.price.ToString("#,0", cul.NumberFormat) + "₫" + "</span>" +
                                "</td>" +
                            "</tr>";
                    }
                    SubOrderTotal = pricesum.ToString("#,0", cul.NumberFormat) + "₫";
                    double discount_price = discount.discount_price;
                    if (discount_price == 0)
                    {
                        Discount_Price = "0₫";
                    }
                    else
                    {
                        if (discount.discount_price <= 100)
                        {
                            double discount_max_calc = ((discount.discount_price * pricesum) / 100);
                            if (discount_max_calc >= discount.discount_max)
                            {

                                Discount_Price = "<span style='color: #28a745'>" + discount.discount_max.ToString("-#,0", cul.NumberFormat) + "₫" + "</span>";
                            }
                            else
                            {
                                Discount_Price = "<span style='color: #28a745'>" + discount_max_calc.ToString("-#,0", cul.NumberFormat) + "₫" + "</span>";
                            }
                        }
                        else
                        {
                            Discount_Price = "<span style='color: #28a745'>" + discount.discount_price.ToString("-#,0", cul.NumberFormat) + "₫" + "</span>";
                        }
                    }
                    if (order.payment_id != 1 && order.payment_transaction == "2")
                    {
                        OrderTotal = "0₫";
                    }
                    else
                    {
                        OrderTotal = order.total.ToString("#,0", cul.NumberFormat) + "₫";
                    }
                    string OrderPayment = db.Payments.Where(m => m.payment_id == order.payment_id).FirstOrDefault().payment_name;
                    string OrderStatus = "<span style='color:#28a745;'>Giao thành công</span>";
                    ButtonConfirmlink = Request.Url.Scheme + "://" + Request.Url.Authority + "/account/order_detail/" + order.order_id;
                    SendEmailOrders(ButtonConfirmlink, Discount_Price, SubOrderTotal, ProductOrder, OrderPayment, OrderStatus, emailID, OrderID, OrderTotal,
                    OrderPhone, OrderName, OrderAddress, OrderDelivery, DeliveryId, "ChangeComplete");
                    db.SaveChanges();
                    Notification.set_flash("Đã chuyển trạng thái đơn hàng: " + id + " sang Hoàn thành!", "success");
                    return RedirectToAction("OrderIndex");
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("OrderIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Gửi Email trạng thái sản phẩm
        public void SendEmailOrders(string ButtonConfirmlink,string Discount_Price,string SubOrderTotal, string ProductOrder,string OrderPayment, string OrderStatus, string emailID,string OrderID,string OrderTotal,
        string OrderPhone,string OrderName,string OrderAddress,string OrderDelivery,string DeliveryId, string emailFor)
        {
            // đường dẫn mail gồm có controller "Account"  +"emailfor" +  "code reset đã được mã hóa(mội lần gửi email quên mật khẩu sẽ random 1 code reset mới"
            ///để dùng google email gửi email reset cho người khác bạn cần phải vô đây "https://www.google.com/settings/security/lesssecureapps" Cho phép ứng dụng kém an toàn: Bật
            var fromEmail = new MailAddress(AccountEmail.UserEmail, AccountEmail.Name); // "username email-vd: vn123@gmail.com" ,"tên hiển thị mail khi gửi"
            var toEmail = new MailAddress(emailID);
            //nhập password của bạn
            var fromEmailPassword = AccountEmail.Password;
            string subject = "";
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "MailOrders" + ".cshtml"); //dùng body mail html , file template nằm trong thư mục "EmailTemplate/Text.cshtml"
            if (emailFor == "ChangeProcessing")
            {
                subject = "Đơn hàng #"+ OrderID + " đang được xử lý";
                body = body.Replace("{{OrderId}}", OrderID);
                body = body.Replace("{{BodyContent}}", "Một kiện hàng thuộc đơn hàng <span style='color: rgb(1, 181, 187); font-weight: 500;'>#" + OrderID + "</span> đã được đóng gói và giao cho đơn vị vận chuyển.");
                body = body.Replace("{{OrderStatus}}", OrderStatus);
                body = body.Replace("{{ButtonConfirm}}", "Quản lý đơn hàng");
                body = body.Replace("{{ButtonConfirmLink}}", ButtonConfirmlink); 
                body = body.Replace("{{UserEmail}}", emailID);
                body = body.Replace("{{DiscountPrice}}", Discount_Price);
                body = body.Replace("{{UserName}}", OrderName);
                body = body.Replace("{{UserAddress}}", OrderAddress);
                body = body.Replace("{{UserPhoneNumber}}", OrderPhone);
                body = body.Replace("{{SubOrderTotal}}", SubOrderTotal);
                body = body.Replace("{{OrderTotal}}", OrderTotal);
                body = body.Replace("{{ProductOrder}}", ProductOrder);
                body = body.Replace("{{Payment}}", OrderPayment);
                body = body.Replace("{{Delivery}}", OrderDelivery);
                body = body.Replace("{{DeliveryId}}", DeliveryId);
            }
            else if (emailFor == "ChangeComplete")
            {
                subject = "Đơn hàng #" + OrderID + " đã giao thành công";
                body = body.Replace("{{OrderId}}", OrderID);
                body = body.Replace("{{BodyContent}}", "Đơn hàng <span style='color: rgb(1, 181, 187); font-weight: 500;'>#" + OrderID + "</span> đã được giao thành công. Cảm ơn bạn đã tin tưởng BFDComputer, hi vọng trong tương lai bạn sẽ tiếp tục sử dụng dịch vụ của Chúng tôi.");
                body = body.Replace("{{ButtonConfirm}}", "Quản lý đơn hàng");
                body = body.Replace("{{ButtonConfirmLink}}", ButtonConfirmlink); 
                body = body.Replace("{{OrderStatus}}", OrderStatus);
                body = body.Replace("{{UserEmail}}", emailID);
                body = body.Replace("{{DiscountPrice}}", Discount_Price);
                body = body.Replace("{{UserName}}", OrderName);
                body = body.Replace("{{UserAddress}}", OrderAddress);
                body = body.Replace("{{UserPhoneNumber}}", OrderPhone);
                body = body.Replace("{{SubOrderTotal}}", SubOrderTotal);
                body = body.Replace("{{OrderTotal}}", OrderTotal);
                body = body.Replace("{{ProductOrder}}", ProductOrder);
                body = body.Replace("{{Payment}}", OrderPayment);
                body = body.Replace("{{Delivery}}", OrderDelivery);
                body = body.Replace("{{DeliveryId}}", DeliveryId);
            }
            else if (emailFor == "CancleOrders")
            {
                subject = "Đơn hàng #" + OrderID + " đã bị huỷ";
                body = body.Replace("{{OrderId}}", OrderID);
                body = body.Replace("{{BodyContent}}", "Đơn hàng <span style='color: rgb(1, 181, 187); font-weight: 500;'>#" + OrderID + "</span> đã được hủy theo yêu cầu của bạn. BFDComputer bắt đầu tiến hành thủ tục hoàn tiền và sẽ thông tin đến bạn khi đã hoàn tất.");
                body = body.Replace("{{ButtonConfirm}}", "Quản lý đơn hàng");
                body = body.Replace("{{ButtonConfirmLink}}", ButtonConfirmlink);
                body = body.Replace("{{OrderStatus}}", OrderStatus);
                body = body.Replace("{{UserEmail}}", emailID);
                body = body.Replace("{{DiscountPrice}}", Discount_Price);
                body = body.Replace("{{UserName}}", OrderName);
                body = body.Replace("{{UserAddress}}", OrderAddress);
                body = body.Replace("{{UserPhoneNumber}}", OrderPhone);
                body = body.Replace("{{SubOrderTotal}}", SubOrderTotal);
                body = body.Replace("{{OrderTotal}}", OrderTotal);
                body = body.Replace("{{ProductOrder}}", ProductOrder);
                body = body.Replace("{{Payment}}", OrderPayment);
                body = body.Replace("{{Delivery}}", OrderDelivery);
                body = body.Replace("{{DeliveryId}}", DeliveryId);
            }
            var smtp = new SmtpClient
            {
                Host = AccountEmail.Host, //tên mấy chủ nếu bạn dùng gmail thì đổi  "Host = "smtp.gmail.com"; email thi Host = AccountEmail.Host
                Port = 587,
                EnableSsl = true, //bật ssl
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };
            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        //doanh thu 
        public ActionResult Turnover(string sortOrder, int? size, int? page)
        {
            var pageSize = size ?? 10;
            var pageNumber = page ?? 1;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.MonthSortParm = sortOrder == "total_month" ? "total_month" : "total_month";
            ViewBag.YearSortParm = sortOrder == "total_year" ? "total_year" : "total_year";
            var order = db.Orders.ToList();
            switch (sortOrder)
            {
                case "total_month":
                    order.Where(m => m.status == "2").Sum(m => m.total);
                    break;
                default:
                    order.Where(m => m.status == "3").Sum(m => m.total);
                    break;
            }
            return View(order.ToPagedList(pageNumber, pageSize));
        }
        //Report xuất excel
        public void ExportDataToExcel(DateTime picker1 , DateTime picker2, string order__status,int payment__method)
        {
            //Create an instance of ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Initialize Application
                IApplication application = excelEngine.Excel;
                //Set the default application version as Excel 2016
                application.DefaultVersion = ExcelVersion.Excel2016;
                //Create a workbook with a worksheet
                IWorkbook workbook = application.Workbooks.Create(1);
                //Access first worksheet from the workbook instance
                IWorksheet worksheet = workbook.Worksheets[0];               
                //Create a DataTable with four columns
                DataTable table = new DataTable();
                table.Columns.Add("STT", typeof(int));
                table.Columns.Add("ID Đơn hàng", typeof(int));
                table.Columns.Add("Họ tên", typeof(string));
                table.Columns.Add("Số điện thoại", typeof(string));
                table.Columns.Add("Địa chỉ nhận hàng", typeof(string));
                table.Columns.Add("Phương thức thanh toán", typeof(string));
                table.Columns.Add("Trạng thái giao dịch", typeof(string));
                table.Columns.Add("Ngày đặt", typeof(DateTime));
                table.Columns.Add("Trạng thái đơn hàng", typeof(string));
                table.Columns.Add("Total", typeof(double));
                List<Order> order = db.Orders.ToList();
                int stt = 0;
                string payment_transaction="Không";
                string order_status;
                foreach (var item in order)
                {
                    if (item.oder_date.Date>= picker1.Date && item.oder_date.Date <= picker2.Date)
                    {
                        if (item.payment_transaction == "2" && item.payment_id != 1)
                        {
                            payment_transaction = "Đã thanh toán";
                        }
                        else if (item.payment_transaction == "1" && item.payment_id != 1)
                        {
                            payment_transaction = "Chưa hoàn tất thanh toán";
                        }
                        else
                        {
                            payment_transaction = "Không";
                        }
                        switch(item.status)
                        {
                            case "0":
                                order_status = "Đã huỷ";
                                break;
                            case "1":
                                order_status = "Chờ xử lý";
                                break;
                            case "2":
                                order_status = "Đang xử lý";
                                break;
                            default:
                                order_status = "Đã hoàn thành";
                                break;
                        }
                        //order__status == "4" hoặc payment__method == 4 thì in ra tất cả các trạng thái, phương thức thanh toán của list đơn hàng, 
                        if (order__status == "4" && payment__method == 4)
                        {
                            stt++;
                            table.Rows.Add(stt, item.order_id, item.OrderAddress.order_address_username, "'" + item.OrderAddress.order_address_phonenumber, item.OrderAddress.order_address_content, item.Payment.payment_name, payment_transaction, item.oder_date, order_status, Convert.ToDouble(item.total));
                        }
                        else if (order__status=="4" && item.payment_id == payment__method) 
                        {
                            stt++;
                            table.Rows.Add(stt, item.order_id, item.OrderAddress.order_address_username,"'" + item.OrderAddress.order_address_phonenumber,item.OrderAddress.order_address_content, item.Payment.payment_name, payment_transaction, item.oder_date, order_status, Convert.ToDouble(item.total));
                        }
                        else if (payment__method == 4 && item.status == order__status)
                        {
                            stt++;
                            table.Rows.Add(stt, item.order_id, item.OrderAddress.order_address_username, "'" + item.OrderAddress.order_address_phonenumber, item.OrderAddress.order_address_content, item.Payment.payment_name, payment_transaction, item.oder_date, order_status, Convert.ToDouble(item.total));
                        }
                        else if(item.status == order__status && item.payment_id == payment__method)
                        {
                            stt++;
                            table.Rows.Add(stt, item.order_id, item.OrderAddress.order_address_username,"'" + item.OrderAddress.order_address_phonenumber, item.OrderAddress.order_address_content, item.Payment.payment_name, payment_transaction,  item.oder_date, order_status, Convert.ToDouble(item.total));
                        }
                    }
                }
                DataRow row = table.NewRow();
                row["Trạng thái đơn hàng"] = "Tổng doanh thu:";
                row["Total"] = table.Compute("Sum(Total)", "Total > 0");
                table.Rows.Add(row);
                worksheet.ImportDataTable(table, true, 1, 1);
                worksheet.UsedRange.AutofitColumns();
                string file = Convert.ToString((new Random()).Next(1000));
                //Save the workbook to disk in xlsx format
                workbook.SaveAs("BFD_Report_"+ picker1.ToString("dd-MM-yyyy") + "_" + picker2.ToString("dd-MM-yyyy") + "_" + file + ".xlsx", ExcelSaveType.SaveAsXLS, HttpContext.ApplicationInstance.Response, ExcelDownloadType.Open);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}