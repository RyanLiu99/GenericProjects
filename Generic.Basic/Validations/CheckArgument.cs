using Generic.Basic.Linq;
using System;
using System.Collections.Generic;

namespace Generic.Basic.Validations
{
    /// <summary>
    /// check parameters, and thorw ArgumentException  or ArgumentNullException
    /// </summary>
    public static class CheckArgument
    {
        public static void NotNull(object paramValue, string paramName)
        {
            if (paramValue == null)
                throw new ArgumentNullException("paramName");
        }

        public static void NotNullOrWhiteSpace(string str, string paramName)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException(paramName + ", can't be null or just white space.", paramName);
            }
        }


        public static void NotNullOrEmpty(string str, string paramName)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException(paramName + ", can't be null or empty.", paramName);
            }
        }

        

        public static void NotNullOrEmpty<T>(IEnumerable<T> array, string paramName)
        {
            if (array.IsNullOrEmpty())
            {
                throw new ArgumentException(paramName + ", can't be null or empty.", paramName);
            }
        }

        public static void NotDefault<T>(T paramValue, string paramName)
        {
            T defaultT = default(T);

            //or uses Comparer.Equals(defaultT, paramValue) 
            if (defaultT == null)
            {
                if (paramValue == null)
                    throw new ArgumentNullException(paramName);
            }
            else
            {
                if (defaultT.Equals(paramValue))
                    throw new ArgumentException(paramName + " just has default value " + defaultT, paramName);
            }
        }


        public static void NotFalse(bool condition, string message)
        {
            if (condition == false)
                throw new ArgumentException(message);
        }
    }
}
