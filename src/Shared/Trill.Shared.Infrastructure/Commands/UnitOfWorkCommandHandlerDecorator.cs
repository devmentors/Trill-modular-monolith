using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Exceptions;
using Trill.Shared.Abstractions.Messaging;
using Trill.Shared.Infrastructure.Mongo;

namespace Trill.Shared.Infrastructure.Commands
{
    [Decorator]
    internal sealed class UnitOfWorkCommandHandlerDecorator<T> : ICommandHandler<T> where T : class, ICommand
    {
        private readonly ICommandHandler<T> _handler;
        private readonly IMongoSessionFactory _sessionFactory;
        private readonly IExceptionToMessageMapperResolver _exceptionToMessageMapperResolver;
        private readonly IMessageBroker _messageBroker;
        private readonly MongoOptions _options;
        private readonly ILogger<UnitOfWorkCommandHandlerDecorator<T>> _logger;

        public UnitOfWorkCommandHandlerDecorator(ICommandHandler<T> handler, IMongoSessionFactory sessionFactory,
            IExceptionToMessageMapperResolver exceptionToMessageMapperResolver, IMessageBroker messageBroker,
            MongoOptions options, ILogger<UnitOfWorkCommandHandlerDecorator<T>> logger)
        {
            _handler = handler;
            _sessionFactory = sessionFactory;
            _exceptionToMessageMapperResolver = exceptionToMessageMapperResolver;
            _messageBroker = messageBroker;
            _options = options;
            _logger = logger;
        }

        public async Task HandleAsync(T command)
        {
            if (_options.DisableTransactions)
            {
                await TryHandleAsync(command);
                return;
            }

            using var session = await _sessionFactory.CreateAsync();
            await TryHandleAsync(command, () => session.CommitTransactionAsync(), () => session.AbortTransactionAsync());
        }

        private async Task TryHandleAsync(T command, Func<Task> onSuccess = null, Func<Task> onError = null)
        {
            try
            {
                await _handler.HandleAsync(command);
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
                
                // Not a background processing
                if (command.Id == Guid.Empty)
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