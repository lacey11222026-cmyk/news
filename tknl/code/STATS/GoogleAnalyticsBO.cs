using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web;

namespace STATS
{
    public class GoogleAnalyticsBO
    {
        //static string gaUser = "hoai611@gmail.com";
        //static string oauthTokenFilestorage = "MyOAuthStorage";
        private static string gaApplication = "My Project";
        private static string serviceAccountEmail = "468332990245-kn4e6sc8h31rcf1918umea94ihe5njlt@developer.gserviceaccount.com";
        private static string profileId = "ga:66134485";
        private static AnalyticsService analyticsService;

        public GoogleAnalyticsBO()
        {
            //UserCredential credential;
            var certificate = new X509Certificate2(HttpContext.Current.Server.MapPath("~/") + "key.p12", "notasecret", X509KeyStorageFlags.Exportable);

            var credential = new ServiceAccountCredential(
               new ServiceAccountCredential.Initializer(serviceAccountEmail)
               {
                   Scopes = new[] { AnalyticsService.Scope.AnalyticsReadonly }
               }.FromCertificate(certificate));


            //using (var stream = new FileStream(HttpContext.Current.Server.MapPath("~/") + "client_secret.json", FileMode.Open, FileAccess.Read))
            //{
            //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        new[] { AnalyticsService.Scope.AnalyticsReadonly },
            //        gaUser,
            //        CancellationToken.None,
            //        new FileDataStore(oauthTokenFilestorage)).Result;
            //}

            analyticsService = new AnalyticsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = gaApplication
            });
        }

        public List<OverviewByHour> GetOverviewByHour(DateTime from, DateTime to)
        {
            var result = new List<OverviewByHour>();

            var start = from.ToString("yyyy-MM-dd");
            var end = to.ToString("yyyy-MM-dd");

            var metrics = "ga:sessions,ga:users,ga:pageviews";
            var query = analyticsService.Data.Ga.Get(profileId, start, end, metrics);

            query.Dimensions = "ga:hour";
            query.SamplingLevel = DataResource.GaResource.GetRequest.SamplingLevelEnum.HIGHERPRECISION;
            query.MaxResults = 24;

            var response = query.Execute();

            var str = string.Empty;

            foreach (var row in response.Rows)
            {
                var item = new OverviewByHour
                {
                    Hour = Convert.ToInt32(row[0]),
                    Sessions = Convert.ToInt32(row[1]),
                    Users = Convert.ToInt32(row[2]),
                    Pageviews = Convert.ToInt32(row[3])
                };

                result.Add(item);
            }

            return result;
        }

        public List<TopContentByDay> GetTopCategory(DateTime from, DateTime to, int site = 0)
        {
            var result = new List<TopContentByDay>();

            var siteName = GetSiteName(site);
            var start = from.ToString("yyyy-MM-dd");
            var end = to.ToString("yyyy-MM-dd");

            var metrics = "ga:pageviews,ga:users";
            var query = analyticsService.Data.Ga.Get(profileId, start, end, metrics);

            query.Dimensions = "ga:pagePath,ga:pageTitle,ga:pagePathLevel2";
            //query.Filters = "ga:pageTitle=@Báo Giao thông điện tử;ga:pagePathLevel2=~^/c.*/";
            query.Filters = "ga:pagePathLevel2=~^/c.*/";
            query.Sort = "-ga:pageviews";
            query.SamplingLevel = DataResource.GaResource.GetRequest.SamplingLevelEnum.HIGHERPRECISION;
            query.MaxResults = 50;

            var response = query.Execute();

            var str = string.Empty;

            foreach (var row in response.Rows)
            {
                if (row[2].Length < 6)
                {
                    var item = new TopContentByDay
                    {
                        Url = row[0].ToString(),
                        Title = row[1].ToString().Split("|".ToCharArray()).FirstOrDefault().Trim(),
                        SiteName = "Tiết kiệm năng lượng",
                        Pageviews = Convert.ToInt32(row[3]),
                        Users = Convert.ToInt32(row[4])
                    };
                    result.Add(item);
                }
            }

            var filterResult = new List<TopContentByDay>();
            foreach (var item in result)
            {
                if (!item.SiteName.Contains("Trang-"))
                    filterResult.Add(item);
            }

            return filterResult;
        }

        public List<TopContentByDay> GetTopContent(DateTime from, DateTime to, int site = 0)
        {
            var result = new List<TopContentByDay>();

            var siteName = GetSiteName(site);
            var start = from.ToString("yyyy-MM-dd");
            var end = to.ToString("yyyy-MM-dd");

            var metrics = "ga:pageviews,ga:users";
            var query = analyticsService.Data.Ga.Get(profileId, start, end, metrics);

            query.Dimensions = "ga:pagePath,ga:pageTitle,ga:pagePathLevel2";
            query.Filters = "ga:pagePathLevel3=~^/t.*/";
            query.Sort = "-ga:pageviews";
            query.SamplingLevel = DataResource.GaResource.GetRequest.SamplingLevelEnum.HIGHERPRECISION;
            query.MaxResults = 50;

            var response = query.Execute();

            var str = string.Empty;

            foreach (var row in response.Rows)
            {
                if(row.Count>4)
                {
                    if (row[2].Length < 10)
                    {
                        var item = new TopContentByDay
                        {
                            Url = row[0].ToString(),
                            Title = row[1].ToString().Split("|".ToCharArray()).FirstOrDefault().Trim(),
                            SiteName = "Tiết kiệm năng lượng",
                            Pageviews = Convert.ToInt32(row[3]),
                            Users = Convert.ToInt32(row[4])
                        };
                        result.Add(item);
                    }
                }
                    
               
            }

            return result;
        }

        private string GetSiteName(int site)
        {
            switch (site)
            {
                case 0:
                    return "Báo Giao thông điện tử";
                    break;
                case 1:
                    return "Chuyên trang An toàn giao thông";
                    break;
                default:
                    return "Báo Giao thông điện tử";
                    break;
            }
        }
    }
}
