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


       
        public ActionResult Menu()
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);
            ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", ngày" + DateTime.Now.ToString(" dd ") + "tháng" + DateTime.Now.ToString(" MM ") + "năm " + DateTime.Now.Year.ToString();
            return PartialView(lstcategory);
        }

        [OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
       
        public ActionResult Support()
        {
            var MaxMainSupport = Convert.ToInt32(ConfigurationManager.AppSettings["MaxMainSupport"]);
            var lstdata = new SupportBO().GetTopSupports(MaxMainSupport, false);
            return PartialView(lstdata);
        }
        [OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
       
        public ActionResult Marquee()
        {

            var lstcontent = new ContentBO().GetTopLastestContentFulls(5, 4);
            return PartialView(lstcontent);
        }

        [OutputCache(Duration = 300, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult BannerRight()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 2, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 30, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult BannerRight2()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 5, 1);
            return PartialView(lstBanner);
        }

        [OutputCache(Duration = 300, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult BannerBottom()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 3, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 300, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult BannerBottomLeft()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 6, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 300, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult Banner()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 1, 1);
            return PartialView(lstBanner);
        }

       
        public ActionResult SearchInput()
        {
            return PartialView();
        }


        [OutputCache(Duration = 300, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult HomeVideo()
        {
            
            var lstid = new SystemConfigBO().GetValueByKey("HotVideo");
            var lstdata = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);
            return PartialView(lstdata.FirstOrDefault());
        }

        [OutputCache(Duration = 300, VaryByParam = "*", VaryByCustom = "browser")]
       
        public ActionResult Slide()
        {

            var lstid = new SystemConfigBO().GetValueByKey("HotNews");
            var lstcontent3 = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);
            return PartialView(lstcontent3);
        }

        [OutputCache(Duration = 300, VaryByParam = "none", VaryByCustom = "browser")]
       
        public ActionResult TopAlbum(string CateName, int CategoryId, int Top)
        {
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.Album);
            ViewBag.CateName = CateName;
            var Albums = new AlbumBO().GetTopLastestAlbumsFull(Top, CategoryId);
            return PartialView(Albums);
        }

        [OutputCache(Duration = 300, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult LastestNews(string CateName, int CategoryId,string Icon="", int MaxLastestNews = 0)
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
            //ViewBag.Url =UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);

            //ViewBag.CateName = CateName;
            var lstdata = new ContentBO().GetHotNews(CategoryId, MaxLastestNews);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,
                Css= Icon,
                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 300, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult RightLastestNews(string CateName, int CategoryId, string Icon = "", int MaxLastestNews = 0)
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
            //ViewBag.Url =UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);

            //ViewBag.CateName = CateName;
            var lstdata = new ContentBO().GetHotNews(CategoryId, MaxLastestNews);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,
                Css = Icon,
                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 300, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult TopDocument()
        {
            var MaxDocuments = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDocuments"]);
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxDocuments);
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
       
        public ActionResult TopGroup()
        {

            return PartialView();
        }
        #endregion

        public ActionResult Index()
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            ViewBag.PageClass = "home";
            var _childCategory = new CategoryBO().GetAllChildCategories(4, 10, false);

            //var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxDocShow"]);
            //int Total = 0;

            //var listdata = new DocumentBO().GetDocumentsSearchPaged("", 0, 1, 1, PageSize, "2014-01-01 00:00:00.000", "2014-01-17 00:00:00.000", ref Total);
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
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            return View();
        }
        public ActionResult Error()
        {
            var requestpage = HttpUtility.UrlDecode(Request.ServerVariables["QUERY_STRING"].Replace("404;", ""));
            ViewBag.requestpage = requestpage;
            if (requestpage.Contains("Upload"))
            {
                requestpage = requestpage.Substring(requestpage.IndexOf("Images"));
                var arequestpage = requestpage.Split("/".ToCharArray());
                string url;
                try
                {
                    var atargetfile = arequestpage[arequestpage.Length - 1].Split(".".ToCharArray());
                    int sourceId;
                    if (!int.TryParse(arequestpage[arequestpage.Length - 2], out sourceId))
                    {
                    }
                    var w = atargetfile[atargetfile.Length - 3];
                    var h = atargetfile[atargetfile.Length - 2];
                    var f = "";
                    for (var i = 0; i < arequestpage.Length - 1; i++)
                    {
                        f += arequestpage[i] + "/";
                    }
                    for (var i = 0; i < atargetfile.Length - 3; i++)
                    {
                        f += atargetfile[i];
                        if (i < atargetfile.Length - 4) f += ".";
                    }
                    f = f.Replace("/", "\\");
                    try
                    {
                        Convert.ToInt32(w);
                        Convert.ToInt32(h);
                        url = "/srv_thumb.ashx?source=" + sourceId + "&w=" + w + "&h=" + h + "&f=" + HttpUtility.UrlEncode(f);
                    }
                    catch (Exception)
                    {
                        url = "/images/upload/no_image.jpg";
                    }

                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    url = "/images/upload/no_image.jpg";
                }
                return Redirect(url);
            }
            else
            {
                if (requestpage.Contains("xdmedia"))
                {
                    requestpage = requestpage.Substring(requestpage.IndexOf("xdmedia"));
                    var arequestpage = requestpage.Split("/".ToCharArray());
                    string url;
                    try
                    {
                        var atargetfile = arequestpage[arequestpage.Length - 1].Split(".".ToCharArray());
                        int sourceId;
                        if (!int.TryParse(arequestpage[arequestpage.Length - 2], out sourceId))
                        {
                        }
                        var w = atargetfile[atargetfile.Length - 3];
                        var h = atargetfile[atargetfile.Length - 2];
                        var f = "";
                        for (var i = 0; i < arequestpage.Length - 1; i++)
                        {
                            f += arequestpage[i] + "/";
                        }
                        for (var i = 0; i < atargetfile.Length - 3; i++)
                        {
                            f += atargetfile[i];
                            if (i < atargetfile.Length - 4) f += ".";
                        }
                        f = f.Replace("/", "\\");
                        try
                        {
                            Convert.ToInt32(w);
                            Convert.ToInt32(h);
                            url = "/srv_thumb.ashx?source=" + sourceId + "&w=" + w + "&h=" + h + "&f=" + HttpUtility.UrlEncode(f);
                        }
                        catch (Exception)
                        {
                            url = "/images/upload/no_image.jpg";
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                        url = "/images/upload/no_image.jpg";
                    }
                    return Redirect(url);
                }

            }
            return View();
        }
        public ActionResult SiteMap()
        {
            ViewBag.Description = "Sơ đồ website";
            ViewBag.Keywords = "Sơ đồ website";
            ViewBag.Title = "Sơ đồ website";
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);

            return View(lstcategory);
        }
        public ActionResult Send()
        {
            ViewBag.Description = "Gửi tin bài, báo cáo";
            ViewBag.Keywords = "Gửi tin bài, báo cáo";
            ViewBag.Title = "Gửi tin bài, báo cáo";


            return View();
        }
    }
}
