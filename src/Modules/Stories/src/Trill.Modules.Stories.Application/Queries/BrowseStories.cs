using System;
using Trill.Modules.Stories.Application.DTO;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Modules.Stories.Application.Queries
{
    internal class BrowseStories : PagedQueryBase, IQuery<Paged<StoryDto>>
    {
        public string Query { get; set; }
        public DateTime Now { get; set; } = DateTime.UtcNow;
    }
}