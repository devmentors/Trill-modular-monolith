using System;
using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Ads.Core.Domain.Exceptions
{
    internal class CannotChangeAdStateException : DomainException
    {
        public Guid AdId { get; }

        public CannotChangeAdStateException(Guid adId) : base($"Ad with ID: '{adId}' state cannot be changed.")
        {
            AdId = adId;
        }
    }
}