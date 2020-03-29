using MinJSON.JSON;
using MinJSON.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinJsonNetFramework.Serialization.Converter
{
    public class IntEnumConverter : IJsonValueConverter
    {
        public object ConvertFromJson(JsonValue jsonValue, Type targetType)
        {
            if (!targetType.IsEnum)
            {
                throw new NotSupportedException("This converter can only convert int to enum");
            }
            return Enum.ToObject(targetType, jsonValue.As<int>());
        }

        public JsonValue ConvertTo(object obj, Type objectType)
        {
            if (!objectType.IsEnum)
            {
                throw new NotSupportedException("This converter can only be used to convert enum");
            }
            return new JsonNumber((int)obj);
        }
    }
}
