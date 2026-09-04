using BIZ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebMVC4
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var lstCarModel = new CarModelBO().GetTopLastestCarModel();
            var ListCate = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.None).Where(x => x.Published == 1).ToList();
            //var ListSize = new CarSizeBO().GetTopLastestCarSize(-1, -1, 1);
            //var ListManu = new ManufactoryBO().GetAllManufactoryFulls(-1, -1, 1);
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region "CMS"
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




            #region "tin tuc"
          
            foreach (var item in ListCate.Where(x => x.Type == 1))
            {
                routes.MapRoute(
                   item.Name + item.Id,
                   item.Link, // URL with parameters
                    new { controller = "Intro", action = "Index", CategoryId = (int)item.Id, }
                  );
            }
            foreach (var item in ListCate.Where(x => x.Type == 2))
            {
                routes.MapRoute(
                   item.Name + item.Id,
                    item.Link, // URL with parameters
                    new { controller = "News", action = "Index", CategoryId = (int)item.Id, }
                  );
            }
            routes.MapRoute(
           "Chi tiet Tin",
           "blogs/news/{url}", // URL with parameters
           new { controller = "News", action = "Detail", url = (string)null, } // Parameter defaults
          );
            
           
            #endregion

            #region "san pham"
            foreach (var item in ListCate.Where(x => x.Type == 0))
            {
                routes.MapRoute(
                   item.Name + item.Id,
                   "collections/" + item.Link, // URL with parameters
                    new { controller = "Product", action = "Index", CategoryId = (int)item.Id, }
                  );
            }
            //foreach (var item in ListManu)
            //{
            //    routes.MapRoute(
            //       item.Website + item.Id,
            //       item.Website, // URL with parameters
            //        new { controller = "Product", action = "Index", CategoryId = item.CategoryId, ManuId=item.Id }
            //      );
            //}
            //foreach (var item in ListSize.Where(x=>x.CategoryId==2))
            //{
            //    routes.MapRoute(
            //       item.Url + item.Id,
            //       item.Url, // URL with parameters
            //        new { controller = "Product", action = "Index", CategoryId = item.CategoryId, v = item.Id }
            //      );
            //}
            //foreach (var item in ListSize.Where(x => x.CategoryId == 8))
            //{
            //    routes.MapRoute(
            //       item.Url + item.Id,
            //       item.Url, // URL with parameters
            //        new { controller = "Product", action = "Index", CategoryId = item.CategoryId, size = item.Id }
            //      );
            //}
            routes.MapRoute(
            "Chi tiet Sp url",
            "products/{url}", // URL with parameters
            new { controller = "Product", action = "Detail", url = (string)null, } // Parameter defaults
            );
            #endregion
            #region "Chi tiet xe"
            foreach (var item in lstCarModel)
            {
                routes.MapRoute(
                   item.Name+ item.Id,
                   item.Url, // URL with parameters
                   new { controller = "Product", action = "CarDetail", Id = (int)item.Id } // Parameter defaults
                  );
            }
            #endregion



            /*Shop*/
            routes.MapRoute(
            "Shop",
            "lien-he", // URL with parameters
            new { controller = "Contact", action = "Index" } // Parameter defaults
           );
            routes.MapRoute(
            "search",
            "search", // URL with parameters
            new { controller = "Product", action = "Search" } // Parameter defaults
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