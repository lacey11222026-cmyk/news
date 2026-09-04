using System.Web.Routing;
using BIZ;
using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using  WebEN.Helper;

namespace WebEN.Controllers
{
    
    public class IntroController : BaseController
    {
        //
        // GET: /Intro/
        //protected override void Initialize(RequestContext requestContext)
        //{
        //    var lang = requestContext.HttpContext.Request["lang"];
        //    if (!String.IsNullOrEmpty(lang))
        //    {
        //        switch (lang)
        //        {
        //            case "en-us":
        //                CultureHelper.SetCulture(requestContext, lang);
        //                break;
        //            case "en-us":
        //                CultureHelper.SetCulture(requestContext, lang);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    base.Initialize(requestContext);

        //}
        public ActionResult Index(int CategoryId, string CateName)
        {
            var lstrelate = new List<CATEGORY_FULL>();
            var intro = new CATEGORY_FULL();
            //return View(intro);
            intro = new CategoryBO().GetCategoryFull(CategoryId);
            if (intro == null)
                return RedirectToAction("Error", "Home");
            if (CateName != Utils.ConvertToRewriteLink(intro.Name))
                return RedirectToAction("Index", "Intro", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(intro.Name) });

            var culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLowerInvariant();
            if (culture != intro.Language)
            {

                CultureHelper.SetCulture(HttpContext.Request.RequestContext, culture);
                return RedirectToAction("Index", "Intro", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(intro.Name) });
            }

            var metaDescription = Utils.StripHtmlTag(intro.Description);
            var siteTitle = intro.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.CurrentCategoryId = intro.Id;
            ViewBag.ParentCategoryId = intro.ParentId;
          
            
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
