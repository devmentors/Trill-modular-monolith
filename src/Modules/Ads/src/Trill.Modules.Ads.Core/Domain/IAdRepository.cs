using System;
using System.Threading.Tasks;

namespace Trill.Modules.Ads.Core.Domain
{
    internal interface IAdRepository
    {
        Task<Ad> GetAsync(Guid id);
        Task AddAsync(Ad ad);
        Task UpdateAsync(Ad ad);
    }
}