namespace Obaki.LocalStorageCache
{
    public static class LocalStorageCacheExtensions
    {
        public static async ValueTask<T> GetOrCreateCacheAsync<T>(this ILocalStorageCache localCache, string key,
            Func<ILocalStorageCache, Task<T>> creator)
        {
            var result = await localCache.TryGetCacheValue<T>(key);
            if (result.isCacheExist)
            {
                if (result.cacheData is null)
                    throw new ArgumentNullException(nameof(result.cacheData), "Cache data is empty;");

                return result.cacheData;
            }

            var newCacheData = await creator(localCache).ConfigureAwait(false);
            await localCache.SetCacheValue(key, newCacheData);

            return newCacheData;
        }

        public static async ValueTask<T> GetCacheAsync<T>(this ILocalStorageCache localCache, string key)
        {
            return await localCache.GetCacheValue<T>(key);
        }

        public static async ValueTask ClearCacheAsync(this ILocalStorageCache localCache, string key)
        {
            await localCache.ClearCacheValue(key);
        }

        public static ILocalStorageCache SetExpirationHrs(this ILocalStorageCache localCache, int expirationHrs)
        {
            localCache.CacheExpirationHrs = expirationHrs;
            return localCache;
        }
    }
}
