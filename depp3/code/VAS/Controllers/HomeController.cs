using System.IO;
using System.Net;
using BIZ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATA;
using UTILS;
using  VAS.Helper;
using BIZ.Entity;
using VAS.Models;
using VAS.Filter;

namespace VAS.Controllers
{
    public class HomeController : Controller
    {
        [LocalizationActionFilter]
        public ActionResult Index()
        {
            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = Resources.Global.SiteTitle;
            ViewBag.Title = Resources.Global.SiteTitle;
            //var request = System.Web.HttpContext.Current.Request;
            //var mobileHelper = new MobileDetectHelper(request);

            //ViewBag.IsMobile = mobileHelper.DetectMobileLong();

           
           
            return View();

        }
        [LocalizationActionFilter]
        public ActionResult Index2(string lang)
        {
            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = Resources.Global.SiteTitle;
            ViewBag.Title = Resources.Global.SiteTitle;
            //var request = System.Web.HttpContext.Current.Request;
            //var mobileHelper = new MobileDetectHelper(request);

            //ViewBag.IsMobile = mobileHelper.DetectMobileLong();

            WorkContext.SetLanguage(lang);

            return View();

        }
        public ActionResult Language(string lang)
        {
            WorkContext.SetLanguage(lang);

            return RedirectToAction("Index");
        }
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult TopMission(string lang = "vi-vn")
        {
            var lstBanner = new List<MISSION_FULL>();

            lstBanner = new MissionBO().GetTopLastestMissionsFull(16, -1);

            lstBanner = lstBanner.Where(x => x.Code == lang).ToList();

            return PartialView(lstBanner);
        }
        public ActionResult Error()
        {
            var requestpage = HttpUtility.UrlDecode(Request.ServerVariables["QUERY_STRING"].Replace("404;", ""));
            ViewBag.requestpage = requestpage;
            

          
            //var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);
            return View();
        }
       
        
    }
}
