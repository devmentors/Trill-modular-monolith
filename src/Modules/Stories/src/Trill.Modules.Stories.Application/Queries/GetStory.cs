using System;
using Trill.Modules.Stories.Application.DTO;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Modules.Stories.Application.Queries
{
    internal class GetStory : IQuery<StoryDetailsDto>
    {
        public long StoryId { get; set; }
        public Guid? UserId { get; set; }
    }
}