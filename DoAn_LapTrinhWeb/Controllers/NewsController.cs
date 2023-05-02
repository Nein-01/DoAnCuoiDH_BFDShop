using System.Web.Mvc;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using static DoAn_LapTrinhWeb.DTOs.NewsDTO;
using DoAn_LapTrinhWeb.DTOs;
using PagedList;
using DoAn_LapTrinhWeb.Common.Helpers;

namespace DoAn_LapTrinhWeb.Controllers
{
    public class NewsController : Controller
    {
        private DbContext db = new DbContext();
        // category
        public PartialViewResult Listcategory()
        {
            ViewBag.newscategory = db.ChildCategory.ToList();
            return PartialView("Listcategory", db.ParentCategory.Where(m=>m.status=="1" &&(m.ChildCategory.Count>0)).ToList());
        }
        //category dành cho giao diện mobile
        public PartialViewResult Listcategory_mobile()
        {
            ViewBag.newscategory = db.ChildCategory.ToList();
            return PartialView("Listcategory_mobile", db.ParentCategory.Where(m => m.status == "1").ToList());
        }
        public ActionResult News()
        {
            return View();
        }
        //trang chủ bài viết
        public ActionResult NewsIndex()
        {
            DateTime today = DateTime.Today;
            //=>danh mục bài viết: con
            ViewBag.childcategory = db.ChildCategory.Where(m=>m.status=="1").ToList();
            //=>danh mục bài viết: cha
            ViewBag.parentcategory = db.ParentCategory.Where(m => m.status == "1").ToList();
            //=> bài viết
            ViewBag.post_ofnewscategory = db.News.Where(m => m.status == "1").Take(4).ToList();
            //=> bài viết gần đây
            ViewBag.recentnews = db.News.OrderByDescending(m => m.news_id).Where(m => m.status == "1").OrderByDescending(m=>m.create_at).Take(10).ToList();
            //=> top commnet of month
            List<News> topcommentofmonth = db.News.Where(m => m.status == "1" && m.create_at.Month == today.Month).OrderByDescending(m => m.NewsComments.Count()).Take(6).ToList();
            List<News> topcomment = db.News.Where(m => m.status == "1").OrderByDescending(m => m.NewsComments.Count()).Take(6).ToList();
            if (topcommentofmonth.Count()>0)
            {
                ViewBag.topcomment = topcommentofmonth;
            }
            else
            {
                ViewBag.topcomment = topcomment;
            }
            List<News> topnews = db.News.Where(m => m.status == "1").OrderByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(20).ToList();
            List<News> topnews_2 = db.News.Where(m => m.status == "1" && m.create_at.Month == today.Month).OrderByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(20).ToList();
            if (topnews.Count() > 0)
            {
                ViewBag.topnews = topnews;
            }
            else
            {
                ViewBag.topnews = topnews_2;
            }
            //=>
            ViewBag.gamepost = db.News.Where(m => m.status == "1" && m.ChildCategory.ParentCategory.parentcategory_id==2 && m.create_at.Year == DateTime.Now.Year).OrderByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(4).ToList();
            //=> Bài viết sản phẩm
            ViewBag.productsreviews = db.NewsProducts.OrderByDescending(m=>m.News.ViewCount).ToList();
            ViewBag.productnew  = db.Products.Where(m => m.status == "1" && m.create_at.Year==DateTime.Now.Year).OrderByDescending(m=>m.NewsProducts.Count()).Take(5).ToList();
            //=>mẹo hay mỗi ngày
            //ViewBag.tips =db.ChildCategory.Where(m=>m.childcategory_id== && )
            //=> Trendingnow
            List<News> trendingnow = db.News.Where(m => m.status == "1").OrderByDescending(m=>m.create_at).ThenByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(4).ToList();
            ViewBag.trendingnow = trendingnow;
            //=> ghim bài
            ViewBag.stickyPosts = db.StickyPosts.Where(m => m.News.status == "1" && ( m.sticky_start<DateTime.Now && m.sticky_end>DateTime.Now)).Take(1).ToList();
            List<News> Hostpostofday = db.News.Where(m => m.status == "1" && m.create_at.Day == today.Day).OrderByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(4).ToList();
            List<News> Hostpostofmonth = db.News.Where(m => m.status == "1" && m.create_at.Month == today.Month).OrderByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(4).ToList();
            List<News> Hostpost = db.News.Where(m => m.status == "1").OrderByDescending(m => (m.ViewCount + m.NewsComments.Count())).Take(4).ToList();
            if (Hostpostofday.Count()>0)
            {
                ViewBag.Hostpost = Hostpost;
            }
            else if (Hostpostofmonth.Count() > 0)
            {
                ViewBag.Hostpost = Hostpost;
            }
            else 
            {
                ViewBag.Hostpost = Hostpost;
            }
            //=> tag
            ViewBag.listtag  = db.Tags.OrderByDescending(m => Guid.NewGuid()).Take(9).ToList();
            //tính lượt bình luận
            ViewBag.countcomment = db.NewsComments.Where(m => m.status == "2").ToList();
            //tính số lượt trả lời bình luận
            ViewBag.replycountcomment = db.Reply_Comments.Where(m => m.status == "2").ToList();
            BannerGlobal();
            return View();
        }
        //danh sách childcategory của category
        public ActionResult ListNewsCategory(int? page,string slug)
        {
            ViewBag.categoryimage = db.ParentCategory.Where(m => m.slug == slug).FirstOrDefault().image2;
            ViewBag.category = db.ParentCategory.Where(m => m.slug == slug).FirstOrDefault().name;
            BannerGlobal();
            var parentcate = db.ParentCategory.Where(m => m.status == "1" && m.slug==slug && (m.ChildCategory.Count()>0)).FirstOrDefault();
            return View("ChildCategory", GetNewsCategory(m => m.status == "1" && m.ParentCategory.slug == parentcate.slug, page));
        }
        //nav category bên phải và tag
        public ActionResult MenuCategory()
        {
            List<ParentCategory> parentcate = db.ParentCategory.Take(6).ToList();
            List<Tags> listtag = db.Tags.Take(9).Where(m=>m.NewsTags.Count()>0).OrderBy(m=>Guid.NewGuid()).ToList();
            ViewBag.listtag = listtag;
            ViewBag.newscategory = db.ChildCategory;
            if (parentcate.FirstOrDefault().status == "1")
            {
                return PartialView("_MenuCategory", parentcate);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //lấy danh sách loại tin
        private IPagedList GetNewsCategory(Expression<Func<ChildCategory, bool>> expr, int? page)
        {
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            var list = db.ChildCategory.Where(expr).OrderByDescending(m => m.childcategory_id).ToPagedList(pageNumber, pageSize);
            return list;
        }
        //List bài viết của tag
        public ActionResult ListNewsTags(int? page, int? size, string slug)
        {           
            ViewBag.ListName = db.Tags.Where(m=>m.slug==slug).FirstOrDefault().tag_name;
            var pageSize = size ?? 10;
            var pageNumber = page ?? 1;
            var list = from n in db.News
                       join
                        nt in db.NewsTags on n.news_id equals nt.news_id
                       join tg in db.Tags on nt.tag_id equals tg.tag_id
                       where tg.slug == slug &&n.status=="1"
                       orderby n.news_id descending
                       select new NewsDTO
                       {
                           tag_slug = tg.slug,
                           news_title = n.news_title,
                           create_at = n.create_at,
                           tag_name = tg.tag_name,
                           author = n.Accounts.Name,
                           image2 = n.image2,
                           news_slug = n.slug
                       };
            BannerGlobal();
            ViewBag.CountPost = list.Count();
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        //List bài viết của tác giá
        public ActionResult Author(int? page, int? size, int id)
        {
            var account = db.Accounts.Where(m => m.account_id == id).FirstOrDefault();
            ViewBag.CountPost = account.News.Count();
            ViewBag.AuthorName = account.Name;
            ViewBag.Avatar = account.Avatar;
            ViewBag.CountCommentAuth = account.Reply_Comments.Count();
            ViewBag.ListName = account.Name;
            var pageSize = size ?? 10;
            var pageNumber = page ?? 1;
            var list = from n in db.News
                       join ac in db.Accounts on n.account_id equals ac.account_id
                       where ac.status == "1" && ac.account_id == id
                       orderby n.news_id descending
                       select new NewsDTO
                       {
                           news_id= n.news_id,
                           create_at = n.create_at,
                           image2 = n.image2,
                           news_title=n.news_title,
                           news_slug = n.slug
                       };
            BannerGlobal();
            ViewBag.CountPost = list.Count();
            //tính lượt bình luận
            ViewBag.countcomment = db.NewsComments.Where(m => m.status == "2").ToList();
            //tính số lượt trả lời bình luận
            ViewBag.replycountcomment = db.Reply_Comments.Where(m => m.status == "2").ToList();
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        //List bài viết của sản phẫm
        public ActionResult PostProduct (int? page, int? size,string slug)
        {
            var product = db.NewsProducts.Where(m => m.Product.slug == slug).FirstOrDefault();
            ViewBag.ProductImg = product.Product.image;
            ViewBag.ProductName = product.Product.product_name;
            ViewBag.ProductSlug = product.Product.slug;
            if (product.Product.Discount.discount_start< DateTime.Now && product.Product.Discount.discount_end > DateTime.Now && product.Product.Discount.status=="1")
            {
                ViewBag.Product_Price = product.Product.price - product.Product.Discount.discount_price;
                ViewBag.Product_DCPrice = product.Product.price;
            }
            else
            {
                ViewBag.Product_Price = product.Product.price;
            }
            var pageSize = size ?? 10;
            var pageNumber = page ?? 1;
            var list = from n in db.News
                       join np in db.NewsProducts on n.news_id equals np.news_id
                       join pd in db.Products on np.product_id equals pd.product_id
                       where n.status == "1" && pd.status=="1" && np.Product.slug==slug
                       orderby n.news_id descending
                       select new NewsDTO
                       {
                           create_at = n.create_at,
                           news_id=n.news_id,
                           image2 = n.image2,
                           news_title = n.news_title,
                           news_slug = n.slug
                       };
            BannerGlobal();
            ViewBag.CountPost = list.Count();
            //tính lượt bình luận
            ViewBag.countcomment = db.NewsComments.Where(m => m.status == "2").ToList();
            //tính số lượt trả lời bình luận
            ViewBag.replycountcomment = db.Reply_Comments.Where(m => m.status == "2").ToList();
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        //sản phẩm mới
        public ActionResult PostProductRecent(int? page, int? size)
        {
            var pageSize = size ?? 12;
            var pageNumber = page ?? 1;
            var list = from pd in db.Products
                       join np in db.NewsProducts on pd.product_id equals np.product_id
                       join n in db.News on np.news_id equals n.news_id
                       group new {np, pd} by new {np.Product.slug, np.Product.create_at,np.Product.image,np.Product.product_name,pd.NewsProducts.Count,np.Product.status } into g
                       orderby g.Key.create_at descending
                       where g.Key.status == "1"
                       select new NewsDTO
                       {
                           product_slug = g.Key.slug,
                           create_at = g.Key.create_at,
                           product_image=g.Key.image,
                           product_name = g.Key.product_name,
                           count_product_post=g.Key.Count
                       };
            BannerGlobal();
            ViewBag.CountProduct = list.Count();
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        //tìm kiếm bài viết
        public ActionResult SearchResult(string s, int? page )
        {
            List<NewsDTO> listtag = (from n in db.News
                                  join nt in db.NewsTags on n.news_id equals nt.news_id
                                  join tg in db.Tags on nt.tag_id equals tg.tag_id
                                  where n.news_title.Contains(s) && n.status == "1"
                                  orderby n.news_id descending
                                  select new NewsDTO
                                  {
                                      tag_slug = tg.slug,
                                      news_title = n.news_title,
                                      news_comment = n.NewsComments.Count(),
                                      create_at = n.create_at,
                                      tag_name = tg.tag_name,
                                      author = n.Accounts.Name,
                                      image2 = n.image2,
                                      news_slug = n.slug
                                  }).ToList();
            var list = db.News.OrderByDescending(m => m.news_id);
            ViewBag.listtag = listtag;
            ViewBag.ListName = "Tìm kiếm bài viết";
            //tính lượt bình luận
            ViewBag.countcomment = db.NewsComments.Where(m => m.status == "2").ToList();
            //tính số lượt trả lời bình luận
            ViewBag.replycountcomment = db.Reply_Comments.Where(m => m.status == "2").ToList();
            BannerGlobal();
            return View("News", GetNews(m => m.status == "1" && m.news_title.Contains(s), page));
        }
        //gợi ý search
        [HttpPost]
        public JsonResult Index(string Prefix)
        {
            //tìm sản phẩm theo tên
            var search = (from N in db.Accounts
                          where(N.Name.Contains(Prefix))
                          select new { N.Name});
            return Json(search, JsonRequestBehavior.AllowGet);
        }
        //lấy danh sách tin tức của childcategory
        public ActionResult ListNews(int? page, string slug)
        {
            List<NewsDTO> list = (from n in db.News
                       join
                        nt in db.NewsTags on n.news_id equals nt.news_id
                       join tg in db.Tags on nt.tag_id equals tg.tag_id
                       where n.ChildCategory.slug == slug && n.status == "1"
                       orderby n.news_id descending
                       select new NewsDTO
                       {
                           tag_slug = tg.slug,
                           news_title = n.news_title,
                           create_at = n.create_at,
                           tag_name = tg.tag_name,
                           author = n.Accounts.Name,
                           image2 = n.image2,
                           news_slug = n.slug
                       }).ToList();
            ViewBag.listtag = list;
            var ChildCategory= db.ChildCategory.Where(m => m.slug == slug && m.status=="1").FirstOrDefault();
            ViewBag.ListName = ChildCategory.name;
            ViewBag.Countpost = db.News.Where(m => m.status == "1" && (m.ChildCategory.slug == ChildCategory.slug)).Count();
            //tính lượt bình luận
            ViewBag.countcomment = db.NewsComments.Where(m => m.status == "2").ToList();
            //tính số lượt trả lời bình luận
            ViewBag.replycountcomment = db.Reply_Comments.Where(m => m.status == "2").ToList();
            BannerGlobal();
            return View("News", GetNews(m => m.status == "1" && (m.ChildCategory.slug == slug ), page));
        }
        //lấy tất cả tin tức
        public ActionResult AllListNews(int? page, string sortOrder)
        {
            List<NewsDTO> list = (from n in db.News
                                  join nt in db.NewsTags on n.news_id equals nt.news_id
                                  join tg in db.Tags on nt.tag_id equals tg.tag_id
                                  where n.status == "1"
                                  orderby n.create_at descending
                                  select new NewsDTO
                                  {
                                      tag_slug = tg.slug,
                                      news_title = n.news_title,
                                      create_at = n.create_at,
                                      tag_name = tg.tag_name,
                                      author = n.Accounts.Name,
                                      image2 = n.image2,
                                      news_slug = n.slug
                                  }).Take(10).ToList();          
            ViewBag.ListName = "Tin tức mới nhất";
            ViewBag.Countpost = list.Count();
            //tính lượt bình luận
            ViewBag.countcomment = db.NewsComments.Where(m => m.status == "2").ToList();
            //tính số lượt trả lời bình luận
            ViewBag.replycountcomment = db.Reply_Comments.Where(m => m.status == "2").ToList();
            BannerGlobal();
            return View("News", GetNews(m => m.status == "1", page));
        }
        //Xử lý code get bài viết
        private IPagedList GetNews(Expression<Func<News, bool>> expr, int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.category = db.ParentCategory.ToList();
            ViewBag.newscategory = db.ChildCategory.ToList();
            var list = db.News.Where(expr).OrderByDescending(m => m.news_id).ToPagedList(pageNumber, pageSize);
            return list;
        }
        //Chi tiết bài viết
        public ActionResult NewsDetail(string slug,int ?page)
        {
            int pagesize = 5;//cho phép hiện 12 sản phẩm trên mỗi loại sản phẩm
            int cpage = page ?? 1;
            var news = db.News.Where(m => m.slug == slug && m.status=="1" && m.ChildCategory.status=="1").OrderByDescending(m=>m.news_id).ToList().FirstOrDefault();
            ViewBag.recentnews = db.News.Where(m => m.slug != slug && m.status == "1").OrderByDescending(m => m.create_at).ToList().Take(5);
            ViewBag.hotnews  = db.News.Where(m => m.slug != slug && m.status == "1").OrderByDescending(m => m.ViewCount).ToList().Take(3);
            //bình luận bài viết
            var comment = db.NewsComments.Where(m => m.news_id == news.news_id && m.status == "2").OrderByDescending(m=>m.comment_id).ToList();
            ViewBag.comment = comment.ToPagedList(cpage, pagesize);
            //trả lời bài viết
            ViewBag.reply_comment = db.Reply_Comments.Where(m=>m.status == "2").ToList();
            //tính lượt bình luận
            ViewBag.countcomment = db.NewsComments.Where(m => m.news_id == news.news_id && m.status == "2").Count();
            //tính số lượt trả lời bình luận
            ViewBag.replycountcomment = db.Reply_Comments.Where(m =>m.status == "2" && m.NewsComments.news_id==news.news_id).Count();
            //tính số lượt like dislike bình luận bài viết
            ViewBag.reaction = db.CommentLikes.ToList();
            ViewBag.replyreaction = db.ReplyCommentLikes.ToList();
            ViewBag.newstags = db.NewsTags.Where(m => m.news_id == news.news_id).ToList();
            ViewBag.newsproducts = db.NewsProducts.Where(m => m.news_id == news.news_id ).ToList();
            List<News> relatedpost = db.News.Where(m => m.ChildCategory.ParentCategory.parentcategory_id == news.ChildCategory.ParentCategory.parentcategory_id && m.news_id != news.news_id &&m.status=="1").OrderByDescending(m=>m.news_id).Take(6).ToList();
            ViewBag.relatedpost = relatedpost;
            ViewBag.hostpostcomment = db.NewsComments.Where(m=>m.status=="2").ToList();
            ViewBag.hostpostrepcomment = db.Reply_Comments.Where(m => m.status == "2").ToList();
            BannerGlobal();
            news.ViewCount++;
            db.SaveChanges();
            return View(news);
        }
        //gợi ý search
        [HttpPost]
        public JsonResult GetPostSearch(string Prefix)
        {
            //tìm sản phẩm theo tên
            var search = (from c in db.News
                          where c.status == "1" && (c.news_title.Contains(Prefix) || c.ChildCategory.name.ToString().Contains(Prefix) || c.ChildCategory.ParentCategory.name.Contains(Prefix))
                          orderby c.news_title ascending
                          select new NewsDTO
                          {
                              news_title = c.news_title,
                              news_slug = c.slug,
                              image = c.image,
                          });
            return Json(search, JsonRequestBehavior.AllowGet);
        }

        //bình luận bài viết
        [ValidateInput(false)]
        public JsonResult CommentPost(NewsComments comment)
        {
            bool result;
            try {
                if (User.Identity.IsAuthenticated)
                {
                    comment.account_id = User.Identity.GetUserId();
                    comment.status = "1";
                    comment.create_at = DateTime.Now;
                    db.NewsComments.Add(comment);
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
        //phản hồi bình luận
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult ReplyCommentPost(int id,string reply_content, Reply_Comments reply)
        {
            bool result;
            var comment_post = db.NewsComments.FirstOrDefault(m => m.comment_id==id);
            if (comment_post != null && User.Identity.IsAuthenticated)
            {
                reply.comment_id = id;
                reply.account_id = User.Identity.GetUserId();
                reply.reply_comment_content = reply_content;
                reply.create_at = DateTime.Now;
                if (User.Identity.GetRole() == 4)
                {
                    reply.status = "2";
                }
                else
                {
                    reply.status = "1";
                }
                db.Reply_Comments.Add(reply);
                db.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        ////like comment bài viết
        [HttpPost]
        public JsonResult ReactionCommmentPost(CommentLikes commentLikes, int comment_id)
        {
            bool result;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int Userid = User.Identity.GetUserId();
                    var comment = db.CommentLikes.FirstOrDefault(m => m.comment_id == comment_id && m.account_id == Userid);
                    if (comment != null)
                    {
                        db.CommentLikes.Remove(comment);
                    }
                    else
                    {
                        commentLikes.account_id = User.Identity.GetUserId();
                        commentLikes.create_at = DateTime.Now;
                        commentLikes.comment_id = comment_id;
                        commentLikes.comment_like = "1";
                        db.CommentLikes.Add(commentLikes);
                    }
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
        ////like reply comment
        [HttpPost]
        public JsonResult ReactionReplyCommment(ReplyCommentLikes rep_commentLikes, int rep_comment_id)
        {
            bool result;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int Userid = User.Identity.GetUserId();
                    var rep_comment = db.ReplyCommentLikes.FirstOrDefault(m => m.reply_comment_id == rep_comment_id && m.account_id == Userid);
                    if (rep_comment != null)
                    {
                        db.ReplyCommentLikes.Remove(rep_comment);
                    }
                    else
                    {
                        rep_commentLikes.account_id = User.Identity.GetUserId();
                        rep_commentLikes.create_at = DateTime.Now;
                        rep_commentLikes.reply_comment_id = rep_comment_id;
                        rep_commentLikes.reply_like = "1";
                        db.ReplyCommentLikes.Add(rep_commentLikes);
                    }
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
        //hiện thị banner toàn layout
        public void BannerGlobal()
        {
            ViewBag.BannerTopHorizontal = db.Banners.OrderByDescending(m => Guid.NewGuid()).Where(m => m.banner_start < DateTime.Now && m.banner_end > DateTime.Now && m.status == "1" && m.banner_type == 3).Take(8).ToList();
        }
    }
}