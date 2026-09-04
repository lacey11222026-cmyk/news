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
    public class NoteController : Controller
    {
        //
        // GET: /Note/

        [LocalizationActionFilter]
        public ActionResult Index(int CategoryId, string CateName, int Page = 1, int Type = 0)
        {
            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
            if (cateobj == null)
                return RedirectToAction("Error", "Home");
            if (CateName != Utils.ConvertToRewriteLink(cateobj.Name))
                return RedirectToAction("Index", "Note", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(cateobj.Name) });

            //ViewBag.CurrentCategoryId = cateobj.Id;
            //ViewBag.ParentCategoryId = cateobj.ParentId;
            //ViewBag.CateName = cateobj.Name;


            var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            var siteTitle = cateobj.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + Resources.Global.SiteTitle;
            ViewBag.CategoryId = CategoryId;
            ViewBag.CateName = cateobj.Name;
            var PageSize = 10;
            int Total = 0;
            var articles = new NoteBO().GetNotesPaged("",Page, PageSize, ref Total, 1, CategoryId);
            var model = new NoteModel { listdata = articles, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = CategoryId };

            return View(model);
        }
        [LocalizationActionFilter]
        public ActionResult Detail(int Id, string Title)
        {
            ViewBag.PageClass = "detail";
            var newsobj = new NoteBO().GetNote(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");
            if (Title != Utils.ConvertToRewriteLink(newsobj.Title))
                return RedirectToAction("Detail", "Note", new { Id = Id, Title = Utils.ConvertToRewriteLink(newsobj.Title) });
            var metaDescription = newsobj.Title ;
            var siteTitle = newsobj.Title;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle;

            
           
          
            return View(newsobj);
        }

    }
}
