using System.Threading.Tasks;
using Trill.Modules.Stories.Core.Entities;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Stories.Application.Events.External.Handlers
{
    internal sealed class UserCreatedHandler : IEventHandler<UserCreated>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UserCreatedHandler(IUserRepository userRepository, IDateTimeProvider dateTimeProvider)
        {
            _userRepository = userRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task HandleAsync(UserCreated @event)
        {
            var user = new User(@event.UserId, @event.Name, _dateTimeProvider.Get());
            await _userRepository.AddAsync(user);
        }
    }
}