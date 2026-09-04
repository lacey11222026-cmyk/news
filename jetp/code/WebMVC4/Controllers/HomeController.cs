using BIZ;
using DATA.ContentDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UTILS;
using WebMVC4.Filter;
using WebMVC4.Helper;
using WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class HomeController : Controller
    {
        #region "Cache"

        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Intro(int CategoryId)
        {
            var intro = new CategoryBO().GetCategoryFull(CategoryId);

            return PartialView(intro);
        }
        //[OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult MenuMobile(string lang)
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 28, false);
            ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", ngày" + DateTime.Now.ToString(" dd ") + "/" + DateTime.Now.ToString(" MM ") + "/" + DateTime.Now.Year.ToString();
            return PartialView(lstcategory.Where(x => x.Language == lang).ToList());
        }
        //[OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult Menu(string lang)
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 28, false);
            ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", ngày" + DateTime.Now.ToString(" dd ") + "/" + DateTime.Now.ToString(" MM ") + "/" + DateTime.Now.Year.ToString();
            return PartialView(lstcategory.Where(x => x.Language == lang).ToList());
        }
        //[OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult MenuBottom(string lang)
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.Footer, 28, false);
            //var lstcategory = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.News);
            //lstcategory = lstcategory.Where(x => x.Id !=4 && x.Published == 1).Where(x => x.ParentId == 0 || x.ParentId == 4).ToList();
            //ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + "," + DateTime.Now.ToString(" dd") + "/" + DateTime.Now.ToString("MM")+"/" + DateTime.Now.Year.ToString();
            return PartialView(lstcategory.Where(x => x.Language == lang).ToList());
        }
        [LocalizationActionFilter]
        public ActionResult SiteMap()
        {
  
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);
            ViewBag.Description = Resources.Global.SiteDescription;
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;
            var lang = WorkContext.GetLanguage();
            return View(lstcategory.Where(x => x.Language == lang).ToList());
        }
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerRight(int top = 0,string cssClass="",string lang = "vi-vn")
        {
            var lstBanner = new List<DATA.Banner>();
            if (lang == "vi-vn")
                lstBanner = new BannerBO().GetTopLastestBanners(top, 2, 1);
            else
                lstBanner = new BannerBO().GetTopLastestBanners(top, 2, 1);
            ViewBag.cssClass = cssClass;
            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerBottom(string lang = "vi-vn")
        {
            var lstBanner = new List<DATA.Banner>();
            lstBanner = new BannerBO().GetTopLastestBanners(0, 3, 1);
            //if (lang == "vi-vn")
            //    lstBanner = new BannerBO().GetTopLastestBanners(0, 3, 1);
            //else
            //    lstBanner = new BannerBO().GetTopLastestBanners(0, 13, 1);
            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult BannerRight2(string lang = "vi-vn")
        {
            var lstBanner = new List<DATA.Banner>();
            lstBanner = new BannerBO().GetTopLastestBanners(0, 5, 1);
            //if (lang == "vi-vn")
            //    lstBanner = new BannerBO().GetTopLastestBanners(0, 5, 1);
            //else
            //    lstBanner = new BannerBO().GetTopLastestBanners(0, 15, 1);
            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult BannerRight3()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 4, 1);
            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult Banner(string lang="vi-vn")
        {
            var lstBanner = new List<DATA.Banner>();
            if(lang == "vi-vn")
                lstBanner = new BannerBO().GetTopLastestBanners(0, 1, 1);
            else
                lstBanner = new BannerBO().GetTopLastestBanners(0, 6, 1);

            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult BannerNews(string lang = "vi-vn")
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 6, 1);
            return PartialView(lstBanner);
        }
        public ActionResult BannerVideo(string lang = "vi-vn")
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 7, 1);
            return PartialView(lstBanner);
        }
        public ActionResult SearchInput()
        {
            return PartialView();
        }

        //[OutputCache(Duration = 60, VaryByParam = "*")]

        public ActionResult HomeVideo(int CategoryId, string CateName)
        {
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);
            //var lstid = Utils.GetAppSettingValue("HotVideo");
            //var lstid = new SystemConfigBO().GetValueByKey("HotVideo");
            var lstdata = new ContentBO().GetHotNews(CategoryId, 4);
            //var lstdata = new ContentBO().GetTopLastestContentFulls(5, 6);
            return PartialView(lstdata);
        }
        //[OutputCache(Duration = 60, VaryByParam = "*")]

        public ActionResult HotNews(int CategoryId)
        {

            //var lstid = new SystemConfigBO().GetValueByKey("HotNewsForCate_" + Config.WebSite);

            //var lstHotNews = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);
            var MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);
            //var lstHotNews = new ContentBO().GetTopLastestContentFulls(100, 0).Where(x => x.CategoryId.GetValueOrDefault() == 36 || x.CategoryId.GetValueOrDefault() == 38||x.CategoryId.GetValueOrDefault() == 40).Take(MaxLastestNews).ToList();


            var lstHotNews = new ContentBO().GetHotNews(CategoryId, MaxLastestNews);
            return PartialView(lstHotNews);
        }
        public ActionResult Project()
        {

            //var lstid = new SystemConfigBO().GetValueByKey("HotNewsForCate_" + Config.WebSite);

            return PartialView();
        }
        //[OutputCache(Duration = 60, VaryByParam = "*")]

        public ActionResult Slide()
        {

            var lstid = new SystemConfigBO().GetValueByKey("HotNewsForCate_" + Config.WebSite);
            var lstTopViewId = new SystemConfigBO().GetValueByKey("TopViewNews_" + Config.WebSite);
            var lstHotNews = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);
            var lstcontent = new ContentBO().GetTopLastestContentFulls(14, 0, "").Where(x => x.Type == 1).ToList();
            var lstTopViewNews = new ContentBO().GetTopContentByIdsFulls(lstTopViewId, 0, true);
           


            var model = new SlideModel
            {
                LstHotNews = lstHotNews,
                LstLastestNews = lstcontent,
                LstTopViewNews = lstTopViewNews
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
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNewsAuthor(string CateName, int CategoryId, int MaxLastestNews = 0)
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
           foreach(var item in lstdata)
            {
                try
                {
                    MembershipUser user = Membership.GetUser(item.Alias);
                    var author = JsonConvert.DeserializeObject<AuthorProfile>(user.Comment);
                    item.Avatar = author.Avatar;
                    item.FullName = author.FullName;
                }
                catch
                {

                }
                
            }
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
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNews2(string CateName, int CategoryId, int MaxLastestNews = 0,string lang="")
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);
            }
            if (string.IsNullOrEmpty(CateName))
            {
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                if (lang == "" || lang == "vi-vn")
                    CateName = cateobj.Name;
                else
                    CateName = cateobj.Description;
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
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

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
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNews4(int MaxLastestNews ,string lang)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

            }
            
            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, 0,"", lang).ToList();

            
            return PartialView(lstdata);
        }
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNews5(int MaxLastestNews ,string lang)
        {

            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

            }

            var lstTopViewId = new SystemConfigBO().GetValueByKey("TopViewNews_" + Config.WebSite);
            var lstData = new ContentBO().GetTopViewContentFulls(MaxLastestNews,0,lang);
            if (string.IsNullOrEmpty(lstTopViewId))
            {
                return PartialView(lstData);
            }
            var lstTopViewNews = new ContentBO().GetTopContentByIdsFulls(lstTopViewId, 0, true);
            if (lstTopViewNews == null)
            {
                return PartialView(lstData);
            }

            foreach (var item in lstData)
            {

                if (lstTopViewNews.Where(x => x.Id == item.Id).ToList().Count == 0)
                {
                    lstTopViewNews.Add(item);

                }
            }
            if (lstTopViewNews != null)
                lstTopViewNews = lstTopViewNews.Take(MaxLastestNews).ToList();
            return PartialView(lstTopViewNews);
            
        }
        [OutputCache(Duration = 30, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNews(string CateName, int CategoryId, int MaxLastestNews = 0, string cssClass = "",string lang="")
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
        //[OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]

        public ActionResult TopDocument(int CategoryId)
        {
            var MaxDocuments = 4;
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxDocuments, CategoryId);
            return PartialView(lstcontent);
        }
        public ActionResult TopMission( string lang)
        {
            var MaxDocuments = 3;
            var CategoryId = 2;
            if(lang == "vi-vn")
                CategoryId = 1;
            var lstcontent = new MissionBO().GetTopLastestMissionsFull(MaxDocuments, CategoryId);
            return PartialView(lstcontent);
        }
        public ActionResult TopRightMission(string lang,int top)
        {
            var MaxDocuments = 3;
            var CategoryId = 2;
            if (lang == "vi-vn")
                CategoryId = 1;
            var lstcontent = new MissionBO().GetTopLastestMissionsFull(MaxDocuments, CategoryId);
            return PartialView(lstcontent);
        }
        //[OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]

        public ActionResult TopProject(string lang, int top)
        {
            
            var lstcontent = Project2DAL.TopProject(top,1,"", lang);
            return PartialView(lstcontent);
        }
        #endregion
        [LocalizationActionFilter]
        public ActionResult Language(string lang)
        {
            WorkContext.SetLanguage(lang);

            return RedirectToAction("Index");
        }
        [LocalizationActionFilter]
        public ActionResult Index()
        {
            ViewBag.Description = Resources.Global.SiteDescription;
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;
            ViewBag.PageName = "Home";
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
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
            //EmailService.SendMail("Xác minh tài khoản của bạn", "chào bạn", "cuongpmk49ca@gmail.com");
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
