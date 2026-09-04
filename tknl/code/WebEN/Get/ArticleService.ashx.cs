using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using BIZ;
using BIZ.Entity;
using UTILS;

namespace WebEN.Get
{
    /// <summary>
    /// Summary description for ArticleService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ArticleService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                // get current page and record per page
                string method = context.Request["__m"];
                var pageIndex = context.Request["_pi"];
                var pageSize = context.Request["_ps"];
                if (!Utils.IsNumber(pageIndex))
                    pageIndex = "1";
                if (!Utils.IsNumber(pageSize))
                    pageSize = "10";
                int total = 0;
                switch (method.ToLower())
                {
              

                    case "get_articles":
                        string cId = context.Request["cid"];
                        string nId = context.Request["nid"];
						if (!Utils.IsNumber(nId))
						 return;
                        var lstnews = new ContentBO().GetPageContentFullsFrontend(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize),Convert.ToInt32(cId), ref total,"","","",nId);
                        var data = new List<CONTENT_FULL>();
                        
                        
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append("{Total:").Append(total).Append(",Items:").Append(Utils.ConvertToJson(lstnews, string.Empty)).Append("}");

                        var json = stringBuilder.ToString();
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + json + ")");
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