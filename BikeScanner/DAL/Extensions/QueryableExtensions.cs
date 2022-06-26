using System;
using System.Linq.Expressions;

namespace BikeScanner.DAL.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> expr, bool condition) =>
            condition ? queryable.Where(expr) : queryable;

        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, int page, int limit) =>
            queryable.Skip(limit * (page - 1)).Take(limit);
    }

}

