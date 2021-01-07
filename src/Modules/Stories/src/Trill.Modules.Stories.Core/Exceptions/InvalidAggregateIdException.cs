using System;
using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Stories.Core.Exceptions
{
    internal class InvalidAggregateIdException : DomainException
    {
        public Guid Id { get; }

        public InvalidAggregateIdException(Guid id) : base($"Invalid aggregate id: {id}")
            => Id = id;
    }
}