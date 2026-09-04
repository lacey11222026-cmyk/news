using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BIZ;
using BIZ.Entity;
using Constants = UTILS.Constants;
using UTILS;

namespace CMS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static string SiteUrl;
        public static string AdminSiteUrl;
        public static string StaticSiteUrl;


       
        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
            //WriteTotalVisited();
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            // HttpContext ctx = HttpContext.Current;

            //Exception ex = ctx.Server.GetLastError();
            //ExHandler.Handle(ex, "Application_Error", "Application_Error");
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            //Application.Lock();
            //int n = new Random().Next(1, 2);
            ////Application["OnlineUsers"] = (int)Application["OnlineUsers"] + n+10;
            //if (Application["OnlineUsers"] == null)
            //    Application["OnlineUsers"] = 1;
            //else
            //    Application["OnlineUsers"] = (int)Application["OnlineUsers"] + 1;
            //Application["TotalVisited"] = Convert.ToInt32(Application["TotalVisited"]) + n;
            //Application.UnLock();

            //if (Convert.ToInt32(Application["TotalVisited"]) % 10 == 0)
            //{
            //    Application["TotalVisited"] = Convert.ToInt32(Application["TotalVisited"]);
            //    WriteTotalVisited();
            //}
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
            //Application.Lock();
            //Application["OnlineUsers"] = (int)Application["OnlineUsers"] - 1;
            //Application.UnLock();
        }

        void WriteTotalVisited()
        {
            try
            {

                UTILS.Utils.SetAppSettingValue("TotalVisited", Application["TotalVisited"].ToString(), Request.ApplicationPath);
            }
            catch
            {

            }
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SiteUrl = ConfigurationManager.AppSettings["SiteUrl"] ?? "http://localhost:64696/";
            StaticSiteUrl = ConfigurationManager.AppSettings["StaticSiteUrl"] ?? "http://localhost:64697/";
            AdminSiteUrl = ConfigurationManager.AppSettings["AdminSiteUrl"] ?? "http://localhost:64698/";


        }
        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            // Check for culture type of cachine
            if (custom == "culture")
            {
                // culture name (e.g. "en-US") is what should vary caching
                return Thread.CurrentThread.CurrentCulture.Name;
            }
            else
                return base.GetVaryByCustomString(context, custom);
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            /*admin*/
            /*edit tin*/
            routes.MapRoute(
            "News Edit",
            "quan-tri-tin/sua-tin/", // URL with parameters
            new { controller = "AdminNews2", action = "Edit" } // Parameter defaults
           );


            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Admin", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

    }
}