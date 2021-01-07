using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Modules.Stories.Infrastructure.Mongo.Documents;

namespace Trill.Modules.Stories.Infrastructure.Mongo.Repositories
{
    internal class StoryRatingMongoRepository : IStoryRatingRepository
    {
        private const string Schema = "stories-module";
        private readonly IMongoDatabase _database;

        public StoryRatingMongoRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<int> GetTotalRatingAsync(StoryId storyId)
            => await _database.GetCollection<StoryRatingDocument>($"{Schema}.ratings")
                .AsQueryable()
                .Where(x => x.StoryId == storyId)
                .SumAsync(x => x.Rate);

        public Task SetAsync(StoryRating rating)
            => _database.GetCollection<StoryRatingDocument>($"{Schema}.ratings")
                .ReplaceOneAsync(x => x.StoryId == rating.Id.StoryId && x.UserId == rating.Id.UserId,
                    new StoryRatingDocument(rating), new ReplaceOptions
                    {
                        IsUpsert = true
                    });
    }
}