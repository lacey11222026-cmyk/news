using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WebEN.Post
{
    /// <summary>
    /// Summary description for TrafficManual
    /// </summary>
    public class TrafficManual : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            try
            {
                string method = context.Request["m"];
                var data = string.Empty;
                var sectionId = context.Request.Params["id"];

                switch (method.ToLower())
                {
                    case "getsection":
                        using (var webClient = new WebClient())
                        {
                            webClient.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                            data = webClient.DownloadString("http://antoangiaothong.gov.vn/ajax/giotau.ashx?type=tuyen&tuyen=" + sectionId);
                        }

                        context.Response.Write(data);
                        return;

                    case "gettime":
                        var train = context.Request.Params["train"];
                        var from = HttpContext.Current.Server.UrlEncode(context.Request.Params["from"]);
                        var to = HttpContext.Current.Server.UrlEncode(context.Request.Params["to"]);
                        sectionId = context.Request.Params["section"];

                        using (var webClient = new WebClient())
                        {
                            webClient.Headers.Add("Content-Type", "text/html; charset=utf-8");
                            //webClient.Headers.Add("Host", "antoangiaothong.gov.vn");
                            var url = string.Format("http://antoangiaothong.gov.vn/ajax/giotau.aspx?section={0}&train={1}&from={2}&to={3}", sectionId, train, from, to);
                            //url = "http://antoangiaothong.gov.vn/ajax/giotau.aspx?section=3&train=&from=H%C3%A0%20N%E1%BB%99i&to=S%C3%A0i%20G%C3%B2n";
                            data = webClient.DownloadString(url);
                            data = data.Replace("-bcb", "-bgt").Replace("MÁC TẦU", "MÃ TÀU");
                        }

                        context.Response.ContentType = "text/html";
                        context.Response.Write(data);
                        return;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
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