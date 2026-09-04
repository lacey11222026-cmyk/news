using BIZ;
using DATA.ContentDB;
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
            return View(intro);
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
            data.ResponedUser = "";
            data.Question = Utils.RemoveSqlInjection(data.Question);
            var result = FeedbackDAL.InsertUpdate(data);
            if (result > 0)
            {
                string mailsubject = "Thông báo phản hồi";
                string mailform = System.IO.File.ReadAllText(Server.MapPath("/Content/MailFormat/mailfeedback.html"));
                string mailbody = String.Format(mailform, data.Name, data.Email,  data.Question);



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
