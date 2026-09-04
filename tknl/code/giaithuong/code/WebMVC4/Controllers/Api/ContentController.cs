using BIZ;
using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UTILS;

namespace WebMVC4.Controllers.Api
{
    public class ContentController : ApiController
    {
        [System.Web.Http.HttpGet]
        public List<CONTENT_API> GetHotNews(int top)
        {
            var lstHotNews = new List<CONTENT_FULL>();
            var configValue = new SystemConfigBO().GetValueByKey("HotNewsForCate_" + Config.WebSite);
            if (!string.IsNullOrEmpty(configValue))
            {

                lstHotNews = new ContentBO().GetTopContentByIdsFulls(configValue, 0, true);

            }
            var lstItem = new List<CONTENT_API>();
            
            foreach (var content in lstHotNews)
            {
                var contentFull = new CONTENT_API()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId.GetValueOrDefault(),
                    CategoryName = content.CategoryName,
                    IntroText = content.IntroText,
                    MainImage = content.MainImage,
                    LinkUrl = content.LinkUrl,
                    PublishDate = content.PublishDate,
                    Title = content.Title,

                };

                lstItem.Add(contentFull);
            }
            return lstItem.Take(top).ToList();
        }
    }
}
