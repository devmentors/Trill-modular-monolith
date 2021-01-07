using System.Threading.Tasks;
using Trill.Modules.Stories.Application.Exceptions;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Stories.Application.Events.External.Handlers
{
    internal sealed class UserUnlockedHandler : IEventHandler<UserUnlocked>
    {
        private readonly IUserRepository _userRepository;

        public UserUnlockedHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(UserUnlocked @event)
        {
            var user = await _userRepository.GetAsync(@event.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(@event.UserId);
            }
                
            user.Unlock();
            await _userRepository.UpdateAsync(user);
        }
    }
}