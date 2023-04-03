using System.Web.Mvc;

namespace DoAn_LapTrinhWeb.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", Controller = "Dashboard", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Admin_Product",
                "AdminProduct",
                new { action = "ProductIndex", Controller = "ProductsAdmin"}
            );
        }

    }
}