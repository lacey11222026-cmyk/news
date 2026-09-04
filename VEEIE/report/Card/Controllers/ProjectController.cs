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
using System.Globalization;

namespace Car.CMS.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IUsersService _userservice;
        private readonly IUsersLogService _userlogservice;
        private readonly IFucntionsService _functionservice;
        private readonly IUserRoleService _userroleservice;
        private readonly IProjectsService _projectservice;
        private UserSession CurrentUser { get { return ((UserSession)Session[SessionsManager.SESSION_USER]); } }
        private Users CurrentFullUser { get { return ((Users)Session[SessionsManager.SESSION_USER_FULL]); } }
        public ProjectController(IProjectsService projectservice, IUsersService userservice, IUsersLogService userlogservice, IFucntionsService functionservice, IUserRoleService userroleservice)
        {
            _userservice = userservice;
            _userlogservice = userlogservice;
            _userroleservice = userroleservice;
            _functionservice = functionservice;
            _projectservice = projectservice;
        }
        [PermissionFilter(FunctionCode = FunctionCode.Project)]
        public ActionResult Index()
        {
            ViewBag.Title = "Quản trị dự án";
            return View();
        }
        [PermissionFilter(FunctionCode = FunctionCode.Project)]
        public ActionResult ListProject(int? status)
        {

            var data = new List<Project>();
            data = _projectservice.GetList(CurrentUser.Username);
            return PartialView(data);
        }
        [PermissionFilter(FunctionCode = FunctionCode.Project)]
        public ActionResult Info(int? Id)
        {
            int PageID = Id == null ? 0 : (int)Id;
            var model = new Project
            {
                Status = 1,
                Time=DateTime.Now,
                UserName=CurrentUser.Username,
                Bank=CurrentFullUser.CreatedUser,
            };

            if (PageID > 0)
            {
                model = _projectservice.GetProject(PageID);
               
            }

            ViewBag.id = Id;

            if (Id > 0)
            {
                ViewBag.Title = "Cập nhật dự án";
            }
            else
            {
                ViewBag.Title = "Thêm mới dự án";
            }
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        [PermissionFilter(FunctionCode = FunctionCode.Project)]
        public JsonResult SaveData(Project Project)
        {
            var ReturnData = new ReturnData();

            try
            {
                //banner.ImageUrl = Config.UrlRoot + (string.IsNullOrEmpty(banner.ImageUrl) ? string.Empty : banner.ImageUrl.Substring(12));
              
                Project.Order = Convert.ToInt32(Project.Order);
                Project.UserName = CurrentUser.Username;
                var result = _projectservice.UpdateProject(Project);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (Project.Id > 0)
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
        public JsonResult Get(int id)
        {
            var ReturnData = _projectservice.GetProject(id);
            return Json(ReturnData);
        }
        [HttpPost]
        [PermissionFilter(FunctionCode = FunctionCode.Project)]
        public JsonResult UpdateSortOrder(int Id, bool SortOrder)
        {
            try
            {
                var updateResult = _projectservice.UpdateOrder(Id, SortOrder, CurrentUser.Username);
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
        [PermissionFilter(FunctionCode = FunctionCode.Project)]
        public JsonResult UpdateStatus(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = _projectservice.UpdateStatus(id);
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