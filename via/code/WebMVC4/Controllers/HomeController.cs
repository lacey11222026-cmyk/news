using BIZ;
using BIZ.Entity;
using DATA;
using DATA.DocumentDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
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

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Intro(int CategoryId)
        {
            var intro = new CategoryBO().GetCategoryFull(CategoryId);

            return PartialView(intro);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult MenuMobile(string lang)
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);
            ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", ngày" + DateTime.Now.ToString(" dd ") + "/" + DateTime.Now.ToString(" MM ") + "/" + DateTime.Now.Year.ToString();
            return PartialView(lstcategory.Where(x => x.Language == lang).ToList());
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Menu(string lang)
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);
            ViewBag.Date = Utils.formatDateofWeek(DateTime.Now.DayOfWeek) + ", ngày" + DateTime.Now.ToString(" dd ") + "/" + DateTime.Now.ToString(" MM ") + "/" + DateTime.Now.Year.ToString();
            return PartialView(lstcategory.Where(x => x.Language == lang).ToList());
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult MenuBottom(string lang)
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.Footer, 18, false);
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
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult BannerRight(int top = 0, string cssClass = "", string lang = "vi-vn")
        {
            var lstBanner = new List<DATA.Banner>();
            if (lang == "vi-vn")
                lstBanner = new BannerBO().GetTopLastestBanners(top, 2, 1);
            else
                lstBanner = new BannerBO().GetTopLastestBanners(top, 2, 1);
            ViewBag.cssClass = cssClass;
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
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
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

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
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult BannerRight3()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 4, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult Banner(string lang = "vi-vn")
        {
            var lstBanner = new List<DATA.Banner>();
            if (lang == "vi-vn")
                lstBanner = new BannerBO().GetTopLastestBanners(0, 1, 1);
            else
                lstBanner = new BannerBO().GetTopLastestBanners(0, 1, 1);

            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

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

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult HomeVideo(int CategoryId, string CateName)
        {
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);
            //var lstid = Utils.GetAppSettingValue("HotVideo");
            //var lstid = new SystemConfigBO().GetValueByKey("HotVideo");
            var lstdata = new ContentBO().GetHotNews(CategoryId, 5);
            //var lstdata = new ContentBO().GetTopLastestContentFulls(5, 6);
            return PartialView(lstdata);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

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
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult Slide(string lang)
        {
            //var Title = Utils.ReplaceVietnameseChar("Phú Thọ xây nhà máy phát điện từ rác thải");
            // var lstid = new SystemConfigBO().GetValueByKey("HotNewsForCate_"+Config.WebSite);
            //var lstTopViewId = new SystemConfigBO().GetValueByKey("TopViewNews_" + Config.WebSite);
            //var lstHotNews = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);
            var lstcontent = new ContentBO().GetTopLastestContentFulls(30, 0, "",lang);
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
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult TopAlbum(string CateName, int CategoryId, int Top)
        {
            //ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.Album);
            //ViewBag.CateName = CateName;
            var lstdata = new AlbumBO().GetTopLastestAlbumsFull(Top, CategoryId);

            try
            {
                //var lstid = new SystemConfigBO().GetValueByKey("HotAlbum");
                //if (string.IsNullOrEmpty(lstid))
                //{
                //    return PartialView(lstdata);
                //}
                //var lstcontent = new AlbumBO().GetTopAlbumByIdsFulls(lstid, 0, true).ToList();

                //if (lstcontent == null)
                //{
                //    return PartialView(lstdata);
                //}

                //foreach (var item in lstdata)
                //{

                //    if (lstcontent.Where(x => x.Id == item.Id).ToList().Count == 0)
                //    {
                //        lstcontent.Add(item);

                //    }
                //}
                //if (lstcontent != null)
                //    lstcontent = lstcontent.Take(Top).ToList();
                //return PartialView(lstcontent);
            }
            catch
            {

                return PartialView(lstdata);
            }
            return PartialView(lstdata);
            //return PartialView(Albums);
        }

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

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
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

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
            foreach (var item in lstdata)
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
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNews2(string CateName, int CategoryId, int MaxLastestNews = 0, string lang = "")
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
                //Css = cssClass,
                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNews4(int MaxLastestNews, string lang)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

            }

            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, 0, "", lang).ToList();


            return PartialView(lstdata);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNews5(int MaxLastestNews, string lang)
        {

            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);

            }

            var lstTopViewId = new SystemConfigBO().GetValueByKey("TopViewNews_" + Config.WebSite);
            var lstData = new ContentBO().GetTopViewContentFulls(MaxLastestNews, 0, lang);
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
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult LastestNews(string CateName, int CategoryId, int MaxLastestNews = 0, string cssClass = "", string lang = "")
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
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]

        public ActionResult TopDocument(int CategoryId)
        {
            var MaxDocuments = 6;
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxDocuments, CategoryId);
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult TopMission(string lang)
        {
            var MaxDocuments = 3;
            var CategoryId = 2;
            if (lang == "vi-vn")
                CategoryId = 1;
            var lstcontent = new MissionBO().GetTopLastestMissionsFull(MaxDocuments, CategoryId);
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult TopRightMission(string lang)
        {
            var MaxDocuments = 3;
            var CategoryId = 2;
            if (lang == "vi-vn")
                CategoryId = 1;
            var lstcontent = new MissionBO().GetTopLastestMissionsFull(MaxDocuments, CategoryId);
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
            //ConvertNews();
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

        private void ConvertDoc()
        {
            var lstdata = OfficialDAL.Get();
            foreach (var item in lstdata)
            {
                try

                {
                    var doc = new DOCUMENT_FULL();
                    doc.CreatedBy = "quantri";
                    doc.DocType = 1;
                    doc.Name = item.Trich_yeu;
                    doc.Code = item.So_hieu; ;
                    //category
                    
                    doc.CreatedDate = item.Ngay_ban_hanh;
                    //ngày ban hành
                    doc.PublishDate = item.Ngay_ban_hanh;
                    doc.EffectiveDate = item.Ngay_co_hieu_luc;

                    doc.ExpiryDate = Utils.ConvertToDate("01/01/9999", "dd-MM-yyyy");
                    doc.SignedBy = item.Nguoi_ky_duyet;
                    doc.SignedByDesc = "";
                    doc.Description = "";

                    doc.Hits = 1;//còn hiệu lực
                    doc.DocType = 1;

                    doc.CategoryId = GetCate(item.DocumentTypeName);
                    doc.Agent = GetCoquan(item.Co_quan_ban_hanh);
                    doc.Type =GetType(item.Hinh_thuc_van_ban);
                    doc.Area = GetLinhVuc(item.Linh_vuc);
                    doc.Status = 1;
                    doc.FilePath = "";
                    if (doc.CategoryId > 0 &&doc.Agent>0 && doc.Type>0)
                    {
                        doc.CategoryPathway = $",3,{doc.CategoryId.ToString()},";



                        //file

                        new DocumentBO().CreateUpdateDocument(doc);


                    }
                    

                }
                catch
                {
                    NLogLogger.DebugMessage("Error" + item.So_hieu);
                }

            }
        }
        //43
        private void ConvertNews()
        {
            var lstdata = LicensingDAL.SelectDynamic();
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
                    doc.Title = item.Tieu_de;
                    doc.IntroText = item.Tom_tat_noi_dung;
                    doc.Contents = item.ParagraphContent.Replace("https://cdn.via.gov.vn/", "https://media02via.vtkmedia.vn/");
                    doc.Url ="";
                    doc.Hits = item.So_lan_xem;
                    
                    doc.Params = "";
                    doc.Thumbnail = "";
                    doc.SiteId = 0;
                  
                    doc.Type = 1;
                    doc.PublishDate = item.PublishOnDate;
                    doc.CreatedDate = item.PublishOnDate;

                    doc.Image = "https://media02via.vtkmedia.vn/"+item.Duong_dan_anh_dai_dien;

                    doc.CategoryId = GetNewsCate(item.PrimaryTermName);
                    doc.CategoryPathway = GetSNewsCate(item.PrimaryTermName);
                    if (doc.CategoryId>0)
                    {
                        new ContentBO().CreateUpdateContent(doc);
                        System.Threading.Thread.Sleep(100);
                    }    

                    
                }
                catch
                {
                    NLogLogger.DebugMessage("Error" );
                }

            }
        }
        public int GetNewsCate(string type)
        {
            int result = 0;
            switch (type)
            {

                case "Tin tức - Sự kiện":
                    result = 5;
                    break;
                case "Hoạt động công thương":
                    result = 38;
                    break;

                case "Hợp tác quốc tế":
                    result = 81;
                    break;

                case "Lĩnh vực Công nghiệp chế biến, chế tạo":
                    result = 86;
                    break;

                case "Lĩnh vực Công nghiệp hỗ trợ":
                    result = 40;
                    break;
                case "Lĩnh vực công nghiệp thực phẩm":
                    result = 95;
                    break;

                case "Lĩnh vực khoáng sản luyện kim":
                    result = 80;
                    break;
                case "Chương trình đề án":
                    result = 69;
                    break;

                case "Tin mới nhất":
                    result = 96;
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }
        public string GetSNewsCate(string type)
        {
            string result = "";
            switch (type)
            {

                case "Tin tức - Sự kiện":
                    result = ",5,";
                    break;
                case "Hoạt động công thương":
                    result = ",5,38,";
                    break;

                case "Hợp tác quốc tế":
                    result = ",5,81,";
                    break;

                case "Lĩnh vực Công nghiệp chế biến, chế tạo":
                    result = ",5,86,";
                    break;

                case "Lĩnh vực Công nghiệp hỗ trợ":
                    result = ",5,40,";
                    break;
                case "Lĩnh vực công nghiệp thực phẩm":
                    result = ",5,95,";
                    break;

                case "Lĩnh vực khoáng sản luyện kim":
                    result = ",5,80,";
                    break;
                case "Chương trình đề án":
                    result = ",69,";
                    break;
                case "Tin mới nhất":
                    result = ",96,";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }
        public  int GetCate(string type)
        {
            int result = 0;
            switch (type)
            {

                case "Văn bản điều hành":
                    result =68;
                    break;
                case "Văn bản pháp quy":
                    result = 67;
                    break;
               
                case "Văn bản hợp nhất":
                    result = 37;
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }
        public int GetCoquan(string type)
        {
            int result = 0;
            switch (type)
            {

                case "Bộ Công thương":
                    result = 2;
                    break;
                case "Thủ tướng Chính Phủ":
                    result = 11;
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }
        public int GetLinhVuc(string type)
        {
            int result = 0;
            switch (type)
            {

                case "Công nghiệp chế biến, chế tạo":
                    result = 3;
                    break;
                case "Công nghiệp hỗ trợ":
                    result = 4;
                    break;
                case "Công nghiệp nặng":
                    result = 1;
                    break;
                case "Công nghiệp thực phẩm":
                    result = 2;
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }
        public int GetType(string type)
        {
            int result = 0;
            switch (type)
            {

                case "Công điện":
                    result = 18;
                    break;
                case "Quy định":
                    result = 19;
                    break;
                case "Quyết định":
                    result = 11;
                    break;
                case "Thông tư":
                    result = 13;
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }
    }
}
