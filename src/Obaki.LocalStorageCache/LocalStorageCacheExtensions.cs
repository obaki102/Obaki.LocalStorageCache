namespace Obaki.LocalStorageCache
{
    public static class LocalStorageCacheExtensions
    {
        public static async ValueTask<T?> GetOrCreateCacheAsync<T>(this ILocalStorageCache localStorageCache, string key, TimeSpan cacheExpiration,
            Func<ValueTask<T>> cacheGenerator)
        {
            localStorageCache.CacheExpiration = cacheExpiration;
            var (isCacheExist, cacheData) = await localStorageCache.TryGetCacheAsync<T>(key);

            if (isCacheExist)
                return cacheData;

            var newCacheData = await cacheGenerator().ConfigureAwait(false);
            await localStorageCache.SetCacheAsync(key, newCacheData);

            return newCacheData;
        }

        public static T? GetOrCreateCache<T>(this ILocalStorageCacheSync localStorageCache, string key, TimeSpan cacheExpiration,
            Func<T> cacheGenerator)
        {
            localStorageCache.CacheExpiration = cacheExpiration;
            var (isCacheExist, cacheData) =  localStorageCache.TryGetCache<T>(key);

            if (isCacheExist)
                return cacheData;

            var newCacheData =  cacheGenerator();
            localStorageCache.SetCache(key, newCacheData);

            return newCacheData;
        }
    }
}
