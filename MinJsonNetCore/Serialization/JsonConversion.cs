using MinJSON.Exception;
using MinJSON.JSON;

namespace MinJSON.Serialization
{
    public class JsonConversion
    {
        public static JsonObject SerializeJsonObject<T>(T instance)
        {
            JsonSerializer<T> js = new JsonSerializer<T>();
            JsonValue jv = js.Pack(instance);
            if(jv.isObject)
            {
                return jv as JsonObject;
            }
            throw new JsonSerializationException($"{typeof(T).Name} can not be serialize as json object.");
        }

        public static JsonArray SerializeJsonArray<T>(T instance)
        {
            JsonSerializer<T> js = new JsonSerializer<T>();
            JsonValue jv = js.Pack(instance);
            if (jv.isArray)
            {
                return jv as JsonArray;
            }
            throw new JsonSerializationException($"{typeof(T).Name} can not be serialize as json array.");
        }

        public static T DeserializeJsonObject<T>(JsonObject jobject)
        {
            JsonDeserializer<T> js = new JsonDeserializer<T>();
            T v = js.Unpack(jobject);
            if (v != null)
            {
                return v;
            }
            throw new JsonSerializationException($"this json object can not be deserialize as {typeof(T).Name}");
        }

        public static T DeserializeJsonArray<T>(JsonArray jobject)
        {
            JsonDeserializer<T> js = new JsonDeserializer<T>();
            T v = js.Unpack(jobject);
            if (v != null)
            {
                return v;
            }
            throw new JsonSerializationException($"this json array can not be deserialize as {typeof(T).Name}");
        }
    }
}
