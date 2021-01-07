using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Ads.Core.Clients.Stories;
using Trill.Modules.Ads.Core.Clients.Users;
using Trill.Modules.Ads.Core.Domain;
using Trill.Modules.Ads.Core.Exceptions;
using Trill.Modules.Ads.Core.Persistence;
using Trill.Shared.Infrastructure.Modules;

[assembly: InternalsVisibleTo("Trill.Modules.Ads.Api")]
namespace Trill.Modules.Ads.Core
{
    internal static class Extensions
    {
        internal static IServiceCollection AddCore(this IServiceCollection services)
        {
            services
                .AddExceptionToMessageMapper<AdsExceptionToMessageMapper>()
                .AddScoped<IAdRepository, AdRepository>()
                .AddScoped<IStoryApiClient, StoryApiClient>()
                .AddScoped<IUsersApiClient, UsersApiClient>();

            return services;
        }
    }
}