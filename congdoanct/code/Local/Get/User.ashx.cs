using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using UTILS;

namespace Local.Get
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public class User : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                // get current page and record per page


                var lstuser = Membership.GetAllUsers();

                context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstuser, string.Empty) + ")");
                        return;



                
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                return;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}