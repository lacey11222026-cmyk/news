using System;
using System.Web;
using System.Web.Services;
using BIZ;
using UTILS;

namespace Local.Get
{
    [WebService ( Namespace = "http://tempuri.org/" )]
    [WebServiceBinding ( ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public class PromotionService: IHttpHandler
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

                    case "get_all_promotions":
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( string.Empty, string.Empty ) + ")" );
                        return;
                    case "get_filter_promotions":
                        string promotionCode = context.Request ["tit"];
                        string groupId = context.Request ["gid"];
                        categoryId = context.Request ["cid"];
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( new PromotionBO ().FilterPromotionFulls ( promotionCode, Convert.ToInt32 ( categoryId ) ), string.Empty ) + ")" );
                        return;
                    case "get_promotions_bycategory":
                        categoryId = context.Request ["cid"];
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( new PromotionBO ().GetPromotionsByCategory ( Convert.ToInt32 ( categoryId ) ), string.Empty ) + ")" );
                        return;
                    case "get_promotion":
                        string promotionId = context.Request ["_id"];
                        if ( !Utils.IsNumber ( promotionId ) )
                        {
                            context.Response.Write ( context.Request.Params ["jsoncallback"] + "(null)" );
                            return;
                        }
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( new PromotionBO ().GetPromotionFull ( Convert.ToInt32 ( promotionId ) ), string.Empty ) + ")" );
                        return;
                    case "get_all_promotions_paged":
                        var pageIndex = context.Request ["_pi"];
                        var pageSize = context.Request ["_ps"];
                        if ( !Utils.IsNumber ( pageIndex ) )
                            pageIndex = "1";
                        if ( !Utils.IsNumber ( pageSize ) )
                            pageSize = "10";

                        var json = new PromotionBO ().GetAllPromotionsPaged_JSON ( Convert.ToInt32 ( pageIndex ), Convert.ToInt32 ( pageSize ) );
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + json + ")" );
                        return;
                }
            }
            catch ( Exception ex )
            {
                NLogLogger.PublishException(ex);
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