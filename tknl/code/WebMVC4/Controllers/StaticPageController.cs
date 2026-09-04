using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  WebMVC4.Models;

namespace WebMVC4.Controllers
{
    public class StaticPageController : Controller
    {
        //
        // GET: /StaticPage/

        public ActionResult Train()
        {
            ViewBag.Title = "Cẩm nang giao thông";
            return View();
        }
        public ActionResult Bus(int cityId = 1, string BusCode = "", string keyword = "", int page = 1)
        {
            ViewBag.Title = "Cẩm nang giao thông";

            // bind dropdownlist
            var allBus = BusInfoDBBase.Create().GetAllBusInfos(cityId, 1).ToList();
            ViewBag.BusCode = new SelectList(allBus.GroupBy(x => x.Number).Select(y => y.First()), "Number", "Number");

            ViewBag.CityName = GetCityName(cityId);

            // get list
            var where = string.Format(" [CityId] = {0} AND [Status]=1 ", cityId);
            if (!string.IsNullOrEmpty(BusCode))
            {
                if (BusCode.Length > 5)
                {
                    //BusCode = BusCode.Substring(0, 5);
                    where += " AND Number LIKE N'%" + BusCode + "%'";
                }
                else
                    where += " AND Number = '" + BusCode + "'";
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                where += string.Format(" AND (TurnOn LIKE N'%{0}%' OR TurnOff LIKE N'%{0}%') ", keyword.Trim());
            }

            var pageSize = 10;
            var order = " Number ASC ";
            var totalRecords = 0;
            var listBus = BusInfoDBBase.Create().GetAllBusInfosPagedDyn("*", where, order, page, pageSize, ref totalRecords).ToList();

            var model = new BusInfoModel
            {
                ListData = listBus,
                PageIndex = page,
                PageSize = pageSize,
                Total = totalRecords
            };

            return View(model);
        }

        private string GetCityName(int cityId)
        {
            switch (cityId)
            {
                case 1: return "Hà Nội";
                case 2: return "TP Hồ Chí Minh";
                default: return "";
            }
        }
        public ActionResult Rescue()
        {
            ViewBag.Title = "Cẩm nang giao thông";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Title = "Liên hệ tòa soạn";
            return View();
        }
        public ActionResult ContactADV()
        {
            ViewBag.Title = "Liên hệ quảng cáo";
            return View();
        }

        public ActionResult OnlineDiscussion()
        {
            ViewBag.Title = "Đăng ký tham gia Giao lưu trực tuyến";
            return View();
        }
    }
}
