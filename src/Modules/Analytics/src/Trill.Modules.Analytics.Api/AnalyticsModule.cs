using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Analytics.Core;
using Trill.Shared.Infrastructure.Modules;

[assembly: InternalsVisibleTo("Trill.Bootstrapper")]
namespace Trill.Modules.Analytics.Api
{
    internal class AnalyticsModule : IModule
    {
        public const string BasePath = "analytics-module";
        public string Name { get; } = "Analytics";
        public string Path => BasePath;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCore();
        }

        public void ConfigureMiddleware(IApplicationBuilder app)
        {
            app.UseCore();
        }

        public void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(Path, ctx => ctx.Response.WriteAsync($"{Name} module"));
        }
    }
}