using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.DataAccess.EF
{
    public class DataContextEF : DataContext<DbContext>
    {
        public DataContextEF(DbContext dbContext)
            : base(dbContext)
        {
        }

        public override void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
