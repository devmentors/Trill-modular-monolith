using System;
using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Users.Core.Domain.Exceptions
{
    internal class InsufficientFundsException : DomainException
    {
        public Guid UserId { get; }

        public InsufficientFundsException(Guid userId) : base($"User with ID: '{userId}' has insufficient funds.")
        {
            UserId = userId;
        }
    }
}