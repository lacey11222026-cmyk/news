using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace WebMVC4.Filter
{
    public class LocalizationActionFilter : ActionFilterAttribute, IActionFilter

    {

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)

        {

            if (filterContext.RouteData.Values["lang"] != null &&

                 !string.IsNullOrWhiteSpace(filterContext.RouteData.Values["lang"].ToString()))

            {

                // set the culture from the route data (url)

                var lang = filterContext.RouteData.Values["lang"].ToString();

                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(lang);

            }

            else

            {

                // load the culture info from the cookie

                var cookie = filterContext.HttpContext.Request.Cookies["lang"];

                var langHeader = string.Empty;

                if (cookie != null)

                {

                    // set the culture by the cookie content

                    //langHeader = cookie.Value;
                    //hardcode
                    langHeader = "vi-VN";

                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);

                }

                else

                {
                    // set the culture by the location if not speicified

                    langHeader = "vi-VN";

                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(langHeader);

                }

                // set the lang value into route data

                filterContext.RouteData.Values["lang"] = langHeader;

            }



            // save the location into cookie

            HttpCookie _cookie = new HttpCookie("lang", Thread.CurrentThread.CurrentUICulture.Name);

            _cookie.Expires = DateTime.Now.AddYears(1);

            filterContext.HttpContext.Response.SetCookie(_cookie);



            this.OnActionExecuting(filterContext);

        }

    }
}