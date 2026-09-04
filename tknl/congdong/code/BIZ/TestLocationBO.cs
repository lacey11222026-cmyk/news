namespace BIZ
{
    using DATA.ContentDB;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UTILS;

    public class TestLocationBO
    {
        private readonly string strGroupKeyCached = "TestLocationGroup";

        public List<TestLocation> GetAll()
        {
            List<TestLocation> source = new TestLocationDAL().GetList();
            return ((source != null) ? source.ToList<TestLocation>() : new List<TestLocation>());
        }

        public List<TestLocation> GetAllCache()
        {
            List<TestLocation> list2;
            try
            {
                string key = this.strGroupKeyCached + "TestLocationList" + "TestLocation";
                object data = RedisCaching.GetData(key);
                if (data != null)
                {
                    list2 = JsonConvert.DeserializeObject<List<TestLocation>>(data.ToString());
                }
                else
                {
                    List<TestLocation> all = this.GetAll();
                    if (all == null)
                    {
                        list2 = null;
                    }
                    else
                    {
                        if (all.Count > 0)
                        {
                            RedisCaching.Add(key, JsonConvert.SerializeObject(all));
                        }
                        list2 = all;
                    }
                }
            }
            catch (Exception exception1)
            {
                NLogLogger.PublishException(exception1);
                list2 = null;
            }
            return list2;
        }
    }
}
