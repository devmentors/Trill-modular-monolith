using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Trill.Modules.Stories.Infrastructure.Mongo.Documents;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Modules.Stories.Infrastructure.Mongo
{
    internal static class Extensions
    {
        private const string Schema = "stories-module";
        
        internal static IApplicationBuilder UseMongo(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();
            var users = scope.ServiceProvider.GetService<IMongoRepository<UserDocument, Guid>>().Collection;
            var userBuilder = Builders<UserDocument>.IndexKeys;
            Task.Run(async () => await users.Indexes.CreateManyAsync(
                new[]
                {
                    new CreateIndexModel<UserDocument>(userBuilder.Ascending(i => i.Name),
                        new CreateIndexOptions
                        {
                            Unique = true
                        })
                }));

            var ratings = scope.ServiceProvider.GetService<IMongoDatabase>()
                .GetCollection<StoryRatingDocument>($"{Schema}.ratings");
            var ratingBuilder = Builders<StoryRatingDocument>.IndexKeys;
            Task.Run(async () => await ratings.Indexes.CreateManyAsync(
                new[]
                {
                    new CreateIndexModel<StoryRatingDocument>(
                        ratingBuilder.Ascending(i => i.UserId).Descending(i => i.StoryId),
                        new CreateIndexOptions
                        {
                            Unique = true
                        }),
                }));

            return builder;
        }
    }
}