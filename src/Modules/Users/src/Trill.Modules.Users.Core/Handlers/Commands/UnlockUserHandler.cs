using System.Threading.Tasks;
using Trill.Modules.Users.Core.Commands;
using Trill.Modules.Users.Core.Domain.Repositories;
using Trill.Modules.Users.Core.Events;
using Trill.Modules.Users.Core.Exceptions;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Users.Core.Handlers.Commands
{
    internal sealed class UnlockUserHandler : ICommandHandler<UnlockUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageBroker _messageBroker;

        public UnlockUserHandler(IUserRepository userRepository, IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _messageBroker = messageBroker;
        }
        
        public async Task HandleAsync(UnlockUser command)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            if (user.Unlock())
            {
                await _userRepository.UpdateAsync(user);
                await _messageBroker.PublishAsync(new UserUnlocked(user.Id));
            }
        }
    }
}