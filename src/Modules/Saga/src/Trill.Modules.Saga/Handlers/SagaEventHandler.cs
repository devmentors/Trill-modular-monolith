using System.Threading.Tasks;
using Chronicle;
using Trill.Modules.Saga.Events.External;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Saga.Handlers
{
    internal sealed class SagaEventHandler :
        IEventHandler<AdApproved>,
        IEventHandler<AdPaid>,
        IEventHandler<AdPublished>,
        IEventHandler<AdActionRejected>
    {
        private readonly ISagaCoordinator _coordinator;

        public SagaEventHandler(ISagaCoordinator coordinator)
        {
            _coordinator = coordinator;
        }

        public Task HandleAsync(AdApproved @event) => HandleSagaAsync(@event);
        public Task HandleAsync(AdPaid @event) => HandleSagaAsync(@event);
        public Task HandleAsync(AdPublished @event) => HandleSagaAsync(@event);
        public Task HandleAsync(AdActionRejected @event) => HandleSagaAsync(@event);

        private Task HandleSagaAsync<T>(T @event) where T : class, IEvent
            => _coordinator.ProcessAsync(@event, SagaContext.Empty);
    }
}