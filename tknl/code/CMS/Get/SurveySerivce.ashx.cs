using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIZ;
using UTILS;

namespace CMS.Get
{
    /// <summary>
    /// Summary description for ServeySerivce
    /// </summary>
    public class SurveySerivce : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                // get current page and record per page
                string method = context.Request["__m"];
               
               
                switch (method.ToLower())
                {
                   
                    case "get_filter_survey":

                        string contentTitle = context.Request["tit"];
                        //string groupId = context.Request ["gid"];
                        string categoryId = context.Request["cid"];
                        string type = context.Request["tid"];

                        var createdby = String.Empty;
                        int total = 0;
                        var lstdata = new SurveyBO().GetAllSurveys(30,1,-1,contentTitle);

                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstdata, string.Empty) + ")");
                        return;
                }
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "ArticleServiceeGet", "ArticleService");
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