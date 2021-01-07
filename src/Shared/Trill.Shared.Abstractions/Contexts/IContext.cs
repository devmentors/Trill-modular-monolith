using System;

namespace Trill.Shared.Abstractions.Contexts
{
    public interface IContext
    {
        string RequestId { get; }
        Guid CorrelationId { get; }
        string TraceId { get; }
        IIdentityContext Identity { get; }
    }
}