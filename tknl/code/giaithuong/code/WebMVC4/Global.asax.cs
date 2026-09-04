using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BIZ;
using UTILS;
using ImageResizer.Plugins.DiskCache;

namespace WebMVC4
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        //protected void Application_Start()
        //{
        //    AreaRegistration.RegisterAllAreas();

        //    //WebApiConfig.Register(GlobalConfiguration.Configuration);
        //    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        //    RegisterRoutes(RouteTable.Routes);

        //    //UNCAccess UNCAccessInstance = new UNCAccess(@"\\10.9.5.165\tinmoi\", "goNews", "", "News123#@!");
        //}
        //void Application_End(object sender, EventArgs e)
        //{
        //    //  Code that runs on application shutdown
        //    WriteTotalVisited();
        //}
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception.GetType() == typeof(HttpException))
            {
                Server.Transfer("/");
            }
            else
            {
                NLogLogger.PublishException(exception);
            }

        }

        //void Session_Start(object sender, EventArgs e)
        //{
        //    // Code that runs when a new session is started
        //    Application.Lock();
        //    int n = new Random().Next(1, 2);
        //    //Application["OnlineUsers"] = (int)Application["OnlineUsers"] + n+10;
        //    if (Application["OnlineUsers"] == null)
        //        Application["OnlineUsers"] = 1;
        //    else
        //        Application["OnlineUsers"] = (int)Application["OnlineUsers"] + 1;

        //    Application["TotalVisited"] = Convert.ToInt32(Application["TotalVisited"]) + n;


        //    if (Convert.ToInt32(Application["TotalVisited"]) % 20 == 0)
        //    {
        //        Application["TotalVisited"] = Convert.ToInt32(Application["TotalVisited"]);
        //        if (Convert.ToInt32(Application["TotalVisited"]) > 10000)
        //        {
        //            WriteTotalVisited();
        //        }
        //        else
        //        {
        //            Application["TotalVisited"] = Convert.ToInt32(new SystemConfigBO().GetValueByKey("TotalVisited"));
        //        }

        //    }
        //    Application.UnLock();
        //}
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
            Response.Headers.Remove("X-Powered-By");
        }
        //void Session_End(object sender, EventArgs e)
        //{
        //    // Code that runs when a session ends. 
        //    // Note: The Session_End event is raised only when the sessionstate mode
        //    // is set to InProc in the Web.config file. If session mode is set to StateServer 
        //    // or SQLServer, the event is not raised.
        //    Application.Lock();
        //    Application["OnlineUsers"] = (int)Application["OnlineUsers"] - 1;
        //    Application.UnLock();
        //}

        //void WriteTotalVisited()
        //{
        //    try
        //    {
        //        new SystemConfigBO().SetByKey("TotalVisited", Application["TotalVisited"].ToString());

        //    }
        //    catch
        //    {

        //    }
        //}
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = true;
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


    }
}