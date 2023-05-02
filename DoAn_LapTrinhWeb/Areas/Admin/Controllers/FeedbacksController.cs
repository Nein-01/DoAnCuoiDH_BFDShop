using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using DoAn_LapTrinhWeb;
using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class FeedbacksController : BaseController
    {
        private readonly DbContext db = new DbContext();
        //View list đánh giá
        public ActionResult FeedbackIndex(int?size,int?page,string search,string show,string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = size ?? 10;
                    var pageNumber = page ?? 1;
                    ViewBag.countTrash = db.Feedbacks.Count(a => a.status == "0"); //  đếm tổng sp có trong thùng rác
                    var reply_fb = db.Feedbacks.ToList();
                    ViewBag.reply_fb = reply_fb;
                    var fb_img = db.Feedback_Image.ToList();
                    ViewBag.fb_img = fb_img;
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.ResetSort = "";
                    ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_asc" : "date_desc";
                    ViewBag.NameSort = sortOrder == "name_desc" ? "name_asc" : "name_desc";
                    ViewBag.StarSort = sortOrder == "star_desc" ? "star_asc" : "star_desc";
                    ViewBag.SentiSort = sortOrder == "senti_good" ? "senti_bad" : "senti_good";
                    ViewBag.SentiSortNorm = sortOrder == "senti_norm" ? "senti_error" : "senti_norm" ;
                    var list = from fb in db.Feedbacks
                               join p in db.Products on fb.product_id equals p.product_id
                               join a in db.Accounts on fb.account_id equals a.account_id
                               where fb.status != "0" && fb.parent_feedback_id == 0
                               orderby fb.feedback_id descending
                               select new FeedbackDTOs
                               {
                                   product_name = p.product_name,
                                   product_slug = p.slug,
                                   feedback_id = fb.feedback_id,
                                   genre_id = p.genre_id,
                                   discount_id = p.disscount_id,
                                   description = fb.description,
                                   rating_star = fb.rate_star,
                                   AI_rated = fb.AI_rated,
                                   status = fb.status,
                                   create_at = fb.create_at,
                                   User_Email = a.Email,
                                   account_id = a.account_id,
                                   product_id = fb.product_id,
                               };
                    //Sort
                    switch (sortOrder)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Mới nhất";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status != "0" && fb.parent_feedback_id == 0
                                   orderby fb.feedback_id descending
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                        case "date_asc":
                            ViewBag.sortname = "Cũ nhất";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status != "0" && fb.parent_feedback_id == 0
                                   orderby fb.feedback_id
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                        case "name_desc":
                            ViewBag.sortname = "Tên sản phẩm(A-Z)";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status != "0" && fb.parent_feedback_id == 0
                                   orderby p.product_name descending
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                        case "name_asc":
                            ViewBag.sortname = "Tên sản phẩm(Z-A)";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status != "0" && fb.parent_feedback_id == 0
                                   orderby p.product_name descending
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                        case "star_desc":
                            ViewBag.sortname = "Số sao(1-5)";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status != "0" && fb.parent_feedback_id == 0
                                   orderby fb.rate_star descending
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                        case "star_asc":
                            ViewBag.sortname = "Số sao(5-1)";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status != "0" && fb.parent_feedback_id == 0
                                   orderby fb.rate_star
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                        //"senti_good" ? "star_good" : "star_bad"
                        case "senti_good":
                            ViewBag.sortname = "Tình trạng(Tệ)";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status != "0" && fb.parent_feedback_id == 0 && fb.AI_rated == "1"
                                   orderby fb.rate_star
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                        case "senti_bad":
                            ViewBag.sortname = "Tình trạng(Tốt)";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status != "0" && fb.parent_feedback_id == 0 && fb.AI_rated == "2"
                                   orderby fb.rate_star
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                        case "senti_norm":
                            ViewBag.sortname = "Tình trạng(Lỗi)";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status != "0" && fb.parent_feedback_id == 0 && fb.AI_rated == "0"
                                   orderby fb.rate_star
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                        case "senti_error":
                            ViewBag.sortname = "Tình trạng(Thường)";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status != "0" && fb.parent_feedback_id == 0 && fb.AI_rated == "Predict error!"
                                   orderby fb.rate_star
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                    }
                    //filter search
                    if (string.IsNullOrEmpty(search)) return View(list.ToPagedList(pageNumber, pageSize));
                    switch (show)
                    {
                        //tìm kiếm tất cả
                        case "1":
                            list = list.Where(s => s.feedback_id.ToString().Contains(search) || s.product_name.ToString().Contains(search) || s.rating_star.ToString().Contains(search));
                            break;
                        //theo id đánh giá
                        case "2":
                            list = list.Where(s => s.feedback_id.ToString().Contains(search));
                            break;
                        //theo tên sản phẩm
                        case "3":
                            list = list.Where(s => s.product_name.ToString().Contains(search));
                            break;
                        //theo số sao
                        case "4":
                            list = list.Where(s => s.rating_star.ToString().Contains(search));
                            break;
                    }
                    return View("FeedbackIndex", list.ToPagedList(pageNumber, 50));
                }
                else
                {
                    //nếu không phải là admin hoặc cộng tác viên thì sẽ back về trang chủ bảng điều khiển
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //View list trash đánh giá
        public ActionResult FeedbackTrash(int? size, int? page, string search, string show,string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.ResetSort = "";
                ViewBag.DateSortParm = sortOrder == "date_desc" ? "date_asc" : "date_desc";
                ViewBag.NameSort = sortOrder == "name_desc" ? "name_asc" : "name_desc";
                ViewBag.StarSort = sortOrder == "star_desc" ? "star_asc" : "star_desc";
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var fb_img = db.Feedback_Image.ToList();
                    ViewBag.fb_img = fb_img;
                    var pageSize = size ?? 10;
                    var pageNumber = page ?? 1;
                    var list = from fb in db.Feedbacks
                               join p in db.Products on fb.product_id equals p.product_id
                               join a in db.Accounts on fb.account_id equals a.account_id
                               where fb.status == "0"
                               orderby fb.feedback_id descending
                               select new FeedbackDTOs
                               {
                                   product_name = p.product_name,
                                   product_slug = p.slug,
                                   feedback_id = fb.feedback_id,
                                   genre_id = p.genre_id,
                                   discount_id = p.disscount_id,
                                   description = fb.description,
                                   rating_star = fb.rate_star,
                                   AI_rated = fb.AI_rated,
                                   status = fb.status,
                                   create_at = fb.create_at,
                                   User_Email = a.Email,
                                   account_id = a.account_id,
                                   product_id = fb.product_id,
                               };
                    //Sort
                    switch (sortOrder)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Mới nhất";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status == "0" && fb.parent_feedback_id == 0
                                   orderby fb.feedback_id descending
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                        case "date_asc":
                            ViewBag.sortname = "Cũ nhất";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status == "0" && fb.parent_feedback_id == 0
                                   orderby fb.feedback_id
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                        case "name_desc":
                            ViewBag.sortname = "Tên sản phẩm(A-Z)";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status == "0" && fb.parent_feedback_id == 0
                                   orderby p.product_name descending
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                        case "name_asc":
                            ViewBag.sortname = "Tên sản phẩm(Z-A)";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status == "0" && fb.parent_feedback_id == 0
                                   orderby p.product_name
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                        case "star_desc":
                            ViewBag.sortname = "Số sao(1-5)";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status == "0" && fb.parent_feedback_id == 0
                                   orderby fb.rate_star descending
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                        case "star_asc":
                            ViewBag.sortname = "Số sao(5-1)";
                            list = from fb in db.Feedbacks
                                   join p in db.Products on fb.product_id equals p.product_id
                                   join a in db.Accounts on fb.account_id equals a.account_id
                                   where fb.status == "0" && fb.parent_feedback_id == 0
                                   orderby fb.rate_star
                                   select new FeedbackDTOs
                                   {
                                       product_name = p.product_name,
                                       product_slug = p.slug,
                                       feedback_id = fb.feedback_id,
                                       genre_id = p.genre_id,
                                       discount_id = p.disscount_id,
                                       description = fb.description,
                                       rating_star = fb.rate_star,
                                       AI_rated = fb.AI_rated,
                                       status = fb.status,
                                       create_at = fb.create_at,
                                       User_Email = a.Email,
                                       account_id = a.account_id,
                                       product_id = fb.product_id,
                                   };
                            break;
                    }
                    //filter search
                    if (string.IsNullOrEmpty(search)) return View(list.ToPagedList(pageNumber, pageSize));
                    switch (show)
                    {
                        //tìm kiếm tất cả
                        case "1":
                            list = list.Where(s => s.feedback_id.ToString().Contains(search) || s.product_name.ToString().Contains(search) || s.rating_star.ToString().Contains(search));
                            break;
                        //theo id đánh giá
                        case "2":
                            list = list.Where(s => s.feedback_id.ToString().Contains(search));
                            break;
                        //theo tên sản phẩm
                        case "3":
                            list = list.Where(s => s.product_name.ToString().Contains(search));
                            break;
                        //theo số sao
                        case "4":
                            list = list.Where(s => s.rating_star.ToString().Contains(search));
                            break;
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
        //search đánh giá sản phẩm
        public JsonResult GetFeedbackSearch(string Prefix)
        {
            var search = (from c in db.Products
                join b in db.Feedbacks on c.product_id equals b.product_id
                where c.product_name.Contains(Prefix)
                orderby c.product_name ascending
                select new { b.feedback_id,c.product_name,b.rate_star });
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //phản hồi đánh giá
        public JsonResult ReplyFeedback(string feedback_content,int product_id,int genre_id,int parent_feedback_id,Feedback feedback)
        {
            bool result;
            try
            {
                if (User.Identity.Permiss_Modify() == true)
                {
                    feedback.rate_star = 5;
                    feedback.description = feedback_content;
                    feedback.product_id = product_id;
                    feedback.genre_id = genre_id;
                    feedback.parent_feedback_id = parent_feedback_id;
                    feedback.status = "2";//cái này dùng cho admin feedback
                    feedback.update_at = DateTime.Now;
                    feedback.create_at = DateTime.Now;
                    feedback.create_by = User.Identity.GetEmail();
                    feedback.update_by = User.Identity.GetEmail();
                    feedback.account_id = User.Identity.GetUserId();
                    db.Feedbacks.Add(feedback);
                    db.SaveChanges();
                    result = true;
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
        //Xác nhận duyệt đánh giá
        public ActionResult ChangeComplete(int? id, string RatingStar, string feedbackcontent,string imagefeedback)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
                {
                    CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                    var feedback = db.Feedbacks.SingleOrDefault(fb => fb.feedback_id == id);
                    var feedbackimage = db.Feedback_Image.Where(m => m.feedback_id == feedback.feedback_id).FirstOrDefault();
                    if (feedback != null)
                    {
                        feedback.status = "2";
                        feedback.update_by = User.Identity.GetName();
                        feedback.update_at = DateTime.Now;
                        db.SaveChanges();
                        Notification.set_flash("Đã xét duyệt đánh giá: " + feedback.feedback_id + "", "success");
                        if (feedback.description == null)
                        {
                            feedbackcontent = "";
                        }
                        else
                        {
                            feedbackcontent = "<div style='font-weight: 500;'>Nội dung đánh giá:</div>" + "<div>" + feedback.description + "</div>";
                        }
                        string emailID = feedback.Account.Email;
                        string Feedbackid = feedback.feedback_id.ToString();
                        string productslug = feedback.Product.slug;
                        string FeebackProduct = "<tr style='border-bottom: 1px solid rgba(0,0,0,.05);' class='product_item'>" +
                                              "<td valign='middle' width ='80%' style='text-align:left; padding:0 2.5em;'> " +
                                                  "<div class='product-entry'>" +
                                                      "<img src='https://bfd.vn/" + feedback.Product.image + "' style = 'width: 100px; max-width: 600px; height: auto; padding-bottom:5px; display: block;'>" +
                                                      "<div class='text'>" +
                                                          "<a href='" + Request.Url.Scheme + "://" + Request.Url.Authority + "/product/" + feedback.Product.slug + "'> <div class='product_name'>" + feedback.Product.product_name + "</div></a>" +
                                                      "</div>" +
                                                  "</div>" +
                                              "</td>" +
                                              "<td valign='middle' width='20%' style='text-align:right; padding-right: 2.5em;'>" +
                                                  "<span class='price' style='color: #005f8f; font-size: 14px; font-weight: 500;'>" + feedback.Product.price.ToString("#,0", cul.NumberFormat) + "₫" + "</span>" +
                                              "</td>" +
                                          "</tr>";
                        string star = "";
                        if (feedback.rate_star == 1)
                        { star = "<img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'> "; }
                        else if (feedback.rate_star == 2)
                        { star = "<img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'>"; }
                        else if (feedback.rate_star == 3)
                        { star = "<img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'>"; }
                        else if (feedback.rate_star == 4)
                        { star = "<img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'>"; }
                        else
                        { RatingStar = "<img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'>"; }
                        RatingStar = star;
                        string FeedbackStatus = "<span style='color:#28a745;'>Đã duyệt</span>";
                        string productname = feedback.Product.product_name;
                        SendEmailFeedback(RatingStar, FeebackProduct, FeedbackStatus, emailID, Feedbackid, productname, feedbackcontent, productslug, "FeedbackAccept");
                    }
                    return RedirectToAction("FeedbackIndex");
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("FeedbackIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Huỷ đánh giá
        [HttpPost]
        public JsonResult CancleFeedback(int id, string RatingStar)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
            Boolean result;
            var feedback = db.Feedbacks.Where(m => m.feedback_id == id).FirstOrDefault();
            var feedbackimage = db.Feedback_Image.Where(m => m.feedback_id == feedback.feedback_id).FirstOrDefault();
            if (feedback.status == "2")
            {
                result = false;
            }
            else
            {
                if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
                {
                    feedback.status = "0";
                    feedback.update_by = User.Identity.GetName();
                    feedback.update_at = DateTime.Now;
                    db.SaveChanges();
                    result = true;
                    string emailID = feedback.Account.Email;
                    string Feedbackid = feedback.feedback_id.ToString();
                    string feedbackcontent = "<div style='font-weight: 500;'>Lý do:</div>" +
                                            "<div>Vi phạm vào 1 trong các điều khoản khi đánh giá sản phẩm <a href='#' style='color: rgb(216, 1, 1); font-weight: 500 !important; text-decoration: none!important;'>(Xem tại đây)</a></div>";
                    string productslug = feedback.Product.slug;
                    string FeebackProduct = "<tr style='border-bottom: 1px solid rgba(0,0,0,.05);' class='product_item'>" +
                                          "<td valign='middle' width ='80%' style='text-align:left; padding:0 2.5em;'> " +
                                              "<div class='product-entry'>" +
                                                  "<img src='https://bfd.vn/" + feedback.Product.image + "' style = 'width: 100px; max-width: 600px; height: auto; padding-bottom:5px; display: block;'>" +
                                                  "<div class='text'>" +
                                                      "<a href='" + Request.Url.Scheme + "://" + Request.Url.Authority + "/product/" + feedback.Product.slug + "'> <div class='product_name'>" + feedback.Product.product_name + "</div></a>" +
                                                  "</div>" +
                                              "</div>" +
                                          "</td>" +
                                          "<td valign='middle' width='20%' style='text-align:right; padding-right: 2.5em;'>" +
                                              "<span class='price' style='color: #005f8f; font-size: 14px; font-weight: 500;'>" + feedback.Product.price.ToString("#,0", cul.NumberFormat) + "₫" + "</span>" +
                                          "</td>" +
                                      "</tr>";
                    string star = "";
                    if (feedback.rate_star == 1)
                    { star = "<img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'> "; }
                    else if (feedback.rate_star == 2)
                    { star = "<img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'>"; }
                    else if (feedback.rate_star == 3)
                    { star = "<img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'>"; }
                    else if (feedback.rate_star == 4)
                    { star = "<img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620663/star-o_yuph59.png'>"; }
                    else
                    { RatingStar = "<img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'> <img src='https://res.cloudinary.com/van-nam/image/upload/v1635620736/star_zxwzal.png'>"; }
                    RatingStar = star;
                    string FeedbackStatus = "<span style='color:#dc3545;'>Bị huỷ</span>";
                    string productname = feedback.Product.product_name;
                    SendEmailFeedback(RatingStar, FeebackProduct, FeedbackStatus, emailID, Feedbackid, productname, feedbackcontent, productslug, "FeedbackCancled");
                    Notification.set_flash("Hủy đánh giá id " + id + " thành công", "success");
                }
                else
                {
                    result = false;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //send mail khi thay đổi trạng thái
        public void SendEmailFeedback(string RatingStar, string FeedbackProduct, string FeedbackStatus, string emailID,string Feedbackid, string productname,  string feedbackcontent, string productslug, string emailFor)
        {
            // đường dẫn mail gồm có controller "Account"  +"emailfor" +  "code reset đã được mã hóa(mội lần gửi email quên mật khẩu sẽ random 1 code reset mới"
            ///để dùng google email gửi email reset cho người khác bạn cần phải vô đây "https://www.google.com/settings/security/lesssecureapps" Cho phép ứng dụng kém an toàn: Bật
            var fromEmail = new MailAddress(AccountEmail.UserEmail, AccountEmail.Name); // "username email-vd: vn123@gmail.com" ,"tên hiển thị mail khi gửi"
            var toEmail = new MailAddress(emailID);
            //nhập password của bạn
            var fromEmailPassword = AccountEmail.Password;
            string subject = "";
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "MailFeedback" + ".cshtml"); //dùng body mail html , file template nằm trong thư mục "EmailTemplate/Text.cshtml"
            if (emailFor == "FeedbackAccept")
            {
                subject = "Đánh giá của bạn về sản phẩm '"+ productname + "' Đã được duyệt";
                body = body.Replace("{{FeedbackRatingStar}}", RatingStar);
                body = body.Replace("{{ProductFeedback}}", FeedbackProduct);
                body = body.Replace("{{Feedbackcontent}}", feedbackcontent);
                body = body.Replace("{{FeedbackStatus}}", FeedbackStatus);
                body = body.Replace("{{ButtonConfirmLink}}", Request.Url.Scheme + "://" + Request.Url.Authority + "/product/" + productslug); 
                body = body.Replace("{{ButtonConfirmName}}", "Xem đánh giá");
            }
            else if (emailFor == "FeedbackCancled")
            {
                subject = "Đánh giá của bạn về sản phẩm '" + productname + "' Đã bị huỷ";
                body = body.Replace("{{FeedbackRatingStar}}", RatingStar);
                body = body.Replace("{{ProductFeedback}}", FeedbackProduct);
                body = body.Replace("{{Feedbackcontent}}", feedbackcontent);
                body = body.Replace("{{FeedbackStatus}}", FeedbackStatus);
                body = body.Replace("{{ButtonConfirmLink}}",  Request.Url.Scheme + "://" + Request.Url.Authority +"/product/" + productslug);
                body = body.Replace("{{ButtonConfirmName}}", "Xem sản phẩm");
            }
            var smtp = new SmtpClient
            {
                Host = AccountEmail.Host, //tên mấy chủ nếu dùng gmail thì đổi  "Host = "smtp.gmail.com"
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
            try
            {
                smtp.Send(message);
            }
            catch(Exception ex)
            {
                    Notification.set_flash("Có lỗi khi gửi thông báo duyệt đánh giá! " +
                                           "/nDetail: "+ex,"danger");
                    
            }
                
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
