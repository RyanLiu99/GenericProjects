using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.TabularData.Extensions
{
    public static class TabularDataDataTableJSExtension
    {
        public static string ToDataTableJSON(this TabularTable data)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"{""aaData"": [");
            if (data.rows.Any())
            {
                foreach (var row in data.rows)
                {
                    sb.AppendFormat("[");
                    foreach (var cell in row)
                    {
                        sb.AppendFormat("\"{0}\",", cell == null ? string.Empty : cell.ToString().EscapeJSON());
                    }
                    sb.Remove(sb.Length - 1, 1); //remove last extra , brought by last cell
                    sb.AppendFormat("],");
                }
                sb.Remove(sb.Length - 1, 1); //remove last extra , brought by last row 
            }
            sb.AppendFormat("], \"iTotalDisplayRecords\" : {0}, \"iTotalRecords\": {1} }}", data.TotalRows, data.TotalRows);
            return sb.ToString();        
        }
    }
}
