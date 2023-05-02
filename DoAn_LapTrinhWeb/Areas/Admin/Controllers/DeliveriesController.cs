using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Model;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class DeliveriesController : BaseController
    {
        private readonly DbContext _db = new DbContext();
        //View list đơn vị vận chuyển
        public ActionResult DeliveryIndex(string search,string show, int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 10);
                    var pageNumber = (page ?? 1);

                    ViewBag.countTrash = _db.Deliveries.Count(a => a.status == "2"); //  đếm tổng sp có trong thùng rác

                    var list = from a in _db.Deliveries
                               where (a.status == "1" || a.status == "0")
                               orderby a.delivery_id descending
                               select a;
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))//tìm kiếm tất cả
                            list = (IOrderedQueryable<Delivery>)list.Where(s => s.delivery_id.ToString().Contains(search) || s.delivery_name.Contains(search)
                            || s.create_by.Contains(search));
                        else if (show.Equals("2"))//theo id
                            list = (IOrderedQueryable<Delivery>)list.Where(s => s.delivery_id.ToString().Contains(search));
                        else if (show.Equals("3"))//theo tên 
                            list = (IOrderedQueryable<Delivery>)list.Where(s => s.delivery_name.ToString().Contains(search));
                        return View("DeliveryIndex", list.ToPagedList(pageNumber, 50));
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
        //View list trash đơn vị vận chuyển
        public ActionResult DeliveryTrash(string search, string show, int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 10);
                    var pageNumber = (page ?? 1);
                    var list = from a in _db.Deliveries
                               where a.status == "2"
                               orderby a.update_at descending
                               select a;
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))//tìm kiếm tất cả
                            list = (IOrderedQueryable<Delivery>)list.Where(s => s.delivery_id.ToString().Contains(search) || s.delivery_name.Contains(search)
                            || s.create_by.Contains(search));
                        else if (show.Equals("2"))//theo id
                            list = (IOrderedQueryable<Delivery>)list.Where(s => s.delivery_id.ToString().Contains(search));
                        else if (show.Equals("3"))//theo tên 
                            list = (IOrderedQueryable<Delivery>)list.Where(s => s.delivery_name.ToString().Contains(search));
                        return View("DeliveryTrash", list.ToPagedList(pageNumber, 50));
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
        //Thông tin chi tiết đơn vị vận chuyển
        public ActionResult DeliveryDetails(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Read() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var delivery = _db.Deliveries.SingleOrDefault(a => a.delivery_id == id);
                    if (delivery != null && id != null) return View(delivery);
                    Notification.set_flash("Không tồn tại: " + delivery.delivery_name + "", "warning");
                    return RedirectToAction("DeliveryIndex");
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
        //View thêm đơn vị vận chuyển
        public ActionResult DeliveryCreate()
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
                    return RedirectToAction("DeliveryIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Code xử lý thêm đơn vị vận chuyển
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DeliveryCreate(Delivery delivery)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    delivery.status = delivery.status;
                    delivery.create_at = DateTime.Now;
                    delivery.create_by = User.Identity.GetEmail();
                    delivery.update_at = DateTime.Now;
                    delivery.update_by = User.Identity.GetEmail();
                    _db.Deliveries.Add(delivery);
                    _db.SaveChanges();
                    Notification.set_flash("Đã thêm mới đơn vị vận chuyển: " + delivery.delivery_name + "", "success");
                    return RedirectToAction("DeliveryIndex");
                }
                catch
                {
                    Notification.set_flash("Lỗi", "danger");
                }
                return View(delivery);
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //View chỉnh sửa thông tin đơn vị vận chuyển
        public ActionResult DeliveryEdit(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var delivery = _db.Deliveries.SingleOrDefault(a => a.delivery_id == id);
                if ((User.Identity.Permiss_Update() == true) && (delivery.status == "1" || delivery.status == "0"))
                {
                    if (delivery != null && id != null)
                    {
                        return View(delivery);
                    }
                    else
                    {
                        Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                        return RedirectToAction("DeliveryIndex");
                    }
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("DeliveryIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Code xử lý chỉnh sửa thông tin đơn vị vận chuyển
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult DeliveryEdit(Delivery delivery)
        {
            try
            {
                delivery.update_at = DateTime.Now;
                delivery.update_by = User.Identity.GetEmail();
                delivery.status = delivery.status;
                _db.Entry(delivery).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Đã cập nhật lại thông tin đơn vị vận chuyển: "+ delivery.delivery_name + "", "success");
                return RedirectToAction("DeliveryIndex");
            }
            catch
            {
                Notification.set_flash("404!", "warning");
            }

            return View(delivery);
        }
        //Vô hiệu hóa đơn vị vận chuyển
        public ActionResult DelTrash(int? id) //bỏ sp vào thùng rác
        {
            if (User.Identity.Permiss_Update() == true)
            {
                var delivery = _db.Deliveries.SingleOrDefault(a => a.delivery_id == id);
                if (delivery == null || id == null)
                {
                    Notification.set_flash("Không tồn tại: " + delivery.delivery_name + "", "warning");
                    return RedirectToAction("DeliveryIndex");
                }
                delivery.status = "2";
                delivery.update_at = DateTime.Now;
                delivery.update_by = User.Identity.GetEmail();
                _db.Entry(delivery).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Đã chuyển đơn vị vận chuyển: " + delivery.delivery_name + " vào thùng rác", "success");
                return RedirectToAction("DeliveryIndex");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("DeliveryIndex");
            }
        }
        //Khôi phục đơn vị vận chuyển
        public ActionResult Undo(int? id) // khôi phục từ thùng rác
        {
            if (User.Identity.Permiss_Update() == true)
            {
                var delivery = _db.Deliveries.SingleOrDefault(a => a.delivery_id == id);
                if (delivery == null || id == null)
                {
                    Notification.set_flash("Không tồn tại! (ID = " + id + ")", "warning");
                    return RedirectToAction("DeliveryIndex");
                }
                delivery.status = "1";
                delivery.update_at = DateTime.Now;
                delivery.update_by = User.Identity.GetEmail();
                _db.Entry(delivery).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Khôi phục thành công: " + delivery.delivery_name + "", "success");
                return RedirectToAction("DeliveryTrash");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("DeliveryTrash");
            }
        }
        //Xóa đơn vị vận chuyển
        public ActionResult DeliveryDelete(int? id,string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("DeliveryDelete", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                if (User.Identity.Permiss_Delete() == true)
                {
                    var deli = _db.Deliveries.SingleOrDefault(a => a.delivery_id == id);
                    if (deli == null || id == null)
                    {
                        Notification.set_flash("Không tồn tại! đơn vị vận chuyển " + deli.delivery_name + "", "warning");
                        return RedirectToAction("DeliveryTrash");
                    }
                    return View(deli);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("DeliveryTrash");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Xác nhận xóa vĩnh viển đơn vị vận chuyển
        [HttpPost]
        [ActionName("DeliveryDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            var deli = _db.Deliveries.SingleOrDefault(a => a.delivery_id == id);
            _db.Deliveries.Remove(deli);
            _db.SaveChanges();
            Notification.set_flash("Đã xoá vĩnh viễn đơn vị vận chuyển: " + deli.delivery_name + "", "success");
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("DeliveryTrash");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}