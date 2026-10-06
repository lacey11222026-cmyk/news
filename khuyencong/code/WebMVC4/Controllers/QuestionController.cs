using BIZ;
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
    public class QuestionController : Controller
    {
        //
        // GET: /Question/

        public ActionResult Index(int Page = 1,int Id=0)
        {

            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            ViewBag.Title = "HỎI ĐÁP VỀ THÔNG TƯ 21";
            ViewBag.Id = Id;
            var PageSize = 30;
            int Total = 0;
            var albums = new QABO().GetQAsPaged(Page, PageSize, ref Total, 1,"");
            var Model = new QAModel { listdata = albums, pageIndex = Page, pageSize = PageSize, total = Total, };
            return View(Model);
        }
        public ActionResult TopQuestion()
        {

          
            var PageSize = 8;
            int Total = 0;
            var albums = new QABO().GetQAsPaged(1, PageSize, ref Total, 1, "");
 
            return PartialView(albums);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Send(DATA.QA data)
        {
            if (Session["sendfeedback"] == null)
            {
                Session["sendfeedback"] = "1";
            }

            var countsession = Convert.ToInt32(Session["sendfeedback"].ToString());
            if (countsession > 10)
            {
                return Json(-4);
            }
            data.Status = 0;
            var result = new QABO().CreateUpdateQA(data);

            Session["sendfeedback"] = (countsession + 1).ToString();
            return Json(result > 0 ? 1 : result);

        }

    }
   
}
