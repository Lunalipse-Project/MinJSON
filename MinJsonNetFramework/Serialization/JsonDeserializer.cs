using MinJSON.Exception;
using MinJSON.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MinJSON.Serialization
{
    public class JsonDeserializer<T>
    {
        public T Unpack(JsonValue jsonValue)
        {
            return (T)unpack(jsonValue, null, typeof(T));
        }

        private object unpack(JsonValue jvalue, JsonPropertyAttribute propertyAttribute, Type instance_type)
        {
            if(propertyAttribute!=null && propertyAttribute.ValueConverter != null)
            {
                return propertyAttribute.ValueConverter.ConvertFromJson(jvalue, instance_type);
            }
            if (jvalue.isNullValue)
            {
                return null;
            }
            if(instance_type.isPrimitiveValue())
            {
                return Convert.ChangeType(jvalue.Value, instance_type);
            }
            else if (instance_type.isDictionaryLike())
            {
                return DeserializeDictionary(jvalue, propertyAttribute, instance_type);
            }
            else if (instance_type.IsArray)
            {
                return DeserializeArray(jvalue as JsonArray, propertyAttribute, instance_type.GetElementType());
            }
            else if (instance_type.isListLike())
            {
                return DeserializeCollection(jvalue as JsonArray, propertyAttribute, instance_type);
            }
            else
            {
                return DeserializeClass(jvalue as JsonObject, instance_type);
            }
        }

        private IDictionary DeserializeDictionary(JsonValue jvalue, JsonPropertyAttribute propertyAttribute, Type instance_type)
        {
            IDictionary dictionary = (IDictionary)Activator.CreateInstance(instance_type);
            Type keyType = instance_type.GenericTypeArguments[0];
            Type valType = instance_type.GenericTypeArguments[1];
            JsonObject jo = jvalue as JsonObject;
            bool needKeyConverter = false;
            if (jo == null)
            {
                throw new InvalidCastException($"Unable to deserialize {jvalue.ValueType} as dictionary-like data type");
            }
            if(!keyType.Equals(typeof(string)) && (propertyAttribute==null || propertyAttribute.KeyConverter == null))
            {
                throw new InvalidCastException("Unable to convert string key value to non-string key value in absence of appropriate KeyConverter");
            }
            needKeyConverter = !keyType.Equals(typeof(string));
            foreach (KeyValuePair<string, JsonValue> entry in jo)
            {
                object key = entry.Key;
                object val = null;
                if (needKeyConverter)
                {
                    key = propertyAttribute.KeyConverter.ConvertFrom(entry.Key, keyType);
                }
                val = unpack(entry.Value as JsonValue, null, valType);
                dictionary.Add(key, val);
            }
            return dictionary;
        }

        private object DeserializeClass(JsonObject jobject, Type classType)
        {
            JsonSerializable serializable = classType.GetTypeInfo().GetCustomAttribute(typeof(JsonSerializable)) as JsonSerializable;
            object[] parameters = serializable == null ? null : serializable.ConsturctorParams;
            object classInstance = null;
            try
            {
                classInstance = Activator.CreateInstance(
                                        classType,
                                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                                        null,
                                        parameters,
                                        null,
                                        null);
            }
            catch(System.Exception e)
            {
                if(e is ArgumentException || e is ArgumentNullException)
                {
                    throw new JsonSerializationException("The target class with non-parameterless constructor must be marked as JsonSerializable attribute and specify the default parameter.");
                }
                throw e;
            }
            foreach (FieldInfo member in classType.GetRuntimeFields())
            {
                if (member.IsPublic || member.GetCustomAttribute(typeof(JsonPropertyAttribute)) != null)
                {
                    JsonPropertyAttribute propertyAttribute =
                        member.GetCustomAttribute(typeof(JsonPropertyAttribute)) as JsonPropertyAttribute;
                    string key = member.Name;
                    if (propertyAttribute != null && !string.IsNullOrEmpty(propertyAttribute.PropertyName))
                    {
                        key = propertyAttribute.PropertyName;
                    }
                    JsonValue value = jobject[key];
                    if (value != null)
                    {
                        member.SetValue(classInstance, unpack(value, propertyAttribute, member.FieldType));
                    }
                }
            }
            return classInstance;
        }

        private Array DeserializeArray(JsonArray jarray, JsonPropertyAttribute propertyAttribute, Type elementType)
        {
            Array array = Array.CreateInstance(elementType, jarray.Length);
            for (int i = 0; i < jarray.Length; i++)
            {
                object obj = null;
                obj = unpack(jarray[i], propertyAttribute, elementType);
                array.SetValue(obj, i);
            }
            return array;
        }

        private IList DeserializeCollection(JsonArray jarray, JsonPropertyAttribute propertyAttribute, Type listType)
        {
            IList list = (IList)Activator.CreateInstance(listType);
            Type elementType = listType.GenericTypeArguments[0];
            for (int i = 0; i < jarray.Length; i++)
            {
                object obj = null;
                obj = unpack(jarray[i], propertyAttribute, elementType);
                list.Add(obj);
            }
            return list;
        }
    }
}
