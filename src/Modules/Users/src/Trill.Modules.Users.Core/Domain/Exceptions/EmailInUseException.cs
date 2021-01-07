using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Users.Core.Domain.Exceptions
{
    internal class EmailInUseException : DomainException
    {
        public string Email { get; }

        public EmailInUseException(string email) : base($"Email {email} is already in use.")
        {
            Email = email;
        }
    }
}