using BIZ;
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
    public class NhaMayController : Controller
    {
        //
        // GET: /NhaMay/

   
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult TopNhaMays(int Top = 0)
        {
            var data = new NhaMayBO().GetTopLastestNhaMaysFull(Top);

            return PartialView(data);
        }
        public ActionResult Index(int Page = 1, int loai = -1,int hinhthuc=-1,int status=-1,string fromdate="",string todate="", string keyword = "")
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = ConfigurationManager.AppSettings["DefMetaSiteTitle"];


            var PageSize = 15;
            int Total = 0;
            var data = new NhaMayBO().GetNhaMaysFuLLPaged(keyword, Page, PageSize, ref Total,loai,hinhthuc,status, fromdate, todate);
            ViewBag.keyword = keyword;
            var Model = new NhaMayModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total,loai=loai,hinhthuc=hinhthuc,status=status,fromdate=fromdate,todate=todate };
            return View(Model);
        }
        public ActionResult Detail(int Id,string Title)
        {

            ViewBag.PageClass = "detail";
            var newsobj = new NhaMayBO().GetNhaMay(Id);
            if (newsobj == null)
                return RedirectToAction("Error", "Home");

            if (Title != Utils.ConvertToRewriteLink(newsobj.TenNhaMay))
                return RedirectToAction("Detail", "NhaMay", new { Id = Id, Title = Utils.ConvertToRewriteLink(newsobj.TenNhaMay) });
            
            var metaDescription = newsobj.TenNhaMay;
            var siteTitle = newsobj.TenNhaMay + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = newsobj.TenNhaMay;


            return View(newsobj);
        }
       
    }
}
