using MinJSON.Exception;
using MinJSON.JSON;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Writer
{
    public class JSONSeqentialWriter
    {
        Stack<JsonContext> contexts;

        public JSONSeqentialWriter()
        {
            contexts = new Stack<JsonContext>();
        }

        public void WriteJsonValue(JsonValue jsonValue)
        {
            if (!isEmpty())
            {
                JsonContext top = contexts.Pop();
                JsonContext upper = top;
                if (top.IsKVPair())
                {
                    upper = contexts.Peek();
                    top.SetJsonValue(jsonValue);
                }
                if (upper.GetValue().isObject && top.IsKVPair())
                {
                    (upper.GetValue() as JsonObject).AddValuePair(top.GetKey(), top.GetValue());
                }
                else if(upper.GetValue().isArray)
                {
                    (upper.GetValue() as JsonArray).AddValue(jsonValue);
                    contexts.Push(upper);
                }
                else
                {
                    throw new JsonWriterException("Primitive value must be contained in an object or array");
                }
            }
            else
            {
                contexts.Push(new JsonContext(jsonValue));
            }
        }

        public void WriteString(string value)
        {
            WriteJsonValue(new JsonString(value));
        }

        public void WriteBoolean(bool value)
        {
            WriteJsonValue(new JsonBoolean(value));
        }

        public void WriteNull()
        {
            WriteJsonValue(new JsonNull());
        }

        public void WriteDecimal(decimal value)
        {
            WriteJsonValue(new JsonNumber(value));
        }
        public void WriteInteger(int value)
        {
            WriteJsonValue(new JsonNumber(value));
        }
        public void WriteDouble(double value)
        {
            WriteJsonValue(new JsonNumber(new decimal(value)));
        }
        public void WriteSingle(float value)
        {
            WriteJsonValue(new JsonNumber(new decimal(value)));
        }
        public void WriteLong(long value)
        {
            WriteJsonValue(new JsonNumber(value));
        }
        public void WriteByte(byte value)
        {
            WriteJsonValue(new JsonNumber(value));
        }

        public void WriteProperty(string propertyName)
        {
            contexts.Push(new JsonContext(propertyName));
        }

        public void WriteObjectBegin()
        {
            contexts.Push(new JsonContext(new JsonObject()));
        }
        public void WriteObjectEnd()
        {
            //JsonObject jsonObject = new JsonObject();
            //JsonContext context = null;
            //while(!isEmpty() && ((context = contexts.Pop()).IsMarked() || !context.GetValue().isObject))
            //{
            //    if(context.IsKVPair())
            //    {
            //        jsonObject.AddValuePair(context.GetKey(), context.GetValue());
            //    }
            //    else
            //    {
            //        throw new JsonWriterException("An object can only contains json property (a name/value pair).");
            //    }
            //}
            JsonContext context = contexts.Pop();
            if (!context.GetValue().isObject)
            {
                throw new JsonWriterException("WriteJsonObjectEnd can only be used after WriteJsonObjectBegin.");
            }
            WriteJsonValue(context.GetValue());
        }

        public void WriteArrayBegin()
        {
            contexts.Push(new JsonContext(new JsonArray()));
        }
        public void WriteArrayEnd()
        {
            //JsonArray jsonArray = new JsonArray();
            //JsonContext context = null;
            //while (!isEmpty() && ((context = contexts.Pop()).IsMarked() || !context.GetValue().isArray))
            //{
            //    if (!context.IsKVPair())
            //    {
            //        jsonArray.AddValue(context.GetValue());
            //    }
            //    else
            //    {
            //        throw new JsonWriterException("An array can only contains json value.");
            //    }
            //}
            JsonContext context = contexts.Pop();
            if (!context.GetValue().isArray)
            {
                throw new JsonWriterException("WriteJsonArrayEnd can only be used after WriteJsonArrayBegin.");
            }
            (context.GetValue() as JsonArray).Reverse();
            WriteJsonValue(context.GetValue());
        }

        public JsonValue ToJsonValue()
        {
            if(contexts.Count!=1)
            {
                throw new JsonWriterException("Json writer is not fully enclosed or it's empty");
            }
            JsonValue jv = contexts.Peek().GetValue();
            if(!jv.isObject && !jv.isArray)
            {
                throw new JsonWriterException("Invalid formation of json");
            }
            return jv;
        }

        public override string ToString()
        {
            return ToJsonValue().ToString();
        }

        public void Reset()
        {
            contexts.Clear();
        }
        private bool isEmpty()
        {
            return contexts.Count == 0;
        }
    }
}
