using Microsoft.Extensions.Options;

namespace Obaki.LocalStorageCache
{
    public class LocalStorageCacheOptions : IOptions<LocalStorageCacheOptions>
    {
        //Not implemented as of yet
        public required string DataKey { get; set; } = string.Empty;
        public int NumberOfHrsToRefreshCache { get; set; } = 1;
        public LocalStorageCacheOptions Value =>  this;
    }
}
