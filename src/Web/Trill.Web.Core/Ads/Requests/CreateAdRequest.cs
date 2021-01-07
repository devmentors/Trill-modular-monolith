using System;
using System.Collections.Generic;

namespace Trill.Web.Core.Ads.Requests
{
    public class CreateAdRequest
    {
        public string Header { get; set; }
        public string Content { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}