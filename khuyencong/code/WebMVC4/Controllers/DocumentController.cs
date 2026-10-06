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
    public class DocumentController : Controller
    {
        //
        // GET: /Document/
        [LocalizationActionFilter]
        public ActionResult Index(int Id, string fromdate="", string todate="", string keyword="",string code="", int agent = 0, int area = 0, int type = 0, int Page = 1)

        {

            ViewBag.PageClass = "list";
            var cateobj = new CategoryBO().GetCategoryFull(Id);
            if (cateobj == null)
                return RedirectToAction("Error", "Home");

            ViewBag.CurrentCategoryId = cateobj.Id;
            ViewBag.ParentCategoryId = cateobj.ParentId;

            var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            var siteTitle = cateobj.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription ;
            ViewBag.Keywords = metaKeyword;
            ViewBag.Title = siteTitle;
           
            var PageSize = 20;
            int Total = 0;

            var data = new List<DOCUMENT_FULL>();
            data = new DocumentBO().GetDocumentsSearchPaged2(keyword.Trim(),code.Trim(),agent,area,type, Id, 1, Page, PageSize, fromdate, todate, ref Total);


            var lstcate = new List<CATEGORY_FULL>();
            //if (cateobj.Id == 3 || cateobj.ParentId ==3)
            //{
            //    lstcate = new CategoryBO().GetAllChildCategories(3, 10, false).Where(x=>x.Published==1).ToList();
            //}
            //else
            //{
            //    lstcate = new CategoryBO().GetAllChildCategories(30, 10, false).Where(x => x.Published == 1).ToList();
            //}

            var Model = new DocumentModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = Id, subcate = lstcate };


            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.keyword = keyword;
            ViewBag.type = type;
            ViewBag.agent = agent;
            ViewBag.area = area;
            ViewBag.code = code;
            return View(Model);

        }
        [LocalizationActionFilter]
        public ActionResult Detail(int Id)
        {

            ViewBag.PageClass = "detail";
            var newsobj = new DocumentBO().GetDocumentFull(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");

            var metaDescription = newsobj.Name ;
            var siteTitle = newsobj.Name ;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword;
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

            Action<int> addview = ViewAdd;
            var asynSendNoti = addview.BeginInvoke(Id, null, null);
            return View();
        }
        public ActionResult DownloadFile(int Id)
        {
            var newsobj = new DocumentBO().GetDocumentFull(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");

            Action<int> addview = ViewAdd;
            var asynSendNoti = addview.BeginInvoke(Id, null, null);
            return Redirect(newsobj.FilePath);
        }
        private void ViewAdd(int Id)
        {
            new DocumentBO().ViewAdd(Id);
        }
    }
}
