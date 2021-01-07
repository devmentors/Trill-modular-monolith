using System;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Users.Core.Commands
{
    internal class LockUser : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; }

        public LockUser(Guid userId)
        {
            UserId = userId;
        }
    }
}