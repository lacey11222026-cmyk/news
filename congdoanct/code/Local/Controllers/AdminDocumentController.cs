using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Local.Controllers
{
     [Authorize(Roles = "Administrator,Document,Local")]
    public class AdminDocumentController : Controller
    {
        //
        // GET: /AdminDocument/
         protected override void Initialize(RequestContext requestContext)
         {

             base.Initialize(requestContext);
             if (!User.IsInRole("Local"))
             {
                 Response.Redirect("/");
             }
         }

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
