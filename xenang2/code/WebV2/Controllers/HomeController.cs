using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Security.Application;

namespace WebV2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var x=AntiXss.HtmlAttributeEncode("abc");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}