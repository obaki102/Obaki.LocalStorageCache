using System;

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
            {
                if (cacheData is null)
                    return default;

                return cacheData;
            }

            var newCacheData = await cacheGenerator().ConfigureAwait(false);
            await localStorageCache.SetCacheAsync(key, newCacheData);

            return newCacheData;
        }

        public static async ValueTask<T?> GetOrCreateProtectedCacheAsync<T>(this ILocalStorageCache localStorageCache, string key, TimeSpan cacheExpiration,
            Func<ValueTask<T>> cacheGenerator)
        {
            localStorageCache.CacheExpiration = cacheExpiration;
            var (isCacheExist, cacheData) = await localStorageCache.TryGetProtectedCacheAsync<T>(key);

            if (isCacheExist)
            {
                if (cacheData is null)
                    return default;

                return cacheData;
            }

            var newCacheData = await cacheGenerator().ConfigureAwait(false);
            await localStorageCache.SetProtectedCacheAsync(key, newCacheData);

            return newCacheData;
        }
    }
}
