using System.Threading.Tasks;
using Trill.Modules.Users.Core.Domain.Entities;
using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Users.Core.Domain.Repositories
{
    internal interface IUserRepository
    {
        Task<User> GetAsync(AggregateId id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByNameAsync(string name);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}