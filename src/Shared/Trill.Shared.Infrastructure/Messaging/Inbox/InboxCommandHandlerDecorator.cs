using System.Threading.Tasks;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Shared.Infrastructure.Messaging.Inbox
{
    [Decorator]
    internal class InboxCommandHandlerDecorator<T> : ICommandHandler<T> where T : class, ICommand
    {
        private readonly ICommandHandler<T> _handler;
        private readonly IInbox _inbox;
        private readonly string _module;

        public InboxCommandHandlerDecorator(ICommandHandler<T> handler, IInbox inbox)
        {
            _handler = handler;
            _inbox = inbox;
            _module = _handler.GetModuleName();
        }

        public Task HandleAsync(T command)
            => _inbox.Enabled
                ? _inbox.HandleAsync(command, () => _handler.HandleAsync(command), _module)
                : _handler.HandleAsync(command);
    }
}