using System;
using System.Web;
using System.Web.Services;
using BIZ;
using UTILS;

namespace WebMVC4.Get
{
    [WebService ( Namespace = "http://tempuri.org/" )]
    [WebServiceBinding ( ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public class ManufactoryService: IHttpHandler
    {

        public void ProcessRequest ( HttpContext context )
        {
            context.Response.ContentType = "text/plain";
            try
            {
                // get current page and record per page
                string method = context.Request ["__m"];
              //string categoryId;
                switch ( method.ToLower () )
                {
                    case "get_manufactory":
                        string id = context.Request ["_id"];
                        if ( !Utils.IsNumber ( id ) )
                        {
                            context.Response.Write ( context.Request.Params ["jsoncallback"] + "(null)" );
                            return;
                        }

                        var manufactory = new ManufactoryBO ().GetManufactoryFull ( Convert.ToInt32 ( id ) );
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( manufactory, string.Empty ) + ")" );
                        return;

                    case "get_all_manufactories_paged":
                        var pageIndex = context.Request ["_pi"];
                        var pageSize = context.Request ["_ps"];
                        if ( !Utils.IsNumber ( pageIndex ) )
                            pageIndex = "1";
                        if ( !Utils.IsNumber ( pageSize ) )
                            pageSize = "10";

                        var json = new ManufactoryBO ().GetAllManufactoriesPaged_JSON ( Convert.ToInt32 ( pageIndex ), Convert.ToInt32 ( pageSize ) );
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + json + ")" );
                        return;

                    case "get_filter_manufactories":
                        string manufactoryTitle = context.Request ["tit"];
                        context.Response.Write ( context.Request.Params ["jsoncallback"] + "(" + Utils.ConvertToJson ( new ManufactoryBO ().FilterManufactoryFulls ( manufactoryTitle ), string.Empty ) + ")" );
                        return;

                    case "get_all_manufactories":
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(new ManufactoryBO().GetAllManufactoryFulls(), string.Empty) + ")");
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