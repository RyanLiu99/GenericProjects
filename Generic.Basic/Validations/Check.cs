using System;


namespace Generic.Basic.Validations
{
    [Obsolete("Please use CheckArgument class. And methods in this class will be moved into CheckArgument and other (new) classes over time")]
    public static class Check
    {
        #region Range Methods

        #region RangeMaxExclusive

        /// <summary>Range Maximum Test - value must be less than the maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="max">Maximum value</param>
        public static void RangeMaxExclusive(double val, string paramName, double max)
        {
            if (!(val < max))
            {
                throw new ArgumentException("The value, " + paramName + ", must be less than " + max + "; it's value is " + val + ".");
            }
        }

        /// <summary>Range Maximum Test - value must be less than the maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="max">Maximum value</param>
        public static void RangeMaxExclusive(DateTime val, string paramName, DateTime max)
        {
            if (!(val < max))
            {
                throw new ArgumentException(string.Format("The value, {0}, must be less than {1}; it's value is {2}.", paramName, max.ToShortDateString(), val.ToShortDateString()));
            }
        }

        /// <summary>Range Maximum Test - value must be less than the maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="max">Maximum value</param>
        public static void RangeMaxExclusive(int val, string paramName, int max)
        {
            RangeMaxExclusive((double) val, paramName, (double) max);
        }

        /// <summary>Range Maximum Test - value must be less than the maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="max">Maximum value</param>
        public static void RangeMaxExclusive(decimal val, string paramName, decimal max)
        {
            RangeMaxExclusive((double) val, paramName, (double) max);
        }

        /// <summary>Range Maximum Test - value must be less than the maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="max">Maximum value</param>
        public static void RangeMaxExclusive(float val, string paramName, float max)
        {
            RangeMaxExclusive((double) val, paramName, (double) max);
        }

        #endregion

        #region RangeMinExclusive

        /// <summary>Range Minimum Test - value must be greater than the minimum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        public static void RangeMinExclusive(double val, string paramName, double min)
        {
            if (!(val > min))
            {
                throw new ArgumentException("The value, " + paramName + ", must be greater than " + min + "; it's value is " + val + ".");
            }
        }

        /// <summary>Range Minimum Test - value must be greater than the minimum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        public static void RangeMinExclusive(DateTime val, string paramName, DateTime min)
        {
            if (!(val > min))
            {
                throw new ArgumentException(string.Format("The value, {0}, must be greater than {1}; it's value is {2}.", paramName, min.ToShortDateString(), val.ToShortDateString()));
            }
        }

        /// <summary>Range Minimum Test - value must be greater than the minimum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        public static void RangeMinExclusive(int val, string paramName, int min)
        {
            RangeMinExclusive((double) val, paramName, (double) min);
        }

        /// <summary>Range Minimum Test - value must be greater than the minimum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        public static void RangeMinExclusive(decimal val, string paramName, decimal min)
        {
            RangeMinExclusive((double) val, paramName, (double) min);
        }

        /// <summary>Range Minimum Test - value must be greater than the minimum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        public static void RangeMinExclusive(float val, string paramName, float min)
        {
            RangeMinExclusive((double) val, paramName, (double) min);
        }

        #endregion

        #region RangeMaxInclusive

        /// <summary>Range Maximum Test - value must be less than or equal to maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="max">Maximum value</param>
        public static void RangeMaxInclusive(double val, string paramName, double max)
        {
            if (!(val <= max))
            {
                throw new ArgumentException("The value, " + paramName + ", must be less than or equal to " + max + "; it's value is " + val + ".");
            }
        }

        /// <summary>Range Maximum Test - value must be less than or equal to maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="max">Maximum value</param>
        public static void RangeMaxInclusive(DateTime val, string paramName, DateTime max)
        {
            if (!(val <= max))
            {
                throw new ArgumentException(string.Format("The value, {0}, must be less than or equal to {1}; it's value is {2}.", paramName, max.ToShortDateString(), val.ToShortDateString()));
            }
        }

