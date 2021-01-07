using Microsoft.Extensions.DependencyInjection;
using Trill.Web.Core.Ads;
using Trill.Web.Core.Ads.Services;
using Trill.Web.Core.Analytics;
using Trill.Web.Core.Analytics.Services;
using Trill.Web.Core.Services;
using Trill.Web.Core.Storage;
using Trill.Web.Core.Stories;
using Trill.Web.Core.Stories.Services;
using Trill.Web.Core.Timelines;
using Trill.Web.Core.Timelines.Services;
using Trill.Web.Core.Users;
using Trill.Web.Core.Users.Services;

namespace Trill.Web.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IHttpClient, CustomHttpClient>();
            services.AddScoped<ILocalStorageService, LocalStorageService>();
            services.AddScoped<IAdsService, AdsService>();
            services.AddScoped<IAnalyticsService, AnalyticsService>();
            services.AddScoped<IStoriesService, StoriesService>();
            services.AddScoped<ITimelineService, TimelineService>();
            services.AddScoped<IUsersService, UsersService>();

            return services;
        }
    }
}