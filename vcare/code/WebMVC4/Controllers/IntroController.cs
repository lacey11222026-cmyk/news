using BIZ;
using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Filter;

namespace WebMVC4.Controllers
{
    
    public class IntroController : Controller
    {
        //
        // GET: /Intro/
        [LocalizationActionFilter]
        public ActionResult Index(int CategoryId)
        {
            var intro = new CategoryBO().GetCategoryFull(CategoryId);
            if (intro == null)
                return RedirectToAction("Error", "Home");
          
            var metaDescription = Utils.StripHtmlTag(intro.Description);
            var siteTitle = intro.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.CurrentCategoryId = intro.Id;
            ViewBag.ParentCategoryId = intro.ParentId;

            
           
            
            ViewBag.SiteDescription = metaDescription + " " + Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.SiteTitle = siteTitle + Resources.Global.SiteTitle;

            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            return View(intro);
        }
        [ChildActionOnly]
        public ActionResult Relate(List<CATEGORY_FULL> data)
        {
            data = data.Where(x => x.Type == 1).ToList();
            return PartialView(data);
        }
        [ChildActionOnly]
        public ActionResult SystemPage()
        {

            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult OrganPage()
        {

            return PartialView();
        }
    }
}
