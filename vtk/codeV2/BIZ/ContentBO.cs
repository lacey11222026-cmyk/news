using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using UTILS;

namespace BIZ
{
    public class ContentBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_CONTENT;
        protected delegate void DelegateFlushAllContentCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE

        public int CreateUpdateContent(Content content)
        {
            return ContentDBBase.Create().CreateUpdateContent(content);
        }

        public int CreateUpdateContent(CONTENT_FULL contentFull)
        {
            Content content = contentFull.ConvertToBase();
            int returnVal = CreateUpdateContent(content);
            if (returnVal != -1)
            {
                //UpdateCache(contentFull);
                FlushAllContentCache(strGroupKeyCached);
            }

            return returnVal;
        }
        public int Mark(long id, float mark)
        {


            var returnVal = ContentDBBase.Create().Mark(id, mark);
            if (returnVal >= 0)
            {
                FlushAllContentCache(strGroupKeyCached);

            }


            return returnVal;

        }
        public List<LogView> GetTopViewsContent(int top, string fromdate = "", string todate = "")
        {
            var result = ContentDBBase.Create().GetTopViewsContent(top, fromdate, todate);
            if (result == null)
                return null;
            return result.ToList();
        }
        public List<LogView> GetTopViewsCate(int top, string fromdate = "", string todate = "")
        {
            var result = ContentDBBase.Create().GetTopViewsCate(top, fromdate, todate);
            if (result == null)
                return null;
            return result.ToList();
        }
        public int ViewAdd(long id,int CategoryId)
        {

            var returnVal = ContentDBBase.Create().ViewAdd(id, CategoryId);
            //if (returnVal >= 0)
            //{
            //    var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_CONTENT + id;
            //    var item = (CONTENT_FULL)LocalCaching.GetData(strKeyCached);
            //    if (item != null)
            //    {
            //        item.Hits = item.Hits + 1;

            //        LocalCaching.Add(strKeyCached, item);

            //    }

            //}
            return returnVal;

        }
        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get content by content id
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <returns></returns>
        public Content GetContent(int contentId)
        {
            return ContentDBBase.Create().GetContent(contentId);
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get content by id => add to local cache
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <returns></returns>
        public CONTENT_FULL GetContentFull(int contentId)
        {
            try
            {
                var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_CONTENT + contentId;
                
                var cachedata = RedisCaching.GetData(strKeyCached);
                if (cachedata != null)
                    return JsonConvert.DeserializeObject<CONTENT_FULL>(cachedata.ToString());
                //var item = (CONTENT_FULL)LocalCaching.GetData(strKeyCached);
                //if (item != null)
                //    return item;

                var content = GetContent(contentId);

                var item = new CONTENT_FULL
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Title = content.Title,
                    Alias = content.Alias,
                    IntroText = content.IntroText,
                    Contents = content.Contents,
                    Image = content.Image,
                    Thumbnail = content.Thumbnail,
                    //ChannelId = content.ChannelId,
                    Url = content.Url,
                    CreatedBy = content.CreatedBy,
                    CreatedDate = content.CreatedDate,
                    CategoryPathway = content.CategoryPathway,
                    PublishDate = content.PublishDate,
                    Status = content.Status,
                    Type = content.Type,
                    Hits = content.Hits,
                    Params = content.Params,
                    SiteId = content.SiteId,
                    SiteUrl = content.SiteUrl,
                };

                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));

