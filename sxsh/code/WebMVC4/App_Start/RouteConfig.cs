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
            routes.MapRoute(
           "News Edit2",
           "quan-tri/muc/tin/sua-em-tin", // URL with parameters
           new { controller = "AdminNews2", action = "GetENewsDetail" } // Parameter defaults
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
          "Chi tiet Tin3",
          "tin-tuc/t{Id}/{Title}", // URL with parameters
          new { controller = "News", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
         );
            routes.MapRoute(
                "Chi tiet Tin2",
                "tin-tuc/t{Id}", // URL with parameters
                new { controller = "News", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
            );
            /*List Tin*/
            routes.MapRoute(
           "List Tin3",
           "tin-tuc/c{CategoryId}/{CateName}", // URL with parameters
           new { controller = "News", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
          );
            routes.MapRoute(
            "List Tin",
            "tin-tuc/c{CategoryId}/{CateName}.html", // URL with parameters
            new { controller = "News", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           );
            routes.MapRoute(
                "List Tin2",
                "tin-tuc/c{CategoryId}", // URL with parameters
                new { controller = "News", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
            );
            /*note/
             *  /*Chi tiet Tin*/
            routes.MapRoute(
            "Chi tiet thong bao",
            "thong-bao/t{Id}/{Title}.html", // URL with parameters
            new { controller = "Note", action = "Detail", Id = (int)1, Title = (string)null, } // Parameter defaults
           );
           
            /*List Tin*/
            routes.MapRoute(
            "List tb",
            "thong-bao/c{CategoryId}/{CateName}.html", // URL with parameters
            new { controller = "Note", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
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
            "ket-qua-trien-khai.html", // URL with parameters
            new { controller = "Intro", action = "ResultPage" } // Parameter defaults
           );
            /*Intro*/
            routes.MapRoute(
            "Intro3",
            "faq.html", // URL with parameters
            new { controller = "Intro", action = "Question" } // Parameter defaults
           );
            /*Intro*/
            routes.MapRoute(
            "Intro4",
            "link.html", // URL with parameters
            new { controller = "Intro", action = "Link" } // Parameter defaults
           );
            /*Intro*/
            routes.MapRoute(
            "Intro5",
            "he-thong-to-chuc.html", // URL with parameters
            new { controller = "Intro", action = "Organ" } // Parameter defaults
           );
            /*Intro*/
            routes.MapRoute(
            "danhba",
            "danh-ba.html", // URL with parameters
            new { controller = "Contact", action = "Index" } // Parameter defaults
           );
            /*Intro*/
            routes.MapRoute(
            "chitietdanhba",
            "chi-tiet-danh-ba.html", // URL with parameters
            new { controller = "Contact", action = "Detail" } // Parameter defaults
           );
            /*Intro*/
            routes.MapRoute(
            "chitietdanhba2",
            "chi-tiet-to-chuc.html", // URL with parameters
            new { controller = "Contact", action = "OrganDetail" } // Parameter defaults
           );
            /*Intro*/
            routes.MapRoute(
            "danhbatochuc",
            "danh-ba-to-chuc.html", // URL with parameters
            new { controller = "Contact", action = "Organ" } // Parameter defaults
           );
            /*tieu diem*/
            routes.MapRoute(
            "tieu diem",
            "tieu-diem.html", // URL with parameters
            new { controller = "News", action = "Index9" } // Parameter defaults
           );
            routes.MapRoute(
          "podcast",
          "podcast/{Id}", // URL with parameters
          new { controller = "Podcast", action = "Index", Id = (int)0 } // Parameter defaults
         );
            /*Chi tiet Video*/
            routes.MapRoute(
            "Chi tiet Video",
            "trang-chu-video.html", // URL with parameters
            new { controller = "Video", action = "Index", VideoId = UrlParameter.Optional } // Parameter defaults
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