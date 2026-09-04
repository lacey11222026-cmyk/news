using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BIZ;
using DATA;
using UTILS;
using  WebEN.Models;

namespace WebEN.Controllers
{
    public class VoteController : Controller
    {

        [ChildActionOnly]
        public ActionResult HomeVote()
        {

            var key = "HotSurvey";
            var surveyObj = new Survey();
            var configValue = new SystemConfigBO().GetByKey(key);
            if (configValue != null)
            {
                var listdata = new SurveyBO().GetSurveyByIds(configValue.ConfigValue, 30, true);
                foreach (var item in listdata)
                {
                    if (DateTime.Compare(DateTime.Now, item.BeginDate) >= 0 && DateTime.Compare(DateTime.Now, item.EndDate.AddDays(1).AddMinutes(-1)) <= 0)
                    {
                        surveyObj = item;
                        break;
                        ;
                    }
                }

            }
            if (surveyObj.Id <= 0)
                return PartialView(null);
            var lstdata = new SurveyItemBO().GetSurveyItemsBy(surveyObj.Id, 1);
            var model = new SurveyItemModel { listdata = lstdata, obj = surveyObj };
            return PartialView(model);
        }
        [ChildActionOnly]
        public ActionResult HomeVoteMobile()
        {

            var key = "HotSurvey";
            var surveyObj = new Survey();
            var configValue = new SystemConfigBO().GetByKey(key);
            if (configValue != null)
            {
                var listdata = new SurveyBO().GetSurveyByIds(configValue.ConfigValue, 30, true);
                foreach (var item in listdata)
                {
                    if (DateTime.Compare(DateTime.Now, item.BeginDate) >= 0 && DateTime.Compare(DateTime.Now, item.EndDate.AddDays(1).AddMinutes(-1)) <= 0)
                    {
                        surveyObj = item;
                        break;
                        ;
                    }
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
            //var data = " ['Bình chọn', '"+surveyObj.Title+"'],";
            //foreach (var surveyItem in lstdata.Select((value, i) => new { i, value }))
            //{
            //    if(surveyItem.i==lstdata.Count()-1)
            //    {
            //        data += " ['" + string.Format("{0} ({1})", surveyItem.value.Content, surveyItem.value.Count) + "', " + surveyItem.value.Count + "]";

            //    }
            //    else
            //    {
            //        data += " ['" + string.Format("{0} ({1})", surveyItem.value.Content, surveyItem.value.Count) + "'," + surveyItem.value.Count + "],";

            //    }
            //}

            var data = string.Empty;
            var totalVote = lstdata.Sum(x => x.Count);
            data += string.Format("['Tiêu chí', {0}],", string.Join(",", lstdata.Select(x => string.Format("'{0} ({1}%)'", x.Content, Math.Round((decimal)(x.Count * 100 / totalVote), 0)))));
            data += string.Format("['Số phiếu', {0}]", string.Join(",", lstdata.Select(x => x.Count)));

            var model = new SurveyItemModel { listdata = lstdata, obj = surveyObj, data = data };
            ViewBag.Title = "Kết quả bình chọn |" + surveyObj.Content;
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
