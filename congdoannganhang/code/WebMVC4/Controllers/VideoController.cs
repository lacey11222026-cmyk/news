using BIZ;
using BIZ.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Models;
namespace WebMVC4.Controllers
{
    public class VideoController : Controller
    {
        //
        // GET: /Video/

        public ActionResult Index(int VideoId = 0)
        {
            var model = new CONTENT_FULL();
            if (VideoId == 0)
            {
                model = new ContentBO().GetTopLastestContentFulls(1, 110).FirstOrDefault();
            }
            else
            {
                model = new ContentBO().GetContentFull(VideoId);
            }
            var metaDescription = model.Title + Utils.StripHtmlTag(model.IntroText);
            var siteTitle = model.Title + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            Action<long> addview = ViewAdd;
            var asynSendNoti = addview.BeginInvoke(model.Id, null, null);

            return View(model);
        }
        public ActionResult LoadVideo(int VideoId, int page)
        {
            var pagesize = 12;
            ViewBag.page = page;
            int total = 0;
            var lst = new ContentBO().GetPageContentFullsFrontend(page, pagesize, 110, ref total, "", "", "", VideoId.ToString(), "", -1);
            var Model = new NewsModel { listdata = lst, pageIndex = page, pageSize = pagesize, total = total, Id = VideoId };
            return PartialView(Model);
        }
        public ActionResult Audio(int VideoId = 0)
        {
            var model = new CONTENT_FULL();
            if (VideoId == 0)
            {
                model = new ContentBO().GetTopLastestContentFulls(1, 112).FirstOrDefault();
            }
            else
            {
                model = new ContentBO().GetContentFull(VideoId);
            }
            var metaDescription = model.Title + Utils.StripHtmlTag(model.IntroText);
            var siteTitle = model.Title + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            Action<long> addview = ViewAdd;
            var asynSendNoti = addview.BeginInvoke(model.Id, null, null);

            return View(model);
        }
        public ActionResult LoadAudio(int VideoId, int page)
        {
            var pagesize = 12;
            ViewBag.page = page;
            int total = 0;
            var lst = new ContentBO().GetPageContentFullsFrontend(page, pagesize, 112, ref total, "", "", "", VideoId.ToString(), "", -1);
            var Model = new NewsModel { listdata = lst, pageIndex = page, pageSize = pagesize, total = total, Id = VideoId };
            return PartialView(Model);
        }
        public ActionResult Elearning(int Id = 0)
        {
            var model = new CONTENT_FULL();
            if (Id == 0)
            {
                model = new ContentBO().GetTopLastestContentFulls(1, 177).FirstOrDefault();
            }
            else
            {
                model = new ContentBO().GetContentFull(Id);
            }
            var metaDescription = model.Title + Utils.StripHtmlTag(model.IntroText);
            var siteTitle = model.Title + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            Action<long> addview = ViewAdd;
            var asynSendNoti = addview.BeginInvoke(model.Id, null, null);

            return View(model);
        }
        public ActionResult LoadElearning(int Id, int page)
        {
            var pagesize = 12;
            ViewBag.page = page;
            int total = 0;
            var lst = new ContentBO().GetPageContentFullsFrontend(page, pagesize, 177, ref total, "", "", "", Id.ToString(), "", -1);
            var Model = new NewsModel { listdata = lst, pageIndex = page, pageSize = pagesize, total = total, Id = Id };
            return PartialView(Model);
        }
        private void ViewAdd(long Id)
        {
            new ContentBO().ViewAdd(Id);
        }
    }
}
