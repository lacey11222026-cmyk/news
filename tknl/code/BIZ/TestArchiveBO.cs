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
    public class TestArchiveBO
    {
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_TEST + "Archive";
        protected delegate void DelegateFlushAllContentCache(string strGroupKeyCached, string containKey);
        protected delegate void DelegateUpdateCache( object data);

        #region CREATE UPDATE

        public int InsertUpdate(TestArchive manuFactory)
        {
            if (manuFactory.Id <= 0)
            {
              
                manuFactory.CreatedDate = DateTime.Now;
                manuFactory.StartTime = DateTime.Now;
            }
            else
            {
                TimeSpan span = (manuFactory.EndTime.GetValueOrDefault() - manuFactory.StartTime.GetValueOrDefault());
                manuFactory.TestTime = (int)span.TotalSeconds;
            }
            var result=TestArchiveDBBase.Create().CreateUpdateTestArchive(manuFactory);
            if(result>-1)
            {

                if (manuFactory.Id <= 0)
                {
                    manuFactory.Id = result;
                    manuFactory.CreatedDate = DateTime.Now;
                    manuFactory.StartTime = manuFactory.CreatedDate;
                }
                UpdateContentCache(manuFactory);
            }
            return result;
        }


        #endregion

        #region READ
        public List<TestArchiveTop> SelecTop()
        {
            var manuFactorys = TestArchiveDBBase.Create().SelectTop();
            if (manuFactorys == null)
                return new List<TestArchiveTop>();
            return manuFactorys.ToList();
        }
        public List<TestArchive> GetByRegistorId(int id, string mobile, int pageIndex, int pageSize, ref int totalRecords, int OrderType,int status)
        {
            var manuFactorys = TestArchiveDBBase.Create().GetByRegistorId(id, mobile, pageIndex, pageSize, ref totalRecords, OrderType,status);
            if (manuFactorys == null)
                return new List<TestArchive>();
            return manuFactorys.ToList();
        }
        public List<TestArchive> GetByMobile(int id, string mobile)
        {
            var manuFactorys = TestArchiveDBBase.Create().GetByMobile(id, mobile);
            if (manuFactorys == null)
                return null;
            return manuFactorys;
        }
        public List<TestArchiveReport> Report(int id)
        {
            string keyCache = strGroupKeyCached + "KeyReportArchive" + id;
            var lstItem = RedisCaching.GetData(keyCache);
            if (lstItem != null)
                return JsonConvert.DeserializeObject<List<TestArchiveReport>>(lstItem.ToString());
            var manuFactorys = TestArchiveDBBase.Create().Report(id);
            if (manuFactorys == null)
                return null;
            if (manuFactorys.Count > 0)
            {
                //RedisCaching.Add(keyCache, lstItem);
                RedisCaching.Add(keyCache, JsonConvert.SerializeObject(manuFactorys));
                //RedisCaching.AddToGroupKey(keyCache, strGroupKeyCached);
            }
            return manuFactorys;
        }
        public TestArchive GetById(int id)
        {
            var strKeyCached = strGroupKeyCached + id;

            var item = RedisCaching.GetData(strKeyCached);
            if (item != null)
                return JsonConvert.DeserializeObject<TestArchive>(item.ToString());

            var data = TestArchiveDBBase.Create().GetById(id);
            var newitem = new TestArchive
            {
                Id = data.Id,
                Archive = data.Archive,
                EndTime = data.EndTime,
                CreatedDate = data.CreatedDate,
                FulName = data.FulName,
                Location = data.Location,
                Mark = data.Mark,
                TestTime = data.TestTime,
                Mobile = data.Mobile,
                Questions = data.Questions,
                RegistorId = data.RegistorId,
                StartTime = data.StartTime,
                Status = data.Status,


            };
            RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(newitem));

            return newitem;

        }

        public int Delete(int productId)
        {

            return TestArchiveDBBase.Create().Delete(productId);

        }
        public void FlushAllContentCache(string containKey)
        {
            RedisCaching.RemoveGroup(containKey);
            //RedisCaching.RemoveContainKeyInGroupKey ( strGroupKeyCached, containKey );
        }
        public void UpdateContentCache(TestArchive obj )
        {
            var strKeyCached = strGroupKeyCached + obj.Id;
            RedisCaching.RemoveGroup(strKeyCached);
            RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(obj));
        }
        #endregion



    }
}
