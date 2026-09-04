
using BIZ.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web;
using UTILS;

namespace BIZ
{
    public class ServerProcess
    {
        
        private static string getDomain(int Type)
        {
            if (Type == 1)
                return "http://localhost:8080/";
            return "http://congnghesinhhoc.com.vn/";
        }
        public static CONTENT_FULL GetDetail(int type, int id)
        {

            try
            {
                var domain = getDomain(type);
                var url = String.Format("{0}api/Content/GetDetail/?id={1}", domain, id);
                var apitext = Utilities.HttpRequestGet(url);

                return JsonConvert.DeserializeObject<CONTENT_FULL>(apitext);
            }

            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return null;

            }

        }
        public static List<CONTENT_API> GetTopNews(int  type, int top,int category)
        {

            try
            {
                var domain = getDomain(type);
                var url = String.Format("{0}api/Content/GetTop/?top={1}&categoryId={2}", domain, top, category);
                var apitext = Utilities.HttpRequestGet(url);

                return JsonConvert.DeserializeObject<List<CONTENT_API>>(apitext);
            }

            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return null;

            }

        }
        public static List<CONTENT_API> GetHotNews(string domain,int top)
        {

            try
            {
                
                var url = String.Format("{0}api/Content/GetHotNews/?top={1}", domain, top);
                var apitext = Utilities.HttpRequestGet(url);
               
                return JsonConvert.DeserializeObject<List<CONTENT_API>>(apitext);
            }

            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return null;

            }

        }

       
    }
}
