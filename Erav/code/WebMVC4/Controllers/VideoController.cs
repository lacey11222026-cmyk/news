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
using WebMVC4.Filter;
using WebMVC4.Helper;
using WebMVC4.Models;
namespace WebMVC4.Controllers
{
    public class VideoController : Controller
    {
        //
        // GET: /Video/
        [LocalizationActionFilter]
        public ActionResult Index(int VideoId = 0)
        {
            var model = new CONTENT_FULL();
            var CategoryId = 84;

            //if (WorkContext.GetLanguage() == "en-us")
            //{
            //    CategoryId = 59;
            //}
            if (VideoId == 0)
            {
                model = new ContentBO().GetHotNews(CategoryId, 1).FirstOrDefault();
            }
            else
            {
                model = new ContentBO().GetContentFull(VideoId);
            }
            ViewBag.CategoryId = CategoryId;
            var metaDescription = Utils.StripHtmlTag(model.IntroText);
            var siteTitle = model.Title + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = model.Title;
            ViewBag.SiteImage = model.MainImage;
            ViewBag.MailShare = String.Format("https://mail.google.com/mail/u/0/?ui=2&view=cm&fs=1&tf=1&su={0}&body={1}", HttpUtility.UrlEncode(model.Title), HttpUtility.UrlEncode(Request.Url.AbsoluteUri));
            Action<long, int> addview = ViewAdd;
            
            var asynSendNoti = addview.BeginInvoke(model.Id, CategoryId, null, null);

            return View(model);
        }
        public ActionResult HotVideo(int VideoId)
        {
            var CategoryId = 84;

            //if (WorkContext.GetLanguage() == "en-us")
            //{
            //    CategoryId = 59;
            //}
            var model = new ContentBO().GetHotNews(CategoryId, 5);
            model = model.Where(x => x.Id != VideoId).Take(4).ToList();
            var lstNotId = "";
            foreach (var item in model)
            {
                lstNotId += item.Id + ",";
            }
            lstNotId += VideoId + ",";
            Session["lstNotId"] = lstNotId;
            return PartialView(model);
        }
        public ActionResult LoadVideo(int VideoId, int page, int CategoryId)
        {
            var pagesize = 16;
            ViewBag.page = page;
            int total = 0;
            ViewBag.CategoryId = CategoryId;
            var lstNotId = "";
            
            lstNotId += VideoId + ",";
            var lst = new ContentBO().GetPageContentFullsFrontend(page, pagesize, CategoryId, ref total, "", "", "", lstNotId, "", -1);
            var Model = new NewsModel { listdata = lst, pageIndex = page, pageSize = pagesize, total = total, Id = VideoId, CategoryId = CategoryId };
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
            //Action<long,int> addview = ViewAdd;
            //var asynSendNoti = addview.BeginInvoke(model.Id, null, null);

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
        private void ViewAdd(long Id, int CategoryId)
        {
            new ContentBO().ViewAdd(Id, CategoryId);
        }
    }
}
