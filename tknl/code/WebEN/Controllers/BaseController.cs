using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using  WebEN.Helper;

namespace WebEN.Controllers
{
    public class BaseController : Controller
    {
        //public void SetCulture(string culture)
        //{
        //    // Validate input
        //    culture = CultureHelper.GetValidCulture(culture);

        //    // Save culture in a cookie
        //    HttpCookie cookie = Request.Cookies["_culture"];
        //    if (cookie != null)
        //        cookie.Value = culture;   // update cookie value
        //    else
        //    {

        //        cookie = new HttpCookie("_culture");
        //        cookie.HttpOnly = false; // Not accessible by JS.
        //        cookie.Value = culture;
        //        cookie.Expires = DateTime.Now.AddYears(1);
        //    }
        //    Response.Cookies.Add(cookie);
            
        //}
        //protected override void Initialize(RequestContext requestContext)
        //{
        //    var lang = requestContext.HttpContext.Request["lang"];
        //    if (!String.IsNullOrEmpty(lang))
        //    {
        //        switch (lang)
        //        {
        //            case "en-us":
        //                SetCulture(lang);
        //                break;
        //            case "en-us":
        //                SetCulture(lang);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    base.Initialize(requestContext);
            
        //}
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Is it View ?
            ViewResultBase view = filterContext.Result as ViewResultBase;
            if (view == null) // if not exit
                return;

            string cultureName = Thread.CurrentThread.CurrentCulture.Name; // e.g. "en-US" // filterContext.HttpContext.Request.UserLanguages[0]; // needs validation return "en-us" as default            

            // Is it default culture? exit
            if (cultureName == CultureHelper.GetDefaultCulture())
                return;


            // Are views implemented separately for this culture?  if not exit
            bool viewImplemented = CultureHelper.IsViewSeparate(cultureName);
            if (viewImplemented == false)
                return;

            string viewName = view.ViewName;

            //int i = 0;

            //if (string.IsNullOrEmpty(viewName))
            //    viewName = filterContext.RouteData.Values["action"] + "." + cultureName; // Index.en-US
            //else if ((i = viewName.IndexOf('.')) > 0)
            //{
            //    // contains . like "Index.cshtml"                
            //    viewName = viewName.Substring(0, i + 1) + cultureName + viewName.Substring(i);
            //}
            //else
            //    viewName += "." + cultureName; // e.g. "Index" ==> "Index.en-Us"

            //view.ViewName = viewName;

            filterContext.Controller.ViewBag._culture = "." + cultureName;

            base.OnActionExecuted(filterContext);
        }


        protected override void ExecuteCore()
        {
            string cultureName = null;
            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                //cultureName = Request.UserLanguages[0]; // obtain it from HTTP header AcceptLanguages
                cultureName = "en-us";
            // Validate culture name
            cultureName = CultureHelper.GetValidCulture(cultureName); // This is safe



            // Modify current thread's culture            
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);


            base.ExecuteCore();
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }
       

    }

}
