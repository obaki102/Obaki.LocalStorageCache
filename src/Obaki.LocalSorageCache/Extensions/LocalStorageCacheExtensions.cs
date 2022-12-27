
namespace Obaki.LocalSorageCache.Extensions
{
    public static class LocalStorageCacheExtensions
    {
        public static async Task<T> GetOrCreateCacheAsync<T>(this ILocalStorageCache localCache, string key,
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
            await localCache.SetData(key, newCacheData);

            return newCacheData;

        }

        public static ILocalStorageCache SetExpirationHrs(this ILocalStorageCache localCache, int expirationHrs)
        {
            localCache.CacheExpirationHrs = expirationHrs;
            return localCache;
        }
    }
}
