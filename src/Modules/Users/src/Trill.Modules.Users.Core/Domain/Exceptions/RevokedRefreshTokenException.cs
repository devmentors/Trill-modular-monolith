using Trill.Shared.Abstractions.Kernel;

namespace Trill.Modules.Users.Core.Domain.Exceptions
{
    internal class RevokedRefreshTokenException : DomainException
    {
        public RevokedRefreshTokenException() : base("Revoked refresh token.")
        {
        }
    }
}