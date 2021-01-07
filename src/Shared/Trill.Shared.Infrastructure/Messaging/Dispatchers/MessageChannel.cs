using System.Threading.Channels;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Shared.Infrastructure.Messaging.Dispatchers
{
    internal class MessageChannel : IMessageChannel
    {
        private readonly Channel<IMessage> _messages = Channel.CreateUnbounded<IMessage>();
        public ChannelReader<IMessage> Reader => _messages.Reader;
        public ChannelWriter<IMessage> Writer => _messages.Writer;
    }
}