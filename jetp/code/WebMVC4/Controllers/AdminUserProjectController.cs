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
using System.Web.SessionState;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using WebMVC4.Helper;

namespace WebMVC4.Controllers
{

    [Authorize(Roles = "Administrator,Sale")]
    public class AdminUserProjectController : Controller
    {

        public ActionResult ManageProject()
        {
            ViewBag.Title = "Danh sách dự án đề xuất";

            return View();
        }

        public ActionResult ListProject(int? type, string username, string keyword, int? currentPage, int? pageSize)
        {

            var data = new List<UserProject>();

            //int TotalRecord = 0;
            int Type = type == null ? -1 : (int)type;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 100 : (int)pageSize;
            int TotalRecord = 0;
            data = UserProjectDAL.GetSearch(1, Type, username, keyword, CurrPage, RecordPerPage, ref TotalRecord);
            if (data != null)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;



            return PartialView(data);
        }
        [HttpGet]
        public ActionResult ExportExcel(int Id)
        {
            var obj = new UserProjectFull();

            var project = UserProjectDAL.GetDetail(Id);
            if (project == null)
            {
                return RedirectToAction("ManageProject");
            }

            UserProjectDAL.UpdateView(Id);
            obj.Id = project.Id;
            obj.Name = project.Name;
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
            try
            {
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        var worksheet = xlPackage.Workbook.Worksheets.Add("VIE");


                        //độ rộng cột
                        worksheet.Column(1).Width = 36;
                        worksheet.Column(2).Width = 88;
                      

                        var allCells = worksheet.Cells[1, 1, 50, 50];
                        var cellFont = allCells.Style.Font;
                        
                        cellFont.SetFromFont(new Font("Calibri", 12));
                        worksheet.Cells["A1"].Value = "Các trường thông tin";
                        worksheet.Cells["A1"].Style.Font.Size = 14;
                        worksheet.Cells["A1"].Style.Font.Bold = true;
                      
                        worksheet.Cells["B1"].Value = "Mô tả/ Yêu cầu về trường thông tin";
                        worksheet.Cells["B1"].Style.Font.Size = 14;
                        worksheet.Cells["B1"].Style.Font.Bold = true;
                        for(int i=1;i<=28;i++)
                        {
                            worksheet.Cells["A"+i].Style.Border.Right.Style = worksheet.Cells["A"+i].Style.Border.Top.Style = worksheet.Cells["A"+i].Style.Border.Left.Style = worksheet.Cells["A"+i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells["B"+i].Style.Border.Right.Style = worksheet.Cells["B"+i].Style.Border.Top.Style = worksheet.Cells["B"+i].Style.Border.Left.Style = worksheet.Cells["B"+i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            //worksheet.Row(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Row(i).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Row(i).Style.WrapText = true;
                           
                        }
                        worksheet.Cells["A2"].Value = "1. Tên dự án: ";
                        worksheet.Cells["A2"].Style.Font.Bold = true;
                        worksheet.Cells["B2"].Value = obj.Name ;

                        worksheet.Cells["A3"].Value = "2. Địa điểm dự án:";
                        worksheet.Cells["A3"].Style.Font.Bold = true;

                        var listLocation = new TestLocationBO().GetAllCache();
                        if(listLocation.Exists(x=>x.Id.ToString()==obj.Location))
                        {
                            obj.Location = listLocation.FirstOrDefault(x => x.Id.ToString() == obj.Location).Name;
                        }    
                        worksheet.Cells["B3"].Value = obj.Location;

                        worksheet.Cells["A4"].Value = "3. Hạng mục dự án: ";
                        worksheet.Cells["A4"].Style.Font.Bold = true;
                        worksheet.Cells["B4"].Value = HtmlHelpers.UserProjectSubType(obj.SubType, obj.Type);

                        worksheet.Cells["A5"].Value = "4. Đơn vị đăng ký đề xuất: ";
                        worksheet.Cells["A5"].Style.Font.Bold = true;
                        worksheet.Cells["B5"].Value = obj.Unit;

                        worksheet.Cells["A6"].Value = "5. Thông tin liên lạc của đại diện đơn vị ký đề xuất:  ";
                        worksheet.Cells["A6"].Style.Font.Bold = true;
                        worksheet.Cells["B6"].Value =  obj.ProjectConfig.Fullname + "\r\n" + obj.ProjectConfig.Role + "\r\n" + obj.ProjectConfig.Email + "\r\n" + obj.ProjectConfig.Mobile;

                        worksheet.Cells["A7"].Value = "6. Đơn vị phát triển dự án:";
                       // worksheet.Cells["A7"].Style.Font.Bold = true;
                        worksheet.Cells["B7"].Value = obj.ProjectConfig.UnitDev;

                        worksheet.Cells["A8"].Value = "7. Chủ dự án: ";
                        worksheet.Cells["A8"].Style.Font.Bold = true;
                        worksheet.Cells["B8"].Value = obj.Organ;

                        worksheet.Cells["A9"].Value = "8. Tổng chi phí dự án: ";
                        worksheet.Cells["A9"].Style.Font.Bold = true;
                        worksheet.Cells["B9"].Value = obj.Total;
                        //worksheet.Cells["B9"].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells["B9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        worksheet.Cells["A10"].Value = "9. Đơn vị tiền tệ đầu tư mong muốn: ";
                        worksheet.Cells["A10"].Style.Font.Bold = true;
                        worksheet.Cells["B10"].Value = obj.Currency;

                        worksheet.Cells["A11"].Value = "10. Chi tiết ngân sách:  ";
                        //worksheet.Cells["A10"].Style.Font.Bold = true;
                        worksheet.Cells["B11"].Value = obj.Detail;

                        worksheet.Cells["A12"].Value = "11. Nguồn tài chính:  ";
                        //worksheet.Cells["A10"].Style.Font.Bold = true;
                        worksheet.Cells["B12"].Value = obj.Source;


                        worksheet.Cells["A13"].Value = "12. Loại hình tài chính hiện có và mong muốn:";
                        //worksheet.Cells["A10"].Style.Font.Bold = true;
                        worksheet.Cells["B13"].Value = obj.ProjectConfig.Finance;

                        worksheet.Cells["A14"].Value = "13. Dòng doanh thu dự kiến: ";
                        //worksheet.Cells["A10"].Style.Font.Bold = true;
                        worksheet.Cells["B14"].Value = obj.ProjectConfig.Revenue;

                        worksheet.Cells["A15"].Value = "14. Tiến độ dự án: ";
                        worksheet.Cells["A15"].Style.Font.Bold = true;
                        worksheet.Cells["B15"].Value = HtmlHelpers.UserProjectProgress(obj.Progress);

                        worksheet.Cells["A16"].Value = "15. Ngày dự kiến khởi công: ";
                        worksheet.Cells["A16"].Style.Font.Bold = true;
                        worksheet.Cells["B16"].Value = obj.ProjectConfig.Time;

                        worksheet.Cells["A17"].Value = "16. Ngày dự kiến hoàn thành (CoD): ";
                        worksheet.Cells["A17"].Style.Font.Bold = true;
                        worksheet.Cells["B17"].Value = obj.ProjectConfig.Finish;

                        worksheet.Cells["A18"].Value = "17. Hiện trạng pháp lý của dự án: ";
                        worksheet.Cells["A18"].Style.Font.Bold = true;
                        worksheet.Cells["B18"].Value = obj.LegalStatus;

                        worksheet.Cells["A19"].Value = "18. Mô tả ngắn gọn dự án và mục tiêu dự án:";
                        worksheet.Cells["A19"].Style.Font.Bold = true;
                        worksheet.Cells["B19"].Value = obj.Description;


                        worksheet.Cells["A20"].Value = "19. Tác động dự kiến dự án tạo ra:";
                        //worksheet.Cells["A20"].Style.Font.Bold = true;
                        worksheet.Cells["B20"].Value = obj.Impact;

                        worksheet.Cells["A21"].Value = "20. Cần gói Hỗ trợ Kỹ thuật (TA) để hỗ trợ dự án?\r\n";
                        worksheet.Cells["A21"].Style.Font.Bold = true;
                        worksheet.Cells["B21"].Value = obj.ProjectConfig.TADetail;

                        worksheet.Cells["A22"].Value = "21. Đáp ứng 4 nguyên tắc chung của JETP: \r\n";
                        worksheet.Cells["A22"].Style.Font.Bold = true;

                        worksheet.Cells["A23"].Value = "21.1 Nguyên tắc chung 1: \r\n";
                        worksheet.Cells["A23"].Style.Font.Bold = true;
                        worksheet.Cells["B23"].Value = obj.Rule1;

                        worksheet.Cells["A24"].Value = "21.2 Nguyên tắc chung 2: \r\n";
                        worksheet.Cells["A24"].Style.Font.Bold = true;
                        worksheet.Cells["B24"].Value = obj.Rule2;


                        worksheet.Cells["A25"].Value = "21.3 Nguyên tắc chung 3: \r\n";
                        worksheet.Cells["A25"].Style.Font.Bold = true;
                        worksheet.Cells["B25"].Value = obj.Rule3;


                        worksheet.Cells["A26"].Value = "21.4 Nguyên tắc chung 4: \r\n";
                        worksheet.Cells["A26"].Style.Font.Bold = true;
                        worksheet.Cells["B26"].Value = obj.Rule4;

                        worksheet.Cells["A27"].Value = "22. Các đánh giá tiền khả thi:\r\n";
                        //worksheet.Cells["A20"].Style.Font.Bold = true;
                        worksheet.Cells["B27"].Value = obj.ProjectConfig.Rate;

                        worksheet.Cells["A28"].Value = "23. Tài liệu dự án: \r\n";
                        //worksheet.Cells["A20"].Style.Font.Bold = true;
                        worksheet.Cells["B28"].Value = obj.Document;

                        xlPackage.Save();
                    }
                   
                    bytes = stream.ToArray();
                }
                return File(bytes, "text/xls", UTILS.Utils.ReplaceVietnameseChar(obj.Name) +"_"+Id.ToString("D5") + ".xlsx");
            }
            catch (Exception ex)
            {
                NLogLogger.DebugMessage(ex);
                return RedirectToAction("ManageProject");
            }
        }
        [HttpGet]
        public ActionResult ExportExcel2(int Id)
        {
            var obj = new UserProjectFull();

            var project = UserProjectDAL.GetDetail(Id);
            if (project == null)
            {
                return RedirectToAction("ManageProject");
            }
            UserProjectDAL.UpdateView(Id);

            obj.Id = project.Id;
            obj.Name = project.Name;
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
            try
            {
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    using (var xlPackage = new ExcelPackage(stream))
                    {
                        var worksheet = xlPackage.Workbook.Worksheets.Add("VIE");


                        //độ rộng cột
                        worksheet.Column(1).Width = 36;
                        worksheet.Column(2).Width = 88;


                        var allCells = worksheet.Cells[1, 1, 50, 50];
                        var cellFont = allCells.Style.Font;

                        cellFont.SetFromFont(new Font("Calibri", 12));
                        worksheet.Cells["A1"].Value = "Các trường thông tin";
                        worksheet.Cells["A1"].Style.Font.Size = 14;
                        worksheet.Cells["A1"].Style.Font.Bold = true;

                        worksheet.Cells["B1"].Value = "Mô tả/ Yêu cầu về trường thông tin";
                        worksheet.Cells["B1"].Style.Font.Size = 14;
                        worksheet.Cells["B1"].Style.Font.Bold = true;
                        for (int i = 1; i <= 23; i++)
                        {
                            worksheet.Cells["A" + i].Style.Border.Right.Style = worksheet.Cells["A" + i].Style.Border.Top.Style = worksheet.Cells["A" + i].Style.Border.Left.Style = worksheet.Cells["A" + i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells["B" + i].Style.Border.Right.Style = worksheet.Cells["B" + i].Style.Border.Top.Style = worksheet.Cells["B" + i].Style.Border.Left.Style = worksheet.Cells["B" + i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            //worksheet.Row(i).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Row(i).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            worksheet.Row(i).Style.WrapText = true;

                        }
                        worksheet.Cells["A2"].Value = "1. Tên dự án: ";
                        worksheet.Cells["A2"].Style.Font.Bold = true;
                        worksheet.Cells["B2"].Value = obj.Name;

                        worksheet.Cells["A3"].Value = "2. Địa điểm dự án:";
                        worksheet.Cells["A3"].Style.Font.Bold = true;
                       
                       var listLocation = new TestLocationBO().GetAllCache();
                        if (listLocation.Exists(x => x.Id.ToString() == obj.Location))
                        {
                            obj.Location = listLocation.FirstOrDefault(x => x.Id.ToString() == obj.Location).Name;
                        }
                        else
                        {
                            obj.Location = "Toàn quốc";
                        }
                        worksheet.Cells["B3"].Value = obj.Location;

                        worksheet.Cells["A4"].Value = "3. Hạng mục dự án: ";
                        worksheet.Cells["A4"].Style.Font.Bold = true;
                        worksheet.Cells["B4"].Value = HtmlHelpers.UserProjectSubType(obj.SubType, obj.Type);

                        worksheet.Cells["A5"].Value = "4. Hỗ trợ Dự án đầu tư có cấu phần xây dựng nào:\r\n ";
                        //worksheet.Cells["A5"].Style.Font.Bold = true;
                        worksheet.Cells["B5"].Value = obj.ProjectConfig.Support;


                        worksheet.Cells["A6"].Value = "5. Đơn vị đăng ký đề xuất: ";
                        worksheet.Cells["A6"].Style.Font.Bold = true;
                        worksheet.Cells["B6"].Value = obj.Unit;

                        worksheet.Cells["A7"].Value = "6. Thông tin liên lạc của đại diện đơn vị ký đề xuất:  ";
                        worksheet.Cells["A7"].Style.Font.Bold = true;

                        worksheet.Cells["B7"].Value = obj.ProjectConfig.Fullname + "\r\n" + obj.ProjectConfig.Role + "\r\n" + obj.ProjectConfig.Email + "\r\n" + obj.ProjectConfig.Mobile;


                        worksheet.Cells["A8"].Value = "7. Chủ dự án: ";
                        worksheet.Cells["A8"].Style.Font.Bold = true;
                        worksheet.Cells["B8"].Value = obj.Organ;


                        worksheet.Cells["A9"].Value = "8. Cơ quan chủ quản: ";
                        worksheet.Cells["A9"].Style.Font.Bold = true;
                        worksheet.Cells["B9"].Value = obj.ProjectConfig.UnitDev;

                        worksheet.Cells["A10"].Value = "9. Tổng vốn dự án: \r\n\r\n";
                        worksheet.Cells["A10"].Style.Font.Bold = true;
                        worksheet.Cells["B10"].Value = obj.Total +" " + obj.Currency; ;
                       // worksheet.Cells["B9"].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells["B10"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                        //worksheet.Cells["A10"].Value = "9. Đơn vị tiền tệ đầu tư mong muốn: ";
                        //worksheet.Cells["A10"].Style.Font.Bold = true;
                        //worksheet.Cells["B10"].Value = obj.Currency;

                        worksheet.Cells["A11"].Value = "10. Chi tiết ngân sách:  ";
                        //worksheet.Cells["A10"].Style.Font.Bold = true;
                        worksheet.Cells["B11"].Value = obj.Detail;

                        worksheet.Cells["A12"].Value = "11. Nguồn tài chính:  ";
                        //worksheet.Cells["A10"].Style.Font.Bold = true;
                        worksheet.Cells["B12"].Value = obj.Source;


                       
                        worksheet.Cells["A13"].Value = "12. Tiến độ dự án: ";
                        worksheet.Cells["A13"].Style.Font.Bold = true;
                        worksheet.Cells["B13"].Value = HtmlHelpers.UserProjectProgress(obj.Progress);

                        worksheet.Cells["A14"].Value = "13. Thời gian thực hiện: ";
                        worksheet.Cells["A14"].Style.Font.Bold = true;
                        worksheet.Cells["B14"].Value = obj.ProjectConfig.Time;

                        worksheet.Cells["A15"].Value = "14. Hiện trạng pháp lý của dự án: ";
                        worksheet.Cells["A15"].Style.Font.Bold = true;
                        worksheet.Cells["B15"].Value = obj.LegalStatus;

                        worksheet.Cells["A16"].Value = "15. Mô tả ngắn gọn dự án và mục tiêu dự án:";
                        worksheet.Cells["A16"].Style.Font.Bold = true;
                        worksheet.Cells["B16"].Value = obj.Description;


                        worksheet.Cells["A17"].Value = "16. Tác động dự kiến dự án tạo ra:";
                        //worksheet.Cells["A20"].Style.Font.Bold = true;
                        worksheet.Cells["B17"].Value = obj.Impact;

                      
                        worksheet.Cells["A18"].Value = "17. Đáp ứng 4 nguyên tắc chung của JETP: \r\n";
                        worksheet.Cells["A18"].Style.Font.Bold = true;

                        worksheet.Cells["A19"].Value = "17.1 Nguyên tắc chung 1: \r\n";
                        worksheet.Cells["A19"].Style.Font.Bold = true;
                        worksheet.Cells["B19"].Value = obj.Rule1;

                        worksheet.Cells["A20"].Value = "17.2 Nguyên tắc chung 2: \r\n";
                        worksheet.Cells["A20"].Style.Font.Bold = true;
                        worksheet.Cells["B20"].Value = obj.Rule2;


                        worksheet.Cells["A21"].Value = "17.3 Nguyên tắc chung 3: \r\n";
                        worksheet.Cells["A21"].Style.Font.Bold = true;
                        worksheet.Cells["B21"].Value = obj.Rule3;


                        worksheet.Cells["A22"].Value = "17.4 Nguyên tắc chung 4: \r\n";
                        worksheet.Cells["A22"].Style.Font.Bold = true;
                        worksheet.Cells["B22"].Value = obj.Rule4;

                      

                        worksheet.Cells["A23"].Value = "18. Tài liệu dự án: \r\n";
                        //worksheet.Cells["A20"].Style.Font.Bold = true;
                        worksheet.Cells["B23"].Value = obj.Document;

                        xlPackage.Save();
                    }

                    bytes = stream.ToArray();
                }
                return File(bytes, "text/xls", UTILS.Utils.ReplaceVietnameseChar(obj.Name) + "_" + Id.ToString("D5") + ".xlsx");
            }
            catch (Exception ex)
            {
                NLogLogger.DebugMessage(ex);
                return RedirectToAction("ManageProject");
            }
        }

        public ActionResult Detail(int Id)
        {

            var obj = new UserProjectFull();

            var project = UserProjectDAL.GetDetail(Id);
            if (project == null)
            {
                return RedirectToAction("Project");
            }

            UserProjectDAL.UpdateView(Id);
            obj.Id = project.Id;
            obj.Name = project.Name;
            obj.Location = project.Location;
            var listLocation = new TestLocationBO().GetAllCache();
            if (listLocation.Exists(x => x.Id.ToString() == obj.Location))
            {
                obj.Location = listLocation.FirstOrDefault(x => x.Id.ToString() == obj.Location).Name;
            }
            else
            {
                obj.Location = "Toàn quốc";
            }
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
            if (obj.Type == 2)
            {
                return PartialView("Detail2", obj);
            }
            return PartialView(obj);
        }
    }
}
