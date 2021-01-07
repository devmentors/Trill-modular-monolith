using System.Threading.Tasks;
using Trill.Modules.Analytics.Core.Models;
using Trill.Modules.Analytics.Core.Services;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Analytics.Core.Events.External.Handlers
{
    internal sealed class UserCreatedHandler : IEventHandler<UserCreated>
    {
        private readonly IUsersService _usersService;

        public UserCreatedHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task HandleAsync(UserCreated @event)
        {
            await _usersService.AddAsync(new User
            {
                Id = @event.UserId,
                Name = @event.Name
            });
        }
    }
}