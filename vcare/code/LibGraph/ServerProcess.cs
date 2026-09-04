using LibsGraph;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTILS;

namespace LibGraph
{
    public class ServerProcess
    {
        private static readonly string Url = "https://www.impex-jp.com/api/parts/search.html";
        public static ApiResponse Search(string part_no)
        {
            try
            {
                var requestUrl = $"{Url}?part_no={part_no}";
                var apiResponsetext = Utilities.HttpRequestGet(requestUrl);
                //NLogLogger.DebugMessage(apiResponsetext);
                if(apiResponsetext== "{\"original_parts\":[]}")
                    return null;
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(apiResponsetext);

                return apiResponse;
            }

            catch (Exception ex)
            {
                ExHandler.Handle(ex, "ServerProcess", "Search");

                return null;
            }
        }
        public static string test(string lang)
        {
            try
            {
                var requestUrl = "http://finos-core-alb-439983116.ap-southeast-1.elb.amazonaws.com:8895/content-service/api/v1/distributors/tutorials/investment/consumer";
                var apiResponsetext = Utilities.HttpRequestGet(requestUrl,lang);
                //NLogLogger.DebugMessage(apiResponsetext);
                if (apiResponsetext == "{\"original_parts\":[]}")
                    return "";
                //var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(apiResponsetext);

                return apiResponsetext;
            }

            catch (Exception ex)
            {
                ExHandler.Handle(ex, "ServerProcess", "Search");

                return null;
            }
        }
    }
}
