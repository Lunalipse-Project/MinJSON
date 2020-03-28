using MinJSON.Exception;
using MinJSON.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MinJSON.JSON
{
    public class JsonArray : JsonValue, IEnumerable
    {
        List<JsonValue> array;
        public JsonArray() : base(JsonValueType.JArray)
        {
            array = new List<JsonValue>();
        }

        public void AddValue(JsonValue value)
        {
            array.Add(value);
        }

        public void AddValues(List<JsonValue> values)
        {
            array.AddRange(values);
        }

        public void Reverse()
        {
            array.Reverse();
        }

        public int Length
        {
            get => array.Count;
        }

        public JsonValue this[int index]
        {
            get => array[index];
        }

        public IEnumerator GetEnumerator()
        {
            return array.GetEnumerator();
        }

        public override string ToString()
        {
            IEnumerator<JsonValue> enumerator = (IEnumerator<JsonValue>)GetEnumerator();
            StringBuilder sb = new StringBuilder();
            if(enumerator.MoveNext())
            {
                sb.Append(enumerator.Current);
                while(enumerator.MoveNext())
                {
                    sb.Append($",{enumerator.Current}");
                }
            }
            return $"[{sb}]";
        }

        public override object AsType(Type type)
        {
            if (type.IsArray)
            {
                Array array = Array.CreateInstance(type.GetElementType(), this.array.Count);
                for (int i = 0; i < Length; i++)
                {
                    array.SetValue(this[i].AsType(type.GetElementType()), i);
                }
                return array;
            }
            else if (Utils.isEnumerable(type))
            {
                Type t = type.GetGenericArguments()[0];
                IList list = (IList)Activator.CreateInstance(t);
                foreach(JsonValue value in this)
                {
                    list.Add(value.AsType(t));
                }
                return list;
            }
            return null;
        }

        public static JsonArray Parse(string jsonString)
        {
            JsonParser jparser = new JsonParser(new Parser.Lexer.JsonLexer(jsonString));
            JsonValue jval = jparser.Parse();
            if (jval.isArray)
            {
                return jval as JsonArray;
            }
            throw new JsonValueConversionException("Not a valid JsonObject");
        }

        public static JsonArray LoadFrom(Stream stream)
        {
            JsonParser jparser = new JsonParser(new Parser.Lexer.JsonLexer(stream));
            JsonValue jval = jparser.Parse();
            if (jval.isArray)
            {
                return jval as JsonArray;
            }
            throw new JsonValueConversionException("Not a valid JsonObject");
        }
    }
}
