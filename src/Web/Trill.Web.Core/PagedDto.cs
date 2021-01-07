using System.Collections.Generic;
using System.Linq;

namespace Trill.Web.Core
{
    public class PagedDto<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public bool Empty => Items is null || !Items.Any();
        public int CurrentPage { get; set; }
        public int ResultsPerPage { get; set; }
        public int TotalPages { get; set; }
        public long TotalResults { get; set; }
    }
}