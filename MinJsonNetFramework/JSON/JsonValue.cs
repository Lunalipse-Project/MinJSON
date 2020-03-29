using MinJSON.Exception;
using System;

namespace MinJSON.JSON
{
    public class JsonValue
    {
        public bool isPrimitiveValue { get; private set; }
        public bool isNullValue { get; private set; }
        public bool isArray { get; private set; }
        public bool isObject { get; private set; }

        public JsonValueType ValueType { get; private set; }

        public JsonValue(JsonValueType valueType)
        {
            ValueType = valueType;
            isNullValue = (valueType == JsonValueType.JNull);
            isPrimitiveValue = (valueType == JsonValueType.JPrimitive);
            isArray = (valueType == JsonValueType.JArray);
            isObject = (valueType == JsonValueType.JObject);
        }

        /// <summary>
        /// Convert to desire type
        /// </summary>
        /// <exception cref="Exception.JsonValueConversionException"></exception>
        /// <returns></returns>
        public virtual T As<T>()
        {
            object obj = AsType(typeof(T));
            if(obj==null)
            {
                return default(T);
            }
            return (T)obj;
        }

        /// <summary>
        /// Convert to desire type
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="Exception.JsonValueConversionException"></exception>
        /// <returns></returns>
        public virtual object AsType(Type type)
        {
            if (type.IsSubclassOf(typeof(JsonValue)))
            {
                return Convert.ChangeType(this, type);
            }
            throw new JsonValueConversionException($"Unable to convert to {type.Name}");
        }

        public virtual object Value
        {
            get
            {
                return null;
            }
        }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
