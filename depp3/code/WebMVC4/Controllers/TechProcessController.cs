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
    public class TechProcessController : Controller
    {
        //
        // GET: /TechProcess/

        public ActionResult Index()
        {
            return View();
        }
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult TopTechProcesss(int Top = 0)
        {
            var data = new TechProcessBO().GetTopLastestTechProcesssFull(Top);

            return PartialView(data);
        }
        public ActionResult Index(int Page = 1, string keyword = "")
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];


            var PageSize = 16;
            int Total = 0;
            var data = new TechProcessBO().GetTechProcesssFuLLPaged(keyword, Page, PageSize, ref Total);
            var Model = new TechProcessModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total };
            return View(Model);
        }
        public ActionResult Detail(int Id)
        {

            ViewBag.PageClass = "detail";
            var newsobj = new TechProcessBO().GetTechProcess(Id);
            if (newsobj == null )
                return RedirectToAction("Error", "Home");

            var metaDescription = newsobj.Name;
            var siteTitle = newsobj.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = newsobj.Name;

        
            return View(newsobj);
        }
        public ActionResult ViewFile(int Id)
        {
            var newsobj = new TechProcessBO().GetTechProcess(Id);
            if (newsobj == null )
                return RedirectToAction("Error", "Home");
            ViewBag.FilePath = newsobj.FilePath;
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = newsobj.Name;
            ViewBag.url = $"https://docs.google.com/gview?url={newsobj.FilePath}&embedded=true";

        
            return View();
        }
        public ActionResult DownloadFile(int Id)
        {
            var newsobj = new TechProcessBO().GetTechProcess(Id);
            if (newsobj == null )
                return RedirectToAction("Error", "Home");

         
            return Redirect(newsobj.FilePath);
        }
    }
}
