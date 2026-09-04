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
    [Authorize(Roles = "Administrator,Album")]
    public class AdminAlbumController : Controller
    {
        //
        // GET: /AdminAlbum/
        private List<CATEGORY_FULL> _staticCategoryList;
        protected override void Initialize(RequestContext requestContext)
        {

            //_staticCategoryList = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.Album);
            //var lstcate = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.Album);
            //_staticCategoryList = new CategoryBO().GetCategoryByUserName(lstcate, "", true);
            _staticCategoryList = new List<CATEGORY_FULL>();
            base.Initialize(requestContext);
        }
        public ActionResult Index()
        {
            var lstCate = _staticCategoryList;
            lstCate.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Chọn chuyên mục--" });
            ViewBag.CategoryList = lstCate;
            return View();
        }
        public ActionResult ListAlbum(int? cateId, int? status, string title, int? currentPage, int? pageSize)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int TotalRecord = 0;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;
            int CateId = cateId == null ? -1 : (int)cateId;
            int Status = status == null ? -1 : (int)status;

            var data = new AlbumBO().GetAlbumsFuLLPaged(title, CateId, Status, CurrPage, RecordPerPage, ref TotalRecord);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;
            ViewBag.CategoryList = _staticCategoryList;
            return PartialView(data);
        }
        public ActionResult GetAlbumDetail(int Id = 0)
        {
            ViewBag.CategoryList = _staticCategoryList;
            var model = new Album_FULL { Id = 0, PublishDate = DateTime.Now };
            model.ImageParam = new ImageParam();
            ViewBag.FileImage = new List<SelectListItem>();
            if (Id > 0)
            {

                model = new AlbumBO().GetAlbum(Id);
                if (model == null)
                    return RedirectToAction("Index");
                try
                {
                    model.ImageParam = JsonConvert.DeserializeObject<ImageParam>(model.Images);
                }
                catch
                {

                    model.ImageParam = new ImageParam();
                }
                if (model.ImageParam == null)
                {
                    model.ImageParam = new ImageParam();
                }

                ViewBag.Title = "Cập nhật công trình";
            }
            else
            {
                ViewBag.Title = "Thêm mới công trình";
            }
            ViewBag.ImgUrl = UTILS.Utils.GetImageUrl(Id, UTILS.EntityName.Album, false);
            return View(model);
        }
        public ActionResult Reference(string ids)
        {
            var lstCate = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.Product).Where(x=>x.ParentId==0).ToList(); 
            lstCate.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Chọn chuyên mục--" });
            ViewBag.CategoryList = lstCate;
            ViewBag.ids = ids;
            return PartialView();
        }
        public ActionResult FileInfo(int Id)
        {
            ViewBag.ImagesUrl = UTILS.Utils.GetImageUrl(Id, UTILS.EntityName.Album, false);
            ViewBag.Id = Id;
            return PartialView();
        }
        public ActionResult SaveFileUpload(int id)
        {
            //bool isSavedSuccessfully = true;
            string fName = "";
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                //Save file content goes here
                fName = file.FileName;
                if (file != null && file.ContentLength > 0)
                {
                    StringBuilder strBuilder = new StringBuilder();
                    // divided 1000000 files in folder               
                    strBuilder.Append(Request.PhysicalApplicationPath).Append(ConfigurationManager.AppSettings["UploadPath"]).Append("Album").Append("\\").Append(id / 100000).Append("\\").Append(id / 100).Append("\\").Append(id).Append("\\");

                    string pathString = strBuilder.ToString();

                    //var fileName1 = Path.GetFileName(file.FileName);
                     var fileName1 = UTILS.Utils.ReplaceVietnameseChar(Path.GetFileNameWithoutExtension(file.FileName)).Replace(")", "").Replace("(","").Replace(" ", "_");
                    //fileName1 = UTILS.Utils.SubString(fileName1, 30);
                    var extend1 = Path.GetExtension(file.FileName);
                    bool isExists = System.IO.Directory.Exists(pathString);

                    if (!isExists)
                        System.IO.Directory.CreateDirectory(pathString);

                    var path = string.Format("{0}\\{1}", pathString, fileName1 + extend1);
                    if (System.IO.File.Exists(path))
                    {
                        path = string.Format("{0}\\{1}", pathString, fileName1 + new Random().Next(10000) + extend1);
                    }
                    file.SaveAs(path);


                }

            }
            return Json(new { Message = fName });
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public JsonResult SaveData(Album_FULL doc, ImageParam ImageParam)
        {
            var ReturnData = new ReturnData();
            try
            {
                //doc.CreatedBy = HttpContext.User.Identity.Name;
                //var listImages = "";
                //if (images != null)
                //{
                //    foreach (var image in images)
                //    {
                //        listImages += image + ",";
                //    }

                //}
                //listImages = "[" + listImages.TrimEnd(',') + "]";
                //doc.Images = JsonConvert.SerializeObject(images);
                IFormatProvider culture = new CultureInfo("en-US", true);
                doc.PublishDate = DateTime.ParseExact(doc.SPublishDate, "dd/MM/yyyy HH:mm", culture);
                doc.Images = Utils.ConvertToJson(ImageParam, string.Empty);
                var result = new AlbumBO().CreateUpdateAlbum(doc);

                if (result >= 0)
                {
                    //var lognewsobj = new ContentLog
                    //{
                    //    UserName = HttpContext.User.Identity.Name,
                    //    ItemtType = (int)Constants.CategoryType.Album,
                    //    ItemId = doc.Id,
                    //    ItemName = doc.Title,
                    //    Note = "Xóa công trình",
                    //    Type = 1

                    //};
                    if (doc.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                        //lognewsobj.Note = "Update album";
                        ReturnData.ResponseCode = 0;
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        //lognewsobj.Note = "Tạo mới công trình";
                        ReturnData.ResponseCode = 1;
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
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UpdateStatus(string _id, string Title)
        {
            int Id = int.Parse(Utils.Base64Decode(_id));
            var ReturnData = new ReturnData();
            try
            {
                if (Id > 0)
                {
                    var obj = new AlbumBO().GetAlbum(Id);
                    if (obj != null)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.Album,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Duyệt album",
                            Type = 1

                        };
                        if (obj.Status == 0)
                        {
                            obj.Status = 1;
                        }
                        else
                        {
                            obj.Status = 0;
                            lognewsobj.Note = "Khóa album";
                        }
                        new AlbumBO().CreateUpdateAlbum(obj);
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Cập nhật album Thành Công";
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
                    var result = new AlbumBO().DeleteAlbum(Id);
                    if (result >= 0)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.Album,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Xóa album",
                            Type = 1

                        };
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Xóa album Thành Công";
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
        private List<SelectListItem> GetAlbumImage(int id)
        {
            if (id <= 0)
                return null;
            var strBuilder = new StringBuilder();
            // divided 1000000 files in folder               
            strBuilder.Append(Request.PhysicalApplicationPath).Append(ConfigurationManager.AppSettings["UploadPath"]).Append("Album").Append("\\").Append(Convert.ToInt32(id / 100000)).Append("\\").Append(Convert.ToInt32(id) / 100).Append("\\").Append(id).Append("\\");

            var upload_path = strBuilder.ToString();

            // if folder not exist => create folder follow rule
            if (!Directory.Exists(upload_path))
            {
                return null;
            }
            // get all file in avarta of club 
            var fileList = (from file in Directory.GetFiles(upload_path) select file.Replace(upload_path, "").Trim()).ToList();
            fileList = fileList.Where(x => x.Split('.').Length == 2).ToList();


            var imgurl = UTILS.Utils.GetImageUrl(id, UTILS.EntityName.Album, false);
            var list = fileList.Select(x => new SelectListItem()
            {
                Value = x.ToString(),
                Text = String.Format("{0}{1}", imgurl, x),
            }).ToList();

            return list;
        }
    }
}
