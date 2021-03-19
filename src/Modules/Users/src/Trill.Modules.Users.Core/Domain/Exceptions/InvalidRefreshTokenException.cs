using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Users.Core.Domain.Exceptions
{
    internal class InvalidRefreshTokenException : DomainException
    {
        public InvalidRefreshTokenException() : base("Invalid refresh token.")
        {
        }
    }
}