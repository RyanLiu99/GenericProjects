using Generic.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.DataAccess.EF
{
    public class DataAccessEF : IDataAccess, IDisposable
    {
        private DbContext _dbContext;


        public DataAccessEF(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<DbEntity> All<DbEntity>() where DbEntity : class
        {
            return _dbContext.Set<DbEntity>().AsQueryable();
        }

        public void Insert<DbEntity>(DbEntity entity) where DbEntity : class
        {
            //_dbContext.Entry(entity).State = EntityState.Added;
            _dbContext.Set<DbEntity>().Add(entity);
            
        }

        public void Update<DbEntity>(DbEntity entity) where DbEntity : class
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete<DbEntity>(params object[] keyValues) where DbEntity : class
        {
            var set = _dbContext.Set<DbEntity>();
            var entity = set.Find(keyValues);
            set.Remove(entity);
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
