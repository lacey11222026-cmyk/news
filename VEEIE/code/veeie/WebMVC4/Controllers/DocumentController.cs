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
using DATA;
using WebMVC4.Helper;

namespace WebMVC4.Controllers
{
    public class DocumentController : Controller
    {
        //
        // GET: /Document/
        [LocalizationActionFilter]
        public ActionResult Index(int Id, string fromdate, string todate, string keyword, int Page = 1)
        {
            var cateobj = new CategoryBO().GetCategoryFull(Id);
            if (cateobj == null)
                return RedirectToAction("Error", "Home");

            ViewBag.CurrentCategoryId = cateobj.Id;
            ViewBag.ParentCategoryId = cateobj.ParentId;

            var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            var siteTitle = cateobj.Name ;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle;
            if (Page > 1)
            {
                ViewBag.Title = siteTitle + " Trang-" + Page.ToString() + " | " + Resources.Global.SiteTitle;
                ViewBag.Description = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(Resources.Global.SiteDescription);
            }
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
            if (cateobj.Id ==19 || cateobj.ParentId == 19)
            {
                lstcate = new CategoryBO().GetAllChildCategories(19, 10, false).Where(x=>x.Type==5).ToList();
            }
            else if (cateobj.Id == 6 || cateobj.ParentId == 6)
            {
                lstcate = new CategoryBO().GetAllChildCategories(6, 10, false).Where(x => x.Type == 5).ToList();
            }
           

            var Model = new DocumentModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = Id, subcate = lstcate };


            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.keyword = keyword;

            if(cateobj.Id==19 || cateobj.Id==6|| cateobj.ParentId == 19 || cateobj.ParentId == 6)
            {
                return View("Index2",Model);
            }
            return View(Model);

        }
        [LocalizationActionFilter]
        public ActionResult Detail(int Id)
        {
            var newsobj = new DocumentBO().GetDocumentFull(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");

            var metaDescription = newsobj.Name + " , " + Utils.StripHtmlTag(newsobj.Description);
            
            //var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords =ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = newsobj.Name;

            ViewBag.CurrentCategoryId = newsobj.CategoryId;

            //var cateboj = new CategoryBO().GetCategoryFull(newsobj.CategoryId.Value);
            //if (cateboj != null)
            //    ViewBag.ParentCategoryId = cateboj.ParentId;
            //ViewBag.ParentCategoryId = newsobj.CateLiteObj.ParrentId;
           

            var lstRef = new DocumentBO().GetTopLastestDocumentsFull(4, int.Parse(newsobj.CategoryId.ToString()));
            ViewBag.lstRef = lstRef;
            if (newsobj.CategoryPathway.Contains(",19,")|| newsobj.CategoryPathway.Contains(",6,"))
            {
                return View("Detail2", newsobj);
            }
            return View(newsobj);
        }
        [LocalizationActionFilter]
        public ActionResult ConfirmDownload(int Id,string HeaderTitle,string Url)
        {
            var newsobj = new Feedback();
            if(Session["FeedBack"]!=null)
            {
                var obj = WorkContext.GetSessionKey("FeedBack");
                if(obj!=null)
                {
                    newsobj = obj as Feedback;
                    newsobj.DocId = Id;
                    newsobj.Id = 0;
                }
            }
            ViewBag.Id = Id;
            ViewBag.HeaderTitle = HeaderTitle;
            ViewBag.Url = Url;
            return PartialView(newsobj);
        }

        [LocalizationActionFilter]
        public ActionResult ViewFile(int Id)
        {
            var newsobj = new DocumentBO().GetDocumentFull(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");
            if (newsobj.FileName.ToLower().Equals("rar") || newsobj.FileName.ToLower().Equals("zip") || newsobj.FileName.ToLower().Equals("7z"))
            {
                return RedirectToAction("DownloadFile", new { Id = Id });
            }
            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = newsobj.Name;
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
            new DocumentBO().ViewAdd(Id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Feedback(Feedback data)
        {
            if (Session["sendfeedback"] == null)
            {
                Session["sendfeedback"] = "1";
            }

            var countsession = Convert.ToInt32(Session["sendfeedback"].ToString());
            if (countsession > 5)
            {
                return Json(-4);
            }
            Session["FeedBack"] = data;
            var result = new FeedbackBO().CreateUpdateFeedback(data);
           
            Session["sendfeedback"] = (countsession + 1).ToString();

           
            return Json(result > 0 ? 1 : result);



        }
    }
}
