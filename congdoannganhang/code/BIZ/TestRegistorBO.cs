using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using UTILS;

namespace BIZ
{
    public class TestRegistorBO
    {

        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_TEST;
        protected delegate void DelegateFlushAllContentCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache(string key, object data);
        #region CREATE UPDATE

        public int InsertUpdate(TestRegistor manuFactory)
        {
            var returnVal = TestRegistorDBBase.Create().CreateUpdateTestRegistor(manuFactory);
            if (returnVal != -1)
            {
                FlushAllContentCache(strGroupKeyCached);
            }
            return returnVal;
        }


        #endregion

        #region READ

        public List<TestRegister_Full> GetTestRegistor()
        {
            var strKeyCached = strGroupKeyCached+Constants.CACHE_KEY_TEST;
           

            var lstItem = RedisCaching.GetData(strKeyCached);
            //var lstItem = new List<CONTENT_FULL> ();
            if (lstItem != null)
                return JsonConvert.DeserializeObject<List<TestRegister_Full>>(lstItem.ToString());

            var lstItemBase = TestRegistorDBBase.Create().GetTestRegistor().ToList();
            var lstItemNew = new List<TestRegister_Full>();
            foreach (var data in lstItemBase)
            {
                var item = new TestRegister_Full
                {
                    Id = data.Id,
                    Desciption = data.Desciption,
                    EndTime = data.EndTime,
                    Title = data.Title,
                    StartTime = data.StartTime,
                    NumberQuestion = data.NumberQuestion,
                    Status = data.Status,
                    Type=data.Type,
                    TestTime = data.TestTime
                };

                lstItemNew.Add(item);
            }
            if (lstItemNew.Count > 0)
            {

                RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(lstItemNew));
                //RedisCaching.AddToGroupKey(strKeyCached, strGroupKeyCached);
            }
            return lstItemNew.ToList();
        }
        public TestRegistor GetById(int id)
        {

            return TestRegistorDBBase.Create().GetById(id);
        }
        public TestRegister_Full GetFullById(int id)
        {
            var strKeyCached = strGroupKeyCached + Constants.CACHE_KEY_TEST + id;

            var item = RedisCaching.GetData(strKeyCached);
            if (item != null)
                return JsonConvert.DeserializeObject<TestRegister_Full>(item.ToString());

            var data = TestRegistorDBBase.Create().GetById(id);
            var newitem = new TestRegister_Full
            {
                Id = data.Id,
                Desciption = data.Desciption,
                EndTime = data.EndTime,
                Title = data.Title,
                StartTime = data.StartTime,
                NumberQuestion = data.NumberQuestion,
                Status = data.Status,
                TestTime = data.TestTime
            };
            RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(newitem));

            return newitem;
        }
        public List<TestRegistor> GetAll()
        {
            var manuFactorys = TestRegistorDBBase.Create().GetAll();
            if (manuFactorys == null)
                return new List<TestRegistor>();
            return manuFactorys.ToList();
        }
        public List<TestRegistor> GetAll(string keyword, int status, int pageIndex, int pageSize, ref int totalRecords)
        {
            var manuFactorys = TestRegistorDBBase.Create().GetAll(keyword, status, pageIndex, pageSize, ref totalRecords);
            if (manuFactorys == null)
                return new List<TestRegistor>();
            return manuFactorys.ToList();
        }
        public int Delete(int productId)
        {
            FlushAllContentCache(strGroupKeyCached);
            return TestRegistorDBBase.Create().Delete(productId);

        }
        #endregion
        
        public void FlushAllContentCache(string containKey)
        {
            RedisCaching.RemoveGroup(containKey);
            //RedisCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
        }

    }
}
