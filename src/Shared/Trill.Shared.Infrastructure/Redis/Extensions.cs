using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Trill.Shared.Infrastructure.Redis
{
    internal static class Extensions
    {
        private const string SectionName = "redis";

        public static IServiceCollection AddRedis(this IServiceCollection services, string sectionName = SectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                sectionName = SectionName;
            }

            var options = services.GetOptions<RedisOptions>(sectionName);

            services
                .AddSingleton(options)
                .AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(options.ConnectionString))
                .AddTransient(sp => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase(options.Database))
                .AddStackExchangeRedisCache(o =>
                {
                    o.Configuration = options.ConnectionString;
                    o.InstanceName = options.Instance;
                });

            return services;
        }
    }
}