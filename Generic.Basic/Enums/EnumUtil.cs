using System;
using System.Linq;

namespace Generic.Basic.Enums
{
    public class EnumUtil
    {
        public static string GetEnumNames<TEnum>(string separator = ", ")
        {
            var result = string.Join(separator, Enum.GetNames(typeof(TEnum))
                .Select(t => "'" + t + "'"));

            return result;
        }
    }
}
