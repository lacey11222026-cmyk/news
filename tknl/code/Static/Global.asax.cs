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
using Static.App_Start;


namespace Static
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        //public static string SiteUrl;
        //public static string AdminSiteUrl;
        //public static string StaticSiteUrl;
        public static string ImageWidth;
        public static string ImageHeight;

      
        void Application_End(object sender, EventArgs e)
        {
            
        }

        void Application_Error(object sender, EventArgs e)
        {
           
        }

        void Session_Start(object sender, EventArgs e)
        {
            
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

        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

           
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            ImageWidth = ConfigurationManager.AppSettings["ImageWidth"] ?? ",0,100,200,400,600,800,";
            ImageHeight = ConfigurationManager.AppSettings["ImageHeight"] ?? ",0,100,200,400,600,800,";
           
        }
       
        public static void RegisterRoutes(RouteCollection routes)
        {
           
         

           // routes.MapRoute(
           //    "Error", // Route name
           //    "404.html", // URL with parameters
           //    new { controller = "Home", action = "Error" } // Parameter defaults
           //);

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

    }
}