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
        public ActionResult Index(int Page = 1)
        {

            ViewBag.Description = Resources.Global.SiteDescription;
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;
            int CategoryId = 1;
            if (WorkContext.GetLanguage() == "en-us")
                CategoryId = 2;
            
            var PageSize = 12;
            int Total = 0;
            var data = new MissionBO().GetMissionsFuLLPaged("", CategoryId, 1, -1, -1, Page, PageSize, ref Total);
            var Model = new MissionModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = CategoryId };
            return View(Model);
        }
        [LocalizationActionFilter]
        public ActionResult Detail(int Id)
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
           
            var Model = new MissionBO().GetMissionFull(Id);
            ViewBag.Title = Model.Name;
            return View(Model);
        }

    }
}
