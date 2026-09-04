using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace UTILS
{
    public static class RequestHelper
    {
        public static string WebRequestPost(string postData, string url)
        {
            var _return = string.Empty;

            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest != null)
            {
                StreamWriter requestWriter;
                webRequest.Method = "POST";
                webRequest.ServicePoint.Expect100Continue = false;
                //webRequest.Timeout = 200000;
                webRequest.ContentType = "application/x-www-form-urlencoded";

                //POST the data.
                using (requestWriter = new StreamWriter(webRequest.GetRequestStream(), Encoding.ASCII))
                {
                    requestWriter.Write(postData);
                }
            }

            if (webRequest != null)
            {
                var resp = (HttpWebResponse)webRequest.GetResponse();
                var resStream = resp.GetResponseStream();
                if (resStream != null)
                {
                    var reader = new StreamReader(resStream);
                    _return = reader.ReadToEnd();
                }
            }

            return _return;
        }

        public static string WebRequestGet(string url)
        {
            var _return = string.Empty;

            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest != null)
            {
                StreamWriter requestWriter;
                webRequest.Method = "GET";
                webRequest.ServicePoint.Expect100Continue = false;
                webRequest.Timeout = 200000;
                webRequest.ContentType = "text/xml; encoding='utf-8'";

                //POST the data.
                //using (requestWriter = new StreamWriter(webRequest.GetRequestStream(), Encoding.ASCII))
                //{
                //    requestWriter.Write(postData);
                //}
            }

            if (webRequest != null)
            {
                var resp = (HttpWebResponse)webRequest.GetResponse();
                var resStream = resp.GetResponseStream();
                if (resStream != null)
                {
                    var reader = new StreamReader(resStream);
                    _return = reader.ReadToEnd();
                }
            }

            return _return;
        }
    }
}
