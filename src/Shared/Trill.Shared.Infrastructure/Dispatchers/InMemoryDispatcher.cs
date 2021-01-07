using System.Threading.Tasks;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Shared.Infrastructure.Dispatchers
{
    internal class InMemoryDispatcher : IDispatcher
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public InMemoryDispatcher(ICommandDispatcher commandDispatcher, IEventDispatcher eventDispatcher,
            IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _eventDispatcher = eventDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        public Task SendAsync<T>(T command) where T : class, ICommand
            => _commandDispatcher.SendAsync(command);

        public Task PublishAsync<T>(T @event) where T : class, IEvent
            => _eventDispatcher.PublishAsync(@event);

        public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
            => _queryDispatcher.QueryAsync(query);

        public Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>
            => _queryDispatcher.QueryAsync<TQuery, TResult>(query);
    }
}