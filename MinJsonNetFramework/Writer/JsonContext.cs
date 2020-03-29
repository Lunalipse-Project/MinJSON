using MinJSON.JSON;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Writer
{
    public class JsonContext
    {
        bool isMarked = false;
        JsonValue jsonValue;
        string valueKey;
        bool isKeyValuePair;
        public JsonContext(string vkey, bool mark = false)
        {
            valueKey = vkey;
            jsonValue = new JsonNull();
            isKeyValuePair = true;
            isMarked = mark;
        }

        public JsonContext(JsonValue jvalue, bool mark = false)
        {
            jsonValue = jvalue;
            isKeyValuePair = false;
            isMarked = mark;
        }

        public void SetJsonValue(JsonValue jvalue, bool mark = false)
        {
            jsonValue = jvalue;
            isMarked = mark;
        }

        public string GetKey()
        {
            return valueKey;
        }

        public JsonValue GetValue()
        {
            return jsonValue;
        }

        public bool IsKVPair()
        {
            return isKeyValuePair;
        }

        public bool IsMarked()
        {
            return isMarked;
        }

        public void Mark()
        {
            isMarked = true;
        }
    }
}
