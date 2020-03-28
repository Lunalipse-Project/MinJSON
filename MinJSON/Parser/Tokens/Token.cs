using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Parser.Tokens
{
    public class Token
    {
        public string Value { get; private set; }
        public TokenType Type{ get; private set; }
        public int Line { get; private set; }
        public int Column { get; private set; }
        public Token(string value, TokenType tokenType, int line, int column)
        {
            Value = value;
            Type = tokenType;
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return $"<{Type}@{Line},{Column}; {Value} >";
        }


        public static readonly Token EOF = new Token("#EOF#", TokenType.GAMMY, -1, -1);
    }
}
