using System;
using Trill.Modules.Users.Core.Domain.Exceptions;
using Trill.Shared.Kernel.BuildingBlocks;

namespace Trill.Modules.Users.Core.Domain.Entities
{
    internal class RefreshToken : AggregateRoot<Guid>
    {
        public AggregateId UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public bool Revoked => RevokedAt.HasValue;

        public RefreshToken(AggregateId id, AggregateId userId, string token, DateTime createdAt,
            DateTime? revokedAt = null) : base(id)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new EmptyRefreshTokenException();
            }

            UserId = userId;
            Token = token;
            CreatedAt = createdAt;
            RevokedAt = revokedAt;
        }

        public void Revoke(DateTime revokedAt)
        {
            if (Revoked)
            {
                throw new RevokedRefreshTokenException();
            }

            RevokedAt = revokedAt;
        }
    }
}