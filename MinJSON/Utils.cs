using System;
using System.Collections;

namespace MinJSON
{
    public class Utils
    {
        public static bool isDictionaryLike(Type t)
        {
            return t.GetInterface(nameof(IDictionary)) != null;
        }
        public static bool isEnumerable(Type t)
        {
            return t.GetInterface(nameof(IEnumerable)) != null;
        }

        public static bool isNumeric(Type t)
        {
            switch(Type.GetTypeCode(t))
            {
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.SByte:
                    return true;
                default:
                    return false;
            }
        }
    }
}
