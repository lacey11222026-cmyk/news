using BIZ;
using BIZ.Entity;
using DATA;
using DATA.DocumentDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
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

            //ConvertNewsImg();
            //ConvertNewsContentFile();
            //ConvertNews();
            return View(lstcategory);
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
                    doc.Title = item.title;
                    doc.IntroText = UTILS.Utils.RemoveAllHtmlTags(item.description);
                    doc.Contents = item.content;

                    doc.Hits = item.views;

                    //chinh tri xh
                    if (item.category_id == 1|| item.category_id == 3)
                    {
                        doc.CategoryId = 7;
                        doc.CategoryPathway = ",7,";
                    }
                    //cong doan viet nam
                    if (item.category_id == 2)
                    {
                        doc.CategoryId = 15;
                        doc.CategoryPathway = ",15,";
                    }
                    //bo nn
                    if (item.category_id == 4)
                    {
                        doc.CategoryId = 19;
                        doc.CategoryPathway = ",19,";
                    }
                    //cong doan nn
                    if (item.category_id == 8)
                    {
                        doc.CategoryId = 53;
                        doc.CategoryPathway = ",53,";
                    }

                    //cap tren co so
                    if (item.category_id == 5)
                    {
                        doc.CategoryId = 67;
                        doc.CategoryPathway = ",67,";
                    }
                    //co so
                    if (item.category_id == 6)
                    {
                        doc.CategoryId = 54;
                        doc.CategoryPathway = ",54,";
                    }
                   
                    doc.Params = "";
                    doc.Thumbnail = "";

                    doc.CreatedDate = DateTime.Now.AddYears(-5);
                    doc.PublishDate = DateTime.Now.AddYears(-5);




                    doc.Image = item.image;



                    doc.CreatedDate = item.created_at.DateTime;
                    doc.PublishDate = item.publish_date.DateTime;

                    new ContentBO().CreateUpdateContent(doc);
                    System.Threading.Thread.Sleep(100);
                }
                catch
                {
                    NLogLogger.DebugMessage("Error" + item.id);
                }

            }
        }
        private void ConvertNewsImg()
        {
            var lstdata = OfficialDAL.GetTop(19);

            foreach (var item in lstdata)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(item.image))
                        continue;

                    DownloadNewsImage(item.image);

                    // Nghỉ 100ms tránh request quá nhanh
                    System.Threading.Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    NLogLogger.DebugMessage(
                        "Error ID: " + item.id +
                        " | Image: " + item.image +
                        " | " + ex.Message
                    );
                }
            }
        }

        private void DownloadNewsImage(string image)
        {
            const string baseUrl =
                "http://khuyencongonline.gov.vn/media/";

            const string rootPath =
                @"C:\WebServer\khuyencong\web\Media";

            // Ví dụ:
            // images/news/2018/11/KC_Quang_ngai_1.JPG
            string relativePath = image
                .Trim()
                .TrimStart('/');

            if (string.IsNullOrEmpty(relativePath))
                return;

            // Trường hợp dữ liệu có media/ ở đầu thì bỏ media/
            if (relativePath.StartsWith(
                "media/",
                StringComparison.OrdinalIgnoreCase))
            {
                relativePath = relativePath.Substring(6);
            }

            // URL download
            string imageUrl =
                baseUrl + relativePath.Replace("\\", "/");

            // Chuyển đường dẫn URL thành đường dẫn Windows
            string localRelativePath =
                relativePath.Replace(
                    "/",
                    Path.DirectorySeparatorChar.ToString()
                );

            // Đường dẫn file local
            string localPath =
                Path.Combine(rootPath, localRelativePath);

            // Đã tồn tại thì bỏ qua
            if (System.IO.File.Exists(localPath))
                return;

            // Tạo folder nếu chưa tồn tại
            string directory = Path.GetDirectoryName(localPath);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Download
            using (var client = new WebClient())
            {
                client.Headers.Add(
                    "User-Agent",
                    "Mozilla/5.0"
                );

                client.DownloadFile(
                    imageUrl,
                    localPath
                );
            }
        }

        private void ConvertNewsContentFile()
        {
            var lstdata = OfficialDAL.GetTop(19);

            foreach (var item in lstdata)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(item.content))
                        continue;

                    DownloadFilesFromContent(item.content, item.id);

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    NLogLogger.DebugMessage(
                        "Content Error ID: " + item.id +
                        " | " + ex.Message
                    );
                }
            }
        }

        private void DownloadFilesFromContent(string content, long newsId)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;

            // Bắt cả:
            // src="/media/uploads/..."
            // src='/media/uploads/...'
            // href="/media/uploads/..."
            // href='/media/uploads/...'
            //
            // (?<url>...) dùng để lấy riêng URL
            string pattern =
                @"(?:src|href)\s*=\s*[""'](?<url>/media/uploads/[^""']+)[""']";

            MatchCollection matches = Regex.Matches(
                content,
                pattern,
                RegexOptions.IgnoreCase
            );

            if (matches.Count == 0)
                return;

            // Tránh tải 1 file 2 lần nếu vừa có src vừa có href
            HashSet<string> urls =
                new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (Match match in matches)
            {
                string url = match.Groups["url"].Value;

                if (!string.IsNullOrWhiteSpace(url))
                    urls.Add(url);
            }

            foreach (string url in urls)
            {
                try
                {
                    DownloadContentFile(url);

                    // Nghỉ nhẹ giữa các file
                    Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    NLogLogger.DebugMessage(
                        "Download Content File Error" +
                        " | NewsID: " + newsId +
                        " | URL: " + url +
                        " | " + ex.Message
                    );
                }
            }
        }

        private void DownloadContentFile(string mediaUrl)
        {
            const string baseUrl =
                "http://khuyencongonline.gov.vn";

            const string rootPath =
                @"C:\WebServer\khuyencong\web\Media";

            if (string.IsNullOrWhiteSpace(mediaUrl))
                return;

            mediaUrl = mediaUrl.Trim();

            // Chỉ xử lý /media/uploads/
            if (!mediaUrl.StartsWith(
                "/media/uploads/",
                StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // -------------------------------------------
            // URL nguồn
            // -------------------------------------------
            // /media/uploads/2026/09/11/a.jpg
            //
            // =>
            //
            // http://khuyencongonline.gov.vn/media/uploads/2026/09/11/a.jpg

            string downloadUrl =
                baseUrl + mediaUrl;


            // -------------------------------------------
            // Đường dẫn local
            // -------------------------------------------
            // Bỏ "/media/" ở đầu
            //
            // /media/uploads/2026/09/11/a.jpg
            //
            // =>
            //
            // uploads/2026/09/11/a.jpg
            // -------------------------------------------

            string relativePath =
                mediaUrl.Substring("/media/".Length);

            relativePath =
                relativePath
                    .TrimStart('/')
                    .Replace(
                        "/",
                        Path.DirectorySeparatorChar.ToString()
                    );

            string localPath =
                Path.Combine(
                    rootPath,
                    relativePath
                );


            // -------------------------------------------
            // File đã tồn tại -> bỏ qua
            // -------------------------------------------

            if (System.IO.File.Exists(localPath))
                return;


            // -------------------------------------------
            // Tạo folder
            // -------------------------------------------

            string directory =
                Path.GetDirectoryName(localPath);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);


            // -------------------------------------------
            // Download
            // -------------------------------------------

            using (var client = new WebClient())
            {
                client.Headers.Add(
                    "User-Agent",
                    "Mozilla/5.0"
                );

                client.DownloadFile(
                    downloadUrl,
                    localPath
                );
            }
        }
        [LocalizationActionFilter]
        public ActionResult Language(string lang)
        {
            WorkContext.SetLanguage(lang);

            return RedirectToAction("Index");
        }
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
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

        public ActionResult BannerRight3(string lang)
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 6, 1);
            ViewBag.lang = lang;
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
                lstBanner = new BannerBO().GetTopLastestBanners(0, 8, 1);
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
            var lstdata = new ContentBO().GetHotNews(6, 5);
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

        public ActionResult Slide()
        {
            //var Title = Utils.ReplaceVietnameseChar("Phú Thọ xây nhà máy phát điện từ rác thải");
            // var lstid = new SystemConfigBO().GetValueByKey("HotNewsForCate_"+Config.WebSite);
            //var lstTopViewId = new SystemConfigBO().GetValueByKey("TopViewNews_" + Config.WebSite);
            //var lstHotNews = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);
            var lstcontent = new ContentBO().GetTopLastestContentFulls(30, 0, "");
            if (lstcontent != null)
            {
                lstcontent = lstcontent.Where(x => x.Type == 1).ToList();
            }
            //var lstTopViewNews = new ContentBO().GetTopContentByIdsFulls(lstTopViewId, 0, true);

            var lstHotNews = new List<CONTENT_FULL>();
            var lstTopViewNews = new List<CONTENT_FULL>();
            //if (lang == "vi-vn")
            //{
            //    lstHotNews = new HotNewsBO().GetTopHotNews(0, "hotnews", 1);
            //    lstTopViewNews = new HotNewsBO().GetTopHotNews(0, "topviewnews", 1);
            //}
            //else
            //{
            //    lstHotNews = new HotNewsBO().GetTopHotNews(0, "hotnewsen", 1);
            //    lstTopViewNews = new HotNewsBO().GetTopHotNews(0, "topviewnewsen", 1);
            //}

            //lstHotNews = new ContentBO().GetHotNews(0, 5);
            //lstTopViewNews = new ContentBO().GetHotNews(-1, 5);
            var configValue = new SystemConfigBO().GetByKey("HotNewsForCate_0");
            if (configValue != null)
            {

                lstHotNews = new ContentBO().GetTopContentByIdsFulls(configValue.ConfigValue, 9, true);

            }
            var configValue2 = new SystemConfigBO().GetByKey("HotNewsForCate_-1");
            if (configValue2 != null)
            {

                lstTopViewNews = new ContentBO().GetTopContentByIdsFulls(configValue2.ConfigValue, 9, true);

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
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]

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
                //Css = cssClass,
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

        public ActionResult TopDocument(int CategoryId, int MaxLastestNews = 3)
        {
            // var MaxDocuments = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDocuments"]);
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxLastestNews, CategoryId);
            ViewBag.CateId = CategoryId;
            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
            ViewBag.CateName = cateobj.Name;
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


            var lstBanner = new List<Banner>();


            //if (WorkContext.GetLanguage()=="vi-vn")
            //{
            //    lstBanner = new BannerBO().GetTopLastestBanners(0, 3, 1);
            //}
            //else
            //{
            //    lstBanner = new BannerBO().GetTopLastestBanners(0, 9, 1);
            //}
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            //if (mobileHelper.DetectMobileLong())
            //    return View("MIndex");

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
