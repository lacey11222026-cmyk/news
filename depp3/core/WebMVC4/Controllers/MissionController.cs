using BIZ;
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
    public class MissionController : Controller
    {
        //
        // GET: /Mission/
        [LocalizationActionFilter]
        public ActionResult Index( int Page = 1)
        {
            ViewBag.Description = Resources.Global.SiteTitle;
            ViewBag.Keywords = Resources.Global.SiteTitle;
            ViewBag.Title = Resources.Global.SiteTitle;


            
            var PageSize = 20;
            int Total = 0;
            var data = new MissionBO().GetMissionsFuLLPaged(WorkContext.GetLanguage(), -1, 1, -1, -1, Page, PageSize, ref Total);
            var Model = new MissionModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total };
            return View(Model);
        }
        public ActionResult Detail(int Id)
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            var Model = new MissionBO().GetMissionFull(Id);
            return View(Model);
        }

    }
}
