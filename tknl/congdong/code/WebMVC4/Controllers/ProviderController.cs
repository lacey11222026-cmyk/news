using BIZ;
using DATA.ContentDB;
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

        public ActionResult Index(string keyword, string province, int Page = 1)
        {
            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = "Đơn vị cung cấp dịch vụ, đơn vị tư vấn";
            var PageSize = 12;

            int Total = 0;
            var data = ProviderDAL.GetSearch(-1, keyword, Page, PageSize, ref Total, province);
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
    }
}
