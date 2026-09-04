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

namespace WebMVC4
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
            // Code that runs when a new session is started
            Application.Lock();
            int n = new Random().Next(1, 2);
            //Application["OnlineUsers"] = (int)Application["OnlineUsers"] + n+10;
            if (Application["OnlineUsers"] == null)
                Application["OnlineUsers"] = 1;
            else
                Application["OnlineUsers"] = (int)Application["OnlineUsers"] + 1;

            Application["TotalVisited"] = Convert.ToInt32(Application["TotalVisited"]) + n;

            if (!Request.Url.Host.Contains("cms"))
            {
                if (Convert.ToInt32(Application["TotalVisited"]) % 5 == 0)
                {
                    Application["TotalVisited"] = Convert.ToInt32(Application["TotalVisited"]);
                    if (Convert.ToInt32(Application["TotalVisited"]) > 10000)
                    {
                        WriteTotalVisited();
                    }
                    else
                    {
                        Application["TotalVisited"] = Convert.ToInt32(new SystemConfigBO().GetValueByKey("TotalVisited"));
                    }

                }
            }
            Application.UnLock();
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

            }
            catch
            {

            }
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SiteUrl = ConfigurationManager.AppSettings["SiteUrl"] ?? "http://localhost:64696/";
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
        //protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        //{
        //    if (!Request.Url.Host.Contains("sandbox"))
        //    {


        //        HttpApplication app = sender as HttpApplication;
        //        string acceptEncoding = app.Request.Headers["Accept-Encoding"];
        //        Stream prevUncompressedStream = app.Response.Filter;

        //        if (!(app.Context.CurrentHandler is System.Web.UI.Page ||
        //            app.Context.CurrentHandler.GetType().Name == "SyncSessionlessHandler") ||
        //            app.Request["HTTP_X_MICROSOFTAJAX"] != null)
        //            return;

        //        if (acceptEncoding == null || acceptEncoding.Length == 0)
        //            return;

        //        acceptEncoding = acceptEncoding.ToLower();

        //        if (acceptEncoding.Contains("deflate") || acceptEncoding == "*")
        //        {
        //            // deflate
        //            app.Response.Filter = new DeflateStream(prevUncompressedStream,
        //                CompressionMode.Compress);
        //            app.Response.AppendHeader("Content-Encoding", "deflate");
        //        }
        //        else if (acceptEncoding.Contains("gzip"))
        //        {
        //            // gzip
        //            app.Response.Filter = new GZipStream(prevUncompressedStream,
        //                CompressionMode.Compress);
        //            app.Response.AppendHeader("Content-Encoding", "gzip");
        //        }
        //    }
        //}
        public static void RegisterRoutes(RouteCollection routes)
        {

            /*admin*/
            routes.MapRoute(
            "Admin",
            "quantrivtk", // URL with parameters
            new { controller = "Admin", action = "Index2" } // Parameter defaults
           );
            /*edit tin*/
            routes.MapRoute(
            "News Edit",
            "quan-tri/muc/tin/sua-tin", // URL with parameters
            new { controller = "AdminNews2", action = "GetNewsDetail" } // Parameter defaults
           );
            /*edit tin*/
            routes.MapRoute(
            "News Edit2",
            "quan-tri/muc/tin/sua-em-tin", // URL with parameters
            new { controller = "AdminNews2", action = "GetENewsDetail" } // Parameter defaults
           );


            /*edit intro*/
            routes.MapRoute(
            "intro Edit",
            "quan-tri/muc/intro/sua-intro", // URL with parameters
            new { controller = "AdminCategory", action = "IntroAddEdit" } // Parameter defaults
           );

            /*Chi tiet van ban*/
            routes.MapRoute(
            "Chi tiet doc",
            "doc/d{Id}/{Name}.html", // URL with parameters
            new { controller = "Document", action = "Detail", Id = (int)1, Name = (string)null, } // Parameter defaults
           );
            /*tieu diem*/
            routes.MapRoute(
            "tieu diem",
            "tieu-diem.html", // URL with parameters
            new { controller = "News", action = "Index9" } // Parameter defaults
           );
            /*Chi tiet album*/
            routes.MapRoute(
            "Chi tiet album2",
            "thu-vien-anh/p{Id}/{Title}", // URL with parameters
            new { controller = "Album", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
           );
            /*Chi tiet album*/
            routes.MapRoute(
            "Chi tiet album",
            "thu-vien-anh/p{Id}/{Title}.html", // URL with parameters
            new { controller = "Album", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
           );
            /*List album*/
            routes.MapRoute(
            "List album2",
            "thu-vien-anh/c{CategoryId}/{CateName}", // URL with parameters
            new { controller = "Album", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           );
            /*List album*/
            routes.MapRoute(
            "List album",
            "thu-vien-anh/c{CategoryId}/{CateName}.html", // URL with parameters
            new { controller = "Album", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           );
            /*Chi tiet Video*/
            routes.MapRoute(
            "Chi tiet Video2",
            "trang-chu-video", // URL with parameters
            new { controller = "Video", action = "Index", VideoId = UrlParameter.Optional } // Parameter defaults
           );
            /*Chi tiet Video*/
            routes.MapRoute(
            "Chi tiet Video",
            "trang-chu-video.html", // URL with parameters
            new { controller = "Video", action = "Index", VideoId = UrlParameter.Optional } // Parameter defaults
           );
            /*List Tin*/
            routes.MapRoute(
          "List Tin2",
          "tin-tuc/c{CategoryId}/{CateName}", // URL with parameters
          new { controller = "News", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
         );
            /*Chi tiet Tin*/
            routes.MapRoute(
            "Chi tiet Tin2",
            "tin-tuc/{CateName}/t{Id}/{Title}", // URL with parameters
            new { controller = "News", action = "Detail", Id = (long)1, Title = (string)null, CateName = (string)null } // Parameter defaults
           );
            /*Chi tiet Tin*/
            routes.MapRoute(
            "Chi tiet Tin",
            "tin-tuc/{CateName}/t{Id}/{Title}.html", // URL with parameters
            new { controller = "News", action = "Detail", Id = (long)1, Title = (string)null, CateName = (string)null } // Parameter defaults
           );
            routes.MapRoute(
         "podcastload",
         "podcast/LoadVideo", // URL with parameters
         new { controller = "Podcast", action = "LoadVideo" } // Parameter defaults
        );
            routes.MapRoute(
           "podcast",
           "podcast/{Id}", // URL with parameters
           new { controller = "Podcast", action = "Index", Id = (long)0 } // Parameter defaults
          );



            routes.MapRoute(
            "List Tin",
            "tin-tuc/c{CategoryId}/{CateName}.html", // URL with parameters
            new { controller = "News", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           );
            /*Intro*/
            routes.MapRoute(
          "Intro2",
          "gioi-thieu/c{CategoryId}/{CateName}", // URL with parameters
          new { controller = "Intro", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
         );
            routes.MapRoute(
            "Intro",
            "gioi-thieu/c{CategoryId}/{CateName}.html", // URL with parameters
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
            new { controller = "Document", action = "Index", id = (int)25, fromdate = (string)null, todate = (string)null } // Parameter defaults
        );
            routes.MapRoute(
      "to-chuc-kiem-toan-nang-luong",
      "to-chuc-kiem-toan-nang-luong", // URL with parameters
      new { controller = "Provider", action = "Index" } // Parameter defaults
     );

            routes.MapRoute(
      "to-chuc-kiem-toan-nang-luongct",
      "to-chuc-kiem-toan-nang-luong/don-vi-{Id}", // URL with parameters
      new { controller = "Provider", action = "Detail", Id = (int)0 } // Parameter defaults
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