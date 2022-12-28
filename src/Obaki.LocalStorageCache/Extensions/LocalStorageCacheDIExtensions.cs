using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Obaki.LocalStorageCache.Extensions
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
            services.TryAddScoped<ILocalStorageCache,LocalStorageCache>();
            return services;
        }

        public static IServiceCollection AddLocalStorageCacheAsSingleton(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddBlazoredLocalStorageAsSingleton();
            services.TryAddSingleton<ILocalStorageCache, LocalStorageCache>();
            return services;
        }
    }
}
