using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using UTILS;
using Comment = DATA.Comment;
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
               // UpdateCache(Comment);
                FlushAllCommentCache(strGroupKeyCached);
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
        public Comment GetComment(int CommentId)
        {
            try
            {
                var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_Comment + CommentId;

                var cachedata = RedisCaching.GetData(strKeyCached);
                if (cachedata != null)
                    return JsonConvert.DeserializeObject<Comment>(cachedata.ToString());

                var item = CommentDBBase.Create().GetComment(CommentId);

                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(item));

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
            var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_TOP_LASTEST_Comments + top + "_type" + type + "_itemid" + itemid + "_status" + status;


            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
                return JsonConvert.DeserializeObject<List<Comment>>(cachedata.ToString());


            var lstItemBase = CommentDBBase.Create().GetTopLastestComments(top, type, itemid, status).OrderBy(x=>x.CreatedTime).ToList();
            if (lstItemBase.Count > 0)
            {
                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItemBase));
            }

            return lstItemBase;
        }
        public string GetCommentsPaged_JSON(string title, int type ,long itemid, int status, int pageIndex, int pageSize)
        {
            string keyCache = Constants.CACHE_KEY_CommentS_PAGED_JSON + pageIndex+"_"+ pageSize+"_"+title+"_"+type+"_"+itemid+"_"+status;
            var cachedata = RedisCaching.GetData(keyCache);
            if (cachedata != null)
                return cachedata.ToString();

            int totalRecords = 0;
            List<Comment> Comments = CommentDBBase.Create().GetCommentsByFilter(title, type,itemid, status, pageIndex, pageSize, ref totalRecords).ToList();

            if (Comments == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{Total:").Append(totalRecords).Append(",Items:").Append(Utils.ConvertToJson(Comments, string.Empty)).Append("}");

            var json = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(json))
            {
                RedisCaching.Add(keyCache, json.ToString());
            }

            return json;
        }
       








        #endregion

        #region UPDATE

        //public void UpdateCache(Comment Comment)
        //{
        //    var strKeyCached = strGroupKeyCached+"_"+Constants.CACHE_KEY_Comment + Comment.Id;
        //    DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
        //    delegateUpdateCache.BeginInvoke(strKeyCached, Comment, null, null);

        //}

        #endregion

        #region DELETE

        public int DeleteComments(string listIds)
        {
            var returnVal = CommentDBBase.Create().DeleteComments(listIds);
            if (returnVal != -1)
                FlushAllCommentCache(strGroupKeyCached);
            return returnVal;
        }
        public int PublishedComments(string listIds)
        {
            var returnVal = CommentDBBase.Create().PublishedComments(listIds);
            if (returnVal != -1)
                FlushAllCommentCache(strGroupKeyCached);
            return returnVal;
        }

        public int DeleteComment(int id)
        {
            var returnVal = CommentDBBase.Create().DeleteComment(id);
            if (returnVal != -1)
                FlushAllCommentCache(strGroupKeyCached);
            return returnVal;
        }

        #endregion

        public void FlushAllCommentCache(string containKey)
        {
            RedisCaching.RemoveGroup(containKey);


        }

    }
}
