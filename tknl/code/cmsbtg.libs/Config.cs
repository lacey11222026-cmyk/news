using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net;
using System.Web;
using System.Web.Security;

namespace cms.libs
{
    /// <summary>
    /// Summary description for Config
    /// </summary>
    public sealed class Config
    {
        private static readonly Config instance = new Config();
        private string _UrlRoot;
        private string _SiteUrl;
        private string _SQLConn;
        private string _SQLConn_search;
        private string _SQLConn_common;
        private string _CachePool;

        public static string SQLConn
        {
            get
            {
                return instance._SQLConn;
            }
        }

        public static string SQLConn_search
        {
            get
            {
                return instance._SQLConn_search;
            }
        }

        public static string SQLConn_common
        {
            get
            {
                return instance._SQLConn_common;
            }
        }

        private string _SSODomain;

        public static string SSODomain
        {
            get
            {
                return instance._SSODomain;
            }
        }

        private string _mediaUrl;

        public static string mediaUrl
        {
            get
            {
                string sRet = instance._mediaUrl;
                if (!sRet.EndsWith("/"))
                    sRet += "/";
                return sRet;
            }
        }

        private string _mediaPath;

        public static string mediaPath
        {
            get
            {
                string sRet = instance._mediaPath;
                if (!sRet.EndsWith("/"))
                    sRet += "/";
                return sRet;
            }
        }
        public static string UrlRoot
        {
            get
            {
                return instance._UrlRoot;
            }
        }

        public static string SiteUrl
        {
            get
            {
                return instance._SiteUrl;
            }
        }

        public static string CachePool
        {
            get
            {
                return instance._CachePool;
            }
        }

        private Config()
        {
            _SQLConn = getConnStr("SQLConn");
            _CachePool = getAppStr("CachedPool");
            _SSODomain = getAppStr("SSO_DOMAIN");
            _mediaUrl = getAppStr("mediaUrl");
            _mediaPath = getAppStr("mediaPath");
            _UrlRoot = getAppStr("rootPath");
            _SiteUrl = getAppStr("SITE_URL");
        }

        private string getConnStr(string Name)
        {
            return ConfigurationManager.ConnectionStrings[Name].ConnectionString;
        }

        private string getAppStr(string Name)
        {
            return ConfigurationSettings.AppSettings[Name] == null ? "" : ConfigurationSettings.AppSettings[Name].ToString();
        }

        private static Config Instance
        {
            get { return instance; }
        }
    }
}