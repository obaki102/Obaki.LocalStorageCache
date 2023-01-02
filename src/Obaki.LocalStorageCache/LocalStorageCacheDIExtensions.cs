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
            services.TryAddScoped<ILocalStorageCache, LocalStorageCacheServiceAsync>();
            services.TryAddScoped<ILocalStorageCacheSync, LocalStorageCacheServiceSync>();
            return services;
        }

        public static IServiceCollection AddLocalStorageCacheAsSingleton(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddBlazoredLocalStorageAsSingleton();
            services.TryAddSingleton<ILocalStorageCache, LocalStorageCacheServiceAsync>();
            services.TryAddSingleton<ILocalStorageCacheSync, LocalStorageCacheServiceSync>();
            return services;
        }
    }
}
