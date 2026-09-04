using BIZ;
using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Models;
using static System.Net.WebRequestMethods;

namespace WebMVC4.Controllers
{
    public class AlbumController : Controller
    {
        //
        // GET: /Album/

        public ActionResult Index(int CategoryId, string CateName, int Page = 1)
        {

            ViewBag.PageClass = "list";
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

            ViewBag.CurrentCategoryId = album.CategoryId;

            var cateobj = new CategoryBO().GetCategoryFull(album.CategoryId.GetValueOrDefault());
            if (cateobj == null)
                return RedirectToAction("Error", "Home");
            ViewBag.ParentCategoryId = cateobj.ParentId;
            var lstimages = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AlbumImage>>(album.Images);
            var listimgful = new List<AlbumImage>();
            if (lstimages.Count > 0)
            {
                foreach (var albumImage in lstimages)
                {
                    var imgimtem = new AlbumImage
                    {

                        Text = albumImage.Text,
                        Name = "http://cms.vecea.vn/" +Utils.GetImageUrl(album.Id, EntityName.Album, false) + albumImage.Name

                    };
                    listimgful.Add(imgimtem);

                }

            }
            album.AlbumImage = listimgful;
            //ViewBag.ImgList = listimgful;
            return View(album);
        }
        
      
        public ActionResult LastestNews(int albumId )
        {
            
            var model = new AlbumBO().GetTopLastestAlbumsFull(6, 11);
            if (model.Count > 0)
            {
                model = model.Where(x => x.Id != albumId).ToList();
            }
            return PartialView(model);
            
        }

    }
}
