using System;
using System.Threading.Tasks;

namespace Trill.Modules.Ads.Core.Clients.Users
{
    internal interface IUsersApiClient
    {
        Task<bool> ChargeFundsAsync(Guid userId, decimal amount);
    }
}