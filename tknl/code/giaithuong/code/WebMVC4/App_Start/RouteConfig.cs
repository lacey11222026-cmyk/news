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
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

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

            /*Chi tiet album*/
            routes.MapRoute(
            "Chi tiet album",
            "thu-vien-anh/p{Id}/{Title}.html", // URL with parameters
            new { controller = "Album", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
           );
            routes.MapRoute(
                "Chi tiet album2",
                "thu-vien-anh/p{Id}", // URL with parameters
                new { controller = "Album", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
            );
            /*List album*/
            routes.MapRoute(
            "List album",
            "thu-vien-anh/c{CategoryId}/{CateName}.html", // URL with parameters
            new { controller = "Album", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           );
            routes.MapRoute(
                "List album2",
                "thu-vien-anh/c{CategoryId}", // URL with parameters
                new { controller = "Album", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
            );
            /*Chi tiet Tin*/
            routes.MapRoute(
            "Chi tiet Tin",
            "tin-tuc/t{Id}/{Title}.html", // URL with parameters
            new { controller = "News", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
           );
            routes.MapRoute(
                "Chi tiet Tin2",
                "tin-tuc/t{Id}", // URL with parameters
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
            routes.MapRoute(
               "Chi tiet san pham",
               "tu-lanh-{Id}", // URL with parameters
               new { controller = "Product", action = "Detail", Id = (int)1, Title = (string)null, CateName = (string)null } // Parameter defaults
           );
            routes.MapRoute(
              "Chi tiet san pham1",
              "dieu-hoa-khong-khi-{Id}", // URL with parameters
              new { controller = "Product", action = "Detail", Id = (int)1, Title = (string)null, CateName = (string)null } // Parameter defaults
          );
            routes.MapRoute(
              "Chi tiet san pham2",
              "may-giat-{Id}", // URL with parameters
              new { controller = "Product", action = "Detail", Id = (int)1, Title = (string)null, CateName = (string)null } // Parameter defaults
          );
            routes.MapRoute(
              "Chi tiet san pham3",
              "den-led-{Id}", // URL with parameters
              new { controller = "Product", action = "Detail", Id = (int)1, Title = (string)null, CateName = (string)null } // Parameter defaults
          );
            routes.MapRoute(
             "Chi tiet san pham4",
             "san-pham-khac-{Id}", // URL with parameters
             new { controller = "Product", action = "Detail", Id = (int)1, Title = (string)null, CateName = (string)null } // Parameter defaults
         );
            routes.MapRoute(
            "Chi tiet san pham5",
            "binh-nuoc-nong-{Id}", // URL with parameters
            new { controller = "Product", action = "Detail", Id = (int)1, Title = (string)null, CateName = (string)null } // Parameter defaults
        );
            /*Danh sach*/
            routes.MapRoute(
            "dieu hoa",
            "dieu-hoa-khong-khi", // URL with parameters
            new { controller = "Product", action = "Index", CategoryId = (int)38 } 
           );
            /*Danh sach*/
            routes.MapRoute(
            "tu lanh",
            "tu-lanh", // URL with parameters
            new { controller = "Product", action = "Index", CategoryId = (int)39 }
           );
            
            /*Danh sach*/
            routes.MapRoute(
            "mat giat",
            "may-giat", // URL with parameters
            new { controller = "Product", action = "Index", CategoryId = (int)40 }
           );
            /*Danh sach*/
            routes.MapRoute(
            "denled",
            "den-led", // URL with parameters
            new { controller = "Product", action = "Index", CategoryId = (int)41 }
           );
            /*Danh sach*/
            routes.MapRoute(
            "binh nuoc nong",
            "binh-nuoc-nong", // URL with parameters
            new { controller = "Product", action = "Index", CategoryId = (int)43 }
           );
            /*Danh sach*/
            routes.MapRoute(
            "san pham khac",
            "san-pham-khac", // URL with parameters
            new { controller = "Product", action = "Index", CategoryId = (int)15 }
           );
            routes.IgnoreRoute("{*apple}", new { apple = @"(.*/)?apple-touch-icon.*\.png(/.*)?" });
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