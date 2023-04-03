using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;


namespace DoAn_LapTrinhWeb
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ConfigLogging();
            //dòng này được thêm vào để sử dụng messages toastr xem  file notification.cs
            Application["Notification"] = "";
        }
        protected void Session_Start()
        {
            Application.Lock();
            Application.UnLock();
        }
        protected void Session_End()
        {
            Application.Lock();
            Application.UnLock();
        }
        public void ConfigLogging()
        {
            //  string sLogFile = HttpRuntime.AppDomainAppPath + "log4net.config";
            string sLogFile = HttpContext.Current.Server.MapPath("~/log4net.config");
            if ((System.IO.File.Exists(sLogFile)))
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(sLogFile));
            }
        }

    }
}