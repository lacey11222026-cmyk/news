using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace BIZ
{
    public class MarkLogBO
    {
        protected delegate void DelegateUpdateCache(string key, object data);
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_MARKLOG;

        protected delegate void DelegateFlushAllCache(string strGroupKeyCached, string containKey);

        #region CREATE


        public int CreateUpdateMarkLog(MarkLog MarkLog)
        {

            int returnVal = MarkLogDBBase.Create().CreateUpdateMarkLog(MarkLog);
            if (returnVal != -1)
            {
                UpdateCache(MarkLog);
                FlushAllCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ


        public List<MarkLog> GetMarkLogsByContentId(long ContentId)
        {
            string keyCache = Constants.CACHE_KEY_ALL_MARKLOG_BYCONTENT + ContentId;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_MarkLog;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var lstCachedMarkLogs = (List<MarkLog>)LocalCaching.GetData(keyCache);

            if (lstCachedMarkLogs != null)
                return lstCachedMarkLogs;

            var MarkLogs = MarkLogDBBase.Create().GetMarkLog(ContentId);

            if (MarkLogs == null)
                return null;


           
            lstCachedMarkLogs = MarkLogs;
           

            if (lstCachedMarkLogs.Count > 0)
            {
                LocalCaching.Add(keyCache, lstCachedMarkLogs);
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }

            return lstCachedMarkLogs;
        }


        #region UPDATE

        public void UpdateCache(MarkLog Contact)
        {
            var strKeyCached = Constants.CACHE_GROUPKEY_MARKLOG + Contact.Id;
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
