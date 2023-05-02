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
    public class TagsAdminController : BaseController
    {
        private DbContext db = new DbContext();
        public ActionResult TagsIndex(string search,string show, int ? size,int ? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 9);
                    var pageNumber = (page ?? 1);
                    var list = from a in db.Tags
                               orderby a.tag_id descending
                               select a;
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))//tìm kiếm tất cả
                            list = (IOrderedQueryable<Tags>)list.Where(s => s.tag_id.ToString().Contains(search) || s.tag_name.Contains(search));
                        else if (show.Equals("2"))//theo id
                            list = (IOrderedQueryable<Tags>)list.Where(s => s.tag_id.ToString().Contains(search));
                        else if (show.Equals("3"))//theo tên thể loại
                            list = (IOrderedQueryable<Tags>)list.Where(s => s.tag_name.Contains(search));
                        return View("TagsIndex", list.ToPagedList(pageNumber, 50));
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
        //Thêm tag mới
        [HttpPost]
        public JsonResult TagCreate(string name, Tags tags)
        {
            bool result;
            if (User.Identity.Permiss_Create() == true)
            {
                var checksexist = db.Tags.Any(m => m.tag_name == name);
                if (checksexist)
                {
                    result = false;
                }
                else
                {
                    tags.tag_name = name;
                    tags.slug = SlugGenerator.SlugGenerator.GenerateSlug(tags.tag_name);
                    db.Tags.Add(tags);
                    db.SaveChanges();
                    result = true;
                    Notification.set_flash("Thêm mới thành công", "success");
                }
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //sửa tag
        public JsonResult TagEdit(string name, int id)
        {
            Boolean result;
            if (User.Identity.Permiss_Modify() == true)
            {
                Tags tags = db.Tags.Find(id);
                var checksnotexist = db.Tags.Any(m => m.tag_name == name && m.tag_id != id);
                if (checksnotexist)
                {
                    result = false;
                }
                else
                {
                    tags.tag_name = name;
                    tags.slug = SlugGenerator.SlugGenerator.GenerateSlug(tags.tag_name);
                    db.SaveChanges();
                    result = true;
                    Notification.set_flash("Cập nhật tag id : "+ id + " thành công", "success");
                }
            }
            else
            {
                result = false;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //xoá tag
        [HttpPost]
        public JsonResult DeleteTag(int id)
        {
            Boolean result;
            if (User.Identity.Permiss_Delete() == true)
            {
                Tags tags = db.Tags.Find(id);
                if (tags.NewsTags.Count > 0)
                {
                    result = false;
                }
                else
                {
                    db.Tags.Remove(tags);
                    db.SaveChanges();
                    result = true;
                    Notification.set_flash("Xóa tag id " + id + " thành công", "success");
                }
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
