using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Events;

namespace Trill.Shared.Infrastructure.Logging
{
    [Decorator]
    internal sealed class LoggingEventHandlerDecorator<T> : IEventHandler<T> where T : class, IEvent
    {
        private readonly IEventHandler<T> _handler;
        private readonly ILogger<IEventHandler<T>> _logger;

        public LoggingEventHandlerDecorator(IEventHandler<T> handler, ILogger<IEventHandler<T>> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public async Task HandleAsync(T @event)
        {
            using (LogContext.PushProperty("CorrelationId", $"{@event.CorrelationId:N}"))
            {
                var module = @event.GetModuleName();
                var name = @event.GetType().Name.Underscore();
                _logger.LogInformation($"Handling an event: '{name}' ('{module}')...");
                await _handler.HandleAsync(@event);
                _logger.LogInformation($"Completed handling an event: '{name}' ('{module}').");
            }
        }
    }
}