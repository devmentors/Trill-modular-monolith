using System.Collections.Generic;
using Trill.Shared.Abstractions.Kernel;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Stories.Application.Services
{
    public interface IEventMapper
    {
        IEnumerable<IMessage> Map(params IDomainEvent[] events);
    }
}