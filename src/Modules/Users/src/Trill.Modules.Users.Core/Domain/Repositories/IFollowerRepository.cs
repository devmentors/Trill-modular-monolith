using System.Threading.Tasks;
using Trill.Modules.Users.Core.Domain.Entities;
using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Users.Core.Domain.Repositories
{
    internal interface IFollowerRepository
    {
        Task<Follower> GetAsync(AggregateId followerId, AggregateId followeeId);
        Task AddAsync(Follower follower);
        Task DeleteAsync(AggregateId id);
    }
}