using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace cms.libs
{
    public class Utilities
    {
        static Utilities()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static string getIP()
        {
            string IP = "";
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (IP == "")
            {
                IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return IP;
        }
        public static void jAlert(string Message)
        {
            HttpContext.Current.Response.Write("<script type='text/javascript' language='javascript'>alert('" + Message + "');window.location.href='" + HttpContext.Current.Request.Url.PathAndQuery + "';</script>");

        }
    }

}
