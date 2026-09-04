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
using System.Text;
using System.Net;
using System.IO;

namespace WebMVC4.Controllers
{
    public class HomeController : Controller
    {
        #region "Cache"

        [OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult Menu()
        {
            
          
            var model = new MenuModel
            {
                ListCate= new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.None).Where(x=>x.Published==1).ToList(),
                //ListSize= new CarSizeBO().GetTopLastestCarSize(-1, -1, 1),
                //ListManu= new ManufactoryBO().GetAllManufactoryFulls(-1, -1, 1)
                ListMainCate= new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu,10,false).Where(x => x.Published == 1).ToList(),

            };
            return PartialView(model);
        }
        [ChildActionOnly]
        public ActionResult MenuMobile()
        {


            var model = new MenuModel
            {
                //ListCate = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.None).Where(x => x.Published == 1).ToList(),
                //ListSize= new CarSizeBO().GetTopLastestCarSize(-1, -1, 1),
                //ListManu= new ManufactoryBO().GetAllManufactoryFulls(-1, -1, 1)
                ListMainCate = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 10, false).Where(x => x.Published == 1).ToList(),

            };
            return PartialView(model);
        }
        //[OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult MenuLeft()
        {


            var model = new MenuModel
            {
                ListCate = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.None).Where(x => x.Published == 1).ToList(),
                //ListSize= new CarSizeBO().GetTopLastestCarSize(-1, -1, 1),
                //ListManu= new ManufactoryBO().GetAllManufactoryFulls(-1, -1, 1)
                ListMainCate = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.MainMenu, 10, false).Where(x => x.Published == 1).ToList(),


            };
            return PartialView(model);
        }
        [OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult FooterMenu()
        {
            var model = new MenuModel
            {
                ListCate = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.None).Where(x => x.Published == 1).ToList(),
                
                ListManu = new ManufactoryBO().GetAllManufactoryFulls(-1, -1, 1)

            };
            return PartialView(model);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
      
        public ActionResult IntroMenu(int categoryId)
        {
            var lstcategory = new CategoryBO().GetAllChildCategories(categoryId,10,false);

            return PartialView(lstcategory.Where(x => x.Published == 1).ToList());
        }
		[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult ProductCate()
        {
            var lstcategory = new CategoryBO().GetAllCategoriesFull(0, WorkContext.GetLanguage());

            return PartialView(lstcategory.Where(x => x.ParentId == 0).ToList());
        }
        //[OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]
        
        public ActionResult Support()
        {
           
            return View();
        }
        [OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]
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
        [OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult BannerFooter()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 5, 1);
            return PartialView(lstBanner);
        }

        [OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult BannerBottom()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 3, 1);
            return PartialView(lstBanner);
        }

        [OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult Banner()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 2, 1);
            return PartialView(lstBanner);
        }
        [OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]
        public ActionResult Service()
        {
           // var lstBanner = new BannerBO().GetTopLastestBanners(0, 2, 1);
            return PartialView();
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult Intro(int CategoryId)
        {
            var intro = new CategoryBO().GetCategoryFull(CategoryId);
           
            return PartialView(intro);
        }
        [ChildActionOnly]
        public ActionResult SearchInput()
        {
            var CarGroupList = new CarGroupBO().GetTopLastestCarGroup(1);
            return PartialView(CarGroupList.Where(x=>x.Id>0).ToList());
        }

        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult HomeVideo(int CategoryId, string CateName,int MaxLastestNews)
        {
            if (string.IsNullOrEmpty(CateName))
            {
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                CateName = cateobj.Name;
            }
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.News);
            //var lstid = Utils.GetAppSettingValue("HotVideo");
            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            //var request = System.Web.HttpContext.Current.Request;
            //var mobileHelper = new MobileDetectHelper(request);
            //ViewBag.IsIpad = mobileHelper.DetectIpad();
            //ViewBag.Iphone = mobileHelper.DetectIphone();
            return PartialView(lstdata);
        }

        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult Slide()
        {
            var lstBanner = new BannerBO().GetTopLastestBanners(0, 2, 1);
            return PartialView(lstBanner);
            
        }

        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult LastestNews( int CategoryId, int MaxLastestNews = 0)
        {
            if (MaxLastestNews == 0)
            {
                MaxLastestNews = Convert.ToInt32(ConfigurationManager.AppSettings["MaxLastestNews"]);
            }
            
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                
            
            var lstdata = new ContentBO().GetTopLastestContentFulls(MaxLastestNews, CategoryId);
            var model = new LastestNewModel
            {
                lstdata = lstdata,
                HeaderTitle = cateobj.Name,
                Url = "/"+cateobj.Link,
                CategoryId = CategoryId,
                IsMobile = false,
            };
            return PartialView(model);
        }
        [OutputCache(Duration = 60, VaryByParam = "none", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult TopDocument()
        {
            var MaxDocuments = Convert.ToInt32(ConfigurationManager.AppSettings["MaxDocuments"]);
            var lstcontent = new DocumentBO().GetTopLastestDocumentsFull(MaxDocuments);
            return PartialView(lstcontent);
        }
        [OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult TopGroup()
        {

            var lstdata = new ManufactoryBO().GetAllManufactoryFulls(-1,-1,-1);
            return PartialView(lstdata.Where(x => x.Published == 1).ToList());
        }
        
        public ActionResult MenuGroup()
        {

            var lstdata = new ManufactoryBO().GetAllManufactoryFulls(-1,-1,-1);
            return PartialView(lstdata.Where(x => x.Published == 1).ToList());
        }
        #endregion
        //[Authorize]
        private void DownloadImage(string fromPath, string uri, string name)
        {
            try
            {
                var webClient = new WebClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                if (!Directory.Exists(fromPath))
                    Directory.CreateDirectory(fromPath);
                var url = fromPath + name + ".png";
                if (!System.IO.File.Exists(url))

                    webClient.DownloadFile(uri, url);
            }
            catch (WebException ex)
            {
                // add some kind of error processing
                NLogLogger.PublishException(ex);
            }
        }
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


            //var lstCar = new CarModelBO().GetTopLastestCarModel();

            //lstCar = lstCar.Where(x => x.Description.Length < 10).ToList();

            //var crawlctl = new CrawlBO();
            //foreach (var item in lstCar)
            //{
            //    var carhtml = "";
            //    var webcontent = crawlctl.GetPage("https://g7auto.vn/" + item.Url);
            //    var producthtml = crawlctl.getbyclass("motadm", "div", webcontent);
            //    if (!string.IsNullOrEmpty(producthtml))
            //    {
            //        var img = crawlctl.getattr("img", "src", producthtml);

            //        if (!string.IsNullOrEmpty(img))
            //        {

            //            carhtml += $"<div style=\"text-align: center;\"><img src=\"{img}\" ></div>";
            //        }
            //        var title = crawlctl.getLastbyclass("", "h2", producthtml);
            //        if (!string.IsNullOrEmpty(title))
            //        {
            //            carhtml += $"<h2>{title}</h2>";
            //        }
            //        var content = crawlctl.getbyclass("", "ul", producthtml);
            //        if (!string.IsNullOrEmpty(content))
            //        {
            //            content = content.Replace("https://g7auto.vn/", "/");
            //            carhtml += $"<ul>{content}</ul>";
            //        }
            //    }
            //    item.Description = carhtml;
            //    new CarModelBO().UpdateDynamic($"Set Description=N'{item.Description}' ", $"Id={item.Id}");
            //}




            //NLogLogger.DebugMessage(carhtml);
            //var lstcategory = new CategoryBO().GetAllCategoryFullsByPosition(UTILS.Constants.CategoryPosition.Footer, 18, false);
            //return RedirectToAction("Index", "Admin");
            //return View(lstcategory.Where(x => x.ParentId == 0).ToList());
            //var data = new CarSizeBO().GetTopLastestCarSize(8,-1, 1);
            //foreach (var item in data)
            //{
            //    item.Name = item.Name.TrimStart().TrimEnd();
            //    item.Url =  "lop-" + Utils.ConvertToRewriteLink(item.Name);
            //    new CarSizeBO().UpdateDynamic($"Set Name='{item.Name}',Url='{item.Url}' ", $"Id={item.Id}");

            //}
            //var ListCate = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.None).Where(x => x.Published == 1).ToList();
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

            ViewBag.Result = ServerProcess.test("en");

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
