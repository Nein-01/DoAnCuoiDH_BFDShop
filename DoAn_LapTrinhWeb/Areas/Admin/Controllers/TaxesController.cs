using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn_LapTrinhWeb;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Models;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class TaxesController : BaseController
    {
        private DbContext db = new DbContext();
        public ActionResult TaxesIndex(string search,string show, int ? size,int ? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 9);
                    var pageNumber = (page ?? 1);
                    var list = from a in db.Taxes
                               orderby a.taxes_id descending
                               select a;
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))//tìm kiếm tất cả
                            list = (IOrderedQueryable<Taxes>)list.Where(s => s.taxes_id.ToString().Contains(search) || s.taxes_name.Contains(search));
                        else if (show.Equals("2"))//theo id
                            list = (IOrderedQueryable<Taxes>)list.Where(s => s.taxes_id.ToString().Contains(search));
                        else if (show.Equals("3"))//theo tên thể loại
                            list = (IOrderedQueryable<Taxes>)list.Where(s => s.taxes_name.Contains(search));
                        return View("TaxesIndex", list.ToPagedList(pageNumber, 50));
                    }
                    return View(list.ToPagedList(pageNumber, pageSize));
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
        //Thêm mới VAT
        [HttpPost]
        public JsonResult TaxesCreate(string name, int value, Taxes taxes)
        {
            bool result;
            try
            {
                if (User.Identity.Permiss_Create() == true)
                {
                    taxes.taxes_name = name;
                    taxes.taxes_value = value;
                    taxes.update_at = DateTime.Now;
                    taxes.create_at = DateTime.Now;
                    db.Taxes.Add(taxes);
                    db.SaveChanges();
                    result = true;
                    Notification.set_flash("Thêm mới thành công", "success");
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //sửa VAT
        public JsonResult TaxesEdit(string name, int value, int id)
        {
            Taxes taxes = db.Taxes.Find(id);
            bool result;
            try
            {
                if (User.Identity.Permiss_Modify() == true)
                {
                    taxes.taxes_name = name;
                    taxes.taxes_value = value;
                    taxes.update_at = DateTime.Now;
                    db.SaveChanges();
                    result = true;
                    Notification.set_flash("Cập nhật id "+id+" thành công", "success");
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //xoá VAT
        public JsonResult TaxesDelete(int id)
        {
            bool result;
            Taxes taxes = db.Taxes.Find(id);
            if (taxes.Products.Count > 0)
            {
                result = false;
            }
            else
            {
                if (User.Identity.Permiss_Delete() == true)
                {
                    db.Taxes.Remove(taxes);
                    db.SaveChanges();
                    result = true;
                    Notification.set_flash("Xóa id " + id + " thành công", "success");
                }
                else
                {
                    result = false;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
