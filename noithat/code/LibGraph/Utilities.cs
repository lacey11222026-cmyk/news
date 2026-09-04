using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UTILS;

namespace LibsGraph
{
    public class Utilities
    {
        public static bool CheckPassword(string password)
        {
            //phai bao gom chu va so
            string pattern = @"^(?=.*\d)(?=.*[A-Za-z])(?=.*)(?=.*\d)(.{6,16})$";
            var myRegex = new Regex(pattern);
            var m = myRegex.Match(password);
            if (m.Success)
            {
                return true;
            }

            return false;
        }
        public static string Md5(string input)
        {
            try
            {
                //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
                MD5 md5 = new MD5CryptoServiceProvider();
                var originalBytes = Encoding.Default.GetBytes(input);
                var encodedBytes = md5.ComputeHash(originalBytes);

                //Convert encoded bytes back to a 'readable' string
                return BitConverter.ToString(encodedBytes).ToLower().Replace("-", "");
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string HttpRequestGet(string url)
        {
            var result = string.Empty;
            try
            {
                ServicePointManager.Expect100Continue = false;
                ServicePointManager.DefaultConnectionLimit = 64;
                ServicePointManager.MaxServicePointIdleTime = 500;
                var request = (HttpWebRequest)WebRequest.Create(url);
               
                //request.Proxy = null;
                request.UseDefaultCredentials = true;
                request.CookieContainer = new CookieContainer();
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("X_FORWARDED_FOR", Constants.ClientIP);
                request.Headers.Add("REMOTE_ADDR", Constants.ClientIP);
                //request.ContentType = "text/xml; encoding='utf-8'";
                request.KeepAlive = false;
                request.AllowAutoRedirect = true;
                request.Proxy = null;
                request.ServicePoint.Expect100Continue = false;
                request.Timeout = 6868;
                var response = (HttpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();
                if (stream != null)
                {
                    result = new StreamReader(stream).ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }
        //byte[] array = Encoding.ASCII.GetBytes(input);

        


        public static string HttpRequestPostData(string url, string content)
        {
            string result = string.Empty;

            try
            {
                ServicePointManager.Expect100Continue = false;
                ServicePointManager.DefaultConnectionLimit = 64;
                ServicePointManager.MaxServicePointIdleTime = 500;
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.UseDefaultCredentials = true;
                request.CookieContainer = new CookieContainer();
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.AllowAutoRedirect = true;

                request.Proxy = null;
                request.ServicePoint.Expect100Continue = false;
                request.Timeout = 6868;
                //request.Accept = "JSON";
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
                requestWriter.Write(content);
                requestWriter.Close();


                var response = (HttpWebResponse)request.GetResponse();
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        result = new StreamReader(stream).ReadToEnd();
                        stream.Close();
                        response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        public static string HttpRequestPost(string url, byte[] postData)
        {
            string result = string.Empty;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.UseDefaultCredentials = true;
                request.CookieContainer = new CookieContainer();
                request.Method = "POST";
               // request.ContentType = "raw";
                request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = postData.Length;
                //myRequest.KeepAlive = false;
                request.AllowAutoRedirect = true;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                    stream.Close();
                }
                var response = (HttpWebResponse)request.GetResponse();
                using (var stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        result = new StreamReader(stream).ReadToEnd();
                        stream.Close();
                        response.Close();
                    }
                }
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }
    }
}
