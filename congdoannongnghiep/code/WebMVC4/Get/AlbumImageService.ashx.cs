using System;
using System.Web;
using System.Web.Services;
using BIZ;
using UTILS;

namespace WebMVC4.Get
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AlbumImageService : IHttpHandler
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
                string type;
                string order;
                var pageIndex = context.Request["_pi"];
                var pageSize = context.Request["_ps"];
                if (!Utils.IsNumber(pageIndex))
                    pageIndex = "1";
                if (!Utils.IsNumber(pageSize))
                    pageSize = "10";
                switch (method.ToLower())
                {
                    case "get_album":
                        string id = context.Request["_id"];
                        if (!Utils.IsNumber(id))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }

                        var Album = new AlbumImageBO().GetAlbum(Convert.ToInt32(id));
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(Album, string.Empty) + ")");
                        return;


                    case "get_album_byids":
                        string lstid = context.Request["_lstid"];
                        if (string.IsNullOrEmpty(lstid))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }
                        var lstcontent = new AlbumImageBO().GetTopAlbumByIdsFulls(lstid, 0, true);
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstcontent, string.Empty) + ")");
                        return;
                    case "get_filter_albums":
                        string AlbumTitle = context.Request["tit"];

                        categoryId = context.Request["cid"];
                        status = context.Request["status"];
                        order = context.Request["order"];
                        
                        var fromdate = context.Request["fromdate"];
                        var todate = context.Request["todate"];
                            type = context.Request["type"];
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(" + new AlbumImageBO().GetAlbumsPaged_JSON(AlbumTitle, Convert.ToInt32(categoryId), Convert.ToInt32(status), Convert.ToInt32(type), Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), fromdate, todate, order) + ")");
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