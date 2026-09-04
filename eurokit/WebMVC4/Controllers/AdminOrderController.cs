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
    public class AdminOrderController : Controller
    {
        //
        // GET: /AdminOrder/

        public ActionResult Index()
        {
            var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var toDate = DateTime.Now;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            return View();
        }
        public ActionResult ListOrder(int? status, string Name, int? currentPage, int? pageSize, int? region, string fromDate, string endDate)
        {
          
            var data = new List<Order>();

            int TotalRecord = 0;
            int Status = status == null ? -1000 : (int)status;
            int Region = region == null ? -1 : (int)region;
           
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;
            data =new OrderBO().GetOrdersPaged(Name, Status,  CurrPage, RecordPerPage, fromDate, endDate,ref TotalRecord);
            if (data.Count > 0)
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

            var data = new OrderBO().GetOrder(PageID);
           

          
            var model = new OrderModel
            {
                Order = data

            };
            model.ListProduct = JsonConvert.DeserializeObject<List<OrderProductMapping_Full>>(data.APIRespone);


            ViewBag.id = Id;
            
            return View(model);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id > 0)
                {
                    var result = new OrderBO().DeleteOrder(id);
                    if (result >= 0)
                    {
                        ReturnData.Description = "Cập nhật trạng thái Thành Công";
                    }
                    else switch (result)
                        {
                            case -1: ReturnData.Description = "Không thể xóa sản phẩm này vì đã có đơn hàng tồn tại"; break;
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
                ExHandler.Handle(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveData(int id)
        {
            var ReturnData = new ReturnData();
          
            try
            {

                var data = new OrderBO().GetOrder(id);
                if (data == null)
                {
                    ReturnData.Description = "Không tồn tại đơn hàng này";
                    ReturnData.ResponseCode = -1;
                    return Json(ReturnData);
                }
               
                //trạng thái mới
                var newStatus = 2;
                //nếu đang là giao hàng=>đã giao hàng
                if (data.Status == 0)
                {
                    newStatus = 1;
                }
                data.Status = newStatus;


                var result = new OrderBO().Confirm(data);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (data.Id > 0)
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
                ExHandler.Handle(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }
    }
}
