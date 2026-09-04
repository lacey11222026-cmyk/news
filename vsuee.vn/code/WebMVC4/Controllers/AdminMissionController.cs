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

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Category")]
    public class AdminMissionController : Controller
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

            var data = new MissionBO().GetMissionsFuLLPaged(title, CateId, Status, -1, -1, CurrPage, RecordPerPage, ref TotalRecord);
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

            var model = new MISSION_FULL { Id = 0, PublishDate = DateTime.Now, FromDate = 0, ToDate = 0, Accept = 0, Result = 2,Code=DateTime.Now.ToString("dd/MM/yyyy") };
            if (Id > 0)
            {
                model = new MissionBO().GetMissionFull(Id);
                if (model == null)
                    return RedirectToAction("Index");

                ViewBag.Title = "Cập nhật sự kiện";
            }
            else
            {
                ViewBag.Title = "Thêm mới sự kiện";
            }
            return View(model);
        }
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult SaveData(MISSION_FULL doc)
        {
            var ReturnData = new ReturnData();
            try
            {

                IFormatProvider culture = new CultureInfo("en-US", true);
                doc.PublishDate = DateTime.ParseExact(doc.SPublishDate, "dd/MM/yyyy", culture);
                var result = new MissionBO().CreateUpdateMission(doc);
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
                    var obj = new MissionBO().GetMissionFull(Id);
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
                        new MissionBO().CreateUpdateMission(obj);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string _id, string Title)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var result = new MissionBO().DeleteMission(Id);
                    if (result >= 0)
                    {
                        //var lognewsobj = new ContentLog
                        //{
                        //    UserName = HttpContext.User.Identity.Name,
                        //    ItemtType = (int)Constants.CategoryType.Doc,
                        //    ItemId = Id,
                        //    ItemName = Title,
                        //    Note = "Xóa nhiệm vụ",
                        //    Type = 1

                        //};
                        //Ghi log
                        //<ContentLog> send = InsertContentLog;
                        //var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Xóa nhiệm vụ Thành Công";
                    }
                    else switch (result)
                        {
                            case -50: ReturnData.Description = "Bài Viết không tồn tại"; break;
                            case -99: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                        }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định bài viết cần xóa";
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
