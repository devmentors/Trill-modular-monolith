using System;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Stories.Infrastructure.Clients.Users.Requests
{
    [Message("users")]
    internal class GetUser
    {
        public Guid UserId { get; set; }
        
        private class Contract : Contract<GetUser>
        {
            public Contract()
            {
                RequireAll();
            }
        }
    }
}