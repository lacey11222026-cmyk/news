using Car.CMS.Filter;
using Car.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Car.CMS.Controllers
{
    public class DashBoardController : Controller
    {
        // GET: DashBoard
        [PermissionFilter(FunctionCode = FunctionCode.Revenue)]
        public ActionResult Revenue()
        {
            return View();
        }
    }
}