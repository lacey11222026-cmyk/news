using BIZ;
using BIZ.Entity;
using DATA;
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


        [Route("GetTop")]

        public List<CONTENT_API> GetTop(int top, int categoryId)
        {
            var lstdata = new List<CONTENT_API>();
            var data = new ContentBO().GetHotNews(categoryId, top);
            foreach (var content in data)
            {
                var item = new CONTENT_API
                {
                    CategoryId = content.CategoryId.GetValueOrDefault(),
                    IntroText = content.IntroText,
                    Id = content.Id,
                    MainImage = content.MainImage,
                    LinkUrl = content.LinkUrl,
                    PublishDate = content.PublishDate,
                    Title = content.Title,

                };
                lstdata.Add(item);
            }
            return lstdata;
        }
        [Route("GetHotNews")]
        public List<CONTENT_API> GetHotNews(int top, string lang)
        {
            var lstdata = new List<CONTENT_API>();
            var data = new List<CONTENT_FULL>();
            int categoryId = 0;
            if (lang == "vi-vn")
            {
                // var configValue = new SystemConfigBO().GetByKey("HotNews");
                categoryId = 99;
            }
            else
            {
                //var configValue = new SystemConfigBO().GetByKey("HotNews_" + OtherPage.EngPage);
                categoryId = 104;
            }
            var configValue = new SystemConfigBO().GetByKey("HotNewsForCate_" + categoryId);
            data = new ContentBO().GetTopContentByIdsFulls(configValue.ConfigValue, top, true).ToList();
            foreach (var content in data)
            {
                var item = new CONTENT_API
                {
                    CategoryId = content.CategoryId.GetValueOrDefault(),
                    IntroText = content.IntroText,
                    Id = content.Id,
                    MainImage = content.MainImage,
                    LinkUrl = "https://scp.gov.vn"+content.LinkUrl,
                    PublishDate = content.PublishDate,
                    Title = content.Title,

                };
                lstdata.Add(item);
            }
            return lstdata;
        }
        [Route("GetDetail")]

        public CONTENT_APIFULL GetDetail(int id)
        {
            var data = new ContentBO().GetContentFull(id);
            var item = new CONTENT_APIFULL
            {
                Title = data.Title,
                Contents = data.Contents,
                MainImage = data.MainImage,
            };
            return item;
        }
    }
    public class CONTENT_API
    {
        public string MainImage
        {
            get;
            set;
        }
        public string LinkUrl
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string IntroText
        {
            get;
            set;
        }
        public int CategoryId
        {
            get;
            set;
        }
        public string CategoryName
        {
            get;
            set;
        }
        public long Id
        {
            get;
            set;
        }
        public DateTime PublishDate
        {
            get;
            set;
        }
    }
    public class CONTENT_APIFULL
    {
        public string Title
        {
            get;
            set;
        }
        public string Contents
        {
            get;
            set;
        }
        public string MainImage
        {
            get;
            set;
        }
    }
}
