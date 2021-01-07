using System;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Users.Core.Commands
{
    internal class RevokeAccessToken : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public string AccessToken { get; }

        public RevokeAccessToken(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}