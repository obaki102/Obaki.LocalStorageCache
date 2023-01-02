namespace Obaki.LocalStorageCache;

public  interface ILocalStorageCacheSync
{
    bool TryGetCache<T>(string key, out T?  cacheData);

    T? GetCache<T>(string key);

    void ClearCache(string key);

    void SetCache<T>(string key, T data);

    TimeSpan CacheExpiration { get; set; }
}
