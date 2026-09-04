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
    public class ProjectController : Controller
    {
        //
        // GET: /Project/
        public ActionResult Index(int Page = 1, string keyword = "")
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];


            var PageSize = 16;
            int Total = 0;
            var data = new ProjectBO().GetProjectsByFilter(keyword,1, Page, PageSize, ref Total);
            var Model = new ProjectModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total};
            return View(Model);
        }
        public ActionResult Detail(int Id)
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            var obj = new ProjectBO().GetProject(Id);
            return View(obj);
        }
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult TopProject(int Top=0)
        {
            var data = new ProjectBO().GetTopProject(6, 1);

            return PartialView(data);
        }

        public ActionResult RelateProject(int  Id= 0)
        {
            var data = new ProjectBO().GetTopProject(9, 1);
            data = data.Where(x => x.Id != Id).Take(8).ToList();

            return PartialView(data);
        }
    }
}
