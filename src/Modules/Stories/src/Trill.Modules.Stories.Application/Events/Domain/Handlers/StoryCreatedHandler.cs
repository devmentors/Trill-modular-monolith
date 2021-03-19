using System.Threading.Tasks;
using Trill.Modules.Stories.Core.Events;
using Trill.Modules.Stories.Core.Repositories;
using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Application.Events.Domain.Handlers
{
    internal sealed class StoryCreatedHandler : IDomainEventHandler<StoryCreated>
    {
        private readonly IUserRepository _userRepository;

        public StoryCreatedHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task HandleAsync(StoryCreated domainEvent)
        {
            await Task.CompletedTask;
        }
    }
}