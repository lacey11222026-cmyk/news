using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BIZ;
using BIZ.Entity;
using Newtonsoft.Json;
using UTILS;
using WebMVC4.Areas.Image2016.Models;

namespace WebMVC4.Areas.Image2016.Controllers
{
    public class HomeAlbumController : Controller
    {
        //
        // GET: /Image2016/Home/

        public ActionResult Index(string keyword, string fromdate, string todate, int status, int type = -1, int page = 1)
        {
            ViewBag.keyword = keyword;
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            ViewBag.status = status;
            ViewBag.page = page;
            ViewBag.type = type;
            //var lstcontent = new ContentBO().GetTopLastestContentFulls(2, 139);
            return View();
        }
        public ActionResult Intro(int CategoryId, string CateName)
        {
            var intro = new CategoryBO().GetCategoryFull(CategoryId);
            if (intro == null)
                return RedirectToAction("Error", "Home");


            var metaDescription = Utils.StripHtmlTag(intro.Description);
            var siteTitle = intro.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.CurrentCategoryId = intro.Id;
            ViewBag.ParentCategoryId = intro.ParentId;




            ViewBag.Description = metaDescription + " " + Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle + ConfigurationManager.AppSettings["DefMetaSiteTitle"];

            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsIpad = mobileHelper.DetectIpad();
            ViewBag.Iphone = mobileHelper.DetectIphone();
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            return View(intro);
        }
        public ActionResult LoadAllBum(int page, string keyword, int type, int status)
        {
            var pagesize = 40;
            ViewBag.page = page;
            int total = 0;
            //var lst = new AlbumImageBO().GetAlbumsFuLLPaged(keyword, -1, status, -1, page, pagesize, ref total, "", "", "NEWID()");

            var lst = new AlbumImageBO().GetAlbumsFuLLPaged(keyword, -1, status, -1, page, pagesize, ref total, "", "", "Id ASC");
            foreach (var item in lst)
            {
                try
                {
                    item.Album = JsonConvert.DeserializeObject<List<AlbumImageInfo>>(item.Description);
                }
                catch
                {
                    item.Album = new List<AlbumImageInfo>();
                }
            }
            var Model = new AlbumImageModel { listdata = lst, pageIndex = page, pageSize = pagesize, total = total, CategoryId = -1 };
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.status = status;
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            ViewBag.Height = 500;
            if (ViewBag.IsMobile)
                ViewBag.Height = 400;

            return PartialView(Model);
        }
        [HttpPost]
        public ActionResult VoteAlbum(long id, int point)
        {
            string results = "0";
            try
            {
                if (Session["Game" + id] == null)
                {
                    Session["Game" + id] = "1";
                }

                var countsession = Convert.ToInt32(Session["Game" + id].ToString());
                if (countsession > 20)
                {
                    return Json("-4");
                }
                Session["Game" + id] = (countsession + 1).ToString();

                var resultf = new AlbumImageBO().Vote(id, point);
                results = "1";
            }
            catch
            {

                results = "0";
            }
            return Json(results);

        }
        [HttpPost]
        public ActionResult VoteAlbum2(long id, int point)
        {
            string results = "0";
            try
            {
                if (Session["Game" + id] == null)
                {
                    Session["Game" + id] = "1";
                }

                var countsession = Convert.ToInt32(Session["Game" + id].ToString());
                if (countsession > 20)
                {
                    return Json("-4");
                }
                Session["Game" + id] = (countsession + 1).ToString();

                var resultf = new AlbumImageBO().Vote2(id, point);
                results = "1";
            }
            catch
            {

                results = "0";
            }
            return Json(results);

        }
    }
}
