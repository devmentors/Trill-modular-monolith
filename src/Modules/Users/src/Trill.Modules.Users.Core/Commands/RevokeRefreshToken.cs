using System;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Users.Core.Commands
{
    internal class RevokeRefreshToken : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public string RefreshToken { get; }

        public RevokeRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}