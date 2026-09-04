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
                var PageSize = 5;
                data = new ContentBO().GetTopLastestContentFulls(PageSize, CategoryId);

            }
            //ViewBag.HeaderTitle = HeaderTitle;
            //ViewBag.PageNextShow = PageNextShow;
            //ViewBag.pageNext = pageNext;
            //ViewBag.CategoryId = CategoryId;
            //ViewBag.CateName = CateName;
            return PartialView(data);
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
        public ActionResult Index(int CategoryId, string CateName, int Page = 1, int Type = 0)
        {

           
            if (CategoryId == 6)
                return RedirectToAction("Index", "Video");
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
            //var lstHotNews = new ContentBO().GetHotNews(CategoryId, 5);
            //ViewBag.hotnews = lstHotNews;
            var lstNotId = "";
            //foreach (var item in lstHotNews)
            //{
            //    lstNotId += item.Id + ",";
            //}
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
            var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxAriticleShow"]);
            int Total = 0;
            var articles = new ContentBO().GetPageContentFullsFrontend(Page, PageSize, CategoryId, ref Total, "", "", "", lstNotId);

            //var pageNext = Page + 1;
            //var PageNextShow = false;
            //if (Total <= PageSize)
            //{
            //    pageNext = 1;
            //    PageNextShow = false;
            //}
            //else
            //{
            //    PageNextShow = true;
            //}

            ViewBag.Total = Total;
            ViewBag.Page = Page;
            ViewBag.Type = Type;
            ViewBag.PageSize = PageSize;
            ViewBag.CategoryId = CategoryId;
            ViewBag.CateName = CateName;


            ViewBag.PageClass = "list";
            var model = new News2Model
            {
                hotnews = null,
                articles = articles
            };


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
                return null;
            var articles = new ContentBO().GetTopContentByIdsFulls(ids, 0, true).ToList();
            if (articles == null)
                return null;
            articles.Remove(articles.Where(x => x.Id == Id).FirstOrDefault());
            return articles;
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
            var articles = new ContentBO().GetPageContentFullsFrontend(Page, pageSize, categoryId, ref total, fromdate, todate, q, "","",-1,Config.WebSite);
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
        public ActionResult Detail(int Id, string Title)
        {
            var newsobj = new ContentBO().GetContentFull(Id);
            if (newsobj == null || newsobj.Status != 4)
                return RedirectToAction("Error", "Home");
            if (Title != Utils.ConvertToRewriteLink(newsobj.Title))
                return RedirectToAction("Detail", "News", new { Id = Id, Title = Utils.ConvertToRewriteLink(newsobj.Title) });
            if (newsobj.Type == 2)
            {
                return RedirectToAction("Index", "Video", new { VideoId = Id });
            }
            var metaDescription = newsobj.IntroText ;
            var siteTitle = newsobj.Title ;
            var metaKeyword = siteTitle.Replace(" | ", ",");
           
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle ;

            var lstRef = GetRefArticle(newsobj.Params, Id);
            ViewBag.lstRef = lstRef;
            ViewBag.SiteImage = newsobj.MainImage;
            ViewBag.CurrentCategoryId = newsobj.CategoryId;
            ViewBag.ParentCategoryId = newsobj.CateLiteObj.ParrentId;
            ViewBag.Url = newsobj.CateLiteObj.Url;
            var request = System.Web.HttpContext.Current.Request;
           
            ViewBag.MailShare = String.Format("https://mail.google.com/mail/u/0/?ui=2&view=cm&fs=1&tf=1&su={0}&body={1}", HttpUtility.UrlEncode(newsobj.Title), HttpUtility.UrlEncode(Request.Url.AbsoluteUri));

            Action<int> addview = ViewAdd;
            var asynSendNoti = addview.BeginInvoke(Id, null, null);
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
            return View(newsobj);
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
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);

            ViewBag.CateName = CateName;
            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            return PartialView(lstdata);
        }

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
            var lstdata = new ContentBO().GetHotNews(CategoryId, MaxLastestNews);
            //var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, 0,Config.WebSite);
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
        private void ViewAdd(int Id)
        {
            new ContentBO().ViewAdd(Id);
        }
    }
}
