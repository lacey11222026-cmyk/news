using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web;
using cms.libs;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;

/// <summary>
/// Summary description for Class1
/// </summary>
public class CacheUtilEnyim : ICacheUtil
{
    private static readonly object tobj = new object();
    private static readonly CacheUtilEnyim instance = new CacheUtilEnyim();
    private static readonly string ALL_CACHE_KEYS = Config.CachePool;
    private static List<string> list_keys;
    private MemcachedClient _mcClient;

    private CacheUtilEnyim()
    {
        
        //
        // TODO: Add constructor logic here
        //
    }

    public static CacheUtilEnyim Instance
    {
        get
        {
            return instance;
        }
    }

    #region ICacheUtil Members

    public void RemoveByGroup(params string[] keys)
    {
        lock (tobj)
        {
            string key;
            IDictionaryEnumerator ce = HttpRuntime.Cache.GetEnumerator();
            while (ce.MoveNext())
            {
                key = ce.Key as string;
                bool ok = true;
                foreach (string k in keys)
                {
                    if (!key.Contains(k))
                    {
                        ok = false;
                        break;
                    }
                }

                if (ok) HttpRuntime.Cache.Remove(key);
            }
        }
    }

    /// <summary>
    /// Remove cache group by name
    /// </summary>
    /// <param name="CacheGroupName"></param>
    public void RemoveByCacheGroupName(string cacheGroupName)
    {
        if (_mcClient == null) LoadConfigCache();
        if (_mcClient != null)
        {
            list_keys = (List<string>)_mcClient.Get(ALL_CACHE_KEYS);
            var ls = new List<string>();
            if (list_keys != null)
            {
                foreach (string key in list_keys)
                {
                    if (key.IndexOf(cacheGroupName) > -1) _mcClient.Remove(key);
                    else ls.Insert(0, key);
                }
                _mcClient.Store(StoreMode.Replace, ALL_CACHE_KEYS, ls);
            }
        }
    }

    /// <summary>
    /// Remove cache group by name and account
    /// </summary>
    /// <param name="CacheGroupName"></param>
    public void RemoveByCacheGroupName(string cacheGroupName, string pAccount)
    {
        if (_mcClient == null) LoadConfigCache();
        if (_mcClient != null)
        {
            string all_key = string.Empty;
            // kiem tra tat ca cac cachkey hien co
            if (pAccount.IndexOf("VTCNews") > -1)
            {
                // find accountId in key
                int idStart = pAccount.IndexOf("VTCNews");
                int idEnd;
                if (pAccount.IndexOf("_", idStart) > -1) idEnd = pAccount.IndexOf("_", idStart);
                else idEnd = pAccount.Length;
                // lay danh sach cache theo accountid
                all_key = ALL_CACHE_KEYS + pAccount.Substring(idStart, idEnd - idStart);
            }
            else
            {
                all_key = ALL_CACHE_KEYS;
            }
            list_keys = (List<string>)_mcClient.Get(all_key);
            var ls = new List<string>();
            if (list_keys != null)
            {
                foreach (string key in list_keys)
                {
                    if (key.IndexOf(cacheGroupName) > -1 && key.IndexOf(pAccount) > -1) _mcClient.Remove(key);
                    else ls.Insert(0, key);
                }
                _mcClient.Store(StoreMode.Replace, all_key, ls);
            }
        }
    }

    public void Remove(string cacheKey)
    {
        if (_mcClient == null) LoadConfigCache();
        if (_mcClient != null)
        {
            _mcClient.Remove(cacheKey);
        }
    }

    public void RemoveAll()
    {
        if (_mcClient == null) LoadConfigCache();
        if (_mcClient != null)
        {
            _mcClient.FlushAll();
        }
    }

    public bool CheckCache(string key)
    {
        if (_mcClient == null) LoadConfigCache();
        if (_mcClient != null)
        {
            return (_mcClient.Get(key) != null);
        }
        else return false;
    }

