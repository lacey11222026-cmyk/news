using BIZ;
using DATA.DocumentDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;

namespace Local.Controllers
{
    public class HomeController : Controller
    {
        #region "Cache"


        [ChildActionOnly]
        public ActionResult Menu()
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);
            //ViewBag.Date = "Hôm nay " + Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", ngày" + DateTime.Now.ToString(" dd ") + "tháng" + DateTime.Now.ToString(" MM ")+"năm " + DateTime.Now.Year.ToString();
            return PartialView(lstcategory);
        }
        [ChildActionOnly]
        public ActionResult MenuLeft()
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.Footer, 18, false);
            //ViewBag.Date = "Hôm nay " + Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", ngày" + DateTime.Now.ToString(" dd ") + "tháng" + DateTime.Now.ToString(" MM ") + "năm " + DateTime.Now.Year.ToString();
            return PartialView(lstcategory);
        }
        [OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult Support()
        {
            var MaxMainSupport = Convert.ToInt32(ConfigurationManager.AppSettings["MaxMainSupport"]);
            var lstdata = new SupportBO().GetTopSupports(MaxMainSupport, false);
            return PartialView(lstdata);
        }
        [OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult Marquee()
        {

            var lstcontent = new ContentBO().GetTopLastestContentFulls(5, 4);
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult BannerRight()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 2, 1);
            return PartialView(lstBanner);
        }

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult BannerBottom()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 3, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult Banner()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 1, 1);
            return PartialView(lstBanner);
        }

        [ChildActionOnly]
        public ActionResult SearchInput()
        {
            return PartialView();
        }

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult HomeVideo(int CategoryId, string CateName)
        {
            if (string.IsNullOrEmpty(CateName))
            {
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                CateName = cateobj.Name;
            }
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);
            var lstid = Utils.GetAppSettingValue("HotVideo");
            var lstdata = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);
            return PartialView(lstdata);
        }

        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult Slide()
        {
            var lstid = Utils.GetAppSettingValue("HotNews");
            var lstcontent3 = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);
            var lstcontent = new ContentBO().GetTopLastestContentFulls(14, 0);
            var lstcontent2 = new ContentBO().GetTopViewContentFulls(14, 0);
            ViewBag.Contents = lstcontent2;
            ViewBag.HotNews = lstcontent3;
            return PartialView(lstcontent);
        }

        [OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult TopAlbum(string CateName, int CategoryId, int Top)
        {
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.Album);
            ViewBag.CateName = CateName;
            var Albums = new AlbumBO().GetTopLastestAlbumsFull(Top, CategoryId);
            return PartialView(Albums);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
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
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult LastestNews2(int CategoryId, int MaxLastestNews = 0)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

            }
            ViewBag.CategoryId = CategoryId;
            
            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            return PartialView(lstdata);
        }
        //[OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult TopDocument(int CategoryId ,int Top)
        {
            //var MaxDocuments = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDocuments"]);
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(Top, CategoryId);


            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
            //ViewBag.CateName = cateobj.Name;
            ViewBag.CategoryId = CategoryId;
            //ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, cateobj.Name, (int)UTILS.Constants.CategoryType.Doc);
            return PartialView(lstcontent);
        }
        public ActionResult TopDocumentHome(int Top)
        {
            //var MaxDocuments = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDocuments"]);
            var lstcontent = DocumentHomeDAL.GetTopLastestDocuments(Top, -1);


            
            return PartialView(lstcontent);
        }
       
        #endregion
         [Authorize]
        public ActionResult Index()
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            //var _childCategory = new CategoryBO().GetAllChildCategories(4, 10, false);

            //var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxDocShow"]);
            //int Total = 0;

            //var listdata = new DocumentBO().GetDocumentsSearchPaged("", 0, 1, 1, PageSize, "2014-01-01 00:00:00.000", "2014-01-17 00:00:00.000", ref Total);
            var data = new ContentBO().GetTopLastestContentFulls(1, 6).FirstOrDefault(); ;
            return View(data);
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
    }
}
