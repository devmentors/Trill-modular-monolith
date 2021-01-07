using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Trill.Modules.Stories.Application.DTO;
using Trill.Modules.Stories.Application.Queries;
using Trill.Modules.Stories.Infrastructure.Mongo.Documents;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Modules.Stories.Infrastructure.Mongo.Queries.Handlers
{
    internal class GetStoryHandler : IQueryHandler<GetStory, StoryDetailsDto>
    {
        private const string Schema = "stories-module";
        private readonly IMongoDatabase _database;

        public GetStoryHandler(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<StoryDetailsDto> HandleAsync(GetStory query)
        {
            var story = await _database.GetCollection<StoryDocument>($"{Schema}.stories")
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Id == query.StoryId);

            if (story is null)
            {
                return null;
            }

            var now = DateTime.UtcNow.ToUnixTimeMilliseconds();
            if (story.From > now || story.To < now)
            {
                return null;
            }

            var rates = await _database.GetCollection<StoryRatingDocument>($"{Schema}.ratings")
                .AsQueryable()
                .Where(x => x.StoryId == query.StoryId)
                .ToListAsync();

            return story.ToDetailsDto(rates, query.UserId);
        }
        
    }
}