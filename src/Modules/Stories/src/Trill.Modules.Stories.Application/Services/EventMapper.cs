using System.Collections.Generic;
using System.Linq;
using Trill.Modules.Stories.Application.Events;
using Trill.Modules.Stories.Core.Events;
using Trill.Shared.Abstractions.Kernel;
using Trill.Shared.Abstractions.Messaging;

namespace Trill.Modules.Stories.Application.Services
{
    internal class EventMapper : IEventMapper
    {
        public IEnumerable<IMessage> Map(params IDomainEvent[] events) => events.Select(Map);

        private static IMessage Map(IDomainEvent @event)
            => @event switch
            {
                StoryCreated e => new StorySent(e.Story.Id,
                    new StorySent.AuthorModel(e.Story.Author.Id, e.Story.Author.Name),
                    e.Story.Title, e.Story.Tags, e.Story.CreatedAt,
                    new StorySent.VisibilityModel(e.Story.Visibility.From, e.Story.Visibility.To,
                        e.Story.Visibility.Highlighted)),
                _ => null
            };
    }
}