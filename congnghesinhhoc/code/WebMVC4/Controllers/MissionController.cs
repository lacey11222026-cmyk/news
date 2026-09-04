using BIZ;
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
    public class MissionController : Controller
    {
        //
        // GET: /Mission/

        public ActionResult Index(int? categoryId, int? createdBy, int? year, int Page = 1, string keyword = "")
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];


            int CategoryId = categoryId == null ? -1 : (int)categoryId;
            int CreatedBy = createdBy == null ? -1 : (int)createdBy;
            int Year = year == null ? -1 : (int)year;
            var PageSize = 20;
            int Total = 0;
            var data = new MissionBO().GetMissionsFuLLPaged(keyword, CategoryId, 1, CreatedBy, Year, Page, PageSize, ref Total);
            var Model = new MissionModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total, Year = Year, CategoryId = CategoryId, CreatedBy = CreatedBy, keyword = keyword };
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
