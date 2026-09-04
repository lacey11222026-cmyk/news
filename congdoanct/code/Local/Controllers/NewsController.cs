using BIZ;
using BIZ.Entity;
using Local.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;

namespace Local.Controllers
{
     [Authorize]
    public class NewsController : Controller
    {
        //
        // GET: /News/

        #region"Child"
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult Breadcrumb(int CategoryId)
        {
            var data = new CategoryBO().GetCategoryFull(CategoryId);
            return PartialView("BreadcrumbObj", data);
        }
        [ChildActionOnly]
        public ActionResult BreadcrumbObj(CATEGORY_FULL data)
        {

            return PartialView(data);
        }
        [ChildActionOnly]
        public ActionResult Relate(List<CONTENT_FULL> data, int CategoryId,string CateName,string HeaderTitle, Boolean PageNextShow = false, int pageNext=1)
        {
            if (data == null)
            {
                var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]);
                data = new ContentBO().GetTopLastestContentFulls(PageSize,CategoryId);

            }
            ViewBag.HeaderTitle = HeaderTitle;
            ViewBag.PageNextShow = PageNextShow;
            ViewBag.pageNext = pageNext;
            ViewBag.CategoryId = CategoryId;
            ViewBag.CateName = CateName;
            return PartialView(data);
        }
        [ChildActionOnly]
        public ActionResult Share(CONTENT_FULL data)
        {
            ViewBag.FacebookShare = string.Format("https://facebook.com/sharer.php?u={0}", Request.Url.AbsoluteUri);
            ViewBag.GoogleShare = string.Format("https://plus.google.com/share?url={0}", HttpUtility.UrlEncode(Request.Url.AbsoluteUri));
            ViewBag.TwitterShare = string.Format("http://twitter.com/intent/tweet?url={0}&text=Title of the post&via=your-twitter-handle", HttpUtility.UrlEncode(Request.Url.AbsoluteUri));
            ViewBag.ZingShare = string.Format("http://link.apps.zing.vn/share?u={0}", Request.Url.AbsoluteUri);

            return PartialView(data);
        }
        #endregion
      
        public ActionResult Index(int CategoryId,string CateName,int Page=1,int Type=0)
        {

           
            
            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
            if (cateobj == null)
                return RedirectToAction("Error", "Home");
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

            //var _childCategory = new CategoryBO().GetAllChildCategories(CategoryId, 10, false);
            //if (_childCategory != null)
            //{
            //    return View("Index2", _childCategory);
            //}
            if (Page > 1)
            {
                ViewBag.Title = siteTitle + " Trang-" + Page.ToString() + " | " + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
                ViewBag.Description = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            }
            var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]) * 2;
            int Total=0;
            var articles = new ContentBO().GetPageContentFulls(Page, PageSize, CategoryId, ref Total);

            var Model = new NewsModel { listdata = articles, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = CategoryId };
            ViewBag.Type = Type;
            ViewBag.PageSize = PageSize;
            ViewBag.CategoryId = CategoryId;
            ViewBag.CateName = cateobj.Name;
            return View(Model);
        }

        public ActionResult Index2(List<CATEGORY_FULL> data)
        {


            //var cateobj = new CategoryBO().GetCategoryFull(data.FirstOrDefault().ParentId.Value);
            //if (cateobj == null)
            //    return RedirectToAction("Error", "Home");
            ViewBag.CurrentCategoryId = data.FirstOrDefault().Id;
            ViewBag.ParentCategoryId = data.FirstOrDefault().ParentId;
            //var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            //var siteTitle = cateobj.Name + " | ";
            //var metaKeyword = siteTitle.Replace(" | ", ",");
            //ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            //ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            //ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];

            return View(data);
        }
        protected List<CONTENT_FULL> GetRefArticle(string ids,int Id)
        {
            if (string.IsNullOrEmpty(ids))
                return null;
            var articles = new ContentBO().GetTopContentByIdsFulls(ids, 0, true).ToList();
            if (articles == null)
                return null;
            articles.Remove(articles.Where(x => x.Id == Id).FirstOrDefault());
            return articles;
        }
        public ActionResult Detail(int Id, string Title)
        {
            var newsobj = new ContentBO().GetContentFull(Id);
            if (newsobj == null || newsobj.Status !=4 )
                return RedirectToAction("Error", "Home");
            if (Title != Utils.ConvertToRewriteLink(newsobj.Title))
                return RedirectToAction("Detail", "News", new { Id = Id, Title = Utils.ConvertToRewriteLink(newsobj.Title) });
            var metaDescription = newsobj.Title + " , " + newsobj.CategoryName + " , " + Utils.StripHtmlTag(newsobj.IntroText);
            var siteTitle = newsobj.Title + " | " + newsobj.CategoryName + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = newsobj.Title;

            ViewBag.CateName = newsobj.CateLiteObj.Name;
            ViewBag.CurrentCategoryId = newsobj.CategoryId;
            ViewBag.ParentCategoryId = newsobj.CateLiteObj.ParrentId;
            try
            {
                newsobj.FileParam = JsonConvert.DeserializeObject<FileInfo>(newsobj.Thumbnail);
            }
            catch
            {

                newsobj.FileParam = new FileInfo();
            }
            if (newsobj.FileParam == null)
            {
                newsobj.FileParam = new FileInfo();
            }
            new ContentBO().ViewAdd(Id);
            return View(newsobj);
        }
        public ActionResult Print(int Id)
        {
            var newsobj = new ContentBO().GetContentFull(Id);
            if (newsobj == null || newsobj.Status != 4)
                return RedirectToAction("Error", "Home");

            return View(newsobj);

        }
        [ChildActionOnly]
        public ActionResult LastestNews(string CateName, int CategoryId, int MaxLastestNews = 0)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

            }
            if (string.IsNullOrEmpty(CateName))
            {
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                CateName = cateobj.Name;
            }
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);

            ViewBag.CateName = CateName;
            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            return PartialView(lstdata);
        }
        [ChildActionOnly]
        public ActionResult RightLastestNews(string CateName, int CategoryId, int MaxLastestNews = 0)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

            }
            if (string.IsNullOrEmpty(CateName))
            {
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                CateName = cateobj.Name;
            }
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);

            ViewBag.CateName = CateName;
            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            return PartialView(lstdata);
        }
        [ChildActionOnly]
        #region "Comment,Intro"
        public ActionResult Comment(int ItemId, string ItemName, int Type)
        {

            ViewBag.ItemId = ItemId;
            ViewBag.ItemName = ItemName;
            ViewBag.Type = Type;
            return PartialView();
        }
       
        #endregion

    }
}
