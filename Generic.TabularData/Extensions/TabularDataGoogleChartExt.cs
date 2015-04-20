using System.Linq;
using System.Text;
using System;

namespace Generic.TabularData.Extensions
{
    public static class TabularDataGoogleChartExt
    {

        /// <summary>
        /// Generate string can be used as Goolge Chart Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToGoogleChartJsonTable(this TabularTable data)
        {
            StringBuilder sb = new StringBuilder();  
            sb.Append("{\"cols\" : [");
            sb.Append(string.Join(",", data.cols.Select( c => c.ToGoogleChartJsonCol())));
            sb.Append("]"); //end of cols            
            sb.Append(", \"rows\":[");
            if (data.rows.Any())
            {
                foreach (var row in data.rows)
                {
                    sb.Append("{ \"c\":[");                    
                    //add each cells
                    //for (int i = 0; i < row.Count(); i++ )
                    int cellIndex = 0;
                    foreach (var cell in row)
                    {
                        var col = data.cols[cellIndex++];
                        col.ToGoogleChartJsonCell(cell, sb).Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1); //remove last extra , brought by last cell
                    sb.Append("]},"); //end of each row
                }
                sb.Remove(sb.Length - 1, 1); //remove last extra brought by last row ,
            }            
            sb.Append("]}"); //end of rows and table
            return sb.ToString();
        }

        public static string ToGoogleChartJsonCol(this TabularColumn col)
        {            
            var  sb = new StringBuilder(60);
            sb.Append("{");

            sb.AppendFormat("\"label\":\"{0}\", \"type\" : \"{1}\"", col.lable.EscapeJSON(), col.DataType.ToGoogleChartDataType());

            if (!string.IsNullOrWhiteSpace(col.id))
                sb.AppendFormat(", \"id\":\"{0}\"", col.id.EscapeJSON());

            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// append sth like: { v: 12, f: "Ony 12 items" }
        /// </summary>
        /// <param name="col"></param>
        /// <param name="cellValue"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static StringBuilder ToGoogleChartJsonCell( this TabularColumn col,  object cellValue, StringBuilder sb = null)
        {
            if(sb == null) 
                sb = new StringBuilder(20);

            bool isNull = (cellValue == null || cellValue == System.DBNull.Value);
                      
            sb.Append("{ \"v\":");

            if (col.DataType == typeof(bool))
            {
                if (isNull)
                    sb.Append("null");
                else
                {
                    sb.Append(cellValue);
                }
            }
            else if (col.DataType.IsDateTime())
            {
                if (isNull)
                    sb.Append("null");
                else
                {
                    var d = (DateTime)cellValue;
                    sb.AppendFormat("\"Date({0},{1},{2},{3},{4},{5},{6})\"", d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, d.Millisecond);
                }
            }
            else if (col.DataType.IsNumeric())
                sb.Append(isNull ? 0 : cellValue);
            else
            {
                if (isNull)
                    sb.Append("null");
                else
                    sb.AppendFormat("\"{0}\"", cellValue.ToString().EscapeJSON());
            }

            if (col.CellValueFormatter != null)
            {
                sb.AppendFormat(", \"f\": \"{0}\"", col.CellValueFormatter(cellValue).EscapeJSON());
            }
            sb.Append("}");                      
            return sb;
        }

        public static string ToGoogleChartDataType(this Type type)
        {
            if (type.IsNumeric())
                return "number";
            if (type.IsDateTime())
                return "datetime"; //google type fo date, timeofday  is hard to translate
            if (type == typeof(bool))
                return "boolean";
            return "string";
        }
                       
        public static bool IsNumeric(this Type vauleType)
        {
            if (vauleType == typeof(int) || vauleType == typeof(short) || vauleType == typeof(uint) || vauleType == typeof(ushort) ||
                vauleType == typeof(long) || vauleType == typeof(ulong) || vauleType == typeof(decimal) || vauleType == typeof(double))
            {
                return true;
            }
            return false;
        }

        public static bool IsDateTime(this Type vauleType)
        {
            if (vauleType == typeof(System.DateTime) || vauleType == typeof(System.DateTimeOffset))
                return true;
            else 
                return false;
        }

        public static string EscapeJSON(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return input.Replace("\"", "\\\"");
        }
    }
}
