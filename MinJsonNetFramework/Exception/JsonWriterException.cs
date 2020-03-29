using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Exception
{

    public class JsonWriterException : System.Exception
    {
        public JsonWriterException() { }
        public JsonWriterException(string message) : base(message) { }
        public JsonWriterException(string message, System.Exception inner) : base(message, inner) { }
    }
}
