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
    public class SurveyBO
    {
        protected delegate void DelegateUpdateCache(string key, object data);
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_SURVEY;

        protected delegate void DelegateFlushAllCache(string strGroupKeyCached, string containKey);

        #region CREATE

      
        public int CreateUpdateSurvey(Survey Survey)
        {
            
            int returnVal = SurveyDBBase.Create().CreateUpdateSurvey(Survey);
            if (returnVal != -1)
            {
                UpdateCache(Survey);
                FlushAllCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ

       
       

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 08/08/2011 02:02 PM
        /// todo: get Survey by id => add to local cache
        /// </summary>
        /// <param name="SurveyId">The Survey id.</param>
        /// <returns></returns>
        public Survey GetSurvey(int SurveyId)
        {
            try
            {
                var strKeyCached = Constants.CACHE_KEY_SURVEY + SurveyId;


                var data = LocalCaching.GetData(strKeyCached);
                if (data != null)
                    return JsonConvert.DeserializeObject<Survey>(data.ToString());

                var survey = SurveyDBBase.Create().GetSurvey(SurveyId);



                LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(survey));

                return survey;
            }
            catch (Exception e)
            {
                ExHandler.Handle(e,"GetSurvey");
                return null;
            }
        }
        public List<Survey> GetSurveyByIds(string ids, int top, bool isArragne = false)
        {
            var strKeyCached = Constants.CACHE_KEY_SURVEY_BYIDS + top + "_lst" + ids + isArragne;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null)
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);

            var data = LocalCaching.GetData(strKeyCached);
            if (data != null)
                return JsonConvert.DeserializeObject<List<Survey>>(data.ToString());

            var lstItemBase = SurveyDBBase.Create().GetSurveyByIds(ids, top);
            var lstItem = new List<Survey>();
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

                                lstItem.Add(content);
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


                    lstItem.Add(content);
                }
            }


            if (lstItem.Count > 0)
            {


                LocalCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItem));
                LocalCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            }

            return lstItem;
        }
        public List<Survey> GetAllSurveysPaged(int pageIndex, int pageSize, ref int totalRecords,int status)
        {
            string keyCache = Constants.CACHE_KEY_ALL_SURVEY + pageIndex + "_" + pageSize + "_" + status;
            string strKeyCachedTotal = Constants.CACHE_KEY_ALL_SURVEY + pageIndex + "_" + pageSize + "_" + status+ "_total";

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
            {
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
                LocalCaching.AddToGroupKey(strKeyCachedTotal, strGroupKeyCached);
            }
            var data = LocalCaching.GetData(keyCache);
            if (data != null)
            {
                totalRecords = int.Parse((string)LocalCaching.GetData(strKeyCachedTotal));
                return JsonConvert.DeserializeObject<List<Survey>>(data.ToString());
            }
            var surveys = SurveyDBBase.Create().GetAllSurveysPaged(pageIndex, pageSize, ref totalRecords, status).ToList();

            if (surveys.Count > 0)
            {
                LocalCaching.Add(keyCache, JsonConvert.SerializeObject(surveys));
                LocalCaching.Add(strKeyCachedTotal, totalRecords.ToString());
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }

            return surveys.ToList();
        }




        public List<Survey> GetAllSurveys(int top,int status,int category,string title)
        {
            string keyCache = Constants.CACHE_KEY_ALL_SURVEY + status + "_" + status + "_cate" + category + "_title" + title + "_top" + top;
            //string groupKeyCache = Constants.CACHE_GROUPKEY_SURVEY;

            var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            var data = LocalCaching.GetData(keyCache);
            if (data != null)
                return JsonConvert.DeserializeObject<List<Survey>>(data.ToString());

            var surveys = SurveyDBBase.Create().GetAllSurveys(top,status, category,title);

            if (surveys == null)
                return null;

            var lstCachedSurveys = surveys.ToList();
           

            if (lstCachedSurveys.Count > 0)
            {
                LocalCaching.Add(keyCache, JsonConvert.SerializeObject(lstCachedSurveys));
                LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }

            return lstCachedSurveys;
        }

    


        #endregion

        #region UPDATE

        public void UpdateCache(Survey survey)
        {
            var strKeyCached = Constants.CACHE_KEY_SURVEY + survey.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, JsonConvert.SerializeObject(survey), null, null);

        }

        #endregion

        #region DELETE

        public int DeleteSurveys(string listIds)
        {

            var returnVal = SurveyDBBase.Create().DeleteSurveys(listIds);
            if (returnVal != -1)
                FlushAllCache(string.Empty);
            return returnVal;
        }

        public int DeleteSurvey(int id)
        {
            var returnVal = SurveyDBBase.Create().DeleteSurvey(id);
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
