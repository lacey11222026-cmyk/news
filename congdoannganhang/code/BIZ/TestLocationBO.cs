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
    public class TestLocationBO
    {

        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_QUESTION;
       

        
       



        

        #region READ
       
        public List<TestLocation> GetAll()
        {
            var manuFactorys = TestLocationDBBase.Create().GetTestLocation();
            if (manuFactorys == null)
                return new List<TestLocation>();
            return manuFactorys.ToList();
        }
        public List<TestLocation> GetAllCache()
        {
            try
            {
                string keyCache = strGroupKeyCached+Constants.CACHE_KEY_ALL_QUESTIONS_BY_ID + "TestLocation";

                

                var lstItem = RedisCaching.GetData(keyCache);
                if (lstItem != null)
                    return JsonConvert.DeserializeObject<List<TestLocation>>(lstItem.ToString());

                var lstItemBase = GetAll();
                if (lstItemBase == null)
                    return null;

               
                if (lstItemBase.Count > 0)
                {
                    //RedisCaching.Add(keyCache, lstItem);
                    RedisCaching.Add(keyCache, JsonConvert.SerializeObject(lstItemBase));
                    //RedisCaching.AddToGroupKey(keyCache, strGroupKeyCached);
                }

                return lstItemBase;
            }
            catch (Exception e)
            {
                NLogLogger.PublishException(e);
                return null;
            }
           
        }



    
        #endregion


        
    }
}
