using BIZ;
using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UTILS;
//using WebMarkupMin.Mvc.ActionFilters;
using WebMVC4.Filter;
using WebMVC4.Helper;
using WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class HomeController : Controller
    {
        #region "Cache"

        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Intro(int CategoryId)
        {
            var intro = new CategoryBO().GetCategoryFull(CategoryId);

            return PartialView(intro);
        }
        public ActionResult MenuMobile(string lang)
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.TopMenu, 100, false);
            //ViewBag.Date = "Hôm nay " + Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", ngày" + DateTime.Now.ToString(" dd ") + "tháng" + DateTime.Now.ToString(" MM ") + "năm " + DateTime.Now.Year.ToString();
            ViewBag.lang = lang;
            return PartialView(lstcategory.Where(x => x.Language == lang).ToList());
        }
        public ActionResult Marquee(string lang)
        {
            var hotnews = new List<HotNews>();
            if (lang == "vi-vn")
                hotnews = new HotNewsBO().GetTopHotNews(5, "hotnews", 1);
            else
                hotnews = new HotNewsBO().GetTopHotNews(5, "hotnewseng", 1);
            return PartialView(hotnews);
        }
        public ActionResult Menu(string lang)
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 100, false);
            //ViewBag.Date = "Hôm nay " + Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", ngày" + DateTime.Now.ToString(" dd ") + "tháng" + DateTime.Now.ToString(" MM ") + "năm " + DateTime.Now.Year.ToString();
            ViewBag.lang = lang;
            return PartialView(lstcategory.Where(x => x.Language == lang).ToList());
        }
        [OutputCache(Duration = 60, VaryByParam = "*")]
        public ActionResult MenuBottom(string lang)
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.Footer, 100, false);
            //var lstcategory = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.News);
            //lstcategory = lstcategory.Where(x => x.Id !=4 && x.Published == 1).Where(x => x.ParentId == 0 || x.ParentId == 4).ToList();
            //ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + "," + DateTime.Now.ToString(" dd") + "/" + DateTime.Now.ToString("MM")+"/" + DateTime.Now.Year.ToString();
            return PartialView(lstcategory.Where(x => x.Language == lang).ToList());
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult Support()
        {
            var MaxMainSupport = Convert.ToInt32(ConfigurationManager.AppSettings["MaxMainSupport"]);
            var lstdata = new SupportBO().GetTopSupports(MaxMainSupport, false);
            RedisCaching.Flush();
            return PartialView(lstdata);
        }
       
        //[OutputCache(Duration = 20, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult BannerRight1(string param)
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 2, 1);
            ViewBag.param = param;
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult BannerRight2()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 3, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult BannerRight3()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 4, 1);
            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 20, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult Banner()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 1, 1);
            return PartialView(lstBanner);
            //return PartialView(lstBanner);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult RightNews(int CategoryId, int MaxLastestNews ,string lang)
        {
            try
            {
                if (MaxLastestNews == 0)
                {
                    MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

                }
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, -1,0, lang, 1);
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

        public ActionResult SearchInput()
        {
            return PartialView();
        }


        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Slide(string lang)
        {
            var cate = 0;
            if(lang=="en-us")
                cate = 102;
            var lstcontent = new ContentBO().GetHotNews(cate, 4);
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Slide2(string lang)
        {
            var cate = 0;
            if (lang == "en-us")
                cate = 102;
            var lstcontent = new ContentBO().GetHotNews(cate, 4);
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult TopAlbum(string CateName, int CategoryId, int Top)
        {
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.Album);
            ViewBag.CateName = CateName;
            var Albums = new AlbumBO().GetTopLastestAlbumsFull(Top, CategoryId);
            return PartialView(Albums);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult LastestNews(string lang)
        {
            //var lstcate =  new List<int>() { 5, 44,  7,49 };
            //if (lang == "en-us")
            //    lstcate = new List<int>() { 29, 30, 45, 31,46,50 };
            //int total = 0;
            //var lstcontent = new ContentBO().GetFilterContentFullsPaged(1, 7,"",-1, lstcate,4,"",ref total);
            var lstcontent = new ContentBO().GetTopLastestContentFulls(7, -1, 0, lang);
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult LastestNews2(string CateName, int CategoryId, int MaxLastestNews,string CssClass)
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
            //var lstdata=new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,
                Css= CssClass,
                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult LastestNews3(string CateName, int CategoryId, int MaxLastestNews = 0)
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
            //var lstdata=new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,

                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult LastestNews5(string CateName, int CategoryId, int MaxLastestNews = 0)
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
            //var lstdata=new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,

                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult LastestNews4(string CateName, int CategoryId, int MaxLastestNews = 0)
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
            var lstdata = new NoteBO().GetTopNote( MaxLastestNews, CategoryId);
            //var lstdata=new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNoteModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,

                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.Note),
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult LastestNews6(string CateName, int CategoryId, int MaxLastestNews = 0)
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
            //var lstdata=new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,

                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult HomeVideo(int CategoryId, string CateName, int MaxLastestNews = 0)
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
            //var lstdata=new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,

                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
           
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            return PartialView(model);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Podcast(int MaxLastestNews = 0)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);
            }
           
            var lstdata = new ContentBO().GetHotNews(59, MaxLastestNews);
            //var lstdata=new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
           
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            var model = new LastestNewModel
            {
                lstdata = lstdata
               
            };
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            return PartialView(model);
        }
       
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Emagazine(int MaxLastestNews = 0)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);
            }

            var lstdata = new ContentBO().GetHotNews(66, MaxLastestNews);
            //var lstdata=new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);

            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);

             var model = new LastestNewModel
            {
                lstdata = lstdata
               
            };
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            return PartialView(model);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult TopDocument(int cateId, int num, string cateName)
        {
            var MaxDocuments = num;
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxDocuments, cateId);
            ViewBag.cateId = cateId;
            ViewBag.cateName = cateName;
            return PartialView(lstcontent);
        }
        #endregion
        public ActionResult Text(int year)
        {

            var lstNews = new ExpertBO().GetTopExpert(0, -1);
            var patch = "/App_File/images/";
            foreach (var item in lstNews)
            {


                var strBuilder = new StringBuilder();
                strBuilder.Append(Request.PhysicalApplicationPath).Append(patch.Replace("/", "\\"));
                try
                {
                    DownloadImage(strBuilder.ToString(), "http://www.sxsh.vn/" + item.Image, item.Image.Replace(patch, ""));
                }
                catch
                {
                }








            }

            return View();
        }
        private void DownloadImage(string fromPath, string uri, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var webClient = new WebClient();
                if (!Directory.Exists(fromPath))
                    Directory.CreateDirectory(fromPath);

                var path = string.Format("{0}\\{1}", fromPath, name);
                if (!System.IO.File.Exists(path))
                {
                    webClient.DownloadFile(uri, fromPath + name);
                }
            }


        }
        [LocalizationActionFilter]
        public ActionResult Language(string lang)
        {
            WorkContext.SetLanguage(lang);

            return RedirectToAction("Index");
        }
        public enum FunctionType
        {
            IsView = 0,
            IsInsert = 1,
            IsUpdate = 2,
            IsDelete = 3,
            IsFullControl = 4,
        }
        public class BookData
        {
           


            public string Name;
            public int Id;
            public ArrayList Cate { get; set; }
        }
        [LocalizationActionFilter]
        //[RemoveWhitespacesAttribute]
        public ActionResult Index()
        {
           
            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;
            var cate = 0;
            if (WorkContext.GetLanguage() == "en-us")
                cate = 102;
            var content = new ContentBO().GetHotNews(cate, 1).FirstOrDefault();
            if (WorkContext.GetLanguage() == "en-us")
            {
                return View("IndexEn", content);
            }
            return View(content);
        }
        [LocalizationActionFilter]
        public ActionResult SiteMap()
        {
            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);

            return View(lstcategory.Where(x => x.Language == WorkContext.GetLanguage()).ToList());
        }
        public ActionResult Search()
        {
            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle + " | Truong mau giao | Truong mam non | Quan Ba Dinh";
            return View();
        }
        public ActionResult Error()
        {
           
            return View();
        }
    }
}
