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
using DoAn_LapTrinhWeb.DTOs;
using static DoAn_LapTrinhWeb.DTOs.NewsDTO;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class NewsAdminController : BaseController
    {
        private DbContext db = new DbContext();

        //view trang chủ danh sách tin tức
        public ActionResult NewsIndex(string search, string show, int? size, int? page, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 10);
                    var pageNumber = (page ?? 1);
                    ViewBag.countTrash = db.News.Count(a => a.status == "0");
                    ViewBag.ResetSort = "";
                    ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
                    ViewBag.CategorySortParm = sortOrder == "category_asc" ? "category_desc" : "category_asc";
                    ViewBag.ViewSortParm = sortOrder == "view_asc" ? "view_desc" : "view_asc";
                    ViewBag.CommentSortParm = sortOrder == "comment_asc" ? "comment_desc" : "comment_asc";
                    var list = from a in db.News
                               where (a.status == "1")
                               orderby a.news_id descending
                               select a;
                    switch (sortOrder)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Mới nhất";
                            list = from a in db.News
                                   where (a.status == "1")
                                   orderby a.news_id descending
                                   select a;
                            break;
                        case "date_asc":
                            ViewBag.sortname = "Cũ nhất";
                            list = from a in db.News
                                   where (a.status == "1")
                                   orderby a.news_id
                                   select a;
                            break;
                        case "name_desc":
                            ViewBag.sortname = "Tên tiêu đề(Z-A)";
                            list = from a in db.News
                                   where (a.status == "1")
                                   orderby a.news_title descending
                                   select a;
                            break;
                        case "name_asc":
                            ViewBag.sortname = "Tên tiêu đề(A-Z)";
                            list = from a in db.News
                                   where (a.status == "1")
                                   orderby a.news_title
                                   select a;
                            break;
                        case "category_desc":
                            ViewBag.sortname = "Tên danh mục(Z-A)";
                            list = from a in db.News
                                   where (a.status == "1")
                                   orderby a.ChildCategory.name descending
                                   select a;
                            break;
                        case "category_asc":
                            ViewBag.sortname = "Tên danh mục(A-Z)";
                            list = from a in db.News
                                   where (a.status == "1")
                                   orderby a.ChildCategory.name
                                   select a;
                            break;
                        case "view_desc":
                            ViewBag.sortname = "Lượt xem(nhiều-ít)";
                            list = from a in db.News
                                   where (a.status == "1")
                                   orderby a.ViewCount descending
                                   select a;
                            break;
                        case "view_asc":
                            ViewBag.sortname = "Lượt xem(ít-nhiều)";
                            list = from a in db.News
                                   where (a.status == "1")
                                   orderby a.ViewCount
                                   select a;
                            break;
                        case "comment_desc":
                            ViewBag.sortname = "Lượt xem(nhiều-ít)";
                            list = from a in db.News
                                   where (a.status == "1")
                                   orderby a.NewsComments.Count descending
                                   select a;
                            break;
                        case "comment_asc":
                            ViewBag.sortname = "Lượt bình luận(ít-nhiều)";
                            list = from a in db.News
                                   where (a.status == "1")
                                   orderby a.NewsComments.Count
                                   select a;
                            break;
                    }
                    if (string.IsNullOrEmpty(search)) return View(list.ToPagedList(pageNumber, pageSize));
                    switch (show)
                    {
                        //tìm kiếm tất cả
                        case "1":
                            list = (IOrderedQueryable<News>)list.Where(s =>
                                s.news_id.ToString().Contains(search) || s.news_title.Contains(search));
                            break;
                        //theo id
                        case "2":
                            list = (IOrderedQueryable<News>)list.Where(s => s.news_id.ToString().Contains(search));
                            break;
                        //theo tên thể loại
                        case "3":
                            list = (IOrderedQueryable<News>)list.Where(s => s.news_title.ToString().Contains(search));
                            break;
                        //theo tên danh mục
                        case "4":
                            list = (IOrderedQueryable<News>)list.Where(s => s.ChildCategory.name.ToString().Contains(search));
                            break;
                    }
                    return View("NewsIndex", list.ToPagedList(pageNumber, 50));
                }
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }

        //view danh danh bài ghim
        public ActionResult StickyPost(string search, string show, int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.News = db.News.Where(m => m.status == "1").OrderByDescending(m => m.news_id).ToList();
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    List<StickyPost> sticky = db.StickyPosts.ToList();
                    foreach (var item in sticky)
                    {
                        if (item.sticky_end < DateTime.Now)
                        {
                            db.StickyPosts.Remove(item);
                            db.SaveChanges();
                        }
                    }
                    var pageSize = (size ?? 10);
                    var pageNumber = (page ?? 1);
                    var list = from a in db.StickyPosts orderby a.id descending select a;
                    if (!string.IsNullOrEmpty(search))
                    {
                        switch (show)
                        {
                            //tìm kiếm tất cả
                            case "1":
                                list = (IOrderedQueryable<StickyPost>)list.Where(s =>
                                    s.id.ToString().Contains(search) || s.id.ToString().Contains(search));
                                break;
                            //theo id
                            case "2":
                                list = (IOrderedQueryable<StickyPost>)list.Where(s => s.id.ToString().Contains(search));
                                break;
                        }
                        return View("NewsIndex", list.ToPagedList(pageNumber, 50) as IPagedList<News>);
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
        //xoá bài ghim
        [HttpPost]
        public JsonResult DeleteStickyPost(int id)
        {
            bool result;
            if (User.Identity.Permiss_Delete() == true)
            {
                StickyPost hotPost = db.StickyPosts.Find(id);
                string tittle = hotPost.News.news_title;
                db.StickyPosts.Remove(hotPost);
                db.SaveChanges();
                result = true;
                Notification.set_flash("Xóa bài ghim id " + id + " thành công", "success");
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //tạo bài ghim
        [HttpPost]
        public JsonResult CreateStickyPost(int id, DateTime sticky_start, DateTime sticky_end)
        {
            bool result;
            if (User.Identity.Permiss_Create() == true)
            {
                StickyPost hotPost = new StickyPost
                {
                    news_id = id,
                    create_by = User.Identity.GetEmail(),
                    update_by = User.Identity.GetEmail(),
                    sticky_start = sticky_start,
                    sticky_end = sticky_end,
                };
                db.StickyPosts.Add(hotPost);
                db.SaveChanges();
                result = true;
                Notification.set_flash("Thêm mới thành công", "success");
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Danh sách bài viết trong thùng rác
        public ActionResult NewsTrash(string search, string show, int? size, int? page, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 10);
                    var pageNumber = (page ?? 1);
                    var list = from a in db.News
                               where (a.status == "0")
                               orderby a.update_at descending
                               select a;
                    ViewBag.ResetSort = "";
                    ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
                    ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
                    ViewBag.CategorySortParm = sortOrder == "category_asc" ? "category_desc" : "category_asc";
                    ViewBag.ViewSortParm = sortOrder == "view_asc" ? "view_desc" : "view_asc";
                    ViewBag.CommentSortParm = sortOrder == "comment_asc" ? "comment_desc" : "comment_asc";
                    switch (sortOrder)
                    {
                        case "date_desc":
                            ViewBag.sortname = "Mới nhất";
                            list = from a in db.News
                                   where (a.status == "0")
                                   orderby a.news_id descending
                                   select a;
                            break;
                        case "date_asc":
                            ViewBag.sortname = "Cũ nhất";
                            list = from a in db.News
                                   where (a.status == "0")
                                   orderby a.news_id
                                   select a;
                            break;
                        case "name_desc":
                            ViewBag.sortname = "Tên tiêu đề(Z-A)";
                            list = from a in db.News
                                   where (a.status == "0")
                                   orderby a.news_title descending
                                   select a;
                            break;
                        case "name_asc":
                            ViewBag.sortname = "Tên tiêu đề(A-Z)";
                            list = from a in db.News
                                   where (a.status == "0")
                                   orderby a.news_title
                                   select a;
                            break;

                        case "category_desc":
                            ViewBag.sortname = "Tên danh mục(Z-A)";
                            list = from a in db.News
                                   where (a.status == "0")
                                   orderby a.ChildCategory.name descending
                                   select a;
                            break;
                        case "category_asc":
                            ViewBag.sortname = "Tên danh mục(A-Z)";
                            list = from a in db.News
                                   where (a.status == "0")
                                   orderby a.ChildCategory.name
                                   select a;
                            break;
                        case "view_desc":
                            ViewBag.sortname = "Lượt xem(nhiều-ít)";
                            list = from a in db.News
                                   where (a.status == "0")
                                   orderby a.ViewCount descending
                                   select a;
                            break;
                        case "view_asc":
                            ViewBag.sortname = "Lượt xem(ít-nhiều)";
                            list = from a in db.News
                                   where (a.status == "0")
                                   orderby a.ViewCount
                                   select a;
                            break;
                        case "comment_desc":
                            ViewBag.sortname = "Lượt xem(nhiều-ít)";
                            list = from a in db.News
                                   where (a.status == "0")
                                   orderby a.NewsComments.Count descending
                                   select a;
                            break;

                        case "comment_asc":
                            ViewBag.sortname = "Lượt bình luận(ít-nhiều)";
                            list = from a in db.News
                                   where (a.status == "0")
                                   orderby a.NewsComments.Count
                                   select a;
                            break;
                    }
                    if (string.IsNullOrEmpty(search)) return View(list.ToPagedList(pageNumber, pageSize));
                    switch (show)
                    {
                        //tìm kiếm tất cả
                        case "1":
                            list = (IOrderedQueryable<News>)list.Where(s =>
                                s.news_id.ToString().Contains(search) || s.news_title.Contains(search));
                            break;
                        //theo id
                        case "2":
                            list = (IOrderedQueryable<News>)list.Where(s => s.news_id.ToString().Contains(search));
                            break;
                        //theo tên thể loại
                        case "3":
                            list = (IOrderedQueryable<News>)list.Where(s => s.news_title.ToString().Contains(search));
                            break;
                        //theo tên danh mục
                        case "4":
                            list = (IOrderedQueryable<News>)list.Where(s => s.ChildCategory.name.ToString().Contains(search));
                            break;
                    }
                    return View("NewsTrash", list.ToPagedList(pageNumber, 50));
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
        //Xem chi tiết tin tức
        public ActionResult NewsDetails(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Read() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    News news = db.News.Find(id);
                    ViewBag.newstags = db.NewsTags.Where(m => m.news_id == news.news_id).ToList();
                    ViewBag.newsproducts = db.NewsProducts.Where(m => m.news_id == news.news_id).ToList();
                    return View(news);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("NewsIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //thêm tin tức mới
        public ActionResult NewsCreate()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Create() == true)
                {
                    ViewBag.ListNewsCategory = new SelectList(db.ChildCategory.Where(m => (m.status == "1")).OrderBy(m => m.name),
                            "childcategory_id", "name", 0);
                    var newstagcheck = from t in db.Tags
                                       orderby t.tag_name ascending
                                       select new
                                       {
                                           t.tag_id,
                                           t.tag_name,
                                           Checked = ((from nt in db.NewsTags where (nt.tag_id == t.tag_id) select nt).Count() > 0)
                                       };
                    var newsproductcheck = from p in db.Products
                                           where p.status == "1"
                                           orderby p.product_name ascending
                                           select new
                                           {
                                               p.product_id,
                                               p.genre_id,
                                               p.product_name,
                                               p.image,
                                               Checked = ((from np in db.NewsProducts
                                                           where (np.product_id == p.product_id) && (np.genre_id == p.genre_id)
                                                           select np).Count() > 0)
                                           };
                    var MynewstagsCheckBoxList = new List<NewstagsCheckbox>();
                    var MynewsproductCheckBoxList = new List<NewsProductsCheckbox>();
                    foreach (var item in newstagcheck)
                    {
                        MynewstagsCheckBoxList.Add(new NewstagsCheckbox { id = item.tag_id, name = item.tag_name });
                    }

                    foreach (var item in newsproductcheck)
                    {
                        MynewsproductCheckBoxList.Add(new NewsProductsCheckbox
                        {
                            id = item.product_id,
                            image = item.image,
                            name = item.product_name,
                            genre_id = item.genre_id
                        });
                    }

                    var newsdto = new NewsDTO();
                    newsdto.Tags = MynewstagsCheckBoxList;
                    newsdto.Products = MynewsproductCheckBoxList;
                    return View(newsdto);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("NewsIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //code xử lý thêm tin tức
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult NewsCreate(NewsDTO newsdto, News news)
        {
            ViewBag.ListNewsCategory = new SelectList(db.ChildCategory.Where(m => (m.status == "1")).OrderBy(m => m.name), "childcategory_id", "name", 0);
            string slug = SlugGenerator.SlugGenerator.GenerateSlug(newsdto.news_slug);
            try
            {
                if (newsdto.news_slug == null)
                {
                    news.slug = SlugGenerator.SlugGenerator.GenerateSlug(newsdto.meta_title);
                }
                else
                {
                    var checkslug = db.News.Any(m => m.slug == slug);
                    if (checkslug)
                    {
                        news.slug = slug + "-" + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") + new Random().Next(1, 1000);
                    }
                    else
                    {
                        news.slug = SlugGenerator.SlugGenerator.GenerateSlug(newsdto.news_slug);
                    }
                }
                news.news_title = newsdto.news_title;
                news.news_content = newsdto.news_content;
                news.childcategory_id = newsdto.child_category_id;
                news.meta_title = newsdto.meta_title;
                news.ViewCount = 0;
                news.account_id = User.Identity.GetUserId();
                news.image = newsdto.image;
                news.image2 = newsdto.image2;
                news.create_at = DateTime.Now;
                news.status = newsdto.status;
                news.update_at = DateTime.Now;
                news.update_by = User.Identity.GetEmail();
                foreach (var item in newsdto.Tags)
                {
                    if (item.Checked)
                    {
                        db.NewsTags.Add(new NewsTags() { news_id = newsdto.news_id, tag_id = item.id });
                    }
                }
                foreach (var item in newsdto.Products)
                {
                    if (item.Checked)
                    {
                        db.NewsProducts.Add(new NewsProducts()
                        { 
                            news_id = newsdto.news_id, product_id = item.id, genre_id = item.genre_id 
                        });
                    }
                }
                db.News.Add(news);
                db.SaveChanges();
                Notification.set_flash("Thêm thành công: " + news.news_title + "", "success");
                return RedirectToAction("NewsIndex");
            }
            catch
            {
                Notification.set_flash("Lỗi", "danger");
            }
            return View(newsdto);
        }
        //Thêm nhanh thể loại
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTags(Tags tags, NewsDTO newsdto)
        {
            string slug = SlugGenerator.SlugGenerator.GenerateSlug(newsdto.tag_name);
            try
            {
                var checkslug = db.Tags.Any(m => m.slug == slug);
                if (checkslug)
                {
                    tags.slug = slug + "-" + DateTime.Now.ToString("HH") + DateTime.Now.ToString("mm") +
                                new Random().Next(1, 1000);
                }
                else
                {
                    tags.slug = SlugGenerator.SlugGenerator.GenerateSlug(newsdto.tag_name);
                }
                tags.tag_name = newsdto.tag_name;
                db.Tags.Add(tags);
                db.SaveChanges();
                Notification.set_flash("Thêm thành công: " + tags.tag_name + "", "success");
            }
            catch
            {
                Notification.set_flash("Thêm mới không thành công", "danger");
            }
            return RedirectToAction("NewsCreate");
        }
        //Sửa bài viết
        public ActionResult NewsEdit(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null &&
                Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("NewsEdit", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                News news = db.News.Where(m => m.news_id == id).FirstOrDefault();
                if ((User.Identity.Permiss_Modify() == true) && (news.status == "1" || news.status == "0"))
                {
                    if (news == null || id == null)
                    {
                        Notification.set_flash("Không tồn tại: " + news.news_title + "", "warning");
                        return RedirectToAction("NewsIndex");
                    }
                    ViewBag.ListNewsCategory =
                        new SelectList(db.ChildCategory.Where(m => (m.status == "1")).OrderBy(m => m.name),
                            "childcategory_id", "name", 0);
                    var newstagcheck = from t in db.Tags
                                       orderby t.tag_name ascending
                                       select new
                                       {
                                           t.tag_id,
                                           t.tag_name,
                                           Checked =((from nt in db.NewsTags where (nt.news_id == id) && (nt.tag_id == t.tag_id) select nt).Count() > 0)
                                       };
                    var newsproductcheck = from p in db.Products
                                           where p.status == "1"
                                           orderby p.product_name ascending
                                           select new
                                           {
                                               p.product_id,
                                               p.genre_id,
                                               p.product_name,
                                               p.image,
                                               Checked = ((from np in db.NewsProducts where (np.news_id == id) && (np.product_id == p.product_id) && (np.genre_id == p.genre_id) select np).Count() > 0)
                                           };
                    var newsdto = new NewsDTO();
                    newsdto.news_id = id.Value;
                    newsdto.counttags = news.NewsTags.Count();
                    newsdto.countproducts = news.NewsProducts.Count();
                    newsdto.child_category_id = news.childcategory_id;
                    newsdto.news_title = news.news_title;
                    newsdto.meta_title = news.meta_title;
                    newsdto.image = news.image;
                    newsdto.image2 = news.image2;
                    newsdto.news_content = news.news_content;
                    newsdto.ViewCount = news.ViewCount;
                    newsdto.status = news.status;
                    var MynewstagsCheckBoxList = new List<NewstagsCheckbox>();
                    var MynewsproductCheckBoxList = new List<NewsProductsCheckbox>();
                    foreach (var item in newstagcheck)
                    {
                        MynewstagsCheckBoxList.Add(new NewstagsCheckbox { id = item.tag_id, name = item.tag_name, Checked = item.Checked });
                    }
                    foreach (var item in newsproductcheck)
                    {
                        MynewsproductCheckBoxList.Add(new NewsProductsCheckbox
                        {
                            id = item.product_id,
                            name = item.product_name,
                            image = item.image,
                            genre_id = item.genre_id,
                            Checked = item.Checked
                        });
                    }
                    newsdto.Tags = MynewstagsCheckBoxList;
                    newsdto.Products = MynewsproductCheckBoxList;
                    return View(newsdto);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("NewsIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Code xử lý chỉnh sửa bài viết
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult NewsEdit(NewsDTO newsdto, string returnUrl)
        {
            ViewBag.ListNewsCategory = new SelectList(db.ChildCategory.Where(m => (m.status == "1")).OrderBy(m => m.name), "childcategory_id", "name", 0);
            ViewBag.products = new MultiSelectList(db.Products.ToList());
            try
            {
                var news = db.News.Find(newsdto.news_id);
                news.news_title = newsdto.news_title;
                news.news_content = newsdto.news_content;
                news.image = newsdto.image;
                news.image2 = newsdto.image2;
                news.childcategory_id = newsdto.child_category_id;
                news.meta_title = newsdto.meta_title;
                news.update_at = DateTime.Now;
                news.update_by = User.Identity.GetEmail();
                news.status = newsdto.status;
                foreach (var item in db.NewsTags)
                {
                    if (item.news_id == newsdto.news_id)
                    {
                        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }
                }

                foreach (var item in newsdto.Tags)
                {
                    if (item.Checked)
                    {
                        db.NewsTags.Add(new NewsTags() { news_id = newsdto.news_id, tag_id = item.id });
                    }
                }

                foreach (var item in db.NewsProducts)
                {
                    if (item.news_id == newsdto.news_id)
                    {
                        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }
                }

                foreach (var item in newsdto.Products)
                {
                    if (item.Checked)
                    {
                        db.NewsProducts.Add(new NewsProducts()
                            { news_id = newsdto.news_id, product_id = item.id, genre_id = item.genre_id });
                    }
                }
                db.SaveChanges();
                Notification.set_flash("Cập nhật thành thông: " + news.news_title + "", "success");
                if (!String.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("NewsIndex");
            }
            catch
            {
                Notification.set_flash("404!", "warning");
            }

            return View(newsdto);
        }
        //Xóa bài viết
        [HttpPost]
        public  JsonResult NewsDelete(int id)
        {
            bool result;
            if (User.Identity.Permiss_Delete() == true)
            {
                var news = db.News.FirstOrDefault(m => m.news_id == id);
                var newstag = db.NewsTags.FirstOrDefault(a => a.news_id == id);
                var newsproduct = db.NewsProducts.FirstOrDefault(m => m.news_id == id);
                if (news.StickyPosts.Count > 0)
                {
                    result =false;
                }
                else
                {
                    if (news.NewsTags.Count > 0)
                    {
                        db.NewsTags.Remove(newstag);
                        db.SaveChanges();
                    }

                    if (news.NewsProducts.Count > 0)
                    {
                        db.NewsProducts.Remove(newsproduct);
                        db.SaveChanges();
                    }
                    db.News.Remove(news);
                    db.SaveChanges();
                    result = true;
                    Notification.set_flash("Xóa id " + id + " thành công", "success");
                }
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Thay đổi trạng thái bài viết
        [HttpPost]
        public JsonResult ChangeStatus(int id, int state = 0)
        {
            bool result;
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                News news = db.News.Where(m => m.news_id == id).FirstOrDefault();
                int title = news.news_id;
                news.status = state.ToString();
                string prefix = state.ToString() == "1" ? "Hiển thị" : "Không hiển thị";
                news.update_at = DateTime.Now;
                news.update_by = User.Identity.GetEmail();
                db.SaveChanges();
                result = true;
                Notification.set_flash("Thay đổi trang thái sang " + prefix + " thành công", "success");
            }
            else
            {
                result = false;
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