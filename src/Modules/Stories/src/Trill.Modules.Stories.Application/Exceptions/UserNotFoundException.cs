using System;
using Trill.Shared.Abstractions;

namespace Trill.Modules.Stories.Application.Exceptions
{
    internal class UserNotFoundException : AppException
    {
        public UserNotFoundException(Guid userId) : base($"User with ID: '{userId}' was not found.")
        {
        }
    }
}