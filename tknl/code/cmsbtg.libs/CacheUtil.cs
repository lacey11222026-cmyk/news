using System.Configuration;
using cms.libs;

/// <summary>
/// Summary description for Class1
/// </summary>
public class CacheUtil
{
    private static bool IsEnable
    {
        get
        {
            bool isenable = false;

            if (ConfigurationSettings.AppSettings["EnableCached"] != null)
            {
                if (ConfigurationSettings.AppSettings["EnableCached"].ToString() == "True")
                {
                    isenable = true;
                }
            }

            return isenable;
        }
    }

    /// <summary>
    /// Provider cung cấp các thư viện xử lý cached khác nhau (Memcached, Enyim, Web)
    /// </summary>
    ///
    private static ICacheUtil CacheProvider
    {
        get
        {
            ICacheUtil cacheUtil;
            if (ConfigurationSettings.AppSettings["CachedProvider"] != null)
            {
                if (ConfigurationSettings.AppSettings["CachedProvider"] == "Enyim")
                    cacheUtil = CacheUtilEnyim.Instance;
                //else if (ConfigurationSettings.AppSettings["CachedProvider"] == "MemoryCache")
                //    cacheUtil = CacheUtilMemory.Instance;
                else cacheUtil = CacheUtilWeb.Instance;
            }
            else cacheUtil = CacheUtilWeb.Instance;
            return cacheUtil;
        }
    }

    /// <summary>
    /// Xoa cached theo một nhóm key (hiện tại không phù hợp với memcached)
    /// </summary>
    /// <param name="keys"></param>
    public static void RemoveByGroup(params string[] keys)
    {
        if (IsEnable)
            CacheProvider.RemoveByGroup(keys);
    }

    /// <summary>
    /// Xóa cache theo nhóm đối tượng
    /// </summary>
    /// <param name="CacheGroupName">format "list_" + typeof(entity).Name</param>
    public static void RemoveByCacheGroupName(string cacheGroupName)
    {
        if (IsEnable)
        {
            CacheProvider.RemoveByCacheGroupName(cacheGroupName);


        }
        else
        {

        }
    }

    /// <summary>
    /// Xóa cache theo AccountId và nhóm đối tượng
    /// </summary>
    /// <param name="cacheGroupName">format "list_" + typeof(entity).Name</param>
    /// <param name="pAccount">format "AccountId" + AccountId.ToString()</param>
    public static void RemoveByCacheGroupName(string cacheGroupName, string pAccount)
    {
        if (IsEnable)
            CacheProvider.RemoveByCacheGroupName(cacheGroupName, pAccount);
    }

    /// <summary>
    /// Xóa cache theo 1 key xác định
    /// </summary>
    /// <param name="cacheKey">Cache key</param>
    public static void Remove(string cacheKey)
    {
        if (IsEnable)
            CacheProvider.Remove(cacheKey);
    }
    
    /// <summary>
    /// Xóa toàn bộ cache
    /// </summary>
    public static void RemoveAll()
    {
        if (IsEnable)
            CacheProvider.RemoveAll();
    }

    /// <summary>
    /// Kiểm tra tồn tại cache
    /// </summary>
    /// <param name="key">cache key</param>
    /// <returns>true/flase</returns>
    public static bool CheckCache(string key)
    {
        if (IsEnable)
            return CacheProvider.CheckCache(key);
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Lấy dữ liệu từ cache
    /// </summary>
    /// <param name="key">cache key</param>
    /// <returns>object cached</returns>
    public static object GetCache(string key)
    {
        if (IsEnable)
            return CacheProvider.GetCache(key);
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Set cache
    /// </summary>
    /// <param name="key">cache ket</param>
    /// <param name="val">giá trị</param>
    public static void SetCache(string key, object val)
    {
        if (IsEnable)
            CacheProvider.SetCache(key, val);
    }

    /// <summary>
    /// Set cache truc tiep (khong xu ly viec phan vung cache)
    /// </summary>
    /// <param name="key">cache ket</param>
    /// <param name="val">giá trị</param>
    public static void Set(string key, object val)
    {
        if (IsEnable)
            CacheProvider.Set(key, val);
    }

    /// <summary>
    /// Đặt Cache với thời gian tồn tại.
    /// </summary>
    /// <param name="key">cache key</param>
    /// <param name="val">giá trị</param>
    /// <param name="Min">thời gian (phút)</param>
    public static void SetCacheWithTime(string key, object val, double min)
    {
        if (IsEnable)
            CacheProvider.SetCacheWithTime(key, val, min);
    }
}