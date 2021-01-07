using System.Threading.Tasks;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Shared.Infrastructure.Messaging.Dispatchers
{
    internal interface IAsyncMessageDispatcher
    {
        Task PublishAsync<T>(T message) where T : class, IMessage;
    }
}