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
            if (VideoId==0)
            {
                model = new ContentBO().GetTopLastestContentFulls(1, 10).FirstOrDefault();
            }
            else
            {
                model = new ContentBO().GetContentFull(VideoId);
            }
            var metaDescription = model.Title  + Utils.StripHtmlTag(model.IntroText);
            var siteTitle = model.Title + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            new ContentBO().ViewAdd(model.Id);
            return View(model);
        }
        public ActionResult LoadVideo(int VideoId,int page)
        {
            var pagesize =20;
            ViewBag.page = page;
            int total = 0;
            var lst = new ContentBO().GetPageContentFullsFrontend(page, pagesize,10, ref total,"","","", VideoId.ToString(),"",-1);
            var Model = new NewsModel { listdata = lst, pageIndex = page, pageSize = pagesize, total = total, Id = VideoId };
            return PartialView(Model);
        }
    }
}
