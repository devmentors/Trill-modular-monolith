using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Timeline.Core;
using Trill.Shared.Bootstrapper;
using Trill.Shared.Bootstrapper.Endpoints;

[assembly: InternalsVisibleTo("Trill.Api")]
namespace Trill.Modules.Timeline.Api
{
    internal class TimelineModule : IModule
    {
        public string Name { get; } = "Timeline";
        public string Path { get; } = "timeline-module";
        public string Schema { get; } = "timeline-module";


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCore();
        }

        public void ConfigureMiddleware(IApplicationBuilder app)
        {
        }

        public void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(Path, ctx => ctx.Response.WriteAsync($"{Name} module"));
            endpoints.MapGet($"{Path}/timelines/{{userId:guid}}", async context =>
            {
                var userId = context.Request.RouteValues["userId"].ToString();
                if (string.IsNullOrWhiteSpace(userId))
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    return;
                }

                var storage = context.RequestServices.GetRequiredService<IStorage>();
                var timeline = await storage.GetTimelineAsync(Guid.Parse(userId));
                await context.Response.WriteJsonAsync(timeline);
            });
        }
    }
}