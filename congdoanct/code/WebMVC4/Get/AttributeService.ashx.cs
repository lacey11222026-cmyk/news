using System;
using System.Web;
using System.Web.Services;
using BIZ;
using UTILS;

namespace WebMVC4.Get
{

    [WebService ( Namespace = "http://tempuri.org/" )]
    [WebServiceBinding ( ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public class AttributeService: IHttpHandler
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
                    case "get_all_attributes":
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( string.Empty, string.Empty ) + ")" );
                        return;
                    case "get_filter_attributes":
                        string attributeTitle = context.Request ["tit"];
                        string groupId = context.Request ["gid"];
                        categoryId = context.Request ["cid"];
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( new AttributeBO ().FilterAttributeFulls ( attributeTitle, Convert.ToInt32 ( categoryId ), Convert.ToInt32 ( groupId ) ), string.Empty ) + ")" );
                        return;
                      
                    case "get_attributes_bycategory":
                        categoryId = context.Request ["cid"];
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( new AttributeBO ().GetAllAttributesByCategory ( Convert.ToInt32 ( categoryId ) ), string.Empty ) + ")" );
                        return;
                    case "get_all_attrgroups":
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( new AttributeGroupBO ().GetAllAttributeGroupsFull (), string.Empty ) + ")" );
                        return;
                    case "get_attribute":
                        string attributeId = context.Request ["_id"];
                        if ( !Utils.IsNumber ( attributeId ) )
                        {
                            context.Response.Write ( context.Request.Params ["jsoncallback"] + "(null)" );
                            return;
                        }
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( new AttributeBO ().GetAttributeFull ( Convert.ToInt32 ( attributeId ) ), string.Empty ) + ")" );
                        return;
                    case "get_attrgroup":
                        string attributeGroupId = context.Request ["_id"];
                        if ( !Utils.IsNumber ( attributeGroupId ) )
                        {
                            context.Response.Write ( context.Request.Params ["jsoncallback"] + "(null)" );
                            return;
                        }
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( new AttributeGroupBO ().GetAttributeGroupFull ( Convert.ToInt32 ( attributeGroupId ) ), string.Empty ) + ")" );
                        return;
                    case "get_all_attributes_paged":
                        var pageIndex = context.Request ["_pi"];
                        var pageSize = context.Request ["_ps"];
                        if ( !Utils.IsNumber ( pageIndex ) )
                            pageIndex = "1";
                        if ( !Utils.IsNumber ( pageSize ) )
                            pageSize = "10";

                        var json = new AttributeBO ().GetAllAttributesPaged_JSON ( Convert.ToInt32 ( pageIndex ), Convert.ToInt32 ( pageSize ) );
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