using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Trill.Shared.Abstractions.Contexts;
using Trill.Shared.Abstractions.Events;

namespace Trill.Shared.Infrastructure.Events
{
    internal sealed class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceScopeFactory _serviceFactory;
        private readonly ILogger<EventDispatcher> _logger;

        public EventDispatcher(IServiceScopeFactory serviceFactory, ILogger<EventDispatcher> logger)
        {
            _serviceFactory = serviceFactory;
            _logger = logger;
        }

        public async Task PublishAsync<T>(T @event) where T : class, IEvent
        {
            if (@event is null)
            {
                return;
            }

            using var scope = _serviceFactory.CreateScope();
            if (@event.CorrelationId == Guid.Empty)
            {
                var context = scope.ServiceProvider.GetRequiredService<IContext>();
                @event.CorrelationId = context.CorrelationId;
            }

            if (typeof(T) == typeof(IEvent))
            {
                var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
                var eventHandlers = scope.ServiceProvider.GetServices(handlerType);
                foreach (var handler in eventHandlers)
                {
                    if (handler is null)
                    {
                        continue;
                    }

                    await (Task) handler.GetType().GetMethod(nameof(IEventHandler<T>.HandleAsync))
                        ?.Invoke(handler, new[] {@event});
                }
                
                // dynamic eventHandlers = scope.ServiceProvider.GetServices(handlerType);
                // foreach (var handler in eventHandlers)
                // {
                //     await (Task) handler.HandleAsync((dynamic) @event);
                // }

                return;
            }

            var handlers = scope.ServiceProvider.GetServices<IEventHandler<T>>();
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}