using System;
using System.Linq;
using System.Web.Mvc;
using DoAn_LapTrinhWeb.DTOs;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly DbContext db = new DbContext();
        //View thống kê 
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
               
                return View();
            }
            else
            {
                return Redirect("~/Account/SignIn");
            }
        }
        
    }
}