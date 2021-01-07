using System;
using System.Collections.Generic;
using System.Linq;

namespace Trill.Shared.Abstractions.Queries
{
    public class Paged<T> : PagedBase
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        public bool IsEmpty => Items is null || !Items.Any();

        public Paged()
        {
        }

        public Paged(IEnumerable<T> items,
            int currentPage, int resultsPerPage,
            int totalPages, long totalResults) :
            base(currentPage, resultsPerPage, totalPages, totalResults)
        {
            Items = items;
        }

        public static Paged<T> Create(IEnumerable<T> items,
            int currentPage, int resultsPerPage,
            int totalPages, long totalResults)
            => new Paged<T>(items, currentPage, resultsPerPage, totalPages, totalResults);

        public static Paged<T> From(PagedBase result, IEnumerable<T> items)
            => new Paged<T>(items, result.CurrentPage, result.ResultsPerPage,
                result.TotalPages, result.TotalResults);

        public static Paged<T> Empty => new Paged<T>();

        public Paged<TResult> Map<TResult>(Func<T, TResult> map)
            => Paged<TResult>.From(this, Items.Select(map));
    }
}