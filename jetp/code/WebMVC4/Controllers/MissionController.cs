using BIZ;
using BIZ.Entity;
using DATA;
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
    public class MissionController : Controller
    {
        //
        // GET: /Mission/
        [LocalizationActionFilter]
        public ActionResult Index(int Page = 1)
        {
            ViewBag.Description = Resources.Global.SiteTitle;
            ViewBag.Keywords = Resources.Global.SiteTitle;
            ViewBag.Title = Resources.Global.SiteTitle;

            var lang = WorkContext.GetLanguage();
            int cateId = 82;
            if(lang!="vi-vn")
                cateId = 85;
            var cateobj = new CategoryBO().GetCategoryFull(cateId);

            try
            {
                cateobj.Param = JsonConvert.DeserializeObject<CategoryParam>(cateobj.Params);
            }
            catch
            {

                cateobj.Param = new CategoryParam();
            }
            ViewBag.CateName = cateobj.Name;
            ViewBag.CateDescription = cateobj.Description;
            ViewBag.CateImg = cateobj.Param.Image;

            var PageSize = 20;
            int Total = 0;
            var data = new MissionBO().GetMissionsFuLLPaged(WorkContext.GetLanguage(), -1, 1, -1, -1, Page, PageSize, ref Total);
            var Model = new MissionModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total };
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
