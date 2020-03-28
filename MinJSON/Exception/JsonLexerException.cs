using MinJSON.Parser.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Exception
{

    [Serializable]
    public class JsonLexerException : System.Exception
    {
        int line, col;
        char err_ch;
        public JsonLexerException() { }
        public JsonLexerException(string message, char err_ch, int line, int col) : base(message)
        {
            this.line = line;
            this.col = col;
            this.err_ch = err_ch;
        }
        public JsonLexerException(string message, char err_ch, int line, int col, System.Exception inner) : base(message, inner)
        {
            this.line = line;
            this.col = col;
            this.err_ch = err_ch;
        }
        protected JsonLexerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public override string Message
        {
            get
            {
                return $"{Message} ({err_ch}@{line}-{col}) " + (InnerException == null ? "" : $"Detail: {InnerException.Message}");
            }
        }
    }
}
