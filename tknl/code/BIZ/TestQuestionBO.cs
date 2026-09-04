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
    public class TestQuestionBO
    {

        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_QUESTION;
       

        protected delegate void DelegateFlushAllProductCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateFlushAllProductAttributeCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);
        #region CREATE UPDATE

        public int InsertUpdate(TestQuestion manuFactory)
        {
          
            int returnValue = TestQuestionDBBase.Create().CreateUpdateTestQuestion(manuFactory);
            if (returnValue != -1)
            {
                //UpdateCache(manuFactory);
                FlushAllProductCache(strGroupKeyCached);
            }

            return returnValue;
        }
        public void FlushAllProductCache(string containKey)
        {
            // remove product cache
            RedisCaching.RemoveGroup(containKey);



        }



        #endregion

        #region READ
        public TestQuestion GetById(int id)
        {
            return TestQuestionDBBase.Create().GetById(id);

        }
        public List<TestQuestion> GetByRegistorId(int id, int status , int pageIndex, int pageSize, ref int totalRecords)
        {
            var manuFactorys = TestQuestionDBBase.Create().GetByRegistorId( id, status,  pageIndex,  pageSize, ref  totalRecords);
            if (manuFactorys == null)
                return new List<TestQuestion>();
            return manuFactorys.ToList();
        }
        public List<TestQuestion_Full> GetByRegistorId(int id)
        {
            try
            {
                string keyCache = strGroupKeyCached+Constants.CACHE_KEY_ALL_QUESTIONS_BY_ID + id;

                

                var lstItem = RedisCaching.GetData(keyCache);
                if (lstItem != null)
                    return JsonConvert.DeserializeObject<List<TestQuestion_Full>>(lstItem.ToString());

                var lstItemBase = TestQuestionDBBase.Create().GetByRegistorId(id);
                if (lstItemBase == null)
                    return null;

                var lstItemNew = new List<TestQuestion_Full>();
                //lstItemBase.Reverse();
                foreach (var itemBase in lstItemBase)
                {
                    var item = new TestQuestion_Full()
                    {
                        Id = itemBase.Id,
                        Title = itemBase.Title,
                        RegistorId = itemBase.RegistorId,
                        Mark = itemBase.Mark,
                        Explain = itemBase.Explain,
                        Answers = itemBase.Answers,
                        Type = itemBase.Type,
                        Contents = itemBase.Contents,
                        Status = itemBase.Status,
                        
                    };
                    item.AnswersInfo = JsonConvert.DeserializeObject<List<AnswerInfo>>(item.Answers);
                    foreach (var an in item.AnswersInfo)
                    {
                        if(an.IsCheck)
                            item.Result += an.Order+",";
                    }
                    item.Result= ","+ item.Result;
                    lstItemNew.Add(item);
                }

                if (lstItemNew.Count > 0)
                {
                    //RedisCaching.Add(keyCache, lstItem);
                    RedisCaching.Add(keyCache, JsonConvert.SerializeObject(lstItemNew));
                    //RedisCaching.AddToGroupKey(keyCache, strGroupKeyCached);
                }

                return lstItemNew;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
           
        }



        public int Delete(int productId)
        {

            FlushAllProductCache(strGroupKeyCached);
            return TestQuestionDBBase.Create().Delete(productId);
            
        }
        #endregion


        
    }
}
