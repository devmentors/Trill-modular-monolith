using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Trill.Modules.Analytics.Core.Models;
using Trill.Modules.Analytics.Core.Mongo;
using Tag = Trill.Modules.Analytics.Core.Models.Tag;

namespace Trill.Modules.Analytics.Core.Services
{
    internal class TrendingService : ITrendingService
    {
        private readonly IDatabaseProvider _databaseProvider;

        public TrendingService(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public async Task<IEnumerable<Story>> GetTopStoriesAsync()
            => await _databaseProvider.Stories.AsQueryable()
                .OrderByDescending(x => x.TotalRate)
                .Take(10)
                .ToListAsync();

        public async Task<IEnumerable<Tag>> GetTopTagsAsync()
            => await _databaseProvider.Tags.AsQueryable()
                .OrderByDescending(x => x.OccurenceCount)
                .Take(10)
                .ToListAsync();

        public async Task<IEnumerable<User>> GetTopUsersAsync()
            => await _databaseProvider.Users.AsQueryable()
                .OrderByDescending(x => x.FollowersCount)
                .ThenByDescending(x => x.StoriesCount)
                .Take(10)
                .ToListAsync();
    }
}