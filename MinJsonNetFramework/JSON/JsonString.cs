﻿using System;

namespace MinJSON.JSON
{
    public class JsonString : JsonValue
    {
        string value;
        public JsonString(string value) : base(JsonValueType.JPrimitive)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return $"\"{Utils.escapeJsonString(value)}\"";
        }

        public override object AsType(Type type)
        {
            if(type.Equals(typeof(string)))
            {
                return value;
            }
            return base.AsType(type);
        }

        public override object Value => value;

    }
}
