using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Album = DATA.Album;
namespace BIZ
{
    public class AlbumBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_ALBUM;

        protected delegate void DelegateFlushAllAlbumCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE


        public int CreateUpdateAlbum(Album Album)
        {

            int returnVal = AlbumDBBase.Create().CreateUpdateAlbum(Album);
            if (returnVal != -1)
            {
                UpdateCache(Album);
                FlushAllAlbumCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get Album by Album id
        /// </summary>
        /// <param name="AlbumId">The Album id.</param>
        /// <returns></returns>
        //public Album GetAlbum(int AlbumId)
        //{
        //    return AlbumDBBase.Create().GetAlbum(AlbumId);
        //}

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get Album by id => add to local cache
        /// </summary>
        /// <param name="AlbumId">The Album id.</param>
        /// <returns></returns>
        public Album_FULL GetAlbum(int AlbumId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_ALBUM + AlbumId;

                var item = (Album_FULL)LocalCaching.GetData(strKeyCached);
                if (item != null)
                    return item;

                var itemBase = AlbumDBBase.Create().GetAlbum(AlbumId);

                item = new Album_FULL()
               {
                   Id = itemBase.Id,
                   CategoryId = itemBase.CategoryId,
                   Title = itemBase.Title,
                   Description = itemBase.Description,
                   Images = itemBase.Images,
                   PublishDate = itemBase.PublishDate,
                   Status = itemBase.Status,
                   Hits = itemBase.Hits,
                    CreatedBy = itemBase.CreatedBy,
                    Param = itemBase.Param,
                  
                };
                LocalCaching.Add(strKeyCached, item);

                return item;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }

        private IEnumerable<Album> GetTopLastestAlbums(int top, int categoryId)
        {
            var result = AlbumDBBase.Create().GetTopLastestAlbums(top, categoryId);
            if (result == null)
                return null;
            return result;
        }
        private IEnumerable<Album> GetTopAlbumByIds(string ids, int top)
        {
            var result = AlbumDBBase.Create().GetTopAlbumByIds(ids, top);
            if (result == null)
                return null;
            return result;
        }
        public List<Album_FULL> GetTopAlbumByIdsFulls(string ids, int top, bool isArragne = false)
        {
            var strKeyCached = Constants.CACHE_KEY_TOP_ALBUMS_BYIDS + top + "_lst" + ids + isArragne;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            var lstItem = (List<Album_FULL>)LocalCaching.GetData(strKeyCached);

            if (lstItem != null && lstItem.Count > 0)
                return lstItem;

            var lstItemBase = GetTopAlbumByIds(ids, top);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            lstItem = new List<Album_FULL>();
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
                                var item = new Album_FULL()
                                {
                                    Id = content.Id,
                                    CategoryId = content.CategoryId,
                                    Title = content.Title,
                                    CreatedBy = content.CreatedBy,
                                    Description = content.Description,
                                    Images = content.Images,
                                    PublishDate = content.PublishDate,
                                    Status = content.Status,
                                    Hits = content.Hits
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
                    var item = new Album_FULL()
                    {
                        Id = content.Id,
                        CategoryId = content.CategoryId,
                        Title = content.Title,
                        CreatedBy = content.CreatedBy,
                        Description = content.Description,
                        Images = content.Images,
                        PublishDate = content.PublishDate,
                        Status = content.Status,
                        Hits = content.Hits
                    };

                    lstItem.Add(item);
                }
            }


            if (lstItem.Count > 0)
            {


                LocalCaching.Add(strKeyCached, lstItem);
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            }

            return lstItem;
        }
        public List<Album_FULL> GetTopLastestAlbumsFull(int top, int categoryId = 0)
        {
            var strKeyCached = Constants.CACHE_KEY_TOP_LASTEST_ALBUMS + top + "_category" + categoryId;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            var lstItem = (List<Album_FULL>)LocalCaching.GetData(strKeyCached);

            if (lstItem != null && lstItem.Count > 0)
                return lstItem;

            var lstItemBase = GetTopLastestAlbums(top, categoryId);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            lstItem = new List<Album_FULL>();

            foreach (var content in lstItemBase)
            {
                var item = new Album_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Title = content.Title,
                    CreatedBy = content.CreatedBy,
                    Description = content.Description,
                    Images = content.Images,
                    PublishDate = content.PublishDate,
                    Status = content.Status,
                    Hits = content.Hits,
                    Param = content.Param

                };

                lstItem.Add(item);
            }

            if (lstItem.Count > 0)
            {
                LocalCaching.Add(strKeyCached, lstItem);
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            }

            return lstItem;
        }

