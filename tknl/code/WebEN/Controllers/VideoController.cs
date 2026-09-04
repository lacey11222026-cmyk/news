using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BIZ;
using BIZ.Entity;
using UTILS;

namespace WebEN.Controllers
{
    public class VideoController : Controller
    {
        //
        // GET: /Video/

        public ActionResult Index(long VideoId=0)
        {
            CONTENT_FULL newsobj;
            if (VideoId == 0)
            {
                newsobj = new ContentBO().GetTopLastestContentFulls(1, 14).FirstOrDefault();
            }
            else
            {
                newsobj = new ContentBO().GetContentFull(VideoId);
            }

            if (newsobj == null )
                return RedirectToAction("Error", "Home");

            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();


            var metaDescription = newsobj.Title + " , " + Utils.StripHtmlTag(newsobj.IntroText);
            var siteTitle = newsobj.Title + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            return View(newsobj);
        }

    }
}
