using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obaki.LocalSorageCache
{
    public class LocalStorageCacheOptions
    {
        public required string DataKey { get; set; } = string.Empty;
        public int NumberOfHrsToRefreshCache { get; set; } = 1;
    }
}
