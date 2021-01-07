using System;

namespace Trill.Modules.Ads.Core.DTO
{
    internal class AdDetailsDto : AdDto
    {
        public string Content { get; set; }
        public decimal Amount { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? RejectedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
}