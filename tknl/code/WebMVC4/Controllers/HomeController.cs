using System.IO;
using System.Net;
using BIZ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DATA;
using UTILS;

using BIZ.Entity;
using WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class HomeController : BaseController
    {
        #region "Cache"
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Intro(int CategoryId)
        {
            var intro = new CategoryBO().GetCategoryFull(CategoryId);

            return PartialView(intro);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult Menu()
        {
            //var culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLowerInvariant();
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.Homepage, 18, false, "vi-vn");
            //var lstMenu = MvcApplication.StaticCategoryAllList.Where(x => x.Params.Contains("\"IsHomepage\":1")).ToList();
            ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", " + DateTime.Now.ToString("dd/MM/yyy ") + "| " + DateTime.Now.ToString("HH:mm") + " GMT+7";
            return PartialView(lstcategory);
        }

        //lấy tin xem nhiều
        private List<CONTENT_FULL> GetTopViewNews()
        {
            var todate = DateTime.Now.ToString("dd/MM/yyyy");
            var fromdate = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
            var lstdata = new ContentBO().GetTopViewContentFulls(14, 0, fromdate, todate, "vi-vn");
            //var configValue = new SystemConfigBO().GetByKey("TopViewNews");
            //if (configValue != null)
            //{
            //    var lstid = configValue.ConfigValue;
            //    if (string.IsNullOrEmpty(lstid))
            //    {
            //        return lstdata;
            //    }
            //    var lstcontent = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true).ToList();

            //    if (lstcontent == null || lstcontent.Count < 1)
            //    {
            //        return lstdata;
            //    }
            //    foreach (var item in lstdata)
            //    {

            //        if (!lstcontent.Where(x => x.Id == item.Id).Any())
            //        {
            //            lstcontent.Add(item);
            //        }
            //    }
            //    return lstcontent;
            //}
            return lstdata;
        }




        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult Slide()
        {
            var lstHotNews = new List<CONTENT_FULL>();
            var configValue = new SystemConfigBO().GetByKey("HotNews");
            if (configValue != null)
            {

                lstHotNews = new ContentBO().GetTopContentByIdsFulls(configValue.ConfigValue, 0, true);

            }
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            var lstcontent = new ContentBO().GetTopLastestContentFulls(14, 0, "vi-vn");
            var lstTopViewNews = GetTopViewNews();

            var model = new SlideModel
            {
                LstHotNews = lstHotNews,
                LstLastestNews = lstcontent,
                LstTopViewNews = lstTopViewNews
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult TopENews(int Top = 7)
        {
            var lstdata = new ContentBO().GetHotNews(55, Top);
            return PartialView(lstdata);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult TopVideo(bool IsMobile, int Top = 7)
        {
            //ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.Album);
            //ViewBag.CateName = CateName;
            //var video = new ContentBO().GetTopLastestContentFulls(Top, -1, 2);
            //var request = System.Web.HttpContext.Current.Request;
            //var mobileHelper = new MobileDetectHelper(request);
            //ViewBag.IsIpad = mobileHelper.DetectIpad();
            //ViewBag.Iphone = mobileHelper.DetectIphone();

            if (Top == 0)
            {
                Top = 7;

            }

            ViewBag.IsMobile = IsMobile;

            try
            {
                var lstdata = new ContentBO().GetHotNews(14, Top);
                return PartialView(lstdata);

            }
            catch (Exception)
            {

                return PartialView(null);
            }
            //return PartialView(null);


        }
        [ChildActionOnly]
        public ActionResult TopPodcast(int Top = 3)
        {
            if (Top == 0)
            {
                Top = 3;
            }
            try
            {
                List<CONTENT_FULL> hotNews = new ContentBO().GetHotNews(56, Top);
                return base.PartialView(hotNews);
            }
            catch (Exception)
            {
                return base.PartialView((string)null);
            }
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult RightNews(int CategoryId, int MaxLastestNews = 0)
        {
            try
            {
                if (MaxLastestNews == 0)
                {
                    MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

                }
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, -1, "vi-vn", 1);
                var model = new LastestNewModel
                {
                    lstdata = lstdata,
                    HeaderTitle = cateobj.Name.ToUpper(),
                    Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, cateobj.Name, (int)UTILS.Constants.CategoryType.News),
                    CategoryId = CategoryId
                };
                return PartialView(model);
            }
            catch (Exception)
            {

                return PartialView(null);
            }

        }
        [ChildActionOnly]

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult RightNews2(int CategoryId, int MaxLastestNews = 0)
        {
            try
            {
                if (MaxLastestNews == 0)
                {
                    MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

                }
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                var lstdata = new ContentBO().GetFocusNews(-1, MaxLastestNews, "vi-vn");
                var model = new LastestNewModel
                {
                    lstdata = lstdata,
                    HeaderTitle = cateobj.Name.ToUpper(),
                    Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, cateobj.Name, (int)UTILS.Constants.CategoryType.News),
                    CategoryId = CategoryId
                };
                return PartialView(model);
            }
            catch (Exception)
            {

                return PartialView(null);
            }

        }

        [ChildActionOnly]

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult LastestNews(int CategoryId, String CateName = "", int MaxLastestNews = 0)
        {
            try
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
                var model = new LastestNewModel
                {
                    lstdata = lstdata,
                    HeaderTitle = CateName.ToUpper(),
                    Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                    CategoryId = CategoryId
                };
                return PartialView(model);
            }
            catch (Exception)
            {

                return PartialView(null);
            }

        }

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult LastestNews2(int CategoryId, String CateName = "", int MaxLastestNews = 0)
        {
            try
            {
                if (MaxLastestNews == 0)
                {
                    MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

                }

                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                CateName = cateobj.Name;

                var lstdata = new ContentBO().GetHotNews(CategoryId, MaxLastestNews);
                var model = new LastestNewModel
                {
                    lstdata = lstdata,
                    HeaderTitle = CateName.ToUpper(),
                    Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                    CategoryId = CategoryId
                };
                return PartialView(model);
            }
            catch (Exception)
            {

                return PartialView(null);
            }

        }

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult LastestNews3(int CategoryId, String CateName = "", int MaxLastestNews = 0)
        {
            try
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
                var model = new LastestNewModel
                {
                    lstdata = lstdata,
                    HeaderTitle = CateName.ToUpper(),
                    Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                    CategoryId = CategoryId
                };
                return PartialView(model);
            }
            catch (Exception)
            {

                return PartialView(null);
            }

        }

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult LastestNews4(int CategoryId, String CateName = "", int MaxLastestNews = 0)
        {
            try
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
                var model = new LastestNewModel
                {
                    lstdata = lstdata,
                    HeaderTitle = CateName.ToUpper(),
                    Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                    CategoryId = CategoryId
                };
                return PartialView(model);
            }
            catch (Exception)
            {

                return PartialView(null);
            }

        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult LastestNews5(int CategoryId, String CateName = "", int MaxLastestNews = 0)
        {
            try
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
                var model = new LastestNewModel
                {
                    lstdata = lstdata,
                    HeaderTitle = CateName.ToUpper(),
                    Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                    CategoryId = CategoryId
                };
                return PartialView(model);
            }
            catch (Exception)
            {

                return PartialView(null);
            }

        }
        #endregion

        public ActionResult Index()
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);

            ViewBag.IsMobile = mobileHelper.DetectMobileLong();

            ViewBag.MainImage = "/images/logo1.jpg";
            ViewBag.NewsDescription = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            ViewBag.NewsTitle = ConfigurationManager.AppSettings["DefMetaDescription"];
            return View();

        }

        public ActionResult Search()
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            return View();
        }

        public ActionResult Error()
        {
            var requestpage = HttpUtility.UrlDecode(Request.ServerVariables["QUERY_STRING"].Replace("404;", ""));
            ViewBag.requestpage = requestpage;


            if (requestpage.EndsWith(".htm"))
            {
                var newsid = requestpage.Substring(requestpage.LastIndexOf("-") + 1).Replace(".htm", "");
                var newsobj = new ContentBO().GetContentFull(int.Parse(newsid));
                if (newsobj == null || newsobj.Status != 1)
                    return RedirectToAction("Error", "Home");

                var categoryobj = new CategoryBO().GetCategoryFull(newsobj.CategoryId.Value);
                if (categoryobj != null)
                    return RedirectToAction("Detail", "News", new { Id = int.Parse(newsid), Title = Utils.ConvertToRewriteLink(newsobj.Title), CateName = Utils.ConvertToRewriteLink(categoryobj.Name) });
            }
            //var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);
            return View();
        }


    }
}
