using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BIZ.Entity;
using DATA;
using UTILS;
using Newtonsoft.Json;

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
                //FlushAllContentCache(string.Empty);
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
        public int ViewAdd(long id, int CategoryId)
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
        public Content GetContent(long contentId)
        {
            return ContentDBBase.Create().GetContent(contentId);
        }

        public List<MarkST> GetFiltertSTMark(string fromdate = "", string todate = "")
        {
            var data = ContentDBBase.Create().GetSTContentMark(string.Empty, 0, 1, "-1", fromdate, todate).ToList();
            return data;
        }
        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get content by id => add to local cache
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <returns></returns>
        public CONTENT_FULL GetContentFull(long contentId)
        {
            try
            {
                var strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_KEY_CONTENT + contentId;

                var cachedata = RedisCaching.GetData(strKeyCached);
                if (cachedata != null)
                    return JsonConvert.DeserializeObject<CONTENT_FULL>(cachedata.ToString());

                var content = GetContent(contentId);

                var item = new CONTENT_FULL
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Title = content.Title,
                    Alias = content.Alias,
                    Album =content.Album,
                    IntroText = content.IntroText,
                    Contents = content.Contents,
                    Image = content.Image,
                    Keywords = content.Keywords,
                    Url = content.Url,
                    CreatedBy = content.CreatedBy,
                    CreatedDate = content.CreatedDate,
                    CategoryPathway = content.CategoryPathway,
                    PublishDate = content.PublishDate,
                    Status = content.Status,
                    Type = content.Type,
                    Hits = content.Hits,
                    Thumbnail=content.Thumbnail,
                    CreatedRole = content.CreatedRole,
                    Params = content.Params,
                    IsHot = content.IsHot,
                    SiteId = content.SiteId,
                };

                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));

                return item;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "ContentBO", "GetContentFull ,contentId= " + contentId);
                return null;
            }
        }


        public List<MarkST> GetFiltertStMark(string fromdate = "", string todate = "")
        {
            var data = ContentDBBase.Create().GetSTContentMark(string.Empty, 0, 1, "-1", fromdate, todate).ToList();
            return data;
        }

        public List<CONTENT_FULL> GetFilterContentFullsPaged(int pageIndex, int pageSize, string title, int categoryId, List<int> lstcate, int status, string createdby, ref int totalRecords, int type = -1, string fromdate = "", string todate = "", string lststatus = "", string orderBy = "PublishDate DESC")
        {
            var scate = "";
            if (lstcate != null)
            {
                foreach (var item in lstcate)
                {
                    scate += item + "|";
                }
            }

            string strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_KEY_FILTER_CONTENTS + pageIndex + "_ps" + pageSize + "_cate" + categoryId + "_fd" + fromdate + "_td" + todate + "_t" + title + "_s" + status + "_cb" + createdby + "_lsts" + lststatus + "_type" + type + "_ob" + orderBy + "_scate" + scate;
            string strKeyCachedTotal = strGroupKeyCached + "_" + Constants.CACHE_KEY_FILTER_CONTENTS + pageIndex + "_ps" + pageSize + "_cate" + categoryId + "_fd" + fromdate + "_td" + todate + "_t" + title + "_s" + status + "_cb" + createdby + "_lsts" + lststatus + "_type" + type + "_ob" + orderBy + "_scate" + scate + "_total";

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //{
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            //    LocalCaching.AddToGroupKey(strKeyCachedTotal, strGroupKeyCached);
            //}
            //var data = LocalCaching.GetData(strKeyCached);
            //if (data != null)
            //{
            //    totalRecords = int.Parse((string)LocalCaching.GetData(strKeyCachedTotal));
            //    return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(data.ToString());
            //}
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());
            }
            var contents = ContentDBBase.Create().GetFilterContents(pageIndex, pageSize, title, categoryId, lstcate, status, createdby, ref totalRecords, fromdate, todate, lststatus, type, orderBy,0);
            if (contents == null)
                return null;
            var lstItem = new List<CONTENT_FULL>();
            var cateDic = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.News).ToDictionary(t => t.Id);
            foreach (var content in contents)
            {
                CONTENT_FULL contentFull = new CONTENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    CategoryName = content.CategoryId.HasValue ? cateDic[content.CategoryId.Value].Name : "",
                    Title = content.Title,
                    Alias = content.Alias,
                    IntroText = content.IntroText,
                    Album = content.Album,
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
                    IsHot = content.IsHot,
                    HitsAudio=content.HitsAudio,
                };
                //if (HttpContext.Current.Request.Url.ToString().Contains("noibo"))
                //{
                //    contentFull.Contents = content.Contents;
                //}
                lstItem.Add(contentFull);
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


        public List<CONTENT_FULL> GetFilterContentMarkPaged(int pageIndex, int pageSize, string title, int categoryId, int status, string createdby, ref int totalRecords, ref int totalMark, string fromdate = "", string todate = "", string orderBy = "PublishDate DESC")
        {


            string strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_KEY_FILTER_CONTENTS_MARK + pageIndex + "_ps" + pageSize + "_cate" + categoryId + "_fd" + fromdate + "_td" + todate + "_t" + title + "_s" + status + "_cb" + createdby + "_ob" + orderBy;
            string strKeyCachedTotal = strGroupKeyCached + "_" + Constants.CACHE_KEY_FILTER_CONTENTS_MARK + pageIndex + "_ps" + pageSize + "_cate" + categoryId + "_fd" + fromdate + "_td" + todate + "_t" + title + "_s" + status + "_cb" + createdby + "_lsts" + "_ob" + orderBy + "_total";
            string strKeyCachedTotalMark = strGroupKeyCached + "_" + Constants.CACHE_KEY_FILTER_CONTENTS_MARK + pageIndex + "_ps" + pageSize + "_cate" + categoryId + "_fd" + fromdate + "_td" + todate + "_t" + title + "_s" + status + "_cb" + createdby + "_lsts" + "_ob" + orderBy + "_totalmark";

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //{
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            //    LocalCaching.AddToGroupKey(strKeyCachedTotal, strGroupKeyCached);
            //    LocalCaching.AddToGroupKey(strKeyCachedTotalMark, strGroupKeyCached);
            //}
            //var data = LocalCaching.GetData(strKeyCached);
            //if (data != null)
            //{
            //    totalMark = int.Parse((string)LocalCaching.GetData(strKeyCachedTotalMark));
            //    totalRecords = int.Parse((string)LocalCaching.GetData(strKeyCachedTotal));
            //    return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(data.ToString());
            //}
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                totalMark = int.Parse(RedisCaching.GetData(strKeyCachedTotalMark).ToString());
                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());
            }
            var contents = ContentDBBase.Create().GetFilterContentsMark(pageIndex, pageSize, title, categoryId, status, createdby, ref totalRecords, ref totalMark, fromdate, todate, orderBy);
            if (contents == null)
                return null;
            var lstItem = new List<CONTENT_FULL>();
            var cateDic = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.News).ToDictionary(t => t.Id);
            foreach (var content in contents)
            {
                CONTENT_FULL contentFull = new CONTENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    CategoryName = content.CategoryId.HasValue ? cateDic[content.CategoryId.Value].Name : "",
                    Title = content.Title,
                    Alias = content.Alias,
                    IntroText = content.IntroText,
                    Album = content.Album,
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
                    Contents = content.Contents,
                };
                //if (HttpContext.Current.Request.Url.ToString().Contains("noibo"))
                //{
                //    contentFull.Contents = content.Contents;
                //}
                lstItem.Add(contentFull);
            }
            if (lstItem.Count > 0)
            {
                //LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                //LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                //LocalCaching.Add(strKeyCachedTotal, totalRecords.ToString());
                //LocalCaching.AddToGroupKey(strKeyCachedTotal, strGroupKeyCached);
                //LocalCaching.Add(strKeyCachedTotalMark, totalMark.ToString());
                //LocalCaching.AddToGroupKey(strKeyCachedTotalMark, strGroupKeyCached);
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                RedisCaching.Add(strKeyCachedTotal, totalRecords.ToString());
                RedisCaching.Add(strKeyCachedTotalMark, totalMark.ToString());
            }
            return lstItem;
        }
        public List<CONTENT_FULL> GetTopContentByChannelId(int channelId, int pageIndex, int pageSize, ref int totalRecords)
        {
            var lstdata = new Channel_DataBO().GetAllChannelDataFullsPaged(channelId, pageIndex, pageSize,
                                                                           ref totalRecords);
            if (lstdata == null)
                return null;
            string ids = "";
            foreach (var item in lstdata)
            {
                ids += item.ContentId + ",";
            }
            ids = ids.TrimEnd(',');
            return GetTopContentByIdsFulls(ids, 0, false);
        }
        public List<CONTENT_FULL> GetHotNews(int categoryId, int maxLastestNews)
        {
            var strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_HOTNEWS + categoryId + "_max" + maxLastestNews;

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);


            //var data = LocalCaching.GetData(strKeyCached);
            //if (data != null)
            //    return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(data.ToString());
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                //totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());
            }

            var lstItemBase = GetHotNewsPrivate(categoryId, maxLastestNews);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;
            if (lstItemBase.Count > 0)
            {


                //LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItemBase));
                //LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItemBase));
            }

            return lstItemBase;
        }
        public string GetFilterContentsPaged_JSON(int pageIndex, int pageSize, string title, int categoryId, int status, string createdby, string fromdate = "", string todate = "")
        {
            string keyCache = strGroupKeyCached + "_" + Constants.CACHE_KEY_ALL_CONTENTS_PAGED_JSON + pageIndex + pageSize + title + categoryId + status + createdby + fromdate + todate;
           
            var cachedata = RedisCaching.GetData(keyCache);
            if (cachedata != null)
                return cachedata.ToString();

            int totalRecords = 0;
            List<CONTENT_FULL> contents = GetFilterContentFullsPaged(pageIndex, pageSize, title, categoryId, null, status, createdby, ref totalRecords,-1, fromdate, todate);

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
        public string GetFilterContentsPaged_JSON2(int pageIndex, int pageSize, string title, int categoryId, int status, string createdby, string fromdate = "", string todate = "")
        {
            string keyCache = strGroupKeyCached + "_" + Constants.CACHE_KEY_ALL_CONTENTS_PAGED_JSON+"2" + pageIndex + pageSize + title + categoryId + status + createdby + fromdate + todate;

            var cachedata = RedisCaching.GetData(keyCache);
            if (cachedata != null)
                return cachedata.ToString();

            int totalRecords = 0;
            List<CONTENT_FULL> contents = GetTopLastestContentFulls(pageSize,-1,"",1).ToList();

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
            var contents = ContentDBBase.Create().GetFilterContents(pageIndex, pageSize, title, categoryId, lstcate, status, createdby, ref totalRecords, fromdate, todate, "", -1, orderBy);
            if (contents == null)
                return null;
            return contents;
        }
        public List<CONTENT_FULL> GetFocusNews(int categoryId, int maxLastestNews,string lang)
        {

            var lstdata = new ContentBO().GetTopLastestContentFulls(maxLastestNews, categoryId, lang,1).ToList();
            var configValue = new SystemConfigBO().GetByKey("FocusNews");
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
        private List<CONTENT_FULL> GetHotNewsPrivate(int categoryId, int maxLastestNews)
        {

            var lstdata = new ContentBO().GetTopLastestContentFulls(maxLastestNews, categoryId).ToList();
            var configValue = new SystemConfigBO().GetByKey("HotNews_" + categoryId);
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
        public List<CONTENT_FULL> GetTopContentByIdsFulls(string ids, int top, bool isArragne = false)
        {
            var strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_CONTENTS_BYIDS + top + "_lst" + ids + isArragne;
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                //totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());
            }
            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);


            //var data = LocalCaching.GetData(strKeyCached);
            //if (data != null)
            //    return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(data.ToString());
            //var cachedata = RedisCaching.GetData(strKeyCached);
            //if (cachedata != null)
            //{
            //    //totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
            //    return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());
            //}

            var lstItemBase = GetTopContentByIds(ids, top);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;
            var cateDic = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.News).ToDictionary(t => t.Id);
            var lstItem = new List<CONTENT_FULL>();
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
                                      //Thumbnail = content.Thumbnail,
                                      Url = content.Url,
                                      CategoryName = cateDic[content.CategoryId.Value].Name
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
                        Url = content.Url,
                        CategoryName = cateDic[content.CategoryId.Value].Name
                    };

                    lstItem.Add(item);
                }
            }


            if (lstItem.Count > 0)
            {
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));

                //LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                //LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            }

            return lstItem;
        }
        public List<CONTENT_FULL> GetPageContentFullsFrontend(int pageIndex, int pageSize, int categoryId, ref int totalRecords, string fromdate = "", string todate = "", string title = "", string lstNotId = "", string lang = "", int type = -1,int isHot=0)
        {
            string strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_CONTENTS_FRONTEND + pageIndex + "_pz" + pageSize + "_cate" + categoryId + "_fd" + fromdate + "_td" + todate + "_t" + title + "_ls" + lstNotId + "_lang" + lang + "_type" + type+ isHot;
            string strKeyCachedTotal = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_CONTENTS_FRONTEND + pageIndex + "_pz" + pageSize + "_cate" + categoryId + "_fd" + fromdate + "_td" + todate + "_t" + title + "_ls" + lstNotId + "_lang" + lang + "_type" + type + "_total"+ isHot;
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());
            }
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
            
            var lstItemBase = ContentDBBase.Create().GetFilterContentsFrontend(pageIndex, pageSize, title, categoryId, ref totalRecords, fromdate, todate, lstNotId, lang, type,isHot);

            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            var lstItem = new List<CONTENT_FULL>();
            var cateDic = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.News).ToDictionary(t => t.Id);
            foreach (var content in lstItemBase)
            {
                var item = new CONTENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    CategoryName = content.CategoryId.HasValue ? cateDic[content.CategoryId.Value].Name : "",
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
        /// <summary>
        /// Cuong pm cread
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryId"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<CONTENT_FULL> GetPageContentFulls(int pageIndex, int pageSize, int categoryId, ref int totalRecords, string fromdate = "", string todate = "", string title = "")
        {
            string strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_CONTENTS + pageIndex + "_pagesize" + pageSize + "_category" + categoryId + "_fromdate" + fromdate + "_todate" + todate + "_title" + title;
            string strKeyCachedTotal = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_CONTENTS + pageIndex + "_pagesize" + pageSize + "_category" + categoryId + "_fromdate" + fromdate + "_todate" + todate + "_title" + title + "_total";
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());
            }
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

            var lstItemBase = ContentDBBase.Create().GetFilterContents(pageIndex, pageSize, title, categoryId, null, 1, string.Empty, ref totalRecords, fromdate, todate);

            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            var lstItem = new List<CONTENT_FULL>();
            var cateDic = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.News).ToDictionary(t => t.Id);
            foreach (var content in lstItemBase)
            {
                var item = new CONTENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    CategoryName = content.CategoryId.HasValue ? cateDic[content.CategoryId.Value].Name : "",
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
        public List<CONTENT_FULL> GetTopViewContentFulls(int top, int categoryId = 0, string fromdate = "", string todate = "",string lang="vi-vn")
        {
            var strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_VIEW_CONTENTS + top + "_category" + categoryId + "_fromdate" + fromdate + "_todate" + todate + "_lang" + lang;

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            //var data = LocalCaching.GetData(strKeyCached);
            //if (data != null)
            //    return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(data.ToString());
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                //totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());
            }

            var lstItemBase = GetTopViewContents(top, categoryId, fromdate, todate, lang);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            var lstItem = new List<CONTENT_FULL>();
            var cateDic = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.News).ToDictionary(t => t.Id);
            foreach (var content in lstItemBase)
            {
                try
                {
                    var item = new CONTENT_FULL()
                            {
                                Id = content.Id,
                                CategoryId = content.CategoryId,
                                CategoryName = content.CategoryId.HasValue ? cateDic[content.CategoryId.Value].Name : "",
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
                                // Status = content.Status,
                                Type = content.Type,
                                Hits = content.Hits,
                                Params = content.Params,
                            };

                    lstItem.Add(item);
                }
                catch 
                {

                }
            }

            if (lstItem.Count > 0)
            {
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                //RedisCaching.Add(strKeyCachedTotal, totalRecords.ToString());
            }

            return lstItem;
        }
        public List<CONTENT_FULL> GetTopLastestContentFulls(int top, int categoryId = 0, string lang = "",int ishot=0,string title="")
        {
            var strKeyCached = strGroupKeyCached + "_" + Constants.CACHE_KEY_TOP_LASTEST_CONTENTS + top + "_category" + categoryId + "_lang" + lang+ ishot + "_title" + title;

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null)
            //    LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            //var data = LocalCaching.GetData(strKeyCached);
            //if (data != null)
            //    return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(data.ToString());
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
            {
                //totalRecords = int.Parse(RedisCaching.GetData(strKeyCachedTotal).ToString());
                return JsonConvert.DeserializeObject<List<CONTENT_FULL>>(cachedata.ToString());
            }

            var lstItemBase = GetTopLastestContents(top, categoryId, lang, ishot, title);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            var lstItem = new List<CONTENT_FULL>();
            var cateDic = new CategoryBO().GetAllCategoriesFull(UTILS.Constants.CategoryType.News).ToDictionary(t => t.Id);
            foreach (var content in lstItemBase)
            {
                var item = new CONTENT_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    CategoryName = content.CategoryId.HasValue ? cateDic[content.CategoryId.Value].Name : "",
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
                    //Status = content.Status,
                    Type = content.Type,
                    Hits = content.Hits,
                    Params = content.Params,
                };

                lstItem.Add(item);
            }

            if (lstItem.Count > 0)
            {
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                //RedisCaching.Add(strKeyCachedTotal, totalRecords.ToString());
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


        private IEnumerable<Content> GetTopLastestContents(int top, int categoryId, string lang = "",int isHot=0, string title = "")
        {
            var result = ContentDBBase.Create().GetTopLastestContents(top, categoryId, lang, isHot, title);
            if (result == null)
                return null;
            return result;
        }
        private IEnumerable<Content> GetTopViewContents(int top, int categoryId, string fromdate = "", string todate = "",string lang="")
        {
            var result = ContentDBBase.Create().GetTopViewContents(top, categoryId, fromdate, todate,lang);
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


        #endregion

        #region UPDATE

        //public void UpdateCache(CONTENT_FULL contentFull)
        //{
        //    var strKeyCached = Constants.CACHE_KEY_CONTENT + contentFull.Id;
        //    DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
        //    delegateUpdateCache.BeginInvoke(strKeyCached, JsonConvert.SerializeObject(contentFull), null, null);

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
