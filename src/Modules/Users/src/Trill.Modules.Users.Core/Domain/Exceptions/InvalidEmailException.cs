using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Users.Core.Domain.Exceptions
{
    internal class InvalidEmailException : DomainException
    {
        public InvalidEmailException(string email) : base($"Invalid email: {email}.")
        {
        }
    }
}