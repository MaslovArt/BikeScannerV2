using System;
using System.Linq;
using BikeScanner.Domain.Models.Base;

namespace BikeScanner.DAL.Extensions
{
    public static class QueryableStatefulExtensions
    {
        public static IQueryable<T> WithState<T>(this IQueryable<T> queryable, string state) where T : StatefulCrudBase =>
            queryable.Where(x => x.State == state);

        public static IQueryable<T> WithStates<T>(this IQueryable<T> queryable, string[] states) where T : StatefulCrudBase =>
            queryable.Where(x => states.Contains(x.State));

        //public static IQueryable<T> ActiveItems<T>(this IQueryable<T> queryable) where T : StatefulCrudBase =>
        //    queryable.Where(x => x.State == CrudStates.Active);

        //public static IQueryable<T> ExcludeDeleted<T>(this IQueryable<T> queryable) where T : StatefulCrudBase =>
        //    queryable.Where(x => x.State != CrudStates.Deleted);
    }

}

