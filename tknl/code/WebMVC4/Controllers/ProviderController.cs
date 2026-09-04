using BIZ;

using DATA.DAL;
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
    public class ProviderController : Controller
    {
        //
        // GET: /Provider/

        public ActionResult Index(string keyword, string province,string mst, int Page = 1)
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
           
            ViewBag.Title = "Tổ chức kiểm toán năng lượng";
            var PageSize = 20;

            int Total = 0;
            var data = ProviderDAL.GetSearch(-1, keyword, Page, PageSize, ref Total, province,mst);
            ViewBag.mst = mst;
            ViewBag.keyword = keyword;
            ViewBag.province = province;
            var Model = new ProviderModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total };
            ViewBag.listLocation = new TestLocationBO().GetAllCache().Where(x => x.Name != "All").ToList();
            return View(Model);
        }
        public ActionResult Detail(int Id)
        {
            var newsobj = ProviderDAL.GetDetail(Id);
            if (newsobj == null )
                return RedirectToAction("Error", "Home");

            var metaDescription = newsobj.Name;

            //var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = newsobj.Name;

         

            //var cateboj = new CategoryBO().GetCategoryFull(newsobj.CategoryId.Value);
            //if (cateboj != null)
            //    ViewBag.ParentCategoryId = cateboj.ParentId;
            //ViewBag.ParentCategoryId = newsobj.CateLiteObj.ParrentId;

           
            return View(newsobj);
        }
        public ActionResult ViewFile(int Id)
        {
            var newsobj = ProviderDAL.GetDetail(Id);
            if (newsobj == null)
                return RedirectToAction("Error", "Home");

            var metaDescription = newsobj.Name;

            //var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = newsobj.Name;
            if (newsobj.Images.Contains("http"))
            {
                ViewBag.url = $"https://docs.google.com/gview?url={newsobj.Images}&embedded=true";
            }
            else
            {
                ViewBag.url = $"https://docs.google.com/gview?url=https://{Request.Url.Host}{newsobj.Images}&embedded=true";
            }

            
            return View();
        }
        public ActionResult DownloadFile(int Id)
        {
            var newsobj = ProviderDAL.GetDetail(Id);
           

         
            return Redirect(newsobj.Images);
        }

    }
}
