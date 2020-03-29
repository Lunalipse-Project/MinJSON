using System;
using System.Collections.Generic;
using System.Text;
using MinJSON.JSON;

namespace MinJSON.Serialization.Converter
{
    public class StringEnumConverter : IJsonValueConverter
    {
        public object ConvertFromJson(JsonValue jsonValue, Type targetType)
        {
            if (!targetType.IsEnum || !(jsonValue is JsonString))
            {
                throw new NotSupportedException("This converter can only convert string to enum");
            }
            return Enum.Parse(targetType, jsonValue.As<string>());
        }

        public JsonValue ConvertTo(object obj, Type objectType)
        {
            return new JsonString(Enum.GetName(objectType, obj));
        }
    }
}
