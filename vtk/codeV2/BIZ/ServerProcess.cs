
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
                return "http://congnghiepcongnghecao.com.vn/";
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
