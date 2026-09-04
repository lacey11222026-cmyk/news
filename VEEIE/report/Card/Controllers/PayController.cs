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
    public class PayController : Controller
    {
        private readonly IUsersService _userservice;
        private readonly IUsersLogService _userlogservice;
        private readonly IFucntionsService _functionservice;
        private readonly IUserRoleService _userroleservice;
      
        private readonly IPaysService _payService;
       
        private UserSession CurrentUser { get { return ((UserSession)Session[SessionsManager.SESSION_USER]); } }
        private Users CurrentFullUser { get { return ((Users)Session[SessionsManager.SESSION_USER_FULL]); } }
        public PayController(IPaysService payService, IUsersService userservice, IUsersLogService userlogservice, IFucntionsService functionservice, IUserRoleService userroleservice)
        {
            _userservice = userservice;
            _userlogservice = userlogservice;
            _userroleservice = userroleservice;
            _functionservice = functionservice;

            _payService = payService;
          
        }
        [PermissionFilter(FunctionCode = FunctionCode.Pay)]
        public ActionResult Index()
        {
            ViewBag.Title = "Kế hoạch giải ngân";
            ViewBag.Type = CurrentFullUser.Type;
            return View();
        }
        [PermissionFilter(FunctionCode = FunctionCode.Pay)]
        public ActionResult ListPay()
        {
           
            var data = _payService.GetList(CurrentUser.Username,  -1);
            ViewBag.Type = CurrentFullUser.Type;
            return PartialView(data);
        }
        [PermissionFilter(FunctionCode = FunctionCode.Pay)]
        public ActionResult Info(int? Id)
        {
            int PageID = Id == null ? 0 : (int)Id;
            var model = new Pay
            {
                Status = 1,
                Total = 0,
                Total1 = 0,
                Total2 = 0,
                Total3 = 0,
                Total4 = 0,
            };

            if (PageID > 0)
            {
                model = _payService.GetPay(PageID);
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
        [PermissionFilter(FunctionCode = FunctionCode.Pay)]
        public JsonResult SaveData(Pay Pay)
        {
            var ReturnData = new ReturnData();

            try
            {

                Pay.Order = Convert.ToInt32(Pay.Order);
                Pay.UserName = CurrentUser.Username;
                var result = _payService.CreateUpdatePay(Pay);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (Pay.Id > 0)
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
        [PermissionFilter(FunctionCode = FunctionCode.Pay)]
        public JsonResult UpdateSortOrder(int Id, bool SortOrder)
        {
            try
            {
                var updateResult = _payService.UpdateOrder(Id, SortOrder, CurrentUser.Username);
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
        [PermissionFilter(FunctionCode = FunctionCode.Pay)]
        public JsonResult UpdateStatus(int id)
        {
            var ReturnData = new ReturnData();
            try
            {

                if (id >= 0)
                {
                    var result = _payService.UpdateStatus(id);
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
        public ActionResult ExportExcel()
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
                        worksheet.Column(1).Width = 8;
                        worksheet.Column(2).Width = 30;
                        worksheet.Column(3).Width = 20;
                        worksheet.Column(4).Width = 20;
                        worksheet.Column(5).Width = 20;
                        worksheet.Column(6).Width = 20;
                        worksheet.Column(7).Width = 14;
                        worksheet.Column(8).Width = 14;
                        worksheet.Column(9).Width = 14;
                        


                        var allCells = worksheet.Cells[1, 1, 50, 50];
                        var cellFont = allCells.Style.Font;
                        cellFont.SetFromFont(new Font("Times New Roman", 12));


                        worksheet.Cells["A1:C1"].Merge = true;
                        worksheet.Cells["A1:C1"].Value = HtmlHelpers.GetBankName(CurrentFullUser.Username);
                        worksheet.Cells["A1:C1"].Style.WrapText = true;

                        worksheet.Cells["D1:F1"].Merge = true;
                        worksheet.Cells["D1:F1"].Value = "Cộng hòa xã hội chủ nghĩa Việt Nam";
                        worksheet.Cells["D1:F1"].Style.WrapText = true;

                        worksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;



                        worksheet.Cells["A2:C2"].Merge = true;
                        worksheet.Cells["A2:C2"].Value = "";
                        worksheet.Cells["A2:C2"].Style.WrapText = true;

                        worksheet.Cells["D2:F2"].Merge = true;
                        worksheet.Cells["D2:F2"].Value = "Độc lập - Tự do - Hạnh phúc";
                        worksheet.Cells["D2:F2"].Style.WrapText = true;

                        worksheet.Row(2).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


                        worksheet.Cells["B3:D3"].Merge = true;
                        worksheet.Cells["B3:D3"].Value = "KẾ HOẠCH GIẢI NGÂN";
                        worksheet.Cells["B3:D3"].Style.WrapText = true;

                        worksheet.Cells["B4:D4"].Merge = true;
                        worksheet.Cells["B4:D4"].Value = "DỰ ÁN TIẾT KIỆM NĂNG LƯỢNG CHO NGÀNH CÔNG NGHIỆP \n VIỆT NAM";
                        worksheet.Cells["B4:D4"].Style.WrapText = true;


                        worksheet.Row(3).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Row(3).Height = 24;
                        worksheet.Row(3).Style.Font.Bold = true;
                        worksheet.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Row(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Row(4).Height = 45;
                        worksheet.Row(4).Style.Font.Bold = true;


                      
                        worksheet.Cells["A6:A7"].Merge = true;
                        worksheet.Cells["A6:A7"].Value = "Năm";
                        worksheet.Cells["A6:A7"].Style.Border.Right.Style = worksheet.Cells["A6:A7"].Style.Border.Top.Style = worksheet.Cells["A6:A7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        worksheet.Cells["B6:B7"].Merge = true;
                        worksheet.Cells["B6:B7"].Value = "Tổng kế hoạch rút vốn (triệu VND)";
                        worksheet.Cells["B6:B7"].Style.Border.Right.Style = worksheet.Cells["B6:B7"].Style.Border.Top.Style = worksheet.Cells["B6:B7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                        worksheet.Cells["C6:F6"].Merge = true;
                        worksheet.Cells["C6:F6"].Value = "Kế hoạch giải ngân";
                        worksheet.Cells["C6:F6"].Style.Border.Right.Style = worksheet.Cells["C6:F6"].Style.Border.Top.Style = worksheet.Cells["C6:F6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                      
                        worksheet.Cells["C7"].Value = "Quý 1 (triệu VND)";
                        worksheet.Cells["D7"].Value = "Quý 2 (triệu VND)";
                        worksheet.Cells["E7"].Value = "Quý 3 (triệu VND)";
                        worksheet.Cells["F7"].Value = "Quý 4 (triệu VND)";

                        worksheet.Cells["C7"].Style.Border.Right.Style = worksheet.Cells["C7"].Style.Border.Top.Style = worksheet.Cells["C7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells["D7"].Style.Border.Right.Style = worksheet.Cells["D7"].Style.Border.Top.Style = worksheet.Cells["D7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
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

                        var lstPlanItem = _payService.GetList(CurrentFullUser.Username, 1);
                        if (lstPlanItem.Count > 0)
                        {
                            int row = 8;
                            
                            for (int i = 0; i < lstPlanItem.Count; i++)
                            {
                               
                                var item = lstPlanItem[i];
                                worksheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                             
                              
                                worksheet.Cells[$"A{row}"].Value = item.Year;
                                worksheet.Cells[$"A{row}"].Style.Border.Right.Style = worksheet.Cells[$"A{row}"].Style.Border.Top.Style = worksheet.Cells[$"A{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                worksheet.Cells[$"A{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                worksheet.Cells[$"A{row}"].Style.WrapText = true;

                              
                                worksheet.Cells[$"B{row}"].Value = item.Total;
                                worksheet.Cells[$"B{row}"].Style.Border.Right.Style = worksheet.Cells[$"B{row}"].Style.Border.Top.Style = worksheet.Cells[$"B{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                               
                                worksheet.Cells[$"C{row}"].Value = item.Total1;
                                worksheet.Cells[$"C{row}"].Style.Border.Right.Style = worksheet.Cells[$"C{row}"].Style.Border.Top.Style = worksheet.Cells[$"C{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                               
                                worksheet.Cells[$"D{row}"].Value = item.Total2;
                                worksheet.Cells[$"D{row}"].Style.Border.Right.Style = worksheet.Cells[$"D{row}"].Style.Border.Top.Style = worksheet.Cells[$"D{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                
                                worksheet.Cells[$"E{row}"].Value = item.Total3;
                                worksheet.Cells[$"E{row}"].Style.Border.Right.Style = worksheet.Cells[$"E{row}"].Style.Border.Top.Style = worksheet.Cells[$"E{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                
                                worksheet.Cells[$"F{row}"].Value = item.Total4;
                                worksheet.Cells[$"F{row}"].Style.Border.Right.Style = worksheet.Cells[$"F{row}"].Style.Border.Top.Style = worksheet.Cells[$"F{row}"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                                row++;
                            }
                            //tổng
                            row += 1;
                           
                           

                            worksheet.Cells[$"E{row}:F{row + 4}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Cells[$"E{row}:F{row + 4}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Row(row + 4).Height = 40;
                            //worksheet.Cells[$"E{row}:F{row + 4}"].Value = "………, Ngày …… tháng ……. năm 20….\n<b>Giám đốc</b>\n(ký tên, đóng dấu)";
                            worksheet.Cells[$"E{row}:F{row + 4}"].Merge = true;
                            worksheet.Cells[$"E{row}:F{row + 4}"].Style.WrapText = true;
                            var cell = worksheet.Cells[$"E{row}:F{row + 4}"];
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
                return File(bytes, "text/xls", "KeHoachGiaiNgan.xlsx");
            }
            catch (Exception ex)
            {
                NLogLogger.DebugMessage(ex);
                return RedirectToAction("Index");
            }
        }
    }
}