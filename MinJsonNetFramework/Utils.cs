using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace MinJSON
{
    public static class Utils
    {
        public static bool isDictionaryLike(this Type t)
        {
            return t.GetInterface(nameof(IDictionary)) != null;
        }
        public static bool isListLike(this Type t)
        {
            return t.GetInterface(nameof(IList)) != null;
        }

        public static bool isPrimitiveValue(this Type t)
        {
            return t.IsPrimitive || t.Equals(typeof(String));
        }

        public static bool isNumeric(this Type t)
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

        public static string escapeJsonString(string jstr)
        {
            StringBuilder sb = new StringBuilder();
            foreach(char c in jstr)
            {
                switch(c)
                {
                    case '\\':
                        sb.Append(@"\\");
                        break;
                    case '"':
                        sb.Append("\\\"");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}
