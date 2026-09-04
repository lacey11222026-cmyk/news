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
using WebMVC4.Filter;

namespace WebMVC4.Controllers
{
    public class Document2Controller : Controller
    {
        //
        // GET: /Document/
        [LocalizationActionFilter]
        public ActionResult Index(int Id, string fromdate = "", string todate = "", string fromdate2 = "", string todate2 = "", string keyword="", int agent = 0, int area = 0, int type = 0, int Page = 1, int hit = -1)
        {

            ViewBag.PageClass = "list";
            var cateobj = new CategoryBO().GetCategoryFull(Id);
            if (cateobj == null)
                return RedirectToAction("Error", "Home");

            ViewBag.Cateurl = cateobj.Link;

            ViewBag.CurrentCategoryId = cateobj.Id;
            ViewBag.ParentCategoryId = cateobj.ParentId;

            var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            var siteTitle = cateobj.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            if (Page > 1)
            {
                ViewBag.Title = siteTitle + " Trang-" + Page.ToString() + " | " + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
                ViewBag.Description = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            }
            var PageSize =15;
            int Total = 0;

            var data = new List<DOCUMENT_FULL>();
            data = new DocumentBO().GetDocumentsSearchPaged(keyword.Trim(), Id, 2, agent, area, type, 1, Page, PageSize, fromdate, todate, ref Total, fromdate2, todate2, hit);


            var Model = new DocumentModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = Id };


            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.keyword = keyword;
            ViewBag.fromdate2 = fromdate2;
            ViewBag.todate2 = todate2;
            ViewBag.hit = hit;
            ViewBag.agent = agent;
            ViewBag.area = area;
            return View(Model);

        }
        [LocalizationActionFilter]
        public ActionResult Detail(int Id,string Title)
        {

            ViewBag.PageClass = "detail";
            var newsobj = new DocumentBO().GetDocumentFull(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");
            if (Title != Utils.ConvertToRewriteLink(newsobj.Name))
                return RedirectToAction("Document2", "News", new { Id = Id, Title = Utils.ConvertToRewriteLink(newsobj.Name) });
            var metaDescription = newsobj.Name + " , " + Utils.StripHtmlTag(newsobj.Description);
            var siteTitle = newsobj.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = newsobj.Name;

            ViewBag.CurrentCategoryId = newsobj.CategoryId;

            //var cateboj = new CategoryBO().GetCategoryFull(newsobj.CategoryId.Value);
            //if (cateboj != null)
            //    ViewBag.ParentCategoryId = cateboj.ParentId;
            ViewBag.ParentCategoryId = newsobj.CateLiteObj.ParrentId;

            var lstRef = new DocumentBO().GetTopLastestDocumentsFull(4, int.Parse(newsobj.CategoryId.ToString()));
            ViewBag.lstRef = lstRef;

            Action<int> addview = ViewAdd;
            var asynSendNoti = addview.BeginInvoke(Id, null, null);
            return View(newsobj);
        }
        public ActionResult ViewFile(int Id)
        {
            var newsobj = new DocumentBO().GetDocumentFull(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");
            ViewBag.FilePath = newsobj.FilePath;
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = newsobj.Name;
            ViewBag.url = $"https://docs.google.com/gview?url={newsobj.FilePath}&embedded=true";

            //Action<int> addview = ViewAdd;
            //var asynSendNoti = addview.BeginInvoke(Id, null, null);
            return View();
        }
        public ActionResult DownloadFile(int Id)
        {
            var newsobj = new DocumentBO().GetDocumentFull(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");

            //Action<int> addview = ViewAdd;
            //var asynSendNoti = addview.BeginInvoke(Id, null, null);
            return Redirect(newsobj.FilePath);
        }
        private void ViewAdd(int Id)
        {
            new DocumentBO().ViewAdd(Id);
        }
    }
}
