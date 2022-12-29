namespace Obaki.LocalStorageCache
{
    public static class LocalStorageCacheExtensions
    {
        public static async ValueTask<T?> GetOrCreateCacheAsync<T>(this ILocalStorageCache localCache, string key,
            Func<ILocalStorageCache, ValueTask<T>> creator)
        {
            var (isCacheExist, cacheData) = await localCache.TryGetCacheAsync<T>(key);

            if (isCacheExist)
            {
                if (cacheData is null)
                    return default;

                return cacheData;
            }

            var newCacheData = await creator(localCache).ConfigureAwait(false);
            await localCache.SetCacheAsync(key, newCacheData);

            return newCacheData;
        }

        public static ILocalStorageCache SetExpiration(this ILocalStorageCache localCache, TimeSpan cacheExpiration)
        {
            localCache.CacheExpiration = cacheExpiration;
            return localCache;
        }
    }
}
