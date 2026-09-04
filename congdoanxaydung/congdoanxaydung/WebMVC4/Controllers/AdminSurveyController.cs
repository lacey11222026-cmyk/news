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
using Newtonsoft.Json;

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Category")]
    public class AdminSurveyController : Controller
    {
        //
        // GET: /AdminSurvey/

        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult ListSurvey( int? status, int? currentPage, int? pageSize)
        {

            int Status = status == null ? -1 : (int)status;
            int TotalRecord = 0;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;

            var data = new SurveyBO().GetAllSurveysPaged( CurrPage, RecordPerPage, ref TotalRecord, Status);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            return PartialView(data);
        }

        public ActionResult SurveyDetail(int Id = 0)
        {
            var model = new Survey { Id = 0, Status = 1, Type = 1,BeginDate = DateTime.Now,EndDate = DateTime.Now.AddMonths(12)};
            if (Id > 0)
            {
                model = new SurveyBO().GetSurvey(Id);

                if (model == null)
                    return RedirectToAction("Index");
                ViewBag.Title = "Cập nhật khảo sát";
                
            }
            else
            {
                ViewBag.Title = "Thêm mới khảo sát";
               
            }
           

            var data = new Survey_Full()
            {
                Id = model.Id,
                Title = model.Title,
                Type = model.Type,
                Status = model.Status,
                BeginDate = model.BeginDate,
                EndDate = model.EndDate,
                Content = model.Content


            };
            //if (Id > 0)
            //{
            //    data.SurveyItems = new SurveyItemBO().GetSurveyItemsBy(Id, -1);
            //}
           
            return View(data);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult SurveySaveData(Survey obj,string SEndDate, string SBeginDate)
        {
            var ReturnData = new ReturnData();
            try
            {
                IFormatProvider culture = new CultureInfo("en-US", true);
                obj.EndDate = DateTime.ParseExact(SEndDate, "dd/MM/yyyy HH:mm", culture);
                obj.BeginDate = DateTime.ParseExact(SBeginDate, "dd/MM/yyyy HH:mm", culture);
                var result = new SurveyBO().CreateUpdateSurvey(obj);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {

                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)Constants.CategoryType.Survey,
                        ItemId = obj.Id,
                        ItemName = obj.Title,
                        Note = "Xóa khảo sát",
                        Type = 1

                    };
                    if (obj.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                        lognewsobj.Note = "Update khảo sát";
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        lognewsobj.Note = "Tạo mới khảo sát";
                    }

                    //Ghi log
                    Action<ContentLog> send = InsertContentLog;
                    var asynSend = send.BeginInvoke(lognewsobj, null, null);
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
        public ActionResult SurveyUpdateStatus(string _id, string Title)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var obj = new SurveyBO().GetSurvey(Id);
                    if (obj != null)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.Survey,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Duyệt khảo sát",
                            Type = 1

                        };
                        if (obj.Status == 0)
                        {
                            obj.Status = 1;
                        }
                        else
                        {
                            obj.Status = 0;
                            lognewsobj.Note = "Khóa khảo sát";
                        }
                        new SurveyBO().CreateUpdateSurvey(obj);
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Cập nhật Thành Công";
                    }
                    else
                    {
                        ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                    }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định văn bản cần thao tác";
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
        public ActionResult DeleteSurvey(string _id, string Title)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var result = new SurveyBO().DeleteSurvey(Id);
                    if (result >= 0)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.Survey,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Xóa khảo sát",
                            Type = 1

                        };
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Xóa Thành Công";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult SurveyItemSaveData(SurveyItem obj)
        {
            var ReturnData = new ReturnData();
            try
            {
                var result = new SurveyItemBO().CreateUpdateSurveyItem(obj);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {

                   
                    if (obj.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                       
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        
                    }

                    
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
        public ActionResult DeleteSurveyItem(string _id)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var result = new SurveyItemBO().DeleteSurveyItem(Id);
                    if (result >= 0)
                    {
                        
                        ReturnData.Description = "Xóa Thành Công";
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
        public ActionResult SurveyItem(int Id)
        {
            ViewBag.Id = Id;
            return PartialView();
        }
        public ActionResult ListSurveyItem(int Id)
        {
            var data = new SurveyItemBO().GetSurveyItemsBy(Id, -1);
            ViewBag.Id = Id;
            return PartialView(data);
        }
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }


    }
}
