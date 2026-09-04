using BIZ;
using DATA;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Filter;

namespace WebMVC4.Controllers
{
    public class ContactController : Controller
    {
        [LocalizationActionFilter]
        public ActionResult Index()
        {
            ViewBag.BodyClass = "wrap product-page";
            ViewBag.SiteTitle = "Liên hệ -" + Resources.Global.SiteTitle;
            return View();
        }
        [LocalizationActionFilter]
        public ActionResult LoadMapping(string type)
        {
            
            return PartialView("LoadMapping",type);
        }
        [HttpPost]
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
            var result = new FeedbackBO().CreateUpdateFeedback(data);
            if (result > 0)
            {
                string mailsubject = "Thông báo phản hồi";


                string mailform = System.IO.File.ReadAllText(Server.MapPath("/Content/MailFormat/mailfeedback.html"));
                string mailbody = String.Format(mailform, data.Name, data.Email,data.Mobile, data.Question);



                Action<string, string, string> send = (string subject, string body, string email) =>
                {
                    EmailService.SendMail(subject, body, email);

                };
                send.BeginInvoke(mailsubject, mailbody, ConfigurationManager.AppSettings["WebsiteEmail"].ToString(), null, null);
            }
            Session["sendfeedback"] = (countsession + 1).ToString();
            return Json(result > 0 ? 1 : result);

        }
    }
}
