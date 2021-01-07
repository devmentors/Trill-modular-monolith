using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Trill.Modules.Analytics.Core.Models;
using Trill.Modules.Analytics.Core.Mongo;

namespace Trill.Modules.Analytics.Core.Services
{
    internal class UsersService : IUsersService
    {
        private readonly IDatabaseProvider _databaseProvider;

        public UsersService(IDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public async Task AddAsync(User user)
        {
            await _databaseProvider.Users.InsertOneAsync(user);
        }

        public async Task IncrementStoriesCountAsync(Guid userId)
        {
            var builder = Builders<User>.Filter;
            var filter = builder.Eq(x => x.Id, userId);
            var update = Builders<User>.Update.Inc(s => s.StoriesCount, 1);
            await _databaseProvider.Users.FindOneAndUpdateAsync(filter, update);
        }

        public Task IncrementFollowersCountAsync(Guid followerId, Guid followeeId)
            => SetFollowersCountAsync(followerId, followeeId, 1);

        public Task DecrementFollowersCountAsync(Guid followerId, Guid followeeId)
            => SetFollowersCountAsync(followerId, followeeId, -1);

        private async Task SetFollowersCountAsync(Guid followerId, Guid followeeId, int value)
        {
            var builder = Builders<User>.Filter;
            var filterFollowee = builder.Eq(x => x.Id, followeeId);
            var updateFollowee = Builders<User>.Update.Inc(s => s.FollowersCount, value);

            var filterFollower = builder.Eq(x => x.Id, followerId);
            var updateFollower = Builders<User>.Update.Inc(s => s.FollowingCount, value);

            await _databaseProvider.Users.FindOneAndUpdateAsync(filterFollowee, updateFollowee);
            await _databaseProvider.Users.FindOneAndUpdateAsync(filterFollower, updateFollower);
        }
    }
}