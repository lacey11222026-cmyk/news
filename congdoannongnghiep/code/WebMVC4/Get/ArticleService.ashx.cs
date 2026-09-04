using System;
using System.Text;
using System.Web;
using System.Web.Services;
using BIZ;
using UTILS;

namespace WebMVC4.Get
{
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
                switch (method.ToLower())
                {
                    case "get_article":
                        string id = context.Request["_id"];
                        if (!Utils.IsNumber(id))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }
                        var content = new ContentBO().GetContentFull(Convert.ToInt32(id));
                        //ExHandler.Handle(new Exception("create"), "ArticleServiceGet", "create " + content.CreatedBy);
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(content, string.Empty) + ")");
                        return;

                    case "get_article_byids":
                        string lstid = context.Request["_lstid"];
                        if (string.IsNullOrEmpty(lstid))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }
                        var lstcontent = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true);
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstcontent, string.Empty) + ")");
                        return;
                    case "get_all_articles_paged":

                        var json = new ContentBO().GetAllContentsPaged_JSON(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize));
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + json + ")");
                        return;
                    case "get_topview_articles":
                        string contentTitle1 = context.Request["tit"];
                        //string groupId = context.Request ["gid"];
                        string categoryId1 = context.Request["cid"];

                        var lstviewdata = new ContentBO().GetTopViewContentFulls(20, Convert.ToInt32(categoryId1), contentTitle1);
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append("{Total:").Append("20").Append(",Items:").Append(Utils.ConvertToJson(lstviewdata, string.Empty)).Append("}");

                        var jsondata = stringBuilder.ToString();
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + jsondata + ")");
                        return;
                    case "get_filter_articles":

                        string contentTitle = context.Request["tit"];
                        //string groupId = context.Request ["gid"];
                        string categoryId = context.Request["cid"];
                        string type = context.Request["tid"];

                        var createdby = String.Empty;

                        if (type == "1" || type == "3")
                        {
                            createdby = context.Request["createdby"];

                        }
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + new ContentBO().GetFilterContentsPaged_JSON(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), contentTitle, Convert.ToInt32(categoryId), Convert.ToInt32(type), createdby) + ")");
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