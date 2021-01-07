using System;
using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Ads.Core.Domain.Exceptions
{
    internal class CannotCreateAdAException : DomainException
    {
        public Guid AdId { get; }

        public CannotCreateAdAException(Guid adId) : base($"Ad with ID: '{adId}' cannot be created.")
        {
            AdId = adId;
        }
    }
}