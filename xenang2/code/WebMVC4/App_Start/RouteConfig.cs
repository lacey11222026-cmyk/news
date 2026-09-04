using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BIZ;
using UTILS;

namespace WebMVC4
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var ListCate = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.Product).Where(x => x.Published == 1).ToList();
            var LstManu = new ManufactoryBO().GetAllManufactoryFulls(-1);
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*admin*/
            routes.MapRoute(
            "Admin",
            "quantri", // URL with parameters
            new { controller = "Admin", action = "Index2" } // Parameter defaults
           );
            /*edit tin*/
            routes.MapRoute(
            "News Edit",
            "quan-tri/muc/tin/sua-tin", // URL with parameters
            new { controller = "AdminNews2", action = "GetNewsDetail" } // Parameter defaults
           );
            /*edit album*/
            routes.MapRoute(
            "album Edit",
            "quan-tri/muc/album/sua-album", // URL with parameters
            new { controller = "AdminAlbum", action = "GetAlbumDetail" } // Parameter defaults
           );
            /*edit intro*/
            routes.MapRoute(
            "intro Edit",
            "quan-tri/muc/intro/sua-intro", // URL with parameters
            new { controller = "AdminCategory", action = "IntroAddEdit" } // Parameter defaults
           );

            
            #region "san pham"
            /*Chi tiet sp*/
            routes.MapRoute(
            "Chi tiet SP",
            "san-pham/p{Id}/{Title}.html", // URL with parameters
            new { controller = "Product", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
           );

            /*List sp*/
            foreach (var item in ListCate.Where(x => !string.IsNullOrEmpty(x.Link)))
            {
                var link= item.Link.Remove(0, 1);
                //NLogLogger.DebugMessage(link);
                foreach (var manu in LstManu)
                {
                    //NLogLogger.DebugMessage(link + "-" + manu.Image);
                    routes.MapRoute(
                   link + item.Id+manu.Id,
                   link+"-"+manu.Image, // URL with parameters
                    new { controller = "Product", action = "Index", CategoryId = (int)item.Id, ManuId=(int)manu.Id }
                  );
                }    
                routes.MapRoute(
                   link + item.Id,
                   link, // URL with parameters
                    new { controller = "Product", action = "Index", CategoryId = (int)item.Id, }
                  );
            }
           // /*List sp*/
           // routes.MapRoute(
           // "List SP",
           // "san-pham/c{CategoryId}/{CateName}.html", // URL with parameters
           // new { controller = "Product", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           //);
            #endregion
            #region "gio hang"
            routes.MapRoute(
             name: "ShoppingCart",
             url: "thong-tin-dat-hang/",
             defaults: new { controller = "Order", action = "ShoppingCart" }
         );
            routes.MapRoute(
                 name: "OrderInfo",
                 url: "thong-tin-giao-dich/",
                 defaults: new { controller = "Order", action = "Info" }
             );

            routes.MapRoute(
             name: "OrderConfirmation",
             url: "xac-nhan-dat-hang/",
             defaults: new { controller = "Order", action = "OrderConfirmation" }
         );
            #endregion
            /*List album*/
            routes.MapRoute(
            "List doc2",
            "tai-lieu.html", // URL with parameters
            new { controller = "Document", action = "Index", Id = (int)93, CateName = (string)null, } // Parameter defaults
           );
            routes.MapRoute(
                "Chi tiet album2",
                "cong-trinh/p{Id}/{Title}", // URL with parameters
                new { controller = "Album", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
            );
            /*List album*/
            routes.MapRoute(
            "List album",
            "cong-trinh", // URL with parameters
            new { controller = "Album", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           );
            /*Chi tiet Tin*/
            routes.MapRoute(
            "Chi tiet Tin",
            "tin-tuc/t{Id}/{Title}.html", // URL with parameters
            new { controller = "News", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
           );
            /*List Tin*/
            routes.MapRoute(
            "List Tin",
            "tin-tuc/c{CategoryId}/{CateName}.html", // URL with parameters
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
            "Contact",
            "lien-he", // URL with parameters
            new { controller = "Contact", action = "Index" } // Parameter defaults
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