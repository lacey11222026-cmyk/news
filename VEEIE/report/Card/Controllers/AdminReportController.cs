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
namespace Car.CMS.Controllers
{
    public class AdminReportController : Controller
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
        public AdminReportController(IPlanRequiresService planRequireService, IPlansService planService, IPlanItemsService planItemService, IPlanStucksService planStuckService, IProjectReportsService projectreportservice, IProjectsService projectservice, IUsersService userservice, IUsersLogService userlogservice, IFucntionsService functionservice, IUserRoleService userroleservice)
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
        [PermissionFilter(FunctionCode = FunctionCode.AdminReport)]
        public ActionResult Index()
        {
            ViewBag.Title = "Báo cáo tổng hợp";
            ViewBag.Type = CurrentFullUser.Type;
            return View();
        }
        [PermissionFilter(FunctionCode = FunctionCode.AdminReport)]
        public ActionResult ListReport(int? type, int? year)
        {
            int Type = type == null ? -1 : (int)type;
            int Year = year == null ? -1 : (int)year;
            var data = new List<Plan>();
            data = _planService.GetList("", Year, Type, 1).Where(x=>x.Type>0).ToList();
            var ListName=data.GroupBy(x => x.Name).Select(a => a.Key).ToList();
            ViewBag.ListName = ListName;
            return PartialView(data);
        }
        [PermissionFilter(FunctionCode = FunctionCode.AdminReport)]
        public ActionResult View(int year,int type)
        {

           
            var title ="Ban Quản lý Dự án TKNL cho ngành CN Việt Nam - ";
            if (type == 5)
            {
                title += "Năm " + year;
            }
            else
            {
                title += HtmlHelpers.GetReportType(type) + " Năm " + year;
            }
            var model = new PlanDetail();
            model.PlanItemAll = new List<PlanItemModel>();
            model.PlanItem = new List<PlanItemModel>();
            model.PlanItemBidv = new List<PlanItemModel>();
            model.PlanRequire = new List<PlanRequire>();
            model.PlanRequireBidv = new List<PlanRequire>();
            var report = _planService.GetList("vcb", year, -1, 1).Where(x=>x.Type.GetValueOrDefault()==type).ToList();
            if(report.Count>0)
            {
                var lstPlanItem = _planItemService.GetList(report.FirstOrDefault().Id).Where(x => x.Status == 1).ToList();
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
                        Time=item.Time,
                        WomanRate=item.WomanRate,
                        NumberPeople=item.NumberPeople,
                    };
                    newItem.Item1 = JsonConvert.DeserializeObject<PlanItemData>(item.Config1);
                    newItem.Item2 = JsonConvert.DeserializeObject<PlanItemData>(item.Config2);
                    newItem.Item3 = JsonConvert.DeserializeObject<PlanItemData>(item.Config3);
                    model.PlanItem.Add(newItem);
                    model.PlanItemAll.Add(newItem);
                    model.PlanRequire = _planRequireService.GetList(report.FirstOrDefault().Id);
                }
                //model.PlanItemAll = model.PlanItem;
            }

            var reportBidv = _planService.GetList("bidv", year, -1, 1).Where(x => x.Type.GetValueOrDefault() == type).ToList();
            if (reportBidv.Count > 0)
            {
                var lstPlanItem = _planItemService.GetList(reportBidv.FirstOrDefault().Id).Where(x => x.Status == 1).ToList();
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
                        WomanRate = item.WomanRate,
                        NumberPeople = item.NumberPeople,
                    };
                    newItem.Item1 = JsonConvert.DeserializeObject<PlanItemData>(item.Config1);
                    newItem.Item2 = JsonConvert.DeserializeObject<PlanItemData>(item.Config2);
                    newItem.Item3 = JsonConvert.DeserializeObject<PlanItemData>(item.Config3);
                    model.PlanItemBidv.Add(newItem);
                    model.PlanItemAll.Add(newItem);
                    model.PlanRequireBidv = _planRequireService.GetList(reportBidv.FirstOrDefault().Id);
                }
                
            }
            //model.PlanRequire = _planRequireService.GetList(id);
            //model.PlanStuck = _planStuckService.GetList(id);



            //model.Plan = planObj;
            ViewBag.Title = title;
            return View(model);
        }

        [HttpGet]
        [PermissionFilter(FunctionCode = FunctionCode.AdminReport)]
        public ActionResult ExportExcel(int year, int type)
        {

            //var planObj = _planService.GetPlan(id);

            try
            {
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        var worksheet = xlPackage.Workbook.Worksheets.Add("Report");


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
                        worksheet.Column(17).Width = 16;
                        var allCells = worksheet.Cells[1, 1, 50, 50];
                        var cellFont = allCells.Style.Font;
                        cellFont.SetFromFont(new Font("Times New Roman", 12));


                        worksheet.Cells["A1:D1"].Merge = true;
                        worksheet.Cells["A1:D1"].Value = "Ban Quản lý Dự án TKNL cho ngành CN Việt Nam";
                        worksheet.Cells["A1:D1"].Style.WrapText = true;

                        worksheet.Cells["G1:L1"].Merge = true;
                        worksheet.Cells["G1:L1"].Value = "Cộng hòa xã hội chủ nghĩa Việt Nam";
                        worksheet.Cells["G1:L1"].Style.WrapText = true;

                        worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;



                        worksheet.Cells["A2:D2"].Merge = true;
                        if (type == 5)
                        {
                            worksheet.Cells["A2:D2"].Value = "Năm báo cáo: " + year;
                        }
                        else
                        {
                            worksheet.Cells["A2:D2"].Value = "Kỳ báo cáo: " + HtmlHelpers.GetReportType(type);
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
                        if (type == 5)
                        {
                            worksheet.Cells["D5:G5"].Value = "Năm " +year;
                        }
                        else
                        {
                            worksheet.Cells["D5:G5"].Value = HtmlHelpers.GetReportType(type) + " Năm " +year;
                        }

                        worksheet.Cells["D5:G5"].Style.WrapText = true;

                        worksheet.Row(5).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Row(5).Style.Font.Italic = true;


                        //Hear Table
                        worksheet.Cells["A7:B7"].Merge = true;
                        if (type == 5)
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
                        worksheet.Cells["K8:K9"].Value = "Lũy kế giải ngân từ đầu dự án (USD)";
                        worksheet.Cells["K8:K9"].Style.Border.Right.Style = worksheet.Cells["K8:K9"].Style.Border.Top.Style = worksheet.Cells["K8:K9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                        worksheet.Cells["L8:L9"].Merge = true;
                        worksheet.Cells["L8:L9"].Value = "Mức TKNL đạt được (MWh/năm)";
                        worksheet.Cells["L8:L9"].Style.Border.Right.Style = worksheet.Cells["L8:L9"].Style.Border.Top.Style = worksheet.Cells["L8:L9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["M8:M9"].Merge = true;
                        worksheet.Cells["M8:M9"].Value = "Mức TKNL đạt được (MJ/năm)";
                        worksheet.Cells["M8:M9"].Style.Border.Right.Style = worksheet.Cells["M8:M9"].Style.Border.Top.Style = worksheet.Cells["M8:M9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["N8:N9"].Merge = true;
                        worksheet.Cells["N8:N9"].Value = "Giảm phát thải KNK (Tấn CO2/năm)";
                        worksheet.Cells["N8:N9"].Style.Border.Right.Style = worksheet.Cells["N8:N9"].Style.Border.Top.Style = worksheet.Cells["N8:N9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["O8:O9"].Merge = true;
                        worksheet.Cells["O8:O9"].Value = "Số lượng người hưởng lợi trực tiếp)";
                        worksheet.Cells["O8:O9"].Style.Border.Right.Style = worksheet.Cells["O8:O9"].Style.Border.Top.Style = worksheet.Cells["O8:O9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["P8:P9"].Merge = true;
                        worksheet.Cells["P8:P9"].Value = "Tỷ lệ người hưởng lợi là phụ nữ (%)";
                        worksheet.Cells["P8:P9"].Style.Border.Right.Style = worksheet.Cells["P8:P9"].Style.Border.Top.Style = worksheet.Cells["P8:P9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["Q8:Q9"].Merge = true;
                        worksheet.Cells["Q8:Q9"].Value = "Ngày bắt đầu vận hành";
                        worksheet.Cells["Q8:Q9"].Style.Border.Right.Style = worksheet.Cells["Q8:Q9"].Style.Border.Top.Style = worksheet.Cells["Q8:Q9"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["E8:I8"].Merge = true;
                        worksheet.Cells["E8:I8"].Value = "Giải ngân trong năm (VND)";
                        worksheet.Cells["E8:I8"].Style.Border.Right.Style = worksheet.Cells["E8:I8"].Style.Border.Top.Style = worksheet.Cells["E8:I8"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["E9"].Value = "Kế hoạch giải ngân năm";


                        worksheet.Cells["F9"].Value = "Kế hoạch tới thời điểm báo cáo";
                        if (type == 5)
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
                        worksheet.Cells["Q10"].Style.Border.Right.Style = worksheet.Cells["Q10"].Style.Border.Top.Style = worksheet.Cells["Q10"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Row(10).Style.Font.Bold = true;
                        worksheet.Row(10).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(10).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        worksheet.Row(10).Style.WrapText = true;

                        worksheet.Cells["A11:P11"].Merge = true;
                        worksheet.Cells["A11:P11"].Value = "Ngân hàng thương mại cổ phần Ngoại thương Việt Nam";
                        worksheet.Cells["A11:P11"].Style.Border.Right.Style = worksheet.Cells["A11:L11"].Style.Border.Top.Style = worksheet.Cells["A11:L11"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Row(11).Style.Font.Bold = true;
                        worksheet.Row(11).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Row(11).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        worksheet.Row(11).Style.WrapText = true;

                        int row = 12;
                        //Danh sách dự án
                        var report = _planService.GetList("vcb", year, -1, 1).Where(x => x.Type.GetValueOrDefault() == type).ToList();
                        var lstPRow = new List<int>();
                        if (report.Count>0)
                        {
                            var lstPlanItem = _planItemService.GetList(report.FirstOrDefault().Id).Where(x => x.Status == 1).ToList();
                            if (lstPlanItem.Count > 0)
                            {


                                for (int i = 0; i < lstPlanItem.Count; i++)
                                {

                                    lstPRow.Add(row + 1);
                                    var item = lstPlanItem[i];
                                    worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                                    worksheet.Row(row + 1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                                    worksheet.Row(row + 2).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                                    worksheet.Row(row + 3).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                                    //tên dự án
                                    worksheet.Cells[$"A{row}:K{row}"].Merge = true;
                                    worksheet.Cells[$"A{row}:K{row}"].Value = item.Name;
                                    worksheet.Cells[$"A{row}:K{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}:K{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}:K{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"A{row}:K{row}"].Style.Font.Bold = true;
                                    worksheet.Cells[$"A{row}:K{row}"].Style.WrapText = true;

                                   
                                    worksheet.Cells[$"L{row}:L{row + 3}"].Merge = true;
                                    worksheet.Cells[$"L{row}:L{row + 3}"].Value = item.Total1;
                                    worksheet.Cells[$"L{row}:L{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"L{row}:L{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"L{row}:L{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"M{row}:M{row + 3}"].Merge = true;
                                    worksheet.Cells[$"M{row}:M{row + 3}"].Formula = $"L{row } *3600";
                                    worksheet.Cells[$"M{row}:M{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"M{row}:M{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"M{row}:M{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"N{row}:N{row + 3}"].Merge = true;
                                    worksheet.Cells[$"N{row}:N{row + 3}"].Value = item.Total2;
                                    worksheet.Cells[$"N{row}:N{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"N{row}:N{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"N{row}:N{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"O{row}:O{row + 3}"].Merge = true;
                                    worksheet.Cells[$"O{row}:O{row + 3}"].Value = item.NumberPeople;
                                    worksheet.Cells[$"O{row}:O{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"O{row}:O{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"O{row}:O{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"P{row}:P{row + 3}"].Merge = true;
                                    worksheet.Cells[$"P{row}:P{row + 3}"].Value = item.WomanRate;
                                    worksheet.Cells[$"P{row}:P{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"P{row}:P{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"P{row}:P{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"Q{row}:Q{row + 3}"].Merge = true;
                                    worksheet.Cells[$"Q{row}:Q{row + 3}"].Value = item.Time.GetValueOrDefault().ToString("dd/MM/yyyy");
                                    worksheet.Cells[$"Q{row}:Q{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"Q{row}:Q{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"Q{row}:Q{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


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

                                    worksheet.Cells[$"K{row + 1}"].Style.Numberformat.Format = "#,##0";
                                    worksheet.Cells[$"K{row + 1}"].Formula = $"ROUND(J{row + 1}/D{row + 1},3)";
                                    worksheet.Cells[$"K{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"K{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"K{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                                    worksheet.Cells[$"K{row + 2}"].Style.Numberformat.Format = "#,##0";
                                    worksheet.Cells[$"K{row + 2}"].Formula = $"ROUND(J{row + 2}/D{row + 2},3)";
                                    worksheet.Cells[$"K{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"K{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"K{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"K{row + 3}"].Style.Numberformat.Format = "#,##0";
                                    worksheet.Cells[$"K{row + 3}"].Formula = $"ROUND(J{row + 3}/D{row + 3},3)";
                                    worksheet.Cells[$"K{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"K{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"K{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    row += 4;
                                }




                            }
                        }
                       
                        var reportBIDV = _planService.GetList("bidv", year, -1, 1).Where(x => x.Type.GetValueOrDefault() == type).ToList();
                        worksheet.Cells[$"A{row}:P{row}"].Merge = true;
                        worksheet.Cells[$"A{row}:P{row}"].Value = "Ngân hàng TMCP Đầu tư và Phát triển Việt Nam";
                        worksheet.Cells[$"A{row}:P{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}:P{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}:P{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Row(row).Style.Font.Bold = true;
                        worksheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        worksheet.Row(row).Style.WrapText = true;
                        row++;
                        if (reportBIDV.Count > 0)
                        {
                            var lstPlanItem = _planItemService.GetList(reportBIDV.FirstOrDefault().Id).Where(x => x.Status == 1).ToList();
                            if (lstPlanItem.Count > 0)
                            {


                                for (int i = 0; i < lstPlanItem.Count; i++)
                                {

                                    lstPRow.Add(row + 1);
                                    var item = lstPlanItem[i];
                                    worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                                    worksheet.Row(row + 1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                                    worksheet.Row(row + 2).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                                    worksheet.Row(row + 3).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                                    //tên dự án
                                    worksheet.Cells[$"A{row}:K{row}"].Merge = true;
                                    worksheet.Cells[$"A{row}:K{row}"].Value = item.Name;
                                    worksheet.Cells[$"A{row}:K{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}:K{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}:K{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"A{row}:K{row}"].Style.Font.Bold = true;
                                    worksheet.Cells[$"A{row}:K{row}"].Style.WrapText = true;

                                    //worksheet.Cells[$"K{row}:K{row + 3}"].Merge = true;
                                    //worksheet.Cells[$"K{row}:K{row + 3}"].Value = item.Total1;
                                    //worksheet.Cells[$"K{row}:K{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"K{row}:K{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"K{row}:K{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"L{row}:L{row + 3}"].Merge = true;
                                    worksheet.Cells[$"L{row}:L{row + 3}"].Value = item.Total1;
                                    worksheet.Cells[$"L{row}:L{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"L{row}:L{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"L{row}:L{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"M{row}:M{row + 3}"].Merge = true;
                                    worksheet.Cells[$"M{row}:M{row + 3}"].Formula = $"L{row } *3600";
                                    worksheet.Cells[$"M{row}:M{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"M{row}:M{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"M{row}:M{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"N{row}:N{row + 3}"].Merge = true;
                                    worksheet.Cells[$"N{row}:N{row + 3}"].Value = item.Total2;
                                    worksheet.Cells[$"N{row}:N{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"N{row}:N{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"N{row}:N{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"O{row}:O{row + 3}"].Merge = true;
                                    worksheet.Cells[$"O{row}:O{row + 3}"].Value = item.NumberPeople;
                                    worksheet.Cells[$"O{row}:O{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"O{row}:O{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"O{row}:O{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"P{row}:P{row + 3}"].Merge = true;
                                    worksheet.Cells[$"P{row}:P{row + 3}"].Value = item.WomanRate;
                                    worksheet.Cells[$"P{row}:P{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"P{row}:P{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"P{row}:P{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"Q{row}:Q{row + 3}"].Merge = true;
                                    worksheet.Cells[$"Q{row}:Q{row + 3}"].Value = item.Time.GetValueOrDefault().ToString("dd/MM/yyyy");
                                    worksheet.Cells[$"Q{row}:Q{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"Q{row}:Q{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"Q{row}:Q{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


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


                                    worksheet.Cells[$"K{row + 1}"].Style.Numberformat.Format = "#,##0.000";
                                    worksheet.Cells[$"K{row + 1}"].Formula = $"ROUND(J{row + 1}/D{row + 1},3)";
                                    worksheet.Cells[$"K{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"K{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"K{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"K{row + 2}"].Style.Numberformat.Format = "#,##0.000";
                                    worksheet.Cells[$"K{row + 2}"].Formula = $"ROUND(J{row + 2}/D{row + 2},3)";
                                    worksheet.Cells[$"K{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"K{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"K{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"K{row + 3}"].Style.Numberformat.Format = "#,##0.000";
                                    worksheet.Cells[$"K{row + 3}"].Formula = $"ROUND(J{row + 3}/D{row + 3},3)";
                                    worksheet.Cells[$"K{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"K{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"K{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    row += 4;
                                }




                            }
                        }
                        //tổng
                        worksheet.Cells[$"A{row}:K{row}"].Merge = true;
                        worksheet.Cells[$"A{row}:K{row}"].Value = "Tổng cộng";
                        worksheet.Cells[$"A{row}:K{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}:K{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}:K{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[$"A{row}:K{row}"].Style.Font.Bold = true;
                        worksheet.Cells[$"A{row}:K{row}"].Style.WrapText = true;
                        worksheet.Cells[$"A{row}:K{row}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[$"A{row}:K{row}"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;

                      
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

                        worksheet.Cells[$"Q{row}:Q{row + 3}"].Merge = true;
                        worksheet.Cells[$"Q{row}:Q{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"Q{row}:Q{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"Q{row}:Q{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


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
                        //var forJ = "";
                        //var forP = "";

                        var forM = "";
                        var forN = "";
                        var forO = "";
                        var forP = "(";
                        var forB2 = "";
                        var forC2 = "";
                        var forE2 = "";
                        var forF2 = "";
                        var forG2 = "";
                        var forH2 = "";
                        var forJ2 = "";
                        var forK2 = "";

                        var forB3 = "";
                        var forC3 = "";
                        var forE3 = "";
                        var forF3 = "";
                        var forG3 = "";
                        var forH3 = "";
                        var forJ3 = "";
                        var forK3 = "";
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
                            //forP += $"+P{PRow}";
                            forM += $"+M{PRow}";
                            forN += $"+N{PRow}";
                            forO += $"+O{PRow}";
                            forP += $"+P{PRow}";
                            forB2 += $"+B{PRow + 1}";
                            forC2 += $"+C{PRow + 1}";
                            forE2 += $"+E{PRow + 1}";
                            forF2 += $"+F{PRow + 1}";
                            forG2 += $"+G{PRow + 1}";
                            forH2 += $"+H{PRow + 1}";
                            forJ2 += $"+J{PRow + 1}";
                            forK2 += $"+K{PRow + 1}";

                            forB3 += $"+B{PRow + 2}";
                            forC3 += $"+C{PRow + 2}";
                            forE3 += $"+E{PRow + 2}";
                            forF3 += $"+F{PRow + 2}";
                            forG3 += $"+G{PRow + 2}";
                            forH3 += $"+H{PRow + 2}";
                            forJ3 += $"+J{PRow + 2}";
                            forK3 += $"+K{PRow + 2}";

                        }
                        forP += ")/"+ lstPRow.Count;
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

                        worksheet.Cells[$"K{row + 1}"].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells[$"K{row + 1}"].Formula = forK;
                        worksheet.Cells[$"K{row + 1}"].Style.Border.Right.Style = worksheet.Cells[$"K{row + 1}"].Style.Border.Top.Style = worksheet.Cells[$"K{row + 1}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells[$"K{row + 2}"].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells[$"K{row + 2}"].Formula = forK2;
                        worksheet.Cells[$"K{row + 2}"].Style.Border.Right.Style = worksheet.Cells[$"K{row + 2}"].Style.Border.Top.Style = worksheet.Cells[$"K{row + 2}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells[$"K{row + 3}"].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells[$"K{row + 3}"].Formula = forK3;
                        worksheet.Cells[$"K{row + 3}"].Style.Border.Right.Style = worksheet.Cells[$"K{row + 3}"].Style.Border.Top.Style = worksheet.Cells[$"K{row + 3}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;




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

                        worksheet.Cells[$"P{row }"].Formula = forP;
                        worksheet.Cells[$"P{row }"].Style.Border.Right.Style = worksheet.Cells[$"P{row }"].Style.Border.Top.Style = worksheet.Cells[$"P{row }"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

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
                       
                        worksheet.Cells[$"A{row}:L{row}"].Merge = true;
                        worksheet.Cells[$"A{row}:L{row}"].Value = "";
                        worksheet.Row(row).Height = 16 ;
                        worksheet.Cells[$"A{row}:L{row}"].Style.WrapText = true;
                        row++;

                        worksheet.Cells[$"A{row}"].Value = "II.2 Quản lý đấu thầu";
                        worksheet.Cells[$"A{row}"].Style.WrapText = false;
                        row++;
                      
                        worksheet.Cells[$"A{row}:L{row}"].Merge = true;
                        worksheet.Cells[$"A{row}:L{row}"].Value = "";
                        worksheet.Row(row).Height = 16 ;
                        worksheet.Cells[$"A{row}:L{row}"].Style.WrapText = true;
                        row++;

                        worksheet.Cells[$"A{row}"].Value = "II.3 Các vấn đề về an toàn môi trường, xã hội";
                        worksheet.Cells[$"A{row}"].Style.WrapText = false;
                        row++;
                       
                        worksheet.Cells[$"A{row}:L{row}"].Merge = true;
                        worksheet.Cells[$"A{row}:L{row}"].Value ="";
                        worksheet.Row(row).Height = 16 ;
                        worksheet.Cells[$"A{row}:L{row}"].Style.WrapText = true;
                        row++;

                        worksheet.Cells[$"A{row}"].Value = "II.4 Các vấn đề khác";
                        worksheet.Cells[$"A{row}"].Style.WrapText = false;
                        row++;
                        
                        worksheet.Cells[$"A{row}:L{row}"].Merge = true;
                        worksheet.Cells[$"A{row}:L{row}"].Value ="";
                        worksheet.Row(row).Height = 16 ;
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
                        

                        worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                       

                        worksheet.Cells[$"A{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[$"A{row}"].Value =1;
                        worksheet.Cells[$"A{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells[$"B{row}"].Value = "";
                        worksheet.Cells[$"B{row}"].Style.Border.Right.Style = worksheet.Cells[$"B{row}"].Style.Border.Top.Style = worksheet.Cells[$"B{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[$"B{row}"].Style.WrapText = true;

                        worksheet.Cells[$"C{row}"].Value = "";
                        worksheet.Cells[$"C{row}"].Style.Border.Right.Style = worksheet.Cells[$"C{row}"].Style.Border.Top.Style = worksheet.Cells[$"C{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[$"C{row}"].Style.WrapText = true;

                        worksheet.Cells[$"D{row}"].Value = "";
                        worksheet.Cells[$"D{row}"].Style.Border.Right.Style = worksheet.Cells[$"D{row}"].Style.Border.Top.Style = worksheet.Cells[$"D{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[$"D{row}"].Style.WrapText = true;

                        worksheet.Cells[$"E{row}"].Value = "";
                        worksheet.Cells[$"E{row}"].Style.Border.Right.Style = worksheet.Cells[$"E{row}"].Style.Border.Top.Style = worksheet.Cells[$"E{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[$"E{row}"].Style.WrapText = true;

                        worksheet.Cells[$"F{row}"].Style.WrapText = true;
                        worksheet.Cells[$"F{row}"].Value = "";
                        worksheet.Cells[$"F{row}"].Style.Border.Right.Style = worksheet.Cells[$"F{row}"].Style.Border.Top.Style = worksheet.Cells[$"F{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells[$"G{row}"].Style.WrapText = true;
                        worksheet.Cells[$"G{row}"].Value = "";
                        worksheet.Cells[$"G{row}"].Style.Border.Right.Style = worksheet.Cells[$"G{row}"].Style.Border.Top.Style = worksheet.Cells[$"G{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        row++;
                        worksheet.Cells[$"A{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[$"A{row}"].Value = 1;
                        worksheet.Cells[$"A{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells[$"B{row}"].Value = "";
                        worksheet.Cells[$"B{row}"].Style.Border.Right.Style = worksheet.Cells[$"B{row}"].Style.Border.Top.Style = worksheet.Cells[$"B{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[$"B{row}"].Style.WrapText = true;

                        worksheet.Cells[$"C{row}"].Value = "";
                        worksheet.Cells[$"C{row}"].Style.Border.Right.Style = worksheet.Cells[$"C{row}"].Style.Border.Top.Style = worksheet.Cells[$"C{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[$"C{row}"].Style.WrapText = true;

                        worksheet.Cells[$"D{row}"].Value = "";
                        worksheet.Cells[$"D{row}"].Style.Border.Right.Style = worksheet.Cells[$"D{row}"].Style.Border.Top.Style = worksheet.Cells[$"D{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[$"D{row}"].Style.WrapText = true;

                        worksheet.Cells[$"E{row}"].Value = "";
                        worksheet.Cells[$"E{row}"].Style.Border.Right.Style = worksheet.Cells[$"E{row}"].Style.Border.Top.Style = worksheet.Cells[$"E{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[$"E{row}"].Style.WrapText = true;

                        worksheet.Cells[$"F{row}"].Style.WrapText = true;
                        worksheet.Cells[$"F{row}"].Value = "";
                        worksheet.Cells[$"F{row}"].Style.Border.Right.Style = worksheet.Cells[$"F{row}"].Style.Border.Top.Style = worksheet.Cells[$"F{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells[$"G{row}"].Style.WrapText = true;
                        worksheet.Cells[$"G{row}"].Value = "";
                        worksheet.Cells[$"G{row}"].Style.Border.Right.Style = worksheet.Cells[$"G{row}"].Style.Border.Top.Style = worksheet.Cells[$"G{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        row++;
                        row++;

                        //Kế hoạch
                        worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        worksheet.Row(row + 1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;

                        if (type == 5)
                        {
                            worksheet.Cells[$"A{row}"].Value = "IV. Kế hoạch trong năm tiếp theo";
                        }
                        else
                        {
                            worksheet.Cells[$"A{row}"].Value = "IV. Kế hoạch trong quý tiếp theo";
                        }
                        worksheet.Cells[$"A{row}"].Style.WrapText = false;
                        row++;
                        
                        worksheet.Cells[$"A{row}:L{row}"].Merge = true;
                        worksheet.Cells[$"A{row}:L{row}"].Value = "";
                        worksheet.Row(row).Height = 32;
                        worksheet.Cells[$"A{row}:L{row}"].Style.WrapText = true;
                        row += 2;
                        //Các vấn đề khác
                        worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        worksheet.Row(row + 1).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        worksheet.Cells[$"A{row}"].Value = "V. Các vấn đề khác";
                        worksheet.Cells[$"A{row}"].Style.WrapText = false;
                        row++;
                       
                        worksheet.Cells[$"A{row}:L{row}"].Merge = true;
                        worksheet.Cells[$"A{row}:L{row}"].Value = "";
                        worksheet.Row(row).Height = 16 ;
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
                        worksheet.Cells[$"B{row}"].Value = "Tên và địa điểm thực hiện dự án";
                        worksheet.Cells[$"B{row}"].Style.Border.Right.Style = worksheet.Cells[$"B{row}"].Style.Border.Top.Style = worksheet.Cells[$"B{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;



                        worksheet.Cells[$"C{row}"].Value = "Loại dự án";
                        worksheet.Cells[$"C{row}"].Style.Border.Right.Style = worksheet.Cells[$"C{row}"].Style.Border.Top.Style = worksheet.Cells[$"C{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                        worksheet.Cells[$"D{row}"].Style.WrapText = true;
                        worksheet.Cells[$"D{row}"].Value = "Tổng chi phí đầu tư(triệu VND)";
                        worksheet.Cells[$"D{row}"].Style.Border.Right.Style = worksheet.Cells[$"D{row}"].Style.Border.Top.Style = worksheet.Cells[$"D{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;



                        worksheet.Cells[$"E{row}"].Style.WrapText = true;
                        worksheet.Cells[$"E{row}"].Value = "Tổng vay(triệu VND)";
                        worksheet.Cells[$"E{row}"].Style.Border.Right.Style = worksheet.Cells[$"E{row}"].Style.Border.Top.Style = worksheet.Cells[$"E{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;



                        worksheet.Cells[$"F{row}"].Style.WrapText = true;
                        worksheet.Cells[$"F{row}"].Value = "Vốn vay IBRD (triệu VND)";
                        worksheet.Cells[$"F{row}"].Style.Border.Right.Style = worksheet.Cells[$"F{row}"].Style.Border.Top.Style = worksheet.Cells[$"F{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                        worksheet.Cells[$"G{row}"].Style.WrapText = true;
                        worksheet.Cells[$"G{row}"].Value = "Hiện trạng";
                        worksheet.Cells[$"G{row}"].Style.Border.Right.Style = worksheet.Cells[$"G{row}"].Style.Border.Top.Style = worksheet.Cells[$"G{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Row(row).Style.Font.Bold = true;
                        worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        worksheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //worksheet.Row(row).Height = 32;

                        row++;
                        worksheet.Cells[$"A{row}:G{row}"].Merge = true;
                        worksheet.Cells[$"A{row}:G{row}"].Value = "Ngân hàng thương mại cổ phần Ngoại thương Việt Nam";
                        worksheet.Cells[$"A{row}:G{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}:G{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}:G{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Row(row).Style.Font.Bold = true;
                        worksheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        worksheet.Row(row).Style.WrapText = true;
                        row++;
                        var firstRequire = row;
                        if (report.Count>0)
                        {
                            var lstPlanRequire = _planRequireService.GetList(report.FirstOrDefault().Id).Where(x => x.Status == 1).ToList();

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

                                    worksheet.Cells[$"C{row}"].Value = itemstuck.Description;
                                    worksheet.Cells[$"C{row}"].Style.Border.Right.Style = worksheet.Cells[$"C{row}"].Style.Border.Top.Style = worksheet.Cells[$"C{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"C{row}"].Style.WrapText = true;

                                    worksheet.Cells[$"D{row}"].Value = itemstuck.Total;
                                    worksheet.Cells[$"D{row}"].Style.Border.Right.Style = worksheet.Cells[$"D{row}"].Style.Border.Top.Style = worksheet.Cells[$"D{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"D{row}"].Style.WrapText = true;

                                    worksheet.Cells[$"E{row}"].Value = itemstuck.Total1;
                                    worksheet.Cells[$"E{row}"].Style.Border.Right.Style = worksheet.Cells[$"E{row}"].Style.Border.Top.Style = worksheet.Cells[$"E{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"E{row}"].Style.WrapText = true;

                                    worksheet.Cells[$"F{row}"].Style.WrapText = true;
                                    worksheet.Cells[$"F{row}"].Value = itemstuck.Total2;
                                    worksheet.Cells[$"F{row}"].Style.Border.Right.Style = worksheet.Cells[$"F{row}"].Style.Border.Top.Style = worksheet.Cells[$"F{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"G{row}"].Style.WrapText = true;
                                    worksheet.Cells[$"G{row}"].Value = itemstuck.Result;
                                    worksheet.Cells[$"G{row}"].Style.Border.Right.Style = worksheet.Cells[$"G{row}"].Style.Border.Top.Style = worksheet.Cells[$"G{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    row++;
                                }


                            }
                        }
                        worksheet.Cells[$"A{row}:G{row}"].Merge = true;
                        worksheet.Cells[$"A{row}:G{row}"].Value = "Ngân hàng TMCP Đầu tư và Phát triển Việt Nam";
                        worksheet.Cells[$"A{row}:G{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}:G{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}:G{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Row(row).Style.Font.Bold = true;
                        worksheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        worksheet.Row(row).Style.WrapText = true;
                        row++;
                        if (reportBIDV.Count>0)
                        {
                            var lstPlanRequire = _planRequireService.GetList(reportBIDV.FirstOrDefault().Id).Where(x => x.Status == 1).ToList();

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

                                    worksheet.Cells[$"C{row}"].Value = itemstuck.Description;
                                    worksheet.Cells[$"C{row}"].Style.Border.Right.Style = worksheet.Cells[$"C{row}"].Style.Border.Top.Style = worksheet.Cells[$"C{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"C{row}"].Style.WrapText = true;

                                    worksheet.Cells[$"D{row}"].Value = itemstuck.Total;
                                    worksheet.Cells[$"D{row}"].Style.Border.Right.Style = worksheet.Cells[$"D{row}"].Style.Border.Top.Style = worksheet.Cells[$"D{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"D{row}"].Style.WrapText = true;

                                    worksheet.Cells[$"E{row}"].Value = itemstuck.Total1;
                                    worksheet.Cells[$"E{row}"].Style.Border.Right.Style = worksheet.Cells[$"E{row}"].Style.Border.Top.Style = worksheet.Cells[$"E{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                    worksheet.Cells[$"E{row}"].Style.WrapText = true;

                                    worksheet.Cells[$"F{row}"].Style.WrapText = true;
                                    worksheet.Cells[$"F{row}"].Value = itemstuck.Total2;
                                    worksheet.Cells[$"F{row}"].Style.Border.Right.Style = worksheet.Cells[$"F{row}"].Style.Border.Top.Style = worksheet.Cells[$"F{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    worksheet.Cells[$"G{row}"].Style.WrapText = true;
                                    worksheet.Cells[$"G{row}"].Value = itemstuck.Result;
                                    worksheet.Cells[$"G{row}"].Style.Border.Right.Style = worksheet.Cells[$"G{row}"].Style.Border.Top.Style = worksheet.Cells[$"G{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                    row++;
                                }


                            }
                        }
                       
                        worksheet.Cells[$"A{row}:C{row}"].Merge = true;
                        worksheet.Cells[$"A{row}:C{row}"].Style.Font.Bold = true;
                        worksheet.Cells[$"A{row}:C{row}"].Value = "Tổng";
                        worksheet.Cells[$"A{row}:C{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}:C{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}:C{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells[$"D{row}"].Formula = $"sum(D{firstRequire}:D{ row - 1})";
                        worksheet.Cells[$"D{row}"].Style.Border.Right.Style = worksheet.Cells[$"D{row}"].Style.Border.Top.Style = worksheet.Cells[$"D{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                        worksheet.Cells[$"E{row}"].Formula = $"sum(E{firstRequire}:E{ row - 1})";
                        worksheet.Cells[$"E{row}"].Style.Border.Right.Style = worksheet.Cells[$"E{row}"].Style.Border.Top.Style = worksheet.Cells[$"E{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells[$"F{row}"].Formula = $"sum(F{firstRequire}:E{ row - 1})";
                        worksheet.Cells[$"F{row}"].Style.Border.Right.Style = worksheet.Cells[$"F{row}"].Style.Border.Top.Style = worksheet.Cells[$"F{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                        worksheet.Cells[$"G{row}"].Style.Border.Right.Style = worksheet.Cells[$"G{row}"].Style.Border.Top.Style = worksheet.Cells[$"G{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        row++;
                        xlPackage.Save();

                    }
                    bytes = stream.ToArray();
                }
                var name = $"Báo cáo quý {type} năm {year}";
                if(type==5)
                    name = $"Báo cáo năm {year}";
                return File(bytes, "text/xls", StringUtils.ReplaceVietnameseChar(name) + ".xlsx");
            }
            catch (Exception ex)
            {
                NLogLogger.DebugMessage(ex);
                return RedirectToAction("Index");
            }
        }
    }
}