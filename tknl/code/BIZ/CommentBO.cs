using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Comment = DATA.Comment;
using Newtonsoft.Json;
namespace BIZ
{
    public class CommentBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_COMMENT;

        protected delegate void DelegateFlushAllCommentCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);

        #region CREATE


        public int CreateUpdateComment(Comment Comment)
        {

            int returnVal = CommentDBBase.Create().CreateUpdateComment(Comment);
            if (returnVal != -1)
            {
                UpdateCache(Comment);
                FlushAllCommentCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 11:40 AM
        /// todo: get Comment by Comment id
        /// </summary>
        /// <param name="CommentId">The Comment id.</param>
        /// <returns></returns>
        //public Comment GetComment(int CommentId)
        //{
        //    return CommentDBBase.Create().GetComment(CommentId);
        //}

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get Comment by id => add to local cache
        /// </summary>
        /// <param name="CommentId">The Comment id.</param>
        /// <returns></returns>
        public Comment GetComment(long CommentId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_Comment + CommentId;

                var data = LocalCaching.GetData(strKeyCached);
                if (data != null)
                    return JsonConvert.DeserializeObject<Comment>(data.ToString());

                var item = CommentDBBase.Create().GetComment(CommentId);

                LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));

                return item;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e, "CommentBO", "GetComment");
                return null;
            }
        }

        public List<Comment> GetTopLastestComments(int top, int type = -1, long itemid = -1, int status = -1)
        {
            var strKeyCached = Constants.CACHE_KEY_TOP_LASTEST_Comments + top + "_type" + type + "_itemid" + itemid + "_status" + status;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            var data = LocalCaching.GetData(strKeyCached);
            if (data != null)
                return JsonConvert.DeserializeObject<List<Comment>>(data.ToString());


            var lstItemBase = CommentDBBase.Create().GetTopLastestComments(top, type, itemid, status).OrderBy(x => x.CreatedTime).ToList();
            if (lstItemBase.Count > 0)
            {
                LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItemBase));
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            }

            return lstItemBase;
        }
        public List<Comment> GetCommentsPaged(string title, int type, long itemid, int status, int pageIndex, int pageSize, ref int Total)
        {
            string strKeyCached = Constants.CACHE_KEY_CommentS_PAGED_JSON + pageIndex + "_" + pageSize + "_" + title + "_" + type + "_" + itemid + "_" + status;
            string strKeyCachedTotal = Constants.CACHE_KEY_CommentS_PAGED_JSON + pageIndex + "_" + pageSize + "_" + title + "_" + type + "_" + itemid + "_" + status + "_total";
            string groupKeyCache = Constants.CACHE_GROUPKEY_COMMENT;

            var listGroupKey = (List<string>)LocalCaching.GetData(groupKeyCache);
            if (listGroupKey == null || listGroupKey.IndexOf(strKeyCached) == -1)
            {
                LocalCaching.AddToGroupKey(strKeyCached, groupKeyCache);
                LocalCaching.AddToGroupKey(strKeyCachedTotal, groupKeyCache);
            }
            var data = LocalCaching.GetData(strKeyCached);
            if (data != null)
            {
                Total = int.Parse((string)LocalCaching.GetData(strKeyCachedTotal));
                return JsonConvert.DeserializeObject<List<Comment>>(data.ToString());
            }

            //int totalRecords = 0;
            var lstItem = CommentDBBase.Create().GetCommentsByFilter(title, type, itemid, status, pageIndex, pageSize, ref Total).ToList();

            if (lstItem == null)
                return null;
            if (lstItem.Count > 0)
            {
                LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                LocalCaching.Add(strKeyCachedTotal, Total.ToString());
                LocalCaching.AddToGroupKey(strKeyCached, groupKeyCache);
                LocalCaching.AddToGroupKey(strKeyCachedTotal, groupKeyCache);
            }


            return lstItem;
        }
        public string GetCommentsPaged_JSON(string title, int type, long itemid, int status, int pageIndex, int pageSize)
        {
            string keyCache = Constants.CACHE_KEY_CommentS_PAGED_JSON + pageIndex + "_" + pageSize + "_" + title + "_" + type + "_" + itemid + "_" + status;
            string groupKeyCache = Constants.CACHE_GROUPKEY_COMMENT;

            var listGroupKey = (List<string>)LocalCaching.GetData(groupKeyCache);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, groupKeyCache);

            var json = (string)LocalCaching.GetData(keyCache);
            if (!string.IsNullOrEmpty(json))
                return json;

            int totalRecords = 0;
            List<Comment> Comments = CommentDBBase.Create().GetCommentsByFilter(title, type, itemid, status, pageIndex, pageSize, ref totalRecords).ToList();

            if (Comments == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(Comments, string.Empty)).Append("}");

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

        public void UpdateCache(Comment comment)
        {
            var strKeyCached = Constants.CACHE_KEY_Comment + comment.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, JsonConvert.SerializeObject(comment), null, null);

        }

        #endregion

        #region DELETE

        public int DeleteComments(string listIds)
        {
            var returnVal = CommentDBBase.Create().DeleteComments(listIds);
            if (returnVal != -1)
                FlushAllCommentCache(string.Empty);
            return returnVal;
        }
        public int PublishedComments(string listIds)
        {
            var returnVal = CommentDBBase.Create().PublishedComments(listIds);
            if (returnVal != -1)
                FlushAllCommentCache(string.Empty);
            return returnVal;
        }
        public int UpdateComments(long Id,int Status)
        {
            var returnVal = CommentDBBase.Create().UpdateComment(Id, Status);
            if (returnVal != -1)
                FlushAllCommentCache(string.Empty);
            return returnVal;
        }
        public int DeleteComment(long id)
        {
            var returnVal = CommentDBBase.Create().DeleteComment(id);
            if (returnVal != -1)
                FlushAllCommentCache(string.Empty);
            return returnVal;
        }

        #endregion

        public void FlushAllCommentCache(string containKey)
        {
            DelegateFlushAllCommentCache delegateFlushAllCommentCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllCommentCache.BeginInvoke(strGroupKeyCached, containKey, null, null);


        }

    }
}
