using System;
using System.Threading.Tasks;
using Trill.Modules.Stories.Application.Clients.Users.DTO;

namespace Trill.Modules.Stories.Application.Clients.Users
{
    internal interface IUsersApiClient
    {
        Task<UserDto> GetAsync(Guid userId);
    }
}