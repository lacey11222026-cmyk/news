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
    public class AdminProductController : Controller
    {
        //
        // GET: /AdminProduct/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListProduct(int? status, int? categoryId, int? manufactoryId, string Name, int? currentPage, int? pageSize, string lang)
        {

            var data = new List<Product_Full>();
            if (lang == "0")
                lang = "";

            int TotalRecord = 0;
            int Status = status == null ? -1 : (int)status;
            int ManufactoryId = manufactoryId == null ? -1 : (int)manufactoryId;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;
            data = new ProductBO().GetProductsPaged(Name, categoryId.GetValueOrDefault(), ManufactoryId, CurrPage, RecordPerPage, ref TotalRecord, status, null, null, lang);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;
            ViewBag.CategoryList = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.Product);
            ViewBag.categoryId = categoryId.GetValueOrDefault();
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
                    var result = new ProductBO().DeleteProduct(id);
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
            int PageID = Id == null ? 0 : (int)Id;


            var model = new Product_Full
            {
                Description = "",
                Intro = "",
                QRImage = "",
                
                Price = 1000000,
                PriceReal = 1000000,
                Status = 1,
                CategoryId = 39,
                ManufactoryId = 0,
                AvailableSell = true
            };
            model.ImageParam = new List<ProductFileInfo>();
            if (PageID > 0)
            {
                model = new ProductBO().GetProductFull(PageID);
                var cateobj = new CategoryBO().GetCategoryFull(model.CategoryId.GetValueOrDefault());
                model.Description = cateobj.Url + "-" + model.Id;
                try
                {
                    model.ImageParam = JsonConvert.DeserializeObject<List<ProductFileInfo>>(model.Album);
                }
                catch
                {

                    model.ImageParam = new List<ProductFileInfo>();
                }
                if (model.ImageParam == null)
                {
                    model.ImageParam = new List<ProductFileInfo>();
                }
                model.ProductParam = JsonConvert.DeserializeObject<ProductParam>(model.Config);
            }
            else
            {

                model.ProductParam = new ProductParam();
            }
            ViewBag.id = Id;

            if (Id > 0)
            {
                ViewBag.Title = "Cập nhật sản phẩm";
            }
            else
            {
                ViewBag.Title = "Thêm mới sản phẩm";
            }
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveData(Product_Full Product)
        {
            var returnData = new ReturnData();

            try
            {

                if (string.IsNullOrEmpty(Product.Name))
                {
                    returnData.ResponseCode = -6001;
                    returnData.Description = "Bạn chưa nhập tên";
                    return Json(returnData);
                }
                //Product.Description = string.IsNullOrEmpty(Product.Description) ? " " : Product.Description;
                Product.QRImage = string.IsNullOrEmpty(Product.QRImage) ? " " : Product.QRImage;
                Product.Config = Utils.ConvertToJson(Product.ProductParam, string.Empty);
                var action = 0;
                if (Product.Id > 0)
                {
                    action = 1;
                    var cateobj = new CategoryBO().GetCategoryFull(Product.CategoryId.GetValueOrDefault());
                    Product.Description =cateobj.Url +"-" + Product.Id;
                }    
                   
                var result = new ProductBO().CreateUpdateProduct(Product);
                returnData.ResponseCode = result;
                if (action == 1)
                    returnData.ResponseCode = 0;
                if (result >= 0)
                {
                    returnData.Description = Product.Id > 0 ? "Cập nhật Thành Công" : "Thêm mới Thành Công";

                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)UTILS.Constants.CategoryType.Product,
                        ItemId = Product.Id,
                        ItemName = Product.Name,
                        Note = "Update sản phẩm",
                        Type = 1

                    };
                    lognewsobj.Note = Product.Id > 0 ? "Update sản phẩm" : "Tạo mới sảnphaamr";

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
                ExHandler.Handle(ex);
                returnData.ResponseCode = -99;
                returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                return Json(returnData);
            }
        }
        [HttpPost]
        public JsonResult UpdateSortOrder(int Id, bool SortOrder)
        {
            try
            {
                var updateResult = new ProductBO().UpdateOrder(Id, SortOrder);
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
        public JsonResult UpdateSortOrderTop(int Id)
        {
            try
            {
                var updateResult = new ProductBO().UpdateOrderTop(Id);
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
            var returnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = new ProductBO().UpdateStatus(id);
                    if (result >= 0)
                    {

                        returnData.Description = "Cập nhật trạng thái Thành Công";
                    }
                    else switch (result)
                        {
                            case -50: returnData.Description = "Tài Khoản không tồn tại"; break;
                            case -99: returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
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
        [HttpPost]
        public JsonResult SetHot(int id)
        {
            var returnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = new ProductBO().SetHot(id);
                    if (result >= 0)
                    {

                        returnData.Description = "Cập nhật trạng thái Thành Công";
                    }
                    else switch (result)
                        {
                            case -50: returnData.Description = "Tài Khoản không tồn tại"; break;
                            case -99: returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
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
        [HttpPost]
        public JsonResult SetNew(int id)
        {
            var returnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = new ProductBO().SetNew(id);
                    if (result >= 0)
                    {

                        returnData.Description = "Cập nhật trạng thái Thành Công";
                    }
                    else switch (result)
                        {
                            case -50: returnData.Description = "Tài Khoản không tồn tại"; break;
                            case -99: returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
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
        [HttpPost]
        public JsonResult SetSell(int id)
        {
            var returnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = new ProductBO().SetSell(id);
                    if (result >= 0)
                    {

                        returnData.Description = "Cập nhật trạng thái Thành Công";
                    }
                    else switch (result)
                        {
                            case -50: returnData.Description = "Tài Khoản không tồn tại"; break;
                            case -99: returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
                            default: returnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau"; break;
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

        public JsonResult GetListCate(string lang)
        {
            if (lang == "0")
                lang = "";
            var data = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.Product);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
    }
}
