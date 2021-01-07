using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Trill.Modules.Stories.Application.Clients.Users;
using Trill.Modules.Stories.Application.Services;
using Trill.Modules.Stories.Core.Policies;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Modules.Stories.Infrastructure.Clients.Users;
using Trill.Modules.Stories.Infrastructure.Mongo;
using Trill.Modules.Stories.Infrastructure.Mongo.Documents;
using Trill.Modules.Stories.Infrastructure.Mongo.Repositories;
using Trill.Modules.Stories.Infrastructure.Services;
using Trill.Shared.Infrastructure.Mongo;

[assembly: InternalsVisibleTo("Trill.Modules.Stories.Api")]
[assembly: InternalsVisibleTo("Trill.Modules.Stories.Tests.Integration")]
namespace Trill.Modules.Stories.Infrastructure
{
    internal static class Extensions
    {
        private const string Schema = "stories-module";
        
        internal static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services
                .AddSingleton<IStoryTextPolicy, DefaultStoryTextPolicy>()
                .AddSingleton<IStoryRequestStorage, StoryRequestStorage>()
                .AddScoped<IUsersApiClient, UsersApiClient>()
                .AddScoped<IStoryRepository, StoryMongoRepository>()
                .AddScoped<IStoryRatingRepository, StoryRatingMongoRepository>()
                .AddScoped<IUserRepository, UserMongoRepository>()
                .AddMongoRepository<StoryDocument, long>($"{Schema}.stories")
                .AddMongoRepository<UserDocument, Guid>($"{Schema}.users");

            return services;
        }

        internal static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseMongo();

            return app;
        }
    }
}