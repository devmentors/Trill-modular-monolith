using System;
using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Stories.Core.Exceptions
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