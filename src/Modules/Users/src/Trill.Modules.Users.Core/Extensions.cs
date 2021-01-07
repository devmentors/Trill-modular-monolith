using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Users.Core.Domain.Repositories;
using Trill.Modules.Users.Core.Exceptions;
using Trill.Modules.Users.Core.Mongo;
using Trill.Modules.Users.Core.Mongo.Documents;
using Trill.Modules.Users.Core.Mongo.Repositories;
using Trill.Modules.Users.Core.Services;
using Trill.Shared.Infrastructure.Modules;
using Trill.Shared.Infrastructure.Mongo;

[assembly: InternalsVisibleTo("Trill.Modules.Users.Api")]
[assembly: InternalsVisibleTo("Trill.Modules.Users.Tests.Unit")]
[assembly: InternalsVisibleTo("Trill.Modules.Users.Tests.Integration")]
[assembly: InternalsVisibleTo("Trill.Tests.EndToEnd")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Trill.Modules.Users.Core
{
    internal static class Extensions
    {
        private const string Schema = "users-module";

        internal static IServiceCollection AddCore(this IServiceCollection services)
        {
            services
                .AddExceptionToMessageMapper<UsersExceptionToMessageMapper>()
                .AddSingleton<IJwtProvider, JwtProvider>()
                .AddSingleton<IPasswordService, PasswordService>()
                .AddSingleton<IPasswordHasher<IPasswordService>, PasswordHasher<IPasswordService>>()
                .AddScoped<IFollowerRepository, FollowerRepository>()
                .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddSingleton<ITokenStorage, TokenStorage>()
                .AddMongoRepository<FollowerDocument, Guid>($"{Schema}.followers")
                .AddMongoRepository<RefreshTokenDocument, Guid>($"{Schema}.refreshTokens")
                .AddMongoRepository<UserDocument, Guid>($"{Schema}.users");

            return services;
        }

        internal static IApplicationBuilder UseCore(this IApplicationBuilder app)
        {
            app.UseMongo();

            return app;
        }
    }
}