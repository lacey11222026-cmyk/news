using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace UTILS
{
    public class LocalCaching
    {
        private const int Timeout = 15;
        private static readonly ICacheManager CacheManager = CacheFactory.GetCacheManager();
        private static readonly ICacheManager CacheManagerExpiration = CacheFactory.GetCacheManager("Expiration Cache Manager");
       
       
        public static void Add(string key, object data)
        {
            //var defaulCacheTimeExpired = int.Parse(ConfigurationManager.AppSettings["defaulCacheSecondTimeExpired"] ?? "1200");
            try
            {
                if (Config.EnableCache)
                {
                    if (Config.IsLocalCache)
                    {
                        CacheManager.Add(key, data, CacheItemPriority.Normal, null,
                                         new SlidingTime(TimeSpan.FromSeconds(Timeout * 60)));
                    }
                    else
                    {
                        Cache.Put(key, data, Timeout);
                    }

                }

            }
            catch (Exception e)
            {
                ExHandler.Handle(e);
            }

        }

        public static void Add(string key, object data, int minutes)
        {
            try
            {
                if (Config.EnableCache)
                {
                    if (Config.IsLocalCache)
                    {
                        CacheManagerExpiration.Add(key, data, CacheItemPriority.Normal, null,
                                                   new SlidingTime(TimeSpan.FromMinutes(minutes)));
                    }
                    else
                    {
                        Cache.Add(key, data, minutes);
                    }
                }

            }
            catch (Exception e)
            {
                ExHandler.Handle(e);
            }

        }



        public static void Remove(string key)
        {
            try
            {
                if (Config.EnableCache)
                {
                    if (Config.IsLocalCache)
                    {
                        CacheManager.Remove(key);
                    }
                    else
                    {
                        Cache.Remove(key);
                    }
                }

            }
            catch (Exception e)
            {
                ExHandler.Handle(e);
            }

        }


        public static void Flush()
        {
            try
            {
                if (Config.EnableCache)
                {
                    if (Config.IsLocalCache)
                    {
                        CacheManager.Flush();
                    }
                    else
                    {
                        Cache.Flush();
                    }
                }

            }
            catch (Exception e)
            {
                ExHandler.Handle(e);
            }

        }

        public static object GetData(string key)
        {
            try
            {
                if (Config.EnableCache)
                {
                    if (Config.IsLocalCache)
                    {
                        return CacheManager.GetData(key);
                    }
                    else
                    {
                        return Cache.Get(key);
                    }
                }
                return null;

            }
            catch (Exception e)
            {
                ExHandler.Handle(e);
                return null;
            }

        }

        public static object GetDataExpiration(string key)
        {
            try
            {
                if (Config.EnableCache)
                {
                    if (Config.IsLocalCache)
                    {
                        return CacheManagerExpiration.GetData(key);
                    }
                    else
                    {
                        return Cache.Get(key);
                    }
                }
                return null;

            }
            catch (Exception e)
            {
                ExHandler.Handle(e);
                return null;
            }
        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 20/08/2011 06:11 PM
        /// todo: add key to groupkey
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="groupKey">The group key.</param>
        public static void AddToGroupKey(string key, string groupKey)
        {
            try
            {
                List<string> listKeyName = (List<string>)GetData(groupKey);
                if (listKeyName == null)
                    listKeyName = new List<string>();

                listKeyName.Add(key);
                Add(groupKey, listKeyName);
            }
            catch (Exception e)
            {
                ExHandler.Handle(e);

            }

        }

        /// <summary>
        /// created by: manhcuong.phung 
        /// date: 20/08/2011 06:10 PM
        /// todo: remove all data where key contain in groupKey
        /// </summary>
        /// <param name="groupKey">The group key.</param>
        /// <param name="containKey">The contain key.</param>
        public static void RemoveContainKeyInGroupKey(string groupKey, string containKey)
        {
            try
            {
                List<string> listKeyName = (List<string>)GetData(groupKey);
                if (listKeyName != null && listKeyName.Count > 0)
                {
                    if (!string.IsNullOrEmpty(containKey))
                    {
                        var searchKey = from p in listKeyName
                                        where p.Contains(containKey)
                                        select p;
                        if (searchKey.Count() > 0)
                        {
                            foreach (var key in searchKey)
                            {
                                Remove(key);
                            }
                        }
                    }
                    else
                    {
                        if (listKeyName.Count > 0)
                        {
                            foreach (var key in listKeyName)
                            {
                                Remove(key);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                ExHandler.Handle(e);
            }

        }


    }
}
