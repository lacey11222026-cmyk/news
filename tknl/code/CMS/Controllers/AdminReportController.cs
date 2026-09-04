using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using STATS;
using BIZ;
using BIZ.Entity;
using DATA;





namespace CMS.Controllers
{
    [Authorize(Roles = "Administrator,Report")]
    public class AdminReportController : Controller
    {
        public ActionResult Index(string Year, int CategoryId = 0)
        {
            if (string.IsNullOrEmpty(Year))
                Year = DateTime.Now.Year.ToString();

            ViewBag.CategoryId = CategoryId;
            ViewBag.Year = Year;
            var datagrib = string.Empty;
            var datatb1 = string.Empty;
            var datatb2 = string.Empty;
            var lstcate = new CategoryBO().GetAllChildCategories(CategoryId, 30, false);
            if (CategoryId == 0)
            {
                lstcate = lstcate.Where(x => x.NodeLevel == 1).ToList();
                lstcate.Add(new CATEGORY_FULL { Id = CategoryId, Name = "Tất cả" });
            }

            ViewBag.lstcate = lstcate;
            filldata(Year, lstcate, ref datagrib, ref datatb1, ref datatb2);

            ViewBag.datagrib = datagrib;
            ViewBag.datatb1 = datatb1;
            ViewBag.datatb2 = datatb2;
            ViewBag.Title = "Thống kê tin bài";
            return View();
        }
        private void filldata(string Year, List<CATEGORY_FULL> lstcate, ref string returndata, ref string tb1, ref string tb2)
        {

            var lstmonth = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };


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

        #region Google Analytics

        public ActionResult Overview(string Start = "", string End = "")
        {
            var startDate = DateTime.Now.AddMonths(-1);
            var endDate = DateTime.Now;

            try { startDate = DateTime.ParseExact(Start, "dd/MM/yyyy", null); }
            catch { }
            try { endDate = DateTime.ParseExact(End, "dd/MM/yyyy", null); }
            catch { }

            var data = new GoogleAnalyticsBO().GetOverviewByHour(startDate, endDate);

            ViewBag.StartDate = startDate.ToString("dd/MM/yyyy");
            ViewBag.EndDate = endDate.ToString("dd/MM/yyyy");

            // for chart
            ViewBag.CategoriesData = data.Select(x => x.Hour).ToList();
            ViewBag.SessionsData = data.Select(x => x.Sessions).ToList();
            ViewBag.UsersData = data.Select(x => x.Users).ToList();
            ViewBag.PageviewsData = data.Select(x => x.Pageviews).ToList();

            return View(data);
        }

        public ActionResult GetTopCategory(string Start = "", string End = "", int Site = 0)
        {
            var startDate = DateTime.Now.AddMonths(-1);
            var endDate = DateTime.Now;

            try { startDate = DateTime.ParseExact(Start, "dd/MM/yyyy", null); }
            catch { }
            try { endDate = DateTime.ParseExact(End, "dd/MM/yyyy", null); }
            catch { }

            var data = new STATS.GoogleAnalyticsBO().GetTopCategory(startDate, endDate, Site);

            ViewBag.StartDate = startDate.ToString("dd/MM/yyyy");
            ViewBag.EndDate = endDate.ToString("dd/MM/yyyy");

            return View(data);
        }

        public ActionResult GetTopContent(string Start = "", string End = "", int Site = 0)
        {
            var startDate = DateTime.Now.AddMonths(-1);
            var endDate = DateTime.Now;

            try { startDate = DateTime.ParseExact(Start, "dd/MM/yyyy", null); }
            catch { }
            try { endDate = DateTime.ParseExact(End, "dd/MM/yyyy", null); }
            catch { }

            var data = new STATS.GoogleAnalyticsBO().GetTopContent(startDate, endDate, Site);

            ViewBag.StartDate = startDate.ToString("dd/MM/yyyy");
            ViewBag.EndDate = endDate.ToString("dd/MM/yyyy");

            return View(data);
        }

        #endregion

    }
}
