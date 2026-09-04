using BIZ;
using DATA.ContentDB;
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
    public class EnergyController : Controller
    {
        //
        // GET: /Energy/

        public ActionResult Index(string keyword, string province, int Page = 1)
        {
            ViewBag.Description = Utils.StripHtmlTag(Resources.Global.SiteDescription);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = "Kiểm toán viên năng lượng";
            var PageSize = 12;

            int Total = 0;
            var data = AuditorDAL.GetSearch(-1, keyword, Page, PageSize, ref Total, 2, province);
            ViewBag.keyword = keyword;
            ViewBag.province = province;
            var Model = new AuditorModel { listdata = data, pageIndex = Page, pageSize = PageSize, total = Total };
            ViewBag.listLocation = new TestLocationBO().GetAllCache().Where(x => x.Name != "All").ToList();
            return View(Model);
        }
        public ActionResult Detail(int Id)
        {
            var project = AuditorDAL.GetDetail(Id);
            if (project == null)
                return RedirectToAction("Error", "Home");



            var metaDescription = project.FullName;

            //var metaKeyword = siteTitle.Replace(" | ", ",");
            ViewBag.Description = metaDescription;
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = project.FullName;
            var obj = new AuditorFull
            {

            };

            obj.Id = project.Id;
            obj.FullName = project.FullName;
            obj.Title = project.Title;
            obj.No = project.No;
            obj.Type = project.Type;
            obj.BirthDay = project.BirthDay;
            obj.Passport = project.Passport;
            obj.Nation = project.Nation;
            obj.Organ = project.Organ;
            obj.Order = project.Order;
            obj.Level = project.Level;
            obj.Organ = project.Organ;
            obj.MSDN = project.MSDN;
            obj.Role = project.Role;
            obj.Address = project.Address;
            obj.Mobile = project.Mobile;
            obj.Email = project.Email;
            obj.Group = project.Group;
            obj.Config = project.Config;
            obj.Status = project.Status;
            obj.Images = project.Images;
            obj.Province = project.Province;
            obj.Cate = 1;
            obj.ProjectConfig = JsonConvert.DeserializeObject<AuditorConfig>(obj.Config);
            if (string.IsNullOrEmpty(obj.ProjectConfig.MobileOffice))
            {
                obj.ProjectConfig.MobileOffice = " ";
            }
            if (string.IsNullOrEmpty(obj.ProjectConfig.TrainingTime))
            {
                obj.ProjectConfig.TrainingTime = " ";
            }
            if (string.IsNullOrEmpty(obj.ProjectConfig.IssueDate))
            {
                obj.ProjectConfig.IssueDate = " ";
            }
            if (string.IsNullOrEmpty(obj.ProjectConfig.ExpirationDate))
            {
                obj.ProjectConfig.ExpirationDate = " ";
            }


            //var cateboj = new CategoryBO().GetCategoryFull(newsobj.CategoryId.Value);
            //if (cateboj != null)
            //    ViewBag.ParentCategoryId = cateboj.ParentId;
            //ViewBag.ParentCategoryId = newsobj.CateLiteObj.ParrentId;


            return View(obj);
        }

    }
}
