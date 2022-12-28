using Blazored.LocalStorage;

namespace Obaki.LocalStorageCache
{
    internal sealed class LocalStorageCache : ILocalStorageCache
    {

        private readonly ILocalStorageService _localStorageService;
        private int _cacheExpirationHrs;

        public LocalStorageCache(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public int CacheExpirationHrs
        {
            get => _cacheExpirationHrs;
            set => _cacheExpirationHrs = value;

        }

        public async Task ClearCacheValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Key is empty.");
            }

            await _localStorageService.RemoveItemAsync(key).ConfigureAwait(false);
        }

        public async Task<T> GetCacheValue<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Key is empty.");
            }

            var cacheData = await _localStorageService.GetItemAsync<CacheData<T>>(key).ConfigureAwait(false);

            if (cacheData is null || cacheData.Cache is null)
            {
                throw new ArgumentNullException(nameof(T), "Cache data is empty.");
            }

            return cacheData.Cache;
        }

        public async Task SetCacheValue<T>(string key, T data)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Key is empty.");
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(T), "Cache data is empty.");
            }

            var cacheData = new CacheData<T>
            {
                Cache = data,
            };

            await _localStorageService.SetItemAsync(key, cacheData).ConfigureAwait(false);
        }

        public async Task<(bool isCacheExist, T? cacheData)> TryGetCacheValue<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Key is empty.");
            }

            var cacheData = await _localStorageService.GetItemAsync<CacheData<T>>(key).ConfigureAwait(false);
            double totalHrsSinceCacheCreated = 0;

            if (cacheData is not null)
            {
                totalHrsSinceCacheCreated = DateTime.Now.Subtract(cacheData.Created).TotalHours;
            }

            if (cacheData is null || totalHrsSinceCacheCreated > CacheExpirationHrs)
            {
                return (false, default);
            }

            return (true, cacheData.Cache);
        }

    }
}