        /// <summary>Range Maximum Test - value must be less than the maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="max">Maximum value</param>
        public static void RangeMaxInclusive(int val, string paramName, int max)
        {
            RangeMaxInclusive((double) val, paramName, (double) max);
        }

        /// <summary>Range Maximum Test - value must be less than the maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="max">Maximum value</param>
        public static void RangeMaxInclusive(decimal val, string paramName, decimal max)
        {
            RangeMaxInclusive((double) val, paramName, (double) max);
        }

        /// <summary>Range Maximum Test - value must be less than the maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="max">Maximum value</param>
        public static void RangeMaxInclusive(float val, string paramName, float max)
        {
            RangeMaxInclusive((double) val, paramName, (double) max);
        }

        #endregion

        #region RangeMinInclusive

        /// <summary>Range Minimum Test - value must be greater than or equal to minimum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        public static void RangeMinInclusive(double val, string paramName, double min)
        {
            if (!(val >= min))
            {
                throw new ArgumentException("The value, " + paramName + ", must be greater or equal to " + min + "; it's value is " + val + ".");
            }
        }

        /// <summary>Range Minimum Test - value must be greater than or equal to minimum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        public static void RangeMinInclusive(DateTime val, string paramName, DateTime min)
        {
            if (!(val >= min))
            {
                throw new ArgumentException(string.Format("The value, {0}, must be greater than or equal to {1}; it's value is {2}.", paramName, min.ToShortDateString(), val.ToShortDateString()));
            }
        }

        /// <summary>Range Minimum Test - value must be greater than or equal to minimum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        public static void RangeMinInclusive(int val, string paramName, int min)
        {
            RangeMinInclusive((double) val, paramName, (double) min);
        }

        /// <summary>Range Minimum Test - value must be greater than or equal to minimum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        public static void RangeMinInclusive(decimal val, string paramName, decimal min)
        {
            RangeMinInclusive((double) val, paramName, (double) min);
        }

        /// <summary>Range Minimum Test - value must be greater than or equal to minimum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        public static void RangeMinInclusive(float val, string paramName, float min)
        {
            RangeMinInclusive((double) val, paramName, (double) min);
        }

        #endregion

        #region RangeInclusive

        /// <summary>Range Test - value must be greater than or equal to minimum, and less than or equal to maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        public static void RangeInclusive(double val, string paramName, double min, double max)
        {
            RangeMinInclusive(val, paramName, min);
            RangeMaxInclusive(val, paramName, max);
        }

        /// <summary>Range Test - value must be greater than or equal to minimum, and less than or equal to maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        public static void RangeInclusive(DateTime val, string paramName, DateTime min, DateTime max)
        {
            RangeMinInclusive(val, paramName, min);
            RangeMaxInclusive(val, paramName, max);
        }

        /// <summary>Range Test - value must be greater than or equal to minimum, and less than or equal to maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        public static void RangeInclusive(int val, string paramName, int min, int max)
        {
            RangeInclusive((double) val, paramName, (double) min, (double) max);
        }

        /// <summary>Range Test - value must be greater than or equal to minimum, and less than or equal to maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        public static void RangeInclusive(float val, string paramName, float min, float max)
        {
            RangeInclusive((double) val, paramName, (double) min, (double) max);
        }

        /// <summary>Range Test - value must be greater than or equal to minimum, and less than or equal to maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        public static void RangeInclusive(decimal val, string paramName, decimal min, decimal max)
        {
            RangeInclusive((double) val, paramName, (double) min, (double) max);
        }

        #endregion

        #region RangeExclusive

        /// <summary>Range Test - value must be greater than the minimum, and less than the maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        public static void RangeExclusive(double val, string paramName, double min, double max)
        {
            RangeMinExclusive(val, paramName, min);
            RangeMaxExclusive(val, paramName, max);
        }

