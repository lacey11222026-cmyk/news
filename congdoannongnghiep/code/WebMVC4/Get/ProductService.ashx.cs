using System;
using System.Web;
using System.Web.Services;
using BIZ;
using UTILS;

namespace WebMVC4.Get
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ProductService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                // get current page and record per page
                string method = context.Request["__m"];
                string categoryId;
                var pageIndex = context.Request["_pi"];
                var pageSize = context.Request["_ps"];
                if (!Utils.IsNumber(pageIndex))
                    pageIndex = "1";
                if (!Utils.IsNumber(pageSize))
                    pageSize = "10";
                switch (method.ToLower())
                {
                    case "get_product":
                        string id = context.Request["_id"];
                        if (!Utils.IsNumber(id))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }

                        var product = new ProductBO().GetProductFull(Convert.ToInt32(id));
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(product, string.Empty) + ")");
                        return;

                    case "get_all_products_paged":
                     

                        var json = new ProductBO().GetAllProductsPaged_JSON(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize));
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + json + ")");
                        return;

                    case "get_filter_products":
                        string productTitle = context.Request["tit"];

                        
                        categoryId = context.Request["cid"];
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + new ProductBO().GetFilterProductsPaged_JSON(Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize),productTitle, Convert.ToInt32(categoryId)) + ")");
                        return;
                    case "get_products_byids":
                        string lstid = context.Request["_lstid"];
                        if (string.IsNullOrEmpty(lstid))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }
                        var lstcontent = new ProductBO().GetTopProductsByIds(lstid, 0, true);
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstcontent, string.Empty) + ")");
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