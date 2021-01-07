using System.Threading.Tasks;
using Chronicle;
using Microsoft.Extensions.Logging;
using Trill.Modules.Saga.Commands.External;
using Trill.Modules.Saga.Events.External;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Saga.Sagas
{
    internal class PublishAdSaga : Saga<PublishAdSagaData>,
        ISagaStartAction<AdApproved>,
        ISagaAction<AdPaid>,
        ISagaAction<AdPublished>,
        ISagaAction<AdActionRejected>
    {
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<PublishAdSaga> _logger;

        public PublishAdSaga(IMessageBroker messageBroker, ILogger<PublishAdSaga> logger)
        {
            _messageBroker = messageBroker;
            _logger = logger;
        }
        
        public override SagaId ResolveId(object message, ISagaContext context)
            => message switch
            {
                AdApproved m => (SagaId) m.AdId.ToString(),
                AdPaid m => (SagaId) m.AdId.ToString(),
                AdPublished m => (SagaId) m.AdId.ToString(),
                AdActionRejected m => (SagaId) m.AdId.ToString(),
                _ => base.ResolveId(message, context)
            };

        public async Task HandleAsync(AdApproved message, ISagaContext context)
        {
            LogStep(message);
            Data.AdId = message.AdId;
            await _messageBroker.PublishAsync(new PayAd(message.AdId));
        }

        public Task CompensateAsync(AdApproved message, ISagaContext context)
        {
            LogStep(message);
            return RejectAsync();
        }

        public async Task HandleAsync(AdPaid message, ISagaContext context)
        {
            LogStep(message);
            await _messageBroker.PublishAsync(new PublishAd(message.AdId));
        }

        public Task CompensateAsync(AdPaid message, ISagaContext context)
        {
            LogStep(message);
            return RejectAsync();
        }

        public Task HandleAsync(AdPublished message, ISagaContext context)
        {
            LogStep(message);
            return CompleteAsync();
        }

        public Task CompensateAsync(AdPublished message, ISagaContext context)
        {
            LogStep(message);
            return RejectAsync();
        }

        private void LogStep<T>(T message) where T : IMessage
            => _logger.LogInformation($"Handling a saga step: '{message.GetType().Name.Underscore()}'...");

        public async Task HandleAsync(AdActionRejected message, ISagaContext context)
        {
            LogStep(message);
            await Task.CompletedTask;
        }

        public async Task CompensateAsync(AdActionRejected message, ISagaContext context)
        {
            LogStep(message);
            await Task.CompletedTask; // An edge case scenario
        }
    }
}