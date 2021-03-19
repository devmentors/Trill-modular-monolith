using System;
using Trill.Shared.Abstractions;

namespace Trill.Modules.Stories.Application.Exceptions
{
    internal class CannotCreateStoryException : AppException
    {
        public CannotCreateStoryException(Guid userId) : base($"Story cannot be created by user with ID: '{userId}'.")
        {
        }
    }
}