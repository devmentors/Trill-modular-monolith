using System;
using System.Threading.Tasks;
using Trill.Modules.Stories.Application.Clients.Users;
using Trill.Modules.Stories.Application.Clients.Users.DTO;
using Trill.Modules.Stories.Infrastructure.Clients.Users.Requests;
using Trill.Shared.Abstractions.Modules;

namespace Trill.Modules.Stories.Infrastructure.Clients.Users
{
    internal class UsersApiClient : IUsersApiClient
    {
        private const string Module = "users-module";
        private readonly IModuleClient _moduleClient;

        public UsersApiClient(IModuleClient moduleClient)
        {
            _moduleClient = moduleClient;
        }

        public async Task<UserDto> GetAsync(Guid userId)
        {
            var response = await _moduleClient.RequestAsync<UserDto>($"{Module}/get-user",
                new GetUser
                {
                    UserId = userId
                });

            return response;
        }
    }
}