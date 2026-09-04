
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
        public class CONTENT_API
        {
            public int Year
            {
                get;
                set;
            }
            public int Type
            {
                get;
                set;
            }
            public double Total1
            {
                get;
                set;
            }
            public double Total2
            {
                get;
                set;
            }
            public long Money
            {
                get;
                set;
            }

        }

        public static CONTENT_API GetData()
        {

            try
            {
                
                var url = "http://localhost:8088/api/HomeReport/GetResult";
                var apitext = Utilities.HttpRequestGet(url);
               
                return JsonConvert.DeserializeObject<CONTENT_API>(apitext);
            }

            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return null;

            }

        }

       
    }
}
