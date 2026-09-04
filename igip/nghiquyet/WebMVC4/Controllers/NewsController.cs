using BIZ;
using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATA;
using Newtonsoft.Json;
using UTILS;
using WebMVC4.Models;
using System.Web.Razor.Tokenizer.Symbols;
using WebMVC4.Filter;

namespace WebMVC4.Controllers
{

    public class NewsController : Controller
    {
        //
        // GET: /News/

        #region"Child"
        [OutputCache(Duration = 120, VaryByParam = "*")]

        public ActionResult Breadcrumb(int CategoryId)
        {
            var data = new CategoryBO().GetCategoryFull(CategoryId);
            return PartialView("BreadcrumbObj", data);
        }

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
            //ViewBag.CategoryId = CategoryId;
            //ViewBag.CateName = CateName;
            return PartialView(data);
        }
        public ActionResult Relate3(int CategoryId, int Id, string HeaderTitle, bool PageNextShow = false, int pageNext = 1)
        {
            List<CONTENT_FULL> model = (from x in new ContentBO().GetTopLastestContentFulls(5, CategoryId, "")
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
            List<CONTENT_FULL> model = (from x in new ContentBO().GetTopLastestContentFulls(5, CategoryId, "")
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
            List<CONTENT_FULL> model = (from x in new ContentBO().GetTopLastestContentFulls(5, CategoryId, "")
                                        where x.Id != Id
                                        select x).Take<CONTENT_FULL>(5).ToList<CONTENT_FULL>();


            ViewBag.HeaderTitle = HeaderTitle;
            ViewBag.PageNextShow = PageNextShow;
            ViewBag.pageNext = pageNext;
            ViewBag.CategoryId = CategoryId;

            return base.PartialView(model);
        }
        #endregion

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
        public ActionResult TopViewNews(string lang,int MaxLastestNews = 0)
        {

            //ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);

            //ViewBag.CateName = CateName;
            var todate = DateTime.Now.ToString("dd/MM/yyyy");
            var fromdate = DateTime.Now.AddMonths(-12).ToString("dd/MM/yyyy");
            var lstdata = new ContentBO().GetTopViewContentFulls(MaxLastestNews*5, -1, lang, fromdate, todate);
            lstdata = lstdata.Where(x => x.CategoryId != 18 && x.CategoryId != 50 && x.Type ==1).Take(MaxLastestNews).ToList();
           
            return PartialView(lstdata);
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
        [LocalizationActionFilter]
        public ActionResult Index(int CategoryId, string CateName, int Page = 1, int Type = 0)
        {

            //if (CategoryId == 18)
            //    return RedirectToAction("Index2", "News");
            //if (CategoryId == 6)
            //    return RedirectToAction("Index", "Video");
            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
            if (cateobj == null)
                return RedirectToAction("Error", "Home");
            if (CateName != Utils.ConvertToRewriteLink(cateobj.Name))
                return RedirectToAction("Index", "News", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(cateobj.Name) });

            ViewBag.CurrentCategoryId = cateobj.Id;
            ViewBag.ParentCategoryId = cateobj.ParentId;



            var metaDescription = Utils.StripHtmlTag(cateobj.Description);
            var siteTitle = cateobj.Name;
            var metaKeyword = siteTitle;
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword;
            ViewBag.Title = siteTitle;
            var lstHotNews = new ContentBO().GetHotNews(CategoryId, 7);
            //ViewBag.hotnews = lstHotNews;
            var lstNotId = "";
            //foreach (var item in lstHotNews)
            //{
            //    lstNotId += item.Id + ",";
            //}
            //if (CategoryId == 50 || CategoryId == 18 || CategoryId == 41 || CategoryId == 42)
            //    lstNotId = "";
            //var _childCategory = new CategoryBO().GetAllChildCategories(CategoryId, 10, false);
            //if (_childCategory != null)
            //{
            //    return View("Index2", _childCategory);
            //}
            //if (Page > 1)
            //{
            //    ViewBag.Title = siteTitle + " Trang-" + Page.ToString() + " | " + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            //    ViewBag.Description = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            //}
            var PageSize = 24;
            int Total = 0;
            var articles = new ContentBO().GetPageContentFullsFrontend(Page, PageSize, CategoryId, ref Total, "", "", "", lstNotId);

            ViewBag.Total = Total;
            ViewBag.Page = Page;
            ViewBag.Type = Type;
            ViewBag.PageSize = PageSize;
            ViewBag.CategoryId = CategoryId;
            ViewBag.CateName = CateName;


            ViewBag.PageClass = "list";
            var model = new News2Model
            {
                hotnews = lstHotNews,
                articles = articles
            };

            if (CategoryId == 5)
                return View("Index3", model);
            //if (CategoryId == 41)
            //    return View("Index3", model);
            //podcast
            if (CategoryId == 6)
                return View("Index4", model);
            return View(model);
        }


        public ActionResult Index2(int Page = 1, int Year = 0)
        {


            var metaDescription = "Bản tin";
            var siteTitle = "Bản tin | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];

            var keyword = "";
            if (Year > 0)
            {
                keyword = Year.ToString();
            }
            var PageSize = 18;
            int Total = 0;
            var articles = new ContentBO().GetPageContentFullsFrontend(Page, PageSize, 18, ref Total, "", "", keyword, "");

            ViewBag.Total = Total;
            ViewBag.Page = Page;
            ViewBag.PageSize = PageSize;
            ViewBag.Year = Year;

            return View(articles);
        }
        protected List<CONTENT_FULL> GetRefArticle(string ids, int Id)
        {

            if (string.IsNullOrEmpty(ids))
                return new List<CONTENT_FULL>();
            try
            {
                var articles = new ContentBO().GetTopContentByIdsFulls(ids, 0, true).ToList();
                if (articles == null)
                    return new List<CONTENT_FULL>();
                articles.Remove(articles.Where(x => x.Id == Id).FirstOrDefault());
                return articles;
            }
            catch
            {
                return new List<CONTENT_FULL>();
            }
        }
        [LocalizationActionFilter]
        public ActionResult Search(string q = "", string fromdate = "", string todate = "", int categoryId = -1, int Page = 1)
        {
            if (q.ToLower().Contains("game") || q.ToLower().Contains("sex") || q.ToLower().Contains("bet"))
            {
                return RedirectToAction("Index", "Home");
            }
            q = Utils.FormatKeywordSearch(q);
            var _staticCategoryList = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.News).ToList();
            var listcategory = _staticCategoryList;
            listcategory.Insert(0, new CATEGORY_FULL { Id = -1, Name = "--Tất cả chuyên mục--" });

            ViewBag.CategoryList = listcategory;
            ViewBag.CategoryId = categoryId;
            var siteTitle = "Tìm kiếm từ khóa |";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            var pageSize = 20; // Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]);
            int total = 0;
            var articles = new ContentBO().GetPageContentFullsFrontend(Page, pageSize, categoryId, ref total, fromdate, todate, q, "", "", -1, Config.WebSite);
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
        public ActionResult Preview(int Id)
        {
            var newsobj = new ContentBO().GetContentFull(Id);
            if (newsobj == null)
                return RedirectToAction("Error", "Home");

            var metaDescription = newsobj.Title + " , " + newsobj.CategoryName + " , " + Utils.StripHtmlTag(newsobj.IntroText);
            var siteTitle = newsobj.Title + " | " + newsobj.CategoryName + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];

            var lstRef = GetRefArticle(newsobj.Params, Id);
            ViewBag.lstRef = lstRef;
            ViewBag.SiteImage = newsobj.MainImage;
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

            if (newsobj.CategoryId == 7)
            {
                return View("Detail" + newsobj.Type.ToString(), newsobj);
            }
            if (newsobj.Type == 2)
            {
                return RedirectToAction("Index", "Video", new { VideoId = Id });
            }
            if (newsobj.Type == 4)
            {
                return RedirectToAction("Index", "Podcast", new { Id = Id });
            }
            return View("Detail", newsobj);
        }
        [LocalizationActionFilter]
        public ActionResult Detail(int Id, string Title)
        {
            var newsobj = new ContentBO().GetContentFull(Id);
            if (newsobj == null || newsobj.Status != 4)
                return RedirectToAction("Error", "Home");
            if (Title != Utils.ConvertToRewriteLink(newsobj.Title))
                return RedirectToAction("Detail", "News", new { Id = Id, Title = Utils.ConvertToRewriteLink(newsobj.Title) });
            //if (newsobj.Type == 2)
            //{
            //    return RedirectToAction("Index", "Video", new { VideoId = Id });
            //}
            var metaDescription = newsobj.IntroText;
            var siteTitle = newsobj.Title;
            var metaKeyword = siteTitle.Replace(" | ", ",");

            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword;
            ViewBag.Title = siteTitle;

            var lstRef = GetRefArticle(newsobj.Params, Id);
            ViewBag.lstRef = lstRef;
            ViewBag.SiteImage = newsobj.MainImage;
            ViewBag.CurrentCategoryId = newsobj.CategoryId;
            ViewBag.ParentCategoryId = newsobj.CateLiteObj.ParrentId;
            ViewBag.Url = newsobj.CateLiteObj.Url;
            var request = System.Web.HttpContext.Current.Request;

            ViewBag.MailShare = String.Format("https://mail.google.com/mail/u/0/?ui=2&view=cm&fs=1&tf=1&su={0}&body={1}", HttpUtility.UrlEncode(newsobj.Title), HttpUtility.UrlEncode(Request.Url.AbsoluteUri));
            ViewBag.FacebookShare = string.Format("https://facebook.com/sharer.php?u={0}", Request.Url.AbsoluteUri);
            Action<int, int> addview = ViewAdd;
            var asynSendNoti = addview.BeginInvoke(Id, newsobj.CategoryId.GetValueOrDefault(), null, null);
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
            //if (newsobj.CategoryId == 40)
            //{
            //    if (string.IsNullOrEmpty(newsobj.Url))
            //    {
            //        newsobj.Url = new CrawlBO().getattr("a", "href", newsobj.Contents).Replace("../../../", "/");
            //    }
            //    //newsobj.Url = $"https://docs.google.com/gview?url=http://{Request.Url.Host}{newsobj.Url}&embedded=true";
            //    return View("Detail2", newsobj);
            //}
            if (newsobj.CategoryId == 7)
            {
                return View("Detail" + newsobj.Type.ToString(), newsobj);
            }
            if (newsobj.Type == 2)
            {
                return RedirectToAction("Index", "Video", new { VideoId = Id });
            }
            if (newsobj.Type == 4)
            {
                return RedirectToAction("Index", "Podcast", new { Id = Id });
            }
            return View(newsobj);
        }

        public ActionResult LastestNews(string lang, int MaxLastestNews = 0)
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

           // ViewBag.CateName = CateName;
            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews*5, -1,lang);
            lstdata = lstdata.Where(x =>x.Type==1).Take(MaxLastestNews).ToList();
            return PartialView(lstdata);
        }

        public ActionResult RightLastestNews(string CateName, int CategoryId, int MaxLastestNews = 0, string lang = "")
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
            //var lstdata = new ContentBO().GetHotNews(CategoryId, MaxLastestNews);
            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, 0, lang);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,
                Url = "",
                CategoryId = CategoryId
            };
            return PartialView(model);
        }

        public ActionResult RightLastestNews2(string CateName, int CategoryId, int MaxLastestNews = 0)
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
            var lstdata = new ContentBO().GetHotNews(CategoryId, MaxLastestNews);
            //var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,
                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            return PartialView(model);
        }

        public ActionResult RightLastestNews3(string CateName, int CategoryId, int MaxLastestNews = 0)
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
            var lstdata = new ContentBO().GetHotNews(CategoryId, MaxLastestNews);
            //var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,
                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            return PartialView(model);
        }

        public ActionResult RightLastestNews4(string CateName, int CategoryId, int MaxLastestNews = 0)
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

        #region "Comment,Intro"
        public ActionResult Comment(int ItemId, string ItemName, int Type)
        {

            ViewBag.ItemId = ItemId;
            ViewBag.ItemName = ItemName;
            ViewBag.Type = Type;
            return PartialView();
        }

        #endregion
        private void ViewAdd(int Id, int CategoryId)
        {
            new ContentBO().ViewAdd(Id, CategoryId);
        }
    }
}
