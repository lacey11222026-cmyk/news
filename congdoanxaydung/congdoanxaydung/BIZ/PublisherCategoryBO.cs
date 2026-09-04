using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UTILS;

namespace BIZ
{
    public class PublisherCategoryBO
    {
        protected delegate void DelegateUpdateCache(string key, object data);
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_PUBLISHERCATEGORY;

        protected delegate void DelegateFlushAllCache(string strGroupKeyCached, string containKey);

        #region CREATE


        public int CreateUpdatePublisherCategory(PublisherCategory PublisherCategory)
        {

            int returnVal = PublisherCategoryDBBase.Create().CreateUpdatePublisherCategory(PublisherCategory);
            if (returnVal != -1)
            {
                UpdateCache(PublisherCategory);
                FlushAllCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ


        public int SetByUserName(string UserName, string Value)
        {
            var data = GetByUserName(UserName);
            if (data != null && !String.IsNullOrEmpty(data.UserName))
            {
                data.CategoryPath = Value;
                return CreateUpdatePublisherCategory(data);
            }
            else
            {
                var newData = new PublisherCategory();
                newData.UserName = UserName;
                newData.CategoryPath = Value;
                return CreateUpdatePublisherCategory(newData);
            }

        }
        public PublisherCategory GetByUserName(string UserName)
        {
            string keyCache = Constants.CACHE_KEY_ALL_PUBLISHERCATEGORY_BYUSERNAME + UserName;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_PublisherCategory;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var data = LocalCaching.GetData(keyCache);
            if (data != null)
                return JsonConvert.DeserializeObject<PublisherCategory>(data.ToString());

            var publisherCategorys = PublisherCategoryDBBase.Create().GetByUserName(UserName);

            if (publisherCategorys == null)
                return new PublisherCategory();


            LocalCaching.Add(keyCache, JsonConvert.SerializeObject(publisherCategorys));
            LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            return publisherCategorys;
        }


        #region UPDATE

        public void UpdateCache(PublisherCategory contact)
        {
            var strKeyCached = Constants.CACHE_GROUPKEY_PUBLISHERCATEGORY + contact.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, JsonConvert.SerializeObject(contact), null, null);

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
