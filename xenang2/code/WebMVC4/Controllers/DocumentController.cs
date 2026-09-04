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
    public class DocumentController : Controller
    {
        //
        // GET: /Document/

        public ActionResult Index(int Id,int Page = 1)
        {
            var cateobj = new CategoryBO().GetCategoryFull(Id);
            if (cateobj == null)
                return RedirectToAction("Error", "Home");

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
            var PageSize = 20;
            int Total = 0;

            var data = new List<DOCUMENT_FULL>();
            data = new DocumentBO().GetDocumentsSearchPaged("", Id, 1, Page, PageSize, "", "", ref Total);

            //var lstcate = new CategoryBO().GetAllChildCategories(3, 10, false);

            var Model = new DocumentModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = Id };



            return View(Model);

        }
        public ActionResult Static()
        {

            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];

            return View();
        }
        public ActionResult SoftPage()
        {

            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];

            return View();
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

            //var cateboj = new CategoryBO().GetCategoryFull(newsobj.CategoryId.Value);
            //if (cateboj != null)
            //    ViewBag.ParentCategoryId = cateboj.ParentId;
            //ViewBag.ParentCategoryId = newsobj.CateLiteObj.ParrentId;

            var lstRef = new DocumentBO().GetTopLastestDocumentsFull(4, int.Parse(newsobj.CategoryId.ToString()));
            ViewBag.lstRef = lstRef;
            return View(newsobj);
        }
        public ActionResult ViewFile(int Id)
        {
            var newsobj = new DocumentBO().GetDocumentFull(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            ViewBag.FilePath = newsobj.FilePath;
            if (newsobj.FilePath.Contains("http"))
            {
                ViewBag.url = $"https://docs.google.com/gview?url={newsobj.FilePath}&embedded=true";
            }
            else
            {
                ViewBag.url = $"https://docs.google.com/gview?url=http://{Request.Url.Host}{newsobj.FilePath}&embedded=true";
            }

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
            //new DocumentBO().ViewAdd(Id);
        }
    }
}
