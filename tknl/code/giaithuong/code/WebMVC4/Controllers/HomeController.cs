using BIZ;
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
    public class HomeController : Controller
    {
        #region "Cache"

        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Intro(int CategoryId)
        {
            var intro = new CategoryBO().GetCategoryFull(CategoryId);

            return PartialView(intro);
        }
        [OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult Menu()
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);
            ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", ngày" + DateTime.Now.ToString(" dd ") + "/" + DateTime.Now.ToString(" MM ") + "/" + DateTime.Now.Year.ToString();
            return PartialView(lstcategory);
        }
        [OutputCache(Duration = 60, VaryByParam = "none")]
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
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerRight()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 2, 1);
            //ViewBag.cssClass = cssClass;
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerBottom()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 3, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult BannerRight2()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 5, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult BannerRight3()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 4, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult Banner()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 1, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult Banner2()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 4, 1);
            return PartialView(lstBanner);
        }

        public ActionResult SearchInput()
        {
            return PartialView();
        }

        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult HomeVideo(int CategoryId, string CateName)
        {
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);
            //var lstid = Utils.GetAppSettingValue("HotVideo");
            //var lstid = new SystemConfigBO().GetValueByKey("HotVideo");
            var lstdata = new ContentBO().GetHotNews(6,7);
            //var lstdata = new ContentBO().GetTopLastestContentFulls(5, 6);
            return PartialView(lstdata);
        }
        [OutputCache(Duration = 60, VaryByParam = "*")]

        public ActionResult HotNews()
        {

            //var lstid = new SystemConfigBO().GetValueByKey("HotNewsForCate_" + Config.WebSite);

            //var lstHotNews = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);
            var MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);
            var lstHotNews = new ContentBO().GetTopLastestContentFulls(100, 0).Where(x => x.CategoryId.GetValueOrDefault() == 36 || x.CategoryId.GetValueOrDefault() == 38||x.CategoryId.GetValueOrDefault() == 40).Take(MaxLastestNews).ToList();


            //var lstHotNews = new ContentBO().GetHotNews(0, MaxLastestNews);
            return PartialView(lstHotNews);
        }
        [OutputCache(Duration = 60, VaryByParam = "*")]

        public ActionResult Slide()
        {

            var lstid = new SystemConfigBO().GetValueByKey("HotNewsForCate_" + Config.WebSite);
            var lstTopViewId = new SystemConfigBO().GetValueByKey("TopViewNews_" + Config.WebSite);
            var lstHotNews = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);
            var lstcontent = new ContentBO().GetTopLastestContentFulls(14, 0, Config.WebSite).Where(x => x.Type == 1).ToList();
            var lstTopViewNews = new ContentBO().GetTopContentByIdsFulls(lstTopViewId, 0, true);
           


            var model = new SlideModel
            {
                LstHotNews = lstHotNews,
                LstLastestNews = lstcontent,
                LstTopViewNews = lstTopViewNews
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 60, VaryByParam = "none")]

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

        [OutputCache(Duration = 60, VaryByParam = "none")]

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
           
            var lstdata = ServerProcess.GetTopNews(1, MaxLastestNews, CategoryId);
            //var lstdata=new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewsApiModel
            {
                lstdata = lstdata,
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNewsAPI2(int CategoryId, int MaxLastestNews = 0, string cssClass = "")
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);
            }

            var lstdata = ServerProcess.GetTopNews(1, MaxLastestNews, CategoryId);
            //var lstdata=new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewsApiModel
            {
                lstdata = lstdata,
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]

        public ActionResult TopDocument()
        {
            var MaxDocuments = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDocuments"]);
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxDocuments,3);
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]

        public ActionResult TopDocument2()
        {
            var MaxDocuments = 4;
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxDocuments);
            return PartialView(lstcontent);
        }
        #endregion
        public ActionResult Index()
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            ViewBag.PageName = "Home";
            //var _childCategory = new CategoryBO().GetAllChildCategories(4, 10, false);
            //return RedirectToAction("Index2", "Admin");
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
