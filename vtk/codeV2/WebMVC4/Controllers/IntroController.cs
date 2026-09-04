using BIZ;
using BIZ.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Filter;

namespace WebMVC4.Controllers
{
    public class IntroController : Controller
    {
        //
        // GET: /Intro/
        [LocalizationActionFilter]
        public ActionResult Index(int CategoryId, string CateName)
        {
            var intro = new CategoryBO().GetCategoryFull(CategoryId);
            if (intro == null)
                return RedirectToAction("Error", "Home");
            if (CateName != Utils.ConvertToRewriteLink(intro.Name))
                return RedirectToAction("Index", "Intro", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(intro.Name) });
            intro.Param = JsonConvert.DeserializeObject<CategoryParam>(intro.Params);
            var metaDescription = Utils.StripHtmlTag(intro.Description);
            var siteTitle = intro.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");

            ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            return View(intro);
        }

    }
}
