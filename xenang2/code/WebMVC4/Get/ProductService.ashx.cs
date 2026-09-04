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
    public class ProductService : IHttpHandler
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
                 

                    case "get_byids":
                        string lstid = context.Request["_lstid"];
                        if (string.IsNullOrEmpty(lstid))
                        {
                            context.Response.Write(context.Request.Params["jsoncallback"] + "(null)");
                            return;
                        }
                        var lstcontent = new ProductBO().GetTopProductByIdsFulls(lstid, 0, true);
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstcontent, string.Empty) + ")");
                        return;
                    
                    case "get_filter":

                        string contentTitle = context.Request["tit"];
                        //string groupId = context.Request ["gid"];
                        string categoryId = context.Request["cid"];
                        string type = context.Request["tid"];
                        int total = 0;
                        var lstdata = new ProductBO().GetProductsPaged(contentTitle, Convert.ToInt32(categoryId), -1,1,Convert.ToInt32(pageSize),ref total, 1,false,false,"");
                        context.Response.Write(context.Request.Params["jsoncallback"] + "(" + Utils.ConvertToJson(lstdata, string.Empty) + ")");
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