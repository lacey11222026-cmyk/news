using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BIZ;
using BIZ.Entity;
using DATA;
using CMS.Models;
using Constants = UTILS.Constants;

namespace CMS.Controllers
{
    [Authorize(Roles = "Mark,Administrator")]
    public class MarkController : Controller
    {
        //
        // GET: /Mark/
        private List<CATEGORY_FULL> _staticCategoryList;
        private List<CATEGORY_FULL> _staticCategoryByUserList;
        protected override void Initialize(RequestContext requestContext)
        {
            _staticCategoryList = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.News);
            _staticCategoryByUserList = new CategoryBO().GetCategoryByUserName(_staticCategoryList, requestContext.HttpContext.User.Identity.Name,
                                                                               requestContext.HttpContext.User.IsInRole("Administrator"));
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

            var listcategory = _staticCategoryByUserList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }
      
            ViewBag.categoryId = categoryId;
            ViewBag.createdby = createdby;
            ViewBag.todate = todate;
            ViewBag.fromdate = fromdate;
            ViewBag.keyword = title;
            listcategory.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Tất cả chuyên mục--" });
            ViewBag.CategoryList = listcategory;
            var lstdata = Membership.GetAllUsers();
            var lstuser = new List<SelectListItem>();
            lstuser.Add(new SelectListItem { Value = "-1", Text = "--Tất cả tác giả" });
            foreach (MembershipUser item in lstdata)
            {
                lstuser.Add(new SelectListItem { Value = item.UserName, Text = item.UserName });
            }
            ViewBag.UserList = lstuser;

            int total = 0;
            int totalmark = 0;
            var lstnews = new List<CONTENT_FULL>();
            
