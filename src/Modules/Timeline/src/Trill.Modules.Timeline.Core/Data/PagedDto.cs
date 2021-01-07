using System.Collections.Generic;
using System.Linq;

namespace Trill.Modules.Timeline.Core.Data
{
    internal class Paged<T>
    {
        public IEnumerable<T> Items { get; set; }
        public bool Empty => Items is null || !Items.Any();
        public int CurrentPage { get; set; }
        public int ResultsPerPage { get; set; }
        public int TotalPages { get; set; }
        public long TotalResults { get; set; }
    }
}