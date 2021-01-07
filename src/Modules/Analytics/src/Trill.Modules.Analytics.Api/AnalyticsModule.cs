using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Analytics.Core;
using Trill.Shared.Bootstrapper;

[assembly: InternalsVisibleTo("Trill.Api")]
namespace Trill.Modules.Analytics.Api
{
    internal class AnalyticsModule : IModule
    {
        public string Name { get; } = "Analytics";
        public string Path { get; } = "analytics-module";
        public string Schema { get; } = "analytics-module";

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