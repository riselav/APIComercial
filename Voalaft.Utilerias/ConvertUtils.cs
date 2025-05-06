

namespace Voalaft.Utilerias
{
    public class ConvertUtils
    {
        public static Int32 ToInt32(object value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }

        public static Int16 ToInt16(object value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch
            {
                return 0;
            }
        }

        public static Int64 ToInt64(object value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch
            {
                return 0;
            }
        }

        public static String ToString(object value)
        {
            try
            {
                return Convert.ToString(value);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static Boolean ToBoolean(object value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }

        public static DateTime ToDateTime(object value)
        {
            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public static Decimal ToDecimal(object value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch
            {
                return 0;
            }
        }

    }
}
