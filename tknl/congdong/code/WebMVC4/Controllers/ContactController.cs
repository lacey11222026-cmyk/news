using BIZ;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Filter;
using WebMVC4.Helper;
using WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/
        [LocalizationActionFilter]
        public ActionResult Index(int? type, int Page = 1, string keyword = "")
        {
            var PageSize = 20;
            int Total = 0;
            ViewBag.keyword = keyword;
            ViewBag.CityList = new CityBO().GetTopCity(0, 1, -1);
            var albums = new ExpertBO().GetExpertsPaged(keyword, Page, PageSize, ref Total, 1, type.GetValueOrDefault(), WorkContext.GetLanguage());
            var Model = new ExpertModel { listdata = albums, pageIndex = Page, pageSize = PageSize, total = Total, };
            Model.CategoryId = type.GetValueOrDefault();
            return View(Model);
        }
        [LocalizationActionFilter]
        public ActionResult Organ(int? type, int Page = 1, string keyword = "")
        {
            var PageSize = 20;
            int Total = 0;
            ViewBag.keyword = keyword;
            ViewBag.CityList = new CityBO().GetTopCity(0, 1, -1);
            var albums = new OrganBO().GetOrgansPaged(keyword, Page, PageSize, ref Total, 1, type.GetValueOrDefault(), WorkContext.GetLanguage());
            var Model = new OrganModel { listdata = albums, pageIndex = Page, pageSize = PageSize, total = Total, };
            Model.CategoryId = type.GetValueOrDefault();
            return View(Model);
        }
        // GET: /Contact/
        [LocalizationActionFilter]
        public ActionResult Detail(int Id)
        {
            var model = new ExpertBO().GetExpert(Id);
            return View(model);
        }
        [LocalizationActionFilter]
        public ActionResult OrganDetail(int Id)
        {
            var model = new OrganBO().GetOrgan(Id);
            return View(model);
        }
    }
}
