using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BIZ;
using DATA;
using UTILS;

namespace WebEN.Controllers
{
    
    public class ChannelController : Controller
    {
        //
        // GET: /Channel/

        public ActionResult Index(int Id, string Name, int Page = 1)
        {
            var obj = new ChannelBO().GetChannelFull(Id);
            if (obj == null)
                return RedirectToAction("Error", "Home");
            if (Name != Utils.ConvertToRewriteLink(obj.Name))
                return RedirectToAction("Index", "Channel", new { Id = Id, Name = Utils.ConvertToRewriteLink(obj.Name) });
             var metaDescription = Utils.StripHtmlTag(obj.Description);
            var siteTitle = obj.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];

             if (Page > 1)
            {
                ViewBag.Title = siteTitle + " Trang-" + Page.ToString() + " | " + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
                ViewBag.Description = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            }
            var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]) * 2;
            int Total = 0;
            var articles = new ContentBO().GetTopContentByChannelId(Id,Page, PageSize, ref Total);

            var pageNext = Page + 1;
            var pageNextShow = false;
            if (Total <= PageSize)
            {
                pageNext = 1;
                pageNextShow = false;
            }
            else
            {
                pageNextShow = true;
            }

            ViewBag.PageNextShow = pageNextShow;
            ViewBag.pageNext = pageNext;

            ViewBag.PageSize = PageSize;
            ViewBag.Id = Id;
            return View(articles);
            
        }
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult TopChannel(int Top = 5)
        {
            var key = "HotChannel";
            var lstchannel2 = new List<DATA.Channel>();
            var configValue = new SystemConfigBO().GetByKey(key);
            if (configValue != null)
            {
                 lstchannel2 = new ChannelBO().GetChannelByIds(configValue.ConfigValue, Top, true);
            }

            return PartialView(lstchannel2);
        }
        [ChildActionOnly]
        public ActionResult TopChannelMobile(int Top = 3)
        {
            var key = "HotChannel";
            var lstchannel2 = new List<DATA.Channel>();
            var configValue = new SystemConfigBO().GetByKey(key);
            if (configValue != null)
            {
                lstchannel2 = new ChannelBO().GetChannelByIds(configValue.ConfigValue, Top, true);
            }

            return PartialView(lstchannel2);
        }
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult Breadcrumb(int Id)
        {
            var obj = new ChannelBO().GetChannelFull(Id);
            return PartialView("BreadcrumbObj", obj);
        }
        [ChildActionOnly]
        public ActionResult BreadcrumbObj(Channel data)
        {

            return PartialView(data);
        }

    }
}
