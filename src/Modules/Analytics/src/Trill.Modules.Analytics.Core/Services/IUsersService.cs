using System;
using System.Threading.Tasks;
using Trill.Modules.Analytics.Core.Models;

namespace Trill.Modules.Analytics.Core.Services
{
    internal interface IUsersService
    {
        Task AddAsync(User user);
        Task IncrementStoriesCountAsync(Guid userId);
        Task IncrementFollowersCountAsync(Guid followerId, Guid followeeId);
        Task DecrementFollowersCountAsync(Guid followerId, Guid followeeId);
    }
}