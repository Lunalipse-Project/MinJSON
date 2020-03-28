using MinJSON.Exception;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.JSON
{
    public class JsonNull : JsonValue
    {
        public JsonNull() : base(JsonValueType.JNull)
        {

        }

        public override string ToString()
        {
            return "null";
        }

        public override object AsType(Type type)
        {
            throw new JsonValueConversionException($"Unable to convert null to type '{type.Name}'");
        }
    }
}
