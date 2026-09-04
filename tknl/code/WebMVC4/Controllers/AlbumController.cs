using BIZ;
using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Helper;
using  WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class AlbumController : BaseController
    {
        //
        // GET: /Album/

        public ActionResult Index(int CategoryId, string CateName, int Page = 1)
        {
            if (CategoryId <= 0)
            {
                ViewBag.Description = ConfigurationManager.AppSettings["DefMetaDescription"];
                ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
                ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            }
            else
            {
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                if (cateobj == null)
                    return RedirectToAction("Error", "Home");
                if (CateName != Utils.ConvertToRewriteLink(cateobj.Name))
                    return RedirectToAction("Index", "Album", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(cateobj.Name) });

                var culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLowerInvariant();
                if (culture != cateobj.Language)
                {

                    CultureHelper.SetCulture(HttpContext.Request.RequestContext, culture);
                    return RedirectToAction("Index", "Album", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(cateobj.Name) });
                }
                ViewBag.CurrentCategoryId = cateobj.Id;
                ViewBag.ParentCategoryId = cateobj.ParentId;
                var metaDescription = Utils.StripHtmlTag(cateobj.Description);
                var siteTitle = cateobj.Name + " | ";
                var metaKeyword = siteTitle.Replace(" | ", ",");
                ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
                ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
                ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
                if (Page > 1)
                {
                    ViewBag.Title = siteTitle + " Trang-" + Page.ToString() + " | " + ConfigurationManager.AppSettings["DefMetaSiteTitle"];
                    ViewBag.Description = metaDescription + " Trang-" + Page.ToString() + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
                }
            }

            var PageSize = Convert.ToByte(ConfigurationManager.AppSettings["MaxAlbumShow"]);
            int Total = 0;
            var albums = new AlbumBO().GetPageLastestAlbumsFull(CategoryId, Page, PageSize, ref Total);
            var Model = new AlbumModel { listdata = albums, pageIndex = Page, pageSize = PageSize, total = Total, CategoryId = CategoryId };
            return View(Model);
        }
        public ActionResult Detail(int Id, string Title)
        {
            var album = new AlbumBO().GetAlbum(Id);
            if (album == null || album.Status != 1)
                return RedirectToAction("Error", "Home");
            if (Title != Utils.ConvertToRewriteLink(album.Title))
                return RedirectToAction("Detail", "Album", new { Id = Id, Title = Utils.ConvertToRewriteLink(album.Title) });
            var culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLowerInvariant();
            if (culture != album.CateLiteObj.Language)
            {

                CultureHelper.SetCulture(HttpContext.Request.RequestContext, culture);
                return RedirectToAction("Detail", "Album", new { Id = Id, Title = Utils.ConvertToRewriteLink(album.Title) });
            }
            var metaDescription = album.Title + " , " + Utils.StripHtmlTag(album.Description);
            var siteTitle = album.Title + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];

            ViewBag.CurrentCategoryId = album.CategoryId;

            //var cateboj = new CategoryBO().GetCategoryFull(album.CategoryId.Value);
            //if (cateboj != null)
            ViewBag.ParentCategoryId = album.CateLiteObj.ParrentId;
            
            var lstimages = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AlbumImage>>(album.Images);
            var listimgful = new List<AlbumImage>();
            if (lstimages.Count > 0)
            {
                foreach (var albumImage in lstimages)
                {
                    var imgimtem = new AlbumImage
                    {

                        Text = albumImage.Text,
                        Name = Utils.GetImageUrl(album.Id, EntityName.Album, false) + albumImage.Name

                    };
                    listimgful.Add(imgimtem);

                }

            }
            ViewBag.ImgList = listimgful;
            return View(album);
        }
        public ActionResult Class(int CategoryId, string CateName)
        {
            if (CategoryId <= 0)
            {
                ViewBag.Description = ConfigurationManager.AppSettings["DefMetaDescription"];
                ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
                ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];
            }
            else
            {
                var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
                if (cateobj == null)
                    return RedirectToAction("Error", "Home");
                if (CateName != Utils.ConvertToRewriteLink(cateobj.Name))
                    return RedirectToAction("Class", "Album", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(cateobj.Name) });

                var metaDescription = Utils.StripHtmlTag(cateobj.Description);
                var siteTitle = cateobj.Name + " | ";
                var metaKeyword = siteTitle.Replace(" | ", ",");
                ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
                ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
                ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];

            }
            ViewBag.CategoryId = CategoryId;

            return View();
        }
        public ActionResult ClassDetail(int CategoryId, string CateName)
        {

            var cateobj = new CategoryBO().GetCategoryFull(CategoryId);
            if (cateobj == null)
                return RedirectToAction("Error", "Home");
           
            var metaDescription = Utils.StripHtmlTag(cateobj.Description) + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            var siteTitle = "Thông tin chi tiết lớp - " + cateobj.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];

           
            var album = new AlbumBO().GetTopLastestAlbumsFull(1, CategoryId).FirstOrDefault();
            var lstimages = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AlbumImage>>(album.Images);
            var listimgful = new List<AlbumImage>();
            if (lstimages.Count > 0)
            {
                foreach (var albumImage in lstimages)
                {
                    var imgimtem = new AlbumImage
                    {

                        Text = albumImage.Text,
                        Name = Utils.GetImageUrl(album.Id, EntityName.Album, false) + albumImage.Name

                    };
                    listimgful.Add(imgimtem);

                }

            }
            var relatealbum = new AlbumBO().GetTopAlbumByIdsFulls(album.Param, 0, true);
            var model = new ClassDetailModel { album = album, listimgful = listimgful, relatealbum = relatealbum, CategoryId = CategoryId };
            return View(model);
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
