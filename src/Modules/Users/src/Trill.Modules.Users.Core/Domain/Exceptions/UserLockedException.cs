using System;
using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Users.Core.Domain.Exceptions
{
    internal class UserLockedException : DomainException
    {
        public Guid UserId { get; }

        public UserLockedException(Guid userId) : base($"User with ID: '{userId}' is locked.")
        {
            UserId = userId;
        }
    }
}