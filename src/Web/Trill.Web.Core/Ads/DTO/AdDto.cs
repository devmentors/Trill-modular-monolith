using System;
using System.Collections.Generic;

namespace Trill.Web.Core.Ads.DTO
{
    public class AdDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Header { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string State { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}