        public List<Album> GetAlbumsPaged(string title, int categoryId, int status, int pageIndex, int pageSize, ref int totalRecords)
        {
            var Albums = AlbumDBBase.Create().GetAlbumsByFilter(title, categoryId, status, pageIndex, pageSize, ref totalRecords);
            if (Albums == null)
                return null;

            return Albums.ToList();
        }
        public List<Album_FULL> GetAlbumsFuLLPaged(string title, int categoryId, int status, int pageIndex, int pageSize, ref int totalRecords)
        {
            var Albums = GetAlbumsPaged(title, categoryId, status, pageIndex, pageSize, ref totalRecords);

            if (Albums == null)
                return null;
            List<Album_FULL> albumFulls = new List<Album_FULL>();
            foreach (var item in Albums)
            {
                Album_FULL AlbumsFull = new Album_FULL()
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    CategoryPathway = item.CategoryPathway,
                    CreatedBy = item.CreatedBy,
                    Description = item.Description,
                    Hits = item.Hits,
                    PublishDate = item.PublishDate,
                    Status = item.Status,
                    Title = item.Title,
                    Images = item.Images,
                    Style = item.Style


                };

                albumFulls.Add(AlbumsFull);
            }

            return albumFulls.ToList();
        }
        public List<Album_FULL> GetPageLastestAlbumsFull(int categoryId, int pageIndex, int pageSize, ref int totalRecords)
        {

            string strKeyCached = Constants.CACHE_KEY_TOP_LASTEST_ALBUMS + pageIndex + "_pagesize" + pageSize + "_category" + categoryId;
            string strKeyCachedTotal = Constants.CACHE_KEY_TOP_LASTEST_ALBUMS + pageIndex + "_pagesize" + pageSize + "_category" + categoryId + "total";

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
            {
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                LocalCaching.AddToGroupKey(strKeyCachedTotal, strGroupKeyCached);
            }
            var lstItem = (List<Album_FULL>)LocalCaching.GetData(strKeyCached);
            //var lstItem = new List<CATEGORY_FULL> ();
            if (lstItem != null && lstItem.Count > 0)
            {
                totalRecords = int.Parse((string)LocalCaching.GetData(strKeyCachedTotal));
                return lstItem;
            }
            lstItem = new List<Album_FULL>();
            var Albums = GetAlbumsPaged(String.Empty, categoryId, 1, pageIndex, pageSize, ref totalRecords);
            if (Albums == null)
                return null;

            foreach (var item in Albums)
            {
                Album_FULL AlbumsFull = new Album_FULL()
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    CategoryPathway = item.CategoryPathway,
                    CreatedBy = item.CreatedBy,
                    Description = item.Description,
                    Hits = item.Hits,
                    PublishDate = item.PublishDate,
                    Status = item.Status,
                    Title = item.Title,
                    Images = item.Images,
                    Style = item.Style


                };

                lstItem.Add(AlbumsFull);
            }

            return lstItem.ToList();
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 23/08/2011 05:01 PM
        /// todo: get list of Albums have paged and in json format
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public string GetAlbumsPaged_JSON(string title, int categoryId, int status, int pageIndex, int pageSize)
        {
            string keyCache = Constants.CACHE_KEY_ALBUMS_PAGED_JSON + "_pageindex" + pageIndex + "_pagesize" + pageSize + "_title" + title + "_categoryId" + categoryId + "_status" + status;
            string groupKeyCache = Constants.CACHE_GROUPKEY_ALBUM;

            var listGroupKey = (List<string>)LocalCaching.GetData(groupKeyCache);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, groupKeyCache);

            var json = (string)LocalCaching.GetData(keyCache);
            if (!string.IsNullOrEmpty(json))
                return json;

            int totalRecords = 0;
            List<Album_FULL> Albums = GetAlbumsFuLLPaged(title, categoryId, status, pageIndex, pageSize, ref totalRecords);

            if (Albums == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(Albums, string.Empty)).Append("}");

            json = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(json))
            {
                LocalCaching.Add(keyCache, json);
                LocalCaching.AddToGroupKey(keyCache, groupKeyCache);
            }

            return json;
        }








        #endregion

        #region UPDATE

        public void UpdateCache(Album Album)
        {
            var strKeyCached = Constants.CACHE_KEY_ALBUM + Album.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, Album, null, null);

        }

        #endregion

        #region DELETE

        public int DeleteAlbums(string listIds)
        {
            var returnVal = AlbumDBBase.Create().DeleteAlbums(listIds);
            if (returnVal != -1)
                FlushAllAlbumCache(string.Empty);
            return returnVal;
        }

        public int DeleteAlbum(int id)
        {
            var returnVal = AlbumDBBase.Create().DeleteAlbum(id);
            if (returnVal != -1)
                FlushAllAlbumCache(string.Empty);
            return returnVal;
        }

        #endregion

        public void FlushAllAlbumCache(string containKey)
        {
            DelegateFlushAllAlbumCache delegateFlushAllAlbumCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllAlbumCache.BeginInvoke(strGroupKeyCached, containKey, null, null);


        }

    }
}
