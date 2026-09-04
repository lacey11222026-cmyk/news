using System;
using System.Web;
using System.Web.Services;
using BIZ;
using UTILS;

namespace WebMVC4.Get
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DocumentService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                // get current page and record per page
                string method = context.Request["__m"];
                string categoryId;
                string status;
                var pageIndex = context.Request["_pi"];
                var pageSize = context.Request["_ps"];
                if (!Utils.IsNumber(pageIndex))
                    pageIndex = "1";
                if (!Utils.IsNumber(pageSize))
                    pageSize = "10";
                switch (method.ToLower())
                {
                    case "get_document":
                        string id = context.Request["_id"];
                        if (!Utils.IsNumber(id))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }

                        var Document = new DocumentBO().GetDocument(Convert.ToInt32(id));
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(Document, string.Empty) + ")");
                        return;



                    case "get_filter_documents":
                        string DocumentTitle = context.Request["tit"];

                        categoryId = context.Request["cid"];
                        status = context.Request["status"];
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + new DocumentBO().GetDocumentsPaged_JSON(DocumentTitle, Convert.ToInt32(categoryId), Convert.ToInt32(status), Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize)) + ")");
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