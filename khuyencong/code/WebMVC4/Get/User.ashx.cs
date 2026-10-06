using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UTILS;

namespace WebMVC4.Get
{
    /// <summary>
    /// Summary description for User
    /// </summary>
    public class User : IHttpHandler
    {
        public class AccountInfo
        {
            public string Value { get; set; }
           
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                // get current page and record per page


                //var lstuser = Membership.GetAllUsers();
                var lstdata = Membership.GetAllUsers();
                //ExHandler.Handle(new Exception(), "User", "User" + lstdata.Count);
                List<AccountInfo> lstuser = new List<AccountInfo>();
                foreach (MembershipUser item in lstdata)
                {
                    lstuser.Add(new AccountInfo { Value = item.UserName });
                }
                context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstuser, string.Empty) + ")");
                        return;

                
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "User", "User");
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