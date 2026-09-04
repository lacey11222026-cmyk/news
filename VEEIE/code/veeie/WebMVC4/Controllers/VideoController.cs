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
            var CategoryId = 13;
            if (WorkContext.GetLanguage()=="en-us")
            {
                CategoryId = 26;
            }
            if (VideoId == 0)
            {
                model = new ContentBO().GetTopLastestContentFulls(1, CategoryId).FirstOrDefault();
            }
            else
            {
                model = new ContentBO().GetContentFull(VideoId);
            }
            model.CategoryId = CategoryId;
            var metaDescription =  Utils.StripHtmlTag(model.IntroText);
            var siteTitle = model.Title;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = model.Title;
            ViewBag.MainImage = model.MainImage;
            Action<long> addview = ViewAdd;
            var asynSendNoti = addview.BeginInvoke(model.Id, null, null);

            return View(model);
        }
        public ActionResult LoadVideo(int VideoId, int page,int CategoryId)
        {
            var pagesize = 16;
            ViewBag.page = page;
            int total = 0;
            var lst = new ContentBO().GetPageContentFullsFrontend(page, pagesize, CategoryId, ref total, "", "", "", VideoId.ToString(), "", -1);
            var Model = new NewsModel { listdata = lst, pageIndex = page, pageSize = pagesize, total = total, Id = VideoId,CategoryId=CategoryId };
            return PartialView(Model);
        }
       
        private void ViewAdd(long Id)
        {
            new ContentBO().ViewAdd(Id);
        }
    }
}
