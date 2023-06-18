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

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class WarrantyAdminController : Controller
    {
        private readonly DbContext db = new DbContext();
        // GET: Admin/WarrantyAdmin
        public ActionResult WarrantyIndex(int? page, int? size)
        {
            if (User.Identity.IsAuthenticated)
            {
                var pageSize = size ?? 10;
                var pageNumber = page ?? 1;

                ViewBag.countTrash = db.Warranty.Count(w => w.status == 0); //  đếm tổng item có trong thùng rác
                //var list = db.Warranty.Where(w => w.status != 0).ToList();

                var list = from a in db.Warranty
                           join b in db.Orders on a.order_id equals b.order_id
                           //group a by new { a.warranty_id, b } into g
                           where a.status != 0
                           orderby a.warranty_id descending
                           select new WarrantyDTOs
                           {
                               warranty_id = a.warranty_id,
                               order_id = a.warranty_id,
                               broken_state = a.broken_state,
                               status = (int)a.status,
                               warranty_date = a.warranty_date,
                               receive_date = a.receive_date,
                               update_at = a.update_at,
                               update_by = a.update_by,
                               Name = b.Account.Name,
                               Email = b.Account.Email,
                               Phone = b.Account.Phone,
                           };

                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
            
        }
        public ActionResult WarrantyTrash(int? page, int? size)
        {
            if (User.Identity.IsAuthenticated)
            {
                var pageSize = size ?? 10;
                var pageNumber = page ?? 1;
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    ViewBag.countTrash = db.Warranty.Count(w => w.status == 0); //  đếm tổng item có trong thùng rác
                                                                                //var list = db.Warranty.Where(w => w.status != 0).ToList();

                    var list = from a in db.Warranty
                               join b in db.Orders on a.order_id equals b.order_id
                               //group a by new { a.warranty_id, b } into g
                               where a.status == 0
                               orderby a.warranty_id descending
                               select new WarrantyDTOs
                               {
                                   warranty_id = a.warranty_id,
                                   order_id = a.warranty_id,
                                   broken_state = a.broken_state,
                                   status = (int)a.status,
                                   warranty_date = a.warranty_date,
                                   receive_date = a.receive_date,
                                   update_at = a.update_at,
                                   update_by = a.update_by,
                                   note = a.note,
                                   Name = b.Account.Name,
                                   Email = b.Account.Email,
                                   Phone = b.Account.Phone,
                               };

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
        // GET: Admin/WarrantyAdmin/Details/5
        public ActionResult WarrantyDetail(int id)
        {
            //ViewBag.WarrantPd = db.Products.ToList();
            var warrantyDetail = (from a in db.Warranty
                       join b in db.Orders on a.order_id equals b.order_id
                       join p in db.Products on a.product_id equals p.product_id
                       //group a by new { a.warranty_id, b } into g
                       where a.warranty_id == id
                       select new WarrantyDTOs
                       {
                           warranty_id = a.warranty_id,
                           order_id = a.warranty_id,
                           product_id = (int)a.product_id,
                           broken_state = a.broken_state,
                           fixed_state = a.fixed_state,
                           product_name = p.product_name,
                           product_image = p.image,
                           status = (int)a.status,
                           warranty_time = p.warranty_time,
                           warranty_date = a.warranty_date,
                           receive_date = a.receive_date,
                           update_at = a.update_at,
                           update_by = a.update_by,
                           addition_parts = a.addition_parts,
                           note = a.note,
                           Name = b.Account.Name,
                           Email = b.Account.Email,
                           Phone = b.Account.Phone,
                       }).FirstOrDefault();

            return View(warrantyDetail);
        }
        // POST: Admin/WarrantyAdmin/Edit/5
        [HttpPost]
        public ActionResult WarrantyDetail(WarrantyDTOs data)
        {
            try
            {
                // TODO: Add update logic here
                var warrantyEdit = db.Warranty.SingleOrDefault(w => w.warranty_id == data.warranty_id);
                if (warrantyEdit != null)
                {
                    warrantyEdit.receive_date = data.receive_date;
                    warrantyEdit.fixed_state = data.fixed_state;
                    warrantyEdit.addition_parts = data.addition_parts;
                    warrantyEdit.note = data.note;
                    
                    db.SaveChanges();
                    Notification.set_flash("Cập nhật thông tin bảo hành thành công", "success");
                    return RedirectToAction("WarrantyIndex");
                }


                return RedirectToAction("WarrantyIndex");
            }
            catch
            {
                Notification.set_flash("Lỗi cập nhật bảo hành", "danger");
                return RedirectToAction("WarrantyIndex");
            }
        }
        //Chuyển trạng thái sang đang xử lý
        public ActionResult ChangeProcessing(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
                {
                    
                    var warranty = db.Warranty.SingleOrDefault(w => w.warranty_id == id);
                    if (warranty != null)
                    {
                        warranty.status = 2;
                        warranty.update_at = DateTime.Now;
                        warranty.update_by = User.Identity.GetName();
                        db.Entry(warranty).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    Notification.set_flash("Đã chuyển trạng thái bảo hành: " + "#" + id + " sang đang xử lý!", "success");
                    return RedirectToAction("WarrantyIndex");
                }
                else
                {
                    //nếu không có quyền thì sẽ back về trang chủ bảng điều khiển
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }

        //Chuyển trạng thái sang đã hoàn thành
        public ActionResult ChangeComplete(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
                {

                    var warranty = db.Warranty.SingleOrDefault(w => w.warranty_id == id);
                    if (warranty != null)
                    {
                        warranty.status = 3;
                        warranty.update_at = DateTime.Now;
                        warranty.update_by = User.Identity.GetName();
                        db.Entry(warranty).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    Notification.set_flash("Đã chuyển trạng thái bảo hành: " + "#" + id + " sang đã hoàn thành!", "success");
                    return RedirectToAction("WarrantyIndex");
                }
                else
                {
                    //nếu không có quyền thì sẽ back về trang chủ bảng điều khiển
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }

        

        // GET: Admin/WarrantyAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/WarrantyAdmin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
