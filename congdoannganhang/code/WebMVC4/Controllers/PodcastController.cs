using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using BIZ;
using BIZ.Entity;
using UTILS;

using WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class PodcastController : Controller
    {
        //
        // GET: /Video/
      
        public ActionResult Index(int Id =0)
        {
            CONTENT_FULL newsobj;
            if (Id == 0x0)
            {
                newsobj = new ContentBO().GetHotNews(175, 1).FirstOrDefault<CONTENT_FULL>();
                return base.RedirectToAction("Index", new { Id = newsobj.Id });
            }
            newsobj = new ContentBO().GetContentFull(Id);
            if (newsobj == null)
            {
                return base.RedirectToAction("Error", "Home");
            }


            var metaDescription = newsobj.Title + " , " + Utils.StripHtmlTag(newsobj.IntroText);
            var siteTitle = newsobj.Title;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle;
            ViewBag.MainImage = newsobj.MainImage;
            Action<long, int> send = ViewAdd;
            var asynSend = send.BeginInvoke(newsobj.Id, newsobj.CategoryId.GetValueOrDefault(), null, null);
            return View(newsobj);
        }

        public ActionResult LoadVideo(int VideoId, int page)
        {
            int pageSize = 0x8;

            int totalRecords = 0;
            string lstNotId = VideoId.ToString() + ",";
            List<CONTENT_FULL> list = new ContentBO().GetPageContentFullsFrontend(page, pageSize, 175, ref totalRecords, "", "", "", lstNotId, "", -1, 0);
            NewsModel model1 = new NewsModel();
            model1.listdata = list;
            model1.pageIndex = page;
            model1.pageSize = pageSize;
            model1.total = totalRecords;
            model1.Id = VideoId;
            NewsModel model = model1;
            return base.PartialView(model);
        }
        private void ViewAdd(long Id, int CategoryId)
        {
            new ContentBO().ViewAdd(Id);
        }

    }
}
