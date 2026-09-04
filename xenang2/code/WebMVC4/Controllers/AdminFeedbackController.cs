using BIZ;
using DATA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Models;
namespace WebMVC4.Controllers
{
    public class AdminFeedbackController : Controller
    {
        //
        // GET: /AdminFeedback/
        [Authorize(Roles = "Administrator,Sale")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListFeedback(int? currentPage, int? pageSize)
        {

            var data = new List<Feedback>();

            int TotalRecord = 0;
           

            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;
            data = new FeedbackBO().GetAllFeedbacksPaged(CurrPage, RecordPerPage,ref TotalRecord);
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
