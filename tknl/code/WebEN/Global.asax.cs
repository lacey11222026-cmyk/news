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

namespace WebEN
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
            Application.Lock();
            int n = new Random().Next(1, 2);
            //Application["OnlineUsers"] = (int)Application["OnlineUsers"] + n+10;
            if (Application["OnlineUsers"] == null)
                Application["OnlineUsers"] = 1;
            else
                Application["OnlineUsers"] = (int)Application["OnlineUsers"] + 1;
            Application["TotalVisited"] = Convert.ToInt32(Application["TotalVisited"]) + n;
            Application.UnLock();

            if (Convert.ToInt32(Application["TotalVisited"]) % 20 == 0)
            {
                Application["TotalVisited"] = Convert.ToInt32(Application["TotalVisited"]);
                if (Convert.ToInt32(Application["TotalVisited"]) > 10000)
                {
                    WriteTotalVisited();
                }
                else
                {
                    Application["TotalVisited"] = Convert.ToInt32(new SystemConfigBO().GetValueByKey("TotalVisitedEN"));
                }
            }
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
            Application.Lock();
            Application["OnlineUsers"] = (int)Application["OnlineUsers"] - 1;
            Application.UnLock();
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
            SiteUrl = ConfigurationManager.AppSettings["SiteUrl"] ?? "http://localhost:64690/";
            StaticSiteUrl = ConfigurationManager.AppSettings["StaticSiteUrl"] ?? "http://localhost:64697/";
            AdminSiteUrl = ConfigurationManager.AppSettings["AdminSiteUrl"] ?? "http://localhost:64698/";



            // Code that runs on application startup
            //Application["OnlineUsers"] = 0;

            //// TotalVisited
            try
            {

                Application["TotalVisited"] = Convert.ToInt32(new SystemConfigBO().GetValueByKey("TotalVisited"));
            }
            catch
            {
                //Application["TotalVisited"] = 0;
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {


            /*Chi tiet van ban*/
            routes.MapRoute(
            "Chi tiet doc",
            "doc/d{Id}/{Name}.html", // URL with parameters
            new { controller = "Document", action = "Detail", Id = (int)1, Name = (string)null, } // Parameter defaults
           );

            /*Chi tiet album*/
            routes.MapRoute(
            "Chi tiet album",
            "thu-vien-anh/p{Id}/{Title}.html", // URL with parameters
            new { controller = "Album", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
           );
            /*List album*/
            routes.MapRoute(
            "List album",
            "thu-vien-anh/c{CategoryId}/{CateName}.html", // URL with parameters
            new { controller = "Album", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           );
            /*Chi tiet Video*/
            routes.MapRoute(
            "Chi tiet Video",
            "trang-chu-video.html", // URL with parameters
            new { controller = "Video", action = "Index", VideoId = UrlParameter.Optional } // Parameter defaults
           );
            /*Chi tiet Tin*/
            routes.MapRoute(
            "Chi tiet Tin",
            "tin-tuc/{CateName}/t{Id}/{Title}.html", // URL with parameters
            new { controller = "News", action = "Detail", Id = (long)1, Title = (string)null, CateName = (string)null } // Parameter defaults
           );
            routes.MapRoute(
           "Chi tiet Tin2",
           "tin-tuc/{CateName}/t{Id}/{Title}", // URL with parameters
           new { controller = "News", action = "Detail", Id = (long)1, Title = (string)null, CateName = (string)null } // Parameter defaults
          );
            /*List Tin*/
            routes.MapRoute(
            "List Tin",
            "tin-tuc/c{CategoryId}/{CateName}.html", // URL with parameters
            new { controller = "News", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           );
            routes.MapRoute(
           "List Tin2",
           "tin-tuc/c{CategoryId}/{CateName}", // URL with parameters
           new { controller = "News", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
          );
            /*Intro*/
            routes.MapRoute(
            "Intro",
            "gioi-thieu/c{CategoryId}/{CateName}.html", // URL with parameters
            new { controller = "Intro", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           );
            /*Intro*/
            routes.MapRoute(
            "Intro2",
            "gioi-thieu/c{CategoryId}/{CateName}", // URL with parameters
            new { controller = "Intro", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           );

            routes.MapRoute(
            "Contact", // Route name
            "lien-he-toa-soan.html", // URL with parameters
            new { controller = "StaticPage", action = "Contact" } // Parameter defaults
        );
            routes.MapRoute(
            "ViewPDF", // Route name
            "xem-online.html", // URL with parameters
            new { controller = "Document", action = "ViewPDF" } // Parameter defaults
            );
            routes.MapRoute(
            "Document", // Route name
            "van-ban.html", // URL with parameters
            new { controller = "Document", action = "Index", id = (int)22, fromdate = (string)null, todate = (string)null } // Parameter defaults
        );


            #region Giao luu truc tuyen

            routes.MapRoute(
               name: "OnlineDiscussionRegister",
               url: "giao-luu/dang-ky.html",
               defaults: new { controller = "OnlineDiscussion", action = "Register" }
            );

            routes.MapRoute(
               name: "OnlineDiscussionDetail",
               url: "giao-luu/{Title}-id{DiscussId}.html",
               defaults: new { controller = "OnlineDiscussion", action = "Details", DiscussID = 0, ReaderId = 0 }
            );

            routes.MapRoute(
                "OnlineDiscussion", // Route name
                "giao-luu-truc-tuyen.html", // URL with parameters
                new { controller = "OnlineDiscussion", action = "Index" } // Parameter defaults
            );

            #endregion

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