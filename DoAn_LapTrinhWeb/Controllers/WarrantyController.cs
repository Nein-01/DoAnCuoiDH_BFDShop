using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using System.IO;
using System.Linq.Expressions;
using PagedList;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class WarrantyController : Controller
    {
        private readonly DbContext db = new DbContext();
        // GET: Warranty
        public ActionResult Index(int? page, int? size)
        {
            if (User.Identity.IsAuthenticated)
            {
                var pageSize = size ?? 3;
                var pageNumber = page ?? 1;
                var list = db.Warranty.ToList();
                ViewBag.itemOrder = db.Warranty.OrderByDescending(m => m.order_id);
                ViewBag.productOrder = db.Products;
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }

        }

        // GET: Warranty/Details/5
        public ActionResult Details(int id)
        {
            Warranty warrandetail = db.Warranty.SingleOrDefault(w => w.warranty_id == id);
            Product warrantPd = db.Products.SingleOrDefault(p => p.product_id == warrandetail.product_id);
            ViewBag.WarrantPd = warrantPd;
            return View(warrandetail);
        }

        // GET: Warranty/Create
        public ActionResult Create(int orderId, int productId)
        {
            Product getPr = db.Products.SingleOrDefault(p => p.product_id == productId);

            ViewBag.odId = orderId;
            ViewBag.pdId = productId;
            ViewBag.pdShow = getPr;
            return View();
        }

        // POST: Warranty/Create
        [HttpPost]
        public ActionResult Create(Warranty warranty)
        {
            try
            {
                // TODO: Add insert logic here
                warranty.broken_state = warranty.broken_state;
                warranty.warranty_date = warranty.warranty_date;
                warranty.receive_date = warranty.warranty_date;
                warranty.status = 1;
                warranty.order_id = warranty.order_id;
                warranty.product_id = warranty.product_id;
                warranty.create_at = DateTime.Now;
                warranty.create_by = User.Identity.GetName();
                warranty.update_at = DateTime.Now;
                warranty.update_by = User.Identity.GetName();
                db.Warranty.Add(warranty);
                db.SaveChanges();
                Notification.set_flash("Gửi yêu cầu bảo hành thành công", "success");

                return RedirectToAction("Index");
            }
            catch
            {
                Notification.set_flash("Gửi yêu cầu bảo hành thất bại", "danger");
                return View();
            }
        }

        //Huỷ bảo hành
        [HttpPost]
        public ActionResult CancelWarranty(Warranty model)
        {
            bool result = false;
            var warranty = db.Warranty.FirstOrDefault(m => m.warranty_id == model.warranty_id);
            try
            {
                if (warranty != null && warranty.status == 1)
                {
                    warranty.status = 0;
                    warranty.note = model.note;
                    warranty.update_by = User.Identity.GetName();
                    warranty.update_at = DateTime.Now;
                    db.SaveChanges();
                    result = true;
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Warranty/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Warranty/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Warranty/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Warranty/Delete/5
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
