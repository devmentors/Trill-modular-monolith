using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Stories.Core.Factories;
using Trill.Modules.Stories.Core.Services;

[assembly: InternalsVisibleTo("Trill.Modules.Stories.Api")]
[assembly: InternalsVisibleTo("Trill.Modules.Stories.Application")]
[assembly: InternalsVisibleTo("Trill.Modules.Stories.Infrastructure")]
[assembly: InternalsVisibleTo("Trill.Modules.Stories.Tests.Unit")]
[assembly: InternalsVisibleTo("Trill.Modules.Stories.Tests.Integration")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Trill.Modules.Stories.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
            => services
                .AddScoped<IStoryRatingService, StoryRatingService>()
                .AddSingleton<IStoryTextFactory, StoryTextFactory>();
    }
}