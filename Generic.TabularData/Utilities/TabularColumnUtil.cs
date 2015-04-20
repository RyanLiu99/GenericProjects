using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic.TabularData.Utilities
{
    public static class TabularDataUtil
    {
        public static readonly Func<object, string> ToCurreny = v => ((decimal)v).ToString("c");
        public static readonly Func<object, string> ToPercent = v => ((decimal)v).ToString("p");
        public static readonly Func<object, string> AddPercent = v => ((decimal)v).ToString("F") + "%";
        public static readonly Func<object, string> FormatBool = v => ((bool)v) ? "Y" : "N";


        public static string ToMonth(object arg)
        {
            return DateTimeFormatInfo.InvariantInfo.GetMonthName((int)arg).Substring(0,3);
        }
    }
}
