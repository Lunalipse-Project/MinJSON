using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Exception
{

    public class JsonSerializationException : System.Exception
    {
        public JsonSerializationException() { }
        public JsonSerializationException(string message) : base(message) { }
        public JsonSerializationException(string message, System.Exception inner) : base(message, inner) { }
    }
}