                return item;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }
        public CONTENT_FULL GetContentFullNotCache(int contentId)
        {
            try
            {
                //var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_CONTENT + contentId;

                //var item = (CONTENT_FULL)LocalCaching.GetData(strKeyCached);
                //if (item != null)
                //    return item;

                var content = GetContent(contentId);

                var item = new CONTENT_FULL
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Title = content.Title,
                    Alias = content.Alias,
                    IntroText = content.IntroText,
                    Contents = content.Contents,
                    Image = content.Image,
                    Thumbnail = content.Thumbnail,
                    //ChannelId = content.ChannelId,
                    Url = content.Url,
                    CreatedBy = content.CreatedBy,
                    CreatedDate = content.CreatedDate,
                    CategoryPathway = content.CategoryPathway,
                    PublishDate = content.PublishDate,
                    Status = content.Status,
                    Type = content.Type,
                    Hits = content.Hits,
                    Params = content.Params,
                    SiteId = content.SiteId,
                    SiteUrl = content.SiteUrl,
                };

                //LocalCaching.Add(strKeyCached, item);

                return item;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }
        private IEnumerable<Content> GetAllContentsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var contents = ContentDBBase.Create().GetAllContentsPaged(pageIndex, pageSize, ref totalRecords);
            if (contents == null)
                return null;

            return contents;
        }

        public List<CONTENT_FULL> GetAllContentFullsPaged(int pageIndex, int pageSize, ref int totalRecords)
        {
            var contents = GetAllContentsPaged(pageIndex, pageSize, ref totalRecords);
            if (contents == null)
                return null;

            List<CONTENT_FULL> contentFulls = new List<CONTENT_FULL>();
            foreach (var content in contents)
            {
                CONTENT_FULL contentFull = new CONTENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Title = content.Title,
                    Alias = content.Alias,
                    IntroText = content.IntroText,
                    //Contents = content.Contents,
                    Image = content.Image,
                    Thumbnail = content.Thumbnail,
                    Url = content.Url,
                    CreatedBy = content.CreatedBy,
                    CreatedDate = content.CreatedDate,
                    CategoryPathway = content.CategoryPathway,
                    PublishDate = content.PublishDate,
                    Status = content.Status,
                    Type = content.Type,
                    Hits = content.Hits,
                    Params = content.Params,
                    SiteId = content.SiteId,
                    SiteUrl = content.SiteUrl,
                };
                if (HttpContext.Current.Request.Url.ToString().Contains("noibo"))
                {
                    contentFull.Contents = content.Contents;
                }
                contentFulls.Add(contentFull);
            }

            return contentFulls;

        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 23/08/2011 05:01 PM
        /// todo: get list of contents have paged and in json format
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public string GetAllContentsPaged_JSON(int pageIndex, int pageSize)
        {
            string keyCache = strGroupKeyCached + "_" + Constants.CACHE_KEY_ALL_CONTENTS_PAGED_JSON + pageIndex + "_" + pageSize;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_CONTENT;

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
            //    LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            //var json = (string)LocalCaching.GetData(keyCache);
            //if (!string.IsNullOrEmpty(json))
            //    return json;
            var cachedata = RedisCaching.GetData(keyCache);
            if (cachedata != null)
                return cachedata.ToString();

            int totalRecords = 0;
            List<CONTENT_FULL> contents = GetAllContentFullsPaged(pageIndex, pageSize, ref totalRecords);

            if (contents == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(contents, string.Empty)).Append("}");

            var json = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(json))
            {
                RedisCaching.Add(keyCache, json.ToString());
            }

            return json;
        }
        public string GetFilterContentsPaged_JSON(int pageIndex, int pageSize, string title, int categoryId, int status, string createdby, string fromdate = "", string todate = "")
        {
            string keyCache = strGroupKeyCached + "_" + Constants.CACHE_KEY_ALL_CONTENTS_PAGED_JSON + pageIndex + pageSize + title + categoryId + status + createdby + fromdate + todate;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_CONTENT;

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
            //    LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            //var json = (string)LocalCaching.GetData(keyCache);
            //if (!string.IsNullOrEmpty(json))
            //    return json;
            var cachedata = RedisCaching.GetData(keyCache);
            if (cachedata != null)
                return cachedata.ToString();

            int totalRecords = 0;
            List<CONTENT_FULL> contents = GetFilterContentFullsPaged(pageIndex, pageSize, title, categoryId, null, status, createdby, ref totalRecords, fromdate, todate);

            if (contents == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(contents, string.Empty)).Append("}");

            var json = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(json))
            {
                RedisCaching.Add(keyCache, json.ToString());
            }

            return json;
        }
        private IEnumerable<Content> FilterContents(int pageIndex, int pageSize, string title, int categoryId, List<int> lstcate, int status, string createdby, ref int totalRecords, string fromdate = "", string todate = "", string lststatus = "", string alias = "", string orderBy = "PublishDate DESC")
        {
            var contents = ContentDBBase.Create().GetFilterContents(pageIndex, pageSize, title, categoryId, lstcate, status, createdby, ref totalRecords, fromdate, todate, lststatus, alias, orderBy);
            if (contents == null)
                return null;
            return contents;
        }
        public List<MarkST> GetFiltertSTMark(string fromdate = "", string todate = "")
        {
            var data = ContentDBBase.Create().GetSTContentMark(string.Empty, 0, 4, "-1", fromdate, todate).ToList();
            return data;
        }

        public List<CONTENT_FULL> GetFilterContentFullsPaged(int pageIndex, int pageSize, string title, int categoryId, List<int> lstcate, int status, string createdby, ref int totalRecords, string fromdate = "", string todate = "", string lststatus = "", string alias = "", string orderBy = "PublishDate DESC")
        {
            var contents = FilterContents(pageIndex, pageSize, title, categoryId, lstcate, status, createdby, ref totalRecords, fromdate, todate, lststatus, alias, orderBy);
            if (contents == null)
                return new List<CONTENT_FULL>();
            List<CONTENT_FULL> lstContentFulls = new List<CONTENT_FULL>();
            foreach (var content in contents)
            {
                CONTENT_FULL contentFull = new CONTENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Title = content.Title,
                    Alias = content.Alias,
                    IntroText = content.IntroText,
                    //Contents = content.Contents,
                    Image = content.Image,
                    //Thumbnail = content.Thumbnail,
                    Url = content.Url,
                    CreatedBy = content.CreatedBy,
                    CreatedDate = content.CreatedDate,
                    CategoryPathway = content.CategoryPathway,
                    PublishDate = content.PublishDate,
                    Status = content.Status,
                    Type = content.Type,
                    Hits = content.Hits,
                    Mark = content.Mark,
                    Params = content.Params,
                    SiteId = content.SiteId,
                    SiteUrl = content.SiteUrl,
                };
                if (HttpContext.Current.Request.Url.ToString().Contains("noibo"))
                {
                    contentFull.Contents = content.Contents;
                }
                lstContentFulls.Add(contentFull);
            }

            return lstContentFulls;
        }

        public List<CONTENT_FULL> GetContentFullsByCategory(int categoryId)
        {
            string keyCache = Constants.CACHE_KEY_ALL_CONTENTS_BYCATEGORY + categoryId;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_CONTENT;

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
            //    LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            //var lstCachedContents = (List<CONTENT_FULL>)LocalCaching.GetData(keyCache);

            //if (lstCachedContents != null)
            //    return lstCachedContents;
            var cachedata = RedisCaching.GetData(keyCache);
            if (cachedata != null)
                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());
            int total = 0;
            var contents = FilterContents(1, 1000, string.Empty, categoryId, null, -1, string.Empty, ref total);

            if (contents == null)
                return null;

            var publishedContents = (from p in contents where p.Status == 4 select p).ToList();

            var lstCachedContents = new List<CONTENT_FULL>();
            foreach (var content in publishedContents)
            {
                CONTENT_FULL contentFull = new CONTENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Title = content.Title,
                    Alias = content.Alias,
                    IntroText = content.IntroText,
                    Contents = content.Contents,
                    Image = content.Image,
                    //Thumbnail = content.Thumbnail,
                    Url = content.Url,
                    CreatedBy = content.CreatedBy,
                    CreatedDate = content.CreatedDate,
                    CategoryPathway = content.CategoryPathway,
                    PublishDate = content.PublishDate,
                    Status = content.Status,
                    Type = content.Type,
                    Hits = content.Hits,
                    Params = content.Params,
                    SiteId = content.SiteId,
                    SiteUrl = content.SiteUrl,
                };

                lstCachedContents.Add(contentFull);
            }

            if (lstCachedContents.Count > 0)
            {
                //LocalCaching.Add(keyCache, lstCachedContents);
                //LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
                RedisCaching.Add(keyCache, JsonConvert.SerializeObject(lstCachedContents));
                //RedisCaching.Add(strKeyCachedTotal, totalRecords.ToString());
            }

            return lstCachedContents;
        }

        private IEnumerable<Content> GetTopLastestContents(int top, int categoryId, int site)
        {
            var result = ContentDBBase.Create().GetTopLastestContents(top, categoryId,site);
            if (result == null)
                return null;
            return result;
        }
        private IEnumerable<Content> GetTopViewContents(int top, int categoryId, string title, string fromdate = "", string todate = "")
        {
            var result = ContentDBBase.Create().GetTopViewContents(top, categoryId, title, fromdate, todate);
            if (result == null)
                return null;
            return result;
        }
        private IEnumerable<Content> GetTopContentByIds(string ids, int top)
        {
            var result = ContentDBBase.Create().GetTopContentByIds(ids, top);
            if (result == null)
                return null;
            return result;
        }
        public List<CONTENT_FULL> GetTopContentByIdsFulls(string ids, int top, bool isArragne = false)
        {
            var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_TOP_CONTENTS_BYIDS + top + "_lst" + ids + isArragne;

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());


            var lstItem = (List<CONTENT_FULL>)LocalCaching.GetData(strKeyCached);
            //var lstItem = new List<CONTENT_FULL> ();
            if (lstItem != null && lstItem.Count > 0)
                return lstItem;

            var lstItemBase = GetTopContentByIds(ids, top);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            lstItem = new List<CONTENT_FULL>();
            if (isArragne)
            {

                var listIds = ids.Split(',').ToList();
                foreach (var itemid in listIds)
                {
                    if (!string.IsNullOrEmpty(itemid))
                    {
                        foreach (var content in lstItemBase)
                        {
                            if (content.Id == long.Parse(itemid))
                            {
                                var item = new CONTENT_FULL()
                                {
                                    Id = content.Id,
                                    Title = content.Title,
                                    PublishDate = content.PublishDate,
                                    IntroText = content.IntroText,
                                    Image = content.Image,
                                    Thumbnail = content.Thumbnail,
                                    Url = content.Url,
                                    Type=content.Type
                                };
                                lstItem.Add(item);
                                break;
                            }

                        }
                    }
                }
            }
            else
            {
                foreach (var content in lstItemBase)
                {
                    var item = new CONTENT_FULL()
                    {
                        Id = content.Id,
                        Title = content.Title,
                        PublishDate = content.PublishDate,
                        IntroText = content.IntroText,
                        Image = content.Image,
                        Url = content.Url
                    };

                    lstItem.Add(item);
                }
            }


            if (lstItem.Count > 0)
            {

                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                //LocalCaching.Add(strKeyCached, lstItem);
                //LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            }

            return lstItem;
        }
        public List<CONTENT_FULL> GetHotNews(int categoryId, int maxLastestNews)
        {
            try
            {
                var strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_HOTNEWS + categoryId + "_max" + maxLastestNews;

                //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
                //if (listGroupKey == null)
                //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                var cachedata = RedisCaching.GetData(strKeyCached);
                if (cachedata != null)
                    return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());

                //var data = LocalCaching.GetData(strKeyCached);
                //if (data != null)
                //    return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(data.ToString());


                var lstItemBase = GetHotNewsPrivate(categoryId, maxLastestNews);
                if (lstItemBase == null || lstItemBase.Count() == 0)
                    return null;
                if (lstItemBase.Count > 0)
                {


                    RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItemBase));
                }

                return lstItemBase;
            }
            catch
            {
                return new List<CONTENT_FULL>();
            }
          
        }
        private List<CONTENT_FULL> GetHotNewsPrivate(int categoryId, int maxLastestNews)
        {

            var lstdata = new ContentBO().GetTopLastestContentFulls(maxLastestNews, categoryId);
            
            var configValue = new SystemConfigBO().GetByKey("HotNewsForCate_" + categoryId);
            if (configValue != null)
            {
                var lstid = configValue.ConfigValue;
                if (string.IsNullOrEmpty(lstid))
                {
                    return lstdata;
                }
                var lstcontent = new ContentBO().GetTopContentByIdsFulls(lstid, 0, true).ToList();

                if (lstcontent == null || lstcontent.Count < 1)
                {
                    return lstdata;
                }
                if (lstdata == null)
                    return lstcontent;
                foreach (var item in lstdata)
                {

                    if (!lstcontent.Where(x => x.Id == item.Id).Any())
                    {
                        lstcontent.Add(item);

                    }
                }
                lstcontent = lstcontent.Take(maxLastestNews).ToList();
                return lstcontent;
            }
            return lstdata;
        }
        public List<CONTENT_FULL> GetPageContentFullsFrontend(int pageIndex, int pageSize, int categoryId, ref int totalRecords, string fromdate = "", string todate = "", string title = "", string lstNotId = "", string lang = "", int type = -1, int site = 0)
        {
            string strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_TOP_LASTEST_CONTENTS_FRONTEND + pageIndex + "_pz" + pageSize + "_cate" + categoryId + "_fd" + fromdate + "_td" + todate + "_t" + title + "_ls" + lstNotId + "_lang" + lang + "_type" + type + "_chid" + site;
            string strKeyCachedTotal = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_CONTENTS_FRONTEND + pageIndex + "_pz" + pageSize + "_cate" + categoryId + "_fd" + fromdate + "_td" + todate + "_t" + title + "_ls" + lstNotId + "_lang" + lang + "_type" + type + "_total" + "_chid" + site;

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //{
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            //    LocalCaching.AddToGroupKey(strKeyCachedTotal, strGroupKeyCached);
            //}
            //var data = LocalCaching.GetData(strKeyCached);
            //if (data != null)
            //{
            //    //totalMark = int.Parse((string)LocalCaching.GetData(strKeyCachedTotalMark));
            //    totalRecords = int.Parse((string)LocalCaching.GetData(strKeyCachedTotal));
            //    return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(data.ToString());
            //}
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());
            }
            var lstItemBase = ContentDBBase.Create().GetFilterContentsFrontend(pageIndex, pageSize, title, categoryId, ref totalRecords, fromdate, todate, lstNotId, lang, type,site);

            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            var lstItem = new List<CONTENT_FULL>();
            //var cateDic = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.News).ToDictionary(t => t.Id);
            foreach (var content in lstItemBase)
            {
                var item = new CONTENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    //CategoryName = content.CategoryId.HasValue ? cateDic[content.CategoryId.Value].Name : "",
                    Title = content.Title,
                    //Alias = content.Alias,
                    IntroText = content.IntroText,
                    //Contents = content.Contents,
                    Image = content.Image,
                    Thumbnail = content.Thumbnail,
                    Url = content.Url,
                    //CreatedBy = content.CreatedBy,
                    //CreatedDate = content.CreatedDate,
                    //CategoryPathway = content.CategoryPathway,
                    PublishDate = content.PublishDate,
                    Status = content.Status,
                    Type = content.Type,
                    Hits = content.Hits,
                    Params = content.Params,
                    SiteId = content.SiteId,
                    SiteUrl = content.SiteUrl,
                };

                lstItem.Add(item);
            }

            if (lstItem.Count > 0)
            {
                //LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                //LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                //LocalCaching.Add(strKeyCachedTotal, totalRecords.ToString());
                //LocalCaching.AddToGroupKey(strKeyCachedTotal, strGroupKeyCached);
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                RedisCaching.Add(strKeyCachedTotal, totalRecords.ToString());
            }

            return lstItem;
        }
        /// <summary>
        /// Cuong pm cread
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryId"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<CONTENT_FULL> GetPageContentFulls(int pageIndex, int pageSize, int categoryId, ref int totalRecords)
        {
            string strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_TOP_LASTEST_CONTENTS + pageIndex + "_pagesize" + pageSize + "_category" + categoryId;
            string strKeyCachedTotal = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_CONTENTS + pageIndex + "_pagesize" + pageSize + "_category" + categoryId + "total";

            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());
            }

            var lstItemBase = FilterContents(pageIndex, pageSize, String.Empty, categoryId, null, 4, String.Empty, ref totalRecords);

            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            var lstItem = new List<CONTENT_FULL>();

            foreach (var content in lstItemBase)
            {
                var item = new CONTENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Title = content.Title,
                    //Alias = content.Alias,
                    IntroText = content.IntroText,
                    //Contents = content.Contents,
                    Image = content.Image,
                    Thumbnail = content.Thumbnail,
                    Url = content.Url,
                    //CreatedBy = content.CreatedBy,
                    //CreatedDate = content.CreatedDate,
                    //CategoryPathway = content.CategoryPathway,
                    PublishDate = content.PublishDate,
                    Status = content.Status,
                    Type = content.Type,
                    Hits = content.Hits,
                    Params = content.Params,
                    SiteId = content.SiteId,
                    SiteUrl = content.SiteUrl,
                };

                lstItem.Add(item);
            }

            if (lstItem.Count > 0)
            {
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                RedisCaching.Add(strKeyCachedTotal, totalRecords.ToString());
            }

            return lstItem;
        }
        public List<CONTENT_FULL> GetTopViewContentFulls(int top, int categoryId = 0, string title = "", string fromdate = "", string todate = "")
        {
            var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_TOP_VIEW_CONTENTS + top + "_category" + categoryId + "_title" + title + "_fr" + fromdate + "_t" + todate;

            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                //totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());
            }

            var lstItemBase = GetTopViewContents(top, categoryId, title, fromdate, todate);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            var lstItem = new List<CONTENT_FULL>();

            foreach (var content in lstItemBase)
            {
                var item = new CONTENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Title = content.Title,
                    //Alias = content.Alias,
                    IntroText = content.IntroText,
                    //Contents = content.Contents,
                    Image = content.Image,
                    Thumbnail = content.Thumbnail,
                    Url = content.Url,
                    //CreatedBy = content.CreatedBy,
                    //CreatedDate = content.CreatedDate,
                    //CategoryPathway = content.CategoryPathway,
                    PublishDate = content.PublishDate,
                    Status = content.Status,
                    Type = content.Type,
                    Hits = content.Hits,
                    Params = content.Params,
                    SiteId = content.SiteId,
                    SiteUrl = content.SiteUrl,
                };

                lstItem.Add(item);
            }

            if (lstItem.Count > 0)
            {
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
            }

            return lstItem;
        }

        public List<CONTENT_FULL> GetTopLastestContentFulls(int top, int categoryId = 0,int site=0)
        {
            var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_TOP_LASTEST_CONTENTS + top + "_category" + categoryId + "_site" + site;

            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                //totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());
            }

            var lstItemBase = GetTopLastestContents(top, categoryId, site);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            var lstItem = new List<CONTENT_FULL>();

            foreach (var content in lstItemBase)
            {
                var item = new CONTENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Title = content.Title,
                    //Alias = content.Alias,
                    IntroText = content.IntroText,
                    //Contents = content.Contents,
                    Image = content.Image,
                    //Thumbnail = content.Thumbnail,
                    Url = content.Url,
                    //CreatedBy = content.CreatedBy,
                    //CreatedDate = content.CreatedDate,
                    //CategoryPathway = content.CategoryPathway,
                    PublishDate = content.PublishDate,
                    Status = content.Status,
                    Type = content.Type,
                    Hits = content.Hits,
                    Params = content.Params,
                    SiteId = content.SiteId,
                    SiteUrl = content.SiteUrl,
                };

                lstItem.Add(item);
            }

            if (lstItem.Count > 0)
            {
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
            }

            return lstItem;
        }
     
        public List<Statistic> GetReport(int categoryid, int year)
        {

            var data = ContentDBBase.Create().GetReport(categoryid, year);
            if (data == null)
                return null;
            var result = new List<Statistic>();
            var lstmonth = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            foreach (var m in lstmonth)
            {
                var item = data.Where(x => x.Month == m).FirstOrDefault();
                if (item != null)
                    result.Add(item);
                else
                    result.Add(new Statistic { Month = m, Number = 0 });

            }

            return result;
        }



        #endregion

        #region UPDATE

        //public void UpdateCache(CONTENT_FULL contentFull)
        //{
        //    var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_CONTENT + contentFull.Id;
        //    DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
        //    delegateUpdateCache.BeginInvoke(strKeyCached, contentFull, null, null);

        //}

        #endregion

        #region DELETE

        public int DeleteContents(string listIds)
        {
            var result = ContentDBBase.Create().DeleteContents(listIds);
            if (result != -1)
                FlushAllContentCache(strGroupKeyCached);
            return result;
        }

        public int DeleteContent(int id)
        {
            var result = ContentDBBase.Create().DeleteContent(id);
            if (result != -1)
                FlushAllContentCache(strGroupKeyCached);
            return result;
        }

        public void FlushAllContentCache(string containKey)
        {
            //DelegateFlushAllContentCache delegateFlushAllContentCache = LocalCaching.RemoveContainKeyInGroupKey;
            //delegateFlushAllContentCache.BeginInvoke(strGroupKeyCached, containKey, null, null);
            //LocalCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
            RedisCaching.RemoveGroup(containKey);
        }
        #endregion
    }
}
