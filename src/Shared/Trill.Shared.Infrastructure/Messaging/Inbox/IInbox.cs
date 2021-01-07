using System;
using System.Threading.Tasks;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Shared.Infrastructure.Messaging.Inbox
{
    internal interface IInbox
    {
        bool Enabled { get; }
        Task HandleAsync(IMessage message, Func<Task> handler, string module);
    }
}