using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using AlbumImage = DATA.AlbumImage;
namespace BIZ
{
    public class AlbumImageBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_ALBUM_IMAGE;

        protected delegate void DelegateFlushAllAlbumCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE


        public int CreateUpdateAlbum(AlbumImage obj)
        {

            int returnVal = AlbumImageDBBase.Create().CreateUpdateAlbumImage(obj);
            if (returnVal != -1)
            {
                UpdateCache(obj);
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
        //    return AlbumImageDBBase.Create().GetAlbum(AlbumId);
        //}

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get Album by id => add to local cache
        /// </summary>
        /// <param name="AlbumId">The Album id.</param>
        /// <returns></returns>
        public AlbumImage_FULL GetAlbum(int AlbumId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_ALBUM_IMAGE + AlbumId;

                var item = (AlbumImage_FULL)LocalCaching.GetData(strKeyCached);
                if (item != null)
                    return item;

                var itemBase = AlbumImageDBBase.Create().GetAlbum(AlbumId);

                item = new AlbumImage_FULL()
               {
                   Id = itemBase.Id,
                   CategoryId = itemBase.CategoryId,
                   Name = itemBase.Name,
                   Author = itemBase.Author,
                   Description = itemBase.Description,
                   Image = itemBase.Image,
                   PublishDate = itemBase.PublishDate,
                   Status = itemBase.Status,
                   TotalVote = itemBase.TotalVote,
                   Point = itemBase.Point,
                   Type = itemBase.Type,
                   Code = itemBase.Code,
                   Param = itemBase.Param,
                   GroupName = itemBase.GroupName,
               };
                LocalCaching.Add(strKeyCached, item);

                return item;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "AlbumImageBO", "GetAlbum");
                return null;
            }
        }

