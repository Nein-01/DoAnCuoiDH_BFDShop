using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Hosting;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using DoAn_LapTrinhWeb.Models;
using PagedList;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class ContactsController : BaseController
    {
        private readonly DbContext _db = new DbContext();
        public ActionResult ContactIndex(string search, string show, int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 10);
                    var pageNumber = (page ?? 1);
                    ViewBag.countTrash = _db.Contacts.Count(a => a.status == "2");
                    var list = from a in _db.Contacts
                               where (a.status == "1" || a.status == "0")
                               orderby a.contact_id descending
                               select a;
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))//tìm kiếm tất cả
                            list = (IOrderedQueryable<Contact>)list.Where(s => s.contact_id.ToString().Contains(search) || s.name.Contains(search) || s.email.Contains(search) || s.phone.ToString().Contains(search));
                        else if (show.Equals("2"))//theo id
                            list = (IOrderedQueryable<Contact>)list.Where(s => s.contact_id.ToString().Contains(search));
                        else if (show.Equals("3"))//theo tên 
                            list = (IOrderedQueryable<Contact>)list.Where(s => s.name.Contains(search));
                        else if (show.Equals("4"))//theo email
                            list = (IOrderedQueryable<Contact>)list.Where(s => s.email.Contains(search));
                        else if (show.Equals("5"))//theo phone
                            list = (IOrderedQueryable<Contact>)list.Where(s => s.phone.ToString().Contains(search));
                        return View("ContactIndex", list.ToPagedList(pageNumber, 50));
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
        //View list trash liên hệ khách hàng
        public ActionResult ContactTrash(string search, string show, int? size, int? page)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Access() == true || User.Identity.Permiss_Create() == true || User.Identity.Permiss_Update() == true ||
                User.Identity.Permiss_Modify() == true || User.Identity.Permiss_Delete() == true)
                {
                    var pageSize = (size ?? 10);
                    var pageNumber = (page ?? 1);
                    var list = from a in _db.Contacts
                               where a.status == "2"
                               orderby a.update_at descending
                               select a;
                    if (!string.IsNullOrEmpty(search))
                    {
                        if (show.Equals("1"))//tìm kiếm tất cả
                            list = (IOrderedQueryable<Contact>)list.Where(s => s.contact_id.ToString().Contains(search) || s.name.Contains(search) || s.email.Contains(search) || s.phone.ToString().Contains(search));
                        else if (show.Equals("2"))//theo id
                            list = (IOrderedQueryable<Contact>)list.Where(s => s.contact_id.ToString().Contains(search));
                        else if (show.Equals("3"))//theo tên
                            list = (IOrderedQueryable<Contact>)list.Where(s => s.name.Contains(search));
                        else if (show.Equals("4"))//theo email
                            list = (IOrderedQueryable<Contact>)list.Where(s => s.email.Contains(search));
                        else if (show.Equals("5"))//theo phone
                            list = (IOrderedQueryable<Contact>)list.Where(s => s.phone.ToString().Contains(search));
                        return View("ContactTrash", list.ToPagedList(pageNumber, 50));
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
        //View trả lời khách hàng
        public ActionResult Reply(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.Permiss_Modify() == true)
                {
                    var contact = _db.Contacts.SingleOrDefault(a => a.contact_id == id);
                    if (contact == null || id == null)
                    {
                        Notification.set_flash("Không tồn tại liên hệ từ khách hàng!", "danger");
                        return RedirectToAction("ContactIndex");
                    }
                    return View(contact);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("ContactIndex");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Code xử lý trả lời liên hệ khách hàng
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Reply(Contact contact,string Contact_id, string UserEmail,string Content,string Reply,string Update_by,string Roles )
        {
            try
            {
                contact.reply = contact.reply;
                contact.content = contact.content;
                contact.flag = 1;
                contact.update_at = DateTime.Now;
                contact.update_by = User.Identity.GetEmail();
                Contact_id = contact.contact_id.ToString();
                UserEmail = contact.email;
                Content = contact.content;
                Reply = contact.reply;
                Update_by= User.Identity.GetName();
                if (User.Identity.GetRole() == 4)
                {
                    Roles = "Quản trị viên";
                }
                else
                {
                    Roles = "Biên tập viên";
                }

                SendEmailReply(Contact_id, UserEmail, Content, Reply , Update_by,Roles);
                Notification.set_flash("Đã trả lời liên hệ!", "success");
                _db.Entry(contact).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("ContactIndex");
            }
            catch
            {
                Notification.set_flash("Lỗi", "danger");
            }
            return View(contact);
        }
        //Gửi mail sau khi trả lời câu hỏi khách hàng
        public void SendEmailReply(string Contact_id, string UserEmail, string Content, string Reply, string Update_by, string Roles)
        {
            ///để dùng google email gửi email reset cho người khác bạn cần phải vô đây "https://www.google.com/settings/security/lesssecureapps" Cho phép ứng dụng kém an toàn: Bật
            var fromEmail = new MailAddress(AccountEmail.UserEmailSupport, AccountEmail.Name); // "username email-vd: vn123@gmail.com" ,"tên hiển thị mail khi gửi"
            var toEmail = new MailAddress(UserEmail);
            //nhập password của bạn
            var fromEmailPassword = AccountEmail.Password;
            string subject = "";
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "MailContact" + ".cshtml"); //dùng body mail html , file template nằm trong thư mục "EmailTemplate/Text.cshtml"
            subject = "Quý khách có câu trả lời từ https://bfd.vn/ [LH" + Contact_id + "]";
            body = body.Replace("{{ViewBag.contact_id}}", "LH"+Contact_id);
            body = body.Replace("{{ViewBag.contact_content}}", Content);
            body = body.Replace("{{ViewBag.contact_reply}}", Reply);
            body = body.Replace("{{ViewBag.Name}}", Update_by);
            body = body.Replace("{{ViewBag.Roles}}", Roles);
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
        //Hủy liên lạc
        public ActionResult DelTrash(int? id) //bỏ sp vào thùng rác
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                var contact = _db.Contacts.SingleOrDefault(a => a.contact_id == id);
                if (contact == null || id == null)
                {
                    Notification.set_flash("Không tồn tại liên hệ: " + "LH" + id + "", "warning");
                    return RedirectToAction("Index");
                }
                contact.status = "2";
                contact.update_at = DateTime.Now;
                contact.update_by = User.Identity.GetEmail();
                _db.Entry(contact).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Đã chuyển liên hệ: " + "LH" + id + " vào thùng rác!", "success");
                return RedirectToAction("ContactIndex");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("ContactIndex");
            }
        }
        //Khôi phục liên lạc từ thùng rác
        public ActionResult Undo(int? id) // khôi phục từ thùng rác
        {
            if (User.Identity.Permiss_Update() == true || User.Identity.Permiss_Modify() == true)
            {
                var contact = _db.Contacts.SingleOrDefault(a => a.contact_id == id);
                if (contact == null || id == null)
                {
                    Notification.set_flash("Không tồn tại liên hệ: " + "LH" + id + "", "warning");
                    return RedirectToAction("Index");
                }
                contact.status = "1";
                contact.update_at = DateTime.Now;
                contact.update_by = User.Identity.GetEmail();
                _db.Entry(contact).State = EntityState.Modified;
                _db.SaveChanges();
                Notification.set_flash("Khôi phục thành công liên hệ: " + "LH" + id + "", "success");
                return RedirectToAction("ContactTrash");
            }
            else
            {
                Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                return RedirectToAction("ContactTrash");
            }
        }
        //Xóa liên hệ
        public ActionResult ContactDelete(int? id, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
                {
                    return RedirectToAction("ContactDelete", new { returnUrl = Request.UrlReferrer.ToString() });
                }
                if (User.Identity.Permiss_Delete() == true)
                {
                    var contact = _db.Contacts.SingleOrDefault(a => a.contact_id == id);
                    if (contact == null)
                    {
                        Notification.set_flash("Không tồn tại liên hệ: " + "LH" + contact.contact_id + "", "warning");
                        return RedirectToAction("ContactTrash");
                    }
                    return View(contact);
                }
                else
                {
                    Notification.set_flash("Bạn không có quyền sử dụng chức năng này", "danger");
                    return RedirectToAction("ContactTrash");
                }
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        //Xác nhận xóa vĩnh viễn liên hệ khách hàng
        [HttpPost]
        [ActionName("ContactDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            var contact = _db.Contacts.SingleOrDefault(a => a.contact_id == id);
            _db.Contacts.Remove(contact);
            _db.SaveChanges();
            Notification.set_flash("Đã xoá vĩnh viễn liên hệ: " + "LH" + id + "", "success");
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("ContactTrash");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}