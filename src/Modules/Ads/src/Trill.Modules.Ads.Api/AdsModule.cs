using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Ads.Core;
using Trill.Modules.Ads.Core.Clients.Stories.Requests;
using Trill.Modules.Ads.Core.Clients.Users.Requests;
using Trill.Shared.Infrastructure.Modules;

[assembly: InternalsVisibleTo("Trill.Bootstrapper")]
namespace Trill.Modules.Ads.Api
{
    internal class AdsModule : IModule
    {
        public const string BasePath = "ads-module";
        public string Name { get; } = "Ads";
        public string Path => BasePath;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCore();
        }

        public void ConfigureMiddleware(IApplicationBuilder app)
        {
            app.UseContracts()
                .RegisterPath<ChargeFunds, ChargeFunds.Response>("users-module/charge-funds");
        }

        public void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(Path, ctx => ctx.Response.WriteAsync($"{Name} module"));
        }
    }
}