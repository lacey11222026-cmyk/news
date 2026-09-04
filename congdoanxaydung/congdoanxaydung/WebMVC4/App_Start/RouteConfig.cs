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
            routes.MapRoute(
                "List Tin2",
                "tin-tuc/c{CategoryId}", // URL with parameters
                new { controller = "News", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
            );
            /*List Tin*/
            routes.MapRoute(
            "Ban Tin",
            "ban-tin.html", // URL with parameters
            new { controller = "News", action = "Index2"} // Parameter defaults
           );
            /*List Tin kenh*/
            routes.MapRoute(
            "List Tin Chanel",
            "kenh-tin/p{Id}/{Name}.html", // URL with parameters
            new { controller = "Channel", action = "Index", Id = (int)1, Name = (string)null, } // Parameter defaults
           );
            /*Intro*/
            routes.MapRoute(
            "Intro",
            "gioi-thieu/c{CategoryId}/{CateName}.html", // URL with parameters
            new { controller = "Intro", action = "Index", CategoryId = (int)1, CateName = (string)null, } // Parameter defaults
           );
            
            /*edit tin*/
            routes.MapRoute(
            "Doc",
            "van-ban.html", // URL with parameters
            new { controller = "Document", action = "Detail" } // Parameter defaults
           );
            routes.MapRoute(
                "Doc2",
                "danh-sach-van-ban.html", // URL with parameters
                new { controller = "Document", action = "Index" } // Parameter defaults
            );
            /*Chi tiet Video*/
            routes.MapRoute(
            "Chi tiet Video",
            "trang-chu-video.html", // URL with parameters
            new { controller = "Video", action = "Index", VideoId = UrlParameter.Optional } // Parameter defaults
           );
            /*Chi tiet Video*/
            routes.MapRoute(
            "Chi tiet audio",
            "trang-chu-audio.html", // URL with parameters
            new { controller = "Video", action = "Audio", VideoId = UrlParameter.Optional } // Parameter defaults
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