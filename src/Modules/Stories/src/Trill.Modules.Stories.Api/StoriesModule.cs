using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Stories.Application;
using Trill.Modules.Stories.Application.Clients.Users.DTO;
using Trill.Modules.Stories.Application.Commands;
using Trill.Modules.Stories.Application.DTO;
using Trill.Modules.Stories.Application.Events.External;
using Trill.Modules.Stories.Application.Queries;
using Trill.Modules.Stories.Application.Services;
using Trill.Modules.Stories.Core;
using Trill.Modules.Stories.Infrastructure;
using Trill.Modules.Stories.Infrastructure.Clients.Users.Requests;
using Trill.Shared.Abstractions.Dispatchers;
using Trill.Shared.Abstractions.Queries;
using Trill.Shared.Infrastructure.Api;
using Trill.Shared.Infrastructure.Modules;

[assembly: InternalsVisibleTo("Trill.Bootstrapper")]
namespace Trill.Modules.Stories.Api
{
    internal class StoriesModule : IModule
    {
        public string Name { get; } = "Stories";
        public string Path { get; } = "stories-module";


        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCore()
                .AddApplication()
                .AddInfrastructure();
        }

        public void ConfigureMiddleware(IApplicationBuilder app)
        {
            app.UseContracts()
                .Register<UserCreated>()
                .RegisterPath<GetUser, UserDto>("users-module/get-user");

            app.UseInfrastructure();
            app.UseModuleRequests()
                .Subscribe<SendStory, SendStory.Response>($"{Path}/send-story", async cmd =>
                {
                    await app.ApplicationServices.GetRequiredService<IDispatcher>().SendAsync(cmd);
                    var storage = app.ApplicationServices.GetRequiredService<IStoryRequestStorage>();
                    var storyId = storage.GetStoryId(cmd.Id);
                    return new SendStory.Response(storyId);
                });
        }

        public void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(Path, ctx => ctx.Response.WriteAsync($"{Name} module"));
            endpoints
                .Get<BrowseStories, Paged<StoryDto>>($"{Path}/stories")
                .Get<GetStory, StoryDetailsDto>($"{Path}/stories/{{storyId}}")
                .Post<SendStory>($"{Path}/stories", after: (cmd, ctx) =>
                {
                    var storage = ctx.RequestServices.GetRequiredService<IStoryRequestStorage>();
                    var storyId = storage.GetStoryId(cmd.Id);
                    return ctx.Response.Created($"{Path}/stories/{storyId}");
                })
                .Post<RateStory>($"{Path}/stories/{{storyId}}/rate");
        }
    }
}