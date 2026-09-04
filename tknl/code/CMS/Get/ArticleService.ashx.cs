using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Services;
using BIZ;
using BIZ.Entity;
using UTILS;

namespace CMS.Get
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
                int total = 0;
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
                    //case "get_all_articles_paged":

                    //    var json = new ContentBO().GetAllContentsPaged_JSON(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize));
                    //    context.Response.Write(context.Request.Params["jsoncallback"] + "(" + json + ")");
                    //    return;
                    case "get_filter_articles":

                        string contentTitle = context.Request["tit"];
                        //string groupId = context.Request ["gid"];
                        string categoryId = context.Request["cid"];
                        string status = context.Request["tid"];
                        string type = context.Request["type"];
                        if (string.IsNullOrEmpty(type))
                            type = "-1";
                        var createdby = String.Empty;

                        var lstdata = new ContentBO().GetFilterContentFullsPaged(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), contentTitle, Convert.ToInt32(categoryId), null, Convert.ToInt32(status), "-1", ref total, Convert.ToInt32(type));

                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstdata, string.Empty) + ")");
                        return;

                    case "get_filter_topview_articles":

                        string viewcontentTitle= context.Request["tit"];
                        //string groupId = context.Request ["gid"];
                        string viewcategoryId = context.Request["cid"];
           
                       var todate = DateTime.Now.ToString("dd/MM/yyyy");
                       var fromdate = DateTime.Now.AddMonths(-3).ToString("dd/MM/yyyy");

                        var lstviewdata = new List<CONTENT_FULL>();
                        if(!string.IsNullOrEmpty(viewcontentTitle))
                        {
                            lstviewdata = new ContentBO().GetFilterContentFullsPaged(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), viewcontentTitle, Convert.ToInt32(viewcategoryId), null, 1, "-1", ref total, -1);
                        }
                        else
                        {
                            lstviewdata = new ContentBO().GetFilterContentFullsPaged(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), viewcontentTitle, Convert.ToInt32(viewcategoryId), null, 1, "-1", ref total, 1,fromdate,todate,"","Hits Desc");
                        }

                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstviewdata, string.Empty) + ")");
                        return;

                    case "get_articles":
                        string cId = context.Request["cid"];
                        string stype = context.Request["type"];
                        if (string.IsNullOrEmpty(stype))
                            stype = "-1";


                        var lstnews = new ContentBO().GetFilterContentFullsPaged(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), String.Empty, Convert.ToInt32(cId), null, 1, "-1", ref total, Convert.ToInt32(stype));
                        var data = new List<CONTENT_FULL>();
                        foreach (var contentFull in lstnews)
                        {
                            contentFull.Album = Utils.FormatUrlRewrite(contentFull.Id, contentFull.Title, "ArticleDetail",
                                                                       contentFull.CategoryName);

                        }
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