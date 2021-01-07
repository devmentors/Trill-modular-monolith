using System;
using Trill.Shared.Abstractions;

namespace Trill.Modules.Users.Core.Exceptions
{
    internal class UserNotFoundException : AppException
    {
        public Guid UserId { get; }
        
        public UserNotFoundException(Guid userId) : base($"User with ID: '{userId}' was not found.")
        {
            UserId = userId;
        }
    }
}