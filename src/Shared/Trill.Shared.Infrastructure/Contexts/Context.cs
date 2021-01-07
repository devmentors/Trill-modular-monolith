using System;
using Microsoft.AspNetCore.Http;
using Trill.Shared.Abstractions.Contexts;

namespace Trill.Shared.Infrastructure.Contexts
{
    internal sealed class Context : IContext
    {
        public string RequestId { get; } = $"{Guid.NewGuid():N}";
        public Guid CorrelationId { get; } = Guid.NewGuid();
        public string TraceId { get; }
        public IIdentityContext Identity { get; }

        internal Context() : this($"{Guid.NewGuid():N}", IdentityContext.Empty)
        {
        }

        internal Context(HttpContext context) : this(context.TraceIdentifier,
            context.User is null ? IdentityContext.Empty : new IdentityContext(context.User))
        {
        }

        internal Context(string traceId, IIdentityContext identity)
        {
            TraceId = traceId;
            Identity = identity;
        }

        internal static IContext Empty => new Context();
    }
}