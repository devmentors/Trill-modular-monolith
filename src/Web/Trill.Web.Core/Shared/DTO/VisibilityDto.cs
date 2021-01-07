using System;

namespace Trill.Web.Core.Shared.DTO
{
    public class VisibilityDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Highlighted { get; set; }
    }
}