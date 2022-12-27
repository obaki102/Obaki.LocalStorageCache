
using System.Threading.Tasks;

namespace Obaki.LocalSorageCache
{
    public interface ILocalStorageCache
    {
        Task<(bool isCacheExist, T? cacheData)> TryGetCacheValue<T>(string key);
        Task SetData<T>(string key, T Data);
        int CacheExpirationHrs { get; set; }
    }
}
