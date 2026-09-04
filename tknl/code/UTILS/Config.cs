using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace UTILS
{
    public class Config
    {
        public static string UploadUrl
        {
            get
            {
                var result = ConfigurationManager.AppSettings["UploadUrl"];

                return !String.IsNullOrEmpty(result) ? result : "https://media.tietkiemnangluong.com.vn/Images/Upload/";
            }
        }
        public static string RedisCacheConfig
        {
            get
            {
                var result = ConfigurationManager.AppSettings["RedisCacheConfig"];

                return !String.IsNullOrEmpty(result) ? result : "127.0.0.1:6379:1";
            }
        }
        public static bool EnableCache
        {
            get
            {
                bool result;

                if (bool.TryParse(ConfigurationManager.AppSettings["EnableCachedSite"], out result))
                    return result;

                return true;
            }
        }
        public static bool IsLocalCache
        {
            get
            {
                bool result;

                if (bool.TryParse(ConfigurationManager.AppSettings["IsLocalCache"], out result))
                    return result;

                return true;
            }
        }
        public static string LogErrorFolder
        {
            get
            {
                var result = ConfigurationManager.AppSettings["LogErrorFolder"];

                return !String.IsNullOrEmpty(result) ? result : "C:\\LogError\\";
            }
        }
    }
}
