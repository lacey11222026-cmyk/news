using System;
using Microsoft.ApplicationServer.Caching;
using System.Collections.Generic;
namespace UTILS.AppFabric
{
    public class CacheProvider : ICacheProvider
    {
        private static DataCacheFactory _Factory;
        private static DataCache _Cache;
        private const string CACHE_NAME_DEFAULT = "tietkiemnl";

        private static DataCache GetCache()
        {
            if (!Config.EnableCache)
            {
                return null;
            }
            if (_Cache != null)
                return _Cache;

            var configuration = new DataCacheFactoryConfiguration();
            _Factory = new DataCacheFactory(configuration);
            _Cache = _Factory.GetCache(CACHE_NAME_DEFAULT);
            return _Cache;
        }

        public void Add(string key, object value)
        {
            this.Add(key, value, 0);
        }

        public void Add(string key, object value, int timeout)
        {
            var cache = GetCache();
            if (timeout > 0)
                cache.Add(key, value, TimeSpan.FromMinutes(timeout));
            else
                cache.Add(key, value);
        }

        public void Add(string key, object value, string tagName, string regionName)
        {
            List<string> tagNames = new List<string>(1) { tagName };
            this.Add(key, value, 0, tagNames, regionName);
        }

        public void Add(string key, object value, int timeout, string tagName, string regionName)
        {
            List<string> tagNames = new List<string>(1) { tagName };
            this.Add(key, value, timeout, tagNames, regionName);
        }

        public void Add(string key, object value, List<string> tagNames, string regionName)
        {
            this.Add(key, value, 0, tagNames, regionName);
        }

        public void Add(string key, object value, int timeout, List<string> tagNames, string regionName)
        {
            var cache = GetCache();
            cache.CreateRegion(regionName);
            DataCacheTag[] tags = new DataCacheTag[tagNames.Count];
            int id = 0;
            foreach (string tagName in tagNames)
            {
                tags[id++] = new DataCacheTag(tagName);
            }

            if (timeout > 0)
                cache.Add(key, value, TimeSpan.FromMilliseconds(timeout), tags, regionName);
            else
                cache.Add(key, value, tags, regionName);
        }

        public void Put(string key, object value)
        {
            this.Put(key, value, 0);
        }

        public void Put(string key, object value, int timeout)
        {
            var cache = GetCache();
            if (timeout > 0)
                cache.Put(key, value, TimeSpan.FromMinutes(timeout));
            else
                cache.Put(key, value);
        }

        public void Put(string key, object value, string regionName)
        {
            this.Put(key, value, regionName);
        }

        public void Put(string key, object value, string tagName, string regionName)
        {
            List<string> tagNames = new List<string>(1) { tagName };
            this.Put(key, value, 0, tagNames, regionName);
        }

        public void Put(string key, object value, int timeout, string tagName, string regionName)
        {

            List<string> tagNames = new List<string>(1) { tagName };
            this.Put(key, value, timeout, tagNames, regionName);
        }

        public void Put(string key, object value, List<string> tagNames, string regionName)
        {
            this.Put(key, value, 0, tagNames, regionName);
        }

        public void Put(string key, object value, int timeout, List<string> tagNames, string regionName)
        {
            var cache = GetCache();
            cache.CreateRegion(regionName);
            DataCacheTag[] tags = new DataCacheTag[tagNames.Count];
            int id = 0;
            foreach (string tagName in tagNames)
            {
                tags[id++] = new DataCacheTag(tagName);
            }

            if (timeout > 0)
                cache.Put(key, value, TimeSpan.FromMinutes(timeout), tags, regionName);
            else
                cache.Put(key, value, tags, regionName);
        }

        public object Get(string key)
        {
            var cache = GetCache();
            return cache.Get(key);
        }

        public object Get(string key, string regionName)
        {
            var cache = GetCache();
            return cache.Get(key, regionName);
        }

        public object this[string key]
        {
            get
            {
                var cache = GetCache();
                return cache.Get(key);
            }
            set
            {
                var cache = GetCache();
                cache.Put(key, value);
            }
        }

        public bool Remove(string key)
        {
            var cache = GetCache();
            return cache.Remove(key);
        }

        public bool Remove(string key, string regionName)
        {
            var cache = GetCache();
            return cache.Remove(key, regionName);
        }

        public void RemoveByTag(string tagName, string regionName)
        {
            var cache = GetCache();
            var items = cache.GetObjectsByTag(new DataCacheTag(tagName), regionName);
            foreach (var item in items)
            {
                cache.Remove(item.Key, regionName);
            }
        }

        public bool RemoveRegion(string regionName)
        {
            var cache = GetCache();
            return cache.RemoveRegion(regionName);
        }

        public void ClearByRegion(string regionName)
        {
            var cache = GetCache();
            cache.ClearRegion(regionName);
        }

        public void Flush()
        {
            var cache = GetCache();
            var regions = cache.GetSystemRegions();
            foreach (var region in regions)
            {
                cache.ClearRegion(region);
            }
        }
    }
}
