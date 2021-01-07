using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Shared.Infrastructure.Mongo
{
    public static class Pagination
    {
        public static async Task<Paged<T>> PaginateAsync<T>(this IMongoQueryable<T> collection, IPagedQuery query)
            => await collection.PaginateAsync(query.Page, query.Results);

        public static async Task<Paged<T>> PaginateAsync<T>(this IMongoQueryable<T> collection,
            int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (resultsPerPage <= 0)
            {
                resultsPerPage = 10;
            }
            var isEmpty = await collection.AnyAsync() == false;
            if (isEmpty)
            {
                return Paged<T>.Empty;
            }
            var totalResults = await collection.CountAsync();
            var totalPages = (int)Math.Ceiling((decimal)totalResults / resultsPerPage);
            var data = await collection.Limit(page, resultsPerPage).ToListAsync();

            return Paged<T>.Create(data, page, resultsPerPage, totalPages, totalResults);
        }

        public static IMongoQueryable<T> Limit<T>(this IMongoQueryable<T> collection, IPagedQuery query)
            => collection.Limit(query.Page, query.Results);

        public static IMongoQueryable<T> Limit<T>(this IMongoQueryable<T> collection,
            int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (resultsPerPage <= 0)
            {
                resultsPerPage = 10;
            }
            var skip = (page - 1) * resultsPerPage;
            var data = collection.Skip(skip)
                .Take(resultsPerPage);

            return data;
        }
    }
}