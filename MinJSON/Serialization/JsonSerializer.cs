using MinJSON.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MinJSON.Serialization
{
    public class JsonSerializer<T>
    {
        public JsonValue Pack(T instance)
        {
            return pack(instance, null, typeof(T));
        }

        private JsonValue pack(object instance, JsonPropertyAttribute propertyAttribute, Type instance_type)
        {
            if (instance_type.IsPrimitive || instance_type.Equals(typeof(string)))
            {
                return serializePrimitive(instance, propertyAttribute, instance_type);
            }
            else if (Utils.isDictionaryLike(instance_type))
            {
                Type key = instance_type.GetGenericArguments()[0];
                Type value = instance_type.GetGenericArguments()[1];
                return serializeDictionaryLike(instance as IDictionary, propertyAttribute, key, value);
            }
            else if (instance_type.IsArray)
            {
                return serializeElement(instance as Array, propertyAttribute, instance_type.GetElementType());
            }
            else if (Utils.isEnumerable(instance_type))
            {
                return serializeElement(instance as IEnumerable, propertyAttribute, instance_type.GetGenericArguments()[0]);
            } 
            else
            {
                return serializeClass(instance, instance_type);
            }
        }

        private JsonObject serializeClass(object instance, Type classType)
        {
            JsonObject jsonValue = new JsonObject();
            foreach (FieldInfo member in classType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if(member.IsPublic || member.GetCustomAttribute(typeof(JsonPropertyAttribute)) != null)
                {
                    JsonPropertyAttribute propertyAttribute = 
                        member.GetCustomAttribute(typeof(JsonPropertyAttribute)) as JsonPropertyAttribute;
                    string key = member.Name;
                    if(propertyAttribute!=null && !string.IsNullOrEmpty(propertyAttribute.PropertyName))
                    {
                        key = propertyAttribute.PropertyName;
                    }
                    jsonValue.AddValuePair(key, pack(member.GetValue(instance), propertyAttribute, member.FieldType));
                }
            }
            return jsonValue;
        }

        private JsonObject serializeDictionaryLike(IDictionary instance, JsonPropertyAttribute propertyAttribute, Type keyType, Type valueType)
        {
            JsonObject jo = new JsonObject();
            foreach (DictionaryEntry entry in instance)
            {
                JsonValue value = null;
                string key;
                if(propertyAttribute!=null && propertyAttribute.KeyConverter != null)
                {
                    key = propertyAttribute.KeyConverter.ConvertTo(entry.Key);
                }
                else if (keyType.Equals(typeof(string)))
                {
                    key = entry.Key.ToString();
                }
                else
                {
                    throw new NotSupportedException("Non-string key can not be serialized unless a key converter is specified.");
                }

                if (propertyAttribute != null && propertyAttribute.ValueConverter != null)
                {
                    value = propertyAttribute.ValueConverter.ConvertTo(entry.Value);
                }
                else
                {
                    value = pack(entry.Value, propertyAttribute, valueType);
                }
                jo.AddValuePair(key, value);
            }
            return jo;
        }

        private JsonArray serializeElement(IEnumerable instance, JsonPropertyAttribute propertyAttribute, Type elementType)
        {
            JsonArray jsonValue = new JsonArray();
            if (!elementType.IsPrimitive || elementType.Equals(typeof(string)))
            {
                foreach (object obj in instance)
                {
                    if (propertyAttribute != null && propertyAttribute.ValueConverter != null)
                    {
                        jsonValue.AddValue(propertyAttribute.ValueConverter.ConvertTo(obj));
                    }
                    else
                    {
                        jsonValue.AddValue(pack(obj,propertyAttribute, elementType));
                    }
                }
            }
            else
            {
                foreach (object obj in instance)
                {
                    if (propertyAttribute != null && propertyAttribute.ValueConverter != null)
                    {
                        jsonValue.AddValue(propertyAttribute.ValueConverter.ConvertTo(obj));
                    }
                    else
                    {
                        jsonValue.AddValue(serializePrimitive(obj, propertyAttribute, elementType));
                    }
                }
            }
            return jsonValue;
        }

        private JsonValue serializePrimitive(object instance, JsonPropertyAttribute propertyAttribute, Type primitiveType)
        {
            if(instance == null)
            {
                return new JsonNull();
            }
            if(propertyAttribute!=null && propertyAttribute.ValueConverter != null)
            {
                return propertyAttribute.ValueConverter.ConvertTo(instance);
            }
            if(Utils.isNumeric(primitiveType))
            {
                return new JsonNumber(Convert.ToDecimal(instance));
            }
            else if(Type.GetTypeCode(primitiveType) == TypeCode.String)
            {
                return new JsonString(instance as string);
            }
            else if (Type.GetTypeCode(primitiveType) == TypeCode.Boolean)
            {
                return new JsonBoolean((bool)instance);
            }
            throw new NotSupportedException();
        }
    }
}
