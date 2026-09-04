using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UTILS;

namespace BIZ
{
    public static class GoogleApiService
    {
        private static string clientId = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["GOOGLE_CLIENT_ID"]) ? ConfigurationManager.AppSettings["GOOGLE_CLIENT_ID"] : "642898042332.apps.googleusercontent.com";
        private static string clientSecrect = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["GOOGLE_CLIENT_SECRECTKEY"]) ? ConfigurationManager.AppSettings["GOOGLE_CLIENT_SECRECTKEY"] : "jTV2bL9YDP5IQVr6PIa3fjLn";

        private const string URL_AUTHOR = "https://accounts.google.com/o/oauth2/auth";
        private const string URL_GET_TOKEN = "https://accounts.google.com/o/oauth2/token";
        private const string URL_GET_USERINFO = "https://www.googleapis.com/oauth2/v1/userinfo?access_token={0}";

        public static string GetUrlLogin(string urlReturn, string state)
        {
            return string.Format("{0}?client_id={1}&response_type=code&scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.email+https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.profile&redirect_uri={2}&state={3}&login_hint=", URL_AUTHOR, clientId, urlReturn, state);
        }

        public static GoogleAccessTokenEntity GetAccessToken(string code, string urlReturn)
        {
            try
            {
                //var sw = new Stopwatch();

                //sw.Start();

                var data =
                    string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code", code, clientId, clientSecrect, urlReturn);

                var dataResponse = RequestHelper.WebRequestPost(data, URL_GET_TOKEN);

                var googleToken = JsonConvert.DeserializeObject<GoogleAccessTokenEntity>(dataResponse);

                //sw.Stop();

                //Logger.Info(string.Format("[GoogleApiService][GetAccessToken] token: {0}, time: {1} ms", googleToken.access_token, sw.ElapsedMilliseconds));

                return googleToken;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return null;
            }
        }

        public static GoogleUserInfo GetUserInfo(string token)
        {
            try
            {
                var sw = new Stopwatch();

                sw.Start();

                var url =
                    string.Format(URL_GET_USERINFO, token);

                var dataResponse = RequestHelper.WebRequestGet(url);

                sw.Stop();

                var googleInfo = JsonConvert.DeserializeObject<GoogleUserInfo>(dataResponse);

                NLogLogger.DebugMessage(string.Format("[GoogleApiService][GetUserInfo] token: {0}, email: {1} time: {2}", token, googleInfo != null ? googleInfo.email : "", sw.ElapsedMilliseconds));

                return googleInfo;
            }
            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                return null;
            }
        }
    }
    public class GoogleAccessTokenEntity
    {
        public string access_token { get; set; }
        public string id_token { get; set; }
        public string expires_in { get; set; }
        public string token_type { get; set; }
    }

    public class GoogleUserInfo
    {
        public string id { get; set; }
        public string email { get; set; }
        public bool verified_email { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string link { get; set; }
        public string gender { get; set; }
        public string locale { get; set; }
    }
}
