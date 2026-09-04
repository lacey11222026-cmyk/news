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
    public class IntroController : Controller
    {
        //
        // GET: /Intro/
       // [LocalizationActionFilter]
        public ActionResult Index(int CategoryId, string CateName)
        {
            
            var intro = new CategoryBO().GetCategoryFull(CategoryId);
            if (intro == null)
                return RedirectToAction("Error", "Home");
            if (CateName != Utils.ConvertToRewriteLink(intro.Name))
                return RedirectToAction("Index", "Intro", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(intro.Name) });

            var metaDescription = Utils.StripHtmlTag(intro.Description);
            var siteTitle = intro.Name ;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            //if(CategoryId==2)
            //    intro = new CategoryBO().GetCategoryFull(11);
            //if (CategoryId == 23)
            //    intro = new CategoryBO().GetCategoryFull(24);
            ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle;
            return View(intro);
        }
        [LocalizationActionFilter]
        public ActionResult ResultPage()
        {

            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;
            var data = new CityBO().GetTopCity(0, 1, -1);
            if(WorkContext.GetLanguage()=="en-us")
            {
                return View("ResultPageEn",data);
            }
            return View(data);
        }
        [LocalizationActionFilter]
        public ActionResult ViewFile(int Id)
        {

            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;
            var newsobj = new CityBO().GetCity(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");

            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;
            ViewBag.FilePath = newsobj.Url;
            ViewBag.url = $"https://docs.google.com/gview?url=http://{Request.Url.Host}{newsobj.Url}&embedded=true";


            return View();
        }

        [LocalizationActionFilter]
        public ActionResult Question(int Page = 1)
        {

            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;
            var PageSize = 30;
            int Total = 0;
            var albums = new QABO().GetQAsPaged( Page, PageSize, ref Total,1, WorkContext.GetLanguage());
            var Model = new QAModel { listdata = albums, pageIndex = Page, pageSize = PageSize, total = Total,};
            return View(Model);
        }
        [LocalizationActionFilter]
        public ActionResult Link(int? type,int Page = 1,string keyword = "" )
        {

            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;
            var PageSize = 20;
            int Total = 0;
            ViewBag.keyword = keyword;
            int Type = type == null ? 0 : (int)type;
            var albums = new ShopBO().GetShopsPaged(keyword,Page, PageSize, ref Total, 1, WorkContext.GetLanguage(), Type);
            var Model = new ShopModel { listdata = albums, pageIndex = Page, pageSize = PageSize, total = Total, };
            Model.CategoryId = type.GetValueOrDefault();
            return View(Model);
        }
        [LocalizationActionFilter]
        public ActionResult Organ(int Page = 1, string keyword = "")
        {

            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;
            var PageSize = 20;
            int Total = 0;
            ViewBag.keyword = keyword;
         
            var albums = new ShopBO().GetShopsPaged(keyword, Page, PageSize, ref Total, 1, WorkContext.GetLanguage(), 1);
            var Model = new ShopModel { listdata = albums, pageIndex = Page, pageSize = PageSize, total = Total, };
           // Model.CategoryId = type.GetValueOrDefault();
            return View(Model);
        }
    }
}
