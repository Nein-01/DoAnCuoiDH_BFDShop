using DoAn_LapTrinhWeb.Common;
using DoAn_LapTrinhWeb.Common.Helpers;
using System.Web.Mvc;
using System.Web.Security;
namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{

    //[Authorize(Roles = Const.ROLE_ADMIN_NAME)]  // Chỉ chấp nhận account đã đăng nhập và có role admin
    //Controller trung gian giữa Admin và User
    public class BaseController : Controller
    {
        public BaseController()
        {
            //nếu không user không thuộc những roles trong đây thì sẽ quay về trang chủ, những role nằm trong file "\Const.cs"  thuộc folder "Common\Const.cs"
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                System.Web.HttpContext.Current.Response.Redirect("~/Account/SignIn");
            }
            else
            {
                if (System.Web.HttpContext.Current.User.Identity.GetRole()==1)
                {
                    System.Web.HttpContext.Current.Response.Redirect("~/Home/Index");
                }
            }
        }
        //Đăng xuất admin quay về trang chủ
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Notification.set_flash("Đăng xuất thành công", "success");
            return Redirect("~/Home/Index");
        }
        //Chuyển từ trang admin về trang thông tin cá nhân
        public ActionResult ViewProfile()
        {
            return Redirect("~/Account/Editprofile");
        }
        //Chuyển từ trang admin về trang chủ
        public ActionResult BackToHome()
        {
            return Redirect("~/Home/Index");
        }
    }
}