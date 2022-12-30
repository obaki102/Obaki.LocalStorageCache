using Blazored.LocalStorage;
using Microsoft.AspNetCore.DataProtection;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Obaki.LocalStorageCache
{
    internal sealed class LocalStorageCacheProvider : ILocalStorageCache
    {

        private readonly ILocalStorageService _localStorageService;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly ConcurrentDictionary<string, IDataProtector> _cachedDataProtectorsByPurpose
        = new ConcurrentDictionary<string, IDataProtector>(StringComparer.Ordinal);
        private TimeSpan _cacheExpiration;

        public LocalStorageCacheProvider(ILocalStorageService localStorageService, IDataProtectionProvider dataProtectionProvider)
        {
            _localStorageService = localStorageService;
            _dataProtectionProvider = dataProtectionProvider;
        }

        public TimeSpan CacheExpiration
        {
            get => _cacheExpiration;
            set => _cacheExpiration = value;
        }

        public async ValueTask ClearCacheAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            await _localStorageService.RemoveItemAsync(key).ConfigureAwait(false);
        }

        public async ValueTask<T?> GetCacheAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var cacheData = await _localStorageService.GetItemAsync<CacheData<T>>(key).ConfigureAwait(false);

            if (cacheData is null || cacheData.Cache is null)
                return default;

            return cacheData.Cache;
        }

        public async ValueTask<T?> GetProtectedCacheAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (!IsKeyValid(key))
                throw new ArgumentException($"Invalid '{key}'. Please make sure the key is intended for this protected cache.");

            var cacheAsString = await _localStorageService.GetItemAsStringAsync(key).ConfigureAwait(false);

            if (string.IsNullOrEmpty(cacheAsString))
                return default;
        
            var cacheData = Unprotect<CacheData<T>>(CreatePurposeFromKey(key), cacheAsString);

            return cacheData.Cache;
        }

        public async ValueTask SetCacheAsync<T>(string key, T data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

         
            if (data is null)
                throw new ArgumentNullException(nameof(T));

            var cacheData = new CacheData<T>(data);

            await _localStorageService.SetItemAsync(key, cacheData).ConfigureAwait(false);
        }

        public async ValueTask SetProtectedCacheAsync<T>(string key, T data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (data is null)
                throw new ArgumentNullException(nameof(T));

            var cacheData = new CacheData<T>(data, true);
        
            var protectedCacheData = Protect(CreatePurposeFromKey(key), cacheData);

            await _localStorageService.SetItemAsStringAsync(key, protectedCacheData).ConfigureAwait(false);

        }

        public async ValueTask<(bool isCacheExist, T? cacheData)> TryGetCacheAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var cacheData = await _localStorageService.GetItemAsync<CacheData<T>>(key).ConfigureAwait(false);

                return (false, default);

            return (true, cacheData.Cache);
        }

        public async ValueTask<(bool isCacheExist, T? cacheData)> TryGetProtectedCacheAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            if (!IsKeyValid(key))
                return (false, default);

            var cacheAsString = await _localStorageService.GetItemAsStringAsync(key).ConfigureAwait(false);
           
            var cacheData = Unprotect<CacheData<T>>(CreatePurposeFromKey(key), cacheAsString);
            
            if (cacheData is null || (DateTime.UtcNow - cacheData.Created) > _cacheExpiration)
            {

            return (true, cacheData.Cache);
        }
            }

        private string Protect(string purpose, object value)
        {
            var json = JsonSerializer.Serialize(value);
            var protector = GetOrCreateCachedProtector(purpose);

            return protector.Protect(json);
        }

        private T Unprotect<T>(string purpose, string protectedJson)
        {
            var protector = GetOrCreateCachedProtector(purpose);
            var json = protector.Unprotect(protectedJson);

            return JsonSerializer.Deserialize<T>(json)!;
        }

        private bool IsKeyValid(string key)
           => _cachedDataProtectorsByPurpose.ContainsKey(CreatePurposeFromKey(key));

        private IDataProtector GetOrCreateCachedProtector(string purpose)
        => _cachedDataProtectorsByPurpose.GetOrAdd(
            purpose,
            _dataProtectionProvider.CreateProtector);

        private string CreatePurposeFromKey(string key)
       => $"{GetType().FullName}:{key}";
    }
}
