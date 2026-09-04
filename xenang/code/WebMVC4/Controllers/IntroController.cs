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
            ViewBag.SiteTitle = intro.Name;
            ViewBag.SiteDescription = intro.Description;
            if (!string.IsNullOrEmpty(intro.Param.SiteTitle))
            {
                ViewBag.SiteTitle = intro.Param.SiteTitle;

            }
            if (!string.IsNullOrEmpty(intro.Param.SiteDescription))
            {
                ViewBag.SiteDescription = intro.Param.SiteDescription;

            }
            ViewBag.CurrentCategoryId = intro.Id;
            ViewBag.ParentCategoryId = intro.ParentId;

            //var lstrelate = new List<CATEGORY_FULL>();

            //if (intro.ParentId == 0)
            //{
            //    lstrelate = new CategoryBO().GetAllChildCategories(intro.Id, 10, false);

            //}

            //else
            //{
            //    var data = new CategoryBO().GetAllChildCategories(Convert.ToInt32(intro.Id), 10, false);
            //    if (data != null)
            //    {
            //        lstrelate = data;
            //    }
            //    else if (intro.ParentId>0)
            //    {
            //        lstrelate = new CategoryBO().GetAllChildCategories(Convert.ToInt32(intro.ParentId), 10, false);
            //    }

            //}

            //if (lstrelate!=null &&lstrelate.Count > 0)
            //{
            //    ViewBag.lstrelate = lstrelate.Where(x => x.Id != CategoryId).ToList();
            //}
            //else
            //{
            //    ViewBag.lstrelate = new List<CATEGORY_FULL>();
            //}
           
            
            //ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(Resources.Global.SiteDescription);
            //ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            //ViewBag.Title = siteTitle + Resources.Global.SiteTitle;

            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            return View(intro);
        }
        [ChildActionOnly]
        public ActionResult Relate(List<CATEGORY_FULL> data)
        {
            data = data.Where(x => x.Type == 1).ToList();
            return PartialView(data);
        }
        [ChildActionOnly]
        public ActionResult SystemPage()
        {

            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult OrganPage()
        {

            return PartialView();
        }
    }
}
