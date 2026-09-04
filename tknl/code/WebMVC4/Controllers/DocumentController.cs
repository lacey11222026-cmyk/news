using System.Runtime.Remoting.Contexts;
using BIZ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BIZ.Entity;
using UTILS;

using WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class DocumentController : BaseController
    {
        //
        // GET: /Document/

        public ActionResult Index(int Id, string fromdate, string todate, string keyword = "", int Page = 1)
        {
            if (Id <= 1)
                Id = 25;


            keyword = Utils.FormatKeywordSearch(keyword);

            ViewBag.CurrentCategoryId = Id;


            //var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            //var siteTitle = cateobj.Name + " | ";
            // var metaKeyword = siteTitle.Replace(" | ", ",");
            // ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            //ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = "Văn bản | "+ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            if (Page > 1)
            {
                ViewBag.Title = "Văn bản - Trang-" + Page.ToString() + " | " + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
                ViewBag.Description = "Văn bản - Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            }
            var PageSize = 15;
            int Total = 0;

            var data = new List<DOCUMENT_FULL>();
            if (String.IsNullOrEmpty(fromdate) && String.IsNullOrEmpty(todate) && String.IsNullOrEmpty(keyword))
            {
                data = new DocumentBO().GetPageLastestDoccumentFull(Id, Page, PageSize, ref Total);
            }
            else
            {
                data = new DocumentBO().GetDocumentsSearchPaged(keyword.Trim(), Id, 1, Page, PageSize, fromdate, todate, ref Total);
            }



            var Model = new DocumentModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = Id };


            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.keyword = keyword;


            return View(Model);

        }
        public ActionResult Detail(int Id)
        {
            var newsobj = new DocumentBO().GetDocumentFull(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");


            var metaDescription = newsobj.Name + " , " + Utils.StripHtmlTag(newsobj.Description);
            var siteTitle = newsobj.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];

            ViewBag.CurrentCategoryId = newsobj.CategoryId;



            var lstRef = new DocumentBO().GetTopLastestDocumentsFull(4, int.Parse(newsobj.CategoryId.ToString()));
            ViewBag.lstRef = lstRef;
            return View(newsobj);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult TopDocument()
        {
            var MaxDocuments = 3;
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxDocuments,25);
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult TopDocument2()
        {
            var MaxDocuments = 3;
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxDocuments, 25);
            return PartialView(lstcontent);
        }
        public ActionResult ViewPDF(string url)
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            ViewBag.url = url;
            return View();
        }
    }
}
