using BIZ;
using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Models;

namespace WebMVC4.Controllers
{
   
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
        #endregion
        [ChildActionOnly]
        public ActionResult Share(CONTENT_FULL data)
        {
            ViewBag.FacebookShare = string.Format("https://facebook.com/sharer.php?u={0}", Request.Url.AbsoluteUri);
            ViewBag.GoogleShare = string.Format("https://plus.google.com/share?url={0}", HttpUtility.UrlEncode(Request.Url.AbsoluteUri));
            ViewBag.TwitterShare = string.Format("http://twitter.com/intent/tweet?url={0}&text=Title of the post&via=your-twitter-handle", HttpUtility.UrlEncode(Request.Url.AbsoluteUri));
            ViewBag.ZingShare = string.Format("http://link.apps.zing.vn/share?u={0}", Request.Url.AbsoluteUri);

            return PartialView(data);
        }
        public ActionResult Print(int Id)
        {
            var newsobj = new ContentBO().GetContentFull(Id);
            if (newsobj == null || newsobj.Status != 4)
                return RedirectToAction("Error", "Home");

            return View(newsobj);

        }
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

            if (Page > 1)
            {
                ViewBag.Title = siteTitle + " Trang-" + Page.ToString() + " | " + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
                ViewBag.Description = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            }
            var PageSize = 7;
            int Total = 0;

            var lstHotNews = new ContentBO().GetHotNews(CategoryId, 7);
            //ViewBag.hotnews = lstHotNews;
            var lstNotId = "";
            foreach (var item in lstHotNews)
            {
                lstNotId += item.Id + ",";
            }
            if (CategoryId == 9|| CategoryId == 16)
            {
                lstNotId = "";
                lstHotNews = null;
            }
            var articles = new ContentBO().GetPageContentFullsFrontend(Page, PageSize, CategoryId, ref Total, "", "", "", lstNotId);

            var Model = new NewsModel { listdata = articles, hotnews=lstHotNews, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = CategoryId };
            ViewBag.Type = Type;
            ViewBag.PageSize = PageSize;
            ViewBag.CategoryId = CategoryId;
            ViewBag.CateName = cateobj.Name;
            return View(Model);
        }
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
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult SendComment(Comment doc)
        {
            var ReturnData = new ReturnData();
            try
            {
                if (Session["comment"] == null)
                {
                    Session["comment"] = "1";
                }
                var countsession = Convert.ToInt32(Session["comment"].ToString());
                Session["comment"] = (countsession + 1).ToString();
                if (countsession <= 20)
                {
                    doc.Published = 0;
                    doc.CreatedTime = DateTime.Now;
                    var result = new CommentBO().CreateUpdateComment(doc);
                    ReturnData.ResponseCode = result;
                    if (result >= 0)
                    {

                        ReturnData.Description = "Gửi bình luận thành công, bình luận của bạn sẽ được kiểm duyệt và hiển thị trong ít phút tới";
                    }
                    else switch (result)
                        {

                            default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                        }
                }
                else
                {
                    ReturnData.Description = "Số lần gửi bình luận quá nhiều";
                }

                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
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
            ViewBag.PageClass = "detail";
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
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            
            var lstRef = GetRefArticle(newsobj.Params,Id);
            ViewBag.lstRef = lstRef;
            ViewBag.MainImage = newsobj.MainImage;
            ViewBag.CurrentCategoryId = newsobj.CategoryId;
            ViewBag.ParentCategoryId = newsobj.CateLiteObj.ParrentId;

            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();

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
        public ActionResult Preview(int Id)
        {
            ViewBag.PageClass = "detail";
            var newsobj = new ContentBO().GetContentFull(Id);
            if (newsobj == null )
                return RedirectToAction("Error", "Home");
            
            var metaDescription = newsobj.Title + " , " + newsobj.CategoryName + " , " + Utils.StripHtmlTag(newsobj.IntroText);
            var siteTitle = newsobj.Title + " | " + newsobj.CategoryName + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];

            var lstRef = GetRefArticle(newsobj.Params, Id);
            ViewBag.lstRef = lstRef;
            ViewBag.MainImage = newsobj.MainImage;
            ViewBag.CurrentCategoryId = newsobj.CategoryId;
            ViewBag.ParentCategoryId = newsobj.CateLiteObj.ParrentId;

            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
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
            return View("Detail", newsobj);
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
            //ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);

            //ViewBag.CateName = CateName;
            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,
                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            return PartialView(model);
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
            //ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);

            //ViewBag.CateName = CateName;
            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,
                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        public ActionResult Search(string q = "", string fromdate = "", string todate = "", int categoryId = -1, int Page = 1)
        {
            // keyword = Utils.FormatKeywordSearch(keyword);
            var _staticCategoryList = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.News).ToList();
            var listcategory = _staticCategoryList;
            listcategory.Insert(0, new CATEGORY_FULL { Id = -1, Name = "--Tất cả chuyên mục--" });

            ViewBag.CategoryList = listcategory;
            ViewBag.CategoryId = categoryId;
            var siteTitle = "Tìm kiếm từ khóa " + q + " |";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            var pageSize = 20; // Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]);
            int total = 0;
            var articles = new ContentBO().GetFilterContentFullsPaged(Page, pageSize, q, categoryId, null, 4, "", ref total, fromdate, todate);
            var model = new NewsModel
            {
                listdata = articles,
                pageIndex = Page,
                pageSize = pageSize,
                total = total

            };
            ViewBag.CategoryId = categoryId;
            ViewBag.PageSize = pageSize;
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.keyword = q;
            return View(model);
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
