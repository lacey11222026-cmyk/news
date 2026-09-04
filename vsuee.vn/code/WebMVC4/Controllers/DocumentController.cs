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
        public ActionResult Index(int Id, string fromdate, string todate, string keyword, int Page = 1)
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
            ViewBag.Description = metaDescription + " ";
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle;
            //if (Page > 1)
            //{
            //    ViewBag.Title = siteTitle + " Trang-" + Page.ToString() + " | " + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            //    ViewBag.Description = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            //}
            var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxDocShow"]);
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


            var lstcate = new List<CATEGORY_FULL>();
            //if (cateobj.Id == 3 || cateobj.ParentId == 3|| cateobj.ParentId == 67)
            //{
            //    lstcate = new CategoryBO().GetAllChildCategories(3, 10, false).Where(x => x.Published == 1).ToList();
            //    ViewBag.lang = "vn";
            //}
            //else
            //{

            //    ViewBag.lang = "en";
            //    lstcate = new CategoryBO().GetAllChildCategories(56, 10, false).Where(x => x.Published == 1).ToList();
                
                
            //}

            var Model = new DocumentModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = Id, subcate = lstcate };


            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.keyword = keyword;

            if (cateobj.Id == 22 || cateobj.ParentId ==22)
            {
                return View("Index2", Model);
            }
            return View(Model);

        }
        [LocalizationActionFilter]
        public ActionResult Detail(int Id)
        {

            ViewBag.PageClass = "detail";
            var newsobj = new DocumentBO().GetDocumentFull(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");

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

            Action<int,int> addview = ViewAdd;
            var asynSendNoti = addview.BeginInvoke(Id,3, null, null);
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
            ViewBag.url = $"https://docs.google.com/gview?url=https://vsuee.vn/{newsobj.FilePath}&embedded=true";

            Action<int,int> addview = ViewAdd;
            var asynSendNoti = addview.BeginInvoke(Id,4, null, null);
            return View();
        }
        public ActionResult DownloadFile(int Id)
        {
            var newsobj = new DocumentBO().GetDocumentFull(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");

            Action<int,int> addview = ViewAdd;
            var asynSendNoti = addview.BeginInvoke(Id,4, null, null);
            return Redirect(newsobj.FilePath);
        }
        private void ViewAdd(int Id,int Type)
        {
            new ContentBO().ViewAdd(Id, Type);
        }
    }
}
