using Blazored.LocalStorage;

namespace Obaki.LocalStorageCache
{
    internal sealed class LocalStorageCacheProvider : ILocalStorageCache
    {

        private readonly ILocalStorageService _localStorageService;
        private TimeSpan _cacheExpiration;

        public LocalStorageCacheProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public TimeSpan CacheExpiration
        {
            get => _cacheExpiration;
            set => _cacheExpiration = value;

        }

        public async ValueTask ClearCacheValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Key is empty.");
            }

            await _localStorageService.RemoveItemAsync(key).ConfigureAwait(false);
        }

        public async ValueTask<T?> GetCacheValue<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Key is empty.");
            }

            var cacheData = await _localStorageService.GetItemAsync<CacheData<T>>(key).ConfigureAwait(false);

            if (cacheData is null || cacheData.Cache is null)
            {
                return default;
            }

            return cacheData.Cache;
        }

        public async ValueTask SetCacheValue<T>(string key, T data)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Key is empty.");
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(T), "Cache data is empty.");
            }

            var cacheData = new CacheData<T>(data);

            await _localStorageService.SetItemAsync(key, cacheData).ConfigureAwait(false);
        }

        public async ValueTask<(bool isCacheExist, T? cacheData)> TryGetCacheValue<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Key is empty.");
            }

            var cacheData = await _localStorageService.GetItemAsync<CacheData<T>>(key).ConfigureAwait(false);

            if (cacheData is null)
            {
                return (false, default);
            }

            if ((DateTime.UtcNow - cacheData.Created) > _cacheExpiration)
            {
                return (false, default);
            }

            return (true, cacheData.Cache);
        }
    }
}
