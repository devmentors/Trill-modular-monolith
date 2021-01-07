using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Contexts;
using Trill.Shared.Abstractions.Messaging;
using Trill.Shared.Abstractions.Modules;
using Trill.Shared.Infrastructure.Messaging.Dispatchers;
using Trill.Shared.Infrastructure.Messaging.Outbox;

namespace Trill.Shared.Infrastructure.Messaging.Brokers
{
    internal class InMemoryMessageBroker : IMessageBroker
    {
        private readonly IModuleClient _moduleClient;
        private readonly IAsyncMessageDispatcher _asyncMessageDispatcher;
        private readonly IContext _context;
        private readonly IOutbox _outbox;
        private readonly MessagingOptions _messagingOptions;
        private readonly ILogger<InMemoryMessageBroker> _logger;

        public InMemoryMessageBroker(IModuleClient moduleClient, IAsyncMessageDispatcher asyncMessageDispatcher,
            IContext context, IOutbox outbox, MessagingOptions messagingOptions, ILogger<InMemoryMessageBroker> logger)
        {
            _moduleClient = moduleClient;
            _asyncMessageDispatcher = asyncMessageDispatcher;
            _context = context;
            _outbox = outbox;
            _messagingOptions = messagingOptions;
            _logger = logger;
        }

        public async Task PublishAsync(params IMessage[] messages)
        {
            if (messages is null)
            {
                return;
            }

            messages = messages.Where(x => x is {}).ToArray();
            if (!messages.Any())
            {
                return;
            }

            foreach (var message in messages)
            {
                if (message.CorrelationId == Guid.Empty)
                {
                    message.CorrelationId = _context.CorrelationId;
                }
            }

            if (_outbox.Enabled)
            {
                _logger.LogInformation("Messages will be saved to the outbox...");
                await _outbox.SaveAsync(messages);
                return;
            }

            foreach (var message in messages)
            {
                var name = message.GetType().Name.Underscore();
                _logger.LogInformation($"Publishing a message: '{name}' with ID: '{message.Id:N}'...");
                if (_messagingOptions.UseBackgroundDispatcher)
                {
                    await _asyncMessageDispatcher.PublishAsync(message);
                    continue;
                }

                await _moduleClient.SendAsync(message);
            }
        }
    }
}