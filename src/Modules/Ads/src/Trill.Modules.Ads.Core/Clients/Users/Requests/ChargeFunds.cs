using System;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Ads.Core.Clients.Users.Requests
{
    [Message("users")]
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

        private class Contract : Contract<ChargeFunds>
        {
            public Contract()
            {
                RequireAll();
            }
        }

        [Message("users")]
        internal class Response
        {
            public bool Charged { get; }

            public Response(bool charged)
            {
                Charged = charged;
            }

            private class Contract : Contract<Response>
            {
                public Contract()
                {
                    RequireAll();
                }
            }
        }
    }
}