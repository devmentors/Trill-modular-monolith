using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Timeline.Core.Persistence;

[assembly: InternalsVisibleTo("Trill.Modules.Timeline.Api")]
namespace Trill.Modules.Timeline.Core
{
    internal static class Extensions
    {
        internal static IServiceCollection AddCore(this IServiceCollection services)
        {
            services
                .AddScoped<IStorage, RedisStorage>();

            return services;
        }
    }
}