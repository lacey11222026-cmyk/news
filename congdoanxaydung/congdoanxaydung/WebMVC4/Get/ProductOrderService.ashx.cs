using System;
using System.Web;
using System.Web.Services;
using BIZ;
using UTILS;

namespace WebMVC4.Get
{
    [WebService ( Namespace = "http://tempuri.org/" )]
    [WebServiceBinding ( ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public class ProductOrderService: IHttpHandler
    {

        public void ProcessRequest ( HttpContext context )
        {
            context.Response.ContentType = "text/plain";
            try
            {
                // get current page and record per page                               
                string method = context.Request ["__m"];
                string categoryId;
                switch ( method.ToLower () )
                {
                    case "get_all_productorders":
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( string.Empty, string.Empty ) + ")" );
                        return;
                    case "get_filter_productorders":
                        string productOrderTitle = context.Request ["tit"];
                        string groupId = context.Request ["gid"];
                        categoryId = context.Request ["cid"];
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( new ProductOrderBO ().FilterProductOrderFulls ( productOrderTitle, Convert.ToInt32 ( categoryId ) ), string.Empty ) + ")" );
                        return;
                    case "get_productorders_bycategory":
                        categoryId = context.Request ["cid"];
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( new ProductOrderBO ().GetProductOrdersByCategory ( Convert.ToInt32 ( categoryId ) ), string.Empty ) + ")" );
                        return;
                    case "get_productorder":
                        string productOrderId = context.Request ["_id"];
                        if ( !Utils.IsNumber ( productOrderId ) )
                        {
                            context.Response.Write ( context.Request.Params ["jsoncallback"] + "(null)" );
                            return;
                        }
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( new ProductOrderBO ().GetProductOrderFull ( Convert.ToInt32 ( productOrderId ) ), string.Empty ) + ")" );
                        return;
                    case "get_all_productorders_paged":
                        var pageIndex = context.Request ["_pi"];
                        var pageSize = context.Request ["_ps"];
                        if ( !Utils.IsNumber ( pageIndex ) )
                            pageIndex = "1";
                        if ( !Utils.IsNumber ( pageSize ) )
                            pageSize = "10";

                        var json = new ProductOrderBO ().GetAllProductOrdersPaged_JSON ( Convert.ToInt32 ( pageIndex ), Convert.ToInt32 ( pageSize ) );
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + json + ")" );
                        return;
                }
            }
            catch ( Exception ex )
            {
                ExHandler.Handle(ex, "ProductOrderService");
                context.Response.Write ( context.Request.Params ["jsoncallback"] + "(null)" );
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