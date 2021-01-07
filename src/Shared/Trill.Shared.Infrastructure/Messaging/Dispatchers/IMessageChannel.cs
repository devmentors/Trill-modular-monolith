using System.Threading.Channels;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Shared.Infrastructure.Messaging.Dispatchers
{
    internal interface IMessageChannel
    {
        ChannelReader<IMessage> Reader { get; }
        ChannelWriter<IMessage> Writer { get; }
    }
}