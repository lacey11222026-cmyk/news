using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace cms.libs
{
    public class Constants
    {
        public static string CacheMenuItem = "VTCNews_MENU_VTC_ITEM";
        public static string CacheMenuList = "VTCNews_MENU_VTC_LIST";
        public static string CacheMenuRewrite = "VTCNews_MENU_VTC_REWRITE";
        public static string CacheArticleItem = "VTCNews_Article_VTC_ITEM";
        public static string CacheArticleList = "VTCNews_Article_VTC_LIST";

        public static string ROOT_PATH
        {
            get
            {
                string sRet = System.Web.HttpContext.Current.Request.ApplicationPath;

                if (!sRet.EndsWith("/"))
                    sRet = sRet + "/";
                return sRet;
            }
        }

        public static int MaxStatus
        {
            get
            {
                return ConfigurationManager.AppSettings["MaxStatus"] == null ? 10 : int.Parse(ConfigurationManager.AppSettings["MaxStatus"]);
            }
        }

        public static int MaxIndex
        {
            get
            {
                return ConfigurationManager.AppSettings["MaxIndex"] == null ? 10 : int.Parse(ConfigurationManager.AppSettings["MaxIndex"]);
            }
        }

        public static string imageFileTypes
        {
            get
            {
                return ConfigurationManager.AppSettings["imageFileTypes"] == null ? "" : ConfigurationManager.AppSettings["imageFileTypes"];
            }
        }

        public static string ArticleMediaFileTypes
        {
            get
            {
                return ConfigurationManager.AppSettings["imageFileTypes"] == null ? "" : ConfigurationManager.AppSettings["imageFileTypes"];
            }
        }

        public static string musicFileTypes
        {
            get
            {
                return ConfigurationManager.AppSettings["musicFileTypes"] == null ? "" : ConfigurationManager.AppSettings["musicFileTypes"];
            }
        }

        public static string mediaFileTypes
        {
            get
            {
                return ConfigurationManager.AppSettings["mediaFileTypes"] == null ? "" : ConfigurationManager.AppSettings["mediaFileTypes"];
            }
        }

        public static string documentFileTypes
        {
            get
            {
                return ConfigurationManager.AppSettings["documentFileTypes"] == null ? "" : ConfigurationManager.AppSettings["documentFileTypes"];
            }
        }

        public static string flashFileTypes
        {
            get
            {
                return ConfigurationManager.AppSettings["flashFileTypes"] == null ? "" : ConfigurationManager.AppSettings["flashFileTypes"];
            }
        }

        public static string getIP()
        {
            string IP = "";
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (IP == "")
            {
                IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return IP;
        }
    }
}