using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BIZ;
using BIZ.Entity;
using UTILS;
using WebMVC4.Models;

namespace WebMVC4.Controllers
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
                newsobj = new ContentBO().GetHotNews(14, 1).FirstOrDefault();
            }
            else
            {
                newsobj = new ContentBO().GetContentFull(VideoId);
            }

            if (newsobj == null )
                return RedirectToAction("Error", "Home");

            //var request = System.Web.HttpContext.Current.Request;
            //var mobileHelper = new MobileDetectHelper(request);
            //ViewBag.IsIpad = mobileHelper.DetectIpad();
            //ViewBag.Iphone = mobileHelper.DetectIphone();


            var metaDescription = newsobj.Title + " , " + Utils.StripHtmlTag(newsobj.IntroText);
            var siteTitle = newsobj.Title ;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle ;
            ViewBag.MainImage = newsobj.MainImage;
            Action<long, int> send = ViewAdd;
            var asynSend = send.BeginInvoke(newsobj.Id, newsobj.CategoryId.GetValueOrDefault(), null, null);
            return View(newsobj);
        }
        public ActionResult LoadVideo(int VideoId, int page)
        {
            var pagesize = 24;
            ViewBag.page = page;
            int total = 0;
            var lst = new ContentBO().GetPageContentFullsFrontend(page, pagesize, 14, ref total, "", "", "", "", "", -1);
            var Model = new NewsModel { listdata = lst, pageIndex = page, pageSize = pagesize, total = total, Id = VideoId };
            return PartialView(Model);
        }
        private void ViewAdd(long Id, int CategoryId)
        {
            new ContentBO().ViewAdd(Id, CategoryId);
        }
    }
}
