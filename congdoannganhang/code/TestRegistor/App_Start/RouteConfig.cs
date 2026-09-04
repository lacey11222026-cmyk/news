using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestRegistor
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            /*Chi tiet Tin*/
            routes.MapRoute(
            name: "Chi tiet Tin",
            url: "tin-tuc/t{Id}/{Title}", // URL with parameters
            defaults: new { controller = "News", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
           );
            /*List Tin*/
            routes.MapRoute(
            name: "List Tin",
            url: "tin-tuc/c{CategoryId}/{CateName}", // URL with parameters
            defaults: new { controller = "News", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
