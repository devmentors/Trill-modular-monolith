using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Stories.Application;
using Trill.Modules.Stories.Application.Commands;
using Trill.Modules.Stories.Application.Services;
using Trill.Modules.Stories.Core;
using Trill.Modules.Stories.Infrastructure;
using Trill.Shared.Infrastructure.Api;
using Trill.Shared.Infrastructure.Modules;

[assembly: InternalsVisibleTo("Trill.Bootstrapper")]
namespace Trill.Modules.Stories.Api
{
    internal class StoriesModule : IModule
    {
        public const string BasePath = "stories-module";
        public string Name { get; } = "Stories";
        public string Path => BasePath;

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCore()
                .AddApplication()
                .AddInfrastructure();
        }

        public void ConfigureMiddleware(IApplicationBuilder app)
        {
            app.UseInfrastructure();
        }

        public void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(Path, ctx => ctx.Response.WriteAsync($"{Name} module"));

            endpoints.Post<SendStory>($"{Path}/stories", after: (cmd, ctx) =>
            {
                var storage = ctx.RequestServices.GetRequiredService<IStoryRequestStorage>();
                var storyId = storage.GetStoryId(cmd.Id);
                return ctx.Response.Created($"{Path}/stories/{storyId}");
            });
        }
    }
}