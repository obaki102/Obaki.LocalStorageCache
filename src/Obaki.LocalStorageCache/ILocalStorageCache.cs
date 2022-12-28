
using System.Threading.Tasks;

namespace Obaki.LocalStorageCache
{
    public interface ILocalStorageCache
    {
        ValueTask<(bool isCacheExist, T? cacheData)> TryGetCacheValue<T>(string key);

        ValueTask<T> GetCacheValue<T>(string key);

        ValueTask ClearCacheValue(string key);

        ValueTask SetCacheValue<T>(string key, T Data);

        int CacheExpirationHrs { get; set; }
    }
}
