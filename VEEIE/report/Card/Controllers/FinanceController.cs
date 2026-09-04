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
    public class FinanceController : Controller
    {
        private readonly IUsersService _userservice;
        private readonly IUsersLogService _userlogservice;
        private readonly IFucntionsService _functionservice;
        private readonly IUserRoleService _userroleservice;

        private readonly IFinancesService _FinanceService;

        private UserSession CurrentUser { get { return ((UserSession)Session[SessionsManager.SESSION_USER]); } }
        private Users CurrentFullUser { get { return ((Users)Session[SessionsManager.SESSION_USER_FULL]); } }
        public FinanceController(IFinancesService FinanceService, IUsersService userservice, IUsersLogService userlogservice, IFucntionsService functionservice, IUserRoleService userroleservice)
        {
            _userservice = userservice;
            _userlogservice = userlogservice;
            _userroleservice = userroleservice;
            _functionservice = functionservice;

            _FinanceService = FinanceService;

        }
        [PermissionFilter(FunctionCode = FunctionCode.Finance)]
        public ActionResult Index()
        {
            ViewBag.Title = "Kế hoạch tài chính";
            ViewBag.Type = CurrentFullUser.Type;
            return View();
        }
        [PermissionFilter(FunctionCode = FunctionCode.Finance)]
        public ActionResult ListFinance(int year)
        {

            var data = _FinanceService.GetList(CurrentUser.Username, -1, year);
            ViewBag.Type = CurrentFullUser.Type;
            return PartialView(data);
        }
        [PermissionFilter(FunctionCode = FunctionCode.Finance)]
        public ActionResult Info(int? Id)
        {
            int PageID = Id == null ? 0 : (int)Id;
            var model = new Finance
            {
                Status = 1,
                Year= DateTime.Now.Year,
                Total = 0,
                Total1 = 0,
                Total2 = 0,
                Total3 = 0,
                Total4 = 0,
            };

            if (PageID > 0)
            {
                model = _FinanceService.GetFinance(PageID);
            }

            ViewBag.id = Id;

            if (Id > 0)
            {
                ViewBag.Title = "Cập nhật kế hoạch";
            }
            else
            {
                ViewBag.Title = "Thêm mới kế hoạch";
            }
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        [PermissionFilter(FunctionCode = FunctionCode.Finance)]
        public JsonResult SaveData(Finance Finance)
        {
            var ReturnData = new ReturnData();

            try
            {

                Finance.Order = Convert.ToInt32(Finance.Order);
                Finance.UserName = CurrentUser.Username;
                var result = _FinanceService.CreateUpdateFinance(Finance);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (Finance.Id > 0)
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
        [PermissionFilter(FunctionCode = FunctionCode.Finance)]
        public JsonResult UpdateSortOrder(int Id, bool SortOrder,int year)
        {
            try
            {
                var updateResult = _FinanceService.UpdateOrder(Id, SortOrder, CurrentUser.Username,year);
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
        [PermissionFilter(FunctionCode = FunctionCode.Finance)]
        public JsonResult UpdateStatus(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = _FinanceService.UpdateStatus(id);
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
        public ActionResult ExportExcel(int year)
        {



            try
            {
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        var worksheet = xlPackage.Workbook.Worksheets.Add("Report");


                        //độ rộng cột
                        worksheet.Column(1).Width = 9;
                        worksheet.Column(2).Width = 20;
                        worksheet.Column(3).Width = 20;
                        worksheet.Column(4).Width = 30;
                        worksheet.Column(5).Width = 20;
                        worksheet.Column(6).Width = 20;
                        worksheet.Column(7).Width = 60;
                        worksheet.Column(8).Width = 14;
                        worksheet.Column(9).Width = 14;



                        var allCells = worksheet.Cells[1, 1, 50, 50];
                        var cellFont = allCells.Style.Font;
                        cellFont.SetFromFont(new Font("Times New Roman", 12));


                        worksheet.Cells["A1:C1"].Merge = true;
                        worksheet.Cells["A1:C1"].Value = HtmlHelpers.GetBankName(CurrentFullUser.Username);
                        worksheet.Cells["A1:C1"].Style.WrapText = false;

                        worksheet.Cells["F1:G1"].Merge = true;
                        worksheet.Cells["F1:G1"].Value = "Cộng hòa xã hội chủ nghĩa Việt Nam";
                        worksheet.Cells["F1:G1"].Style.WrapText = true;

                        worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;



                        worksheet.Cells["A2:C2"].Merge = true;
                        worksheet.Cells["A2:C2"].Value = $"Năm {year}";
                        worksheet.Cells["A2:C2"].Style.WrapText = true;

                        worksheet.Cells["F2:G2"].Merge = true;
                        worksheet.Cells["F2:G2"].Value = "Độc lập - Tự do - Hạnh phúc";
                        worksheet.Cells["F2:G2"].Style.WrapText = true;

                        worksheet.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


                        worksheet.Cells["D3:F3"].Merge = true;
                        worksheet.Cells["D3:F3"].Value = "KẾ HOẠCH TÀI CHÍNH";
                        worksheet.Cells["D3:F3"].Style.WrapText = true;

                        worksheet.Cells["D4:F4"].Merge = true;
                        worksheet.Cells["D4:F4"].Value = "DỰ ÁN TIẾT KIỆM NĂNG LƯỢNG CHO NGÀNH CÔNG NGHIỆP \n VIỆT NAM";
                        worksheet.Cells["D4:F4"].Style.WrapText = true;


                        worksheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Row(3).Height = 24;
                        worksheet.Row(3).Style.Font.Bold = true;
                        worksheet.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Row(4).Height = 45;
                        worksheet.Row(4).Style.Font.Bold = true;



                        worksheet.Cells["A6:A7"].Merge = true;
                        worksheet.Cells["A6:A7"].Value = "Thứ tự";
                        worksheet.Cells["A6:A7"].Style.Border.Right.Style = worksheet.Cells["A6:A7"].Style.Border.Top.Style = worksheet.Cells["A6:A7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["B6:B7"].Merge = true;
                        worksheet.Cells["B6:B7"].Value = "Tên dự án";
                        worksheet.Cells["B6:B7"].Style.Border.Right.Style = worksheet.Cells["B6:B7"].Style.Border.Top.Style = worksheet.Cells["B6:B7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                        worksheet.Cells["C6:C7"].Merge = true;
                        worksheet.Cells["C6:C7"].Value = "Chủ đầu tư";
                        worksheet.Cells["C6:C7"].Style.Border.Right.Style = worksheet.Cells["C6:C7"].Style.Border.Top.Style = worksheet.Cells["C6:C7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["D6:D7"].Merge = true;
                        worksheet.Cells["D6:D7"].Value = "Tổng mức đầu tư (triệu VND)";
                        worksheet.Cells["D6:D7"].Style.Border.Right.Style = worksheet.Cells["D6:D7"].Style.Border.Top.Style = worksheet.Cells["D6:D7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["E6:F6"].Merge = true;
                        worksheet.Cells["E6:F6"].Value = "Tổng vốn vay (triệu VND)";
                        worksheet.Cells["E6:F6"].Style.Border.Right.Style = worksheet.Cells["E6:F6"].Style.Border.Top.Style = worksheet.Cells["E6:F6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["G6:G7"].Merge = true;
                        worksheet.Cells["G6:G7"].Value = "Năng lượng tiết kiệm dự kiến MWh";
                        worksheet.Cells["G6:G7"].Style.Border.Right.Style = worksheet.Cells["G6:G7"].Style.Border.Top.Style = worksheet.Cells["G6:G7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                       
                        worksheet.Cells["E7"].Value = "Nguồn WB";
                        worksheet.Cells["F7"].Value = "Nguồn PFI";

                        worksheet.Cells["E7"].Style.Border.Right.Style = worksheet.Cells["E7"].Style.Border.Top.Style = worksheet.Cells["E7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["F7"].Style.Border.Right.Style = worksheet.Cells["F7"].Style.Border.Top.Style = worksheet.Cells["F7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Row(6).Style.Font.Bold = true;
                        worksheet.Row(6).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(6).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Row(6).Style.WrapText = true;
                        worksheet.Row(7).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(7).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Row(7).Style.Font.Bold = true;
                        worksheet.Row(7).Style.WrapText = true;




                        //Danh sách dự án

                        var lstPlanItem = _FinanceService.GetList(CurrentFullUser.Username, 1,year);
                        if (lstPlanItem.Count > 0)
                        {
                            int row = 8;

                            for (int i = 0; i < lstPlanItem.Count; i++)
                            {

                                var item = lstPlanItem[i];
                                worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;


                                worksheet.Cells[$"A{row}"].Value = i+1;
                                worksheet.Cells[$"A{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[$"A{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[$"A{row}"].Style.WrapText = true;


                                worksheet.Cells[$"B{row}"].Value = item.Name;
                                worksheet.Cells[$"B{row}"].Style.Border.Right.Style = worksheet.Cells[$"B{row}"].Style.Border.Top.Style = worksheet.Cells[$"B{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                                worksheet.Cells[$"C{row}"].Value = item.Investor;
                                worksheet.Cells[$"C{row}"].Style.Border.Right.Style = worksheet.Cells[$"C{row}"].Style.Border.Top.Style = worksheet.Cells[$"C{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                                worksheet.Cells[$"D{row}"].Value = item.Total;
                                worksheet.Cells[$"D{row}"].Style.Border.Right.Style = worksheet.Cells[$"D{row}"].Style.Border.Top.Style = worksheet.Cells[$"D{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                                worksheet.Cells[$"E{row}"].Value = item.Total1;
                                worksheet.Cells[$"E{row}"].Style.Border.Right.Style = worksheet.Cells[$"E{row}"].Style.Border.Top.Style = worksheet.Cells[$"E{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                                worksheet.Cells[$"F{row}"].Value = item.Total2;
                                worksheet.Cells[$"F{row}"].Style.Border.Right.Style = worksheet.Cells[$"F{row}"].Style.Border.Top.Style = worksheet.Cells[$"F{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                worksheet.Cells[$"G{row}"].Value = item.Total3;
                                worksheet.Cells[$"G{row}"].Style.Border.Right.Style = worksheet.Cells[$"G{row}"].Style.Border.Top.Style = worksheet.Cells[$"G{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                row++;
                            }
                            //tổng
                            //row += 1;
                            worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;

                            worksheet.Cells[$"A{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"A{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[$"A{row}"].Style.WrapText = true;

                            worksheet.Cells[$"B{row}:C{row}"].Merge = true;
                            worksheet.Cells[$"B{row}:C{row}"].Style.Border.Right.Style = worksheet.Cells[$"B{row}:C{row}"].Style.Border.Top.Style = worksheet.Cells[$"B{row}:C{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"B{row}:C{row}"].Style.Font.Bold = true;
                            worksheet.Cells[$"B{row}:C{row}"].Style.WrapText = true;
                            worksheet.Cells[$"B{row}:C{row}"].Value = "Tổng cộng";

                            worksheet.Cells[$"D{row}"].Style.Border.Right.Style = worksheet.Cells[$"D{row}"].Style.Border.Top.Style = worksheet.Cells[$"D{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"D{row}"].Formula = $"Sum(D8:D{row - 1})";
                            worksheet.Cells[$"D{row}"].Style.WrapText = true;

                            worksheet.Cells[$"E{row}"].Style.Border.Right.Style = worksheet.Cells[$"E{row}"].Style.Border.Top.Style = worksheet.Cells[$"E{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"E{row}"].Formula = $"Sum(E8:E{row - 1})";
                            worksheet.Cells[$"E{row}"].Style.WrapText = true;

                            worksheet.Cells[$"F{row}"].Style.Border.Right.Style = worksheet.Cells[$"F{row}"].Style.Border.Top.Style = worksheet.Cells[$"F{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"F{row}"].Formula = $"Sum(F8:F{row - 1})";
                            worksheet.Cells[$"F{row}"].Style.WrapText = true;

                            worksheet.Cells[$"G{row}"].Style.Border.Right.Style = worksheet.Cells[$"G{row}"].Style.Border.Top.Style = worksheet.Cells[$"G{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[$"G{row}"].Formula = $"Sum(G8:G{row - 1})";
                            worksheet.Cells[$"G{row}"].Style.WrapText = true;
                            //chữ ký
                            row += 1;
                            worksheet.Cells[$"G{row}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells[$"G{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Row(row).Height = 60;
                           
                            worksheet.Cells[$"G{row}"].Style.WrapText = true;
                            var cell = worksheet.Cells[$"G{row}"];
                            var r1 = cell.RichText.Add("………, Ngày …… tháng ……. năm 20…" + "\r\n");
                            r1.Bold = false;
                            var r2 = cell.RichText.Add("Giám đốc" + "\r\n");
                            r2.Bold = true;
                            var r3 = cell.RichText.Add("(ký tên, đóng dấu)" + "\r\n");
                            r3.Bold = false;
                        }



                        xlPackage.Save();

                    }
                    bytes = stream.ToArray();
                }
                return File(bytes, "text/xls", "KeHoachTaiChinh.xlsx");
            }
            catch (Exception ex)
            {
                NLogLogger.DebugMessage(ex);
                return RedirectToAction("Index");
            }
        }
    }
}