using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Contexts;

namespace Trill.Shared.Infrastructure.Commands
{
    internal sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceScopeFactory _serviceFactory;

        public CommandDispatcher(IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public async Task SendAsync<T>(T command) where T : class, ICommand
        {
            if (command is null)
            {
                return;
            }

            using var scope = _serviceFactory.CreateScope();
            if (command.CorrelationId == Guid.Empty)
            {
                var context = scope.ServiceProvider.GetRequiredService<IContext>();
                command.CorrelationId = context.CorrelationId;
            }

            var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<T>>();
            await handler.HandleAsync(command);
        }
    }
}