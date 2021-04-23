using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Trill.Modules.Stories.Api")]
[assembly: InternalsVisibleTo("Trill.Modules.Stories.Infrastructure")]
[assembly: InternalsVisibleTo("Trill.Modules.Stories.Tests.Integration")]
[assembly: InternalsVisibleTo("Trill.Modules.Stories.Tests.Unit")]
[assembly: InternalsVisibleTo("Trill.Tests.EndToEnd")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Trill.Modules.Stories.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
            => services;
    }
}