        private IEnumerable<AlbumImage> GetTopLastestAlbums(int top, int categoryId)
        {
            var result = AlbumImageDBBase.Create().GetTopLastestAlbums(top, categoryId);
            if (result == null)
                return null;
            return result;
        }
        private IEnumerable<AlbumImage> GetTopAlbumByIds(string ids, int top)
        {
            var result = AlbumImageDBBase.Create().GetTopAlbumByIds(ids, top);
            if (result == null)
                return null;
            return result;
        }
        public List<AlbumImage_FULL> GetTopAlbumByIdsFulls(string ids, int top, bool isArragne = false)
        {
            var strKeyCached = Constants.CACHE_KEY_TOP_ALBUM_IMAGE_BYIDS + top + "_lst" + ids + isArragne;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            var lstItem = (List<AlbumImage_FULL>)LocalCaching.GetData(strKeyCached);

            if (lstItem != null && lstItem.Count > 0)
                return lstItem;

            var lstItemBase = GetTopAlbumByIds(ids, top);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            lstItem = new List<AlbumImage_FULL>();
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
                                var item = new AlbumImage_FULL()
                                {
                                    Id = content.Id,
                                    CategoryId = content.CategoryId,
                                    Name = content.Name,
                                    Author = content.Author,
                                    Description = content.Description,
                                    Image = content.Image,
                                    PublishDate = content.PublishDate,
                                    Status = content.Status,
                                    TotalVote = content.TotalVote,
                                    Point = content.Point,
                                    Type = content.Type,
                                    Code = content.Code,
                                    Param = content.Param,
                                    GroupName = content.GroupName,
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
                    var item = new AlbumImage_FULL()
                    {
                        Id = content.Id,
                        CategoryId = content.CategoryId,
                        Name = content.Name,
                        Author = content.Author,
                        Description = content.Description,
                        Image = content.Image,
                        PublishDate = content.PublishDate,
                        Status = content.Status,
                        TotalVote = content.TotalVote,
                        Point = content.Point,
                        Type = content.Type,
                        Code = content.Code,
                        Param = content.Param,
                        GroupName = content.GroupName,
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
        public List<AlbumImage_FULL> GetTopLastestAlbumsFull(int top, int categoryId = 0)
        {
            var strKeyCached = Constants.CACHE_KEY_TOP_LASTEST_ALBUM_IMAGE + top + "_category" + categoryId;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            var lstItem = (List<AlbumImage_FULL>)LocalCaching.GetData(strKeyCached);

            if (lstItem != null && lstItem.Count > 0)
                return lstItem;

            var lstItemBase = GetTopLastestAlbums(top, categoryId);
            if (lstItemBase == null || lstItemBase.Count() == 0)
                return null;

            lstItem = new List<AlbumImage_FULL>();

            foreach (var content in lstItemBase)
            {
                var item = new AlbumImage_FULL()
                {
                    Id = content.Id,
                    CategoryId = content.CategoryId,
                    Name = content.Name,
                    Author = content.Author,
                    Description = content.Description,
                    Image = content.Image,
                    PublishDate = content.PublishDate,
                    Status = content.Status,
                    TotalVote = content.TotalVote,
                    Point = content.Point,
                    Type = content.Type,
                    Code = content.Code,
                    Param = content.Param,
                    GroupName = content.GroupName,
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

        public List<AlbumImage> GetAlbumsPaged(string title, int categoryId, int status, int type, int pageIndex, int pageSize, ref int totalRecords, string fromdate = "", string todate = "", string orderBy = "PublishDate DESC")
        {
            var Albums = AlbumImageDBBase.Create().GetAlbumsByFilter(title, categoryId, status, type, pageIndex, pageSize, ref totalRecords, fromdate, todate, orderBy);
            if (Albums == null)
                return null;

            return Albums.ToList();
        }
        public List<AlbumImage_FULL> GetAlbumsFuLLPaged(string title, int categoryId, int status, int type, int pageIndex, int pageSize, ref int totalRecords, string fromdate = "", string todate = "", string orderBy = "PublishDate DESC")
        {
            var Albums = GetAlbumsPaged(title, categoryId, status, type, pageIndex, pageSize, ref totalRecords, fromdate, todate, orderBy);

            if (Albums == null)
                return null;
            List<AlbumImage_FULL> albumFulls = new List<AlbumImage_FULL>();
            foreach (var item in Albums)
            {
                AlbumImage_FULL AlbumsFull = new AlbumImage_FULL()
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    CategoryPathway = item.CategoryPathway,
                    Author = item.Author,
                    Description = item.Description,
                    TotalVote = item.TotalVote,
                    Point = item.Point,
                    PublishDate = item.PublishDate,
                    Status = item.Status,
                    Name = item.Name,
                    Image = item.Image,
                    Type = item.Type,
                    Code = item.Code,
                    Param = item.Param,
                    GroupName = item.GroupName,

                };

                albumFulls.Add(AlbumsFull);
            }

            return albumFulls.ToList();
        }
        public List<AlbumImage_FULL> GetPageLastestAlbumsFull(string keyword, int categoryId, int status, int type, int pageIndex, int pageSize, ref int totalRecords, string fromdate = "", string todate = "", string orderBy = "PublishDate DESC")
        {

            string strKeyCached = Constants.CACHE_KEY_TOP_LASTEST_ALBUM_IMAGE + pageIndex + "_pagesize" + pageSize + "_category" + categoryId + "_orderBy" + orderBy + "_type" + type + "_stt" + status + fromdate + todate;
            string strKeyCachedTotal = Constants.CACHE_KEY_TOP_LASTEST_ALBUM_IMAGE + pageIndex + "_pagesize" + pageSize + "_category" + categoryId + "_type" + type + "_stt" + status + fromdate + todate + "total";

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
            {
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
                LocalCaching.AddToGroupKey(strKeyCachedTotal, strGroupKeyCached);
            }
            var lstItem = (List<AlbumImage_FULL>)LocalCaching.GetData(strKeyCached);
            //var lstItem = new List<CATEGORY_FULL> ();
            if (lstItem != null && lstItem.Count > 0)
            {
                totalRecords = int.Parse((string)LocalCaching.GetData(strKeyCachedTotal));
                return lstItem;
            }
            lstItem = new List<AlbumImage_FULL>();
            var Albums = GetAlbumsPaged(keyword, categoryId, status, type, pageIndex, pageSize, ref totalRecords, fromdate, todate, orderBy);
            if (Albums == null)
                return null;

            foreach (var item in Albums)
            {
                AlbumImage_FULL AlbumsFull = new AlbumImage_FULL()
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    CategoryPathway = item.CategoryPathway,
                    Author = item.Author,
                    Description = item.Description,
                    TotalVote = item.TotalVote,
                    Point = item.Point,
                    PublishDate = item.PublishDate,
                    Status = item.Status,
                    Name = item.Name,
                    Code = item.Code,
                    GroupName = item.GroupName,
                    Param = item.Param,
                    Image = item.Image,
                    Type = item.Type


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
        public string GetAlbumsPaged_JSON(string title, int categoryId, int status, int type, int pageIndex, int pageSize, string fromdate = "", string todate = "", string orderBy = "PublishDate DESC")
        {
            string keyCache = Constants.CACHE_KEY_ALBUMS_IMAGE_PAGED_JSON + "_pageindex" + pageIndex + "_pagesize" + pageSize + "_title" + title + "_categoryId" + categoryId + "_orderBy" + orderBy + "_status" + status + "_type" + type + "_fromdate" + fromdate + "_todate" + todate;
            string groupKeyCache = Constants.CACHE_GROUPKEY_ALBUM_IMAGE;

            var listGroupKey = (List<string>)LocalCaching.GetData(groupKeyCache);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, groupKeyCache);

            var json = (string)LocalCaching.GetData(keyCache);
            if (!string.IsNullOrEmpty(json))
                return json;

            int totalRecords = 0;
            List<AlbumImage_FULL> Albums = GetAlbumsFuLLPaged(title, categoryId, status, type, pageIndex, pageSize, ref totalRecords,fromdate,todate,orderBy);

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

        public void UpdateCache(AlbumImage obj)
        {
            var strKeyCached = Constants.CACHE_KEY_ALBUM_IMAGE + obj.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, obj, null, null);

        }

        #endregion

        #region DELETE

        public int DeleteAlbums(string listIds)
        {
            var returnVal = AlbumImageDBBase.Create().DeleteAlbums(listIds);
            if (returnVal != -1)
                FlushAllAlbumCache(string.Empty);
            return returnVal;
        }
        public int Vote(long albumId,int point)
        {
            var returnVal = AlbumImageDBBase.Create().Vote(albumId,point);
            if (returnVal != -1)
                FlushAllAlbumCache(string.Empty);
            return returnVal;
        }
        public int DeleteAlbum(int id)
        {
            var returnVal = AlbumImageDBBase.Create().DeleteAlbum(id);
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
