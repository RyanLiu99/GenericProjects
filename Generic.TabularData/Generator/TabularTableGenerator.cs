using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.TabularData.Generator
{
    public class TabularTableGenerator
    {
        //to do: make cols optional
        public static TabularTable FromDataReader(IDataReader dataReader, IList<TabularColumn> cols)
        {
            //to do: input parameter check
            var rows = new List<List<object>>();
            TabularTable table = new TabularTable(cols, rows);
            while (dataReader.Read())
            {
                var row = new List<object>(cols.Count);
                foreach (var col in cols)
                {
                    row.Add(dataReader[col.id]);
                }
                rows.Add(row);
            }
            return table;
        }

        public static TabularTable FromDataTable(DataTable dataTable, IList<TabularColumn> cols)
        {
            //to do: input parameter check
            var rows = new List<List<object>>();
            TabularTable table = new TabularTable(cols, rows);
            foreach (DataRow dr in dataTable.Rows)
            {
                var row = new List<object>(cols.Count);
                foreach (var col in cols)
                {                    
                    row.Add(dr[col.id]);
                }
                rows.Add(row);
            }
            return table;
        }

        public static TabularTable FromEnumerableObjects<T>(IEnumerable<T> objects, IList<TabularColumn> cols)
        {            
            var rows = new List<List<object>>();
            TabularTable table = new TabularTable(cols, rows);
            foreach(var obj in objects)
            {
                var row = new List<object>(cols.Count);
                foreach (var col in cols)
                {
                    row.Add(GetValue(obj, col.id));
                }
                rows.Add(row);
            }
            return table;
        }

        private static object GetValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
    }
}