            string inputuser = "notspider";
            if (!String.IsNullOrEmpty(createdby))
            {
                inputuser = createdby;
            }
            lstnews = new ContentBO().GetFilterContentMarkPaged(page, 40, title, categoryId, 1, inputuser, ref total,ref totalmark, fromdate, todate);
            ViewBag.TotalMark = totalmark;
            //if (User.IsInRole("Administrator"))
            //{
            //    lstnews = new ContentBO().GetFilterContentFullsPaged(page, 40, title, categoryId, null, 1, inputuser, ref total, -1, fromdate, todate);
            //}
            //else
            //{
            //    lstnews = new ContentBO().GetFilterContentFullsPaged(page, 40, title, categoryId, listcategory.Where(x => x.ParentId == 0).Select(x => x.Id).ToList(), 1, inputuser, ref total, -1, fromdate, todate);
            //}
            //var lstnews = new ContentBO().GetFilterContentFullsPaged(page, 40, title, categoryId,null, 1, createdby, ref total,-1, fromdate, todate);
            var model = new NewsModel { CategoryId = categoryId, pageIndex = page, pageSize = 40, listdata = lstnews, total = total };
            ViewBag.title = "Quản lý nhuận bút";
            return View(model);
        }
        public ActionResult ToExcel(int categoryId = 0, int page = 1, string createdby = "-1", string fromdate = "", string todate = "", string title = "")
        {
            var listcategory = _staticCategoryByUserList;
            if (listcategory == null)
            { return RedirectToAction("AssesDenied", "Admin"); }
      
            DateTime _endDate = DateTime.Now;
            DateTime _startDate = new DateTime(_endDate.Year, _endDate.Month, 1);
            if (string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            {
                fromdate = _startDate.ToString("dd/MM/yyyy");
                todate = _endDate.ToString("dd/MM/yyyy");

            }
            int total = 0;
            var lstnews = new List<CONTENT_FULL>();
            if (User.IsInRole("Administrator"))
            {
                lstnews = new ContentBO().GetFilterContentFullsPaged(page, 1000, title, categoryId, null, 1, createdby, ref total, -1, fromdate, todate);
            }
            else
            {
                lstnews = new ContentBO().GetFilterContentFullsPaged(page, 1000, title, categoryId, listcategory.Where(x => x.ParentId == 0).Select(x => x.Id).ToList(), 1, createdby, ref total, -1, fromdate, todate);
            }
            //var lstnews = new ContentBO().GetFilterContentFullsPaged(page, 1000, title, categoryId,null,1, createdby, ref total, -1,fromdate, todate);
            var lstdata = new List<MarkExcelInfo>();
            if (lstnews != null)
            {

                foreach (var contentFull in lstnews)
                {
                    var obj = new MarkExcelInfo { Author = contentFull.CreatedBy, Title = contentFull.Title, Category = contentFull.CategoryName, View = contentFull.Hits.ToString(), PublishedDate = contentFull.PublishDate.ToString("dd/MM/yyyy HH:mm"), Mark = contentFull.Mark.GetValueOrDefault() };
                    lstdata.Add(obj);
                }
            }

            Response.Clear();
            Response.Buffer = true;
            //Response.Charset = "UTF-8"; 

            Response.AppendHeader("Content-Disposition", "attachment;filename=GT_Mark.xls");
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

            var data = oStringWriter.ToString().Replace("Title", "Tên bài viết ").Replace("Author", "Tác giả").Replace("Category", "Chuyên mục").Replace("PublishedDate", "Ngày xuất bản").Replace("View", "Lượt xem").Replace("Mark", "Tiền nhuận bút(VND)");
            Response.Write(data);
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

            var lstdata = new ContentBO().GetFiltertStMark(fromdate, todate);
            Response.Clear();
            Response.Buffer = true;
            //Response.Charset = "UTF-8"; 

            Response.AppendHeader("Content-Disposition", "attachment;filename=GT_MarkReport.xls");
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

            var data = oStringWriter.ToString().Replace("Title", "Tên bài viết ").Replace("TotalContent", "Tổng số bài viết").Replace("Category", "Chuyên mục").Replace("CreatedBy", "Người viết").Replace("View", "Lượt xem").Replace("TotalMark", "Tổng tiền nhuận bút(VND)");
            Response.Write(data);
            Response.Flush();
            Response.End();
            return RedirectToAction("Report");

        }
        public ActionResult Report(string fromdate = "", string todate = "")
        {
            DateTime endDate = DateTime.Now;
            DateTime startDate = new DateTime(endDate.Year, endDate.Month, 1);
            if (string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            {
                fromdate = startDate.ToString("dd/MM/yyyy");
                todate = endDate.ToString("dd/MM/yyyy");

            }
            ViewBag.todate = todate;
            ViewBag.fromdate = fromdate;
            var lstdata = new ContentBO().GetFiltertStMark(fromdate, todate);
            ViewBag.title = "Thống kê nhuận bút theo tác giả";
            return View(lstdata);
        }
        public ActionResult ReportByCate(string fromdate = "", string todate = "")
        {
            DateTime endDate = DateTime.Now;
            DateTime startDate = new DateTime(endDate.Year, endDate.Month, 1);
            if (string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            {
                fromdate = startDate.ToString("dd/MM/yyyy");
                todate = endDate.ToString("dd/MM/yyyy");

            }
            ViewBag.todate = todate;
            ViewBag.fromdate = fromdate;
            var lstdata = new List<MarkST>();
            var total = 0;
            var totalmark = 0;
            var lstcate = new CategoryBO().GetAllChildCategories(0, 100, false).Where(x => x.Type == 2 && x.Published == 1).ToList();
            //lstcate.Add(new CATEGORY_FULL { Id = 0, Name = "Tất cả" });
            
            foreach (var itemparam in lstcate)
            {
                var data = new ContentBO().GetFilterContentMarkPaged(1, 1, string.Empty, itemparam.Id, 1, "notspider", ref total, ref totalmark, fromdate, todate);
                var item = new MarkST
                               {
                                   CreatedBy = itemparam.Name,
                                   TotalContent = total,
                                   TotalMark = totalmark,
                               };
                lstdata.Add(item);
            }
            ViewBag.title = "Thống kê nhuận bút theo chuyên mục";
            return View(lstdata);
        }
        public ActionResult ToExcelReportByCate(string fromdate = "", string todate = "", string title = "")
        {
            DateTime _endDate = DateTime.Now;
            DateTime _startDate = new DateTime(_endDate.Year, _endDate.Month, 1);
            if (string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
            {
                fromdate = _startDate.ToString("dd/MM/yyyy");
                todate = _endDate.ToString("dd/MM/yyyy");

            }

            var lstdata = new List<MarkST>();
            var total = 0;
            var totalmark = 0;
            var lstcate = new CategoryBO().GetAllChildCategories(0, 15, false).Where(x => x.ParentId == 0&&x.Published==1).ToList();
            //lstcate.Add(new CATEGORY_FULL { Id = 0, Name = "Tất cả" });

            foreach (var itemparam in lstcate)
            {
                var newsdata = new ContentBO().GetFilterContentMarkPaged(1, 1, string.Empty, itemparam.Id, 1, "notspider", ref total, ref totalmark, fromdate, todate);
                var item = new MarkST
                {
                    CreatedBy = itemparam.Name,
                    TotalContent = total,
                    TotalMark = totalmark,
                };
                lstdata.Add(item);
            }
            Response.Clear();
            Response.Buffer = true;
            //Response.Charset = "UTF-8"; 

            Response.AppendHeader("Content-Disposition", "attachment;filename=GT_MarkReportByCate.xls");
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

            var data = oStringWriter.ToString().Replace("Title", "Tên bài viết ").Replace("TotalContent", "Tổng số bài viết").Replace("Category", "Chuyên mục").Replace("CreatedBy", "Chuyên mục").Replace("View", "Lượt xem").Replace("TotalMark", "Tổng tiền nhuận bút(VND)");
            Response.Write(data);
            Response.Flush();
            Response.End();
            return RedirectToAction("Report");

        }
        public ActionResult AddMark(int id)
        {
            var newscontent = new ContentBO().GetContentFull(id);
            ViewBag.contentid = id;
            ViewBag.ContentTitle = newscontent.Title;
            ViewBag.Title = "Chấm nhuận bút";
            var lstdata = new MarkLogBO().GetMarkLogsByContentId(id).ToList();
            return View(lstdata);
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
