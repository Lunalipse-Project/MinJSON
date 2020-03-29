using MinJSON.Exception;
using System;

namespace MinJSON.JSON
{
    public class JsonNumber : JsonValue
    {
        decimal number;
        public JsonNumber(decimal number) : base(JsonValueType.JPrimitive)
        {
            this.number = number;
        }

        public JsonNumber(string number) : this(decimal.Parse(number))
        {

        }

        public override string ToString()
        {
            return number.ToString();
        }

        public override object AsType(Type type)
        {
            try
            {
                return Convert.ChangeType(number, type);
            }
            catch(System.Exception e)
            {
                throw new JsonValueConversionException(
                    $"Unable to convert JSON number:{number} to type {type.Name}.",
                    e);
            }
        }

        public override object Value => number;

    }
}
