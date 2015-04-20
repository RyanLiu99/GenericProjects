using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Generic.DataAccess
{

    public interface IDataRepository 
    {       
        IList<TDbModel> GetList<TDbModel>(
                Func<TDbModel, bool> where = null,
                string sortOrder = null,
                int? skip = null, 
                int? take = null,
                params Expression<Func<TDbModel, object>>[] navigationProperties) where TDbModel : class;


        Task<IList<TDbModel>> GetListAsync<TDbModel>(
        Func<TDbModel, bool> where = null,
        string sortOrder = null,
        int? skip = null,
        int? take = null,
        params Expression<Func<TDbModel, object>>[] navigationProperties) where TDbModel : class;


        /// <summary>
        /// support transforming and multiple porperty(even property of property) sorting, each has its own sort order
        /// </summary>
        /// <typeparam name="TDbModel"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <param name="where"></param>
        /// <param name="sortOrder">sth like [Status ASC,name DESC, result_file.Length], by default it is ASC, and asc/desc is not case senstive</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="navigationProperties"></param>
        /// <returns></returns>
        IList<TResult> GetListProjected<TDbModel, TResult>(
           Func<TDbModel, TResult> selector,
           Func<TDbModel, bool> where = null,
           string sortOrder = null,
           int? skip = null, 
           int? take = null,
           params Expression<Func<TDbModel, object>>[] navigationProperties) where TDbModel : class;

       
        Task<IList<TResult>> GetListProjectedAsync<TDbModel, TResult>(
         Func<TDbModel, TResult> selector,
         Func<TDbModel, bool> where = null,
         string sortOrder = null,
         int? skip = null,
         int? take = null,
         params Expression<Func<TDbModel, object>>[] navigationProperties) where TDbModel : class;

        TDbModel GetSingle<TDbModel>(
            Func<TDbModel, bool> where,
             params Expression<Func<TDbModel, object>>[] navigationProperties) where TDbModel : class;

        TResult GetSingleProjected<TDbModel,TResult>(
             Func<TDbModel, TResult> selector,
            Func<TDbModel, bool> where,
             params Expression<Func<TDbModel, object>>[] navigationProperties) where TDbModel : class;

        Task<TResult> GetSingleProjectedAsync<TDbModel, TResult>(
          Func<TDbModel, TResult> selector,
          Func<TDbModel, bool> where,
          params Expression<Func<TDbModel, object>>[] navigationProperties
          ) where TDbModel : class;

         Task<int> AddAsync<TDbModel>(params TDbModel[] items) where TDbModel : class;

        Task UpdateAsync<TDbModel>(params TDbModel[] items) where TDbModel : class;
        void Remove<TDbModel>(params TDbModel[] items) where TDbModel : class;

        Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);

        Task<TElement[]> QuerySqlCommandAsync<TElement>(string sql, params object[] parameters);

        Task<TElement> QuerySqlCommandSingleAsync<TElement>(string sql, params object[] parameters);


        
    }
}
