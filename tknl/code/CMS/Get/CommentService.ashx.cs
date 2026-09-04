using System;
using System.Web;
using System.Web.Services;
using BIZ;
using UTILS;

namespace CMS.Get
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CommentService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                // get current page and record per page
                string method = context.Request["__m"];
                string type;
                string itemid;
                string status;
                var pageIndex = context.Request["_pi"];
                var pageSize = context.Request["_ps"];
                if (!Utils.IsNumber(pageIndex))
                    pageIndex = "1";
                if (!Utils.IsNumber(pageSize))
                    pageSize = "10";
                switch (method.ToLower())
                {
                    case "get_comment":
                        string id = context.Request["_id"];
                        if (!Utils.IsNumber(id))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }

                        var Comment = new CommentBO().GetComment(Convert.ToInt32(id));
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(Comment, string.Empty) + ")");
                        return;

                    case "get_top_comments":
                        string top = context.Request["top"];
                        itemid = context.Request["itemid"];

                        var lstComment = new CommentBO().GetTopLastestComments(Convert.ToInt32(top), -1, Convert.ToInt64(itemid), 1);
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstComment, string.Empty) + ")");
                        return;

                    case "get_filter_comments":
                        string CommentTitle = context.Request["tit"];

                        type = context.Request["type"];
                        itemid = context.Request["itemid"];
                        status = context.Request["status"];
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + new CommentBO().GetCommentsPaged_JSON(CommentTitle, Convert.ToInt32(type), Convert.ToInt64(itemid), int.Parse(status), Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize)) + ")");
                        return;

                }
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "CommentServiceGet", "CommentService");
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