using System;
using System.Web;
using BIZ;
using UTILS;

namespace Local.Get
{
    /// <summary>
    /// Summary description for SupportService
    /// </summary>
    public class SupportService : IHttpHandler
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
                    case "get_all_supports":
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(string.Empty, string.Empty) + ")");
                        return;
                    case "get_filter_supports":
                        string supporter = context.Request["tit"];                       
                        categoryId = context.Request["cid"];
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(new SupportBO().FilterSupportFulls(supporter, Convert.ToInt32(categoryId)), string.Empty) + ")");
                        return;
                    case "get_supports_bycategory":
                        categoryId = context.Request["cid"];
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(new SupportBO().GetSupportsByCategory(Convert.ToInt32(categoryId)), string.Empty) + ")");
                        return;
                    case "get_support":
                        string supportId = context.Request["_id"];
                        if (!Utils.IsNumber(supportId))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(new SupportBO().GetSupportFull(Convert.ToInt32(supportId)), string.Empty) + ")");
                        return;
                    case "get_all_supports_paged":
                        var pageIndex = context.Request["_pi"];
                        var pageSize = context.Request["_ps"];
                        if (!Utils.IsNumber(pageIndex))
                            pageIndex = "1";
                        if (!Utils.IsNumber(pageSize))
                            pageSize = "10";

                        var json = new SupportBO().GetAllSupportsPaged_JSON(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize));
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + json + ")");
                        return;
                }
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "SupportServiceGet", "SupportService");
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