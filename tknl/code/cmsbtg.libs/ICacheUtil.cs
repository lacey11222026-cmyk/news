public interface ICacheUtil
{
    void RemoveByGroup(params string[] keys);

    /// <summary>
    /// Remove cache group by name
    /// </summary>
    /// <param name="CacheGroupName"></param>
    void RemoveByCacheGroupName(string CacheGroupName);

    void RemoveByCacheGroupName(string CacheGroupName, string pAccount);

    void Remove(string cachKey);

    void RemoveAll();

    bool CheckCache(string key);

    object GetCache(string key);

    /// <summary>
    /// Dat cache cho 1 object voi thoi gian song 3 phut.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="val"></param>
    void SetCache(string key, object val);

    /// <summary>
    /// Set cache truc tiep (khong xu ly viec phan vung cache)
    /// </summary>
    /// <param name="key">cache key</param>
    /// <param name="val">cache value</param>
    void Set(string key, object val);

    /// <summary>
    /// Dat Cache voi thoi gian song cho cache.
    /// </summary>
    /// <param name="key">Cache Key</param>
    /// <param name="val">Doi tuong truyen vao</param>
    /// <param name="Min">Thoi gian theo don vi phut</param>
    void SetCacheWithTime(string key, object val, double Min);
}