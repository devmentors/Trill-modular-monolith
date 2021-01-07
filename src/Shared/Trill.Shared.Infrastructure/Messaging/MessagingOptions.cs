using Trill.Shared.Infrastructure.Messaging.Inbox;
using Trill.Shared.Infrastructure.Messaging.Outbox;

namespace Trill.Shared.Infrastructure.Messaging
{
    internal class MessagingOptions
    {
        public bool UseBackgroundDispatcher { get; set; }
        public InboxOptions Inbox { get; set; }
        public OutboxOptions Outbox { get; set; }
    }
}