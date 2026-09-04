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

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Category")]
    public class AdminNhaMayController : Controller
    {



        public ActionResult Index()
        {



            return View();
        }
        public ActionResult ListNhaMay(int?hinhthuc, int? loai, string title, int? currentPage, int? pageSize)
        {
            int Hinhthuc = hinhthuc == null ? -1 : (int)hinhthuc;
            int Loai = loai == null ? -1 : (int)loai;
            string Title = string.IsNullOrEmpty(title) ? string.Empty : title;
            int TotalRecord = 0;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 20 : (int)pageSize;
         
            var data = new NhaMayBO().GetNhaMaysFuLLPaged(title, CurrPage, RecordPerPage, ref TotalRecord, Loai, Hinhthuc);
            if (data.Count > 0)
                ViewBag.TotalRecord = TotalRecord;
            else
                ViewBag.TotalRecord = 0;
            ViewBag.PageSize = RecordPerPage;
            ViewBag.CurrentPage = CurrPage;



            return PartialView(data);
        }
        public ActionResult GetNhaMayDetail(int Id = 0)
        {

            var model = new NhaMay { Id = 0};
            if (Id > 0)
            {
                model = new NhaMayBO().GetNhaMay(Id);
                if (model == null)
                    return RedirectToAction("Index");

                ViewBag.Title = "Cập nhật nhà máy";
            }
            else
            {
                ViewBag.Title = "Thêm mới nhà máy";
            }
            return View(model);
        }
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult SaveData(NhaMay doc,string SPublishDate)
        {
            var ReturnData = new ReturnData();
            try
            {

                IFormatProvider culture = new CultureInfo("en-US", true);
                doc.NgayThamGia = DateTime.ParseExact(SPublishDate, "dd/MM/yyyy", culture);
                doc.DaXoa =false;
                doc.ThuTuHienThi = 1;
                var result = new NhaMayBO().CreateUpdateNhaMay(doc);
                ReturnData.ResponseCode = result;
                if (result >= 0)
                {

                 
                    if (doc.Id > 0)
                    {
                        ReturnData.Description = "Cập nhật Thành Công";
                        //lognewsobj.Note = "Update nhiệm vụ";
                    }

                    else
                    {
                        ReturnData.Description = "Thêm mới Thành Công";
                        //lognewsobj.Note = "Tạo mới nhiệm vụ";
                    }

                    //Ghi log
                    //Action<ContentLog> send = InsertContentLog;
                    //var asynSend = send.BeginInvoke(lognewsobj, null, null);
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
        
        
    }
}
