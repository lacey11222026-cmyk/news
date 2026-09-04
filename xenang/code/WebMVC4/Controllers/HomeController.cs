using BIZ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Helper;
using WebMVC4.Models;
using WebMVC4.Filter;
using LibGraph;

namespace WebMVC4.Controllers
{
    public class HomeController : Controller
    {
        #region "Cache"

        [OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
       
        public ActionResult Menu()
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);

            return PartialView(lstcategory.Where(x=>x.ParentId==0).ToList());
        }
        [OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
        
        public ActionResult MenuProduct()
        {
            var lstcategory = new CategoryBO().GetAllCategoriesFull((int)UTILS.Constants.CategoryType.Product);

            return PartialView(lstcategory.Where(x => x.Published == 1).ToList());
        }
        //[OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]

        public ActionResult MenuProductMobile()
        {
            var lstcategory = new CategoryBO().GetAllCategoriesFull((int)UTILS.Constants.CategoryType.Product);

            return PartialView(lstcategory.Where(x => x.Published == 1).ToList());
        }
        [OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]

        public ActionResult MenuMobile()
        {
            var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 18, false);

            return PartialView(lstcategory.Where(x => x.ParentId == 0).ToList());
        }
        [OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult FooterMenu()
        {
            var lstcategory = new CategoryBO().GetAllCategoriesFull(0);

            return PartialView(lstcategory.Where(x => x.ParentId == 0).ToList());
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
      
        public ActionResult IntroMenu(int categoryId)
        {
            var lstcategory = new CategoryBO().GetAllChildCategories(categoryId,10,false);
            if(lstcategory!=null)
                lstcategory= lstcategory.Where(x => x.Published == 1).ToList();
            return PartialView(lstcategory);
        }
		[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult ProductCate()
        {
            var lstcategory = new CategoryBO().GetAllCategoriesFull(0, WorkContext.GetLanguage());

            return PartialView(lstcategory.Where(x => x.ParentId == 0).ToList());
        }
        //[OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
        
        public ActionResult Support()
        {
           
            return View();
        }
        [OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult Marquee()
        {

            var lstcontent = new ContentBO().GetTopLastestContentFulls(5, 4);
            return PartialView(lstcontent);
        }

        [ChildActionOnly]
        public ActionResult BannerRight()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 5, 1);
            return PartialView(lstBanner);
        }
        [ChildActionOnly]
        public ActionResult BannerHomeRight()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 1, 1);
            return PartialView(lstBanner);
        }

        [ChildActionOnly]
        public ActionResult BannerTop()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 4, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult BannerFooter()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 5, 1);
            return PartialView(lstBanner);
        }

        [OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult BannerBottom()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 3, 1);
            return PartialView(lstBanner);
        }

        [OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult Banner()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 1, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Intro(int CategoryId)
        {
            var intro = new CategoryBO().GetCategoryFull(CategoryId);
           
            return PartialView(intro);
        }
        [ChildActionOnly]
        public ActionResult SearchInput()
        {
            return PartialView();
        }

        //[OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
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
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            return PartialView(lstdata);
        }

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Slide()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 2, 1);
            return PartialView(lstBanner);
            
        }

        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult LastestNews(string CateName, int CategoryId, bool IsMobile=false,int MaxLastestNews = 0)
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
            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = CateName,
                Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News),
                CategoryId = CategoryId,
                IsMobile = IsMobile
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 120, VaryByParam = "none", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult TopDocument()
        {
            var MaxDocuments = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDocuments"]);
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxDocuments);
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult TopGroup()
        {

            return PartialView();
        }
        #endregion
        //[Authorize]

        [LocalizationActionFilter]
        public ActionResult Index()
        {
           
            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;


            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();

            ViewBag.BodyClass = "common-home";

            //var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.Footer, 18, false);

            //return View(lstcategory.Where(x => x.ParentId == 0).ToList());
            return View();
        }
        [LocalizationActionFilter]
        public ActionResult Product()
        {
            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;


            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();

            ViewBag.BodyClass = "product-category";
            return View();
        }
        [LocalizationActionFilter]
        public ActionResult ProductDetail()
        {
            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;


            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();

            ViewBag.BodyClass = "product-category";
            return View();
        }
        [LocalizationActionFilter]
        public ActionResult Language(string lang)
        {
            WorkContext.SetLanguage(lang);
            OrderHelper.ClearCart();
            WorkContext.RemoveSessionKey(OrderConstants.SessionOrderModelKey);
            return RedirectToAction("Index");
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
