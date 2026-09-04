using BIZ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BIZ.Entity;
using UTILS;
using WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class AgentController : Controller
    {
        //
        // GET: /Agent/

        public ActionResult Index(string keyword, int Page = 1)
        {
            var metaDescription = "";
            var siteTitle = "";
          
            ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            if (Page > 1)
            {
                ViewBag.Title = siteTitle + " Trang-" + Page.ToString() + " | " + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
                ViewBag.Description = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            }
            var PageSize = 12;
            int Total = 0;
            var data = new ShopBO().GetShopsPaged(keyword, Page, PageSize, ref Total,1);
            ViewBag.keyword = keyword;
            var Model = new AgentModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total };
            return View(Model);
        }

    }
}
