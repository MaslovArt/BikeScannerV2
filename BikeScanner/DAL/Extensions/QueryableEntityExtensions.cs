using System;
using BikeScanner.Domain.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace BikeScanner.DAL.Extensions
{
    public static class QueryableEntityExtensions
    {
        public static Task<T> GetByIdAsync<T>(this IQueryable<T> queryable, int id) where T : EntityBase =>
            queryable.FirstOrDefaultAsync(x => x.Id == id);

        public static IQueryable<T> WithId<T>(this IQueryable<T> queryable, int id) where T : EntityBase =>
            queryable.Where(x => x.Id == id);

        public static Task<bool> ExistsAsync<T>(this IQueryable<T> queryable, int id) where T : EntityBase =>
            queryable.WithId(id).AnyAsync();
    }

}

