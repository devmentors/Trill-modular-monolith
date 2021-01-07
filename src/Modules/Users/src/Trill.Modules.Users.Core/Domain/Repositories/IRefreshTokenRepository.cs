using System.Threading.Tasks;
using Trill.Modules.Users.Core.Domain.Entities;

namespace Trill.Modules.Users.Core.Domain.Repositories
{
    internal interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetAsync(string token);
        Task AddAsync(RefreshToken refreshToken);
        Task UpdateAsync(RefreshToken refreshToken);
    }
}