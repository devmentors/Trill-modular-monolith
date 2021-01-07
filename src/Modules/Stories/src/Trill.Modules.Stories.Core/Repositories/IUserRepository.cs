using System.Threading.Tasks;
using Trill.Modules.Stories.Core.Entities;

namespace Trill.Modules.Stories.Core.Repositories
{
    internal interface IUserRepository
    {
        Task<User> GetAsync(UserId id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}