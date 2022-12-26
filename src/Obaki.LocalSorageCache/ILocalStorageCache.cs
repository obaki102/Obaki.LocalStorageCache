using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obaki.LocalSorageCache
{
    public interface ILocalStorageCache<T>
    {
        Task<T> GetCacheData();
        LocalStorageCacheOptions? Options { get; set; }
        Task<bool> IsCacheNeedsDataRefresh();
        Task SetData(T data);
        Task ClearCache();
    }
}
