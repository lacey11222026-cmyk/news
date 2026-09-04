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
            //routes.MapRoute(
            //    "Chi tiet Tin2",
            //    "tin-tuc/t{Id}", // URL with parameters
            //    new { controller = "News", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
            //);
            /*List Tin*/
            routes.MapRoute(
            "List Tin",
            "tin-tuc/c{CategoryId}/{CateName}.html", // URL with parameters
            new { controller = "News", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           );
            /*Chi tiet Video*/
            routes.MapRoute(
            "Chi tiet Video",
            "trang-chu-video.html", // URL with parameters
            new { controller = "Video", action = "Index", VideoId = UrlParameter.Optional } // Parameter defaults
           );
            routes.MapRoute(
                "List Tin2",
                "tin-tuc/c{CategoryId}", // URL with parameters
                new { controller = "News", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
            );
            /*List Tin*/
            routes.MapRoute(
            "thanh-vien-thi-truong",
            "thanh-vien-thi-truong", // URL with parameters
            new { controller = "NhaMay", action = "Index" } // Parameter defaults
           );
            routes.MapRoute(
           "Chi tiet tv",
           "thong-tin-nha-may/{Title}-{Id}.html", // URL with parameters
           new { controller = "NhaMay", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
          );
            /*List Tin*/
            /*document*/
            routes.MapRoute(
            "van-ban-phap-luat",
            "van-ban-phap-luat", // URL with parameters
            new { controller = "Document", action = "Index", Id = (int)69 } // Parameter defaults
           );
          
            routes.MapRoute(
            "van-ban-QPPL-nganh-dien",
            "van-ban-QPPL-nganh-dien", // URL with parameters
            new { controller = "Document", action = "Index", Id = (int)81 } // Parameter defaults
           );
            routes.MapRoute(
            "du-thao-lay-y-kien-gop-y",
            "du-thao-lay-y-kien-gop-y", // URL with parameters
            new { controller = "Document", action = "Index", Id = (int)83 } // Parameter defaults
           );
            routes.MapRoute(
          "van-ban-lien-quan",
          "van-ban-lien-quan", // URL with parameters
          new { controller = "Document", action = "Index", Id = (int)82 } // Parameter defaults
         );

            routes.MapRoute(
            "quan-ly-cap-phep",
            "quan-ly-cap-phep", // URL with parameters
            new { controller = "Document2", action = "Index", Id = (int)68 } // Parameter defaults
           );
            routes.MapRoute(
              "Chi tiet van ban",
              "van-ban/{Title}-{Id}.html", // URL with parameters
              new { controller = "Document", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
             );
            routes.MapRoute(
             "Chi tiet giay phep",
             "giay-phep/{Title}-{Id}.html", // URL with parameters
             new { controller = "Document2", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
            );
            /*Intro*/
            routes.MapRoute(
            "Intro",
            "gioi-thieu/c{CategoryId}/{CateName}.html", // URL with parameters
            new { controller = "Intro", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           );
            /*media*/
            routes.MapRoute(
            "Media",
            "lien-he", // URL with parameters
            new { controller = "Home", action = "Contact" } // Parameter defaults
           );
            routes.IgnoreRoute("{*apple}", new { apple = @"(.*/)?apple-touch-icon.*\.png(/.*)?" });
            routes.MapRoute(
               "Error", // Route name
               "404.html", // URL with parameters
               new { controller = "Home", action = "Error" } // Parameter defaults
           );

            routes.MapRoute(
          "bannerright",
          "tintuc/t2575/bannerright3.html", // URL with parameters
          new { controller = "Home", action = "BannerRight3" } // Parameter defaults
         );
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }
    }
}