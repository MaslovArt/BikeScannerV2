using System;
using System.Linq;
using BikeScanner.Domain.Models.Base;
using BikeScanner.Domain.States;

namespace BikeScanner.DAL.Extensions
{
    public static class QueryableStatefulExtensions
    {
        public static IQueryable<T> WithActiveState<T>(this IQueryable<T> queryable) where T : StatefulCrudBase =>
            queryable.Where(x => x.State == BaseStates.Active.ToString());

        public static IQueryable<T> WithState<T>(this IQueryable<T> queryable, Enum state) where T : StatefulCrudBase =>
            queryable.Where(x => x.State == state.ToString());

        public static IQueryable<T> WithStates<T>(this IQueryable<T> queryable, Enum[] states) where T : StatefulCrudBase =>
            queryable.Where(x => states.Select(s => s.ToString()).Contains(x.State));

        //public static IQueryable<T> ActiveItems<T>(this IQueryable<T> queryable) where T : StatefulCrudBase =>
        //    queryable.Where(x => x.State == CrudStates.Active);

        //public static IQueryable<T> ExcludeDeleted<T>(this IQueryable<T> queryable) where T : StatefulCrudBase =>
        //    queryable.Where(x => x.State != CrudStates.Deleted);
    }

}

