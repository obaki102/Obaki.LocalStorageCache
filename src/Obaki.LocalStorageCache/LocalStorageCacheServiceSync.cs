using Blazored.LocalStorage;

namespace Obaki.LocalStorageCache
{
    internal sealed class LocalStorageCacheServiceSync : ILocalStorageCacheSync
    {

        private readonly ISyncLocalStorageService _localSyncStorageService;
        private TimeSpan _cacheExpiration;

        public LocalStorageCacheServiceSync(ISyncLocalStorageService localSyncStorageService)
        {
            _localSyncStorageService = localSyncStorageService ?? throw new ArgumentNullException(nameof(localSyncStorageService));
        }

        public TimeSpan CacheExpiration
        {
            get => _cacheExpiration;
            set => _cacheExpiration = value;
        }

        public void ClearCache(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            _localSyncStorageService.RemoveItem(key);
        }

        public T? GetCache<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var cacheData = _localSyncStorageService.GetItem<CacheData<T>>(key);

            if (cacheData is null || cacheData.Cache is null)
                return default;

            return cacheData.Cache;
        }

        public void SetCache<T>(string key, T data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (data is null)
                throw new ArgumentNullException(nameof(data));

            var cacheData = new CacheData<T>(data);

            _localSyncStorageService.SetItem(key, cacheData);
        }

        public bool TryGetCache<T>(string key, out T? cacheData)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

             var currentCache = _localSyncStorageService.GetItem<CacheData<T>>(key);

            if (currentCache is null || (DateTime.UtcNow - currentCache.Created) > _cacheExpiration)
            {
                cacheData = default;
                return false;
            }

            cacheData = currentCache.Cache;
            return true;
        }
    }
}