        /// <summary>Range Test - value must be greater than the minimum, and less than the maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        public static void RangeExclusive(DateTime val, string paramName, DateTime min, DateTime max)
        {
            RangeMinExclusive(val, paramName, min);
            RangeMaxExclusive(val, paramName, max);
        }

        /// <summary>Range Test - value must be greater than the minimum, and less than the maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        public static void RangeExclusive(int val, string paramName, int min, int max)
        {
            RangeExclusive((double) val, paramName, (double) min, (double) max);
        }

        /// <summary>Range Test - value must be greater than the minimum, and less than the maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        public static void RangeExclusive(decimal val, string paramName, decimal min, decimal max)
        {
            RangeExclusive((double) val, paramName, (double) min, (double) max);
        }

        /// <summary>Range Test - value must be greater than the minimum, and less than the maximum</summary>
        /// <param name="val">Value to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        public static void RangeExclusive(float val, string paramName, float min, float max)
        {
            RangeExclusive((double) val, paramName, (double) min, (double) max);
        }

        #endregion

        #endregion

        /// <summary>Not Empty Test - String cannot be empty</summary>
        /// <param name="str">string to test</param>
        /// <param name="paramName">Parameter Name to test</param>
        /// <param name="message">Message to raise as exception if string is empty</param>
        public static void NotEmpty(string str, string paramName, string message)
        {
            if (str == null || string.IsNullOrEmpty(str))
            {
                throw new ArgumentException(message, paramName);
            }
        }
      

        #region NotNull

 

        /// <summary>Not Null Test - Object cannot be null</summary>
        /// <param name="obj">Object to test</param>
        /// <param name="objectName">Name of object to test</param>
        /// <param name="message">Message to raise as exception if object is empty</param>
        public static void NotNull(object obj, string objectName, string message)
        {
            if (obj == null)
            {
                throw new NullReferenceException(string.Format("The object, {0}, can not be null. {1}", objectName, message));
            }
        }

        #endregion

        /// <summary>Not True Test - boolean value must be false</summary>
        /// <param name="condition">Boolean value to test</param>
        /// <param name="conditionName">name of Boolean value to test</param>
        public static void NotTrue(bool condition, string conditionName)
        {
            True(!condition, conditionName);
        }

        /// <summary>Not True Test - boolean value must be false</summary>
        /// <param name="condition">Boolean value to test</param>
        /// <param name="conditionName">name of Boolean value to test</param>
        /// <param name="message">Message to raise as exception if condition fails</param>
        public static void NotTrue(bool condition, string conditionName, string message)
        {
            True(!condition, conditionName, message);
        }

        /// <summary>True Test - boolean value must be true</summary>
        /// <param name="condition">Boolean value to test</param>
        /// <param name="conditionName">name of Boolean value to test</param>
        public static void True(bool condition, string conditionName)
        {
            True(condition, conditionName, null);
        }

        /// <summary>True Test - boolean value must be true</summary>
        /// <param name="condition">Boolean value to test</param>
        /// <param name="conditionName">name of Boolean value to test</param>
        /// <param name="message">Message to raise as exception if condition fails</param>
        public static void True(bool condition, string conditionName, string message)
        {
            if (!condition)
            {
                throw new ApplicationException(string.Format("The condition, {0}, is not true. {1}", conditionName, message));
            }
        }

        /// <summary>Object Type Test - object must be of specified type</summary>
        /// <param name="obj">Object to test</param>
        /// <param name="type">Type to compare</param>
        public static void TypeOf(object obj, Type type)
        {
            TypeOf(obj, type, null);
        }

        /// <summary>Object Type Test - object must be of specified type</summary>
        /// <param name="obj">Object to test</param>
        /// <param name="type">Type to compare</param>
        /// <param name="message">Message to raise as exception if condition fails</param>
        public static void TypeOf(object obj, Type type, string message)
        {
            if (obj.GetType() != type)
            {
                throw new ApplicationException(string.Format("The object, {0}, is not of type {1}. {2}", obj.ToString(), type.FullName, message));
            }
        }
    }
}