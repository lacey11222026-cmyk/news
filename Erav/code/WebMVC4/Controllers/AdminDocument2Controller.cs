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

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Document")]
    public class AdminDocument2Controller : Controller
    {

        private List<CATEGORY_FULL> _staticCategoryList;
        protected override void Initialize(RequestContext requestContext)
        {

            var lstcate = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.Doc);
            _staticCategoryList = new CategoryBO().GetCategoryByUserName(lstcate, "", true);
            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {
            var fromDate = new DateTime(DateTime.Now.Year, 1, 1);
            var toDate = DateTime.Now;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            var lstCate = _staticCategoryList.Where(x=>x.ParentId==69).ToList();
            lstCate.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Chọn chuyên mục--" });
            ViewBag.CategoryList = lstCate;

            return View();
        }
        public ActionResult ListDocument(int? cateId, int? status, string fromDate, string endDate, string title, int? currentPage, int? pageSize)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int TotalRecord = 0;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;
            int CateId = cateId == null ? -1 : (int)cateId;
            int Status = status == null ? -1 : (int)status;

            var data = new DocumentBO().GetDocumentsSearchPaged(title, CateId, 1,0,0,0,Status, CurrPage, RecordPerPage, fromDate, endDate, ref TotalRecord);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            ViewBag.CategoryList = _staticCategoryList;

            return PartialView(data);
        }
        public ActionResult GetDocumentDetail(int Id = 0)
        {
            ViewBag.CategoryList = _staticCategoryList.Where(x => x.ParentId == 69).ToList(); ;
            var model = new DOCUMENT_FULL { Id = 0 };
            if (Id > 0)
            {
                model = new DocumentBO().GetDocumentFull(Id);
                if (model == null)
                    return RedirectToAction("Index");
                var listcategory = _staticCategoryList;
                if (listcategory.Find(x => x.Id == model.CategoryId) == null)
                {
                    return RedirectToAction("Intro");
                }
                ViewBag.Title = "Cập nhật văn bản";
            }
            else
            {
                ViewBag.Title = "Thêm mới văn bản";
            }
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult SaveData(DOCUMENT_FULL doc)
        {
            
            var ReturnData = new ReturnData();
            try
            {
                doc.CreatedBy = HttpContext.User.Identity.Name;
                doc.DocType = 1;
                IFormatProvider culture = new CultureInfo("en-US", true);

                if (string.IsNullOrEmpty(doc.SEffectiveDate))
                {
                    doc.EffectiveDate = Utils.ConvertToDate("01/01/9999", "dd-MM-yyyy");
                }
                else
                {
                    doc.EffectiveDate = DateTime.ParseExact(doc.SEffectiveDate, "dd/MM/yyyy", culture);
                }
                if (string.IsNullOrEmpty(doc.SExpiryDate))
                {
                    doc.ExpiryDate = Utils.ConvertToDate("01/01/9999", "dd-MM-yyyy");
                }
                else
                {
                    doc.ExpiryDate = DateTime.ParseExact(doc.SExpiryDate, "dd/MM/yyyy", culture);
                }
                doc.PublishDate = DateTime.ParseExact(doc.SPublishDate, "dd/MM/yyyy", culture);
                var result = new DocumentBO().CreateUpdateDocument(doc);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {

                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)Constants.CategoryType.Doc,
                        ItemId = doc.Id,
                        ItemName = doc.Name,
                        Note = "Xóa văn bản",
                        Type = 1

                    };
                    if (doc.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                        lognewsobj.Note = "Update văn bản";
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        lognewsobj.Note = "Tạo mới văn bản";
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
                    var obj = new DocumentBO().GetDocumentFull(Id);
                    if (obj != null)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.Doc,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Duyệt văn bản",
                            Type = 1

                        };
                        if (obj.Status == 0)
                        {
                            obj.Status = 1;
                        }
                        else
                        {
                            obj.Status = 0;
                            lognewsobj.Note = "Khóa văn bản";
                        }
                        new DocumentBO().CreateUpdateDocument(obj);
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Cập nhật văn bản Thành Công";
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
                    var result = new DocumentBO().DeleteDocument(Id);
                    if (result >= 0)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.Doc,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Xóa văn bản",
                            Type = 1

                        };
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Xóa văn bản Thành Công";
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
