using System;
using System.Web;
using BIZ;
using UTILS;

namespace WebMVC4.Get
{
    /// <summary>
    /// Summary description for ContactService
    /// </summary>
    public class ContactService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                // get current page and record per page                               
                string method = context.Request["__m"];
                string categoryId;
                switch (method.ToLower())
                {
                   
                    case "get_contacts_bycategory":
                        categoryId = context.Request["cid"];
                        
                        string status = context.Request["status"];
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(new ContactBO().GetContactsByCategory(Convert.ToInt32(categoryId), Convert.ToInt32(status)), string.Empty) + ")");
                        return;
                    case "get_contact":
                        string ContactId = context.Request["_id"];
                        if (!Utils.IsNumber(ContactId))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(new ContactBO().GetContact(Convert.ToInt32(ContactId)), string.Empty) + ")");
                        return;
                   
                }
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