using System;
using System.Collections.Generic;

namespace Trill.Web.Core.Shared.DTO
{
    public class StoryDto
    {
        public long Id { get; set; }
        public AuthorDto Author { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public DateTime CreatedAt { get; set; }
        public VisibilityDto Visibility { get; set; }
        public int TotalRate { get; set; }
    }
}