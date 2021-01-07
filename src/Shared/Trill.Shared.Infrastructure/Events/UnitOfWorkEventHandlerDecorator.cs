using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Exceptions;
using Trill.Shared.Abstractions.Messaging;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Shared.Infrastructure.Events
{
    [Decorator]
    internal sealed class UnitOfWorkEventHandlerDecorator<T> : IEventHandler<T> where T : class, IEvent
    {
        private readonly IEventHandler<T> _handler;
        private readonly IMongoSessionFactory _sessionFactory;
        private readonly IExceptionToMessageMapperResolver _exceptionToMessageMapperResolver;
        private readonly IMessageBroker _messageBroker;
        private readonly MongoOptions _options;
        private readonly ILogger<UnitOfWorkEventHandlerDecorator<T>> _logger;

        public UnitOfWorkEventHandlerDecorator(IEventHandler<T> handler, IMongoSessionFactory sessionFactory,
            IExceptionToMessageMapperResolver exceptionToMessageMapperResolver, IMessageBroker messageBroker,
            MongoOptions options, ILogger<UnitOfWorkEventHandlerDecorator<T>> logger)
        {
            _handler = handler;
            _sessionFactory = sessionFactory;
            _exceptionToMessageMapperResolver = exceptionToMessageMapperResolver;
            _messageBroker = messageBroker;
            _options = options;
            _logger = logger;
        }

        public async Task HandleAsync(T @event)
        {
            if (_options.DisableTransactions)
            {
                await TryHandleAsync(@event);
                return;
            }

            using var session = await _sessionFactory.CreateAsync();
            await TryHandleAsync(@event, () => session.CommitTransactionAsync(), () => session.AbortTransactionAsync());
        }

        private async Task TryHandleAsync(T @event, Func<Task> onSuccess = null, Func<Task> onError = null)
        {
            try
            {
                await _handler.HandleAsync(@event);
                if (onSuccess is {})
                {
                    await onSuccess();
                }
            }
            catch (Exception exception)
            {
                if (onError is {})
                {
                    await onError();
                }

                if (@event is IActionRejected)
                {
                    throw;
                }

                var rejectedEvent = _exceptionToMessageMapperResolver.Map(exception);
                if (rejectedEvent is {})
                {
                    _logger.LogInformation("Publishing the rejected event...");
                    await _messageBroker.PublishAsync(rejectedEvent);
                }

                throw;
            }
        }
    }
}