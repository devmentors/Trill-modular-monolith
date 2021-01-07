using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Tag = Trill.Modules.Analytics.Core.Models.Tag;

namespace Trill.Modules.Analytics.Core.Mongo
{
    internal static class Extensions
    {
        internal static IApplicationBuilder UseMongo(this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();
            var databaseProvider = scope.ServiceProvider.GetService<IDatabaseProvider>();
            var tagBuilder = Builders<Tag>.IndexKeys;
            Task.Run(async () => await databaseProvider.Tags.Indexes.CreateManyAsync(
                new[]
                {
                    new CreateIndexModel<Tag>(tagBuilder.Ascending(i => i.Name),
                        new CreateIndexOptions
                        {
                            Unique = true
                        })
                }));

            return builder;
        }
    }
}