using System;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Users.Core.Commands
{
    internal class AddFunds : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; }
        public decimal Amount { get; }

        public AddFunds(Guid userId, decimal amount)
        {
            UserId = userId;
            Amount = amount;
        }
    }
}