using System;
using System.Collections;
using System.Web;

/// <summary>
/// Summary description for Class1
/// </summary>
public class CacheUtilWeb : ICacheUtil
{
    private static readonly object tobj = new object();
    private static readonly CacheUtilWeb instance = new CacheUtilWeb();

    private CacheUtilWeb()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static CacheUtilWeb Instance
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
    public void RemoveByCacheGroupName(string CacheGroupName)
    {
        string key = "";
        foreach (DictionaryEntry entry in HttpContext.Current.Cache)
        {
            key = (string) entry.Key;
            if (key.IndexOf(CacheGroupName) > -1) HttpContext.Current.Cache.Remove(key);
        }
    }

    /// <summary>
    /// Remove cache group by name and account
    /// </summary>
    /// <param name="CacheGroupName"></param>
    public  void RemoveByCacheGroupName(string cacheGroupName, string pAccount)
    {
        string key = "";
        foreach (DictionaryEntry entry in HttpContext.Current.Cache)
        {
            key = (string) entry.Key;
            if (key.IndexOf(cacheGroupName) > -1 && key.IndexOf(pAccount) > -1)
                HttpContext.Current.Cache.Remove(key);
        }
    }

    public void Remove(string cacheKey)
    {
        lock (tobj)
        {
            HttpContext.Current.Cache.Remove(cacheKey);
        }
    }
    public static void RemoveUrl(string url)
    {
        try
        {
            new System.Net.WebClient().DownloadData(url);
        }
        catch {
        }
         
    }
    public void RemoveAll()
    {
        lock (tobj)
        {
            IDictionaryEnumerator ce = HttpRuntime.Cache.GetEnumerator();
            while (ce.MoveNext()) HttpRuntime.Cache.Remove(ce.Key as string);
        }
    }

    public bool CheckCache(string key)
    {
        lock (tobj)
        {
            return (HttpRuntime.Cache[key] != null);
        }
    }

    public object GetCache(string key)
    {
        lock (tobj)
        {
            return HttpRuntime.Cache[key];
        }
    }

    /// <summary>
    /// Dat cache cho 1 object voi thoi gian song 3 phut.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="val"></param>
    public void SetCache(string key, object val)
    {
        //HttpRuntime.Cache[key] = val;
        SetCacheWithTime(key, val, 5);
    }

    /// <summary>
    /// Set cache truc tiep (khong xu ly viec phan vung cache)
    /// </summary>
    /// <param name="key"></param>
    /// <param name="val"></param>
    public void Set(string key, object val)
    {
        //HttpRuntime.Cache[key] = val;
        SetCacheWithTime(key, val, 5);
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
            HttpRuntime.Cache.Remove(key);
            if (val != null)
                HttpRuntime.Cache.Insert(key, val, null, DateTime.Now.AddMinutes(Min), TimeSpan.Zero);
        }
    }

    #endregion
}