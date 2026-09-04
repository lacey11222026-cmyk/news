using BIZ;
using BIZ.Entity;
using DATA.ContentDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using UTILS;
using WebMVC4.Filter;
using WebMVC4.Helper;
using WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class ProjectController : Controller
    {
        //
        [LocalizationActionFilter]
        public ActionResult Index(int Page = 1, string keyword = "", int systemtype = -1, int type = -1)
        {
            ViewBag.Description = Resources.Global.SiteDescription;
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = Resources.Global.SiteTitle;

            var lang = WorkContext.GetLanguage();
            int cateId = 66;
            if (lang != "vi-vn")
                cateId = 79;
            var cateobj = new CategoryBO().GetCategoryFull(cateId);

            try
            {
                cateobj.Param = JsonConvert.DeserializeObject<CategoryParam>(cateobj.Params);
            }
            catch
            {

                cateobj.Param = new CategoryParam();
            }
            ViewBag.CateName = cateobj.Name;
            ViewBag.CateDescription = cateobj.Description;
            ViewBag.CateImg = cateobj.Param.Image;

            var PageSize = 16;
            int Total = 0;
            var data = Project2DAL.GetSearch(1, systemtype, lang, keyword, Page, PageSize, ref Total, "", "", type);
            var Model = new ProjectModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total };
            ViewBag.keyword = keyword;
            ViewBag.status = systemtype;
            ViewBag.type = type;
            return View(Model);
        }
        [LocalizationActionFilter]
        public ActionResult Detail(int Id)
        {
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];

            var obj = Project2DAL.GetDetail(Id);
            var projectconfig = JsonConvert.DeserializeObject<UserProjectConfig>(obj.Config);

            var listLocation = new TestLocationBO().GetAllCache();


            if (listLocation.Exists(x => x.Id.ToString() == obj.Location))
            {
                obj.Location = listLocation.FirstOrDefault(x => x.Id.ToString() == obj.Location).Name;

            }
            else
            {
                if (obj.Username == "vi-vn")
                {
                    if (obj.Location == "999")
                        obj.Location = "Toàn quốc";
                    if (obj.Location == "998")
                        obj.Location = "Miền Bắc";
                    if (obj.Location == "997")
                        obj.Location = "Miền Trung";
                    if (obj.Location == "996")
                        obj.Location = "Miền Nam";

                }
                else
                {
                    if (obj.Location == "999")
                        obj.Location = "All";

                    if (obj.Location == "998")
                        obj.Location = "North Region";
                    if (obj.Location == "997")
                        obj.Location = "Central Region";
                    if (obj.Location == "996")
                        obj.Location = "Southern Region";
                }

            }


            ViewBag.Title = obj.Name;
            var data = new Project2FullV2();
            data.Project = obj;
            data.ProjectConfig = projectconfig;

            if (obj.Type == 2)
            {

                return View("Detail2", data);
            }
            return View(data);
        }
        //[OutputCache(Duration = 60, VaryByParam = "*", VaryByCustom = "browser")]
        public ActionResult TopProject(int Top = 0)
        {
            var data = new ProjectBO().GetTopProject(6, 1);

            return PartialView(data);
        }

        public ActionResult RelateProject(string lang, int Id = 0)
        {
            var data = Project2DAL.TopProject(4, 1, "", lang);
            data = data.Where(x => x.Id != Id).Take(3).ToList();

            return PartialView(data);
        }
    }
}
