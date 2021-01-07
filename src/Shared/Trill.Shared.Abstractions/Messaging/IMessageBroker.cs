using System.Threading.Tasks;

namespace Trill.Shared.Abstractions.Messaging
{
    public interface IMessageBroker
    {
        Task PublishAsync(params IMessage[] messages);
    }
}