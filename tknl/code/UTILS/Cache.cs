using System;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
namespace UTILS
{
    public static class Cache
    {
        private static readonly ICacheProvider CacheProvider;


        static Cache()
        {
            try
            {
                if (CacheProvider != null)
                    return;
                if (Config.EnableCache)
                {
                    var container = new UnityContainer();
                    var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                    section.Configure(container, "defaultContainer");
                    CacheProvider = container.Resolve<ICacheProvider>();
                }
            }
            catch (Exception ex)
            {

                ExHandler.Handle(ex);
            }

            //var serviceLocator = new UnityServiceLocator(container);
            //ServiceLocator.SetLocatorProvider(() => serviceLocator);
            //CacheProvider = (ICacheProvider)ServiceLocator.Current.GetInstance(typeof(ICacheProvider));
        }

        public static void Add(string key, object value)
        {
            CacheProvider.Add(key, value);
        }

        public static void Add(string key, object value, int timeout)
        {
            CacheProvider.Add(key, value, timeout);
        }

        public static void Add(string key, object value, string tagName, RegionName regionName)
        {
            CacheProvider.Add(key, value, tagName, regionName.ToString());
        }

        public static void Add(string key, object value, int timeout, string tagName, RegionName regionName)
        {
            CacheProvider.Add(key, value, timeout, tagName, regionName.ToString());
        }

        public static void Add(string key, object value, List<string> tagNames, RegionName regionName)
        {
            CacheProvider.Add(key, value, tagNames, regionName.ToString());
        }

        public static void Add(string key, object value, int timeout, List<string> tagNames, RegionName regionName)
        {
            CacheProvider.Add(key, value, timeout, tagNames, regionName.ToString());
        }

        public static void Put(string key, object value)
        {
            CacheProvider.Put(key, value);
        }

        public static void Put(string key, object value, int timeout)
        {
            try
            {
                CacheProvider.Put(key, value, timeout);
            }
            catch (Exception ex)
            {

                ExHandler.Handle(ex);
            }
        }

        public static void Put(string key, object value, RegionName regionName)
        {
            CacheProvider.Put(key, value, regionName.ToString());
        }

        public static void Put(string key, object value, string tagName, RegionName regionName)
        {
            CacheProvider.Put(key, value, tagName, regionName.ToString());
        }

        public static void Put(string key, object value, int timeout, string tagName, RegionName regionName)
        {
            try
            {
                if (Config.EnableCache)
                {
                    CacheProvider.Put(key, value, timeout, tagName, regionName.ToString());
                }

            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "Cache", "Put, key=" + key.ToString());

            }
        }

        public static void Put(string key, object value, List<string> tagNames, RegionName regionName)
        {
            CacheProvider.Put(key, value, tagNames, regionName.ToString());
        }

        public static void Put(string key, object value, int timeout, List<string> tagNames, RegionName regionName)
        {
            CacheProvider.Put(key, value, timeout, tagNames, regionName.ToString());
        }

        public static object Get(string key)
        {
            //return null;
            return CacheProvider.Get(key);
        }

        public static object Get(string key, RegionName regionName)
        {
            try
            {
                //return null;
                if (Config.EnableCache)
                {
                    return CacheProvider.Get(key, regionName.ToString());
                    ExHandler.Handle(new Exception("cache"), "Cache", "Get, key=" + key.ToString());
                }
                return null;

            }
            catch (Exception ex)
            {
                ExHandler.Handle(ex, "Cache", "Get, key=" + key.ToString());
                return null;
            }
        }

        public static bool Remove(string key)
        {
            return CacheProvider.Remove(key);
        }
        public static bool Remove(string key, RegionName regionName)
        {
            return CacheProvider.Remove(key, regionName.ToString());
        }

        public static void RemoveByTag(string tagName, RegionName regionName)
        {
            try
            {
                if (Config.EnableCache)
                {
                    CacheProvider.RemoveByTag(tagName, regionName.ToString());
                }
            }
            catch (Exception ex)
            {

                ExHandler.Handle(ex, "Cache", "RemoveByTag, tagName=" + tagName.ToString());
            }
        }

        public static bool RemoveRegion(RegionName regionName)
        {
            return CacheProvider.RemoveRegion(regionName.ToString());
        }

        public static void ClearByRegion(RegionName regionName)
        {
            CacheProvider.ClearByRegion(regionName.ToString());
        }
        public static void Flush()
        {
            CacheProvider.Flush();
            foreach (RegionName regionName in Enum.GetValues(typeof(RegionName)))
            {
                try
                {
                    ClearByRegion(regionName);
                    RemoveRegion(regionName);
                }
                catch (Exception ex)
                {

                }
            }
        }

        public enum RegionName
        {
            NEWS_REGION

        }

        public enum TagName
        {
            NEWS_TAG


        }

    }
}
