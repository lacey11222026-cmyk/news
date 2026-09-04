
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
