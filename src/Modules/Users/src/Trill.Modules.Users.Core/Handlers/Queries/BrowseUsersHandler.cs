using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Trill.Modules.Users.Core.DTO;
using Trill.Modules.Users.Core.Mongo.Documents;
using Trill.Modules.Users.Core.Queries;
using Trill.Shared.Abstractions.Queries;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Modules.Users.Core.Handlers.Queries
{
    internal class BrowseUsersHandler : IQueryHandler<BrowseUsers, Paged<UserDto>>
    {
        private const string Schema = "users-module";
        private readonly IMongoDatabase _database;

        public BrowseUsersHandler(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<Paged<UserDto>> HandleAsync(BrowseUsers query)
        {
            var result = await _database.GetCollection<UserDocument>($"{Schema}.users")
                .AsQueryable()
                .OrderByDescending(x => x.CreatedAt)
                .PaginateAsync(query);

            var followers = new HashSet<Guid>();
            if (query.UserId.HasValue)
            {
                var userId = query.UserId.Value;
                followers = new HashSet<Guid>(await _database.GetCollection<FollowerDocument>($"{Schema}.followers")
                    .AsQueryable()
                    .Where(x => x.FollowerId == userId)
                    .Select(x => x.FolloweeId)
                    .ToListAsync());
            }

            var pagedResult = Paged<UserDto>.From(result, result.Items.Select(x => Map(x, followers)));
            return new Paged<UserDto>
            {
                CurrentPage = pagedResult.CurrentPage,
                TotalPages = pagedResult.TotalPages,
                ResultsPerPage = pagedResult.ResultsPerPage,
                TotalResults = pagedResult.TotalResults,
                Items = pagedResult.Items
            };
        }

        private static UserDto Map(UserDocument user, ISet<Guid> followers)
            => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                CreatedAt = user.CreatedAt,
                Locked = user.Locked,
                IsFollowing = followers.Contains(user.Id)
            };
    }
}