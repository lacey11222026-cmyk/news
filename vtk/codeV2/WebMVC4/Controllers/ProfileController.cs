using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMVC4.Filter;

namespace WebMVC4.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/
        [LocalizationActionFilter]
        public ActionResult Index()
        {
            return View();
        }

    }
}
