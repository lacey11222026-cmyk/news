using DATA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace BIZ
{
    public class SystemConfigBO
    {
        protected delegate void DelegateUpdateCache(string key, object data);
        readonly string strGroupKeyCached = Constants.CACHE_GROUPKEY_SYSTEMCONFIG;

        protected delegate void DelegateFlushAllCache(string strGroupKeyCached, string containKey);

        #region CREATE


        public int CreateUpdateSystemConfig(SystemConfig SystemConfig)
        {

            int returnVal = SystemConfigDBBase.Create().CreateUpdateSystemConfig(SystemConfig);
            if (returnVal != -1)
            {
                UpdateCache(SystemConfig);
                FlushAllCache(string.Empty);
            }
            return returnVal;
        }

        #endregion

        #region READ
         public int SetByKey(string Key,string Value)
         {
             var data = GetByKey(Key);
             if(data!=null)
             {
                 data.ConfigValue = Value;
                 return CreateUpdateSystemConfig(data);
             }
             else
             {
                 var newData = new SystemConfig();
                 newData.ConfigValue = Value;
                 newData.ConfigKey = Key;
                 return CreateUpdateSystemConfig(newData);
             }

         }
         public string GetValueByKey(string key)
         {
             var data = GetByKey(key);
             if (data != null)
                 return data.ConfigValue;
             return "";
         }
        public SystemConfig GetByKey(string  key)
        {
            //string keyCache = Constants.CACHE_KEY_ALL_SYSTEMCONFIG_BYKEY + key;
            ////string groupKeyCache = Constants.CACHE_GROUPKEY_SystemConfig;

            //var listGroupKey = (List<string>)LocalCaching.GetData(strGroupKeyCached);
            //if (listGroupKey == null || listGroupKey.IndexOf(keyCache) == -1)
            //    LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            //var data = LocalCaching.GetData(keyCache);
            //if (data != null)
            //    return JsonConvert.DeserializeObject<SystemConfig>(data.ToString());

            //var SystemConfigs = SystemConfigDBBase.Create().GetSystemConfig(key);

            //if (SystemConfigs == null)
            //    return null;

            //var lstCachedSystemConfigs = SystemConfigs;


            //LocalCaching.Add(keyCache, JsonConvert.SerializeObject(lstCachedSystemConfigs));
            //LocalCaching.AddToGroupKey(keyCache, strGroupKeyCached);

            //return lstCachedSystemConfigs;
            return SystemConfigDBBase.Create().GetSystemConfig(key);
        }


        #region UPDATE

        public void UpdateCache(SystemConfig obj)
        {
            var strKeyCached = Constants.CACHE_GROUPKEY_SYSTEMCONFIG + obj.Id;
            DelegateUpdateCache delegateUpdateCache = LocalCaching.Add;
            delegateUpdateCache.BeginInvoke(strKeyCached, JsonConvert.SerializeObject(obj), null, null);

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
