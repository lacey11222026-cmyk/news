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
    [RoutePrefix("api/Content")]
    public class ContentController : ApiController
    {
        [Route("GetHotNews")]
        public List<CONTENT_API> GetHotNews(int top)
        {
            var lstHotNews = new List<CONTENT_FULL>();
            var configValue = new SystemConfigBO().GetValueByKey("HotNewsForCate_" + Config.WebSite);
            if (!string.IsNullOrEmpty(configValue))
            {

                lstHotNews = new ContentBO().GetTopContentByIdsFulls(configValue, 0, true);

            }
            // var lstHotNews = new ContentBO().GetTopLastestContentFulls(100, 0).Where(x => x.CategoryId.GetValueOrDefault() == 36 || x.CategoryId.GetValueOrDefault() == 38 || x.CategoryId.GetValueOrDefault() == 40).Take(top).ToList();

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
        [Route("GetTop")]

        public List<CONTENT_FULL> GetTop(int top, string title)
        {
            int total = 0;
            return new ContentBO().GetPageContentFullsFrontend(1, top, -1, ref total, "", "", title);
        }
        [Route("GetDetail")]

        public CONTENT_FULL GetDetail(int id)
        {
            return new ContentBO().GetContentFull(id);
        }
    }
}
