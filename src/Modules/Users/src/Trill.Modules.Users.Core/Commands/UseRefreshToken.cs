using System;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Users.Core.Commands
{
    internal class UseRefreshToken : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public string RefreshToken { get; }

        public UseRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}