using BIZ;
using BIZ.Entity;
using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Constants = UTILS.Constants;
using WebMVC4.Models;
using UTILS;
using Newtonsoft.Json;
using DATA.ContentDB;
namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Category,Customer")]
    public class AdminProjectController : Controller
    {
        //
        // GET: /AdminProject/

        public ActionResult ManageProject()
        {
            ViewBag.Title = "Danh sách dự án";
            var fromDate = new DateTime(DateTime.Now.Year, 1, 1);
            var toDate = DateTime.Now;
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;

            return View();
        }

        public ActionResult ListProject(int? type, string fromDate, string endDate, string title, int? status,string lang, int? currentPage, int? pageSize)
        {

            var data = new List<Project2>();
            int Type = type == null ? -1 : (int)type;
            //int TotalRecord = 0;
            int Status = status == null ? -1 : (int)status;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 100 : (int)pageSize;
            int TotalRecord = 0;
            data = Project2DAL.GetSearch(Status, Type, lang, title, CurrPage, RecordPerPage,ref TotalRecord, fromDate, endDate);
            if (data != null)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;

            ViewBag.lang = lang;
            
            return PartialView(data);
        }

        public ActionResult Info(int? Id)
        {
            int PageID = Id == null ? 0 : (int)Id;
            var data = new ProjectFull();
            var model = new Projects
            {
               
                Status = 1,
            };
            ViewBag.listLocation = new TestLocationBO().GetAllCache();
            var projectconfig = new ProjectConfig();
            if (PageID > 0)
            {
                model = ProjectsDAL.GetDetail(PageID);
                projectconfig = JsonConvert.DeserializeObject<ProjectConfig>(model.Config);


            }
            data.Project = model;
            data.ProjectConfig = projectconfig;
            ViewBag.id = Id;
            
            if (Id > 0)
            {
                ViewBag.Title = "Cập nhật dự án";
            }
            else
            {
                ViewBag.Title = "Thêm mới dự án";
            }
            return View(data);
        }
        public ActionResult AddProject(int Id = 0)
        {
            ViewBag.listLocation = new TestLocationBO().GetAllCache();

            var obj = new Project2Full();
            if (Id > 0)
            {
                var project = Project2DAL.GetDetail(Id);
               
              
                obj.Id = project.Id;
                obj.Name = project.Name;
                obj.Username = project.Username;
                obj.Location = project.Location;
                obj.Type = project.Type;
                obj.SubType = project.SubType;
                obj.Unit = project.Unit;
                obj.UnitIInfo = project.UnitIInfo;
                obj.Organ = project.Organ;
                obj.Total = project.Total;
                obj.Currency = project.Currency;
                obj.Detail = project.Detail;
                obj.Source = project.Source;
                obj.Progress = project.Progress;
                obj.LegalStatus = project.LegalStatus;
                obj.Description = project.Description;
                obj.Impact = project.Impact;
                obj.Document = project.Document;
                obj.Rule1 = project.Rule1;
                obj.Rule2 = project.Rule2;
                obj.Rule3 = project.Rule3;
                obj.Rule4 = project.Rule4;
                obj.Config = project.Config;
                obj.Username = project.Username;
                obj.Status = project.Status;
                obj.ProjectConfig = JsonConvert.DeserializeObject<UserProjectConfig>(obj.Config);
                if (string.IsNullOrEmpty(obj.ProjectConfig.TADetail))
                {
                    obj.ProjectConfig.TADetail = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Time))
                {
                    obj.ProjectConfig.Time = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Finish))
                {
                    obj.ProjectConfig.Finish = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Support))
                {
                    obj.ProjectConfig.Support = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Rate))
                {
                    obj.ProjectConfig.Rate = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Revenue))
                {
                    obj.ProjectConfig.Revenue = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Finance))
                {
                    obj.ProjectConfig.Finance = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.UnitDev))
                {
                    obj.ProjectConfig.UnitDev = " ";
                }

                if (string.IsNullOrEmpty(obj.ProjectConfig.Role))
                {
                    obj.ProjectConfig.Role = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Email))
                {
                    obj.ProjectConfig.Email = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Mobile))
                {
                    obj.ProjectConfig.Mobile = " ";
                }
                if (string.IsNullOrEmpty(obj.ProjectConfig.Fullname))
                {
                    obj.ProjectConfig.Fullname = " ";
                }
               
                if (obj.Type == 2)
                {
                    return View("AddProject2", obj);
                }
            }
            else
            {
                obj.Currency = "";
                obj.Id = 0;

                obj.ProjectConfig = new UserProjectConfig { TA = -1 };
            }
            if (Id > 0)
            {
                ViewBag.Title = "Cập nhật dự án";
            }
            else
            {
                ViewBag.Title = "Thêm mới dự án";
            }
            return View(obj);
        }
        public ActionResult AddProject2(int Id = 0)
        {
            ViewBag.listLocation = new TestLocationBO().GetAllCache();
            var obj = new Project2Full();
            if (Id > 0)
            {
                var project = Project2DAL.GetDetail(Id);
                if (project == null)
                {
                    return RedirectToAction("Project");
                }
               
                obj.Id = project.Id;
                obj.Name = project.Name;
                obj.Username = project.Username;
                obj.Location = project.Location;
                obj.Type = project.Type;
                obj.SubType = project.SubType;
                obj.Unit = project.Unit;
                obj.UnitIInfo = project.UnitIInfo;
                obj.Organ = project.Organ;
                obj.Total = project.Total;
                obj.Currency = project.Currency;
                obj.Detail = project.Detail;
                obj.Source = project.Source;
                obj.Progress = project.Progress;
                obj.LegalStatus = project.LegalStatus;
                obj.Description = project.Description;
                obj.Impact = project.Impact;
                obj.Document = project.Document;
                obj.Rule1 = project.Rule1;
                obj.Rule2 = project.Rule2;
                obj.Rule3 = project.Rule3;
                obj.Rule4 = project.Rule4;
                obj.Config = project.Config;
                obj.Username = project.Username;
                obj.Status = project.Status;
                obj.ProjectConfig = JsonConvert.DeserializeObject<UserProjectConfig>(obj.Config);
                if (obj.Type == 1)
                {
                    return View("AddProject", obj);
                }

            }
            else
            {
                obj.Currency = "";
                obj.Id = 0;

                obj.ProjectConfig = new UserProjectConfig { TA = -1 };
            }
            if (Id > 0)
            {
                ViewBag.Title = "Cập nhật dự án";
            }
            else
            {
                ViewBag.Title = "Thêm mới dự án";
            }
            return View(obj);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveData(Project2 Project, UserProjectConfig ProjectConfig)
        {
            var ReturnData = new ReturnData();

            try
            {
                //banner.ImageUrl = Config.UrlRoot + (string.IsNullOrEmpty(banner.ImageUrl) ? string.Empty : banner.ImageUrl.Substring(12));

                //Project.Order = 1;
                //Project.SystemType = 1;
                Project.Config = Utils.ConvertToJson(ProjectConfig, string.Empty);
                var result = Project2DAL.InsertUpdate(Project);
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
        public JsonResult Delete(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id > 0)
                {
                    var result = Project2DAL.Delete(id);
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
       
       


    }
}
