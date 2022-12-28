
using System.Threading.Tasks;

namespace Obaki.LocalStorageCache
{
    public interface ILocalStorageCache
    {
        Task<(bool isCacheExist, T? cacheData)> TryGetCacheValue<T>(string key);

        Task<T> GetCacheValue<T>(string key);

        Task ClearCacheValue(string key);

        Task SetCacheValue<T>(string key, T Data);

        int CacheExpirationHrs { get; set; }
    }
}
