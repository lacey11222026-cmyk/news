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
    public class AdminPartController : Controller
    {
        //
        // GET: /AdminPart/

        public ActionResult Index()
        {
            return View();
        }
    
        public ActionResult ListPart(int? status, string Name, int? currentPage, int? pageSize)
        {
           
            var data = new List<Part>();
           

            int TotalRecord = 0;
            int Status = status ?? -1;
           
            int currPage = currentPage ?? 1;
            int recordPerPage = pageSize ?? 20;
            data = new PartBO().GetPartsPaged(currPage, recordPerPage, ref TotalRecord,Status, Name);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = recordPerPage;
            ViewBag.CurrentPage = currPage;
           
            return PartialView(data);
        }


        [HttpPost]
        public JsonResult Delete(int id)
        {
            var returnData = new ReturnData();
            try
            {
                
                if (id > 0)
                {
                    var result = new PartBO().DeletePart(id);
                    if (result >= 0)
                    {
                        returnData.Description = "Cập nhật trạng thái Thành Công";
                    }
                    else switch (result)
                        {
                            case -1: returnData.Description = "Không thể xóa sản phẩm này vì đã có đơn hàng tồn tại"; break;
                            case -50: returnData.Description = "Tài Khoản không tồn tại"; break;
                            case -99: returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                        }
                    return Json(returnData);
                }
                returnData.ResponseCode = -100;
                returnData.Description = "Không xác định user cần xóa";
                return Json(returnData);
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex);
                returnData.ResponseCode = -99;
                returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(returnData);
            }
        }
        public ActionResult AddEdit(int? Id)
        {
            int pageId = Id ?? 0;


            var model = new Part
            {
               
                Price = 1000000,
                Status = 1,
                
            };
           
            if (pageId > 0)
            {
                model = new PartBO().GetPart(pageId);
                
               
            }
            ViewBag.id = Id;

            ViewBag.Title = Id > 0 ? "Cập nhật phụ tùng" : "Thêm mới phụ tùng";
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveData(Part Part )
        {
            var returnData = new ReturnData();
           
            try
            {
               
                if (string.IsNullOrEmpty(Part.Name))
                {
                    returnData.ResponseCode = -6001;
                    returnData.Description = "Bạn chưa nhập tên";
                    return Json(returnData);
                }
                
                var result = new PartBO().CreateUpdatePart(Part);
                returnData.ResponseCode = result;

                if (result >= 0)
                {
                    returnData.Description = Part.Id > 0 ? "Cập nhật Thành Công" : "Thêm mới Thành Công";

                   
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
                ExHandler.Handle(ex);
                returnData.ResponseCode = -99;
                returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(returnData);
            }
        }
      
        [HttpPost]
        public JsonResult UpdateStatus(int id)
        {
            var returnData = new ReturnData();
            try
            {
                
                if (id >= 0)
                {
                    var obj = new PartBO().GetPart(id);
                    if (obj != null)
                    {

                        obj.Status = obj.Status == 1 ? 0 : 1;
                        new PartBO().CreateUpdatePart(obj);
                      
                        returnData.Description = "Cập nhật Thành Công";
                    }
                    else
                    {
                        returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                    }
                    return Json(returnData);
                }
                returnData.ResponseCode = -100;
                returnData.Description = "Không xác định trang cần active";
                return Json(returnData);
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex);
                returnData.ResponseCode = -99;
                returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(returnData);
            }
        }
        
    }
}