    public object GetCache(string key)
    {
        //return null;
        if (_mcClient == null) LoadConfigCache();
        if (_mcClient != null)
        {
            string all_key = string.Empty;
            if (key.IndexOf("GoNews") > -1)
            {
                // find accountId in key
                int idStart = key.IndexOf("GoNews");
                int idEnd;
                if (key.IndexOf("_", idStart) > -1) idEnd = key.IndexOf("_", idStart);
                else idEnd = key.Length;
                // day la danh sach tat ca cac cachkey theo account id
                all_key = ALL_CACHE_KEYS + key.Substring(idStart, idEnd - idStart);
                list_keys = (List<string>)_mcClient.Get(all_key);
                if (list_keys != null)
                {
                    if (list_keys != null && !list_keys.Contains(key))
                    {
                        list_keys.Insert(0, key);
                        _mcClient.Store(StoreMode.Set, all_key, list_keys);
                    }
                }
            }
            return _mcClient.Get(key);
        }
        else return null;
    }

    /// <summary>
    /// Dat cache cho 1 object voi thoi gian song 3 phut.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="val"></param>
    public void SetCache(string key, object val)
    {
        SetCacheWithTime(key, val, 360);
    }

    /// <summary>
    /// Set cache truc tiep (khong xu ly viec phan vung cache)
    /// </summary>
    /// <param name="key"></param>
    /// <param name="val"></param>
    public void Set(string key, object val)
    {
        if (_mcClient == null) LoadConfigCache();
        if (_mcClient != null)
        {
            _mcClient.Store(StoreMode.Set, key, val);
        }
    }

    /// <summary>
    /// Dat Cache voi thoi gian song cho cache.
    /// </summary>
    /// <param name="key">Cache Key</param>
    /// <param name="val">Doi tuong truyen vao</param>
    /// <param name="Min">Thoi gian theo don vi phut</param>
    public void SetCacheWithTime(string key, object val, double Min)
    {
        lock (tobj)
        {
            if (_mcClient == null) LoadConfigCache();
            if (_mcClient != null)
            {
                string all_key = string.Empty;
                // kiem tra tat ca cac cachkey hien co
                if (key.IndexOf("GoNews") > -1)
                {
                    // find accountId in key
                    int idStart = key.IndexOf("GoNews");
                    int idEnd;
                    if (key.IndexOf("_", idStart) > -1) idEnd = key.IndexOf("_", idStart);
                    else idEnd = key.Length;
                    // day la danh sach tat ca cac cachkey theo account id
                    all_key = ALL_CACHE_KEYS + key.Substring(idStart, idEnd - idStart);
                    list_keys = (List<string>)_mcClient.Get(all_key);
                }
                else // truong hop nay la tat ca cachkey khong theo accountid
                {
                    all_key = ALL_CACHE_KEYS;
                    list_keys = (List<string>)_mcClient.Get(ALL_CACHE_KEYS);
                }
                if (list_keys == null) list_keys = new List<string>();
                if (val != null)
                {
                    _mcClient.Store(StoreMode.Set, key, val, DateTime.Now.AddMinutes(Min));
                    if (list_keys != null && !list_keys.Contains(key))
                    {
                        list_keys.Insert(0, key);
                        _mcClient.Store(StoreMode.Set, all_key, list_keys);
                    }
                }
            }
        }
    }

    #endregion ICacheUtil Members

    private void LoadConfigCache()
    {
        try
        {
            var configuration = new MemcachedClientConfiguration();
            string str = ConfigurationSettings.AppSettings["ServerCached"].ToString();
            string[] list = str.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string key in list)
            {
                configuration.Servers.Add(new IPEndPoint(IPAddress.Parse(ParseIPAddress(key)), ParseIPPort(key)));
            }

            configuration.NodeLocator = typeof(DefaultNodeLocator);
            //cuongpm comment
            //configuration.KeyTransformer = typeof(SHA1KeyTransformer);
            //configuration.Transcoder = typeof(DefaultTranscoder);
            configuration.SocketPool.MinPoolSize = 1;
            configuration.SocketPool.MaxPoolSize = 10;
            configuration.SocketPool.ConnectionTimeout = new TimeSpan(0, 0, 5);
            configuration.SocketPool.DeadTimeout = new TimeSpan(0, 0, 10);
            _mcClient = new MemcachedClient(configuration);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private string ParseIPAddress(string key)
    {
        if (key.IndexOf(":") > -1)
            return key.Substring(0, key.IndexOf(":"));
        else return key;
    }

    private int ParseIPPort(string key)
    {
        if (key.IndexOf(":") > -1)
            return Convert.ToInt32(key.Substring(key.IndexOf(":") + 1, key.Length - key.IndexOf(":") - 1));
        else return 11211;
    }
}