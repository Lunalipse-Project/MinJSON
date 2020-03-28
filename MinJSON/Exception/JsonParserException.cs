using MinJSON.Parser.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Exception
{

    [Serializable]
    public class JsonParserException : System.Exception
    {
        Token errToken;
        public JsonParserException() { }
        public JsonParserException(string message, Token errToken) : base(message)
        {
            this.errToken = errToken;
        }
        public JsonParserException(string message, Token errToken, System.Exception inner) : base(message, inner)
        {
            this.errToken = errToken;
        }
        protected JsonParserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public override string Message
        {
            get
            {
                return $"Invalid token detected : {errToken}" + (InnerException == null ? "" : $"Detail: {InnerException.Message}");
            }
        }
    }
}
