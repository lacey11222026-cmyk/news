using System.Web.Mvc;

namespace WebMVC4.Areas.Image2016
{
    public class Image2016AreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Image2016";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
           "Image2016_Intro",
           "cuoc-thi-sang-tac-thiep/the-le", // URL with parameters
           new { controller = "HomeAlbum", action = "Intro", CategoryId = (int)167, CateName = (string)null, } // Parameter defaults
          );
            context.MapRoute(
          "Image2016_Intro2",
          "cuoc-thi-sang-tac-thiep/gioi-thieu", // URL with parameters
          new { controller = "HomeAlbum", action = "Intro", CategoryId = (int)166, CateName = (string)null, } // Parameter defaults
         );
            context.MapRoute(
               "Image2016_homepage",
               "cuoc-thi-sang-tac-thiep/tac-pham-du-thi",
               new { controller = "HomeAlbum", action = "Index", status = 1 }
               );
            context.MapRoute(
             "Image2016_homepage2",
             "cuoc-thi-sang-tac-thiep/tac-pham-binh-chon",
             new { controller = "HomeAlbum", action = "Index", status = 2 }
             );
            context.MapRoute(
               "Image2016_homepage3",
               "cuoc-thi-sang-tac-thiep",
               new { controller = "HomeAlbum", action = "Index", status = 1 }
               );

            context.MapRoute(
                "Image2016_default",
                "Image2016/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
