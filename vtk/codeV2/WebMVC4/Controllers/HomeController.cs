using BIZ;
using BIZ.Entity;
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
    public class HomeController : Controller
    {
        #region "Cache"

       // [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Intro(int CategoryId)
        {
            var model = new CategoryBO().GetCategoryFull(CategoryId);
            model.Param = JsonConvert.DeserializeObject<CategoryParam>(model.Params);

            return PartialView(model);
        }
        //[OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult Menu()
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);
            ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", ngày" + DateTime.Now.ToString(" dd ") + "/" + DateTime.Now.ToString(" MM ") + "/" + DateTime.Now.Year.ToString();
            return PartialView(lstcategory);
        }
        //[OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult MenuMobile()
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);
            ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", ngày" + DateTime.Now.ToString(" dd ") + "/" + DateTime.Now.ToString(" MM ") + "/" + DateTime.Now.Year.ToString();
            return PartialView(lstcategory);
        }
        //[OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult MenuBottom()
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.Footer, 18, false);
            //var lstcategory = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.News);
            //lstcategory = lstcategory.Where(x => x.Id !=4 && x.Published == 1).Where(x => x.ParentId == 0 || x.ParentId == 4).ToList();
            //ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + "," + DateTime.Now.ToString(" dd") + "/" + DateTime.Now.ToString("MM")+"/" + DateTime.Now.Year.ToString();
            return PartialView(lstcategory);
        }
        public ActionResult SiteMap()
        {
            ViewBag.Description = "Sơ đồ website";
            ViewBag.Keywords = "Sơ đồ website";
            ViewBag.Title = "Sơ đồ website";
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);

            return View(lstcategory);
        }
        [OutputCache(Duration = 20, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerRight(int top = 0)
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(top, 2, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 20, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerBottom()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 4, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 20, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult BannerRight2()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 5, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 20, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult BannerRight3()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 4, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 20, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult Banner()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 1, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 20, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult Banner2()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 3, 1);
            return PartialView(lstBanner);
        }

        public ActionResult SearchInput()
        {
            return PartialView();
        }

        //[OutputCache(Duration = 20, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult HomeVideo(int CategoryId, string CateName)
        {
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);
            //var lstid = Utils.GetAppSettingValue("HotVideo");
            //var lstid = new SystemConfigBO().GetValueByKey("HotVideo");
            var lstdata = new ContentBO().GetHotNews(6, 5);
            //var lstdata = new ContentBO().GetTopLastestContentFulls(5, 6);
            return PartialView(lstdata);
        }
        //[OutputCache(Duration = 20, VaryByParam = "none")]

        public ActionResult Slide()
        {
            //var Title = Utils.ReplaceVietnameseChar("Phú Thọ xây nhà máy phát điện từ rác thải");
           // var lstid = new SystemConfigBO().GetValueByKey("HotNewsForCate_"+Config.WebSite);
            //var lstTopViewId = new SystemConfigBO().GetValueByKey("TopViewNews_" + Config.WebSite);
            //var lstHotNews = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);
            //var lstcontent = new ContentBO().GetTopLastestContentFulls(14, 0, Config.WebSite).Where(x => x.Type == 1).ToList();
            //var lstTopViewNews = new ContentBO().GetTopContentByIdsFulls(lstTopViewId, 0, true);
            var lstHotNews = new HotNewsBO().GetTopHotNews(0, "hotnews", 1);
           //var lstTopViewNews=new HotNewsBO().GetTopHotNews(0, "topviewnews", 1);


            var model = new SlideModel
            {
                LstHotNews = lstHotNews,
                LstLastestNews = null,
                LstTopViewNews = null
            };
            return PartialView(model);
        }
        //[OutputCache(Duration = 60, VaryByParam = "none")]

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

        //[OutputCache(Duration = 60, VaryByParam = "none")]

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
        [OutputCache(Duration = 20, VaryByParam = "*", VaryByCustom = "browser")]

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
        [OutputCache(Duration = 20, VaryByParam = "*", VaryByCustom = "browser")]

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
        [OutputCache(Duration = 20, VaryByParam = "*", VaryByCustom = "browser")]

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
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNewsAPI(int CategoryId, int MaxLastestNews = 0, string cssClass = "")
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);
            }
            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
            
            var lstdata = ServerProcess.GetHotNews(cateobj.Url, MaxLastestNews);
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
        [OutputCache(Duration = 20, VaryByParam = "none", VaryByCustom = "browser")]

        public ActionResult TopDocument()
        {
            var MaxDocuments = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDocuments"]);
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxDocuments,3);
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 20, VaryByParam = "none", VaryByCustom = "browser")]

        public ActionResult TopDocument2()
        {
            var MaxDocuments = 4;
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxDocuments);
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

       // [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Service()
        {
           

            return PartialView();
        }
        [LocalizationActionFilter]
        public ActionResult Language(string lang)
        {
            WorkContext.SetLanguage(lang);

            return RedirectToAction("Index");
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

            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            //var _childCategory = new CategoryBO().GetAllChildCategories(4, 10, false);

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

            if (requestpage.EndsWith(".jpg")|| requestpage.EndsWith(".jpeg"))
            {
                return Redirect("http://media.khcncongthuong.vn/" + requestpage.Replace("http://khcncongthuong.vn:80",""));
            }

            return View();
        }
    }
}
