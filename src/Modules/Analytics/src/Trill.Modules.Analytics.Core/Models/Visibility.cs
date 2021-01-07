using System;

namespace Trill.Modules.Analytics.Core.Models
{
    internal class Visibility
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool Highlighted { get; set; }
    }
}