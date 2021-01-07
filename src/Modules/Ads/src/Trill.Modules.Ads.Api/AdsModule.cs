using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Ads.Core;
using Trill.Modules.Ads.Core.Clients.Stories.Requests;
using Trill.Modules.Ads.Core.Clients.Users.Requests;
using Trill.Shared.Bootstrapper;
using Trill.Shared.Infrastructure.Modules;

[assembly: InternalsVisibleTo("Trill.Api")]
namespace Trill.Modules.Ads.Api
{
    internal class AdsModule : IModule
    {
        public string Name { get; } = "Ads";
        public string Path { get; } = "ads-module";
        public string Schema { get; } = "ads-module";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCore();
        }

        public void ConfigureMiddleware(IApplicationBuilder app)
        {
            app.UseContracts()
                .RegisterPath<ChargeFunds, ChargeFunds.Response>("users-module/charge-funds")
                .RegisterPath<SendStory, SendStory.Response>("stories-module/send-story");
        }

        public void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(Path, ctx => ctx.Response.WriteAsync($"{Name} module"));
        }
    }
}