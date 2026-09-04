using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using BIZ;
using BIZ.Entity;
using UTILS;

namespace CMS.Get
{
    /// <summary>
    /// Summary description for FileService
    /// </summary>
    public class FileService : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/plain";
                string method = context.Request["__m"];
                switch (method.ToLower())
                {
                    case "get_file":
                        var account = context.Request["_account"];
                        if (string.IsNullOrEmpty(account))
                            account = HttpContext.Current.User.Identity.Name;
                        var title = context.Request["_title"];
                        var fromdate = context.Request["_fromdate"];
                        var todate = context.Request["_todate"];
                        var lstImgFiles = new FileUserBO().GetFileUsersByFilter(100, title, "jpg", account, fromdate, todate);
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstImgFiles, string.Empty) + ")");
                        break;
                    case "get_account":
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(GetUserByNewsRole(), string.Empty) + ")");
                        break;
                   

                }
            }
            catch (Exception ex)
            {
                 ExHandler.Handle(ex, "FileServiceGet", "FileService");
                   context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
            }

        }
        public List<EnumInfo> GetUserByNewsRole()
        {
            var list1 = Roles.GetUsersInRole("Administrator");
            var list2 = Roles.GetUsersInRole("NewsEdit");
            var list3 = Roles.GetUsersInRole("NewsPublish");
            var list4 = Roles.GetUsersInRole("NewsCreate");

            var result = new List<EnumInfo>();

            if (list1 != null)
            {
                foreach (var item in list1)
                {
                    if (!String.IsNullOrEmpty(item) && result.Where(x => x.SValue == item) != null)
                        result.Add(new EnumInfo { SValue = item, Text = item });
                }
            }
            if (list2 != null)
            {
                foreach (var item in list2)
                {
                    if (!String.IsNullOrEmpty(item) && result.Where(x => x.SValue == item) != null)
                        result.Add(new EnumInfo { SValue = item, Text = item });
                }
            }
            if (list3 != null)
            {
                foreach (var item in list3)
                {
                    if (!String.IsNullOrEmpty(item) && result.Where(x => x.SValue == item) != null)
                        result.Add(new EnumInfo { SValue = item, Text = item });
                }
            }
            if (list4 != null)
            {
                foreach (var item in list4)
                {
                    if (!String.IsNullOrEmpty(item) && result.Where(x => x.SValue == item) != null)
                        result.Add(new EnumInfo { SValue = item, Text = item });
                }
            }
            return result;
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