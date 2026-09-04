using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace UTILS
{
    public class Config
    {
        public static string DropCate
        {
            get
            {
                var result = ConfigurationManager.AppSettings["DropCate"];

                return !String.IsNullOrEmpty(result) ? result : "DropCate";
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
        public static string UploadUrl
        {
            get
            {
                var result = ConfigurationManager.AppSettings["UploadUrl"];

                return !String.IsNullOrEmpty(result) ? result : "C:\\LogError\\";
            }
        }
        public static int WebSite
        {
            get
            {
                var result = ConfigurationManager.AppSettings["WebSite"];

                return !String.IsNullOrEmpty(result) ? int.Parse(result) :0;
            }
        }
        public static string Domain
        {
            get
            {
                var result = ConfigurationManager.AppSettings["Domain"];

                return !String.IsNullOrEmpty(result) ? result : "C:\\LogError\\";
            }
        }
    }
}
