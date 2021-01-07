using System.Threading.Tasks;
using Trill.Modules.Users.Core.Commands;
using Trill.Modules.Users.Core.Domain.Repositories;
using Trill.Modules.Users.Core.Events;
using Trill.Modules.Users.Core.Exceptions;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Users.Core.Handlers.Commands
{
    internal sealed class LockUserHandler : ICommandHandler<LockUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageBroker _messageBroker;

        public LockUserHandler(IUserRepository userRepository, IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(LockUser command)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            if (user.Lock())
            {
                await _userRepository.UpdateAsync(user);
                await _messageBroker.PublishAsync(new UserLocked(user.Id));
            }
        }
    }
}