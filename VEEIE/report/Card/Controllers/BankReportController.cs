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
using System.IO;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using Car.CMS.Helper;
using System.Globalization;

namespace Car.CMS.Controllers
{
    public class BankReportController : Controller
    {
        private readonly IUsersService _userservice;
        private readonly IUsersLogService _userlogservice;
        private readonly IFucntionsService _functionservice;
        private readonly IUserRoleService _userroleservice;
        private readonly IProjectsService _projectservice;
        private readonly IProjectReportsService _projectreportservice;

        private readonly IPlansService _planService;
        private readonly IPlanItemsService _planItemService;
        private readonly IPlanStucksService _planStuckService;
        private readonly IPlanRequiresService _planRequireService;
        private UserSession CurrentUser { get { return ((UserSession)Session[SessionsManager.SESSION_USER]); } }
        private Users CurrentFullUser { get { return ((Users)Session[SessionsManager.SESSION_USER_FULL]); } }
        public BankReportController(IPlanRequiresService planRequireService, IPlansService planService, IPlanItemsService planItemService, IPlanStucksService planStuckService, IProjectReportsService projectreportservice, IProjectsService projectservice, IUsersService userservice, IUsersLogService userlogservice, IFucntionsService functionservice, IUserRoleService userroleservice)
        {
            _userservice = userservice;
            _userlogservice = userlogservice;
            _userroleservice = userroleservice;
            _functionservice = functionservice;
            _projectservice = projectservice;
            _projectreportservice = projectreportservice;

            _planService = planService;
            _planItemService = planItemService;
            _planStuckService = planStuckService;

            _planRequireService = planRequireService;
        }
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public ActionResult Index()
        {
            ViewBag.Title = "Quản trị báo cáo";
            ViewBag.Type = CurrentFullUser.Type;
            return View();
        }
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public ActionResult View(int id)
        {

            var planObj = _planService.GetPlan(id);
            var title = HtmlHelpers.GetBankName(planObj.UserName) + " - ";
            if (planObj.Type == 5)
            {
                title += "Năm " + planObj.Year;
            }
            else
            {
                title += HtmlHelpers.GetReportType(planObj.Type.GetValueOrDefault()) + " Năm " + planObj.Year;
            }
            var model = new PlanDetail();
            model.PlanRequire = _planRequireService.GetList(id);
            model.PlanStuck = _planStuckService.GetList(id);
            model.PlanItem = new List<PlanItemModel>();
            var lstPlanItem = _planItemService.GetList(id).Where(x => x.Status == 1).ToList();
            foreach (var item in lstPlanItem)
            {
                var newItem = new PlanItemModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Total1 = item.Total1,
                    Total2 = item.Total2,
                    PlanId = item.PlanId,
                    Status = item.Status,
                    Time = item.Time,
                    NumberPeople = item.NumberPeople,
                    WomanRate = item.WomanRate,
                };
                newItem.Item1 = JsonConvert.DeserializeObject<PlanItemData>(item.Config1);
                newItem.Item2 = JsonConvert.DeserializeObject<PlanItemData>(item.Config2);
                newItem.Item3 = JsonConvert.DeserializeObject<PlanItemData>(item.Config3);
                model.PlanItem.Add(newItem);
            }
            model.Plan = planObj;
            ViewBag.Title = title;
            return View(model);
        }
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public ActionResult ListBankReport(int? type, int? year, string bank)
        {
            int Type = type == null ? -1 : (int)type;
            int Year = year == null ? -1 : (int)year;
            var data = new List<Plan>();
            //admin
            if (CurrentFullUser.Type == 1)
            {
                data = _planService.GetList(bank, Year, Type, -1);
            }
            //banquanly
            if (CurrentFullUser.Type == 2)
            {
                data = _planService.GetList(bank, Year, Type, 1);
            }
            //bank
            if (CurrentFullUser.Type == 3)
            {
                data = _planService.GetList(CurrentUser.Username, Year, Type, -1);
            }
            ViewBag.Type = CurrentFullUser.Type;
            return PartialView(data);
        }
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public ActionResult Add()
        {
            var model = new Plan
            {
                Status = 0,
                Name = "",
                Result = "",
                Result2 = "",
                Result3 = "",
                Result4 = "",
                Problem = "",
                WorkPlan = "",
                Type = 0,
                Year = 0,
                UserName = CurrentUser.Username
            };
            var data = _planService.GetList(CurrentUser.Username, -1, -1, -1).Where(x => x.Name == "").ToList();
            if (data.Count > 0)
            {
                return RedirectToAction("Info", new { id = data.FirstOrDefault().Id });
            }
            var result = _planService.CreateUpdatePlan(model);
            return RedirectToAction("Info", new { id = result });
        }
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public ActionResult Info(int? Id)
        {
            int PageID = Id == null ? 0 : (int)Id;
            var model = new Plan
            {
                Status = 1,
                Result = "",
                Result2 = "",
                Result3 = "",
                Result4 = "",
                Problem = "",
                WorkPlan = "",

            };

            if (PageID > 0)
            {
                model = _planService.GetPlan(PageID);
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
        [ValidateInput(false)]
        [HttpPost]
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public JsonResult SaveData(Plan Plan)
        {
            var ReturnData = new ReturnData();

            try
            {

                Plan.Order = Convert.ToInt32(Plan.Order);
                Plan.UserName = CurrentUser.Username;
                if (Plan.Id > 0)
                {
                    var lstPlan = _planService.GetList(CurrentUser.Username, Plan.Year.GetValueOrDefault(), -1, -1);
                    if (lstPlan.Exists(x => x.Type == Plan.Type.GetValueOrDefault() && x.Id != Plan.Id))
                    {
                        ReturnData.ResponseCode = -1;
                        ReturnData.Description = "Đã tồn tại " + Plan.Name;
                        return Json(ReturnData);
                    }
                }

                var result = _planService.CreateUpdatePlan(Plan);
               
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (Plan.Id > 0)
                        ReturnData.Description = "Cập nhật Thành Công";
                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";


                    }



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
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public JsonResult UpdateSortOrder(int Id, bool SortOrder)
        {
            try
            {
                var updateResult = _planService.UpdateOrder(Id, SortOrder, CurrentUser.Username);
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
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public JsonResult UpdateStatus(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    //var result = _planService.UpdateStatus(id);
                    var report = _planService.GetPlan(id);
                    report.Status = 2;
                    var result = _planService.CreateUpdatePlan(report);
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
        [HttpGet]
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public ActionResult ExportExcel(int id)
        {

            var planObj = _planService.GetPlan(id);

            try
            {
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        var worksheet = xlPackage.Workbook.Worksheets.Add("Report");


                        //độ rộng cột
                        worksheet.Column(1).Width = 20;
                        worksheet.Column(2).Width = 16;
                        worksheet.Column(3).Width = 16;
                        worksheet.Column(4).Width = 16;
                        worksheet.Column(5).Width = 16;
                        worksheet.Column(6).Width = 16;
                        worksheet.Column(7).Width = 16;
                        worksheet.Column(8).Width = 16;
                        worksheet.Column(9).Width = 16;
                        worksheet.Column(10).Width = 16;
                        worksheet.Column(11).Width = 16;
                        worksheet.Column(12).Width = 16;
                        worksheet.Column(13).Width = 16;
                        worksheet.Column(14).Width = 16;
                        worksheet.Column(15).Width = 16;
                        worksheet.Column(16).Width = 16;

                        var allCells = worksheet.Cells[1, 1, 50, 50];
                        var cellFont = allCells.Style.Font;
                        cellFont.SetFromFont(new Font("Times New Roman", 12));


                        worksheet.Cells["A1:D1"].Merge = true;
                        worksheet.Cells["A1:D1"].Value = HtmlHelpers.GetBankName(planObj.UserName);
                        worksheet.Cells["A1:D1"].Style.WrapText = true;

                        worksheet.Cells["G1:L1"].Merge = true;
                        worksheet.Cells["G1:L1"].Value = "Cộng hòa xã hội chủ nghĩa Việt Nam";
                        worksheet.Cells["G1:L1"].Style.WrapText = true;

                        worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;



                        worksheet.Cells["A2:D2"].Merge = true;
                        if (planObj.Type == 5)
                        {
                            worksheet.Cells["A2:D2"].Value = "Năm báo cáo: " + planObj.Year;
                        }
                        else
                        {
                            worksheet.Cells["A2:D2"].Value = "Kỳ báo cáo: " + HtmlHelpers.GetReportType(planObj.Type.GetValueOrDefault());
                        }

                        worksheet.Cells["A2:D2"].Style.WrapText = true;

                        worksheet.Cells["G2:L2"].Merge = true;
                        worksheet.Cells["G2:L2"].Value = "Độc lập - Tự do - Hạnh phúc";
                        worksheet.Cells["G2:L2"].Style.WrapText = true;

                        worksheet.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


                        worksheet.Cells["B4:H4"].Merge = true;
                        worksheet.Cells["B4:H4"].Value = "BÁO CÁO TÓM TẮT TÌNH HÌNH THỰC HIỆN DỰ ÁN TIẾT KIỆM NĂNG LƯỢNG \n CHO NGÀNH CÔNG NGHIỆP VIỆT NAM";
                        worksheet.Cells["B4:H4"].Style.WrapText = true;

                        worksheet.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Row(4).Height = 39;
                        worksheet.Row(4).Style.Font.Bold = true;


                        worksheet.Cells["D5:G5"].Merge = true;
                        if (planObj.Type == 5)
                        {
                            worksheet.Cells["D5:G5"].Value = "Năm " + planObj.Year;
                        }
                        else
                        {
                            worksheet.Cells["D5:G5"].Value = HtmlHelpers.GetReportType(planObj.Type.GetValueOrDefault()) + " Năm " + planObj.Year;
                        }

                        worksheet.Cells["D5:G5"].Style.WrapText = true;

                        worksheet.Row(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Row(5).Style.Font.Italic = true;


                        //Hear Table
                        worksheet.Cells["A7:B7"].Merge = true;
                        if (planObj.Type == 5)
                        {
                            worksheet.Cells["A7:B7"].Value = "I. Giải ngân trong Năm";
                        }
                        else
                        {
                            worksheet.Cells["A7:B7"].Value = "I. Giải ngân trong Quý";
                        }
                        worksheet.Cells["A7:B7"].Style.WrapText = true;

                        worksheet.Cells["A8:A9"].Merge = true;
                        worksheet.Cells["A8:A9"].Value = "Nguồn vốn";
                        worksheet.Cells["A8:A9"].Style.Border.Right.Style = worksheet.Cells["A8:A9"].Style.Border.Top.Style = worksheet.Cells["A8:A9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["B8:B9"].Merge = true;
                        worksheet.Cells["B8:B9"].Value = "Đơn vị tiền tệ (VND)";
                        worksheet.Cells["B8:B9"].Style.Border.Right.Style = worksheet.Cells["B8:B9"].Style.Border.Top.Style = worksheet.Cells["B8:B9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                        worksheet.Cells["C8:C9"].Merge = true;
                        worksheet.Cells["C8:C9"].Value = "Đơn vị tiền tệ (US$)";
                        worksheet.Cells["C8:C9"].Style.Border.Right.Style = worksheet.Cells["C8:C9"].Style.Border.Top.Style = worksheet.Cells["C8:C9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["D8:D9"].Merge = true;
                        worksheet.Cells["D8:D9"].Value = "Tỷ giá";
                        worksheet.Cells["D8:D9"].Style.Border.Right.Style = worksheet.Cells["D8:D9"].Style.Border.Top.Style = worksheet.Cells["D8:D9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["J8:J9"].Merge = true;
                        worksheet.Cells["J8:J9"].Value = "Lũy kế giải ngân từ đầu dự án (VND)";
                        worksheet.Cells["J8:J9"].Style.Border.Right.Style = worksheet.Cells["J8:J9"].Style.Border.Top.Style = worksheet.Cells["J8:J9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["K8:K9"].Merge = true;
                        worksheet.Cells["K8:K9"].Value = "Mức TKNL đạt được (MWh/năm)";
                        worksheet.Cells["K8:K9"].Style.Border.Right.Style = worksheet.Cells["K8:K9"].Style.Border.Top.Style = worksheet.Cells["K8:K9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                        worksheet.Cells["L8:L9"].Merge = true;
                        worksheet.Cells["L8:L9"].Value = "Mức TKNL đạt được (MJ/năm)";
                        worksheet.Cells["L8:L9"].Style.Border.Right.Style = worksheet.Cells["K8:K9"].Style.Border.Top.Style = worksheet.Cells["K8:K9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                        worksheet.Cells["M8:M9"].Merge = true;
                        worksheet.Cells["M8:M9"].Value = "Giảm phát thải KNK (Tấn CO2/năm)";
                        worksheet.Cells["M8:M9"].Style.Border.Right.Style = worksheet.Cells["L8:L9"].Style.Border.Top.Style = worksheet.Cells["L8:L9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["N8:N9"].Merge = true;
                        worksheet.Cells["N8:N9"].Value = "Số lượng người hưởng lợi trực tiếp)";
                        worksheet.Cells["N8:N9"].Style.Border.Right.Style = worksheet.Cells["M8:M9"].Style.Border.Top.Style = worksheet.Cells["M8:M9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["O8:O9"].Merge = true;
                        worksheet.Cells["O8:O9"].Value = "Tỷ lệ người hưởng lợi là phụ nữ (%)";
                        worksheet.Cells["O8:O9"].Style.Border.Right.Style = worksheet.Cells["N8:N9"].Style.Border.Top.Style = worksheet.Cells["N8:N9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["P8:P9"].Merge = true;
                        worksheet.Cells["P8:P9"].Value = "Ngày bắt đầu vận hành";
                        worksheet.Cells["P8:P9"].Style.Border.Right.Style = worksheet.Cells["P8:P9"].Style.Border.Top.Style = worksheet.Cells["P8:P9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["E8:I8"].Merge = true;
                        worksheet.Cells["E8:I8"].Value = "Giải ngân trong năm (VND)";
                        worksheet.Cells["E8:I8"].Style.Border.Right.Style = worksheet.Cells["E8:I8"].Style.Border.Top.Style = worksheet.Cells["E8:I8"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["E9"].Value = "Kế hoạch giải ngân năm";


                        worksheet.Cells["F9"].Value = "Kế hoạch tới thời điểm báo cáo";
                        if (planObj.Type == 5)
                        {
                            worksheet.Cells["G9"].Value = "Giải ngân trong Năm";
                        }
                        else
                        {
                            worksheet.Cells["G9"].Value = "Giải ngân trong Quý";
                        }

                        worksheet.Cells["H9"].Value = "Lũy kế từ đầu năm";
                        worksheet.Cells["I9"].Value = "Tỷ lệ % đạt được so với kế hoạch";

                        worksheet.Cells["E9"].Style.Border.Right.Style = worksheet.Cells["E9"].Style.Border.Top.Style = worksheet.Cells["E9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["F9"].Style.Border.Right.Style = worksheet.Cells["F9"].Style.Border.Top.Style = worksheet.Cells["F9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["G9"].Style.Border.Right.Style = worksheet.Cells["G9"].Style.Border.Top.Style = worksheet.Cells["G9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["H9"].Style.Border.Right.Style = worksheet.Cells["H9"].Style.Border.Top.Style = worksheet.Cells["H9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["I9"].Style.Border.Right.Style = worksheet.Cells["I9"].Style.Border.Top.Style = worksheet.Cells["I9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Row(8).Style.Font.Bold = true;
                        worksheet.Row(8).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(8).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        worksheet.Row(8).Style.WrapText = true;
                        worksheet.Row(9).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(9).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        worksheet.Row(9).Style.Font.Bold = true;
                        worksheet.Row(9).Height = 45;
                        worksheet.Row(9).Style.WrapText = true;

                        worksheet.Cells["A10"].Value = "1";
                        worksheet.Cells["B10"].Value = "2";
                        worksheet.Cells["D10"].Value = "3";
                        worksheet.Cells["E10"].Value = "4";
                        worksheet.Cells["F10"].Value = "5";
                        worksheet.Cells["G10"].Value = "6";
                        worksheet.Cells["H10"].Value = "7";
                        worksheet.Cells["I10"].Value = "(8)=(7)/(4)";
                        worksheet.Cells["J10"].Value = "9";
                        worksheet.Cells["A10"].Style.Border.Right.Style = worksheet.Cells["A10"].Style.Border.Top.Style = worksheet.Cells["A10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["B10"].Style.Border.Right.Style = worksheet.Cells["B10"].Style.Border.Top.Style = worksheet.Cells["B10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["C10"].Style.Border.Right.Style = worksheet.Cells["C10"].Style.Border.Top.Style = worksheet.Cells["C10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["D10"].Style.Border.Right.Style = worksheet.Cells["D10"].Style.Border.Top.Style = worksheet.Cells["D10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["E10"].Style.Border.Right.Style = worksheet.Cells["E10"].Style.Border.Top.Style = worksheet.Cells["E10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["F10"].Style.Border.Right.Style = worksheet.Cells["F10"].Style.Border.Top.Style = worksheet.Cells["F10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["G10"].Style.Border.Right.Style = worksheet.Cells["G10"].Style.Border.Top.Style = worksheet.Cells["G10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["H10"].Style.Border.Right.Style = worksheet.Cells["H10"].Style.Border.Top.Style = worksheet.Cells["H10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["I10"].Style.Border.Right.Style = worksheet.Cells["I10"].Style.Border.Top.Style = worksheet.Cells["I10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["J10"].Style.Border.Right.Style = worksheet.Cells["J10"].Style.Border.Top.Style = worksheet.Cells["J10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["K10"].Style.Border.Right.Style = worksheet.Cells["K10"].Style.Border.Top.Style = worksheet.Cells["K10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["L10"].Style.Border.Right.Style = worksheet.Cells["L10"].Style.Border.Top.Style = worksheet.Cells["L10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["M10"].Style.Border.Right.Style = worksheet.Cells["M10"].Style.Border.Top.Style = worksheet.Cells["M10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["N10"].Style.Border.Right.Style = worksheet.Cells["N10"].Style.Border.Top.Style = worksheet.Cells["N10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["O10"].Style.Border.Right.Style = worksheet.Cells["O10"].Style.Border.Top.Style = worksheet.Cells["O10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["P10"].Style.Border.Right.Style = worksheet.Cells["P10"].Style.Border.Top.Style = worksheet.Cells["P10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Row(10).Style.Font.Bold = true;
                        worksheet.Row(10).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(10).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        worksheet.Row(10).Style.WrapText = true;



                        //Danh sách dự án

                        var lstPlanItem = _planItemService.GetList(id).Where(x => x.Status == 1).ToList();
                        if (lstPlanItem.Count > 0)
                        {
                            int row = 11;
                            var lstPRow = new List<int>();
                            for (int i = 0; i < lstPlanItem.Count; i++)
                            {

                                lstPRow.Add(row + 1);
                                var item = lstPlanItem[i];
                                worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                                worksheet.Row(row + 1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                                worksheet.Row(row + 2).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                                worksheet.Row(row + 3).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                                //tên dự án
                                worksheet.Cells[$"A{row}:J{row}"].Merge = true;
                                worksheet.Cells[$"A{row}:J{row}"].Value = item.Name;
                                worksheet.Cells[$"A{row}:J{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}:J{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}:J{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[$"A{row}:J{row}"].Style.Font.Bold = true;
                                worksheet.Cells[$"A{row}:J{row}"].Style.WrapText = true;

                                worksheet.Cells[$"K{row}:K{row + 3}"].Merge = true;
                                worksheet.Cells[$"K{row}:K{row + 3}"].Value = item.Total1;
                                worksheet.Cells[$"K{row}:K{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"K{row}:K{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"K{row}:K{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"L{row}:L{row + 3}"].Merge = true;
                                worksheet.Cells[$"L{row}:L{row + 3}"].Formula = $"K{row } *3600";
                                worksheet.Cells[$"L{row}:L{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"L{row}:L{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"L{row}:L{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                                worksheet.Cells[$"M{row}:M{row + 3}"].Merge = true;
                                worksheet.Cells[$"M{row}:M{row + 3}"].Value = item.Total2;
                                worksheet.Cells[$"M{row}:M{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"M{row}:M{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"M{row}:M{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"N{row}:N{row + 3}"].Merge = true;
                                worksheet.Cells[$"N{row}:N{row + 3}"].Value = item.NumberPeople;
                                worksheet.Cells[$"N{row}:N{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"N{row}:N{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"N{row}:N{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"O{row}:O{row + 3}"].Merge = true;
                                worksheet.Cells[$"O{row}:O{row + 3}"].Value = item.WomanRate;
                                worksheet.Cells[$"O{row}:O{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"O{row}:O{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"O{row}:O{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"P{row}:P{row + 3}"].Merge = true;
                                worksheet.Cells[$"P{row}:P{row + 3}"].Value = item.Time.GetValueOrDefault().ToString("dd/MM/yyyy");
                                worksheet.Cells[$"P{row}:P{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"P{row}:P{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"P{row}:P{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                                worksheet.Cells[$"A{row + 1}"].Merge = true;
                                worksheet.Cells[$"A{row + 1}"].Value = "Vốn vay WB";
                                worksheet.Cells[$"A{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"A{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"A{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[$"A{row + 2}"].Merge = true;
                                worksheet.Cells[$"A{row + 2}"].Value = "Vốn đối ứng của PFI";
                                worksheet.Cells[$"A{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"A{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"A{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[$"A{row + 3}"].Merge = true;
                                worksheet.Cells[$"A{row + 3}"].Value = "Vốn đối ứng của IE";
                                worksheet.Cells[$"A{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"A{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"A{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                var objConfig1 = JsonConvert.DeserializeObject<PlanItemData>(item.Config1);
                                var objConfig2 = JsonConvert.DeserializeObject<PlanItemData>(item.Config2);
                                var objConfig3 = JsonConvert.DeserializeObject<PlanItemData>(item.Config3);

                                worksheet.Cells[$"B{row + 1}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"B{row + 1}"].Value = objConfig1.Money;
                                worksheet.Cells[$"B{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"B{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"B{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"B{row + 2}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"B{row + 2}"].Value = objConfig2.Money;
                                worksheet.Cells[$"B{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"B{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"B{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"B{row + 3}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"B{row + 3}"].Value = objConfig3.Money;
                                worksheet.Cells[$"B{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"B{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"B{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"C{row + 1}"].Style.Numberformat.Format = "#,##0.000";
                                worksheet.Cells[$"C{row + 1}"].Formula = $"ROUND(B{row + 1}/D{row + 1},3)";
                                worksheet.Cells[$"C{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"C{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"C{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"C{row + 2}"].Style.Numberformat.Format = "#,##0.000";
                                worksheet.Cells[$"C{row + 2}"].Formula = $"ROUND(B{row + 2}/D{row + 2},3)";
                                worksheet.Cells[$"C{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"C{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"C{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"C{row + 3}"].Style.Numberformat.Format = "#,##0.000";
                                worksheet.Cells[$"C{row + 3}"].Formula = $"ROUND(B{row + 3}/D{row + 3},3)";
                                worksheet.Cells[$"C{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"C{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"C{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                                worksheet.Cells[$"D{row + 1}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"D{row + 1}"].Value = objConfig1.CurrencyRate;
                                worksheet.Cells[$"D{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"D{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"D{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"D{row + 2}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"D{row + 2}"].Value = objConfig2.CurrencyRate;
                                worksheet.Cells[$"D{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"D{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"D{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"D{row + 3}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"D{row + 3}"].Value = objConfig3.CurrencyRate;
                                worksheet.Cells[$"D{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"D{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"D{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                                worksheet.Cells[$"E{row + 1}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"E{row + 1}"].Value = objConfig1.PlanYear;
                                worksheet.Cells[$"E{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"E{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"E{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"E{row + 2}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"E{row + 2}"].Value = objConfig2.PlanYear;
                                worksheet.Cells[$"E{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"E{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"E{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"E{row + 3}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"E{row + 3}"].Value = objConfig3.PlanYear;
                                worksheet.Cells[$"E{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"E{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"E{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                                worksheet.Cells[$"F{row + 1}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"F{row + 1}"].Value = objConfig1.PlanCurrent;
                                worksheet.Cells[$"F{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"F{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"F{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"F{row + 2}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"F{row + 2}"].Value = objConfig2.PlanCurrent;
                                worksheet.Cells[$"F{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"F{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"F{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"F{row + 3}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"F{row + 3}"].Value = objConfig3.PlanCurrent;
                                worksheet.Cells[$"F{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"F{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"F{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                                worksheet.Cells[$"G{row + 1}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"G{row + 1}"].Value = objConfig1.PlanQ;
                                worksheet.Cells[$"G{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"G{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"G{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"G{row + 2}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"G{row + 2}"].Value = objConfig2.PlanQ;
                                worksheet.Cells[$"G{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"G{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"G{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"G{row + 3}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"G{row + 3}"].Value = objConfig3.PlanQ;
                                worksheet.Cells[$"G{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"G{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"G{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                                worksheet.Cells[$"H{row + 1}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"H{row + 1}"].Value = objConfig1.BalanceYear;
                                worksheet.Cells[$"H{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"H{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"H{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"H{row + 2}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"H{row + 2}"].Value = objConfig2.BalanceYear;
                                worksheet.Cells[$"H{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"H{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"H{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"H{row + 3}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"H{row + 3}"].Value = objConfig3.BalanceYear;
                                worksheet.Cells[$"H{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"H{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"H{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                                worksheet.Cells[$"I{row + 1}"].Formula = $"ROUND(H{row + 1}*100/E{row + 1},3)";
                                worksheet.Cells[$"I{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"I{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"I{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"I{row + 2}"].Formula = $"ROUND(H{row + 2}*100/E{row + 2},3)";
                                worksheet.Cells[$"I{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"I{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"I{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"I{row + 3}"].Formula = $"ROUND(H{row + 3}*100/E{row + 3},3)";
                                worksheet.Cells[$"I{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"I{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"I{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"J{row + 1}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"J{row + 1}"].Value = objConfig1.Balance;
                                worksheet.Cells[$"J{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"J{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"J{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"J{row + 2}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"J{row + 2}"].Value = objConfig2.Balance;
                                worksheet.Cells[$"J{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"J{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"J{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"J{row + 3}"].Style.Numberformat.Format = "#,##0";
                                worksheet.Cells[$"J{row + 3}"].Value = objConfig3.Balance;
                                worksheet.Cells[$"J{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"J{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"J{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                row += 4;
                            }
                            //tổng
                            worksheet.Cells[$"A{row}:J{row}"].Merge = true;
                            worksheet.Cells[$"A{row}:J{row}"].Value = "Tổng cộng";
                            worksheet.Cells[$"A{row}:J{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}:J{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}:J{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"A{row}:J{row}"].Style.Font.Bold = true;
                            worksheet.Cells[$"A{row}:J{row}"].Style.WrapText = true;
                            worksheet.Cells[$"A{row}:J{row}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[$"A{row}:J{row}"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                            worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;

                            worksheet.Cells[$"K{row}:K{row + 3}"].Merge = true;
                            worksheet.Cells[$"K{row}:K{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"K{row}:K{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"K{row}:K{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"L{row}:L{row + 3}"].Merge = true;
                            worksheet.Cells[$"L{row}:L{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"L{row}:L{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"L{row}:L{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                            worksheet.Cells[$"M{row}:M{row + 3}"].Merge = true;
                            worksheet.Cells[$"M{row}:M{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"M{row}:M{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"M{row}:M{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"N{row}:N{row + 3}"].Merge = true;
                            worksheet.Cells[$"N{row}:N{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"N{row}:N{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"N{row}:N{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"O{row}:O{row + 3}"].Merge = true;
                            worksheet.Cells[$"O{row}:O{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"O{row}:O{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"O{row}:O{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"P{row}:P{row + 3}"].Merge = true;
                            worksheet.Cells[$"P{row}:P{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"P{row}:P{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"P{row}:P{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"A{row + 1}"].Merge = true;
                            worksheet.Cells[$"A{row + 1}"].Value = "Vốn vay WB";
                            worksheet.Cells[$"A{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"A{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"A{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"A{row + 2}"].Merge = true;
                            worksheet.Cells[$"A{row + 2}"].Value = "Vốn đối ứng của PFI";
                            worksheet.Cells[$"A{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"A{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"A{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"A{row + 3}"].Merge = true;
                            worksheet.Cells[$"A{row + 3}"].Value = "Vốn đối ứng của IE";
                            worksheet.Cells[$"A{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"A{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"A{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            var forB = "";
                            var forC = "";
                            var forE = "";
                            var forF = "";
                            var forG = "";
                            var forH = "";
                            var forJ = "";
                            var forL = "";
                            var forK = "";
                            var forM = "";
                            var forN = "";
                            var forO = "(";
                            var forB2 = "";
                            var forC2 = "";
                            var forE2 = "";
                            var forF2 = "";
                            var forG2 = "";
                            var forH2 = "";
                            var forJ2 = "";


                            var forB3 = "";
                            var forC3 = "";
                            var forE3 = "";
                            var forF3 = "";
                            var forG3 = "";
                            var forH3 = "";
                            var forJ3 = "";

                            foreach (var PRow in lstPRow)
                            {
                                forB += $"+B{PRow}";
                                forC += $"+C{PRow}";
                                forE += $"+E{PRow}";
                                forF += $"+F{PRow}";
                                forG += $"+G{PRow}";
                                forH += $"+H{PRow}";
                                forJ += $"+J{PRow}";
                                forL += $"+L{PRow}";
                                forK += $"+K{PRow}";

                                forM += $"+M{PRow}";
                                forN += $"+N{PRow}";
                                forO += $"+O{PRow}";
                                forB2 += $"+B{PRow + 1}";
                                forC2 += $"+C{PRow + 1}";
                                forE2 += $"+E{PRow + 1}";
                                forF2 += $"+F{PRow + 1}";
                                forG2 += $"+G{PRow + 1}";
                                forH2 += $"+H{PRow + 1}";
                                forJ2 += $"+J{PRow + 1}";

                                forB3 += $"+B{PRow + 2}";
                                forC3 += $"+C{PRow + 2}";
                                forE3 += $"+E{PRow + 2}";
                                forF3 += $"+F{PRow + 2}";
                                forG3 += $"+G{PRow + 2}";
                                forH3 += $"+H{PRow + 2}";
                                forJ3 += $"+J{PRow + 2}";

                            }
                            forO += ")/" + lstPRow.Count;
                            worksheet.Cells[$"B{row + 1}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"B{row + 1}"].Formula = forB;
                            worksheet.Cells[$"B{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"B{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"B{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"B{row + 2}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"B{row + 2}"].Formula = forB2;
                            worksheet.Cells[$"B{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"B{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"B{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"B{row + 3}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"B{row + 3}"].Formula = forB3;
                            worksheet.Cells[$"B{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"B{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"B{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"E{row + 1}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"E{row + 1}"].Formula = forE;
                            worksheet.Cells[$"E{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"E{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"E{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"E{row + 2}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"E{row + 2}"].Formula = forE2;
                            worksheet.Cells[$"E{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"E{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"E{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"E{row + 3}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"E{row + 3}"].Formula = forE3;
                            worksheet.Cells[$"E{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"E{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"E{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"F{row + 1}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"F{row + 1}"].Formula = forF;
                            worksheet.Cells[$"F{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"F{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"F{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"F{row + 2}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"F{row + 2}"].Formula = forF2;
                            worksheet.Cells[$"F{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"F{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"F{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"F{row + 3}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"F{row + 3}"].Formula = forF3;
                            worksheet.Cells[$"F{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"F{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"F{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"G{row + 1}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"G{row + 1}"].Formula = forG;
                            worksheet.Cells[$"G{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"G{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"G{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"G{row + 2}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"G{row + 2}"].Formula = forG2;
                            worksheet.Cells[$"G{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"G{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"G{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"G{row + 3}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"G{row + 3}"].Formula = forG3;
                            worksheet.Cells[$"G{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"G{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"G{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"H{row + 3}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"H{row + 1}"].Formula = forH;
                            worksheet.Cells[$"H{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"H{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"H{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"H{row + 2}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"H{row + 2}"].Formula = forH2;
                            worksheet.Cells[$"H{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"H{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"H{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"H{row + 3}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"H{row + 3}"].Formula = forH3;
                            worksheet.Cells[$"H{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"H{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"H{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"J{row + 1}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"J{row + 1}"].Formula = forJ;
                            worksheet.Cells[$"J{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"J{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"J{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"J{row + 2}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"J{row + 2}"].Formula = forJ2;
                            worksheet.Cells[$"J{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"J{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"J{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"J{row + 3}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"J{row + 3}"].Formula = forJ3;
                            worksheet.Cells[$"J{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"J{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"J{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"L{row + 1}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"L{row }"].Formula = forL;
                            worksheet.Cells[$"L{row }"].Style.Border.Right.Style = worksheet.Cells[$"L{row }"].Style.Border.Top.Style = worksheet.Cells[$"L{row }"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"M{row + 1}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"M{row }"].Formula = forM;
                            worksheet.Cells[$"M{row }"].Style.Border.Right.Style = worksheet.Cells[$"M{row }"].Style.Border.Top.Style = worksheet.Cells[$"M{row }"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                            worksheet.Cells[$"N{row }"].Formula = forN;
                            worksheet.Cells[$"N{row }"].Style.Border.Right.Style = worksheet.Cells[$"N{row }"].Style.Border.Top.Style = worksheet.Cells[$"N{row }"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"O{row }"].Formula = forO;
                            worksheet.Cells[$"O{row }"].Style.Border.Right.Style = worksheet.Cells[$"O{row }"].Style.Border.Top.Style = worksheet.Cells[$"O{row }"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                            worksheet.Cells[$"K{row + 1}"].Style.Numberformat.Format = "#,##0";
                            worksheet.Cells[$"K{row}"].Formula = forK;
                            worksheet.Cells[$"K{row}"].Style.Border.Right.Style = worksheet.Cells[$"K{row}"].Style.Border.Top.Style = worksheet.Cells[$"K{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"C{row + 1}"].Style.Numberformat.Format = "#,##0.000";
                            worksheet.Cells[$"C{row + 1}"].Formula = forC;
                            worksheet.Cells[$"C{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"C{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"C{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"C{row + 2}"].Style.Numberformat.Format = "#,##0.000";
                            worksheet.Cells[$"C{row + 2}"].Formula = forC2;
                            worksheet.Cells[$"C{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"C{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"C{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"C{row + 1}"].Style.Numberformat.Format = "#,##0.000";
                            worksheet.Cells[$"C{row + 3}"].Formula = forC3;
                            worksheet.Cells[$"C{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"C{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"C{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"I{row + 1}"].Formula = $"ROUND(H{row + 1}*100/E{row + 1},3)"; ;
                            worksheet.Cells[$"I{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"I{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"I{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"I{row + 2}"].Formula = $"ROUND(H{row + 2}*100/E{row + 2},3)"; ;
                            worksheet.Cells[$"I{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"I{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"I{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"I{row + 3}"].Formula = $"ROUND(H{row + 3}*100/E{row + 3},3)"; ;
                            worksheet.Cells[$"I{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"I{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"I{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;




                            worksheet.Cells[$"D{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"D{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"D{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"D{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"D{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"D{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"D{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"D{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"D{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            row += 5;

                            //Kết quả đạt được
                            worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            worksheet.Row(row + 1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;


                            worksheet.Cells[$"A{row}"].Value = "II. Tóm tắt các kết quả đạt được";
                            worksheet.Cells[$"A{row}"].Style.WrapText = false;
                            row++;

                            worksheet.Cells[$"A{row}"].Value = "II.1 Quản lý tài chính";
                            worksheet.Cells[$"A{row}"].Style.WrapText = false;
                            row++;
                            planObj.Result = planObj.Result.Replace("<br />", "\n");
                            worksheet.Cells[$"A{row}:L{row}"].Merge = true;
                            worksheet.Cells[$"A{row}:L{row}"].Value = planObj.Result;
                            worksheet.Row(row).Height = 16 * (planObj.Result.Split('\n').Length);
                            worksheet.Cells[$"A{row}:L{row}"].Style.WrapText = true;
                            row++;

                            worksheet.Cells[$"A{row}"].Value = "II.2 Quản lý đấu thầu";
                            worksheet.Cells[$"A{row}"].Style.WrapText = false;
                            row++;
                            planObj.Result2 = planObj.Result2.Replace("<br />", "\n");
                            worksheet.Cells[$"A{row}:L{row}"].Merge = true;
                            worksheet.Cells[$"A{row}:L{row}"].Value = planObj.Result2;
                            worksheet.Row(row).Height = 16 * (planObj.Result2.Split('\n').Length);
                            worksheet.Cells[$"A{row}:L{row}"].Style.WrapText = true;
                            row++;

                            worksheet.Cells[$"A{row}"].Value = "II.3 Các vấn đề về an toàn môi trường, xã hội";
                            worksheet.Cells[$"A{row}"].Style.WrapText = false;
                            row++;
                            planObj.Result3 = planObj.Result3.Replace("<br />", "\n");
                            worksheet.Cells[$"A{row}:L{row}"].Merge = true;
                            worksheet.Cells[$"A{row}:L{row}"].Value = planObj.Result3;
                            worksheet.Row(row).Height = 16 * (planObj.Result3.Split('\n').Length);
                            worksheet.Cells[$"A{row}:L{row}"].Style.WrapText = true;
                            row++;

                            worksheet.Cells[$"A{row}"].Value = "II.4 Các vấn đề khác";
                            worksheet.Cells[$"A{row}"].Style.WrapText = false;
                            row++;
                            planObj.Result4 = planObj.Result4.Replace("<br />", "\n");
                            worksheet.Cells[$"A{row}:L{row}"].Merge = true;
                            worksheet.Cells[$"A{row}:L{row}"].Value = planObj.Result4;
                            worksheet.Row(row).Height = 16 * (planObj.Result4.Split('\n').Length);
                            worksheet.Cells[$"A{row}:L{row}"].Style.WrapText = true;

                            row += 2;

                            //Cac vướng mắc
                            worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            worksheet.Cells[$"A{row}"].Value = "III. Các vướng mắc";
                            worksheet.Cells[$"A{row}"].Style.WrapText = false;
                            row++;

                            worksheet.Cells[$"A{row}:A{row + 1}"].Merge = true;
                            worksheet.Cells[$"A{row}:A{row + 1}"].Value = "STT";
                            worksheet.Cells[$"A{row}:A{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}:A{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}:A{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"B{row}:B{row + 1}"].Merge = true;
                            worksheet.Cells[$"B{row}:B{row + 1}"].Style.WrapText = true;
                            worksheet.Cells[$"B{row}:B{row + 1}"].Value = "Tên dự án";
                            worksheet.Cells[$"B{row}:B{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"B{row}:B{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"B{row}:B{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"C{row}:C{row + 1}"].Merge = true;
                            worksheet.Cells[$"C{row}:C{row + 1}"].Style.WrapText = true;
                            worksheet.Cells[$"C{row}:C{row + 1}"].Value = "Mô tả vướng mắc";
                            worksheet.Cells[$"C{row}:C{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"C{row}:C{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"C{row}:C{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"D{row}:D{row + 1}"].Merge = true;
                            worksheet.Cells[$"D{row}:D{row + 1}"].Style.WrapText = true;
                            worksheet.Cells[$"D{row}:D{row + 1}"].Value = "Cơ quan giải quyết";
                            worksheet.Cells[$"D{row}:D{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"D{row}:D{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"D{row}:D{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                            worksheet.Cells[$"E{row}:E{row + 1}"].Merge = true;
                            worksheet.Cells[$"E{row}:E{row + 1}"].Style.WrapText = true;
                            worksheet.Cells[$"E{row}:E{row + 1}"].Value = "Thời hạn giải quyết";
                            worksheet.Cells[$"E{row}:E{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"E{row}:E{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"E{row}:E{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                            worksheet.Cells[$"F{row}:G{row}"].Merge = true;
                            worksheet.Cells[$"F{row}:G{row}"].Style.WrapText = true;
                            worksheet.Cells[$"F{row}:G{row}"].Value = "Tình trạng giải quyết";
                            worksheet.Cells[$"F{row}:G{row}"].Style.Border.Right.Style = worksheet.Cells[$"F{row}:G{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"F{row}:G{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"F{row + 1}"].Style.WrapText = true;
                            worksheet.Cells[$"F{row + 1}"].Value = "Đã giải quyết/Ngày";
                            worksheet.Cells[$"F{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"F{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"F{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"G{row + 1}"].Style.WrapText = true;
                            worksheet.Cells[$"G{row + 1}"].Value = "Chưa giải quyết/Thời hạn mới";
                            worksheet.Cells[$"G{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"G{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"G{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Row(row).Style.Font.Bold = true;
                            worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            worksheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Row(row + 1).Height = 48;
                            worksheet.Row(row + 1).Style.Font.Bold = true;
                            worksheet.Row(row + 1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            worksheet.Row(row + 1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            row += 2;
                            var lstPlanStuck = _planStuckService.GetList(id).Where(x => x.Status == 1).ToList();

                            if (lstPlanStuck.Count > 0)
                            {


                                for (int i = 0; i < lstPlanStuck.Count; i++)
                                {
                                    worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    var itemstuck = lstPlanStuck[i];

                                    worksheet.Cells[$"A{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[$"A{row}"].Value = i + 1;
                                    worksheet.Cells[$"A{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"B{row}"].Value = itemstuck.Name;
                                    worksheet.Cells[$"B{row}"].Style.Border.Right.Style = worksheet.Cells[$"B{row}"].Style.Border.Top.Style = worksheet.Cells[$"B{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"B{row}"].Style.WrapText = true;

                                    itemstuck.Description = itemstuck.Description.Replace("<br />", "\n");
                                    worksheet.Row(row).Height = 16 * (itemstuck.Description.Split('\n').Length);
                                    worksheet.Cells[$"C{row}"].Value = itemstuck.Description;
                                    worksheet.Cells[$"C{row}"].Style.Border.Right.Style = worksheet.Cells[$"C{row}"].Style.Border.Top.Style = worksheet.Cells[$"C{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"C{row}"].Style.WrapText = true;

                                    worksheet.Cells[$"D{row}"].Value = itemstuck.Organ;
                                    worksheet.Cells[$"D{row}"].Style.Border.Right.Style = worksheet.Cells[$"D{row}"].Style.Border.Top.Style = worksheet.Cells[$"D{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"D{row}"].Style.WrapText = true;

                                    worksheet.Cells[$"E{row}"].Value = itemstuck.FinishDate;
                                    worksheet.Cells[$"E{row}"].Style.Border.Right.Style = worksheet.Cells[$"E{row}"].Style.Border.Top.Style = worksheet.Cells[$"E{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"E{row}"].Style.WrapText = true;

                                    worksheet.Cells[$"F{row}"].Style.WrapText = true;
                                    worksheet.Cells[$"F{row}"].Value = itemstuck.Result1;
                                    worksheet.Cells[$"F{row}"].Style.Border.Right.Style = worksheet.Cells[$"F{row}"].Style.Border.Top.Style = worksheet.Cells[$"F{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"G{row}"].Style.WrapText = true;
                                    worksheet.Cells[$"G{row}"].Value = itemstuck.Result2;
                                    worksheet.Cells[$"G{row}"].Style.Border.Right.Style = worksheet.Cells[$"G{row}"].Style.Border.Top.Style = worksheet.Cells[$"G{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    row++;
                                }


                            }
                            row++;

                            //Kế hoạch
                            worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            worksheet.Row(row + 1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;

                            if (planObj.Type == 5)
                            {
                                worksheet.Cells[$"A{row}"].Value = "IV. Kế hoạch trong năm tiếp theo";
                            }
                            else
                            {
                                worksheet.Cells[$"A{row}"].Value = "IV. Kế hoạch trong quý tiếp theo";
                            }
                            worksheet.Cells[$"A{row}"].Style.WrapText = false;
                            row++;
                            planObj.WorkPlan = planObj.WorkPlan.Replace("<br />", "\n");
                            worksheet.Cells[$"A{row}:L{row}"].Merge = true;
                            worksheet.Cells[$"A{row}:L{row}"].Value = planObj.WorkPlan;
                            worksheet.Row(row).Height = 16 * (planObj.WorkPlan.Split('\n').Length);
                            worksheet.Cells[$"A{row}:L{row}"].Style.WrapText = true;
                            row += 2;
                            //Các vấn đề khác
                            worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            worksheet.Row(row + 1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            worksheet.Cells[$"A{row}"].Value = "V. Các vấn đề khác";
                            worksheet.Cells[$"A{row}"].Style.WrapText = false;
                            row++;
                            planObj.Problem = planObj.Problem.Replace("<br />", "\n");
                            worksheet.Cells[$"A{row}:L{row}"].Merge = true;
                            worksheet.Cells[$"A{row}:L{row}"].Value = planObj.Problem;
                            worksheet.Row(row).Height = 16 * (planObj.Problem.Split('\n').Length);
                            worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            worksheet.Cells[$"A{row}:L{row}"].Style.WrapText = true;
                            row += 2;

                            //Dự án tiềm lăng
                            worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            worksheet.Cells[$"A{row}"].Value = "VI. Danh mục dự án tiềm năng";
                            worksheet.Cells[$"A{row}"].Style.WrapText = false;
                            row++;


                            worksheet.Cells[$"A{row}"].Value = "STT";
                            worksheet.Cells[$"A{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                            worksheet.Cells[$"B{row}"].Style.WrapText = true;
                            worksheet.Cells[$"B{row}"].Value = "Tên dự án";
                            worksheet.Cells[$"B{row}"].Style.Border.Right.Style = worksheet.Cells[$"B{row}"].Style.Border.Top.Style = worksheet.Cells[$"B{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"C{row}"].Style.WrapText = true;
                            worksheet.Cells[$"C{row}"].Value = "Địa điểm thực hiện dự án";
                            worksheet.Cells[$"C{row}"].Style.Border.Right.Style = worksheet.Cells[$"C{row}"].Style.Border.Top.Style = worksheet.Cells[$"C{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                            worksheet.Cells[$"D{row}"].Value = "Loại dự án";
                            worksheet.Cells[$"D{row}"].Style.Border.Right.Style = worksheet.Cells[$"D{row}"].Style.Border.Top.Style = worksheet.Cells[$"D{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                            worksheet.Cells[$"E{row}"].Style.WrapText = true;
                            worksheet.Cells[$"E{row}"].Value = "Tổng chi phí đầu tư(triệu VND)";
                            worksheet.Cells[$"E{row}"].Style.Border.Right.Style = worksheet.Cells[$"E{row}"].Style.Border.Top.Style = worksheet.Cells[$"E{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;



                            worksheet.Cells[$"F{row}"].Style.WrapText = true;
                            worksheet.Cells[$"F{row}"].Value = "Tổng vay(triệu VND)";
                            worksheet.Cells[$"F{row}"].Style.Border.Right.Style = worksheet.Cells[$"F{row}"].Style.Border.Top.Style = worksheet.Cells[$"F{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;



                            worksheet.Cells[$"G{row}"].Style.WrapText = true;
                            worksheet.Cells[$"G{row}"].Value = "Vốn vay IBRD (triệu VND)";
                            worksheet.Cells[$"G{row}"].Style.Border.Right.Style = worksheet.Cells[$"G{row}"].Style.Border.Top.Style = worksheet.Cells[$"G{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                            worksheet.Cells[$"H{row}"].Style.WrapText = true;
                            worksheet.Cells[$"H{row}"].Value = "Hiện trạng";
                            worksheet.Cells[$"H{row}"].Style.Border.Right.Style = worksheet.Cells[$"H{row}"].Style.Border.Top.Style = worksheet.Cells[$"H{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                            worksheet.Row(row).Style.Font.Bold = true;
                            worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                            worksheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            //worksheet.Row(row).Height = 32;

                            row++;
                            var firstRequire = row;
                            var lstPlanRequire = _planRequireService.GetList(id).Where(x => x.Status == 1).ToList();

                            if (lstPlanRequire.Count > 0)
                            {

                                for (int i = 0; i < lstPlanRequire.Count; i++)
                                {
                                    worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                    worksheet.Row(row).Height = 16;

                                    var itemstuck = lstPlanRequire[i];

                                    worksheet.Cells[$"A{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[$"A{row}"].Value = i + 1;
                                    worksheet.Cells[$"A{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"B{row}"].Value = itemstuck.FinishDate;
                                    worksheet.Cells[$"B{row}"].Style.Border.Right.Style = worksheet.Cells[$"B{row}"].Style.Border.Top.Style = worksheet.Cells[$"B{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"B{row}"].Style.WrapText = true;

                                    worksheet.Cells[$"C{row}"].Value = itemstuck.Place;
                                    worksheet.Cells[$"C{row}"].Style.Border.Right.Style = worksheet.Cells[$"C{row}"].Style.Border.Top.Style = worksheet.Cells[$"C{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"C{row}"].Style.WrapText = true;

                                    worksheet.Cells[$"D{row}"].Value = itemstuck.Description;
                                    worksheet.Cells[$"D{row}"].Style.Border.Right.Style = worksheet.Cells[$"D{row}"].Style.Border.Top.Style = worksheet.Cells[$"D{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"D{row}"].Style.WrapText = true;

                                    worksheet.Cells[$"E{row}"].Value = itemstuck.Total;
                                    worksheet.Cells[$"E{row}"].Style.Border.Right.Style = worksheet.Cells[$"E{row}"].Style.Border.Top.Style = worksheet.Cells[$"E{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"E{row}"].Style.WrapText = true;

                                    worksheet.Cells[$"F{row}"].Style.WrapText = true;
                                    worksheet.Cells[$"F{row}"].Value = itemstuck.Total1;
                                    worksheet.Cells[$"F{row}"].Style.Border.Right.Style = worksheet.Cells[$"F{row}"].Style.Border.Top.Style = worksheet.Cells[$"F{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"G{row}"].Style.WrapText = true;
                                    worksheet.Cells[$"G{row}"].Value = itemstuck.Total2;
                                    worksheet.Cells[$"G{row}"].Style.Border.Right.Style = worksheet.Cells[$"G{row}"].Style.Border.Top.Style = worksheet.Cells[$"G{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                                    worksheet.Cells[$"H{row}"].Style.WrapText = true;
                                    worksheet.Cells[$"H{row}"].Value = itemstuck.Result;
                                    if (itemstuck.Result.Equals("Khác"))
                                        worksheet.Cells[$"H{row}"].Value = itemstuck.ResultOther;
                                    worksheet.Cells[$"H{row}"].Style.Border.Right.Style = worksheet.Cells[$"H{row}"].Style.Border.Top.Style = worksheet.Cells[$"H{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    row++;
                                }


                            }
                            //row++;
                            worksheet.Cells[$"A{row}:D{row}"].Merge = true;
                            worksheet.Cells[$"A{row}:D{row}"].Style.Font.Bold = true;
                            worksheet.Cells[$"A{row}:D{row}"].Value = "Tổng";
                            worksheet.Cells[$"A{row}:D{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}:D{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}:D{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"E{row}"].Formula = $"sum(E{firstRequire}:E{ row - 1})";
                            worksheet.Cells[$"E{row}"].Style.Border.Right.Style = worksheet.Cells[$"E{row}"].Style.Border.Top.Style = worksheet.Cells[$"E{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"F{row}"].Formula = $"sum(F{firstRequire}:F{ row - 1})";
                            worksheet.Cells[$"F{row}"].Style.Border.Right.Style = worksheet.Cells[$"F{row}"].Style.Border.Top.Style = worksheet.Cells[$"F{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[$"G{row}"].Formula = $"sum(G{firstRequire}:G{ row - 1})";
                            worksheet.Cells[$"G{row}"].Style.Border.Right.Style = worksheet.Cells[$"G{row}"].Style.Border.Top.Style = worksheet.Cells[$"G{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                            worksheet.Cells[$"H{row}"].Style.Border.Right.Style = worksheet.Cells[$"H{row}"].Style.Border.Top.Style = worksheet.Cells[$"H{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            row++;
                        }

                        //worksheet.Cells["A1:G1"].Merge = true;
                        //worksheet.Cells["A1:G1"].Value = planObj.Result.Replace(" -", "\n");
                        //worksheet.Cells["A1:G1"].Style.WrapText = true;

                        //worksheet.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //worksheet.Row(1).Style.Font.Bold = true;
                        //worksheet.Row(1).Style.Font.Size = 16;
                        //worksheet.Row(1).Style.Font.Name = "Times News Roman";
                        //worksheet.Row(1).Style.Font.Color.SetColor(Color.FromArgb(0, 0, 117));
                        //worksheet.Row(1).Height = 15* (planObj.Result.Split('-').Length);

                        //worksheet.Cells["A2:G2"].Merge = true;
                        //worksheet.Cells["A2:G2"].Value = planObj.Name;
                        //worksheet.Cells["A2:G2"].Style.WrapText = true;

                        //worksheet.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //worksheet.Row(2).Style.Font.Bold = true;
                        //worksheet.Row(2).Style.Font.Size = 16;
                        //worksheet.Row(2).Style.Font.Name = "Times News Roman";
                        //worksheet.Row(2).Style.Font.Color.SetColor(Color.FromArgb(0, 0, 117));

                        //worksheet.Cells["B3:G3"].Merge = true;
                        //worksheet.Cells["B3:G3"].Value = "NGÀY 31 THÁNG 5 NĂM 2019" +"\n" + "Second sentence";
                        //worksheet.Cells["B3:G3"].Style.WrapText = true;
                        //worksheet.Row(3).Height = 30;

                        //worksheet.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //worksheet.Row(3).Style.Font.Bold = true;
                        //worksheet.Row(3).Style.Font.Size = 10;
                        //worksheet.Row(3).Style.Font.Name = "Times News Roman";
                        //worksheet.Row(3).Style.Font.Color.SetColor(Color.FromArgb(0, 0, 117));
                        //worksheet.Cells["A3:G3"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                        xlPackage.Save();

                    }
                    bytes = stream.ToArray();
                }
                return File(bytes, "text/xls", StringUtils.ReplaceVietnameseChar(planObj.Name) + ".xlsx");
            }
            catch (Exception ex)
            {
                NLogLogger.DebugMessage(ex);
                return RedirectToAction("Index");
            }
        }
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public ActionResult Detail(int Id)
        {
            var planObj = _planService.GetPlan(Id);
            ViewBag.Id = Id;
            ViewBag.Name = planObj.Name;
            ViewBag.Year = planObj.Year;
            ViewBag.Type = planObj.Type;
            return View();
        }

        #region "Tiến độ giải ngân "
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public ActionResult GetReport(int type, int year, string key)
        {
            var data = _projectreportservice.GetList("", CurrentUser.Username, year, -1, -1, "");
            data = data.Where(x => x.Type == type).ToList();
            if (!string.IsNullOrEmpty(key))
            {
                data = data.Where(x => x.Name.Contains(key)).ToList();
            }
            //data = data.Where(x => x.Type == type && x.Name.Contains(key)).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public ActionResult ListPlanItem(int planId)
        {

            var data = new List<PlanItem>();

            data = _planItemService.GetList(planId);
            ViewBag.Total1 = data.Sum(x => x.Total1);
            ViewBag.Total2 = data.Sum(x => x.Total2);
            ViewBag.NumberPeople = data.Sum(x => x.NumberPeople);
            ViewBag.WomanRate = "";
            if (data.Count > 0)
            {
                ViewBag.WomanRate = data.Sum(x => x.WomanRate) / data.Count;
            }
            return PartialView(data);
        }
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public ActionResult AddPlanItem(int planId)
        {
            var model = new PlanItemModel
            {
                PlanId = planId,
                Name = "",
                Status = 1,
                Item1 = new PlanItemData(),
                Item2 = new PlanItemData(),
                Item3 = new PlanItemData(),
            };
            return PartialView("PlanItemDetail", model);
        }
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public ActionResult PlanItemDetail(int Id)
        {
            var data = _planItemService.GetPlanItem(Id);
            var model = new PlanItemModel
            {
                Id = data.Id,
                Name = data.Name,
                Total1 = data.Total1,
                Total2 = data.Total2,
                PlanId = data.PlanId,
                Status = data.Status,

                NumberPeople = data.NumberPeople,
                WomanRate = data.WomanRate,
                Time = data.Time,
            };
            model.Item1 = JsonConvert.DeserializeObject<PlanItemData>(data.Config1);
            model.Item2 = JsonConvert.DeserializeObject<PlanItemData>(data.Config2);
            model.Item3 = JsonConvert.DeserializeObject<PlanItemData>(data.Config3);
            return PartialView(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public JsonResult PlanItemSaveData(PlanItemModel data, string STime)
        {
            var ReturnData = new ReturnData();

            try
            {
                IFormatProvider culture = new CultureInfo("en-US", true);
                data.Time = DateTime.ParseExact(STime, "dd/MM/yyyy", culture);
                data.Item1 = new PlanItemData(data.Currency1, data.CurrencyRate1, data.PlanYear1, data.PlanCurrent1, data.PlanQ1, data.BalanceYear1, data.Balance1, data.Money1);
                data.Item2 = new PlanItemData(data.Currency2, data.CurrencyRate2, data.PlanYear2, data.PlanCurrent2, data.PlanQ2, data.BalanceYear2, data.Balance2, data.Money2);
                data.Item3 = new PlanItemData(data.Currency3, data.CurrencyRate3, data.PlanYear3, data.PlanCurrent3, data.PlanQ3, data.BalanceYear3, data.Balance3, data.Money3);
                var obj = new PlanItem
                {
                    Id = data.Id,
                    Name = data.Name,
                    PlanId = data.PlanId,
                    Status = data.Status,
                    Total1 = data.Total1,
                    Total2 = data.Total2,
                    NumberPeople = data.NumberPeople,
                    WomanRate = data.WomanRate,
                    Time = data.Time,
                    Config1 = JsonConvert.SerializeObject(data.Item1),
                    Config2 = JsonConvert.SerializeObject(data.Item2),
                    Config3 = JsonConvert.SerializeObject(data.Item3),
                };
                var lstdata = _planItemService.GetList(data.PlanId.Value);
                if (lstdata.Exists(x => x.Name.Equals(data.Name)))
                {
                    obj.Id = lstdata.FirstOrDefault(x => x.Name.Equals(data.Name)).Id;
                }
                //thêm vướng mắc
                if (obj.Id < 1)
                {
                    var PlanStuck = new PlanStuck
                    {
                        Name = data.Name,
                        Description = " ",
                        Organ = "",
                        Result1 = "",
                        Result2 = "",
                        Status = 1,
                        PlanId = data.PlanId
                    };
                    _planStuckService.CreateUpdatePlanStuck(PlanStuck);
                }
                var result = _planItemService.CreateUpdatePlanItem(obj);



                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (obj.Id > 0)
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
        public JsonResult PlanItemUpdateSortOrder(int Id, bool SortOrder, int PlanId)
        {
            try
            {
                var updateResult = _planItemService.UpdateOrder(Id, SortOrder, PlanId);
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
        public JsonResult PlanItemUpdateStatus(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = _planItemService.UpdateStatus(id);


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

        public ActionResult ListPlanStuck(int planId)
        {

            var data = new List<PlanStuck>();

            data = _planStuckService.GetList(planId);

            return PartialView(data);
        }
        public ActionResult AddPlanStuck(int planId)
        {
            var model = new PlanStuck
            {
                PlanId = planId,
                Status = 1,
            };
            return PartialView("PlanStuckDetail", model);
        }
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public ActionResult PlanStuckDetail(int Id)
        {
            var model = _planStuckService.GetPlanStuck(Id);
            return PartialView(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public JsonResult PlanStuckSaveData(PlanStuck Plan)
        {
            var ReturnData = new ReturnData();

            try
            {
                //banner.ImageUrl = Config.UrlRoot + (string.IsNullOrEmpty(banner.ImageUrl) ? string.Empty : banner.ImageUrl.Substring(12));

                Plan.Order = Convert.ToInt32(Plan.Order);
                //Plan.UserName = CurrentUser.Username;
                var result = _planStuckService.CreateUpdatePlanStuck(Plan);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (Plan.Id > 0)
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
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public JsonResult PlanStuckUpdateSortOrder(int Id, bool SortOrder, int PlanId)
        {
            try
            {
                var updateResult = _planStuckService.UpdateOrder(Id, SortOrder, PlanId);
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
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public JsonResult PlanStuckUpdateStatus(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = _planStuckService.UpdateStatus(id);
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

        #endregion

        public ActionResult ListPlanRequire(int planId)
        {

            var data = new List<PlanRequire>();

            data = _planRequireService.GetList(planId);

            return PartialView(data);
        }
        public ActionResult AddPlanRequire(int planId)
        {
            var model = new PlanRequire
            {
                PlanId = planId,
                Status = 1,
                Total = 0,
                Total2 = 0,
                Total1 = 0,
                Result = "",
            };
            return PartialView("PlanRequireDetail", model);
        }
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public ActionResult PlanRequireDetail(int Id)
        {
            var model = _planRequireService.GetPlanRequire(Id);
            return PartialView(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public JsonResult PlanRequireSaveData(PlanRequire Plan)
        {
            var ReturnData = new ReturnData();

            try
            {
                //banner.ImageUrl = Config.UrlRoot + (string.IsNullOrEmpty(banner.ImageUrl) ? string.Empty : banner.ImageUrl.Substring(12));

                Plan.Order = Convert.ToInt32(Plan.Order);
                //Plan.UserName = CurrentUser.Username;
                var result = _planRequireService.CreateUpdatePlanRequire(Plan);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (Plan.Id > 0)
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
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public JsonResult PlanRequireUpdateSortOrder(int Id, bool SortOrder, int PlanId)
        {
            try
            {
                var updateResult = _planRequireService.UpdateOrder(Id, SortOrder, PlanId);
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
        [PermissionFilter(FunctionCode = FunctionCode.BankReport)]
        public JsonResult PlanRequireUpdateStatus(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = _planRequireService.UpdateStatus(id);
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