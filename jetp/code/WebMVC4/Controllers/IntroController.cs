using BIZ;
using BIZ.Entity;
using DATA.ContentDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Filter;
using WebMVC4.Helper;

namespace WebMVC4.Controllers
{
    public class IntroController : Controller
    {
        //
        // GET: /Intro/
        [LocalizationActionFilter]
        public ActionResult Index(int CategoryId, string CateName)
        {
            if (CategoryId == 3)
                CategoryId = 67;
            if (CategoryId == 39)
                CategoryId = 90;

            var intro = new CategoryBO().GetCategoryFull(CategoryId);
            if (intro == null)
                return RedirectToAction("Error", "Home");
            if (CateName != Utils.ConvertToRewriteLink(intro.Name))
                return RedirectToAction("Index", "Intro", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(intro.Name) });

            if (WorkContext.GetLanguage() != intro.Language)
            {
                WorkContext.SetLanguage(intro.Language);
                return RedirectToAction("Index", "Intro", new { CategoryId = CategoryId, CateName = Utils.ConvertToRewriteLink(intro.Name) });
            }
            var metaDescription = Utils.StripHtmlTag(intro.Description);
            var siteTitle = intro.Name + " | ";
            var metaKeyword = siteTitle.Replace(" | ", ",");

            ViewBag.Description = metaDescription;
            ViewBag.Keywords = metaKeyword + ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = siteTitle;
            try
            {
                intro.Param = JsonConvert.DeserializeObject<CategoryParam>(intro.Params);
            }
            catch
            {

                intro.Param = new CategoryParam();
            }
            if (string.IsNullOrEmpty(intro.Param.Image))
                intro.Param.Image = "/Content/images/bg-intro-1.png";
            if (CategoryId == 89 || CategoryId == 68)

            {
                return View("Index2", intro);
            }
            if (CategoryId == 2 || CategoryId == 76)

            {
                return View("Index3", intro);
            }
            return View(intro);
        }
        [LocalizationActionFilter]
        public ActionResult Contact()
        {
            var lang = WorkContext.GetLanguage();
            int cateId = 42;

            if (lang != "vi-vn")
                cateId = 84;
            var cateobj = new CategoryBO().GetCategoryFull(cateId);

            try
            {
                cateobj.Param = JsonConvert.DeserializeObject<CategoryParam>(cateobj.Params);
            }
            catch
            {

                cateobj.Param = new CategoryParam();
            }
            ViewBag.Description = cateobj.Name +" - "+Resources.Global.SiteTitle;
            ViewBag.Keywords = Resources.Global.SiteTitle;
            ViewBag.Title = cateobj.Name + " - " + Resources.Global.SiteTitle;
            ViewBag.CateName = cateobj.Name;
            ViewBag.CateDescription = cateobj.Description;
            ViewBag.CateImg = cateobj.Param.Image;
            ViewBag.FirstName = "";
            ViewBag.LastName = "";
            ViewBag.Email = "";
            var userinfo = (UserSession)Session[SessionsManager.SESSION_USER];
            if (userinfo != null)
            {
                ViewBag.FirstName = userinfo.FirstName;
                ViewBag.LastName = userinfo.LastName;
                ViewBag.Email = userinfo.Email;
            }
            return View(cateobj);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Feedback(Feedback data)
        {
            if (Session["sendfeedback"] == null)
            {
                Session["sendfeedback"] = "1";
            }

            var countsession = Convert.ToInt32(Session["sendfeedback"].ToString());
            if (countsession > 5)
            {
                return Json(-4);
            }
            data.Status = 0;
            data.Answer = "";
            data.Mobile = "";
            //data.ResponedUser = "";
            data.Question = Utils.RemoveSqlInjection(data.Question);
            var result = FeedbackDAL.InsertUpdate(data);
            if (result > 0)
            {
                //string mailsubject = "Thông báo phản hồi";
                //string mailform = System.IO.File.ReadAllText(Server.MapPath("/Content/MailFormat/mailfeedback.html"));
                //string mailbody = String.Format(mailform, data.Name, data.Email, data.Question);



                //Action<string, string, string> send = (string subject, string body, string email) =>
                //{
                //    EmailService.SendMail(subject, body, email);

                //};
                //send.BeginInvoke(mailsubject, mailbody, ConfigurationManager.AppSettings["WebsiteEmail"].ToString(), null, null);
            }
            Session["sendfeedback"] = (countsession + 1).ToString();
            return Json(result > 0 ? 1 : result);

        }
    }
}
