using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BIZ;
using DATA;
using UTILS;
using  WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class VoteController : Controller
    {

        
        public ActionResult HomeVote()
        {
            var request = System.Web.HttpContext.Current.Request;
            var mobileHelper = new MobileDetectHelper(request);
            ViewBag.IsMobile = mobileHelper.DetectMobileLong();
            //var key = "HotSurvey";
            var surveyObj = new Survey();
            var listdata = new SurveyBO().GetAllSurveys(10, 1, -1, "");
            foreach (var item in listdata)
            {
                if (DateTime.Compare(DateTime.Now, item.BeginDate) >= 0 && DateTime.Compare(DateTime.Now, item.EndDate.AddDays(1).AddMinutes(-1)) <= 0)
                {
                    surveyObj = item;
                    break;
                    ;
                }
            }
            if (surveyObj.Id <= 0)
                return PartialView(null);
            var lstdata = new SurveyItemBO().GetSurveyItemsBy(surveyObj.Id, 1);
            var model = new SurveyItemModel { listdata = lstdata, obj = surveyObj };
            return PartialView(model);
        }
       
        public ActionResult View(int Id)
        {
            var surveyObj = new SurveyBO().GetSurvey(Id);
            var lstdata = new SurveyItemBO().GetSurveyItemsBy(surveyObj.Id, 1);

            var cate = string.Empty;
            var data = string.Empty;
            var totalVote = lstdata.Sum(x => x.Count);
            cate += string.Format("{0}", string.Join(",", lstdata.Select(x => string.Format("'{0} ({1}%)'", x.Content, Math.Round((decimal)(x.Count * 100 / totalVote), 0)))));
            data += string.Format("{0}", string.Join(",", lstdata.Select(x => x.Count)));

            var model = new SurveyItemModel { listdata = lstdata, obj = surveyObj, data = data,cate= cate };
            ViewBag.Title = "Kết quả bình chọn ";
            ViewBag.Description = Utils.StripHtmlTag(ConfigurationManager.AppSettings["DefMetaDescription"]);
            ViewBag.Keywords = ConfigurationManager.AppSettings["DefMetaKeyword"];
            
            return View(model);
        }
        [HttpPost]
        public ActionResult SubmitQuestion(string lstAnswer)
        {
            if (Session["Vote"] == null)
            {
                Session["Vote"] = "1";
            }
            var countsession = Convert.ToInt32(Session["Vote"].ToString());
            Session["Vote"] = (countsession + 1).ToString();
            if (countsession <= 20)
            {
                int surveyItem = 0;
                var lstitem = lstAnswer.Split('|');
                foreach (var item in lstitem)
                {
                    int.TryParse(item, out surveyItem);
                    if (surveyItem > 0)
                    {
                        new SurveyItemBO().CountAdd(surveyItem);
                    }
                }
            }

            return Json("4");

        }
    }
}
