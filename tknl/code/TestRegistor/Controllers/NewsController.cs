using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BIZ;
using Newtonsoft.Json;
using TestRegistor.Models;
using UTILS;

namespace TestRegistor.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        [OutputCache(Duration =120, VaryByParam = "*")]
        public ActionResult Index(int CategoryId, string CateName, int Page = 1, int Type = 0)
        {



            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
            if (cateobj == null)
                return RedirectToAction("Index", "Home");
            if (CateName != Utils.ConvertToRewriteLink(cateobj.Name))
                return RedirectToAction("Index", "News", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(cateobj.Name) });

            ViewBag.CurrentCategoryId = cateobj.Id;
            ViewBag.ParentCategoryId = cateobj.ParentId;



            var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            var siteTitle = cateobj.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            
           
            var articles = new ContentBO().GetTopLastestContentFulls(10, CategoryId);

           
          
            ViewBag.CategoryId = CategoryId;
            ViewBag.CateName = cateobj.Name;


            ViewBag.PageClass = "list";
           


            return View(articles);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Detail(int Id, string Title)
        {
            var newsobj = GetDetail("https://tietkiemnangluong.com.vn/", Id);

            if (newsobj == null )
                return RedirectToAction("Index", "Home");
            if (Title != Utils.ConvertToRewriteLink(newsobj.Title))
                return RedirectToAction("Detail", "News", new { Id = Id, Title = Utils.ConvertToRewriteLink(newsobj.Title) });
            var metaDescription = newsobj.Title;
            var siteTitle = newsobj.Title;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];

            
            ViewBag.MainImage = ConfigurationManager.AppSettings["Domain"]+newsobj.MainImage;
            //ViewBag.CurrentCategoryId = newsobj.CategoryId;
           // var cateObj = new CategoryBO().GetCategoryFull(newsobj.CategoryId.GetValueOrDefault());
            //ViewBag.CateName = cateObj.Name;

            
            return View(newsobj);
        }
        public static CONTENT_APIFULL GetDetail(string domain, int top)
        {

            try
            {

                var url = String.Format("{0}api/Content/GetDetail/?id={1}", domain, top);
                var apitext = Utilities.HttpRequestGet(url);

                return JsonConvert.DeserializeObject<CONTENT_APIFULL>(apitext);
            }

            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return null;

            }

        }
        [OutputCache(Duration = 360, VaryByParam = "*")]
        public ActionResult Relate( int CategoryId,int Id)
        {
            var data = new ContentBO().GetTopLastestContentFulls(6, CategoryId);
            if (data != null)
                data = data.Where(x => x.Id != Id).ToList();
            return PartialView(data);
        }
    }
}