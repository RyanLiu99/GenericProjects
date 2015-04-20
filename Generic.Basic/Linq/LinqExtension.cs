using System;
using System.Collections.Generic;
using System.Linq;

namespace Generic.Basic.Linq
{
    public static class LinqExtension
    {

        public static IQueryable<TModel> FilterSortTake<TModel, TSortKey>(this IQueryable<TModel> query, Func<TModel, bool> where, Func<TModel, TSortKey> sortKeySelector, bool? isDesc, int? skip, int? take)
        {
            var newQuery = query;
            if (where != null) newQuery = newQuery.Where(where).AsQueryable();

            if (sortKeySelector != null && isDesc.HasValue)
            {
                if (isDesc.Value)
                    newQuery = newQuery.OrderByDescending(sortKeySelector).AsQueryable();
                else
                    newQuery = newQuery.OrderBy(sortKeySelector).AsQueryable();
            }

            if (skip.HasValue)
                newQuery = newQuery.Skip(skip.Value);


            if (take.HasValue)
                newQuery = newQuery.Take(take.Value);
            return newQuery;
        }

        /// <summary>
        /// support multiple porperty(even property of property) sorting, each has its own sort order
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="query"></param>
        /// <param name="where"></param>
        /// <param name="sortOrder">sth like [Status ASC,name DESC, result_file.Length], by default it is ASC, and asc/desc is not case senstive</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static IQueryable<TModel> FilterSortTake<TModel>(this IQueryable<TModel> query, Func<TModel, bool> where, string sortOrder, int? skip, int? take)
        {
            var newQuery = query;
            if (where != null) newQuery = newQuery.Where(where).AsQueryable();

            if (!string.IsNullOrWhiteSpace(sortOrder))
            {                
                newQuery = newQuery.DynamicSort(sortOrder);
            }

            if (skip.HasValue)
                newQuery = newQuery.Skip(skip.Value);


            if (take.HasValue)
                newQuery = newQuery.Take(take.Value);
            return newQuery;
        }


        public static void ForEach<T>(this IEnumerable<T> enumable, Action<T> action)
        {
            var enumarator = enumable.GetEnumerator();
            while (enumarator.MoveNext())
            {
                action(enumarator.Current);
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumable)
        {
            return enumable == null || !enumable.Any();
        }
        

    }
}
