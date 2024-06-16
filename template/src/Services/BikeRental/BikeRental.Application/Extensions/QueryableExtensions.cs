using BuildingBlocks.Common;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Application.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedItem<TResponse>> PaginateAsync<TResponse>(
        this IQueryable<TResponse> queryable, int skip, int take, Func<IQueryable<TResponse>, IOrderedQueryable<TResponse>>? orderBy = null)
        where TResponse : class
        {
            var records = await queryable.CountAsync();

            if (records > 0)
            {
                if (orderBy != null)
                {
                    queryable = orderBy(queryable);
                }

                var results = await queryable.Skip(skip).Take(take).ToListAsync();
                var pages = (int)Math.Ceiling((double)records / take);
                return new PaginatedItem<TResponse>(records, pages, results);
            }

            return new PaginatedItem<TResponse>(records, 1, new List<TResponse>());
        }
    }
}
