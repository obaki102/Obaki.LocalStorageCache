using Blazored.LocalStorage;

namespace Obaki.LocalSorageCache
{
    public class LocalStorageCache<T> : ILocalStorageCache<T> where T : class
    {
        private readonly ILocalStorageService _localStorageService;
        public LocalStorageCache(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }
        private T? Data { get; set; }
        public LocalStorageCacheOptions? Options { get; set; }
        private bool perfromRefreshData { get; set; } = false;
        public async Task<T> GetCacheData()
        {
            if (Options is null)
            {
                throw new ArgumentNullException($"{nameof(Options)} is null.");
            }

            if (!perfromRefreshData)
            {
                var cacheData = await _localStorageService.GetItemAsync<CacheData<T>>(Options.DataKey).ConfigureAwait(false);
                if (cacheData is not null)
                {
                    Data = cacheData.Content;
                }
            }

            if (Data is not null)
            {
                return Data;
            }

            throw new ArgumentNullException($"{nameof(ILocalStorageService)} is null.");
        }

        //todo: Check how can data refresh  happen inside LocalStorageCache
        public async Task<bool> IsCacheNeedsDataRefresh()
        {
            if (Options is null)
            {
                throw new ArgumentNullException($"{nameof(Options)} is null.");
            }

            var cacheData = await _localStorageService.GetItemAsync<CacheData<T>>(Options.DataKey).ConfigureAwait(false);
            double totalHrsSinceCacheCreated = 0;

            if (cacheData is not null)
            {
                totalHrsSinceCacheCreated = DateTime.Now.Subtract(cacheData.Created).TotalHours;
            }

            if (cacheData is null || totalHrsSinceCacheCreated > Options.NumberOfHrsToRefreshCache)
            {
                perfromRefreshData = true;
                return true;
            }

            return false;
        }

        public async Task ClearCache()
        {
            if (Options is null)
            {
                throw new ArgumentNullException($"{nameof(Options)} is null.");
            }

            await _localStorageService.RemoveItemAsync(Options.DataKey).ConfigureAwait(false);
        }

        public async Task SetData(T data)
        {
            if (Options is null)
            {
                throw new ArgumentNullException($"{nameof(Options)} is null.");
            }

            Data = data;
            var updatedCacheData = new CacheData<T>
            {
                Content = data,
            };
            await _localStorageService.SetItemAsync<CacheData<T>>(Options.DataKey, updatedCacheData).ConfigureAwait(false);
        }


    }
}
