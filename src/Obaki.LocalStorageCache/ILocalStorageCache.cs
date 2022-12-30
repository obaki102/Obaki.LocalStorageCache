
using System.Threading.Tasks;

namespace Obaki.LocalStorageCache
{
    public interface ILocalStorageCache
    {
        ValueTask<(bool isCacheExist, T? cacheData)> TryGetCacheAsync<T>(string key);

        ValueTask<T?> GetCacheAsync<T>(string key);

        ValueTask ClearCacheAsync(string key);

        ValueTask SetCacheAsync<T>(string key, T data);

        TimeSpan CacheExpiration { get; set; }
    }
}
