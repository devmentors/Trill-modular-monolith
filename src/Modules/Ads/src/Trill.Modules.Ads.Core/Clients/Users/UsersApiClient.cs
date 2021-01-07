using System;
using System.Threading.Tasks;
using Trill.Modules.Ads.Core.Clients.Users.Requests;
using Trill.Shared.Abstractions.Modules;

namespace Trill.Modules.Ads.Core.Clients.Users
{
    internal class UsersApiClient : IUsersApiClient
    {
        private const string Module = "users-module";
        private readonly IModuleClient _moduleClient;

        public UsersApiClient(IModuleClient moduleClient)
        {
            _moduleClient = moduleClient;
        }

        public async Task<bool> ChargeFundsAsync(Guid userId, decimal amount)
        {
            var response = await _moduleClient.RequestAsync<ChargeFunds.Response>($"{Module}/charge-funds",
                new ChargeFunds(userId, amount));
            return response?.Charged ?? false;
        }
    }
}