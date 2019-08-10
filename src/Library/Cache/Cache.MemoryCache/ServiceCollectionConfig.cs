using Microsoft.Extensions.DependencyInjection;
using YunHu.Lib.Cache.Abstractions;

namespace YunHu.Lib.Cache.MemoryCache
{
    public class ServiceCollectionConfig : IServiceCollectionConfig
    {
        public IServiceCollection Config(IServiceCollection services, CacheOptions options)
        {
            services.AddMemoryCache();

            services.AddSingleton<ICacheHandler, MemoryCacheHandler>();

            return services;
        }
    }
}
