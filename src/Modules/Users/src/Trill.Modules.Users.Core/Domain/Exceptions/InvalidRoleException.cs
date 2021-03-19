using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Users.Core.Domain.Exceptions
{
    internal class InvalidRoleException : DomainException
    {
        public InvalidRoleException(string role) : base($"Invalid role: {role}.")
        {
        }
    }
}