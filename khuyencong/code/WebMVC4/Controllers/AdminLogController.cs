using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BIZ;
using Constants = UTILS.Constants;
using BIZ.Entity;
using System.Web.Routing;
using DATA;
using WebMVC4.Models;
using UTILS;
using System.Web.Security;


namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminLogController : Controller
    {
        //
        // GET: /AdminLog/

        public ActionResult Index()
        {
            var lstdata = Membership.GetAllUsers();
            //ExHandler.Handle(new Exception(), "User", "User" + lstdata.Count);
            List<AccountInfo> lstuser = new List<AccountInfo>();
            lstuser.Add(new AccountInfo { Value = "", Text = "-Chọn tài khoản-" });
            foreach (MembershipUser item in lstdata)
            {
                lstuser.Add(new AccountInfo { Value = item.UserName, Text = item.UserName });
            }
            ViewBag.UserList = lstuser;
            var fromDate = new DateTime(DateTime.Now.Year, 1, 1);
            var toDate = DateTime.Now;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            return View();
        }
        public ActionResult ListLog(int? itemtType, long? itemid, string username, string title, int? currentPage, int? pageSize, string fromDate, string endDate)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int TotalRecord = 0;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;
            int Type = itemtType == null ? -1 : (int)itemtType;
            long Itemid = itemid == null ? -1 : (long)itemid;

            var data = new ContentLogBO().GetByFilter(username, Type, Itemid, Title, CurrPage, RecordPerPage, ref TotalRecord, fromDate, endDate);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            return PartialView(data);
        }
    }
}
