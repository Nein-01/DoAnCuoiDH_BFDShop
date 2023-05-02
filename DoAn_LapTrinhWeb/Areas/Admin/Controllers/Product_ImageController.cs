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
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Models;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class Product_ImageController : BaseController
    {
        private readonly DbContext db = new DbContext();
        //View list ảnh sản phẩm
        public ActionResult ProductImgIndex(int? page,int? size,string search,string show,string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = size ?? 10;
                    var pageNumber = page ?? 1;
                    var list = from a in db.Products
                               join c in db.Genres on a.genre_id equals c.genre_id
                               join i in db.Product_Images on a.product_id equals i.product_id
                               join e in db.Discounts on a.disscount_id equals e.disscount_id
                               where (a.status == "1" || a.status == "0")
                               orderby i.update_at ascending // giảm dần
                               select new ProductDTOs
                               {
                                   product_img_status = i.status,
                                   Image = a.image,
                                   discount_name = e.discount_name,
                                   genre_name = c.genre_name,
                                   product_id = a.product_id,
                                   product_name = a.product_name,
                                   genre_id = c.genre_id,
                                   discount_id = e.disscount_id,
                                   discount_start = e.discount_start,
                                   discount_end = e.discount_end,
                                   discount_status = e.status,
                                   price = a.price,
                                   discount_price = e.discount_price,
                                   product_img_id = i.product_image_id
                               };
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_asc" : "date_desc";
                    ViewBag.ImgSortParm = "image";
                    ViewBag.OneImgSortParm = "one_image";
                    ViewBag.TwoImgSortParm = "two_images";
                    ViewBag.ThreeImgSortParm = "three_images";
                    ViewBag.FourImgSortParm = "four_images";
                    switch (sortOrder)
                    {
                        case "date_desc":
                            list = from a in db.Products
                                   join c in db.Genres on a.genre_id equals c.genre_id
                                   join i in db.Product_Images on a.product_id equals i.product_id
                                   join e in db.Discounts on a.disscount_id equals e.disscount_id
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.product_id descending // giảm dần
                                   select new ProductDTOs
                                   {
                                       product_img_status = i.status,
                                       Image = a.image,
                                       discount_name = e.discount_name,
                                       genre_name = c.genre_name,
                                       product_id = a.product_id,
                                       product_name = a.product_name,
                                       genre_id = c.genre_id,
                                       discount_id = e.disscount_id,
                                       discount_start = e.discount_start,
                                       discount_end = e.discount_end,
                                       discount_status = e.status,
                                       price = a.price,
                                       discount_price = e.discount_price,
                                       product_img_id = i.product_image_id
                                   };
                            break;
                        case "date_asc":
                            list = from a in db.Products
                                   join c in db.Genres on a.genre_id equals c.genre_id
                                   join i in db.Product_Images on a.product_id equals i.product_id
                                   join e in db.Discounts on a.disscount_id equals e.disscount_id
                                   where (a.status == "1" || a.status == "0")
                                   orderby a.product_id ascending // giảm dần
                                   select new ProductDTOs
                                   {
                                       product_img_status = i.status,
                                       Image = a.image,
                                       discount_name = e.discount_name,
                                       genre_name = c.genre_name,
                                       product_id = a.product_id,
                                       product_name = a.product_name,
                                       genre_id = c.genre_id,
                                       discount_id = e.disscount_id,
                                       discount_start = e.discount_start,
                                       discount_end = e.discount_end,
                                       discount_status = e.status,
                                       price = a.price,
                                       discount_price = e.discount_price,
                                       product_img_id = i.product_image_id
                                   };
                            break;
                        case "image":
                            list = from a in db.Products
                                   join c in db.Genres on a.genre_id equals c.genre_id
                                   join i in db.Product_Images on a.product_id equals i.product_id
                                   join e in db.Discounts on a.disscount_id equals e.disscount_id
                                   where ((a.status == "1" || a.status == "0") && i.image_1 != null && i.image_2 != null && i.image_3 != null && i.image_4 != null && i.image_5 != null)
                                   orderby i.product_id descending // giảm dần
                                   select new ProductDTOs
                                   {
                                       product_img_status = i.status,
                                       Image = a.image,
                                       discount_name = e.discount_name,
                                       genre_name = c.genre_name,
                                       product_id = a.product_id,
                                       product_name = a.product_name,
                                       genre_id = c.genre_id,
                                       discount_id = e.disscount_id,
                                       discount_start = e.discount_start,
                                       discount_end = e.discount_end,
                                       discount_status = e.status,
                                       price = a.price,
                                       discount_price = e.discount_price,
                                       product_img_id = i.product_image_id
                                   };
                            break;
                        case "one_image":
                            list = from a in db.Products
                                   join c in db.Genres on a.genre_id equals c.genre_id
                                   join i in db.Product_Images on a.product_id equals i.product_id
                                   join e in db.Discounts on a.disscount_id equals e.disscount_id
                                   where ((a.status == "1" || a.status == "0") && i.image_1 != null && i.image_2 == null && i.image_3 == null && i.image_4 == null && i.image_5 == null)
                                   orderby i.update_at ascending // giảm dần
                                   select new ProductDTOs
                                   {
                                       product_img_status = i.status,
                                       Image = a.image,
                                       discount_name = e.discount_name,
                                       genre_name = c.genre_name,
                                       product_id = a.product_id,
                                       product_name = a.product_name,
                                       genre_id = c.genre_id,
                                       discount_id = e.disscount_id,
                                       discount_start = e.discount_start,
                                       discount_end = e.discount_end,
                                       discount_status = e.status,
                                       price = a.price,
                                       discount_price = e.discount_price,
                                       product_img_id = i.product_image_id
                                   };
                            break;
                        case "two_images":
                            list = from a in db.Products
                                   join c in db.Genres on a.genre_id equals c.genre_id
                                   join i in db.Product_Images on a.product_id equals i.product_id
                                   join e in db.Discounts on a.disscount_id equals e.disscount_id
                                   where ((a.status == "1" || a.status == "0") && i.image_1 != null && i.image_2 != null && i.image_3 == null && i.image_4 == null && i.image_5 == null)
                                   orderby i.update_at ascending // giảm dần
                                   select new ProductDTOs
                                   {
                                       product_img_status = i.status,
                                       Image = a.image,
                                       discount_name = e.discount_name,
                                       genre_name = c.genre_name,
                                       product_id = a.product_id,
                                       product_name = a.product_name,
                                       genre_id = c.genre_id,
                                       discount_id = e.disscount_id,
                                       discount_start = e.discount_start,
                                       discount_end = e.discount_end,
                                       discount_status = e.status,
                                       price = a.price,
                                       discount_price = e.discount_price,
                                       product_img_id = i.product_image_id
                                   };
                            break;
                        case "three_images":
                            list = from a in db.Products
                                   join c in db.Genres on a.genre_id equals c.genre_id
                                   join i in db.Product_Images on a.product_id equals i.product_id
                                   join e in db.Discounts on a.disscount_id equals e.disscount_id
                                   where ((a.status == "1" || a.status == "0") && i.image_1 != null && i.image_2 != null && i.image_3 != null && i.image_4 == null && i.image_5 == null)
                                   orderby i.update_at ascending // giảm dần
                                   select new ProductDTOs
                                   {
                                       product_img_status = i.status,
                                       Image = a.image,
                                       discount_name = e.discount_name,
                                       genre_name = c.genre_name,
                                       product_id = a.product_id,
                                       product_name = a.product_name,
                                       genre_id = c.genre_id,
                                       discount_id = e.disscount_id,
                                       discount_start = e.discount_start,
                                       discount_end = e.discount_end,
                                       discount_status = e.status,
                                       price = a.price,
                                       discount_price = e.discount_price,
                                       product_img_id = i.product_image_id
                                   };
                            break;
                        case "four_images":
                            list = from a in db.Products
                                   join c in db.Genres on a.genre_id equals c.genre_id
                                   join i in db.Product_Images on a.product_id equals i.product_id
                                   join e in db.Discounts on a.disscount_id equals e.disscount_id
                                   where ((a.status == "1" || a.status == "0") && i.image_1 != null && i.image_2 != null && i.image_3 != null && i.image_4 != null && i.image_5 == null)
                                   orderby i.update_at ascending // giảm dần
                                   select new ProductDTOs
                                   {
                                       product_img_status = i.status,
                                       Image = a.image,
                                       discount_name = e.discount_name,
                                       genre_name = c.genre_name,
                                       product_id = a.product_id,
                                       product_name = a.product_name,
                                       genre_id = c.genre_id,
                                       discount_id = e.disscount_id,
                                       discount_start = e.discount_start,
                                       discount_end = e.discount_end,
                                       discount_status = e.status,
                                       price = a.price,
                                       discount_price = e.discount_price,
                                       product_img_id = i.product_image_id
                                   };
                            break;
                        default:
                            list = from a in db.Products
                                   join c in db.Genres on a.genre_id equals c.genre_id
                                   join i in db.Product_Images on a.product_id equals i.product_id
                                   join e in db.Discounts on a.disscount_id equals e.disscount_id
                                   where (a.status == "1" || a.status == "0")
                                   orderby i.update_at ascending  // giảm dần
                                   select new ProductDTOs
                                   {
                                       product_img_status = i.status,
                                       Image = a.image,
                                       discount_name = e.discount_name,
                                       genre_name = c.genre_name,
                                       product_id = a.product_id,
                                       product_name = a.product_name,
                                       genre_id = c.genre_id,
                                       discount_id = e.disscount_id,
                                       discount_start = e.discount_start,
                                       discount_end = e.discount_end,
                                       discount_status = e.status,
                                       price = a.price,
                                       discount_price = e.discount_price,
                                       product_img_id = i.product_image_id
                                   };
                            break;
                    }
                    if (string.IsNullOrEmpty(search)) return View(list.ToPagedList(pageNumber, pageSize));
                    switch (show)
                    {
                        //tìm kiếm tất cả
                        case "1":
                            list = list.Where(s => s.product_name.Trim().Contains(search) ||
                                                   s.product_id.ToString().Trim().Contains(search) || s.genre_id.ToString().Contains(search)
                                                   || s.discount_id.ToString().Contains(search) || s.product_img_id.ToString().Contains(search));
                            break;
                        //theo id img
                        case "2":
                            list = list.Where(s => s.product_img_id.ToString().Trim().Contains(search));
                            break;
                        //theo id sản phẩm
                        case "3":
                            list = list.Where(s => s.product_id.ToString().Trim().Contains(search));
                            break;
                        //theo tên sản phẩm
                        case "4":
                            list = list.Where(s => s.product_name.Trim().Contains(search));
                            break;
                        //theo tên thể loại
                        case "5":
                            list = list.Where(s => s.genre_id.ToString().Trim().Contains(search));
                            break;
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
        //gợi ý tìm kiếm ảnh sản phẩm
        public JsonResult GetProductImageSearch(string Prefix)
        {
            var search = (from c in db.Products
                join b in db.Product_Images on c.product_id equals b.product_id
                where c.product_name.Contains(Prefix)
                orderby c.product_name ascending
                select new { c.product_name, b.product_image_id,c.product_id });
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //View Chỉnh sửa ảnh sản phẩm
        public ActionResult ProductImgEdit(int? id,string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Modify() == true)
                {
                    if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                    {
                        return RedirectToAction("ProductImgEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                    }
                    var productimg = db.Product_Images.Where(m => m.product_id == id).FirstOrDefault();
                    if (productimg == null)
                    {
                        return HttpNotFound();
                    }
                    return View(productimg);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("ProductImgIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Code xử lý Chỉnh sửa ảnh sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductImgEdit(Product_Image product_Image, string returnUrl,int id)
        {
            var productimg = db.Product_Images.FirstOrDefault(m => m.product_id == id);
            if (ModelState.IsValid)
            {
                productimg.image_1 = product_Image.image_1;
                productimg.image_2 = product_Image.image_2;
                productimg.image_3 = product_Image.image_3;
                productimg.image_4 = product_Image.image_4;
                productimg.image_5 = product_Image.image_5;
                productimg.update_at = DateTime.Now;
                productimg.update_by = User.Identity.GetEmail();
                db.SaveChanges();
                Notification.set_flash("Cập nhật thành công:" +product_Image.product_id, "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("ProductImgIndex");
            }
            return View(product_Image);
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
