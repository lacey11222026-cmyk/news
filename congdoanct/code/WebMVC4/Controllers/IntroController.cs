using BIZ;
using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;

namespace WebMVC4.Controllers
{
    
    public class IntroController : Controller
    {
        //
        // GET: /Intro/

        public ActionResult Index(int CategoryId, string CateName)
        {
            var intro = new CategoryBO().GetCategoryFull(CategoryId);
            if (intro == null)
                return RedirectToAction("Error", "Home");
            if (CateName != Utils.ConvertToRewriteLink(intro.Name))
                return RedirectToAction("Index", "Intro", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(intro.Name) });

            var metaDescription = Utils.StripHtmlTag(intro.Description);
            var siteTitle = intro.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.CurrentCategoryId = intro.Id;
            ViewBag.ParentCategoryId = intro.ParentId;
          
            var lstrelate = new List<CATEGORY_FULL>();
            if (intro.ParentId==0)
                lstrelate = new CategoryBO().GetAllChildCategories(intro.Id, 10, false);
            else
                lstrelate = new CategoryBO().GetAllChildCategories(Convert.ToInt32(intro.ParentId), 10, false);

            ViewBag.lstrelate = lstrelate;
            
            ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            return View(intro);
        }
        [ChildActionOnly]
        public ActionResult Relate(List<CATEGORY_FULL> data)
        {
       
            return PartialView(data);
        }
    }
}
