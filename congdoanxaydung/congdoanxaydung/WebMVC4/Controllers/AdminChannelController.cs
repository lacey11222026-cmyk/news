using BIZ;
using BIZ.Entity;
using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Constants = UTILS.Constants;
using WebMVC4.Models;
using UTILS;
using Newtonsoft.Json;

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Category")]
    public class AdminChannelController : Controller
    {
        //
        // GET: /AdminChannel/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ListChannel(int? cateId, int? status, string fromDate, string endDate, string title, int? currentPage, int? pageSize)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int TotalRecord = 0;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;
            int CateId = cateId == null ? -1 : (int)cateId;
            int Status = status == null ? -1 : (int)status;

            var data = new ChannelBO().GetAllChannelFullsPaged(title, CurrPage, RecordPerPage, ref TotalRecord, Status);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;


            return PartialView(data);
        }
        public ActionResult GetChannelDetail(int Id = 0)
        {
            var model = new Channel { Id = 0 };
            if (Id > 0)
            {
                model = new ChannelBO().GetChannel(Id);
                if (model == null)
                    return RedirectToAction("Index");
                ViewBag.Title = "Cập nhật kênh tin";
            }
            else
            {
                ViewBag.Title = "Thêm mới kênh tin";
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult SaveData(Channel channel)
        {
            var ReturnData = new ReturnData();
            try
            {
                var result = new ChannelBO().CreateUpdateChannel(channel);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {

                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)Constants.CategoryType.Channel,
                        ItemId = channel.Id,
                        ItemName = channel.Name,
                        Note = "Xóa kênh tin",
                        Type = 1

                    };
                    if (channel.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                        lognewsobj.Note = "Update Channel";
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        lognewsobj.Note = "Tạo mới Channel";
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
        public ActionResult UpdateStatus(string _id, string Title)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var obj = new ChannelBO().GetChannel(Id);
                    if (obj != null)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.Channel,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Duyệt kênh tin",
                            Type = 1

                        };
                        if (obj.Published == 0)
                        {
                            obj.Published = 1;
                        }
                        else
                        {
                            obj.Published = 0;
                            lognewsobj.Note = "Khóa kênh tin";
                        }
                        new ChannelBO().CreateUpdateChannel(obj);
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Cập nhật kênh tin Thành Công";
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
        public ActionResult Delete(string _id, string Title)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var result = new ChannelBO().DeleteChannel(Id);
                    if (result >= 0)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.Channel,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Xóa kênh tin",
                            Type = 1

                        };
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Xóa kênh tin Thành Công";
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
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
    }
}
