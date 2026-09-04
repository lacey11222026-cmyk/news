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
using DATA.SMS;

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Contact")]
    public class AdminContactController : Controller
    {
        private List<CATEGORY_FULL> _staticCategoryList;
        protected override void Initialize(RequestContext requestContext)
        {

            var lstcate = new CategoryBO().GetAllCategoriesFull(Constants.CategoryType.Contact);
            _staticCategoryList = new CategoryBO().GetCategoryByUserName(lstcate, "", true);
            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {
            var lstCate = _staticCategoryList;
            //lstCate.Insert(0, new CATEGORY_FULL { Id = 0, Name = "--Chọn chuyên mục--" });
            ViewBag.CategoryList = lstCate;
            return View();
        }
        public ActionResult ListContact(int? cateId, int? status, string title, int? currentPage, int? pageSize)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            //int TotalRecord = 0;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 1000 : (int)pageSize;
            int CateId = cateId == null ? -1 : (int)cateId;
            int Status = status == null ? -1 : (int)status;

            var data = new ContactBO().GetContactsByCategory(CateId, Status, title);
            ViewBag.TotalRecord = data.Count;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            ViewBag.CategoryList = _staticCategoryList;

            return PartialView(data);
        }
        public ActionResult GetContactDetail(int Id = 0)
        {
            ViewBag.CategoryList = _staticCategoryList;
            var model = new Contact { Id = 0 };
            if (Id > 0)
            {
                model = new ContactBO().GetContact(Id);
                if (model == null)
                    return RedirectToAction("Index");
                ViewBag.Title = "Cập nhật danh bạ";
            }
            else
            {
                ViewBag.Title = "Thêm mới danh bạ";
            }
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult SaveData(Contact doc)
        {
            var ReturnData = new ReturnData();
            try
            {

                var result = new ContactBO().CreateUpdateContact(doc);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {

                    var lognewsobj = new ContentLog
                    {
                        UserName = HttpContext.User.Identity.Name,
                        ItemtType = (int)Constants.CategoryType.Contact,
                        ItemId = doc.Id,
                        ItemName = doc.Name,
                        Note = "Xóa danh bạ",
                        Type = 1

                    };
                    if (doc.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                        lognewsobj.Note = "Update danh bạ";
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        lognewsobj.Note = "Tạo mới danh bạ";
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
                    var obj = new ContactBO().GetContact(Id);
                    if (obj != null)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.Contact,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Duyệt danh bạ",
                            Type = 1

                        };
                        if (obj.Published == 0)
                        {
                            obj.Published = 1;
                        }
                        else
                        {
                            obj.Published = 0;
                            lognewsobj.Note = "Khóa danh bạ";
                        }
                        new ContactBO().CreateUpdateContact(obj);
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Cập nhật danh bạ Thành Công";
                    }
                    else
                    {
                        ReturnData.Description = "Hệ thống đang bận. Vui lòng quay lại sau";
                    }
                    return Json(ReturnData);
                }
                ReturnData.ResponseCode = -100;
                ReturnData.Description = "Không xác định danh bạ cần thao tác";
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
                    var result = new ContactBO().DeleteContact(Id);
                    if (result >= 0)
                    {
                        var lognewsobj = new ContentLog
                        {
                            UserName = HttpContext.User.Identity.Name,
                            ItemtType = (int)Constants.CategoryType.Contact,
                            ItemId = Id,
                            ItemName = Title,
                            Note = "Xóa danh bạ",
                            Type = 1

                        };
                        //Ghi log
                        Action<ContentLog> send = InsertContentLog;
                        var asynSend = send.BeginInvoke(lognewsobj, null, null);

                        ReturnData.Description = "Xóa danh bạ Thành Công";
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
        #region "Sendsms"
        public ActionResult SMSLog()
        {


            return View();
        }
        public ActionResult ListSMSLog(int? status, string admin, string title, int? currentPage, int? pageSize)
        {

            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int TotalRecord = 0;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 30 : (int)pageSize;
            int Status = status == null ? -1 : (int)status;
            var data = new SMSLogDAL().GetList(Status, title, admin, ConfigurationManager.AppSettings["SMSCODE"], CurrPage, RecordPerPage, ref TotalRecord);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            return PartialView(data);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SetSMSLog(string content, List<string> contact)
        {

            var ReturnData = new ReturnData();
            try
            {
                string joinId = string.Empty;
                foreach (var id in contact)
                {
                    if (Utils.IsNumber(id))
                        joinId += "," + id;
                }
                var lstContact = new ContactBO().GetContacts(joinId);
                var smsctrl = new SMSLogDAL();
                var smslog = new SMSLog { PartnerCode = ConfigurationManager.AppSettings["SMSCODE"], Message = content, Admin = HttpContext.User.Identity.Name, Ip = Utils.ClientIP, Status = 0 };
                foreach (var item in lstContact)
                {
                    try
                    {
                        smslog.Name = item.Name;
                        smslog.Mobile = item.Mobile;
                        smsctrl.InsertUpdate(smslog);
                    }
                    catch
                    {


                    }
                }
                ReturnData.Description = "Cập nhật danh bạ Thành Công";


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
        #endregion
        private void InsertContentLog(ContentLog lognewsobj)
        {
            new ContentLogBO().CreateUpdateContentLog(lognewsobj);
        }
    }
}
