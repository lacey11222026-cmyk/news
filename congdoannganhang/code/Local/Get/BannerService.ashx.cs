using System;
using System.Web;
using System.Web.Services;
using BIZ;
using UTILS;

namespace Local.Get
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BannerService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                // get current page and record per page
                string method = context.Request["__m"];
                string region;
               
                string status;
                var pageIndex = context.Request["_pi"];
                var pageSize = context.Request["_ps"];
                if (!Utils.IsNumber(pageIndex))
                    pageIndex = "1";
                if (!Utils.IsNumber(pageSize))
                    pageSize = "10";
                switch (method.ToLower())
                {
                    case "get_banner":
                        string id = context.Request["_id"];
                        if (!Utils.IsNumber(id))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }

                        var Banner = new BannerBO().GetBanner(Convert.ToInt32(id));
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(Banner, string.Empty) + ")");
                        return;

                    case "get_top_banners":
                        string top = context.Request["top"];
                        status = context.Request["status"];
                        region = context.Request["region"];
                        var lstBanner = new BannerBO().GetTopLastestBanners(Convert.ToInt32(top), Convert.ToInt32(region), Convert.ToInt32(status));
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstBanner, string.Empty) + ")");
                        return;

                    

                }
            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "BannerServiceGet", "BannerService");
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