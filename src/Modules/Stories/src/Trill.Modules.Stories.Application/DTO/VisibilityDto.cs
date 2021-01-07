using System;

namespace Trill.Modules.Stories.Application.DTO
{
    internal class VisibilityDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Highlighted { get; set; }
    }
}