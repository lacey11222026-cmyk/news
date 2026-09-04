using BIZ;
using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

        
        private void DownloadImage(string fromPath, string uri, string name)
        {
            var webClient = new WebClient();
            if (!Directory.Exists(fromPath))
                Directory.CreateDirectory(fromPath);
            if (!System.IO.File.Exists(fromPath + name + ".jpg"))
                webClient.DownloadFile(uri, fromPath + name + ".jpg");
        }
        public JsonResult LoadCrawl(string url)
        {

            var data = new Product_Full();

            var crawlctl = new CrawlBO();
            var webcontent = crawlctl.GetPage(url);
            data.Url = url.Replace("https://g7auto.vn/", "");
            var producthtml = crawlctl.getbyclass("details-product", "div", webcontent).Replace("\n", "").Replace("\t", "");
            data.Name = crawlctl.getbyclass("title-product", "h1", producthtml);
            data.Description = crawlctl.getbyclass("rte description  rte-summary", "div", producthtml);
            //data.Intro = crawlctl.getbyclass("price product-price", "span", producthtml).Replace(".", "").Replace("đ", "");
            var price = crawlctl.getbyclass("price product-price", "span", producthtml).Replace(".", "");
            var pricereal = crawlctl.getLastbyclass("price product-price", "span", producthtml).Replace(".", "");
            price = price.Remove(price.Length - 1);
            pricereal = pricereal.Remove(pricereal.Length - 1);
            data.Price = Decimal.Parse(price);
            data.PriceReal = Decimal.Parse(pricereal);
            data.ManuName = crawlctl.getbyclass("status_name", "span", producthtml);
            data.DescriptImage = crawlctl.getmetaTag("og:image", webcontent);
            data.ImageParam = new ImageParam();
            data.ImageParam.Path1 = data.DescriptImage;
            data.ImageParam.Path2 = crawlctl.getmetaTag("og:image", 1, webcontent);
            data.ImageParam.Path3 = crawlctl.getmetaTag("og:image", 2, webcontent);
            data.ImageParam.Path4 = crawlctl.getmetaTag("og:image", 3, webcontent);
            data.ImageParam.Path5 = crawlctl.getmetaTag("og:image", 4, webcontent);
            data.Intro = crawlctl.getbyId("tab-1", webcontent);
            

            data.Intro = data.Intro.Replace("https://g7auto.vn/hankook", "/lop-hankook");
            data.Intro = data.Intro.Replace("https://g7auto.vn/", "/");
            data.Intro = data.Intro.Replace("https://bizweb.dktcdn.net/100/366/403/files/", "/Images/Upload/Product/");
            data.Intro = data.Intro.Replace("http://bizweb.dktcdn.net/100/366/403/files/", "/Images/Upload/Product/");
            data.Intro = data.Intro.Replace("http://beta.vision-tech.vn/wp-content/uploads/2018/11/", "/Images/Upload/Product/");

            data.Intro = data.Intro.Replace("//bizweb.dktcdn.net/100/366/403/files/", "/Images/Upload/Product/");
            data.Intro = data.Intro.Replace("bao-gia-", "");
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListProduct(int? status, int? categoryId, int? manufactoryId, string Name, int? currentPage, int? pageSize)
        {

            var data = new List<Product>();
            Name = HttpUtility.UrlDecode(Name);

            int TotalRecord = 0;
            int Status = status == null ? -1 : (int)status;
            int ManufactoryId = manufactoryId == null ? -1 : (int)manufactoryId;
            string Manu = "";
            if (ManufactoryId > 0)
            {

                if (Config.ParentManu.Contains("," + ManufactoryId + ","))
                {
                    Manu = "," + ManufactoryId + ",";

                    foreach (var item in new ManufactoryBO().GetAllManufactoryFulls(ManufactoryId, -1, 1))
                    {
                        Manu += item.Id + ",";
                    }
                }

            }
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 50 : (int)pageSize;
            data = new ProductBO().GetProductsPaged(Name, categoryId.GetValueOrDefault(), ManufactoryId, Manu, -1, -1, CurrPage, RecordPerPage, ref TotalRecord, status, null, null, -1);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;
            ViewBag.CategoryList = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.Product);
            ViewBag.ManuList = new ManufactoryBO().GetAllManufactoryFulls(-1, -1, 1);
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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddEdit(int? Id)
        {
            int PageID = Id == null ? 0 : (int)Id;


            var model = new Product_Full
            {
                Description = " ",
                Intro = "",
                Tech = "",
                Volumn="",
                Language = "",//khoảng giá
                Price = 1000000,
                PriceReal = 1000000,
                Status = 1,
                UpdateTime = DateTime.Now,
                CategoryId = 0,
                IsNew = true,
                ManufactoryId = 0,
                AvailableSell = true,
                IsHot=false,
            };
            model.ImageParam = new ImageParam();
            model.ProParam = new ProParam();
            if (PageID > 0)
            {
                model = new ProductBO().GetProductFull(PageID);
                try
                {
                    model.ImageParam = JsonConvert.DeserializeObject<ImageParam>(model.Album);
                   
                }
                catch
                {

                    model.ImageParam = new ImageParam();
                }
                if (model.ImageParam == null)
                {
                    model.ImageParam = new ImageParam();
                }

                try
                {
                    model.ProParam = JsonConvert.DeserializeObject <ProParam > (model.Config);
                }
                catch
                {

                    model.ProParam = new ProParam();
                }
                if (model.ProParam == null)
                {
                    model.ProParam = new ProParam();
                }
            }
            ViewBag.id = Id;
            ViewBag.ManuList = new ManufactoryBO().GetAllManufactoryFulls(-1, -1, 1).OrderBy(x => x.Title).ToList();
            //ViewBag.CarModelList = new CarModelBO().GetTopLastestCarModel();
            //ViewBag.CarGroupList = new CarGroupBO().GetTopLastestCarGroup();
            //ViewBag.CarSizeList = new CarSizeBO().GetTopLastestCarSize(-1, -1, -1).OrderBy(x => x.Name).ToList(); ;
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
        public JsonResult SaveData(Product Product, ImageParam ImageParam,ProParam ProParam, string SPublishDate)
        {
            var returnData = new ReturnData();

            try
            {

                
                IFormatProvider culture = new CultureInfo("en-US", true);
                Product.UpdateTime = DateTime.ParseExact(SPublishDate, "dd/MM/yyyy HH:mm", culture);



                Product.Description = string.IsNullOrEmpty(Product.Description) ? " " : Product.Description;
                Product.Tech = string.IsNullOrEmpty(Product.Tech) ? " " : Product.Tech;
                Product.Album = Utils.ConvertToJson(ImageParam, string.Empty);
                Product.Config = Utils.ConvertToJson(ProParam, string.Empty);
                if (Product.Id == 0 && String.IsNullOrEmpty(Product.Url))
                {

                    Product.Url = Utils.ConvertToRewriteLink(Product.Name);
                }
                else
                {
                    Product.Url = Product.Url.Replace("https://g7auto.vn/", "");
                }
                if (string.IsNullOrEmpty(ImageParam.Path1))
                {
                   
                    ImageParam.Path1 = Product.DescriptImage;
                }
                var result = new ProductBO().CreateUpdateProduct(Product);
                returnData.ResponseCode = result;


                if (result >= 0)
                {
                    returnData.Description = Product.Id > 0 ? "Cập nhật Thành Công" : "Thêm mới Thành Công";

                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)UTILS.Constants.CategoryType.Product,
                        ItemId = Product.Id,
                        ItemName = Product.Name,
                        Note = "Xóa banner",
                        Type = 1

                    };
                    lognewsobj.Note = Product.Id > 0 ? "Update sản phẩm" : "Tạo mới sản phẩm";

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
                NLogLogger.PublishException(ex);
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
        [OutputCache(Duration = 120, VaryByParam = "*", VaryByCustom = "browser")]
        public JsonResult GetListManu(int CategoryId)
        {


            var data = new List<MANUFACTORY_FULL>();
            if(CategoryId<=0)
            {
                data = new ManufactoryBO().GetAllManufactoryFulls(-1, CategoryId, -1);
            }
            else
            {
                var cate = new CategoryBO().GetCategoryFull(CategoryId);
                if(cate.ParentId==0)
                {
                    data = new ManufactoryBO().GetAllManufactoryFulls(-1, CategoryId, -1);
                }
                else
                {

                    data = new ManufactoryBO().GetAllManufactoryFulls(-1, int.Parse(cate.Pathway.Split(',')[1]), -1);

                }
            }
            data = data.OrderBy(a=>a.ParentId).OrderBy(x => x.Title).ToList();

            //var listdata = new List<MANUFACTORY_FULL>();
            //foreach (var item in listdata)
            //{
            //    if (item.ParentId > 0)
            //    {
            //        var x1 = new MANUFACTORY_FULL { Id = item.Id, ParentId = item.ParentId, Title = item.Title };
            //        if (item.ParentId != 0)
            //        {
            //            x1.Title = "-+ " + x1.Title;
            //        }

            //        var pindex = listdata.Select((Value, Index) => new { Value, Index }).FirstOrDefault(x => x.Value.Id == x1.ParentId);

            //        if (pindex != null)
            //        {
            //            listdata.Insert(pindex.Index + 1, x1);

            //        }
            //        else
            //        {
            //            listdata.Add(item);
            //        }
            //    }
            //    else
            //    {
            //        listdata.Add(item);
            //    }
            //}
            if (data == null|| data.Count()==0)
                data.Add(new MANUFACTORY_FULL { Id = 0, Title = "-Chọn hãng" });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListCate(string lang)
        {
            if (lang == "0")
                lang = "";
            var data = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.Product, lang);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
    }
}
