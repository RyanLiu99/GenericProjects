using Generic.Basic.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Generic.DataAccess.EF
{
    public abstract class BaseDataRepositoryEF : IDataRepository
    {

        public BaseDataRepositoryEF()
        {
        }

        protected abstract DbContext GetDbContext();

        public IList<TDbModel> GetList<TDbModel>(
                Func<TDbModel, bool> where = null,
                string sortOrder = null,
                int? skip = null,
                int? take = null,
                params Expression<Func<TDbModel, object>>[] navigationProperties) where TDbModel : class
        {
            return GetListAsync(where, sortOrder, skip, take, navigationProperties).Result;
        }

        public async Task<IList<TDbModel>> GetListAsync<TDbModel>(
                Func<TDbModel, bool> where = null,
                string sortOrder = null,
                int? skip = null,
                int? take = null,
                params Expression<Func<TDbModel, object>>[] navigationProperties) where TDbModel : class
        {

            using (var context = GetDbContext())
            {
                IQueryable<TDbModel> dbQuery = context.Set<TDbModel>();

                //Apply eager loading
                foreach (Expression<Func<TDbModel, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<TDbModel, object>(navigationProperty);

                dbQuery = dbQuery.FilterSortTake(where, sortOrder, skip, take);

                var query = dbQuery.AsNoTracking();
                var list = await query.ToListAsync<TDbModel>();
                return list;
            }

        }

        public IList<TResult> GetListProjected<TDbModel, TResult>(
          Func<TDbModel, TResult> selector,
          Func<TDbModel, bool> where = null,
          string sortOrder = null,
          int? skip = null,
          int? take = null,
            params Expression<Func<TDbModel, object>>[] navigationProperties) where TDbModel : class
        {
            return GetListProjectedAsync(selector, where, sortOrder, skip, take, navigationProperties).Result;
        }

        public Task<IList<TResult>> GetListProjectedAsync<TDbModel, TResult>(
            Func<TDbModel, TResult> selector,
            Func<TDbModel, bool> where = null,
            string sortOrder = null,
            int? skip = null,
            int? take = null,
              params Expression<Func<TDbModel, object>>[] navigationProperties) where TDbModel : class
        {
            if (selector == null) throw new ArgumentNullException("selector");

            using (var context = GetDbContext())
            {
                IQueryable<TDbModel> dbQuery = context.Set<TDbModel>();

                //Apply eager loading
                foreach (Expression<Func<TDbModel, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<TDbModel, object>(navigationProperty);

                dbQuery = dbQuery.FilterSortTake(where, sortOrder, skip, take);

                var result = dbQuery.AsNoTracking().Select(selector).AsQueryable().ToList(); // .ToListAsync();  not working http://go.microsoft.com/fwlink/?LinkId=287068.
                return Task.FromResult((IList<TResult>)result);
            }
        }


        public virtual TDbModel GetSingle<TDbModel>(Func<TDbModel, bool> where,
             params Expression<Func<TDbModel, object>>[] navigationProperties) where TDbModel : class
        {
            TDbModel item = null;
            using (var context = GetDbContext())
            {
                IQueryable<TDbModel> dbQuery = context.Set<TDbModel>();

                //Apply eager loading
                foreach (Expression<Func<TDbModel, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<TDbModel, object>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }
            return item;
        }


        public TResult GetSingleProjected<TDbModel, TResult>(
          Func<TDbModel, TResult> selector,
          Func<TDbModel, bool> where,
          params Expression<Func<TDbModel, object>>[] navigationProperties
          ) where TDbModel : class
        {

            using (var context = GetDbContext())
            {
                IQueryable<TDbModel> dbQuery = context.Set<TDbModel>();

                //Apply eager loading
                foreach (Expression<Func<TDbModel, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<TDbModel, object>(navigationProperty);

                return dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item                    
                    .Where(where) //Apply where clause
                    .Select(selector)
                    .SingleOrDefault();
            }
        }

        public Task<TResult> GetSingleProjectedAsync<TDbModel, TResult>(
            Func<TDbModel, TResult> selector,
            Func<TDbModel, bool> where,
            params Expression<Func<TDbModel, object>>[] navigationProperties
        ) where TDbModel : class
        {         
            var result  =GetSingleProjected<TDbModel, TResult>(
            selector,
            where,
            navigationProperties);

            return Task.FromResult(result);
        }

        public virtual async Task<int> AddAsync<TDbModel>(params TDbModel[] items) where TDbModel : class
        {
            using (var context = GetDbContext())
            {
                foreach (TDbModel item in items)
                {
                    context.Entry(item).State = System.Data.Entity.EntityState.Added;
                }
                return await context.SaveChangesAsync();
            }
        }

        public virtual async Task UpdateAsync<TDbModel>(params TDbModel[] items) where TDbModel : class
        {
            using (var context = GetDbContext())
            {
                foreach (TDbModel item in items)
                {

                    context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }
                await context.SaveChangesAsync();
            }
        }


        public virtual void Remove<TDbModel>(params TDbModel[] items) where TDbModel : class
        {
            using (var context = GetDbContext())
            {
                foreach (TDbModel item in items)
                {
                    context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }


        public Task<int> ExecuteSqlCommandAsync(string sql, object[] parameters) //this really just ADO.net code
        {
            using (var context = GetDbContext())
            {
                var result = context.Database.ExecuteSqlCommandAsync(sql, parameters);
                return result;
            }
        }



        public Task<TElement[]> QuerySqlCommandAsync<TElement>(string sql, params object[] parameters)
        {
            using (var context = GetDbContext())
            {
                var result = context.Database.SqlQuery<TElement>(sql, parameters).ToArrayAsync();
                return result;
            }
        }

        public Task<TElement> QuerySqlCommandSingleAsync<TElement>(string sql, params object[] parameters)
        {
            using (var context = GetDbContext())
            {
                var result = context.Database.SqlQuery<TElement>(sql, parameters).SingleOrDefaultAsync();
                return result;
            }
        }



    }
}
