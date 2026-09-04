using BIZ.Entity;
using DATA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UTILS;

namespace BIZ
{
    public class Datum
    {
        public string currencyName { get; set; }
        public string currencyCode { get; set; }
        public decimal cash { get; set; }
        public string transfer { get; set; }
        public decimal sell { get; set; }
        public string icon { get; set; }
    }

    public class RateRespone
    {
        public int Count { get; set; }
        public DateTime Date { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<Datum> Data { get; set; }
    }

    public class SystemConfigBO
    {


        #region CREATE


        public int CreateUpdateSystemConfig(SystemConfig SystemConfig)
        {

            int returnVal = SystemConfigDBBase.Create().CreateUpdateSystemConfig(SystemConfig);

            return returnVal;
        }

        #endregion

        #region READ
        public int SetByKey(string Key, string Value)
        {
            var data = GetByKey(Key);
            if (data != null)
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
        private List<Datum> GetRateApi()
        {
            var url = String.Format("https://www.vietcombank.com.vn/api/exchangerates?date={0}", DateTime.Now.ToString("yyyy-MM-dd"));
            try
            {

                var apitext = Utilities.HttpRequestGet(url);
                //NLogLogger.DebugMessage(url);
                //NLogLogger.DebugMessage(apitext);
                return JsonConvert.DeserializeObject<RateRespone>(apitext).Data;
            }

            catch (Exception ex)
            {
                NLogLogger.PublishException(ex);
                NLogLogger.DebugMessage(url);
                return null;

            }

        }
        public List<Datum> GetRate()
        {
            var strKeyCached = "ratedata";
            var cachedata = RedisCaching.GetData(strKeyCached);
            if (cachedata != null)
                return JsonConvert.DeserializeObject<List<Datum>>(cachedata.ToString());

            var data = GetRateApi();
            RedisCaching.Add(strKeyCached, JsonConvert.SerializeObject(data), 3600 * 8);
            return data;
        }
        public SystemConfig GetByKey(string key)
        {
            return SystemConfigDBBase.Create().GetSystemConfig(key);
        }
        public string GetValueByKey(string key)
        {
            var data = SystemConfigDBBase.Create().GetSystemConfig(key);
            if (data != null)
                return data.ConfigValue;
            return "";
        }

        #region UPDATE



        #endregion


        #endregion



    }
}
