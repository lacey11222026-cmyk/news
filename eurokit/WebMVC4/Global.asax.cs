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
            NLogLogger.PublishException(exception);
        }


        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
            Response.Headers.Remove("X-Powered-By");
        }


       
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BundleTable.EnableOptimizations = true;
            new DiskCacheWeb().Install(ImageResizer.Configuration.Config.Current);

        }


    }
}