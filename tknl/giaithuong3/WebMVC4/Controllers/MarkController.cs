using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BIZ;
using BIZ.Entity;
using DATA;
using WebMVC4.Models;
using Constants = UTILS.Constants;

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Mark,Comment")]
    public class MarkController : Controller
    {
        //
        // GET: /Mark/
        protected override void Initialize(RequestContext requestContext)
        {

            base.Initialize(requestContext);
            if (UTILS.Utils.GetAppSettingValue("EnableMark") != "1")
            {
                Response.Redirect("/");
            }
        }
        public ActionResult Index(int categoryId = 0, int page = 1, string createdby = "", string fromdate = "", string todate = "", string title = "")
        {
            DateTime _endDate = DateTime.Now;
            DateTime _startDate = new DateTime(_endDate.Year, _endDate.Month, 1);
            if (string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            {
                fromdate = _startDate.ToString("dd/MM/yyyy");
                todate = _endDate.ToString("dd/MM/yyyy");

            }

            var lstcate = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.News);
            var listcategory = new List<CATEGORY_FULL>();
            foreach (var item in lstcate)
            {
                if (item.ParentId > 0)
                {
                    var x1 = new CATEGORY_FULL { Id = item.Id, ParentId = item.ParentId, Name = item.Name };
                    x1.Name = "-+ " + x1.Name;

                    try
                    {
                        int pindex = listcategory.Select((Value, Index) => new { Value, Index }).FirstOrDefault(x => x.Value.Id == x1.ParentId).Index;
                        listcategory.Insert(pindex + 1, x1);
                    }
                    catch
                    {

                        listcategory.Add(item);
                    }
                }
                else
                {
                    listcategory.Add(item);
                }
            }
            ViewBag.categoryId = categoryId;
            ViewBag.createdby = createdby;
            ViewBag.todate = todate;
            ViewBag.fromdate = fromdate;
            ViewBag.keyword = title;
            listcategory.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Tất cả chuyên mục--" });
            ViewBag.CategoryList = listcategory;
            var lstdata = Membership.GetAllUsers();
            List<SelectListItem> lstuser = new List<SelectListItem>();
            lstuser.Add(new SelectListItem { Value = "-1", Text = "--Tất cả người viết" });
            foreach (MembershipUser item in lstdata)
            {
                lstuser.Add(new SelectListItem { Value = item.UserName, Text = item.UserName });
            }
            ViewBag.UserList = lstuser;

            int total = 0;
            var lstnews = new ContentBO().GetFilterContentFullsPaged(page, 40, title, categoryId, null, 4, "", ref total, fromdate, todate, "", createdby);
            var model = new NewsModel { CategoryId = categoryId, pageIndex = page, pageSize = 40, listdata = lstnews, total = total };
            return View(model);
        }
        public ContentResult AutoCompleteUser(string searchText)
        {
            var lstdata = Membership.GetAllUsers();
            List<SelectListItem> lstuser = new List<SelectListItem>();
            foreach (var item in lstdata)
            {

                lstuser.Add(new SelectListItem { Value = item.ToString(), Text = item.ToString() });
            }
            var lstaccount = lstuser.Select(x => x.Text).Distinct().ToList();
            var filteredaccount = lstaccount.Where(x => x.ToLower().Contains(searchText.ToLower())).ToList();

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            var jsonString = jsonSerializer.Serialize(filteredaccount).ToString();
            return Content(jsonString);

        }
        public ActionResult ToExcel(int categoryId = 0, int page = 1, string createdby = "-1", string fromdate = "", string todate = "", string title = "")
        {
            DateTime _endDate = DateTime.Now;
            DateTime _startDate = new DateTime(_endDate.Year, _endDate.Month, 1);
            if (string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            {
                fromdate = _startDate.ToString("dd/MM/yyyy");
                todate = _endDate.ToString("dd/MM/yyyy");

            }
            int total = 0;
            var lstnews = new ContentBO().GetFilterContentFullsPaged(page, 5000, title, categoryId, null, 4, createdby, ref total, fromdate, todate);
            var lstdata = new List<MarkExcelInfo>();
            if (lstnews != null)
            {

                foreach (var contentFull in lstnews)
                {
                    var obj = new MarkExcelInfo { Author = contentFull.Alias,Category= contentFull.CategoryName, Title = contentFull.Title, View = contentFull.Hits.ToString(), PublishedDate = contentFull.PublishDate.ToString("dd/MM/yyyy HH:mm"), Mark = contentFull.Mark.Value,Link= contentFull.LinkUrl };
                    lstdata.Add(obj);
                }
            }

            Response.Clear();
            Response.Buffer = true;
            //Response.Charset = "UTF-8"; 

            Response.AppendHeader("Content-Disposition", "attachment;filename=VTK_Mark.xls");
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            //EnableViewState = false;
            //var myCItrad = new CultureInfo("VI-VN", true);
            var oStringWriter = new StringWriter();
            var oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
            oHtmlTextWriter.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");

            var grid = new DataGrid { DataSource = lstdata };
            grid.DataBind();
            grid.RenderControl(oHtmlTextWriter);

            Response.Write(oStringWriter.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Index");

        }
        public ActionResult ToExcelReport(string fromdate = "", string todate = "", string title = "")
        {
            DateTime _endDate = DateTime.Now;
            DateTime _startDate = new DateTime(_endDate.Year, _endDate.Month, 1);
            if (string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            {
                fromdate = _startDate.ToString("dd/MM/yyyy");
                todate = _endDate.ToString("dd/MM/yyyy");

            }

            var lstdata = new ContentBO().GetFiltertSTMark(fromdate, todate);
            Response.Clear();
            Response.Buffer = true;
            //Response.Charset = "UTF-8"; 

            Response.AppendHeader("Content-Disposition", "attachment;filename=VTK_MarkReport.xls");
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            //EnableViewState = false;
            //var myCItrad = new CultureInfo("VI-VN", true);
            var oStringWriter = new StringWriter();
            var oHtmlTextWriter = new HtmlTextWriter(oStringWriter);
            oHtmlTextWriter.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");

            var grid = new DataGrid { DataSource = lstdata };
            grid.DataBind();
            grid.RenderControl(oHtmlTextWriter);

            Response.Write(oStringWriter.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("Report");

        }
        public ActionResult Report(string fromdate = "", string todate = "")
        {
            DateTime _endDate = DateTime.Now;
            DateTime _startDate = new DateTime(_endDate.Year, _endDate.Month, 1);
            if (string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            {
                fromdate = _startDate.ToString("dd/MM/yyyy");
                todate = _endDate.ToString("dd/MM/yyyy");

            }
            ViewBag.todate = todate;
            ViewBag.fromdate = fromdate;
            var lstdata = new ContentBO().GetFiltertSTMark(fromdate, todate);

            return View(lstdata);
        }
        public ActionResult AddMark(int id)
        {
            var newscontent = new ContentBO().GetContentFull(id);
            ViewBag.contentid = id;
            ViewBag.ContentTitle = newscontent.Title;
            var lstdata = new MarkLogBO().GetMarkLogsByContentId(id).ToList();
            return PartialView(lstdata);
        }
        [HttpPost]
        public ActionResult SaveMark(long mark, string reason, int contentid)
        {
            string results = "0";
            new ContentBO().Mark(contentid, mark);
            var obj = new MarkLog
            {
                ContentId = contentid,
                Mark = mark,
                Reason = reason,
                UserName = HttpContext.User.Identity.Name
            };
            if (new MarkLogBO().CreateUpdateMarkLog(obj) >= 0)
            {
                results = "1";
            }
            return Json(results);

        }


    }
}
