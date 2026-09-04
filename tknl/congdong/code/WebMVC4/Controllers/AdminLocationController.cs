using DATA.ContentDB;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UTILS;
using WebMVC4.Models;

using Newtonsoft.Json;
using BIZ;
using System.Linq;

namespace WebMVC4.Controllers
{
    [Authorize(Roles = "Administrator,Category")]
    public class AdminLocationController : Controller
    {
        //
        // GET: /AdminLocation/

        public ActionResult Index()
        {
            var listLocation = new TestLocationBO().GetAll();
            return View(listLocation);
        }
        public ActionResult Info(string name)
        {
           
            ViewBag.Title = "Cập nhật";
            var obj= new TestLocationBO().GetAll().FirstOrDefault(x => x.Name==name);
            return View(obj);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult SaveData(TestLocation location)
        {
            var ReturnData = new ReturnData();

            try
            {
                //banner.ImageUrl = Config.UrlRoot + (string.IsNullOrEmpty(banner.ImageUrl) ? string.Empty : banner.ImageUrl.Substring(12));

                var result = TestLocationDAL.InsertUpdate(location);
                ReturnData.ResponseCode = result;

                if (result >= 0)
                {
                    ReturnData.Description = "Cập nhật Thành Công";


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
