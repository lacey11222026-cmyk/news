using DATA.ContentDB;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Models;

using Newtonsoft.Json;
using BIZ;
using System.Linq;
namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Category")]
    public class AdminAuditorController : Controller
    {
        //
        // GET: /AdminQuestion/

        public ActionResult ManageAuditor()
        {
            ViewBag.Title = "Danh sách kiểm toán viên";
            return View();
        }

        public ActionResult ListAuditor(int? status, int? currentPage, int? pageSize, string lang, int? type)
        {

            var data = new List<Auditor>();

            int TotalRecord = 0;
            int Status = status == null ? -1 : (int)status;
            int Type = type == null ? -1 : (int)type;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;
            data = AuditorDAL.GetSearch(Status,"", CurrPage, RecordPerPage, ref TotalRecord,1,"");
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
            var obj = new AuditorFull
            {

            };

            if (PageID > 0)
            {
                var project = AuditorDAL.GetDetail(PageID);

                obj.Id = project.Id;
                obj.FullName = project.FullName;
                obj.Title = project.Title;
                obj.No = project.No;
                obj.Type = project.Type;
                obj.BirthDay = project.BirthDay;
                obj.Passport = project.Passport;
                obj.Nation = project.Nation;
                obj.Organ = project.Organ;
                obj.Order = project.Order;
                obj.Level = project.Level;
                obj.Organ = project.Organ;
                obj.MSDN = project.MSDN;
                obj.Role = project.Role;
                obj.Address = project.Address;
                obj.Mobile = project.Mobile;
                obj.Email = project.Email;
                obj.Group = project.Group;
                obj.Config = project.Config;
                obj.Status = project.Status;
                obj.Images = project.Images;
                obj.Province = project.Province;
                obj.Cate = 1;
                obj.ProjectConfig = JsonConvert.DeserializeObject<AuditorConfig>(obj.Config);
                if (string.IsNullOrEmpty(obj.ProjectConfig.MobileOffice))
                {
                    obj.ProjectConfig.MobileOffice = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.TrainingTime))
                {
                    obj.ProjectConfig.TrainingTime = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.IssueDate))
                {
                    obj.ProjectConfig.IssueDate = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.ExpirationDate))
                {
                    obj.ProjectConfig.ExpirationDate = " ";
                }
               
            }
            else
            {
                obj.ProjectConfig = new AuditorConfig { };
            }
           
            ViewBag.id = Id;

            if (Id > 0)
            {
                ViewBag.Title = "Cập nhật" ;
            }
            else
            {
                ViewBag.Title = "Thêm mới";
            }
            ViewBag.listLocation = new TestLocationBO().GetAllCache().Where(x => x.Name != "All").ToList();
            return View(obj);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveData(Auditor Auditor, AuditorConfig ProjectConfig)
        {
            var ReturnData = new ReturnData();

            try
            {
                //banner.ImageUrl = Config.UrlRoot + (string.IsNullOrEmpty(banner.ImageUrl) ? string.Empty : banner.ImageUrl.Substring(12));

                Auditor.Order = 0;

                if (string.IsNullOrEmpty(ProjectConfig.MobileOffice))
                {   
                
                    ProjectConfig.MobileOffice = " ";
                }
                if (string.IsNullOrEmpty(ProjectConfig.TrainingTime))
                {
                    ProjectConfig.TrainingTime = " ";
                }
                if (string.IsNullOrEmpty(ProjectConfig.IssueDate))
                {
                    ProjectConfig.IssueDate = " ";
                }
                if (string.IsNullOrEmpty(ProjectConfig.ExpirationDate))
                {
                    ProjectConfig.ExpirationDate = " ";
                }
                Auditor.Config = Utils.ConvertToJson(ProjectConfig, string.Empty);
                Auditor.Cate = 1;
                var result = AuditorDAL.InsertUpdate(Auditor);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (Auditor.Id > 0)
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
                    var result = AuditorDAL.Delete(id);
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
                var updateResult = AuditorDAL.UpdateSortOrder(Id, SortOrder);
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
                    var result = AuditorDAL.UpdateStatus(id);
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
