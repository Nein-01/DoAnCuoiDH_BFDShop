using DoAn_LapTrinhWeb.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Threading.Tasks;
using DoAn_LapTrinhWeb; 

[assembly: OwinStartup(typeof(DoAn_LapTrinhWeb.App_Start.Startup))]
namespace DoAn_LapTrinhWeb.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
