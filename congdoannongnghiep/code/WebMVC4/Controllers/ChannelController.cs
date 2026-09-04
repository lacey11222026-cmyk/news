using System;
using System.Collections.Generic;
using System.Linq;
using BIZ;
using BIZ.Entity;
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
    public class ChannelController : Controller
    {
        //
        // GET: /Channel/

        public ActionResult Index(int Id, string Name, int Page = 1)
        {
            var cateobj = new ChannelBO().GetChannelFull(Id);
            if (cateobj == null)
                return RedirectToAction("Error", "Home");
            if (Name != Utils.ConvertToRewriteLink(cateobj.Name))
                return RedirectToAction("Index", "Channel", new { Id = Id, Name = Utils.ConvertToRewriteLink(cateobj.Name) });
            ViewBag.ChannelName = cateobj.Name;
            var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            var siteTitle = cateobj.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            if (Page > 1)
            {
                ViewBag.Title = siteTitle + " Trang-" + Page.ToString() + " | " + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
                ViewBag.Description = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            }
            var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]);
            int Total = 0;
            var articles = new ContentBO().GetPageContentFullsFrontend(Page, PageSize, 0, ref Total, "", "", "", "","",-1,Id);
            ViewBag.Total = Total;
            ViewBag.Page = Page;
            //ViewBag.Type = Type;
            ViewBag.PageSize = PageSize;
            return View(articles);
        }

    }
}
