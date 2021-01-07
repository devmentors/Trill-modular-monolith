using System;
using Trill.Shared.Abstractions.Events;

namespace Trill.Modules.Users.Core.Events
{
    internal class UserActionRejected : IActionRejected
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; }
        public string Code { get; }
        public string Reason { get; }

        public UserActionRejected(Guid userId, string code, string reason)
        {
            UserId = userId;
            Code = code;
            Reason = reason;
        }
    }
}