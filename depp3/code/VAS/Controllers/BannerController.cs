using BIZ;
using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;

namespace VAS.Controllers
{
    public class BannerController : Controller
    {
        //
        // GET: /Banner/
        //[Authorize]
        public ActionResult Index(int Id = 0)
        {
            return View();
        }


        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerTop(string lang = "vi-vn")
        {
            var lstBanner = new List<DATA.Banner>();
            if (lang == "vi-vn")
                lstBanner = new BannerBO().GetTopLastestBanners(0, 101, 1);
            else
                lstBanner = new BannerBO().GetTopLastestBanners(0, 104, 1);

            return PartialView(lstBanner);
        }

        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerCenter(string lang = "vi-vn")
        {
            var lstBanner = new List<DATA.Banner>();
            if (lang == "vi-vn")
                lstBanner = new BannerBO().GetTopLastestBanners(0, 102, 1);
            else
                lstBanner = new BannerBO().GetTopLastestBanners(0, 105, 1);

            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerBotom(string lang = "vi-vn")
        {
            var lstBanner = new List<DATA.Banner>();
            if (lang == "vi-vn")
                lstBanner = new BannerBO().GetTopLastestBanners(0, 103, 1);
            else
                lstBanner = new BannerBO().GetTopLastestBanners(0, 106, 1);

            return PartialView(lstBanner);
        }
    }
}
