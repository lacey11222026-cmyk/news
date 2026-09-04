using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMVC4.Helper
{
    public static class WorkContext
    {


        /// <summary>
        /// Lấy giá trị sesstion bằng key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Trả về dạng string</returns>
        public static object GetSessionKey(string key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                var sesstion = HttpContext.Current.Session[key];
                if (sesstion != null)
                    return sesstion;
            }
            return null;
        }
        /// <summary>
        /// Lấy giá trị sesstion bằng key
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Trả về dạng string</returns>
        public static string GetSessionKeyToString(string key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                var sesstion = HttpContext.Current.Session[key];
                if (sesstion != null)
                    return sesstion as string;
            }
            return string.Empty;
        }
        /// <summary>
        /// Sét giá trị vào sesstion
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Giá trị</param>
        public static void SetSessionKey(string key, object value)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                HttpContext.Current.Session[key] = value;
        }
        /// <summary>
        /// Bỏ giá trị của sesstion
        /// </summary>
        /// <param name="key">Key</param>
        public static void RemoveSessionKey(string key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Response != null && HttpContext.Current.Session[key] != null)
                HttpContext.Current.Session.Remove(key);
        }

        public static string GetLanguage()
        {
            var request = System.Web.HttpContext.Current.Request;
            var lang = "vi-vn";
            if (request.Cookies["lang"] != null)
            {
                lang= request.Cookies["lang"].Value.ToString().ToLowerInvariant();
            }
            if(lang!="vi-vn"&& lang != "en-us")
            {
                lang = "vi-vn";
            }
            return lang;
        }
        public static void SetLanguage(string value)
        {
            HttpCookie ck = new HttpCookie("lang", value) { HttpOnly = true, Path = " / "};
            ck.Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Request.Cookies.Set(ck);
            HttpContext.Current.Response.Cookies.Set(ck);
        }
       
    }
}