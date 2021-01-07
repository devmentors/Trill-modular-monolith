using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Trill.Modules.Users.Core.Mongo.Documents;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Modules.Users.Core.Mongo
{
    internal static class Extensions
    {
        internal static IApplicationBuilder UseMongo(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();
            var users = scope.ServiceProvider.GetService<IMongoRepository<UserDocument, Guid>>().Collection;
            var userBuilder = Builders<UserDocument>.IndexKeys;
            Task.Run(async () => await users.Indexes.CreateManyAsync(
                new[]
                {
                    new CreateIndexModel<UserDocument>(userBuilder.Ascending(i => i.Email),
                        new CreateIndexOptions
                        {
                            Unique = true
                        }),
                    new CreateIndexModel<UserDocument>(userBuilder.Ascending(i => i.Name),
                        new CreateIndexOptions
                        {
                            Unique = true
                        })
                }));

            var followers = scope.ServiceProvider.GetService<IMongoRepository<FollowerDocument, Guid>>().Collection;
            var followerBuilder = Builders<FollowerDocument>.IndexKeys;
            Task.Run(async () => await followers.Indexes.CreateManyAsync(
                new[]
                {
                    new CreateIndexModel<FollowerDocument>(
                        followerBuilder.Ascending(i => i.FollowerId).Descending(i => i.FolloweeId),
                        new CreateIndexOptions
                        {
                            Unique = true
                        }),
                }));

            return builder;
        }
    }
}