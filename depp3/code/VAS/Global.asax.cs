using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Compression;
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
using System.IO;
using ImageResizer.Plugins.DiskCache;

namespace VAS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static string SiteUrl;
        public static string AdminSiteUrl;
        public static string StaticSiteUrl;


        //protected void Application_Start()
        //{
        //    AreaRegistration.RegisterAllAreas();

        //    //WebApiConfig.Register(GlobalConfiguration.Configuration);
        //    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        //    RegisterRoutes(RouteTable.Routes);


        //}
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
            //Application.Lock();
            //int n = new Random().Next(1, 2);
            ////Application["OnlineUsers"] = (int)Application["OnlineUsers"] + n+10;
            //if (Application["OnlineUsers"] == null)
            //    Application["OnlineUsers"] = 1;
            //else
            //    Application["OnlineUsers"] = (int)Application["OnlineUsers"] + 1;
            //Application["TotalVisited"] = Convert.ToInt32(Application["TotalVisited"]) + n;
            //Application.UnLock();

            //if (Convert.ToInt32(Application["TotalVisited"]) % 20 == 0)
            //{
            //    Application["TotalVisited"] = Convert.ToInt32(Application["TotalVisited"]);
            //    if (Convert.ToInt32(Application["TotalVisited"]) > 10000)
            //    {
            //        WriteTotalVisited();
            //    }
            //    else
            //    {
            //        Application["TotalVisited"] = Convert.ToInt32(new SystemConfigBO().GetValueByKey("TotalVisitedEN"));
            //    }
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

                new SystemConfigBO().SetByKey("TotalVisited", Application["TotalVisited"].ToString());
                //UTILS.Utils.SetAppSettingValue("TotalVisitedEN", Application["TotalVisited"].ToString(), Request.ApplicationPath);
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
            //SiteUrl = ConfigurationManager.AppSettings["SiteUrl"] ?? "http://localhost:64690/";
            //StaticSiteUrl = ConfigurationManager.AppSettings["StaticSiteUrl"] ?? "http://localhost:64697/";
            //AdminSiteUrl = ConfigurationManager.AppSettings["AdminSiteUrl"] ?? "http://localhost:64698/";

            new DiskCacheWeb().Install(ImageResizer.Configuration.Config.Current);

            // Code that runs on application startup
            //Application["OnlineUsers"] = 0;

            //// TotalVisited
            //try
            //{

            //    Application["TotalVisited"] = Convert.ToInt32(new SystemConfigBO().GetValueByKey("TotalVisited"));
            //}
            //catch
            //{
            //    //Application["TotalVisited"] = 0;
            //}
        }

        public static void RegisterRoutes(RouteCollection routes)
        {


            /*Chi tiet van ban*/
            routes.MapRoute(
          "Intro4", // Route name
          "lien-he", // URL with parameters
          new { controller = "Intro", action = "Index", CategoryId = (int)82 } // Parameter defaults
          );
            routes.MapRoute(
            "Intro3", // Route name
            "cac-nguon-tai-chinh", // URL with parameters
            new { controller = "Intro", action = "Index", CategoryId = (int)81} // Parameter defaults
            );
            routes.MapRoute(
             "Intro2", // Route name
             "don-vi-tu-van", // URL with parameters
             new { controller = "Intro", action = "Index", CategoryId = (int)80 } // Parameter defaults
             );
            routes.MapRoute(
            "Intro1", // Route name
            "gioi-thieu", // URL with parameters
            new { controller = "Intro", action = "Index", CategoryId = (int)79 } // Parameter defaults
            );


            routes.MapRoute(
           "Intro5", // Route name
           "introduction", // URL with parameters
           new { controller = "Intro", action = "Index", CategoryId = (int)84 } // Parameter defaults
           );
            routes.MapRoute(
            "Intro6", // Route name
            "consulting-unit", // URL with parameters
            new { controller = "Intro", action = "Index", CategoryId = (int)85 } // Parameter defaults
            );
            routes.MapRoute(
             "Intro7", // Route name
             "financial-sources", // URL with parameters
             new { controller = "Intro", action = "Index", CategoryId = (int)87 } // Parameter defaults
             );
            routes.MapRoute(
            "Intro8", // Route name
            "contact", // URL with parameters
            new { controller = "Intro", action = "Index", CategoryId = (int)86 } // Parameter defaults
            );

            routes.MapRoute(
               "Error", // Route name
               "404.html", // URL with parameters
               new { controller = "Home", action = "Error" } // Parameter defaults
           );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

    }
}