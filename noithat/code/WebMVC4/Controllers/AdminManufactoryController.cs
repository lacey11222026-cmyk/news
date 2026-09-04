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
    public class AdminManufactoryController : Controller
    {
        //
        // GET: /AdminManufactory/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListManufactory(int? categoryId, int? status, string Name, int? currentPage, int? pageSize)
        {

            var data = new List<MANUFACTORY_FULL>();


            int TotalRecord = 0;
            int Status = status == null ? -1 : (int)status;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;
            data = new ManufactoryBO().GetAllManufactoryFullPaged(categoryId.GetValueOrDefault(),CurrPage, RecordPerPage, Status, ref TotalRecord);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            ViewBag.CategoryList = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.Product);
            return PartialView(data);
        }
        public ActionResult ListCategoryManufactory(int CategoryId)
        {
            var lstmanu = new ManufactoryBO().GetAllManufactoryFulls(-1);
            var lstcatemanu = new CategoryManufactoryBO().GetByCateId(CategoryId);
            var model = new CategoryManufactoryModel
            {
                Manufactory = lstmanu,
                CategoryManufactory = lstcatemanu
            };
            ViewBag.CategoryId = CategoryId;
            return PartialView(model);
        }
        public ActionResult ListCategoryManufactory2(int ManuId)
        {
            var lstcate = new CategoryBO().GetAllCategoriesFull(0);
            var lstcatemanu = new CategoryManufactoryBO().GetByManuId(ManuId);
            var model = new CategoryManufactoryModel
            {
                Category = lstcate.Where(x=>x.ParentId==0).ToList(),
                CategoryManufactory = lstcatemanu
            };
            ViewBag.ManuId = ManuId;
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id > 0)
                {
                    var result = new ManufactoryBO().DeleteManufactory(id);
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
        public ActionResult AddEdit(int? Id)
        {
            int PageID = Id == null ? 0 : (int)Id;


            var model = new MANUFACTORY_FULL
            {
                Description = "",
                Params=""
            };

            if (PageID > 0)
            {
                model = new ManufactoryBO().GetManufactoryFull(PageID);

            }
            ViewBag.id = Id;

            if (Id > 0)
            {
                ViewBag.Title = "Cập nhật model";
            }
            else
            {
                ViewBag.Title = "Thêm mới model";
            }
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveData(MANUFACTORY_FULL Manufactory)
        {
            var ReturnData = new ReturnData();

            try
            {

                if (string.IsNullOrEmpty(Manufactory.Title))
                {
                    ReturnData.ResponseCode = -6001;
                    ReturnData.Description = "Bạn chưa nhập tên";
                    return Json(ReturnData);
                }
                Manufactory.Description = string.IsNullOrEmpty(Manufactory.Description) ? " " : Manufactory.Description;
                Manufactory.Params = string.IsNullOrEmpty(Manufactory.Params) ? " " : Manufactory.Params;
                Manufactory.Image = string.IsNullOrEmpty(Manufactory.Image) ? " " : Manufactory.Image;
                var result = new ManufactoryBO().CreateUpdateManufactory(Manufactory);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (Manufactory.Id > 0)
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
        [HttpPost]
        public JsonResult UpdateSortOrder(int Id, bool SortOrder)
        {
            try
            {
                var updateResult = new ManufactoryBO().UpdateOrder(Id, SortOrder);
                if (updateResult >= 0)
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
                ExHandler.Handle(ex);
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
                    var result = new ManufactoryBO().UpdateStatus(id);
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
                ExHandler.Handle(ex);
                ReturnData.ResponseCode = -99;
                ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(ReturnData);
            }
        }
        [HttpPost]
        public ActionResult InsertCategoryManufactory(int cateid,int manuid)
        {

            var result = "";
            if (new CategoryManufactoryBO().CreateUpdateCategoryManufactory(cateid,manuid) > 0)
            {

                result = "Thêm dữ liệu thành công";
            }
            else
            {

                result = "Thêm dữ liệu không thành công";
            }
            return Json(result);

        }
        [HttpPost]
        public ActionResult DeleteCategoryManufactory(int id)
        {
            
            var result="";
            if (new CategoryManufactoryBO().DeleteCategoryManufactory(id)>=0)
            {

                result = "Xóa dữ liệu thành công";
            }
            else
            { 

                result = "Xóa dữ liệu không thành công";
            }
            return Json(result);

        }
    }
}
