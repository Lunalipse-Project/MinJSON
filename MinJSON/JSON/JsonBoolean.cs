using MinJSON.Exception;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.JSON
{
    public class JsonBoolean : JsonValue
    {
        bool value;
        public JsonBoolean(bool value) : base (JsonValueType.JPrimitive)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value ? "true" : "false";
        }

        public override object AsType(Type type)
        {
            if(type.Equals(typeof(bool)))
            {
                return value;
            }
            else if(type.Equals(typeof(string)))
            {
                return ToString();
            }
            throw new JsonValueConversionException($"Unable to convert JSON boolean to type '{type.Name}'");
        }
    }
}
