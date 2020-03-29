using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.PDA
{

    public class PDAException : System.Exception
    {
        public PDAException() { }
        public PDAException(string message) : base(message) { }
        public PDAException(string message, System.Exception inner) : base(message, inner) { }
    }
}
