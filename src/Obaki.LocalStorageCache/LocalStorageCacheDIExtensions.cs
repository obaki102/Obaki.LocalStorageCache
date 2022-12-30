using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Obaki.LocalStorageCache
{
    public static class LocalStorageCacheDIExtensions
    {
        public static IServiceCollection AddLocalStorageCacheAsScoped(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddBlazoredLocalStorage();
            services.TryAddScoped<ILocalStorageCache, LocalStorageCacheProvider>();
            return services;
        }
    }
}
