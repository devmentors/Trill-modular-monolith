using System.Threading.Tasks;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Shared.Infrastructure.Messaging.Dispatchers
{
    internal class AsyncMessageDispatcher : IAsyncMessageDispatcher
    {
        private readonly IMessageChannel _channel;

        public AsyncMessageDispatcher(IMessageChannel channel)
        {
            _channel = channel;
        }

        public async Task PublishAsync<T>(T message) where T : class, IMessage
            => await _channel.Writer.WriteAsync(message);
    }
}