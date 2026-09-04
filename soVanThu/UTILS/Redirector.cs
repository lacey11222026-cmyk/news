using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;

namespace UTILS
{
    /// <summary>
    /// Summary description for Redirector
    /// </summary>
    public static class Redirector
    {
        private static Dictionary<string, string> pages;
        private static string centralLoginUrl;

        static Redirector ()
        {
            centralLoginUrl = ConfigurationSettings.AppSettings ["centralLoginUrl"];

            //Register page mappings to force correct casing for the cookie
            //that will eventually be issued.
            pages = new Dictionary<string, string> ( StringComparer.InvariantCultureIgnoreCase );

            pages.Add ( "/Chapter5/AppAUsingCentralLogin/Default.aspx",
                      "/Chapter5/AppAUsingCentralLogin/Default.aspx" );

            pages.Add ( "/Chapter5/AppAUsingCentralLogin/AnotherPage.aspx",
                      "/Chapter5/AppAUsingCentralLogin/AnotherPage.aspx" );

        }

        public static void PerformCentralLogin ( Page p )
        {
            string redirectUrl = FormsAuthentication.GetRedirectUrl ( string.Empty, false );
            //Fixup the casing of the redirect URL to prevent problems with new cookies
            //being issued for a request with incorrect casing on the URL.
            redirectUrl = pages [redirectUrl];
            string baseServer = p.Request.Url.DnsSafeHost;

            string customRedirectUrl = "http://" + baseServer + redirectUrl;

            p.Response.Redirect ( centralLoginUrl + "?CustomReturnUrl=" +
                                p.Server.UrlEncode ( customRedirectUrl ) +
                                "&CustomCookiePath=" +
                                p.Server.UrlEncode ( FormsAuthentication.FormsCookiePath ) );
        }

    }
}