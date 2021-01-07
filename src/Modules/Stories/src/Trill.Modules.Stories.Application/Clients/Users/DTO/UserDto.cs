using System;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Stories.Application.Clients.Users.DTO
{
    [Message("users")]
    internal class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Locked { get; set; }
        
        private class Contract : Contract<UserDto>
        {
            public Contract()
            {
                RequireAll();
            }
        }
    }
}