using BIZ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMVC4.Controllers
{
    public class ShopController : Controller
    {
        //
        // GET: /Shop/

        public ActionResult Index()
        {
            var data = new ShopBO().GetTopShop(-1, -1);
            return View(data);
        }
        public ActionResult LoadMapping(int cityId, int districtId, int agentId)
        {
            var model = new ShopBO().GetTopShop(-1, -1);
            ViewBag.Zoom = 15;
            if (districtId == 0)
                ViewBag.Zoom = 14;
            if (model.Count > 0)
            {

                if (agentId == 0)
                {
                    agentId = model.FirstOrDefault().Id;
                    

                }

            }

            ViewBag.agentId = agentId;
            var Data = "";
            var html = "";
            foreach (var item in model)
            {
                html = String.Format("<strong>{0}</strong><br>{1}<br>Liên hệ: {2}", item.Name, item.Address, item.Phone);
                if (item.Id == agentId)
                {
                    Data += String.Format("addMarkerActive(new google.maps.LatLng({0},{1}), \"{2}\",\"{3}\"); ",  item.Latitude.Trim(), item.Longitude.Trim(), item.Name, html);
                }
                else
                {
                    Data += String.Format("addMarker(new google.maps.LatLng({0},{1}), \"{2}\",\"{3}\"); ",  item.Latitude.Trim(), item.Longitude.Trim(), item.Name, html);
                }

            }
            ViewBag.Data = Data;
            var activeAgent = model.Where(x => x.Id == agentId).FirstOrDefault();
            return PartialView(activeAgent);
        }
    }
}
