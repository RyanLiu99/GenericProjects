using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Generic.TabularData
{
    [Serializable]
    public class TabularTable
    {
        public IList<TabularColumn> cols { get; private set; }
        public IEnumerable<IEnumerable<object>> rows { get; private set; }

        public int TotalRows { get; set; }

         public TabularTable(IList<TabularColumn> columns, IEnumerable<IEnumerable<object>> rows)
        {
            this.cols = columns;
            this.rows = rows;
        }
    }
}