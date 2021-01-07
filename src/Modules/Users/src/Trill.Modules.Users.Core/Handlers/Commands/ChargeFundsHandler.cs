using System.Threading.Tasks;
using Trill.Modules.Users.Core.Commands;
using Trill.Modules.Users.Core.Domain.Repositories;
using Trill.Modules.Users.Core.Exceptions;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Users.Core.Handlers.Commands
{
    internal sealed class ChargeFundsHandler : ICommandHandler<ChargeFunds>
    {
        private readonly IUserRepository _userRepository;

        public ChargeFundsHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task HandleAsync(ChargeFunds command)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            user.ChargeFunds(command.Amount);
            await _userRepository.UpdateAsync(user);
        }
    }
}