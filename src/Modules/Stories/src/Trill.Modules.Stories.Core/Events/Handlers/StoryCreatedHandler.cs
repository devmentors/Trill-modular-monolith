using System.Threading.Tasks;
using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Core.Events.Handlers
{
    internal class StoryCreatedHandler : IDomainEventHandler<StoryCreated>
    {
        public Task HandleAsync(StoryCreated domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}