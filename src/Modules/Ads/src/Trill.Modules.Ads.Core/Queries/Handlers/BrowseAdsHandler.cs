using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Trill.Modules.Ads.Core.Domain;
using Trill.Modules.Ads.Core.DTO;
using Trill.Shared.Abstractions.Queries;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Modules.Ads.Core.Queries.Handlers
{
    internal class BrowseAdsHandler : IQueryHandler<BrowseAds, Paged<AdDto>>
    {
        private const string Schema = "ads-module";
        private readonly IMongoDatabase _database;

        public BrowseAdsHandler(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Paged<AdDto>> HandleAsync(BrowseAds query)
        {
            var ads = _database.GetCollection<Ad>($"{Schema}.ads")
                .AsQueryable();

            if (query.UserId.HasValue)
            {
                ads = ads.Where(x => x.UserId == query.UserId);
            }

            var result = await ads
                .OrderByDescending(x => x.CreatedAt)
                .PaginateAsync(query);

            return new Paged<AdDto>
            {
                CurrentPage = result.CurrentPage,
                TotalPages = result.TotalPages,
                TotalResults = result.TotalResults,
                ResultsPerPage = result.ResultsPerPage,
                Items = result.Items.Select(ad => new AdDto
                {
                    Id = ad.Id,
                    UserId = ad.UserId,
                    Header = ad.Header,
                    Tags = ad.Tags,
                    State = ad.State.ToString().ToLowerInvariant(),
                    From = ad.From,
                    To = ad.To,
                    CreatedAt = ad.CreatedAt
                })
            };
        }
    }
}