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
using WebMVC4.Filter;
using WebMVC4.Helper;
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
        public ActionResult Relate(List<CONTENT_FULL> data, int CategoryId)
        {
            if (data == null)
            {
                var PageSize = 10;
                data = new ContentBO().GetTopLastestContentFulls(PageSize, CategoryId);

            }
            //ViewBag.HeaderTitle = HeaderTitle;
            //ViewBag.PageNextShow = PageNextShow;
            //ViewBag.pageNext = pageNext;
            ViewBag.CategoryUrl = new CategoryBO().GetCategoryFull(CategoryId).Url;
            //ViewBag.CateName = CateName;
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
        [LocalizationActionFilter]
        public ActionResult Print(int Id)
        {
            var newsobj = new ContentBO().GetContentFull(Id);
            if (newsobj == null || newsobj.Status != 4)
                return RedirectToAction("Error", "Home");
            ViewBag.Tilte = newsobj.Title;
            return View(newsobj);

        }
        public ActionResult TopViewNews(int CategoryId, int MaxLastestNews = 0)
        {

            //ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);

            //ViewBag.CateName = CateName;
            var todate = DateTime.Now.ToString("dd/MM/yyyy");
            var fromdate = DateTime.Now.AddMonths(-12).ToString("dd/MM/yyyy");
            var lstdata = new ContentBO().GetTopViewContentFulls(MaxLastestNews, CategoryId, "", fromdate, todate);
            if (lstdata == null || lstdata.Count < 3)
            {
                lstdata = new ContentBO().GetTopViewContentFulls(MaxLastestNews, CategoryId, "", "", "");
            }
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = "",
                Url = "",
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [LocalizationActionFilter]
        public ActionResult Index(int CategoryId, string CateName, int Page = 1, int Type = 0)
        {


            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
            if (cateobj == null)
                return RedirectToAction("Error", "Home");
            if (CateName != Utils.ConvertToRewriteLink(cateobj.Name))
                return RedirectToAction("Index", "News", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(cateobj.Name) });

            if (WorkContext.GetLanguage() != cateobj.Language)
            {
                WorkContext.SetLanguage(cateobj.Language);
                return RedirectToAction("Index", "News", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(cateobj.Name) });
            }
            ViewBag.CurrentCategoryId = cateobj.Id;
            ViewBag.ParentCategoryId = cateobj.ParentId;

            ViewBag.Url = cateobj.Url;
            ViewBag.CateName = cateobj.Name;
            var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            var siteTitle = cateobj.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + Resources.Global.SiteTitle;
            ViewBag.CategoryId = CategoryId;


            var lstHotNews = new ContentBO().GetHotNews(CategoryId, 4);
            //ViewBag.hotnews = lstHotNews;
            var lstNotId = "";
            foreach (var item in lstHotNews)
            {
                lstNotId += item.Id + ",";
            }



            //var _childCategory = new CategoryBO().GetAllChildCategories(CategoryId, 10, false);
            //if (_childCategory != null)
            //{

            //    ViewBag.PageClass = "cap1";
            //    var model2 = new News2Model
            //    {
            //        hotnews = lstHotNews,
            //        listcate = _childCategory
            //    };
            //    return View("Index2", model2);
            //}
            if (Page > 1)
            {
                ViewBag.Title = siteTitle + " Trang-" + Page.ToString() + " | " + Resources.Global.SiteTitle;
                ViewBag.Description = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(Resources.Global.SiteDescription);
            }
            var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]) * 2;
            if (CategoryId == 18)
            {
                lstNotId = "";
                PageSize = 10;
            }

            if (CategoryId == 66 )
            {
                lstNotId = "";
            }
            int Total = 0;
            //var articles = new ContentBO().GetPageContentFulls(Page, PageSize, CategoryId, ref Total);
            var articles = new ContentBO().GetPageContentFullsFrontend(Page, PageSize, CategoryId, ref Total, "", "", "", lstNotId);
            var pageNext = Page + 1;
            var PageNextShow = false;
            if (Total <= PageSize)
            {
                pageNext = 1;
                PageNextShow = false;
            }
            else
            {
                PageNextShow = true;
            }
            ViewBag.Total = Total;
            ViewBag.Page = Page;
            ViewBag.PageNextShow = PageNextShow;
            ViewBag.pageNext = pageNext;
            ViewBag.Type = Type;
            ViewBag.PageSize = PageSize;



            ViewBag.PageClass = "list";
            var model = new NewsModel { listdata = articles, hotnews = lstHotNews, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = CategoryId };

            if (CategoryId == 18)
                return View("Index2", model);
            if (CategoryId == 66)
            {
                return View("Index6", model);
            }
            return View(model);
        }
        [LocalizationActionFilter]
        public ActionResult Index9(int Page = 1)
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];

            ViewBag.MainImage = "/images/logo1.jpg";
            ViewBag.NewsDescription = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            ViewBag.NewsTitle = ConfigurationManager.AppSettings["DefMetaDescription"];

            int Total = 0;


            var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]);
            var lang = WorkContext.GetLanguage();

            var lstHotNews = new ContentBO().GetTopLastestContentFulls(4, -1, -1, lang, 1);
            //ViewBag.hotnews = lstHotNews;
            var lstNotId = "";
            foreach (var item in lstHotNews)
            {
                lstNotId += item.Id + ",";
            }

            var articles = new ContentBO().GetPageContentFullsFrontend(Page, PageSize, -1, ref Total, "", "", "", lstNotId, lang, -1, 1);

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

            ViewBag.PageSize = PageSize;
            //ViewBag.CategoryId = CategoryId;
            //ViewBag.CateName = CateName;

            //var otherTotal = 0;
            //ViewBag.FirstNews = new ContentBO().GetPageContentFulls(1, PageSize, CategoryId, ref otherTotal).First();

            var model = new News2Model
            {
                hotnews = lstHotNews,
                CategoryId = 0,
                articles = articles
            };

            return View(model);
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
        protected List<CONTENT_FULL> GetRefArticle(string ids, int Id)
        {
            if (string.IsNullOrEmpty(ids))
                return null;
            var articles = new ContentBO().GetTopContentByIdsFulls(ids, 0, true).ToList();
            if (articles == null)
                return null;
            articles.Remove(articles.Where(x => x.Id == Id).FirstOrDefault());
            return articles;
        }
        [LocalizationActionFilter]
        public ActionResult Detail(int Id, string Title)
        {
            ViewBag.PageClass = "detail";
            var newsobj = new ContentBO().GetContentFull(Id);
            if (newsobj == null || newsobj.Status != 4)
                return RedirectToAction("Error", "Home");
            if (Title != Utils.ConvertToRewriteLink(newsobj.Title))
                return RedirectToAction("Detail", "News", new { Id = Id, Title = Utils.ConvertToRewriteLink(newsobj.Title) });
            if (newsobj.Type == 2 && newsobj.CategoryId!=66)
            {
                return RedirectToAction("Index", "Video", new { VideoId = Id });
            }
            var metaDescription = Utils.StripHtmlTag(newsobj.IntroText);
            var siteTitle = newsobj.Title;
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle;
            ViewBag.CateName = newsobj.CateLiteObj.Name;
            ViewBag.CateUrl = newsobj.CateLiteObj.Url;
            var lstRef = GetRefArticle(newsobj.Params, Id);
            ViewBag.lstRef = lstRef;
            ViewBag.MainImage = newsobj.MainImage;
            ViewBag.CategoryId = newsobj.CategoryId;
            ViewBag.ParentCategoryId = newsobj.CateLiteObj.ParrentId;
            ViewBag.FacebookLike = string.Format("<div class=\"fb-like\" data-href=\"{0}\" data-layout=\"button_count\" data-action=\"like\" data-show-faces=\"false\" data-share=\"true\"></div>", Request.Url.AbsoluteUri);
            ViewBag.FacebookShare = string.Format("https://facebook.com/sharer.php?u={0}", Request.Url.AbsoluteUri);
            if (WorkContext.GetLanguage() != newsobj.CateLiteObj.Language)
            {
                WorkContext.SetLanguage(newsobj.CateLiteObj.Language);
                return RedirectToAction("Detail", "News", new { Id = Id, Title = Utils.ConvertToRewriteLink(newsobj.Title) });
            }

            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();

            Action<int, int> addview = ViewAdd;
            var asynSendNoti = addview.BeginInvoke(Id, newsobj.CategoryId.GetValueOrDefault(), null, null);
            // var asynSendNoti = addview.BeginInvoke(newsobj.Id, null, null);

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

            if (newsobj.CategoryId == 53|| newsobj.CategoryId == 54)
            {
                return View("Detail39", newsobj);
            }
            if (newsobj.CategoryId == 66)
            {
                return View("Detail" + newsobj.Type.ToString(), newsobj);
            }
            return View(newsobj);
        }
        private void ViewAdd(int Id, int CategoryId)
        {
            new ContentBO().ViewAdd(Id, CategoryId);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AudioViewAdd(int Id)
        {

            new ContentBO().ViewAdd(Id, -99);
            return base.Json(new
            {
                success = true,
                statusCode = 1,
                msg = "success"
            });
        }
        [LocalizationActionFilter]
        public ActionResult Preview(int Id)
        {
            ViewBag.PageClass = "detail";
            var newsobj = new ContentBO().GetContentFull(Id);
            if (newsobj == null)
                return RedirectToAction("Error", "Home");

            var metaDescription = newsobj.Title + " , " + newsobj.CategoryName + " , " + Utils.StripHtmlTag(newsobj.IntroText);
            var siteTitle = newsobj.Title + " | " + newsobj.CategoryName + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + Resources.Global.SiteTitle;

            var lstRef = GetRefArticle(newsobj.Params, Id);
            ViewBag.lstRef = lstRef;
            ViewBag.MainImage = newsobj.MainImage;
            ViewBag.CurrentCategoryId = newsobj.CategoryId;
            ViewBag.ParentCategoryId = newsobj.CateLiteObj.ParrentId;
            ViewBag.CateName = newsobj.CateLiteObj.Name;
            ViewBag.CateUrl = newsobj.CateLiteObj.Url;
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            ViewBag.CateName = newsobj.CateLiteObj.Name;
            ViewBag.CateUrl = newsobj.CateLiteObj.Url;
            ViewBag.FacebookLike = string.Format("<div class=\"fb-like\" data-href=\"{0}\" data-layout=\"button_count\" data-action=\"like\" data-show-faces=\"false\" data-share=\"true\"></div>", Request.Url.AbsoluteUri);
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
          
            if (newsobj.CategoryId == 53 || newsobj.CategoryId == 54)
            {
                return View("Detail39", newsobj);
            }
            if (newsobj.CategoryId == 66)
            {
                return View("Detail" + newsobj.Type.ToString(), newsobj);
            }
            if (newsobj.Type == 2)
            {
                return RedirectToAction("Index", "Video", new { VideoId = Id });
            }
            return View("Detail", newsobj);
        }

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

        public ActionResult LastestNews2(string CateName, int CategoryId, int MaxLastestNews = 0)
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
        public ActionResult RightLastestNews(string lang, int MaxLastestNews = 0)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

            }

            //ViewBag.CateName = CateName;
            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, -1, -1, lang);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = "Tin mới",
                Url = "/",
                CategoryId = 1
            };
            if (lang != "vi-vn")
                model.HeaderTitle = "Latest News";
            return PartialView(model);
        }
        [LocalizationActionFilter]
        public ActionResult Search(string q = "", string fromdate = "", string todate = "", int categoryId = -1, int Page = 1)
        {
            if (q.ToLower().Contains("game") || q.ToLower().Contains("sex") || q.ToLower().Contains("bet"))
            {
                return RedirectToAction("Index", "Home");
            }
            // keyword = Utils.FormatKeywordSearch(keyword);
            var _staticCategoryList = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.News).Where(x => x.Language == WorkContext.GetLanguage()).ToList();
            var listcategory = _staticCategoryList;
            //listcategory.Insert(0, new CATEGORY_FULL { Id = -1, Name = "  " });

            ViewBag.CategoryList = listcategory;
            ViewBag.CategoryId = categoryId;
            var siteTitle = "Tìm kiếm từ khóa |";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + Resources.Global.SiteTitle;
            var pageSize = 20; // Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]);
            int total = 0;
            var articles = new ContentBO().GetFilterContentFullsPaged(Page, pageSize, q, categoryId, _staticCategoryList.Where(x => x.Id > 0).Select(x => x.Id).ToList(), 4, "", ref total, fromdate, todate);
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

        public ActionResult Relate3(int CategoryId, int Id, string HeaderTitle, bool PageNextShow = false, int pageNext = 1)
        {
            List<CONTENT_FULL> model = (from x in new ContentBO().GetTopLastestContentFulls(5, CategoryId, -1, "", -1)
                                        where x.Id != Id
                                        select x).Take<CONTENT_FULL>(5).ToList<CONTENT_FULL>();


            ViewBag.HeaderTitle = HeaderTitle;
            ViewBag.PageNextShow = PageNextShow;
            ViewBag.pageNext = pageNext;
            ViewBag.CategoryId = CategoryId;

            return base.PartialView(model);
        }

        public ActionResult Relate4(int CategoryId, int Id, string HeaderTitle, bool PageNextShow = false, int pageNext = 1)
        {
            List<CONTENT_FULL> model = (from x in new ContentBO().GetTopLastestContentFulls(5, CategoryId, -1,"", -1)
                                        where x.Id != Id
                                        select x).Take<CONTENT_FULL>(5).ToList<CONTENT_FULL>();

            ViewBag.HeaderTitle = HeaderTitle;
            ViewBag.PageNextShow = PageNextShow;
            ViewBag.pageNext = pageNext;
            ViewBag.CategoryId = CategoryId;

            return base.PartialView(model);
        }

        public ActionResult Relate5(int CategoryId, int Id, string HeaderTitle, bool PageNextShow = false, int pageNext = 1)
        {
            List<CONTENT_FULL> model = (from x in new ContentBO().GetTopLastestContentFulls(5, CategoryId, -1, "", 0)
                                        where x.Id != Id
                                        select x).Take<CONTENT_FULL>(5).ToList<CONTENT_FULL>();


            ViewBag.HeaderTitle = HeaderTitle;
            ViewBag.PageNextShow = PageNextShow;
            ViewBag.pageNext = pageNext;
            ViewBag.CategoryId = CategoryId;

            return base.PartialView(model);
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
        #endregion

    }
}
