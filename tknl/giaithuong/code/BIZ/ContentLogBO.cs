using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace BIZ
{
    public class ContentLogBO
    {
        protected delegate void DelegateUpdateCache(string key, object data);
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_CONTENTLOG;

        protected delegate void DelegateFlushAllCache(string strGroupKeyCached, string containKey);

        #region CREATE


        public int CreateUpdateContentLog(ContentLog ContentLog)
        {

            int returnVal = ContentLogDBBase.Create().CreateUpdateContentLog(ContentLog);
            if (returnVal != -1)
            {
                UpdateCache(ContentLog);
                FlushAllCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ

        public ContentLog GetById(long id)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_ALL_CONTENTLOG_BYID + id;

                var item = (ContentLog)LocalCaching.GetData(strKeyCached);
                if (item != null)
                    return item;

                var obj = ContentLogDBBase.Create().GetById(id);

                LocalCaching.Add(strKeyCached, obj);

                return obj;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
        }
        public List<ContentLog> GetByFilter(string UserName, int itemtType, long itemid, string itemName, int pageIndex, int pageSize, ref int totalRecords,string fromdate= "", string todate = "")
        {
            return ContentLogDBBase.Create().GetByFilter( UserName, itemtType,  itemid,  itemName,  pageIndex,  pageSize, ref  totalRecords, fromdate,todate).ToList();
        }
        public List<ContentLog> GetContentLogsByContentId(long contentId,int Type)
        {
            string keyCache = Constants.CACHE_KEY_ALL_CONTENTLOG_BYCONTENT + contentId + "_t" + Type;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_ContentLog;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var lstCachedContentLogs = (List<ContentLog>)LocalCaching.GetData(keyCache);

            if (lstCachedContentLogs != null)
                return lstCachedContentLogs;

            var ContentLogs = ContentLogDBBase.Create().GetContentLog(contentId, Type);

            if (ContentLogs == null)
                return null;
            var lstitem = new List<ContentLog>();
            foreach (var item in ContentLogs)
            {

                var content = new ContentLog
                {

                    Id = item.Id,
                    CreateTime = item.CreateTime,
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    Type = item.Type,
                    UserName = item.UserName,
                    Note = item.Note,
                    
                };
                lstitem.Add(content);
            }


            if (lstitem.Count > 0)
            {
                LocalCaching.Add(keyCache, lstitem);
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }

            return lstitem;
        }


        #region UPDATE

        public void UpdateCache(ContentLog Contact)
        {
            var strKeyCached = Constants.CACHE_GROUPKEY_CONTENTLOG + Contact.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, Contact, null, null);

        }

        #endregion

        #region DELETE

       

        public void FlushAllCache(string containKey)
        {
            DelegateFlushAllCache delegateFlushAllCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllCache.BeginInvoke(strGroupKeyCached, containKey, null, null);
            //LocalCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
        }

        #endregion

        #endregion

      

    }
}
