using System;
using System.Web;
using System.Web.Security;

namespace UTILS
{
    /// <summary>
    /// Summary description for RedirectWrapper
    /// </summary>
    public static class RedirectWrapper
    {
        public static string FormatRedirectUrl ( string redirectUrl )
        {
            HttpContext c = HttpContext.Current;
            if ( c == null )
                throw new InvalidOperationException ( "You must have an active context to perform a redirect" );

            //Don't append the forms auth ticket for unauthenticated users or
            //for users authenticated with a different mechanism
            if ( !c.User.Identity.IsAuthenticated ||
                !( c.User.Identity.AuthenticationType == "Forms" ) )
                return redirectUrl;

            //Determine if we need to append to an existing query string or not
            string qsSpacer;
            if ( redirectUrl.IndexOf ( "?" ) > 0 )
                qsSpacer = "&";
            else
                qsSpacer = "?";

            //Build the new redirect URL
            string newRedirectUrl;
            FormsIdentity fi = ( FormsIdentity ) c.User.Identity;
            newRedirectUrl = redirectUrl + qsSpacer +
                    FormsAuthentication.FormsCookieName + "=" + FormsAuthentication.Encrypt ( fi.Ticket );

            return newRedirectUrl;
        }
    }
}