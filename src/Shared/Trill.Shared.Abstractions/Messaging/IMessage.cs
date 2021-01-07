using System;

namespace Trill.Shared.Abstractions.Messaging
{
    public interface IMessage
    {
        Guid Id { get; set; }
        Guid CorrelationId { get; set; }
    }
}