
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
                return "http://localhost:8091/";
            return "http://localhost:8090/";
        }
        private static string getLocalDomain(string Domain)
        {
            if (Domain == "https://tietkiemnangluong.com.vn/"|| Domain == "https://vneec.gov.vn/")
                return "http://localhost:8080/";
            return "http://localhost:8090/";
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
        public static List<CONTENT_FULL> GetTopNews(int  type, int top,string title)
        {

            try
            {
                var domain = getDomain(type);
                var url = String.Format("{0}api/Content/GetTop/?top={1}&title={2}", domain, top, title);
                var apitext = Utilities.HttpRequestGet(url);

                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(apitext);
            }

            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return null;

            }

        }
        
        public static List<CONTENT_API> GetHotNews(string domain,int top,string lang)
        {

            try
            {
                
                var url = String.Format("{0}api/Content/GetHotNews/?top={1}&lang={2}", getLocalDomain(domain), top,lang);
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
