using System.Threading.Tasks;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Shared.Infrastructure.Messaging.Outbox
{
    internal interface IOutbox
    {
        bool Enabled { get; }
        Task SaveAsync(params IMessage[] messages);
        Task PublishUnsentAsync();
    }
}