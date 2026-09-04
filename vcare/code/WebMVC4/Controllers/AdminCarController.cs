using BIZ;
using BIZ.Entity;
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
    [Authorize(Roles = "Administrator,Sale")]
    public class AdminCarController : Controller
    {
        //
        // GET: /AdminCar/

        public ActionResult Index()
        {
            var lstCate = new CarGroupBO().GetTopLastestCarGroup();
            if (!lstCate.Exists(x => x.Id == 0))
                lstCate.Insert(0, new CarGroup { Id = 0, Name = "--Chọn hãng xe--" });
            ViewBag.GroupList = lstCate;
            return View();
        }
        public ActionResult ListCarModel(int? groupId, int? status)
        {
            int RegionId = groupId == null ? -1 : (int)groupId;
            int Status = status == null ? -1 : (int)status;
            var GroupList = new CarGroupBO().GetTopLastestCarGroup();
            ViewBag.GroupList = GroupList;
            var data = new CarModelBO().GetTopLastestCarModel(RegionId, Status);
            //foreach(var item in data)
            //{
            //    item.Name = item.Name.TrimStart().TrimEnd();
            //    item.Url =  Utils.ConvertToRewriteLink(GroupList.FirstOrDefault(x=>x.Id==item.GroupId).Name) +"-"+ Utils.ConvertToRewriteLink(item.Name);
            //    new CarModelBO().UpdateDynamic($"Set Name='{item.Name}',Url='{item.Url}' ", $"Id={item.Id}");

            //}    
            return PartialView(data);
        }
        public ActionResult GetCarModelDetail(int Id = 0)
        {
            var model = new CarModel { Id = 0, Order = 1 };
            var GroupList = new CarGroupBO().GetTopLastestCarGroup();
            ViewBag.GroupList = GroupList;
            if (Id > 0)
            {
                model = new CarModelBO().GetCarModel(Id);
                if (model == null)
                    return RedirectToAction("Index");
                ViewBag.Title = "Cập nhật xe";
            }
            else
            {
                ViewBag.Title = "Thêm mới xe";
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult SaveData(CarModel banner)
        {
            var returnData = new ReturnData();
            try
            {
                if (banner.Id == 0)
                {
                    if (banner.GroupId == 30)
                    {
                        banner.Url = Utils.ConvertToRewriteLink(banner.Name);
                    }
                    else
                    {
                        var group = new CarGroupBO().GetCarGroup(banner.GroupId);
                        banner.Url = Utils.ConvertToRewriteLink(group.Name) + "-" + Utils.ConvertToRewriteLink(banner.Name);
                    }
                }
                var result = new CarModelBO().CreateUpdateCarModel(banner);
                returnData.ResponseCode = result;
                if (result >= 0)
                {

                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)UTILS.Constants.CategoryType.CarModel,
                        ItemId = banner.Id,
                        ItemName = banner.Name,
                        Note = "Xóa xe",
                        Type = 1

                    };
                    if (banner.Id > 0)
                    {
                        returnData.Description = "Cập nhật Thành Công";
                        lognewsobj.Note = "Update xe";
                    }

                    else
                    {
                        returnData.Description = "Thêm mới Thành Công";
                        lognewsobj.Note = "Tạo mới xe";
                    }

                    //Ghi log
                    Action<ContentLog> send = InsertContentLog;
                    var asynSend = send.BeginInvoke(lognewsobj, null, null);
                }
                else switch (result)
                    {
                        case -51: returnData.Description = "Đã có bài viết này"; break;
                        case -600: returnData.Description = "Tham số truyền vào không hợp lệ"; break;
                        default: returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                    }
                return Json(returnData);
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                returnData.ResponseCode = -99;
                returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(returnData);
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
                    var result = new CarModelBO().DeleteCarModel(Id);
                    if (result >= 0)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)UTILS.Constants.CategoryType.CarModel,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Xóa xe",
                            Type = 1

                        };
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Xóa banner Thành Công";
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
