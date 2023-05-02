using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.PaymentLibrary;
using log4net;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class PaymentMethodsController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        readonly DbContext db = new DbContext();
        //thanh toán với paypal
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal
                //Payer Id will be returned when payment proceeds or click to pay
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/PaymentMethods/PaymentWithPayPal?";

                    //here we are generating guid for storing the paymentID received in session
                    //which will be used in the payment execution
                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment
                    var createdPayment = this.PayPalCreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return RedirectToAction("PayPalFailure");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("PayPalFailure");
            }
            //on successful payment, show success page to user.
            return RedirectToAction("PayPalSuccess");
        }
        //trả về kết quả khi thanh toán thành công
        public ActionResult PayPalSuccess(string DeliveryId, string SubjectMail, string ButtonConfirmlink, string ButtonConfirm, string OrderPayment, string OrderDelivery, string SubOrderTotal, string Discount_Price, string ProductOrder, string OrderId, string OrderStatus, string OrderTotal, string UserEmail, string UserName, string UserPhoneNumber, string UserAddress)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            int orderId = Convert.ToInt32(TempData["OrderId"]);
            ViewBag.OrderID = orderId.ToString();//Mã đơn hàng
            var order = db.Orders.Where(m => m.order_id == orderId).FirstOrDefault();
            var orderdetail = db.Order_Detail.Where(m => m.order_id == order.order_id).ToList();
            var discount_code_order = db.Order_Detail.FirstOrDefault(m => m.order_id == orderId);
            var discount = db.Discounts.FirstOrDefault(m => m.discounts_code == discount_code_order.discount_code);
            ViewBag.PaymentID = order.Payment.payment_name;//Mã giao dịch tại VNPAY 
            ViewBag.Total = Math.Round(Convert.ToDouble(order.total/Convert.ToDouble(order.Payment.Exchange_rates)),2).ToString("#,###"+" USD");//Số tiền thanh toán (VND)
            ViewBag.Message = "Thanh toán thành công";
            //xoá discount cũ
            Session.Remove("Discount");
            Session.Remove("Discountcode");
            double pricesum = 0;
            foreach (var item in orderdetail)
            {
                pricesum += (item.price * item.quantity);
                //khởi tạo danh sách sản phẩm: tên,hình ảnh,giá, số lượng 
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
                            "<span class='price' style='color: #005f8f; font-size: 14px; font-weight: 500;'>" + item.price.ToString("#,###", cul.NumberFormat)+"₫" + "</span>" +
                        "</td>" +
                    "</tr>";
            }
            //tính giảm giá sản phẩm
            //tính giảm giá sản phẩm
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
            OrderId = orderId.ToString();
            SubOrderTotal = pricesum.ToString("#,###", cul.NumberFormat)+"₫";
            UserEmail = order.OrderAddress.order_adress_email;
            UserName = order.OrderAddress.order_address_username;
            UserPhoneNumber = order.OrderAddress.order_address_phonenumber;
            UserAddress = order.OrderAddress.order_address_content;
            OrderDelivery = order.Delivery.delivery_name;
            DeliveryId = OrderId;
            //trang thái '1' chưa thanh toán '2' đã thanh toán
            order.payment_transaction = "2";
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            SubjectMail = "Đơn hàng #" + orderId + " đã thanh toán thành công";
            //thanh toán xong thì tổng đơn hàng bằng 0
            OrderStatus = "<span style='color:#28a745'>Đã thanh toán</span>";
            OrderTotal = "0₫";
            OrderPayment = order.Payment.payment_name;
            ButtonConfirm = "Quản lý đơn hàng";
            ButtonConfirmlink = "/account/order_detail/"+orderId;
            DeliveryId = orderId.ToString();
            SendVerificationLinkEmail(DeliveryId,SubjectMail, ButtonConfirmlink, ButtonConfirm, OrderPayment, OrderDelivery, SubOrderTotal, Discount_Price, UserEmail, UserName, UserPhoneNumber, UserAddress, OrderId, OrderStatus, OrderTotal, ProductOrder);
            Notification.set_flash("Thanh toán thành công", "success");
            BannerGlobal();
            return View();
        }
        //trả về kết quả khi thanh toán thất bại
        public ActionResult PayPalFailure(string SubjectMail, string DeliveryId, string ButtonConfirmlink, string ButtonConfirm, string OrderPayment, string OrderDelivery, string SubOrderTotal, string Discount_Price, string ProductOrder, string OrderId, string OrderStatus, string OrderTotal, string UserEmail, string UserName, string UserPhoneNumber, string UserAddress)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            ViewBag.Error = "Đã có lỗi khi thanh toán";
            int orderId = Convert.ToInt32(TempData["OrderId"]);
            ViewBag.OrderID = orderId.ToString();//Mã đơn hàng
            var order = db.Orders.Where(m => m.order_id == orderId).FirstOrDefault();
            var orderdetail = db.Order_Detail.Where(m => m.order_id == order.order_id).ToList();
            var discount_code_order = db.Order_Detail.FirstOrDefault(m => m.order_id == orderId);
            var discount = db.Discounts.FirstOrDefault(m => m.discounts_code == discount_code_order.discount_code);
            //xoá discount cũ
            Session.Remove("Discount");
            Session.Remove("Discountcode");
            double pricesum = 0;
            foreach (var item in orderdetail)
            {
                pricesum += (item.price * item.quantity);
                //khởi tạo danh sách sản phẩm: tên,hình ảnh,giá, số lượng 
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
                            "<span class='price' style='color: #005f8f; font-size: 14px; font-weight: 500;'>" + item.price.ToString("#,###", cul.NumberFormat)+"₫" + "</span>" +
                        "</td>" +
                    "</tr>";
            }
            //tính giảm giá sản phẩm
            //tính giảm giá sản phẩm
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
            OrderId = orderId.ToString();
            SubOrderTotal = pricesum.ToString("#,0", cul.NumberFormat) + "₫";
            UserEmail = order.OrderAddress.order_adress_email;
            UserName = order.OrderAddress.order_address_username;
            UserPhoneNumber = order.OrderAddress.order_address_phonenumber;
            UserAddress = order.OrderAddress.order_address_content;
            OrderDelivery = order.Delivery.delivery_name;
            DeliveryId = OrderId;
            //trang thái '1' chưa thanh toán '2' đã thanh toán
            SubjectMail = "Đơn hàng #" + orderId + " chưa hoàn tất thanh toán";
            //thanh toán xong thì tổng đơn hàng bằng 0
            OrderStatus = "<span style='color:#41464b'>Chưa thanh toán </span>";
            OrderTotal = order.total.ToString("#,0", cul.NumberFormat) + "₫";
            OrderPayment = order.Payment.payment_name;
            ButtonConfirm = "Thanh toán lại";
            ButtonConfirmlink = "/PaymentMethods/PaymentWithPaypal";
            DeliveryId = orderId.ToString();
            TempData["OrderId"] = order.order_id;
            SendVerificationLinkEmail(DeliveryId,SubjectMail, ButtonConfirmlink, ButtonConfirm, OrderPayment, OrderDelivery, SubOrderTotal, Discount_Price, UserEmail, UserName, UserPhoneNumber, UserAddress, OrderId, OrderStatus, OrderTotal, ProductOrder);
            Notification.set_flash("Thanh toán thất bại", "danger");
            BannerGlobal();
            return View();
        }
        //gọi api
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        //khởi tạo thanh toán
        private Payment PayPalCreatePayment(APIContext apiContext, string redirectUrl)
        {
            int order_id = Convert.ToInt32(TempData["OrderId"]);
            var order = db.Orders.FirstOrDefault(m => m.order_id == order_id);
            var orderitemlist = db.Order_Detail.Where(m => m.order_id == order_id).ToList();
            string shipping_order = Math.Round(Convert.ToDouble(30000 / Convert.ToDouble(order.Payment.Exchange_rates)), 2).ToString();
            //create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };
            //Adding Item Details like name, currency, price etc
            foreach (var item in orderitemlist)
            {
                itemList.items.Add(new Item()
                {
                    name = item.Product.product_name.ToString(),
                    currency = "USD",
                    price = Math.Round((item.price / Convert.ToDouble(order.Payment.Exchange_rates)), 2).ToString(),
                    quantity = item.quantity.ToString(),
                    sku = item.product_id.ToString()
                });
            }
            var payer = new Payer() { payment_method = "paypal" };
            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details
            var details = new Details()
            {
                tax = "0",
                shipping = shipping_order,
                subtotal = Math.Round(((order.total / Convert.ToDouble(order.Payment.Exchange_rates)) - Convert.ToDouble(shipping_order)),2).ToString(),
            };

            //Final amount with details
            var amount = new Amount()
            {
                currency = "USD",
                total = Math.Round((order.total / Convert.ToDouble(order.Payment.Exchange_rates)), 2).ToString(), // Total must be equal to sum of tax, shipping and subtotal.
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction
            transactionList.Add(new Transaction()
            {
                description = "BFDComputer - Thanh toan don hang " + order.order_id,
                invoice_number = DateTime.Now.Ticks.ToString(), //Generate an Invoice No
                amount = amount,
                //item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }
        //Thanh toán VnPay
        public ActionResult vnpay(object sender, EventArgs e)
        {
            Models.API_Key aPI_Key = db.API_Keys.FirstOrDefault(m => m.id == 5 && m.active==true);
            //Get Config Info
            string vnp_Returnurl = Request.Url.Scheme + "://" + Request.Url.Authority+'/'+ aPI_Key.Return_Url; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = aPI_Key.client_id; //Ma website
            string vnp_HashSecret = aPI_Key.client_secret; //Chuoi bi mat
            //Get payment input
            //OrderInfo order = new OrderInfo();
            ////Save order to db
            //order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            //order.Amount = 1000000; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            //order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
            //order.OrderDesc = "Thanh toán đơn hàng";
            //order.CreatedDate = DateTime.Now;
            ////Build URL for VNPAY
            //Get payment input
            OrderInfo order = new OrderInfo();
            //Save order to db
            order.OrderId = Convert.ToInt64(TempData["OrderId"]); // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            order.Amount = Convert.ToDouble(TempData["Total"]);  // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
            order.CreatedDate = DateTime.Now;
            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_BankCode", "");
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "BFDComputer - Thanh toan don hang: #" + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: otherf
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
            //Add Params of 2.1.0 Version
            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return Redirect(paymentUrl);//chuyển đến trang thanh toán của vnpay
        }
        //Trả kết quả thanh toán vnpay
        public ActionResult VnPayReturn(object sender, EventArgs e, string DeliveryId, string SubjectMail, string ButtonConfirmlink, string ButtonConfirm, string OrderPayment, string OrderDelivery, string SubOrderTotal, string Discount_Price, string ProductOrder, string OrderId, string OrderStatus, string OrderTotal, string UserEmail, string UserName, string UserPhoneNumber, string UserAddress)
        {
            log.InfoFormat("Begin VNPAY Return, URL={0}", Request.RawUrl);
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            if (Request.QueryString.Count > 0)
            {
                Models.API_Key aPI_Key = db.API_Keys.FirstOrDefault(m => m.id == 5 && m.active == true);
                string vnp_HashSecret = aPI_Key.client_secret; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();
                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: HmacSHA512 cua du lieu tra ve              
                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];
                String CardType = Request.QueryString["vnp_CardType"];
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                var order = db.Orders.Where(m => m.order_id == orderId).FirstOrDefault();
                var orderdetail = db.Order_Detail.Where(m => m.order_id == order.order_id).ToList();
                var discount_code_order = db.Order_Detail.FirstOrDefault(m => m.order_id == orderId);
                var discount = db.Discounts.FirstOrDefault(m => m.discounts_code == discount_code_order.discount_code);
                //xoá discount cũ
                Session.Remove("Discount");
                Session.Remove("Discountcode");
                double pricesum = 0;
                foreach (var item in orderdetail)
                {
                    pricesum += (item.price * item.quantity);
                    //khởi tạo danh sách sản phẩm: tên,hình ảnh,giá, số lượng 
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
                                "<span class='price' style='color: #005f8f; font-size: 14px; font-weight: 500;'>" + item.price.ToString("#,###", cul.NumberFormat)+ "₫" + "</span>" +
                            "</td>" +
                        "</tr>";
                }
                //tính giảm giá sản phẩm
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
                            Discount_Price = "<span style='color: #28a745'>"+ discount.discount_max.ToString("-#,0", cul.NumberFormat) + "₫" + "</span>";
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
                OrderId = orderId.ToString();
                SubOrderTotal = pricesum.ToString("#,0", cul.NumberFormat) + "₫";
                UserEmail = order.OrderAddress.order_adress_email;
                UserName = order.OrderAddress.order_address_username;
                UserPhoneNumber = order.OrderAddress.order_address_phonenumber;
                UserAddress = order.OrderAddress.order_address_content;
                OrderDelivery = order.Delivery.delivery_name;
                DeliveryId = OrderId;
                if (checkSignature)//kiểm tra chữ ký
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        //Thanh toan thanh cong
                        Notification.set_flash("Thanh toán thành công", "success");
                        ViewBag.Message = "Thanh toán thành công";
                        log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                        //trang thái '0' chưa thanh toán '1' đã thanh toán
                        order.payment_transaction = "2";
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                        SubjectMail = "Đơn hàng #" + orderId + " đã thanh toán thành công";
                        //thanh toán xong thì tổng đơn hàng bằng 0
                        OrderStatus = "<span style='color:#28a745'>Đã thanh toán</span>";
                        OrderTotal = "0đ";
                        OrderPayment = order.Payment.payment_name;
                        ButtonConfirm = "Quản lý đơn hàng";
                        ButtonConfirmlink = "/account/order_detail/" + orderId;
                        DeliveryId = order.order_id.ToString();
                        SendVerificationLinkEmail(DeliveryId,SubjectMail, ButtonConfirmlink, ButtonConfirm, OrderPayment, OrderDelivery, SubOrderTotal, Discount_Price, UserEmail, UserName, UserPhoneNumber, UserAddress, OrderId, OrderStatus, OrderTotal, ProductOrder);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.Error = "Thanh toán không thành công";
                        log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                        SubjectMail = "Đơn hàng #" + orderId + " chưa hoàn tất thanh toán";
                        OrderStatus = "<span style='color:#41464b'>Chưa thanh toán </span>";
                        OrderTotal = order.total.ToString("#,0", cul.NumberFormat) + "₫";
                        OrderPayment = order.Payment.payment_name + "<span style='color:#ffc107;'>(Chưa thanh toán) </span>";
                        ButtonConfirm = "Thanh toán lại";
                        ButtonConfirmlink = "/PaymentMethods/vnpay";
                        DeliveryId = order.order_id.ToString();
                        SendVerificationLinkEmail(DeliveryId,SubjectMail, ButtonConfirmlink, ButtonConfirm, OrderPayment, OrderDelivery, SubOrderTotal, Discount_Price, UserEmail, UserName, UserPhoneNumber, UserAddress, OrderId, OrderStatus, OrderTotal, ProductOrder);
                    }
                    ViewBag.OrderID = orderId.ToString();//Mã đơn hàng
                    ViewBag.VnPayID = vnpayTranId.ToString();//Mã giao dịch tại VNPAY 
                    ViewBag.Total = vnp_Amount.ToString("#,0", cul.NumberFormat) + "₫";//Số tiền thanh toán (VND)
                    ViewBag.bankCode = bankCode;//Ngân hàng thanh toán
                    ViewBag.Cardtype = CardType;//Loại tài khoản/thẻ khách hàng sử dụng:ATM,QRCODE
                }
                else
                {
                    log.InfoFormat("Invalid signature, InputData={0}", Request.RawUrl);
                    Notification.set_flash("Chữ ký không hợp lệ", "danger");
                    ViewBag.Error = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }
            BannerGlobal();
            return View();
        }
        //gửi email khi thanh toán thành công hoặc huỷ thanh toán
        [NonAction]
        public void SendVerificationLinkEmail(string DeliveryId, string SubjectMail, string ButtonConfirmlink, string ButtonConfirm, string OrderPayment, string OrderDelivery, string SubOrderTotal, string Discount_Price, string UserEmail, string UserName, string UserPhoneNumber, string UserAddress, string OrderId, string OrderStatus, string OrderTotal, string ProductOrder)
        {
            ///để dùng google email gửi email reset cho người khác bạn cần phải vô đây "https://www.google.com/settings/security/lesssecureapps" Cho phép ứng dụng kém an toàn: Bật
            var fromEmail = new MailAddress(AccountEmail.UserEmailSupport, AccountEmail.Name); // "username email-vd: vn123@gmail.com" ,"tên hiển thị mail khi gửi"
            var toEmail = new MailAddress(UserEmail);
            //nhập password của bạn
            var fromEmailPassword = AccountEmail.Password;
            string subject;
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "MailOrders" + ".cshtml"); //dùng body mail html , file template nằm trong thư mục "EmailTemplate/Text.cshtml"
            subject = SubjectMail;
            body = body.Replace("{{OrderId}}", OrderId);
            body = body.Replace("{{DeliveryId}}", DeliveryId);
            body = body.Replace("{{BodyContent}}", "Yêu cầu đặt hàng cho đơn hàng <span style='color: rgb(1, 181, 187); font-weight: 500;'>#" + OrderId + "</span> của bạn đã được tiếp nhận và đang chờ Nhà bán hàng xử lý, với hình thức thanh toán là <span>" + OrderPayment + "</span> Chúng tôi sẽ tiếp tục cập nhật với bạn về trạng thái tiếp theo của đơn hàng.");
            body = body.Replace("{{OrderStatus}}", OrderStatus);
            body = body.Replace("{{ButtonConfirm}}", ButtonConfirm);
            body = body.Replace("{{ButtonConfirmLink}}", Request.Url.Scheme + "://" + Request.Url.Authority + ButtonConfirmlink);
            body = body.Replace("{{UserEmail}}", UserEmail);
            body = body.Replace("{{DiscountPrice}}", Discount_Price);
            body = body.Replace("{{UserName}}", UserName);
            body = body.Replace("{{UserAddress}}", UserAddress);
            body = body.Replace("{{UserPhoneNumber}}", UserPhoneNumber);
            body = body.Replace("{{SubOrderTotal}}", SubOrderTotal);
            body = body.Replace("{{OrderTotal}}", OrderTotal);
            body = body.Replace("{{ProductOrder}}", ProductOrder);
            body = body.Replace("{{Payment}}", OrderPayment);
            body = body.Replace("{{Delivery}}", OrderDelivery);
            body = body.Replace("{{DeliveryId}}", DeliveryId);
            var smtp = new SmtpClient
            {
                Host = AccountEmail.Host, //tên mấy chủ nếu bạn dùng gmail thì đổi  "Host = "smtp.gmail.com"
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
        //đổi phương thức thanh toán
        public ActionResult ChangePayments(OrderDTOs model)
        {
            int order_id = Convert.ToInt32(TempData["OrderID"]);
            //thanh toán trong vòng 1 ngày
            var order = db.Orders.FirstOrDefault(m => m.order_id == order_id && m.status == "1");
            TempData["OrderID"] = order.order_id;
            DateTime expired_date = order.oder_date.AddDays(1);
            try
            {
                if (expired_date > DateTime.Now )
                {
                    order.payment_id = model.payment_id;
                    //payment_id tương ứng với id của từng payment trong bảng payment
                    switch (model.payment_id)
                    {
                        case 2:
                            db.SaveChanges();
                            return RedirectToAction("vnpay", "PaymentMethods");
                        case 6:
                            db.SaveChanges();
                            return RedirectToAction("PaymentWithPaypal", "PaymentMethods");
                        default:
                            order.payment_transaction = "2";
                            Notification.set_flash("Chuyển phương thức thanh toán thành công", "success");
                            db.SaveChanges();
                            return RedirectToAction("TrackingOrderDetail", "Account", new { id = order_id });
                    }
                }
                else
                {
                    Notification.set_flash("Đơn hàng vượt quá thời hạn thanh toán", "danger");
                    return RedirectToAction("TrackingOrderDetail", "Account", new { id = order_id });
                }
            }
            catch
            {
                Notification.set_flash("Lỗi, thay đổi phương thức thanh toán thất bại", "danger");
                return RedirectToAction("TrackingOrderDetail", "Account", new { id = order_id });
            }
        }
        //để hiển thị banner top trên toàn bộ layout phải thêm cái này
        public void BannerGlobal()
        {
            ViewBag.BannerTopHorizontal = db.Banners.OrderByDescending(m => Guid.NewGuid()).Where(m => m.banner_start < DateTime.Now && m.banner_end > DateTime.Now && m.status == "1" && m.banner_type == 3).Take(8).ToList();
        }
    }
}