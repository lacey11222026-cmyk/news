using BIZ;
using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Models;
namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Category")]
    public class AdminCityController : Controller
    {
        //
        // GET: /AdminCity/

        public ActionResult ManageCity()
        {
            ViewBag.Title = "Quản trị kết quả";
            return View();
        }

        public ActionResult ListCity(int? status,int? type, int? currentPage, int? pageSize)
        {
           
            var data = new List<City>();

            int TotalRecord = 0;
            int Status = status == null ? -1 : (int)status;
            int Type = type == null ? -1 : (int)type;
            int CurrPage = currentPage == null ? 1 : (int)currentPage;
            int RecordPerPage = pageSize == null ? 200 : (int)pageSize;
            data = new CityBO().GetTopCity(0, Status,Type);
            TotalRecord = data.Count();
            if (data!=null)
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
            var model = new City
            {
                Name = ""
            };


           
            if (PageID > 0)
            {
                model = new CityBO().GetCity(PageID);
            }


            ViewBag.id = Id;

            if (Id > 0)
            {
                ViewBag.Title = "Cập nhật tỉnh thành";
            }
            else
            {
                ViewBag.Title = "Thêm mới tỉnh thành";
            }
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveData(City City)
        {
            var ReturnData = new  ReturnData();

            try
            {
               
               

                //banner.ImageUrl = Config.UrlRoot + (string.IsNullOrEmpty(banner.ImageUrl) ? string.Empty : banner.ImageUrl.Substring(12));

                

                var result = new CityBO().CreateUpdateCity(City);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    if (City.Id > 0)
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
       

    }
}
