using System;
using BikeScanner.Domain.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace BikeScanner.DAL.Extensions
{
	public static class QueryableDctEntityExtensions
	{
        public static Task<int> GetIdByCodeAsync<T>(this DbSet<T> dbSet, string code) where T : DctBase =>
            dbSet
                .Where(x => x.Code == code)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

        public static Task<T> GetByCodeAsync<T>(this IQueryable<T> queryable, string code) where T : DctBase =>
            queryable.FirstOrDefaultAsync(x => x.Code == code);

        public static IQueryable<T> WithCode<T>(this IQueryable<T> queryable, string code) where T : DctBase =>
            queryable.Where(x => x.Code == code);

        public static Task<bool> ExistsAsync<T>(this IQueryable<T> queryable, string code) where T : DctBase =>
            queryable.WithCode(code).AnyAsync();

    }
}

