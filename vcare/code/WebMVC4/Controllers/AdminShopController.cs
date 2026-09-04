using BIZ;
using DATA;
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
    public class AdminShopController : Controller
    {
        //
        // GET: /AdminQuestion/
        [OutputCache(VaryByParam = "*", Duration = 120, VaryByCustom = "*")]
        public JsonResult GetListCity()
        {
          
            var data = CityDBBase.Create().GetTopLastest(-1, 1).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ManageShop()
        {
            ViewBag.Title = "Danh sách hệ thống phân phối";
            return View();
        }

        public ActionResult ListShop(int? status, int? currentPage, int? pageSize)
        {

            var data = new List<Shop>();

            int TotalRecord = 0;
            int Status = status == null ? -1 : (int)status;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;
            data = new ShopBO().GetShopsPaged(CurrPage, RecordPerPage, ref TotalRecord, Status);
            if (data != null)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;
            return PartialView(data);
        }

        public ActionResult Info(int? Id)
        {
            int PageID = Id == null ? 0 : (int)Id;
            var model = new Shop
            {
               CityId=1,
            };
            
            if (PageID > 0)
            {
                model = new ShopBO().GetShop(PageID);
            }
            
            ViewBag.id = Id;

            if (Id > 0)
            {
                ViewBag.Title = "Cập nhật hệ thống phân phối";
            }
            else
            {
                ViewBag.Title = "Thêm mới hệ thống phân phối";
            }
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveData(Shop Shop)
        {
            var ReturnData = new ReturnData();

            try
            {
                //banner.ImageUrl = Config.UrlRoot + (string.IsNullOrEmpty(banner.ImageUrl) ? string.Empty : banner.ImageUrl.Substring(12));

                Shop.Order = Convert.ToInt32(Shop.Order);

                var result = new ShopBO().CreateUpdateShop(Shop);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (Shop.Id > 0)
                        ReturnData.Description = "Cập nhật Thành Công";
                    else
                        ReturnData.Description = "Thêm mới Thành Công";


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
        public JsonResult Delete(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id > 0)
                {
                    var result = new ShopBO().DeleteShop(id);
                    if (result >= 0)
                    {

                        ReturnData.Description = "Xóa trang Thành Công";
                    }
                    else switch (result)
                        {
                            case -50: ReturnData.Description = "Tài Khoản không tồn tại"; break;
                            case -99: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                        }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định user cần xóa";
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
        public JsonResult UpdateSortOrder(int Id, bool SortOrder)
        {
            try
            {
                var updateResult = new ShopBO().UpdateOrder(Id, SortOrder);
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
        [HttpPost]
        public JsonResult UpdateStatus(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = new ShopBO().UpdateStatus(id);
                    if (result >= 0)
                    {

                        ReturnData.Description = "Cập nhật trạng thái Thành Công";
                    }
                    else switch (result)
                        {
                            case -50: ReturnData.Description = "Tài Khoản không tồn tại"; break;
                            case -99: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                        }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định trang cần active";
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
