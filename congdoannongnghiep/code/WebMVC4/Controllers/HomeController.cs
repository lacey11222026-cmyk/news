using BIZ;
using BIZ.Entity;
using DATA.DocumentDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Models;
using Constants = UTILS.Constants;

namespace WebMVC4.Controllers
{
    public class HomeController : Controller
    {
        #region "Cache"


        [OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult Menu()
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);
            //ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + "," + DateTime.Now.ToString(" dd") + "/" + DateTime.Now.ToString("MM")+"/" + DateTime.Now.Year.ToString();
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
        //[OutputCache(Duration = 60, VaryByParam = "none")]

        public ActionResult Support()
        {
            var MaxMainSupport = Convert.ToInt32(ConfigurationManager.AppSettings["MaxMainSupport"]);
            var lstdata = new SupportBO().GetTopSupports(MaxMainSupport, false);
            return PartialView(lstdata);
        }
        [OutputCache(Duration = 60, VaryByParam = "none")]

        public ActionResult Marquee()
        {

            var lstcontent = new ContentBO().GetTopLastestContentFulls(5, 4);
            return PartialView(lstcontent);
        }

        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerRight()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 2, 1);
            return PartialView(lstBanner);
        }


        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerBottomLeft()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 6, 1);
            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerBottomRight()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 5, 1);
            return PartialView(lstBanner);
        }

       [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerBottom()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 3, 1);
            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerRight1()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 2, 1);
            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerRight2()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 6, 1);
            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerCenter1()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 4, 1);
            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerCenter2()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 5, 1);
            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerMobile()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 4, 1);
            return PartialView(lstBanner);
        }
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Banner()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 1, 1);
            return PartialView(lstBanner);
        }


        public ActionResult SearchInput()
        {
            return PartialView();
        }
        [OutputCache(Duration = 60, VaryByParam = "*")]

        public ActionResult RightVideo(int CategoryId, string CateName)
        {
            if (string.IsNullOrEmpty(CateName))
            {
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                CateName = cateobj.Name;
            }
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);
            //var lstid = Utils.GetAppSettingValue("HotVideo");
            var lstdata = new ContentBO().GetHotNews(CategoryId, 2);
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            return PartialView(lstdata);
        }
        [OutputCache(Duration = 60, VaryByParam = "*")]

        public ActionResult HomeVideo(int CategoryId, string CateName)
        {
            if (string.IsNullOrEmpty(CateName))
            {
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                CateName = cateobj.Name;
            }
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);
            //var lstid = Utils.GetAppSettingValue("HotVideo");
            var lstdata = new ContentBO().GetHotNews(CategoryId, 1);
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            return PartialView(lstdata);
        }
        [OutputCache(Duration = 60, VaryByParam = "*")]

        public ActionResult HomeAudio(int CategoryId, string CateName)
        {
            if (string.IsNullOrEmpty(CateName))
            {
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                CateName = cateobj.Name;
            }
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);
            //var lstid = Utils.GetAppSettingValue("HotVideo");
            var lstdata = new ContentBO().GetHotNews(CategoryId, 5);
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            return PartialView(lstdata);
        }
        [OutputCache(Duration = 10, VaryByParam = "*")]

        public ActionResult Slide()
        {
            var lstid = new SystemConfigBO().GetValueByKey("HotNews");
            var lstTopViewId = new SystemConfigBO().GetValueByKey("TopViewNews");
            var lstHotNews = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);

            var lstcontent = new ContentBO().GetTopLastestContentFulls(14, 0).Where(x => x.Type == 1 &&x.PublishDate<=DateTime.Now).ToList();
            var lstTopViewNews = new ContentBO().GetTopContentByIdsFulls(lstTopViewId, 0, true).Where(x => x.Type == 1 && x.PublishDate <= DateTime.Now).ToList(); ;


            var model = new SlideModel
            {
                LstHotNews = lstHotNews,
                LstLastestNews = lstcontent,
                LstLastestNews2 = lstcontent,
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
        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult RightNews( int MaxLastestNews = 0)
        {
            try
            {
                if (MaxLastestNews == 0)
                {
                    MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

                }
                //var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, -1, 1);
                var model = new LastestNewModel
                {
                    lstdata = lstdata,
                    //HeaderTitle = cateobj.Name.ToUpper(),
                    //Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, cateobj.Name, (int)UTILS.Constants.CategoryType.News),
                    //CategoryId = CategoryId
                };
                return PartialView(model);
            }
            catch (Exception)
            {

                return PartialView(null);
            }

        }
        [OutputCache(Duration = 60, VaryByParam = "*")]

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
        [OutputCache(Duration = 60, VaryByParam = "*")]

        public ActionResult LastestNews2(string CateName, int CategoryId, int MaxLastestNews = 0, string cssClass = "")
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
        [OutputCache(Duration = 60, VaryByParam = "*")]

        public ActionResult LastestNews3(string CateName, int CategoryId, int MaxLastestNews = 0, string cssClass = "")
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
        [OutputCache(Duration = 60, VaryByParam = "*")]

        public ActionResult LastestNews4(string CateName, int CategoryId, int MaxLastestNews = 0, string cssClass = "")
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
        [OutputCache(Duration = 60, VaryByParam = "none")]

        public ActionResult TopDocument()
        {
            var MaxDocuments = 3;
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxDocuments,34);
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 60, VaryByParam = "none")]

        public ActionResult TopDocument2()
        {
            var MaxDocuments = 8;
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxDocuments, 32);
            return PartialView(lstcontent);
        }
        #endregion
        //[Authorize]
        public ActionResult Index()
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];


            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            //var _childCategory = new CategoryBO().GetAllChildCategories(4, 10, false);

            //var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxDocShow"]);
            //int Total = 0;

            //var listdata = new DocumentBO().GetDocumentsSearchPaged("", 0, 1, 1, PageSize, "2014-01-01 00:00:00.000", "2014-01-17 00:00:00.000", ref Total);

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

            //ConvertNews;
            //var CreatedDate = Utils.ConvertToDate("7/5/2012 9:15", "dd//MM//yyyy HH:mm");
            //ViewBag.Title = CreatedDate.ToString();
            return View();
        }
        [OutputCache(Duration = 1200, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Weather()
        {

            var crawlctl = new CrawlBO();
            var webcontent = crawlctl.GetPage("https://nchmf.gov.vn/Kttvsite/vi-VN/1/ha-noi-w28.html");
            var nd = crawlctl.getbyclass("uk-width-3-4", "div", webcontent);
            var nd2 = crawlctl.getbyclass("uk-width-3-4", "div", webcontent, 2);
            //NLogLogger.DebugMessage(nd2);
            ViewBag.Icon = "fa-cloud";
            if (nd2.Contains("nắng"))
                ViewBag.Icon = "fa-sun-o";
            //if (nd2.Contains("mưa"))
            //    ViewBag.Icon = "fa-bolt";
            //if (nd2.Contains("không mưa"))
            //    ViewBag.Icon = "fa-bolt";
            ViewBag.ce = nd.Replace(": ", "");
            return PartialView();
        }
        private void ConvertNews()
        {
            var lstdata = OfficialDAL.GetTop(19);
            foreach (var item in lstdata)
            {
                try

                {
                    var doc = new CONTENT_FULL();
                    doc.CategoryId = 0;
                    doc.CreatedBy = "quantri";
                    doc.Alias = "quantri";
                    doc.Status = 4;
                    doc.Mark = 0;
                    doc.Title = item.TieuDe;
                    doc.IntroText = item.TomTat;
                    doc.Contents = item.NoiDung;
                    doc.Url = item.ProCode;
                    doc.Hits = item.Viewed;
                    doc.ChannelId = 0;
                    //chinh tri xh
                    if (item.IdList == 19)
                    {
                        doc.CategoryId = 11;
                        doc.CategoryPathway = ",10,11,";
                    }
                    //cong doan viet nam
                    if (item.IdList == 38)
                    {
                        doc.CategoryId = 32;
                        doc.CategoryPathway = ",10,12,";
                    }
                    //bo nn
                    if (item.IdList == 37)
                    {
                        doc.CategoryId = 13;
                        doc.CategoryPathway = ",10,13,";
                    }
                    //cong doan nn
                    if (item.IdList == 17)
                    {
                        doc.CategoryId = 14;
                        doc.CategoryPathway = ",10,14,";
                    }

                    //cap tren co so
                    if (item.IdList == 20)
                    {
                        doc.CategoryId = 15;
                        doc.CategoryPathway = ",10,15,";
                    }
                    //co so
                    if (item.IdList == 18)
                    {
                        doc.CategoryId = 16;
                        doc.CategoryPathway = ",10,16,";
                    }
                    //dia phuong
                    if (item.IdList == 22)
                    {
                        doc.CategoryId = 17;
                        doc.CategoryPathway = ",10,17,";
                    }
                    //cac cap
                    if (item.IdList == 70)
                    {
                        doc.CategoryId = 18;
                        doc.CategoryPathway = ",10,18,";
                    }

                    //to chuc
                    if (item.IdList == 23)
                    {
                        doc.CategoryId = 20;
                        doc.CategoryPathway = ",19,20,";
                    }
                    //thi dua
                    if (item.IdList == 24)
                    {
                        doc.CategoryId = 21;
                        doc.CategoryPathway = ",19,21,";
                    }
                    //tuyen giao
                    if (item.IdList == 27)
                    {
                        doc.CategoryId = 22;
                        doc.CategoryPathway = ",19,22,";
                    }
                    //nu cong
                    if (item.IdList == 25)
                    {
                        doc.CategoryId = 23;
                        doc.CategoryPathway = ",19,23,";
                    }

                    //dao tao
                    if (item.IdList == 28)
                    {
                        doc.CategoryId = 24;
                        doc.CategoryPathway = ",19,24,";
                    }
                    //kiem tra
                    if (item.IdList == 39)
                    {
                        doc.CategoryId = 25;
                        doc.CategoryPathway = ",19,25,";
                    }
                    //an toan
                    if (item.IdList == 57)
                    {
                        doc.CategoryId = 26;
                        doc.CategoryPathway = ",19,26,";
                    }
                    //đoi ngao
                    if (item.IdList == 6)
                    {
                        doc.CategoryId = 27;
                        doc.CategoryPathway = ",27,";
                    }
                    //nghien cuu trao doi
                   
                    if (item.IdList == 60)
                    {
                        doc.CategoryId = 30;
                        doc.CategoryPathway = ",28,30,";
                    }
                    //tin hoat dong

                    if (item.IdList == 59)
                    {
                        doc.CategoryId = 31;
                        doc.CategoryPathway = ",28,31,";
                    }

                    //chinh sach phap luat
                    if (item.IdList == 26)
                    {
                        doc.CategoryId = 33;
                        doc.CategoryPathway = ",33,";
                    }
                    //lao động giỏi
                    if (item.IdList == 43)
                    {
                        doc.CategoryId = 38;
                        doc.CategoryPathway = ",38,";
                    }
                    try
                    {
                        doc.CreatedDate = Utils.ConvertToDate(item.Ngay, "dd-MM-yyyy");
                        doc.PublishDate = Utils.ConvertToDate(item.Ngay, "dd-MM-yyyy");
                    }
                    catch
                    {
                        doc.CreatedDate = DateTime.Now.AddYears(-5);
                        doc.PublishDate = DateTime.Now.AddYears(-5);
                    }
                    doc.Params = "";
                    doc.Thumbnail = "";






                    doc.Image = item.ImgLink0;





                    new ContentBO().CreateUpdateContent(doc);
                    System.Threading.Thread.Sleep(100);
                }
                catch
                {
                    NLogLogger.DebugMessage("Error" + item.IdTinTuc);
                }

            }
        }
        private void ConvertDoc()
        {
            var lstdata = OfficialDAL.GetTopLastestDocuments(2);
            foreach (var item in lstdata)
            {
                try

                {
                    var doc = new DOCUMENT_FULL();
                    doc.CreatedBy = "quantri";
                    //doc.DocType = 1;
                    doc.Name = item.TieuDe;
                    doc.Code = item.MaSo;
                    //category
                    if (item.IdList == 8)
                    {
                        doc.CategoryId = 34;
                        doc.CategoryPathway = ",34,";
                    }
                    if (item.IdList == 61)
                    {
                        doc.CategoryId = 32;
                        doc.CategoryPathway = ",28,32,";
                    }
                    if(item.Ngay.Length<8)
                    {
                        doc.CreatedDate = DateTime.Now.AddYears(-5);
                        doc.PublishDate = DateTime.Now.AddYears(-5);
                    }
                    else
                    {
                        doc.CreatedDate = Utils.ConvertToDate(item.Ngay, "dd-MM-yyyy");

                        //ngày ban hành
                        doc.PublishDate = Utils.ConvertToDate(item.Ngay, "dd-MM-yyyy");
                    }
                   

                    doc.ExpiryDate = Utils.ConvertToDate("01/01/9999", "dd-MM-yyyy");
                    doc.EffectiveDate = Utils.ConvertToDate("01/01/9999", "dd-MM-yyyy");
                    doc.SignedBy = item.IdType.ToString();
                    doc.SignedByDesc = item.IdCoquan.ToString();
                    if (string.IsNullOrEmpty(item.TomTat))
                    {
                        doc.Description = "";
                    }
                    else
                    {
                        doc.Description = Utils.RemoveAllHtmlTags(item.TomTat);
                    }
                    
                    doc.Status = 1;
                 
                    doc.FilePath = item.FileLink;


                 


                    new DocumentBO().CreateUpdateDocument(doc);
                }
                catch
                {
                    NLogLogger.DebugMessage("Error" + item.IdVanBan);
                }

            }
        }
    }
}
