using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Users.Core.Domain.Exceptions
{
    internal class EmptyRefreshTokenException : DomainException
    {
        public EmptyRefreshTokenException() : base("Empty refresh token.")
        {
        }
    }
}