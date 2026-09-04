using System.Globalization;
using BIZ;
using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebEN.Helper;
using WebEN.Models;

namespace WebEN.Controllers
{

    public class NewsController : BaseController
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
        public ActionResult Share(CONTENT_FULL data)
        {
            ViewBag.FacebookShare = string.Format("https://facebook.com/sharer.php?u={0}", Request.Url.AbsoluteUri);
            ViewBag.GoogleShare = string.Format("https://plus.google.com/share?url={0}", HttpUtility.UrlEncode(Request.Url.AbsoluteUri));
            ViewBag.TwitterShare = string.Format("http://twitter.com/intent/tweet?url={0}&text=Title of the post&via=your-twitter-handle", HttpUtility.UrlEncode(Request.Url.AbsoluteUri));
            ViewBag.ZingShare = string.Format("http://link.apps.zing.vn/share?u={0}", Request.Url.AbsoluteUri);

            return PartialView(data);
        }
        [ChildActionOnly]
        public ActionResult Relate(List<CONTENT_FULL> data, int CategoryId, string CateName, string HeaderTitle, Boolean PageNextShow = false, int pageNext = 1)
        {
            if (data == null)
            {
                var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]);
                data = new ContentBO().GetTopLastestContentFulls(PageSize, CategoryId);

            }
            ViewBag.HeaderTitle = HeaderTitle;
            ViewBag.PageNextShow = PageNextShow;
            ViewBag.pageNext = pageNext;
            ViewBag.CategoryId = CategoryId;
            //ViewBag.CateName = CateName;
            return PartialView(data);
        }
        [ChildActionOnly]
        public ActionResult RelateDocument(List<CONTENT_FULL> data, int CategoryId, string CateName, string HeaderTitle, Boolean PageNextShow = false, int pageNext = 1)
        {
            if (data == null)
            {
                var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]);
                data = new ContentBO().GetTopLastestContentFulls(PageSize, CategoryId);

            }
            ViewBag.HeaderTitle = HeaderTitle;
            ViewBag.PageNextShow = PageNextShow;
            ViewBag.pageNext = pageNext;
            ViewBag.CategoryId = CategoryId;
            //ViewBag.CateName = CateName;
            return PartialView(data);
        }
        [ChildActionOnly]
        public ActionResult Pagging(int pageIndex, int pageSize, int total, int CategoryId, string CateName, Boolean PageNextShow = false, int pageNext = 1)
        {
            ViewBag.pageIndex = pageIndex;
            ViewBag.pageSize = pageSize;
            ViewBag.total = total;
            ViewBag.PageNextShow = PageNextShow;
            ViewBag.pageNext = pageNext;
            ViewBag.CategoryId = CategoryId;
            //ViewBag.CateName = CateName;

            return PartialView();
        }
        #endregion
        #region Search
        public ActionResult Tag(string keyword = "", int page = 1)
        {

            var siteTitle = "Tìm kiếm từ khóa " + keyword + " |";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            var pageSize = 20; // Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]);
            int total = 0;
            var articles = new ContentBO().GetFilterContentFullsPaged(page, pageSize, keyword, -1, null, 1, string.Empty, ref total, -1, "", "");
            var model = new NewsModel
            {
                listdata = articles,
                pageIndex = page,
                pageSize = pageSize,
                total = total

            };

            ViewBag.PageSize = pageSize;

            ViewBag.keyword = keyword;
            return View(model);
        }
        public ActionResult Search(string keyword = "", string fromdate = "", string todate = "", int categoryId = -1, int page = 1)
        {
            if (keyword.ToLower().Contains("game") || keyword.ToLower().Contains("sex") || keyword.ToLower().Contains("bet"))
            {
                return RedirectToAction("Index", "Home");
            }
            keyword = Utils.FormatKeywordSearch(keyword);
            var _staticCategoryList = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.News).Where(x => x.Language == "en-us").ToList();
            var listcategory = new CategoryBO().GetCategoryByUserName(_staticCategoryList,"", true);
            listcategory.Insert(0, new CATEGORY_FULL { Id = -1, Name = "--All Category--" });

            ViewBag.CategoryList = listcategory;
            ViewBag.CategoryId = categoryId;
            var siteTitle = "Tìm kiếm từ khóa";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            var pageSize = 20; // Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]);
            int total = 0;
            if(string.IsNullOrEmpty(keyword))
            {
                return RedirectToAction("Index", "Home");
            }    
            var articles = new ContentBO().GetPageContentFullsFrontend(page, pageSize, categoryId, ref total, fromdate, todate, keyword,"","en-us");
            var model = new NewsModel
            {
                listdata = articles,
                pageIndex = page,
                pageSize = pageSize,
                total = total

            };
            ViewBag.CategoryId = categoryId;
            ViewBag.PageSize = pageSize;
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.keyword = keyword;
            return View(model);
        }
        public ActionResult SearchByDay(string keyword = "", string fromdate = "", string todate = "", int categoryId = -1, int page = 1)
        {

            if (keyword.ToLower().Contains("game") || keyword.ToLower().Contains("sex") || keyword.ToLower().Contains("bet"))
            {
                return RedirectToAction("Index", "Home");
            }
            keyword = Utils.FormatKeywordSearch(keyword);
            //fromdate = new DateTime(2000, 1, 1).ToString("dd/MM/yyyy");
            var _staticCategoryList = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.News).Where(x => x.Language == "en-us").ToList();
            var listcategory = new CategoryBO().GetCategoryByUserName(_staticCategoryList, "", true);
            listcategory.Insert(0, new CATEGORY_FULL { Id = -1, Name = "--All Category--" });

            ViewBag.CategoryList = listcategory;
            ViewBag.CategoryId = categoryId;
            var siteTitle = "Tìm kiếm từ khóa " + keyword + " |";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            var pageSize = 20;
            int total = 0;
            var articles = new ContentBO().GetPageContentFullsFrontend(page, pageSize, categoryId, ref total, fromdate, todate, keyword,"","en-us");
           
            var model = new NewsModel
            {
                listdata = articles,
                pageIndex = page,
                pageSize = pageSize,
                total = total

            };
            ViewBag.CategoryId = categoryId;
            ViewBag.PageSize = pageSize;
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.keyword = keyword;
            return View(model);
        }
       
        #endregion
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Index(int CategoryId, string CateName, int Page = 1, int Type = 0)
        {

            var MaxHotNews = 6;
           


            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
            if (cateobj == null)
                return RedirectToAction("Error", "Home");
            if (CateName != Utils.ConvertToRewriteLink(cateobj.Name))
                return RedirectToAction("Index", "News", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(cateobj.Name) });
          

            ViewBag.CurrentCategoryId = CategoryId;
            //ViewBag.CateName = cateobj.Name;


            var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            var siteTitle = cateobj.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle ;
            int Total = 0;
            
            //if (Page > 1)
            //{
            //    ViewBag.Title = siteTitle + " Trang-" + Page.ToString() + " | " + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            //    ViewBag.Description = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            //}
            var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]) * 2;
            //forwar sang trang tài liệu
            if (CategoryId == 12 || CategoryId == 13)
            {
                var lstDoc = new ContentBO().GetPageContentFullsFrontend(Page, PageSize, CategoryId, ref Total);
                var model2 = new NewsModel { CategoryId = CategoryId, pageIndex = Page, listdata = lstDoc, total = Total, pageSize = PageSize };
                return View("Index2", model2);
            }
            var lstHotNews = new ContentBO().GetHotNews(CategoryId, MaxHotNews);
            //ViewBag.hotnews = lstHotNews;
            var lstNotId = "";
            foreach (var item in lstHotNews)
            {
                lstNotId += item.Id + ",";
            }
            
            var articles = new ContentBO().GetPageContentFullsFrontend(Page, PageSize, CategoryId, ref Total, "", "", "", lstNotId);

            var pageNext = Page + 1;
            var pageNextShow = false;
            if (Total <= PageSize)
            {
                pageNext = 1;
                pageNextShow = false;
            }
            else
            {
                pageNextShow = true;
            }
            ViewBag.Total = Total;
            ViewBag.Page = Page;
            ViewBag.PageNextShow = pageNextShow;
            ViewBag.pageNext = pageNext;
            ViewBag.Type = Type;
            ViewBag.PageSize = PageSize;
            //ViewBag.CategoryId = CategoryId;
            //ViewBag.CateName = CateName;

            var otherTotal = 0;
            ViewBag.FirstNews = new ContentBO().GetPageContentFulls(1, PageSize, CategoryId, ref otherTotal).First();

            var model = new News2Model
            {
                hotnews = lstHotNews,
                articles = articles
            };
            return View(model);
        }

        public ActionResult Index2(News2Model data)
        {



            return View(data);
        }
        protected List<CONTENT_FULL> GetRefArticle(string ids, long Id)
        {
            try
            {
                if (string.IsNullOrEmpty(ids))
                    return null;
                var articles = new ContentBO().GetTopContentByIdsFulls(ids, 0, true).ToList();
                if (articles == null)
                    return null;
                articles.Remove(articles.Where(x => x.Id == Id).FirstOrDefault());
                return articles;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public ActionResult Print(int Id)
        {
            var newsobj = new ContentBO().GetContentFull(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");
            ViewBag.Title = newsobj.Title;
            return View(newsobj);

        }
        //[Authorize]
        public ActionResult DetailReview(long Id)
        {
            var newsobj = new ContentBO().GetContentFull(Id);
            if (newsobj == null)
                return RedirectToAction("Error", "Home");
            var lstRef = GetRefArticle(newsobj.Params, Id);
            ViewBag.lstRef = lstRef;
            var cateobj = new CategoryBO().GetCategoryFull(newsobj.CategoryId.Value);
            //var orthercategory = MvcApplication.StaticCategoryList.Where(x => x.ParentId == cateobj.ParentId && x.Published == 1 && x.ParentId != 0).ToList();
            //ViewBag.orthercategory = orthercategory;
            return View(newsobj);
        }

        public ActionResult Detail(long Id, string Title, string CateName)
        {
            var newsobj = new ContentBO().GetContentFull(Id);
            if (newsobj == null || newsobj.Status != 1)
                return RedirectToAction("Error", "Home");

            var categoryobj = new CategoryBO().GetCategoryFull(newsobj.CategoryId.Value);
            if (Title != Utils.ConvertToRewriteLink(newsobj.Title))
                return RedirectToAction("Detail", "News", new { Id = Id, Title = Utils.ConvertToRewriteLink(newsobj.Title), CateName = Utils.ConvertToRewriteLink(categoryobj.Name) });


            if (CateName != Utils.ConvertToRewriteLink(categoryobj.Name))
                return RedirectToAction("Detail", "News", new { Id = Id, Title = Utils.ConvertToRewriteLink(newsobj.Title), CateName = Utils.ConvertToRewriteLink(categoryobj.Name) });
            
            var metaDescription = Utils.StripHtmlTag(newsobj.IntroText);
            var siteTitle = newsobj.Title ;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle ;

            var lstRef = GetRefArticle(newsobj.Params, Id);
            ViewBag.lstRef = lstRef;

            ViewBag.CurrentCategoryId = newsobj.CategoryId;
            //ViewBag.ParentCategoryId = newsobj.CateLiteObj.ParrentId;

            ViewBag.ParentCategoryId = categoryobj.ParentId;

            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            ViewBag.MainImage = newsobj.MainImage;
            ViewBag.NewsDescription = newsobj.IntroText;
            ViewBag.NewsTitle = newsobj.Title;


            Action<long,int> send = ViewAdd;
            var asynSend = send.BeginInvoke(newsobj.Id,newsobj.CategoryId.GetValueOrDefault(), null, null);
            //var orthercategory = MvcApplication.StaticCategoryList.Where(x => x.ParentId == categoryobj.ParentId && x.Published == 1 && x.ParentId != 0).ToList();
            //ViewBag.orthercategory = orthercategory;
            return View(newsobj);
        }
        private void ViewAdd(long Id, int CategoryId)
        {
            new ContentBO().ViewAdd(Id, CategoryId);
        }
        [ChildActionOnly]
        public ActionResult VideoReview(string url)
        {
            ViewBag.Url = url;
            return PartialView();
        }

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult TopViewNews(int CategoryId, int MaxLastestNews = 0)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

            }
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);

            if (mobileHelper.DetectMobileLong())
            {
                MaxLastestNews = 4;
            }
            var todate = DateTime.Now.ToString("dd/MM/yyyy");
            var fromdate = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
            var lstdata = new ContentBO().GetTopViewContentFulls(MaxLastestNews, CategoryId, fromdate, todate,"en-us");
            if (lstdata == null || lstdata.Count < MaxLastestNews)
            {
                fromdate = DateTime.Now.AddMonths(-3).ToString("dd/MM/yyyy");
                lstdata = new ContentBO().GetTopViewContentFulls(MaxLastestNews, CategoryId, fromdate, todate, "en-us");
            }
            return PartialView(lstdata);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult TopViewNews2(int CategoryId, int MaxLastestNews = 0)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

            }

            var todate = DateTime.Now.ToString("dd/MM/yyyy");
            var fromdate = DateTime.Now.AddMonths(-3).ToString("dd/MM/yyyy");
            var lstdata = new ContentBO().GetTopViewContentFulls(MaxLastestNews, CategoryId, "", "","en-us");
            if (lstdata == null || lstdata.Count < MaxLastestNews)
            {
                fromdate = DateTime.Now.AddMonths(-12).ToString("dd/MM/yyyy");
                lstdata = new ContentBO().GetTopViewContentFulls(MaxLastestNews, CategoryId, fromdate, todate, "en-us");
                if (lstdata == null || lstdata.Count < MaxLastestNews)
                {
                    fromdate = DateTime.Now.AddMonths(-24).ToString("dd/MM/yyyy");
                    lstdata = new ContentBO().GetTopViewContentFulls(MaxLastestNews, CategoryId, fromdate, todate, "en-us");
                }
            }
            return PartialView(lstdata);
        }
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult HotNews(List<CONTENT_FULL> lstdata)
        {
            try
            {
                //if (MaxLastestNews == 0)
                //{
                //    MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

                //}
                //if (string.IsNullOrEmpty(CateName))
                //{
                //    var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                //    CateName = cateobj.Name;
                //}
                //ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);

                //ViewBag.CateName = CateName;
                //var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId).ToList();

                //var configValue = new SystemConfigBO().GetByKey("HotNews_" + CategoryId);
                //if (configValue != null)
                //{
                //    var lstid = configValue.ConfigValue;
                //    if (string.IsNullOrEmpty(lstid))
                //    {
                //        return PartialView(lstdata);
                //    }
                //    var lstcontent = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true).ToList();

                //    if (lstcontent == null || lstcontent.Count < 1)
                //    {
                //        return PartialView(lstdata);
                //    }

                //    foreach (var item in lstdata)
                //    {

                //        if (!lstcontent.Where(x => x.Id == item.Id).Any())
                //        {
                //            lstcontent.Add(item);

                //        }
                //    }
                //    lstcontent = lstcontent.Take(MaxLastestNews).ToList();
                //    return PartialView(lstcontent);
                //}
                var request = System.Web.HttpContext.Current.Request;
                var mobileHelper = new MobileDetectHelper(request);
                ViewBag.IsIpad = mobileHelper.DetectIpad();
                ViewBag.Iphone = mobileHelper.DetectIphone();
                ViewBag.IsMobile = mobileHelper.DetectMobileLong();
                return PartialView(lstdata);
            }
            catch (Exception)
            {
                return PartialView(null);
                throw;
            }

        }
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult LastestNews( int MaxLastestNews = 0)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

            }
            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, 0, "en-us");
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
