using MinJSON.Exception;
using MinJSON.Parser;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MinJSON.JSON
{
    public class JsonObject : JsonValue, IEnumerable
    {
        Dictionary<string, JsonValue> KVPairs;
        public JsonObject() : base(JsonValueType.JObject)
        {
            KVPairs = new Dictionary<string, JsonValue>();
        }

        public void AddValuePair(string key, JsonValue value)
        {
            if(!KVPairs.ContainsKey(key))
            {
                if(value == null)
                {
                    KVPairs.Add(key, new JsonNull());
                }
                else
                {
                    KVPairs.Add(key, value);
                }
            }
        }

        public JsonValue this[string index]
        {
            get
            {
                if(KVPairs.ContainsKey(index))
                {
                    return KVPairs[index];
                }
                return null;
            }
        }

        public override string ToString()
        {
            Dictionary<string, JsonValue>.Enumerator enumerator = (Dictionary<string, JsonValue>.Enumerator) GetEnumerator();
            StringBuilder sbuilder = new StringBuilder();
            if(enumerator.MoveNext())
            {
                KeyValuePair<string, JsonValue> kvp = enumerator.Current;
                sbuilder.Append($"\"{kvp.Key}\":{kvp.Value.ToString()}");
                while(enumerator.MoveNext())
                {
                    kvp = enumerator.Current;
                    sbuilder.Append($",\"{kvp.Key}\":{kvp.Value.ToString()}");
                }
            }
            return $"{{{sbuilder}}}";
        }

        public IEnumerator GetEnumerator()
        {
            return KVPairs.GetEnumerator();
        }

        public static JsonObject Parse(string jsonString)
        {
            JsonParser jparser = new JsonParser(new Parser.Lexer.JsonLexer(jsonString));
            JsonValue jval = jparser.Parse();
            if(jval.isObject)
            {
                return jval as JsonObject;
            }
            throw new JsonValueConversionException("Not a valid JsonObject");
        }

        public static JsonObject LoadFrom(Stream stream)
        {
            JsonParser jparser = new JsonParser(new Parser.Lexer.JsonLexer(stream));
            JsonValue jval = jparser.Parse();
            if (jval.isObject)
            {
                return jval as JsonObject;
            }
            throw new JsonValueConversionException("Not a valid JsonObject");
        }
    }
}
