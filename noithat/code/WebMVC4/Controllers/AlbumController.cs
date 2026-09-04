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
using WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class AlbumController : Controller
    {
        //
        // GET: /Album/

        public ActionResult Index( int Page = 1)
        {

            ViewBag.Description = ConfigurationManager.AppSettings["DefMetaDescription"];
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            

            var PageSize = 12;
            int Total = 0;
            var albums = new AlbumBO().GetPageLastestAlbumsFull(-1, Page, PageSize, ref Total);
            var Model = new AlbumModel { listdata = albums, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = -1 };
            return View(Model);
        }
        public ActionResult Detail(int Id, string Title)
        {

            ViewBag.PageClass = "list";
            var album = new AlbumBO().GetAlbum(Id);
            if (album == null || album.Status != 1)
                return RedirectToAction("Error", "Home");
            if (Title != Utils.ConvertToRewriteLink(album.Title))
                return RedirectToAction("Detail", "Album", new { Id = Id, Title = Utils.ConvertToRewriteLink(album.Title) });
            var metaDescription = album.Title + " , " + Utils.StripHtmlTag(album.Description);
            var siteTitle = album.Title + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];


            try
            {
                album.ImageParam = JsonConvert.DeserializeObject<ImageParam>(album.Images);
            }
            catch
            {

                album.ImageParam = new ImageParam();
            }
            if (album.ImageParam == null)
            {
                album.ImageParam = new ImageParam();
            }

            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            ViewBag.Height = 500;
            if (ViewBag.IsMobile)
                ViewBag.Height = 300;
            return View(album);
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult RelateProduct(string ids)
        {
            try
            {
                var lstdata = new ProductBO().GetTopProductByIdsFulls(ids,0,true);
             
                return PartialView(lstdata);
            }
            catch (Exception)
            {

                return PartialView(null);
            }
        }
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        [ChildActionOnly]
        public ActionResult Breadcrumb(int CategoryId)
        {
            var lstCategorieFulls = new CategoryBO().GetAllCategoriesByPathway(CategoryId);
            int index = 0;
            string html = string.Empty;
            foreach (var categoryFull in lstCategorieFulls)
            {
                var link = "";
                if (categoryFull.Name.ToLower().Equals("danh sách lớp") || categoryFull.Name.ToLower().Equals("lớp học của bé"))
                {

                    link = "/lop-hoc/c" + categoryFull.Id + "/" + Utils.ConvertToRewriteLink(categoryFull.Name) + ".html";

                }

                else
                {

                    link = "/chi-tiet-lop/t" + categoryFull.Id + ".html";

                }
                if (index == 0 || index == lstCategorieFulls.Count - 1)
                    html += "<a href=\"" + link + "\">" + Utils.ToUpperFirstChar(categoryFull.Name) + "</a> >";
                index++;
            }

            ViewBag.Data= html.Substring(0, html.Length - 2);
            return PartialView();
        }
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
            ViewBag.Url = UTILS.Utils.FormatUrlRewriteByType(CategoryId, CateName, (int)UTILS.Constants.CategoryType.Album);

            ViewBag.CateName = CateName;
            var Albums = new AlbumBO().GetTopLastestAlbumsFull(MaxLastestNews, CategoryId);
            return PartialView(Albums);
            
        }

    }
}
