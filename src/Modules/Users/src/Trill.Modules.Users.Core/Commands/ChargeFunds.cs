using System;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Modules.Users.Core.Commands
{
    internal class ChargeFunds : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; }
        public decimal Amount { get; }

        public ChargeFunds(Guid userId, decimal amount)
        {
            UserId = userId;
            Amount = amount;
        }

        internal class Response
        {
            public bool Charged { get; }

            public Response(bool charged)
            {
                Charged = charged;
            }
        }
    }
}