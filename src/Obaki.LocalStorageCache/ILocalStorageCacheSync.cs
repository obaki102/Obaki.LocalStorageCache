namespace Obaki.LocalStorageCache;

public  interface ILocalStorageCacheSync
{
    (bool isCacheExist, T? cacheData) TryGetCache<T>(string key);

    T? GetCache<T>(string key);

    void ClearCache(string key);

    void SetCache<T>(string key, T data);

    TimeSpan CacheExpiration { get; set; }
}
