using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Shared.Infrastructure.Logging
{
    [Decorator]
    internal sealed class LoggingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _handler;
        private readonly ILogger<IQueryHandler<TQuery, TResult>> _logger;

        public LoggingQueryHandlerDecorator(IQueryHandler<TQuery, TResult> handler,
            ILogger<IQueryHandler<TQuery, TResult>> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            var module = query.GetModuleName();
            var name = query.GetType().Name.Underscore();
            _logger.LogInformation($"Handling a query: '{name}' ('{module}')...");
            var result = await _handler.HandleAsync(query);
            _logger.LogInformation($"Completed handling a query: '{name}' ('{module}').");

            return result;
        }
    }
}