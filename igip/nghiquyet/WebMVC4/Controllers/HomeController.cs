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
using System.Web.UI;
using UTILS;
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
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Menu(string lang)
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 100, false);
            ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", ngày" + DateTime.Now.ToString(" dd ") + "/" + DateTime.Now.ToString(" MM ") + "/" + DateTime.Now.Year.ToString();
            return PartialView(lstcategory.Where(x => x.Language == lang).ToList());
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult MenuMobile(string lang)
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 100, false);
            ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", ngày" + DateTime.Now.ToString(" dd ") + "/" + DateTime.Now.ToString(" MM ") + "/" + DateTime.Now.Year.ToString();
            return PartialView(lstcategory.Where(x => x.Language == lang).ToList());
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult MenuBottom(string lang)
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.Footer, 100, false);
            //var lstcategory = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.News);
            //lstcategory = lstcategory.Where(x => x.Id !=4 && x.Published == 1).Where(x => x.ParentId == 0 || x.ParentId == 4).ToList();
            //ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + "," + DateTime.Now.ToString(" dd") + "/" + DateTime.Now.ToString("MM")+"/" + DateTime.Now.Year.ToString();
            return PartialView(lstcategory.Where(x => x.Language == lang).ToList());
        }
        public ActionResult SiteMap()
        {
            ViewBag.Description = "Sơ đồ website";
            ViewBag.Keywords = "Sơ đồ website";
            ViewBag.Title = "Sơ đồ website";
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);

            return View(lstcategory);
        }
        [LocalizationActionFilter]
        public ActionResult Language(string lang)
        {
            WorkContext.SetLanguage(lang);

            return RedirectToAction("Index");
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerRight(int top = 0, string lang = "")
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(top, 2, 1);
            ViewBag.lang = lang;
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerBottom(string lang)
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 3, 1);
            ViewBag.lang = lang;
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult BannerRight2(string lang)
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 5, 1);
            ViewBag.lang = lang;
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult BannerRight3()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 4, 1);
            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult Banner(string lang)
        {
            var lstBanner = new List<Banner>();
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            if (!mobileHelper.DetectMobileLong())
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 1, 1);
            }
            else
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 1, 1);
            }

            ViewBag.lang = lang;
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult Banner2(string lang)
        {
            var lstBanner = new List<Banner>();

            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            if (!mobileHelper.DetectMobileLong())
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 4, 1);
            }
            else
            {
                lstBanner = new BannerBO().GetTopLastestBanners(0, 4, 1);
            }
            ViewBag.lang = lang;
            return PartialView(lstBanner);
        }

        public ActionResult SearchInput()
        {
            return PartialView();
        }

        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult HomeVideo(int CategoryId, string CateName, bool IsMobile)
        {
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);
            //var lstid = Utils.GetAppSettingValue("HotVideo");
            //var lstid = new SystemConfigBO().GetValueByKey("HotVideo");
            var lstdata = new ContentBO().GetHotNews(4, 5);
            //var lstdata = new ContentBO().GetTopLastestContentFulls(5, 6);
            var model = new LastestNewModel
            {
                lstdata = lstdata

            };
            ViewBag.IsMobile = IsMobile;
            return PartialView(model);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Podcast(int CategoryId, int MaxLastestNews = 0)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);
            }

            var lstdata = new ContentBO().GetHotNews(CategoryId, MaxLastestNews);
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
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Emagazine(int CategoryId, int MaxLastestNews = 0)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);
            }

            var lstdata = new ContentBO().GetHotNews(CategoryId, MaxLastestNews);
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
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult Slide(string lang)
        {
            //var Title = Utils.ReplaceVietnameseChar("Phú Thọ xây nhà máy phát điện từ rác thải");
            // var lstid = new SystemConfigBO().GetValueByKey("HotNewsForCate_"+Config.WebSite);
            //var lstTopViewId = new SystemConfigBO().GetValueByKey("TopViewNews_" + Config.WebSite);
            //var lstHotNews = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);
            var lstcontent = new ContentBO().GetTopLastestContentFulls(30, 0, lang).Where(x => x.Type == 1).ToList();
            //var lstTopViewNews = new ContentBO().GetTopContentByIdsFulls(lstTopViewId, 0, true);

            var lstHotNews = new List<HotNews>();
            var lstTopViewNews = new List<HotNews>();
            if (lang == "vi-vn")
            {
                lstHotNews = new HotNewsBO().GetTopHotNews(0, "hotnews", 1);
                lstTopViewNews = new HotNewsBO().GetTopHotNews(0, "topviewnews", 1);
            }
            else
            {
                lstHotNews = new HotNewsBO().GetTopHotNews(0, "hotnewsen", 1);
                lstTopViewNews = new HotNewsBO().GetTopHotNews(0, "topviewnewsen", 1);
            }


            var model = new SlideModel
            {
                LstHotNews = lstHotNews,
                LstLastestNews = lstcontent,
                LstTopViewNews = lstTopViewNews
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult TopAlbum(string CateName, int CategoryId, int Top)
        {
            //ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.Album);
            //ViewBag.CateName = CateName;
            var lstdata = new AlbumBO().GetTopLastestAlbumsFull(Top, CategoryId);

            try
            {
                var lstid = new SystemConfigBO().GetValueByKey("HotAlbum");
                if (string.IsNullOrEmpty(lstid))
                {
                    return PartialView(lstdata);
                }
                var lstcontent = new AlbumBO().GetTopAlbumByIdsFulls(lstid, 0, true).ToList();

                if (lstcontent == null)
                {
                    return PartialView(lstdata);
                }

                foreach (var item in lstdata)
                {

                    if (lstcontent.Where(x => x.Id == item.Id).ToList().Count == 0)
                    {
                        lstcontent.Add(item);

                    }
                }
                if (lstcontent != null)
                    lstcontent = lstcontent.Take(Top).ToList();
                return PartialView(lstcontent);
            }
            catch
            {

                return PartialView(lstdata);
            }
            //return PartialView(Albums);
        }

        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult TopAlbum2(string CateName, int CategoryId, int Top)
        {
            //ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.Album);
            //ViewBag.CateName = CateName;
            var lstdata = new AlbumBO().GetTopLastestAlbumsFull(Top, CategoryId);

            try
            {
                var lstid = new SystemConfigBO().GetValueByKey("HotAlbum");
                if (string.IsNullOrEmpty(lstid))
                {
                    return PartialView(lstdata);
                }
                var lstcontent = new AlbumBO().GetTopAlbumByIdsFulls(lstid, 0, true).ToList();

                if (lstcontent == null)
                {
                    return PartialView(lstdata);
                }

                foreach (var item in lstdata)
                {

                    if (lstcontent.Where(x => x.Id == item.Id).ToList().Count == 0)
                    {
                        lstcontent.Add(item);

                    }
                }
                if (lstcontent != null)
                    lstcontent = lstcontent.Take(Top).ToList();
                return PartialView(lstcontent);
            }
            catch
            {

                return PartialView(lstdata);
            }
            //return PartialView(Albums);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

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
            var lstdata = new ContentBO().GetHotNews(CategoryId, MaxLastestNews);
            //var lstdata=new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,
                //Css = cssClass,
                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

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
                //Css = cssClass,
                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNews4(string CateName, int CategoryId, int MaxLastestNews = 0)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

            }
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);

            ViewBag.CateName = CateName;

            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId).ToList();

            //var lstid = Utils.GetAppSettingValue("HotNewsForCate_" + CategoryId);
            var lstid = new SystemConfigBO().GetValueByKey("HotNewsForCate_" + CategoryId);
            if (string.IsNullOrEmpty(lstid))
            {
                return PartialView(lstdata);
            }
            var lstcontent = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true).ToList();

            if (lstcontent == null)
            {
                return PartialView(lstdata);
            }

            foreach (var item in lstdata)
            {

                if (lstcontent.Where(x => x.Id == item.Id).ToList().Count == 0)
                {
                    lstcontent.Add(item);

                }
            }
            if (lstcontent != null)
                lstcontent = lstcontent.Take(MaxLastestNews).ToList();
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 30, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNews(string CateName, int CategoryId, int MaxLastestNews = 0, string cssClass = "")
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
            if (lstdata == null)
                return PartialView(null);
            //var lstdata=new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,
                Css = cssClass,
                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 360, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNewsAPI(int CategoryId, int MaxLastestNews = 0, string cssClass = "", string lang = "")
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);
            }
            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);

            var lstdata = ServerProcess.GetHotNews(cateobj.Url, MaxLastestNews, lang);
            //var lstdata=new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewsApiModel
            {
                lstdata = lstdata,
                HeaderTitle = cateobj.Name,
                Css = cssClass,
                Url = cateobj.Url,
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 360, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNewsAPI2(int CategoryId, int MaxLastestNews = 0, string cssClass = "", string lang = "")
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);
            }
            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);

            var lstdata = ServerProcess.GetHotNews(cateobj.Url, MaxLastestNews, lang);
            //var lstdata=new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewsApiModel
            {
                lstdata = lstdata,
                HeaderTitle = cateobj.Name,
                Css = cssClass,
                Url = cateobj.Url,
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult TopDocument(int CategoryId=3, int MaxLastestNews = 7, string fromdate = "", string todate = "", string keyword = "", string code = "", int agent = 0, int area = 0, int type = 0)
        {
            // var MaxDocuments = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDocuments"]);
            //var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxLastestNews, CategoryId);
            var PageSize = MaxLastestNews;
            int Total = 0;

            var data = new List<DOCUMENT_FULL>();
            data = new DocumentBO().GetDocumentsSearchPaged2(keyword.Trim(), code.Trim(), agent, area, type, CategoryId, 1, 1, PageSize, fromdate, todate, ref Total);

            ViewBag.CateId = CategoryId;
            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
            ViewBag.CateName = cateobj.Name;
            return PartialView(data);
        }
        [OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]

        public ActionResult TopDocument2()
        {
            //var MaxDocuments = 4;
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(12);
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]

        public ActionResult TopDocument3()
        {
            //var MaxDocuments = 4;
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(3);
            return PartialView(lstcontent);
        }
        #endregion

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
            public FunctionType Type { get; set; }
        }

        [LocalizationActionFilter]
        public ActionResult Index()
        {
            //var book = new BookData
            //{
            //    Id=1,
            //    Name="Test",
            //    Type=FunctionType.IsView
            //};
            //RedisCaching.Add("test6", JsonConvert.SerializeObject(book));
            //var datacache = RedisCaching.GetData("test6");
            //var book2 = JsonConvert.DeserializeObject<BookData>(datacache.ToString());

            //var x = book2;
            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;
            //var _childCategory = new CategoryBO().GetAllChildCategories(4, 10, false);
           

            //var lstBanner = new List<Banner>();
            ViewBag.PageName = "home";


            //if (WorkContext.GetLanguage()=="vi-vn")
            //{
            //    lstBanner = new BannerBO().GetTopLastestBanners(0, 3, 1);
            //}
            //else
            //{
            //    lstBanner = new BannerBO().GetTopLastestBanners(0, 9, 1);
            //}
            //ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            //if (mobileHelper.DetectMobileLong())
            //    return View("MIndex");
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            return View();
        }
        public ActionResult ViewPDF(string url)
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            ViewBag.url = url;
            return View();
        }
        public ActionResult Search()
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"] + " | Truong mau giao | Truong mam non | Quan Ba Dinh";
            return View();
        }
        public ActionResult Error()
        {
            var requestpage = HttpUtility.UrlDecode(Request.ServerVariables["QUERY_STRING"].Replace("404;", ""));

            if (requestpage.EndsWith(".jpg") || requestpage.EndsWith(".jpeg"))
            {
                return Redirect("http://media.khcncongthuong.vn/" + requestpage.Replace("http://khcncongthuong.vn:80", ""));
            }

            return View();
        }
    }
}
