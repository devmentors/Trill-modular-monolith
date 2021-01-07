using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Analytics.Core.Mongo;
using Trill.Modules.Analytics.Core.Services;

[assembly: InternalsVisibleTo("Trill.Modules.Analytics.Api")]
namespace Trill.Modules.Analytics.Core
{
    internal static class Extensions
    {
        internal static IServiceCollection AddCore(this IServiceCollection services)
        {
            services
                .AddScoped<IStoriesService, StoriesService>()
                .AddScoped<ITagsService, TagsService>()
                .AddScoped<ITrendingService, TrendingService>()
                .AddScoped<IUsersService, UsersService>()
                .AddScoped<IDatabaseProvider, DatabaseProvider>();

            return services;
        }

        internal static IApplicationBuilder UseCore(this IApplicationBuilder app)
        {
            app.UseMongo();

            return app;
        }
    }
}