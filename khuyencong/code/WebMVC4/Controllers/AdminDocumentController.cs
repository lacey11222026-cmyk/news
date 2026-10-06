using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMVC4.Controllers
{
     [Authorize(Roles = "Administrator,Document")]
    public class AdminDocumentController : Controller
    {
        //
        // GET: /AdminDocument/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddEdit(int Id = 0)
        {
            ViewBag.Id = Id;
            ViewBag.Createdby = HttpContext.User.Identity.Name;
           
           
            return View();
        }
        public ActionResult IndexPrivate()
        {
            return View();
        }
        public ActionResult AddEditPrivate(int Id = 0)
        {
            ViewBag.Id = Id;
            ViewBag.Createdby = HttpContext.User.Identity.Name;


            return View();
        }
    }
}
