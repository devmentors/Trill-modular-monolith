using System.Threading.Tasks;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Time;

namespace Trill.Modules.Stories.Application.Events.External.Handlers
{
    internal class UserCreatedHandler : IEventHandler<UserCreated>
    {
        private readonly IUserRepository _userRepository;
        private readonly IClock _clock;

        public UserCreatedHandler(IUserRepository userRepository, IClock clock)
        {
            _userRepository = userRepository;
            _clock = clock;
        }

        public async Task HandleAsync(UserCreated @event)
        {
            var user = new User(@event.UserId, @event.Name, _clock.Current());
            await _userRepository.AddAsync(user);
        }
    }
}