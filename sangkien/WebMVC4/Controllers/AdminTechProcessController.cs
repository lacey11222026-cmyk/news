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
    public class AdminTechProcessController : Controller
    {


        [HttpPost]
        public JsonResult UpdateSortOrder(int Id, bool SortOrder)
        {
            try
            {
                var updateResult = new TechProcessBO().UpdateOrder(Id, SortOrder);
                if (updateResult > 0)
                {
                    return Json(new { ResponseCode = updateResult, Msg = "Cập nhật thứ tự thành công" });
                }
                else
                {
                    return Json(new { ResponseCode = -1, Msg = "Cập nhật thứ tự không thành công" });
                }

            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return Json(new { ResponseCode = -99, Msg = "Hệ thống bận bạn vui lòng quay lại sau" });
            }
        }
        public ActionResult Index()
        {



            return View();
        }
        public ActionResult ListTechProcess(string title, int? currentPage, int? pageSize)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int TotalRecord = 0;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;


            var data = new TechProcessBO().GetTechProcesssFuLLPaged(title, CurrPage, RecordPerPage, ref TotalRecord);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;



            return PartialView(data);
        }
        public ActionResult GetTechProcessDetail(int Id = 0)
        {

            var model = new TechProcess { Id = 0 };
            if (Id > 0)
            {
                model = new TechProcessBO().GetTechProcess(Id);
                if (model == null)
                    return RedirectToAction("Index");

                ViewBag.Title = "Cập nhật quy trình";
            }
            else
            {
                ViewBag.Title = "Thêm mới quy trình";
            }
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult SaveData(TechProcess doc)
        {
            var ReturnData = new ReturnData();
            try
            {

                IFormatProvider culture = new CultureInfo("en-US", true);
                
                var result = new TechProcessBO().CreateUpdateTechProcess(doc);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {

                    //var lognewsobj = new ContentLog
                    //{
                    //    UserName = HttpContext.User.Identity.Name,
                    //    ItemtType = (int)Constants.CategoryType.Doc,
                    //    ItemId = doc.Id,
                    //    ItemName = doc.Name,
                    //    Note = "Xóa quy trình",
                    //    Type = 1

                    //};
                    if (doc.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                        //lognewsobj.Note = "Update quy trình";
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        //lognewsobj.Note = "Tạo mới quy trình";
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
                    var result = new TechProcessBO().DeleteTechProcess(Id);
                    if (result >= 0)
                    {
                        //var lognewsobj = new ContentLog
                        //{
                        //    UserName = HttpContext.User.Identity.Name,
                        //    ItemtType = (int)Constants.CategoryType.Doc,
                        //    ItemId = Id,
                        //    ItemName = Title,
                        //    Note = "Xóa quy trình",
                        //    Type = 1

                        //};
                        //Ghi log
                        //<ContentLog> send = InsertContentLog;
                        //var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Xóa quy trình Thành Công";
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
