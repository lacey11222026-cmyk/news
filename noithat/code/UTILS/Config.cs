using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;

namespace UTILS
{
    public class Config
    {
        public static string LogErrorFolder
        {
            get
            {
                var result = ConfigurationManager.AppSettings["LogErrorFolder"];

                return !String.IsNullOrEmpty(result) ? result : "C:\\LogError\\";
            }
        }
        public static string ApplicationUrl
        {
            get
            {
                var url = ConfigurationManager.AppSettings["DOMAIN"] ?? "http://" + HttpContext.Current.Request.Url.Authority +
                   HttpContext.Current.Request.ApplicationPath;
                return url.EndsWith("/") ? url : url + "/";
            }
        }
        public static string AdminAcount
        {
            get
            {
                var result = ConfigurationManager.AppSettings["AdminAcount"];

                return !String.IsNullOrEmpty(result) ? result : ",cuongpmk49ca@gmail.com,tranlieu86@gmail.com,nguyenvu01@gmail.com,hanguyen1606@gmail.com";
            }
        }
    }
}
