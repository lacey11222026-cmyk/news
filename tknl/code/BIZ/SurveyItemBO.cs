using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using UTILS;
using Newtonsoft.Json;

namespace BIZ
{
    public class SurveyItemBO
    {
        protected delegate void DelegateUpdateCache(string key, object data);
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_SURVEYITEM;

        protected delegate void DelegateFlushAllCache(string strGroupKeyCached, string containKey);

        #region CREATE

      
        public int CreateUpdateSurveyItem(SurveyItem SurveyItem)
        {
            
            int returnVal = SurveyItemDBBase.Create().CreateUpdateSurveyItem(SurveyItem);
            if (returnVal != -1)
            {
                UpdateCache(SurveyItem);
                FlushAllCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ

       
       

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get SurveyItem by id => add to local cache
        /// </summary>
        /// <param name="SurveyItemId">The SurveyItem id.</param>
        /// <returns></returns>
        public SurveyItem GetSurveyItem(int SurveyItemId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_SURVEYITEM + SurveyItemId;

                var data = LocalCaching.GetData(strKeyCached);
                if (data != null)
                    return JsonConvert.DeserializeObject<SurveyItem>(data.ToString());

                var surveyItem = SurveyItemDBBase.Create().GetSurveyItem(SurveyItemId);



                LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(surveyItem));

                return surveyItem;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e,"GetSurveyItem");
                return null;
            }
        }

   
       

        

        public List<SurveyItem> GetSurveyItemsBy(int surveyId,int status)
        {
            string keyCache = Constants.CACHE_KEY_ALL_SURVEYITEM + surveyId + "_" + status;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_SURVEYITEM;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var data = LocalCaching.GetData(keyCache);
            if (data != null)
                return JsonConvert.DeserializeObject<List<SurveyItem>>(data.ToString());

            var surveyItems = SurveyItemDBBase.Create().GetBySurveyId(surveyId,status);

            if (surveyItems == null)
                return null;



            var lstCachedSurveyItems = surveyItems.ToList();
          

            if (lstCachedSurveyItems.Count > 0)
            {
                LocalCaching.Add(keyCache, JsonConvert.SerializeObject(lstCachedSurveyItems));
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }

            return lstCachedSurveyItems;
        }

    


        #endregion

        #region UPDATE

        public void UpdateCache(SurveyItem surveyItem)
        {
            var strKeyCached = Constants.CACHE_KEY_SURVEYITEM + surveyItem.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, JsonConvert.SerializeObject(surveyItem), null, null);


        }
      
        #endregion

        #region DELETE
        public int CountAdd(int id)
        {
            var returnVal = SurveyItemDBBase.Create().CountAdd(id);
            if (returnVal != -1)
                FlushAllCache(string.Empty);
            return returnVal;
        }
        public int UpdateStatus(int id, int status)
        {
            var returnVal = SurveyItemDBBase.Create().UpdateStatus(id, status);
            if (returnVal != -1)
                FlushAllCache(string.Empty);
            return returnVal;
        }
        public int DeleteSurveyItems(string listIds)
        {

            var returnVal = SurveyItemDBBase.Create().DeleteSurveyItems(listIds);
            if (returnVal != -1)
                FlushAllCache(string.Empty);
            return returnVal;
        }

        public int DeleteSurveyItem(int id)
        {
            var returnVal = SurveyItemDBBase.Create().DeleteSurveyItem(id);
            if (returnVal != -1)
                FlushAllCache(string.Empty);
            return returnVal;
        }

        public void FlushAllCache(string containKey)
        {
            DelegateFlushAllCache delegateFlushAllCache = LocalCaching.RemoveContainKeyInGroupKey;
            delegateFlushAllCache.BeginInvoke(strGroupKeyCached, containKey, null, null);
            //LocalCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
        }

        #endregion
    }
}
