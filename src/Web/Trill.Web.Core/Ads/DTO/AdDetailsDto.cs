using System;

namespace Trill.Web.Core.Ads.DTO
{
    public class AdDetailsDto : AdDto
    {
        public string Content { get; set; }
        public decimal Amount { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public DateTime? RejectedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? PublishedAt { get; set; }
    }
}