using Microsoft.Extensions.DependencyInjection;
using YunHu.Lib.Cache.Abstractions;

namespace YunHu.Lib.Cache.Redis
{
    public class ServiceCollectionConfig : IServiceCollectionConfig
    {
        public IServiceCollection Config(IServiceCollection services, CacheOptions options)
        {
            var redisHelper = new RedisHelper(options.Redis);
            services.AddSingleton(redisHelper);
            services.AddSingleton<ICacheHandler, RedisCacheHandler>();

            return services;
        }
    }
}
