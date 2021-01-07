using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Trill.Modules.Stories.Application.DTO;
using Trill.Modules.Stories.Application.Queries;
using Trill.Modules.Stories.Infrastructure.Mongo.Documents;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Queries;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Modules.Stories.Infrastructure.Mongo.Queries.Handlers
{
    internal class BrowseStoriesHandler : IQueryHandler<BrowseStories, Paged<StoryDto>>
    {
        private const string Schema = "stories-module";
        private readonly IMongoDatabase _database;

        public BrowseStoriesHandler(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Paged<StoryDto>> HandleAsync(BrowseStories query)
        {
            var now = query.Now.ToUnixTimeMilliseconds();
            var documents = _database.GetCollection<StoryDocument>($"{Schema}.stories")
                .AsQueryable()
                .Where(x => x.From <= now && x.To >= now);

            var input = query.Query;
            if (!string.IsNullOrWhiteSpace(input))
            {
                documents = documents.Where(x =>
                    x.Title.Contains(input) || x.Author.Name.Contains(input) || x.Tags.Contains(input));
            }
            
            var result = await documents.OrderByDescending(x => x.CreatedAt).PaginateAsync(query);
            var storyIds = result.Items.Select(x => x.Id);

            var rates = await _database.GetCollection<StoryRatingDocument>($"{Schema}.ratings")
                .AsQueryable()
                .Where(x => storyIds.Contains(x.StoryId))
                .ToListAsync();
            
            var pagedResult = Paged<StoryDto>.From(result, result.Items.Select(x => x.ToDto(rates)));
            
            return new Paged<StoryDto>
            {
                CurrentPage = pagedResult.CurrentPage,
                TotalPages = pagedResult.TotalPages,
                ResultsPerPage = pagedResult.ResultsPerPage,
                TotalResults = pagedResult.TotalResults,
                Items = pagedResult.Items
            };
        }
    }
}