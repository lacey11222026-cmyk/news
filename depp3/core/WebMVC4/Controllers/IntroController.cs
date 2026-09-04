using BIZ;
using BIZ.Entity;
using DATA;
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
            var siteTitle = intro.Name ;
            // var metaKeyword = siteTitle.Replace(" | ", ",");
            try
            {
                intro.Param = JsonConvert.DeserializeObject<CategoryParam>(intro.Params);
            }
            catch
            {

                intro.Param = new CategoryParam();
            }
            if (!string.IsNullOrEmpty(intro.Param.MetaTitle))
            {
                siteTitle = intro.Param.MetaTitle;
            }

            if (!string.IsNullOrEmpty(intro.Param.MetaDesciption))
            {
                metaDescription = intro.Param.MetaDesciption;
            }
            ViewBag.Description = metaDescription;
            //ViewBag.Keywords = metaKeyword;
            ViewBag.Title = siteTitle ;
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
            data.Answer = Utils.RemoveSqlInjection(data.Answer);
            data.Mobile = "";
            data.ResponedUser = "";
            data.Question = Utils.RemoveSqlInjection(data.Question);
            var result = FeedbackDAL.InsertUpdate(data);
           
            Session["sendfeedback"] = (countsession + 1).ToString();
            return Json(result > 0 ? 1 : result);

        }

    }
}
