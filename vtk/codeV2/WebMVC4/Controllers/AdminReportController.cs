using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Constants = UTILS.Constants;
using BIZ;
using BIZ.Entity;
using DATA;

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,NewsPublish")]
    public class AdminReportController : Controller
    {
        //
        // GET: /AdminReport/

        public ActionResult Index(string Year)
        {
            if (string.IsNullOrEmpty(Year))
                Year = DateTime.Now.Year.ToString();
            ViewBag.Year = Year;
            var datagrib = string.Empty;
            var datatb1 = string.Empty;
            var datatb2 = string.Empty;
            filldata(Year, ref datagrib, ref datatb1, ref datatb2);
            ViewBag.datagrib = datagrib;
            ViewBag.datatb1 = datatb1;
            ViewBag.datatb2 = datatb2;
            return View();
        }
        public ActionResult TopContent()
        {


            var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var toDate = DateTime.Now;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;

            return View();
        }
        public ActionResult ListContent(int? top, string fromDate, string endDate)
        {

            var data = new List<LogView>();

            int Top = top == null ? 20 : (int)top;

            data = new ContentBO().GetTopViewsContent(Top, fromDate, endDate);

            var lstId = "";
            foreach (var item in data)
            {
                lstId += item.Id + ",";
            }
            var lstNews = new ContentBO().GetTopContentByIdsFulls(lstId, Top, false);
            foreach (var item in data)
            {
                item.Name = lstNews.FirstOrDefault(x => x.Id == item.Id).Title;
            }
            return PartialView(data);
        }
        public ActionResult TopCate()
        {


            var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var toDate = DateTime.Now;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;

            return View();
        }
        public ActionResult ListCate(int? top, string fromDate, string endDate)
        {

            var data = new List<LogView>();

            int Top = top == null ? 50 : (int)top;

            data = new ContentBO().GetTopViewsCate(Top, fromDate, endDate);


            var lstNews = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.None);
            foreach (var item in data)
            {
                if (lstNews.Exists(x => x.Id == item.Id))
                    item.Name = lstNews.FirstOrDefault(x => x.Id == item.Id).Name;
                else
                    item.Name = item.Id.ToString();
            }
            return PartialView(data);
        }
        private void filldata(string Year, ref string returndata, ref string tb1, ref string tb2)
        {

            var lstmonth = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var lstcate = new CategoryBO().GetAllCategories(2);
            lstcate.Add(new DATA.Category { Id = -1, Name = "Tất cả" });
            var chartdatasave = new List<Statistic>
            {
                new Statistic{Month=1,Number=1},new Statistic{Month=2,Number=1},new Statistic{Month=3,Number=1},new Statistic{Month=4,Number=1}, new Statistic{Month=5,Number=1}, new Statistic{Month=6,Number=1},new Statistic{Month=7,Number=1}, new Statistic{Month=8,Number=1}, new Statistic{Month=9,Number=1},   new Statistic{Month=10,Number=1},  new Statistic{Month=11,Number=1},   new Statistic{Month=12,Number=1}

            };
            if (lstcate == null)
            {

                returndata = string.Empty;
                tb1 = string.Empty;
                tb2 = string.Empty;
                return;
            }

            var sb = new StringBuilder();
            var sbTable1 = new StringBuilder();
            var sbTable2 = new StringBuilder();
            sb.Append("<script>");
            sb.Append("var categorydata=[");
            foreach (var point in lstmonth) { sb.Append("' Tháng " + point.ToString() + "',"); }
            sb.Append("];");
            var curmoduleId = 0;
            //int max = 12;
            int index = 0;
            var lstChart = new List<XYData>();
            int totalbycategory = 0;

            foreach (var itemparam in lstcate)
            {
                var chartdata = new ContentBO().GetReport(itemparam.Id, Convert.ToInt32(Year));
                if ((chartdata != null) && (chartdata.Count > 0))
                {
                    var chart = new XYData();
                    chart.name = itemparam.Name;

                    //gentable
                    if (curmoduleId != Convert.ToInt32(Year))
                    {
                        curmoduleId = Convert.ToInt32(Year);

                        sbTable1.Append("<tr style=\"background-color:Silver; color:#3E576F; font-family: 'Lucida Grande','Lucida Sans Unicode',Verdana,Arial,Helvetica,sans-serif; font-size: 12px;\">");
                        sbTable1.Append("<td><div style=\"width:210px;\">" + Year + "</div></td>");
                        sbTable1.Append("</tr>");

                        sbTable2.Append("<tr style=\"background-color:Silver; color:#3E576F; font-family: 'Lucida Grande','Lucida Sans Unicode',Verdana,Arial,Helvetica,sans-serif; font-size: 12px;\">");
                        foreach (var point in lstmonth) { sbTable2.Append("<td> Tháng " + point.ToString() + "</td>"); }
                        sbTable2.Append("<td> Tất cả </td>");
                        sbTable2.Append("</tr>");


                    }

                    sbTable1.Append("<tr>");
                    sbTable1.Append("<td  style=\"color:#3E576F; font-family: 'Lucida Grande','Lucida Sans Unicode',Verdana,Arial,Helvetica,sans-serif; font-size: 12px;text-align:left;\">" + itemparam.Name + "</td>");

                    sbTable2.Append("<tr>");
                    bool a = true;

                    //gen dataline

                    foreach (var dataItem in chartdatasave)
                    {
                        int c = 0;

                        while ((c < chartdata.Count) && (a))
                        {
                            if (dataItem.Month == chartdata[c].Month)
                            {
                                chart.data.Add(chartdata[c].Number);
                                sbTable2.Append("<td>" + chartdata[c].Number + "</td>");
                                totalbycategory += chartdata[c].Number;
                                a = false;
                            }

                            c++;

                        }

                        if (a)
                        {

                            chart.data.Add(0);
                            sbTable2.Append("<td></td>");
                        }
                        a = true;
                    }
                    sbTable2.Append("<td>" + totalbycategory + "</td>");
                    totalbycategory = 0;
                    sbTable1.Append("</tr>");
                    sbTable2.Append("</tr>");
                    lstChart.Add(chart);
                }
                index++;
            }

            lstChart.RemoveAt(index - 1);
            var dataline = Newtonsoft.Json.JsonConvert.SerializeObject(lstChart);

            sb.Append("var dataline=" + dataline + ";");
            sb.Append("</script>");
            returndata = sb.ToString();
            tb1 = sbTable1.ToString();
            tb2 = sbTable2.ToString();

        }

    }
}
