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
using System.Globalization;
using DATA.ContentDB;

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Category")]
    public class AdminMission2Controller : Controller
    {



        public ActionResult Index()
        {



            return View();
        }
        public ActionResult ListMission(int? cateId, int? status, string title, int? currentPage, int? pageSize)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int TotalRecord = 0;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;
            int CateId = cateId == null ? -1 : (int)cateId;
            int Status = status == null ? -1 : (int)status;

            var data = Mission2DAL.GetSearch(Status,title, CurrPage, RecordPerPage, ref TotalRecord);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;



            return PartialView(data);
        }
        public ActionResult GetMissionDetail(int Id = 0)
        {

            var model = new Mission2 { Id = 0, PublishDate = DateTime.Now, FromDate = 0, ToDate = 0, Accept = 0, Result = 2 };
            if (Id > 0)
            {
                model = Mission2DAL.GetDetail(Id);
                if (model == null)
                    return RedirectToAction("Index");

                ViewBag.Title = "Cập nhật nhận xét";
            }
            else
            {
                ViewBag.Title = "Thêm mới nhận xét";
            }
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult SaveData(Mission2 doc)
        {
            var ReturnData = new ReturnData();
            try
            {

                IFormatProvider culture = new CultureInfo("en-US", true);
                doc.PublishDate = DateTime.Now;
                var result = Mission2DAL.InsertUpdate(doc);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {

                    //var lognewsobj = new ContentLog
                    //{
                    //    UserName = HttpContext.User.Identity.Name,
                    //    ItemtType = (int)Constants.CategoryType.Doc,
                    //    ItemId = doc.Id,
                    //    ItemName = doc.Name,
                    //    Note = "Xóa nhiệm vụ",
                    //    Type = 1

                    //};
                    if (doc.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                        //lognewsobj.Note = "Update nhiệm vụ";
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        //lognewsobj.Note = "Tạo mới nhiệm vụ";
                    }

                    //Ghi log
                    //Action<ContentLog> send = InsertContentLog;
                    //var asynSend = send.BeginInvoke(lognewsobj, null, null);
                }
                else switch (result)
                    {
                        case -51: ReturnData.Description = "Đã có bài viết này"; break;
                        case -600: ReturnData.Description = "Tham số truyền vào không hợp lệ"; break;
                        default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                    }
                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UpdateStatus(string _id, string Title)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var obj = Mission2DAL.GetDetail(Id);
                    if (obj != null)
                    {
                        //var lognewsobj = new ContentLog
                        //{
                        //    UserName = HttpContext.User.Identity.Name,
                        //    ItemtType = (int)Constants.CategoryType.Doc,
                        //    ItemId = Id,
                        //    ItemName = Title,
                        //    Note = "Duyệt nhiệm vụ",
                        //    Type = 1

                        //};
                        if (obj.Status == 0)
                        {
                            obj.Status = 1;
                        }
                        else
                        {
                            obj.Status = 0;
                            //lognewsobj.Note = "Khóa nhiệm vụ";
                        }
                        Mission2DAL.InsertUpdate(obj);
                        //Ghi log
                        //Action<ContentLog> send = InsertContentLog;
                        //var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Cập nhật nhiệm vụ Thành Công";
                    }
                    else
                    {
                        ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                    }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định nhiệm vụ cần thao tác";
                return Json(ReturnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }
       

    }
}
