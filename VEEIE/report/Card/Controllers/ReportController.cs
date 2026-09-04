using Car.Data.DTO;
using Car.Data.Service;
using Car.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Car.CMS.Filter;
using Car.CMS.Models;
using Car.Data.Api;
using System.Text.RegularExpressions;
using Car.Data;
using Newtonsoft.Json;
using System.Globalization;

namespace Car.CMS.Controllers
{
    public class ReportController : Controller
    {
        private readonly IUsersService _userservice;
        private readonly IUsersLogService _userlogservice;
        private readonly IFucntionsService _functionservice;
        private readonly IUserRoleService _userroleservice;
        private readonly IProjectsService _projectservice;
        private readonly IProjectReportsService _projectreportservice;
        private UserSession CurrentUser { get { return ((UserSession)Session[SessionsManager.SESSION_USER]); } }
        private Users CurrentFullUser { get { return ((Users)Session[SessionsManager.SESSION_USER_FULL]); } }
        public ReportController(IProjectReportsService projectreportservice, IProjectsService projectservice, IUsersService userservice, IUsersLogService userlogservice, IFucntionsService functionservice, IUserRoleService userroleservice)
        {
            _userservice = userservice;
            _userlogservice = userlogservice;
            _userroleservice = userroleservice;
            _functionservice = functionservice;
            _projectservice = projectservice;
            _projectreportservice = projectreportservice;
        }
        [PermissionFilter(FunctionCode = FunctionCode.Report)]
        public ActionResult Index()
        {
            ViewBag.Title = "Quản trị báo cáo";
            return View();
        }
        [PermissionFilter(FunctionCode = FunctionCode.Report)]
        public ActionResult ListProjectReport(int? type, int? year, string keyword)
        {
            int Type = type == null ? -1 : (int)type;
            int Year = year == null ? -1 : (int)year;
            var data = new List<ProjectReport>();

            if (CurrentFullUser.Type == 1)
            {
                data = _projectreportservice.GetList("", "", Year, Type, -1, keyword);
            }
            if (CurrentFullUser.Type == 3)
            {
                data = _projectreportservice.GetList("", CurrentUser.Username, Year, Type, 1, keyword);
            }
            if (CurrentFullUser.Type == 4)
            {
                data = _projectreportservice.GetList(CurrentUser.Username, "", Year, Type, -1, keyword);
            }
            return PartialView(data);
        }
        [PermissionFilter(FunctionCode = FunctionCode.Report)]
        public ActionResult Info(int? Id)
        {
            int PageID = Id == null ? 0 : (int)Id;
            var model = new ProjectReportFull
            {
                Status = 1,
                UserName = CurrentUser.Username,
                Stuck = "",
                Job1 = "",
                Job2 = "",
                Job3 = "",
                Time = DateTime.Now,
                FileParam = new FileInfo()
            };
            var lstProject = _projectservice.GetList(CurrentUser.Username);
            if (lstProject.Count == 0)
                lstProject.Insert(0, new Project { Id = 0, Name = "--Chọn dự án--", });
            ViewBag.ProjectList = lstProject;
            if (PageID > 0)
            {
                var currentobj = _projectreportservice.GetProjectReport(PageID);
                model.Data = _projectservice.GetProject(currentobj.ProjectId.GetValueOrDefault());

                model.Bank = currentobj.Bank;
                model.Id = currentobj.Id;
                model.Result1 = currentobj.Result1;
                model.Result2 = currentobj.Result2;
                model.Job1 = currentobj.Job1 + "";
                model.Job2 = currentobj.Job2 + "";
                model.Job3 = currentobj.Job3 + "";
                model.ProjectId = currentobj.ProjectId;
                model.ProjectInfo = currentobj.ProjectInfo;
                model.Name = currentobj.Name;
                model.UserName = currentobj.UserName;
                model.Year = currentobj.Year;
                model.Type = currentobj.Type;
                model.Status = currentobj.Status;
                model.Order = currentobj.Order;
                model.Time = currentobj.Time;
                model.Stuck = currentobj.Stuck + "";
                model.NumberPeople = currentobj.NumberPeople;
                model.WomanRate = currentobj.WomanRate;
                model.FileData = currentobj.FileData;
            }

            else
            {
                model.Data = lstProject.Last();
                model.ProjectId = model.Data.Id;
            }

            try
            {
                model.FileParam = JsonConvert.DeserializeObject<FileInfo>(model.FileData);
            }
            catch
            {

                model.FileParam = new FileInfo();
            }
            if (model.FileParam == null)
            {
                model.FileParam = new FileInfo();
            }
         
            ViewBag.id = Id;

            if (Id > 0)
            {
                ViewBag.Title = "Cập nhật báo cáo";
            }
            else
            {
                ViewBag.Title = "Thêm mới báo cáo";
            }

            return View(model);
        }
        [PermissionFilter(FunctionCode = FunctionCode.Report)]
        public ActionResult Detail(int? Id)
        {
            int PageID = Id == null ? 0 : (int)Id;
            var model = new ProjectReportFull
            {
                Status = 1,
                UserName = CurrentUser.Username,
                FileParam = new FileInfo()
            };
            var lstProject = _projectservice.GetList(CurrentUser.Username);
            if (lstProject.Count == 0)
                lstProject.Insert(0, new Project { Id = 0, Name = "--Chọn dự án--" });
            ViewBag.ProjectList = lstProject;

            var currentobj = _projectreportservice.GetProjectReport(PageID);
            model.Data = _projectservice.GetProject(currentobj.ProjectId.GetValueOrDefault());

            model.Bank = currentobj.Bank;
            model.Id = currentobj.Id;
            model.Result1 = currentobj.Result1;
            model.Result2 = currentobj.Result2;
            model.Job1 = currentobj.Job1;
            model.Job2 = currentobj.Job2;
            model.Job3 = currentobj.Job3;
            model.ProjectId = currentobj.ProjectId;
            model.ProjectInfo = currentobj.ProjectInfo;
            model.Name = currentobj.Name;
            model.UserName = currentobj.UserName;
            model.Year = currentobj.Year;
            model.Type = currentobj.Type;
            model.Status = currentobj.Status;
            model.Order = currentobj.Order;
            model.Stuck = currentobj.Stuck;
            model.Time = currentobj.Time;
            model.NumberPeople = currentobj.NumberPeople;
            model.WomanRate = currentobj.WomanRate;
            model.FileData = currentobj.FileData;
            
            ViewBag.id = Id;
            try
            {
                model.FileParam = JsonConvert.DeserializeObject<FileInfo>(model.FileData);
            }
            catch
            {

                model.FileParam = new FileInfo();
            }
            if (model.FileParam == null)
            {
                model.FileParam = new FileInfo();
            }
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        [PermissionFilter(FunctionCode = FunctionCode.Report)]
        public JsonResult SaveData(ProjectReport ProjectReport, string STime, FileInfo FileParam)
        {
            var ReturnData = new ReturnData();

            try
            {
                //banner.ImageUrl = Config.UrlRoot + (string.IsNullOrEmpty(banner.ImageUrl) ? string.Empty : banner.ImageUrl.Substring(12));
                IFormatProvider culture = new CultureInfo("en-US", true);
                ProjectReport.Time = DateTime.ParseExact(STime, "dd/MM/yyyy", culture);
                ProjectReport.Order = Convert.ToInt32(ProjectReport.Order);
                ProjectReport.UserName = CurrentUser.Username;


                var project = _projectservice.GetProject(ProjectReport.ProjectId.GetValueOrDefault());
                ProjectReport.Bank = project.Bank;
                ProjectReport.Name = project.Name;
                ProjectReport.ProjectInfo = JsonConvert.SerializeObject(project);
                ProjectReport.FileData=JsonConvert.SerializeObject(FileParam);
                var result = _projectreportservice.UpdateProjectReport(ProjectReport);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {
                    if (ProjectReport.Id > 0)
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
        [PermissionFilter(FunctionCode = FunctionCode.Report)]
        [HttpPost]
        public JsonResult UpdateSortOrder(int Id, bool SortOrder)
        {
            try
            {
                var updateResult = _projectreportservice.UpdateOrder(Id, SortOrder, CurrentUser.Username);
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
        [PermissionFilter(FunctionCode = FunctionCode.Report)]
        public JsonResult UpdateStatus(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = _projectreportservice.UpdateStatus(id);
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
        [HttpPost]
        [PermissionFilter(FunctionCode = FunctionCode.Report)]
        public JsonResult Delete(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = _projectreportservice.Delete(id);
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