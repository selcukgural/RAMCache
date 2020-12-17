using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RAMCache.Options;

namespace RAMCache.Extension
{
    public static class RAMCacheServiceCollectionExtensions
    {
        public static IServiceCollection AddRAMCache(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IRAMCache, RAMCache>());

            return services;
        }
        public static IServiceCollection AddRAMCache(this IServiceCollection services, Action<RAMCacheServiceOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddRAMCache();
            services.Configure(setupAction);

            return services;
        }
    }
}
