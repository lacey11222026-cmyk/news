using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BIZ;
using BIZ.Entity;
using UTILS;

namespace WebMVC4.Post
{
    /// <summary>
    /// Summary description for SystemService
    /// </summary>
    public class SystemService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            ResponseMsg responseMsg = new ResponseMsg();
            try
            {
                if (!HttpContext.Current.User.IsInRole("Administrator"))
                {
                    responseMsg.Success = false;
                    responseMsg.Text = "Không có quyền";
                    context.Response.Write(responseMsg.ToJsonString());
                    return;
                }
                string method = context.Request["__m"];

                switch (method.ToLower())
                {
                    case "flush":
                        UTILS.Utils.SetAppSettingValue("cacheall", DateTime.Now.ToString("ddMMyyyyHHmm"),
                           context.Request.ApplicationPath);
                        return;
                    case "category":
                        //LocalCaching.Flush ();
                        new CategoryBO().FlushAllCategoryCache(string.Empty);
                        return;
                    case "product":
                        // LocalCaching.Flush ();
                        new ProductBO().FlushAllProductCache(string.Empty);
                        return;
                    case "article":
                        //LocalCaching.Flush ();
                        new ContentBO().FlushAllContentCache(string.Empty);
                        return;
                    case "album":
                        //LocalCaching.Flush ();
                        new AlbumBO().FlushAllAlbumCache(string.Empty);
                        return;
                    case "comment":
                        //LocalCaching.Flush ();
                        new CommentBO().FlushAllCommentCache(string.Empty);
                        return;
                    case "save_appsettings":

                        var data = context.Request.Form.GetValues("data[]");
                        foreach (var i in data)
                        {
                            if (!string.IsNullOrEmpty(i))
                            {
                                var key = i.Split('|')[0];
                                var value = i.Split('|')[1];

                                new SystemConfigBO().SetByKey(key, value);
                                new ContentBO().FlushAllContentCache(string.Empty);
                                //UTILS.Utils.SetAppSettingValue(key, value, context.Request.ApplicationPath);
                            }
                        }

                        return;
                    case "save_appsetting":

                        string s = context.Request["data"];

                        if (!string.IsNullOrEmpty(s))
                        {
                            var key = s.Split('|')[0];
                            var value = s.Split('|')[1];

                            new SystemConfigBO().SetByKey(key, value);
                            new ContentBO().FlushAllContentCache(string.Empty);
                        }


                        return;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex);
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