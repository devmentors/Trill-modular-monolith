using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Users.Core;
using Trill.Modules.Users.Core.Commands;
using Trill.Modules.Users.Core.DTO;
using Trill.Modules.Users.Core.Queries;
using Trill.Modules.Users.Core.Services;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Queries;
using Trill.Shared.Bootstrapper;
using Trill.Shared.Bootstrapper.Endpoints;
using Trill.Shared.Infrastructure.Modules;

[assembly: InternalsVisibleTo("Trill.Api")]
namespace Trill.Modules.Users.Api
{
    internal class UsersModule : IModule
    {
        public string Name { get; } = "Users";
        public string Path { get; } = "users-module";
        public string Schema { get; } = "users-module";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCore();
        }

        public void ConfigureMiddleware(IApplicationBuilder app)
        {
            app.UseCore();
            app.UseModuleRequests()
                .Subscribe<GetUser, UserDetailsDto>($"{Path}/get-user", query =>
                    app.ApplicationServices.GetRequiredService<IDispatcher>().QueryAsync(query))
                .Subscribe<ChargeFunds, ChargeFunds.Response>($"{Path}/charge-funds", async cmd =>
                {
                    await app.ApplicationServices.GetRequiredService<IDispatcher>().SendAsync(cmd);
                    return new ChargeFunds.Response(true);
                });
        }

        public void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet(Path, ctx => ctx.Response.WriteAsync($"{Name} module"));
            endpoints
                .Post<SignIn>($"{Path}/sign-in", after: (cmd, ctx) =>
                {
                    var jwt = ctx.RequestServices.GetRequiredService<ITokenStorage>().Get(cmd.Id);
                    return ctx.Response.WriteJsonAsync(jwt);
                })
                .Post<SignUp>($"{Path}/sign-up",
                    after: (cmd, ctx) => ctx.Response.Created($"{Path}/users/{cmd.UserId}"))
                .Post<RevokeAccessToken>($"{Path}/access-tokens/revoke")
                .Post<UseRefreshToken>($"{Path}/refresh-tokens/use", after: (cmd, ctx) =>
                {
                    var jwt = ctx.RequestServices.GetRequiredService<ITokenStorage>().Get(cmd.Id);
                    return ctx.Response.WriteJsonAsync(jwt);
                })
                .Post<RevokeRefreshToken>($"{Path}/refresh-tokens/revoke")
                .Get<GetUser, UserDetailsDto>($"{Path}/users/{{userId:guid}}")
                .Get<BrowseUsers, Paged<UserDto>>($"{Path}/users")
                .Post<FollowUser>($"{Path}/users/{{userId:guid}}/following/{{followeeId:guid}}")
                .Delete<UnfollowUser>($"{Path}/users/{{userId:guid}}/following/{{followeeId:guid}}")
                .Put<LockUser>($"{Path}/users/{{userId:guid}}/lock")
                .Put<UnlockUser>($"{Path}/users/{{userId:guid}}/unlock")
                .Post<AddFunds>($"{Path}/users/{{userId:guid}}/funds")
                .Post<ChargeFunds>($"{Path}/users/{{userId:guid}}/funds/charge");
        }
    }
}