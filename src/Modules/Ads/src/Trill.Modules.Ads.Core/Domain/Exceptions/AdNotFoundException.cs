using System;
using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Ads.Core.Domain.Exceptions
{
    internal class AdNotFoundException : DomainException
    {
        public Guid AdId { get; }

        public AdNotFoundException(Guid adId) : base($"Ad with ID: '{adId}' was not found.")
        {
            AdId = adId;
        }
    }
}