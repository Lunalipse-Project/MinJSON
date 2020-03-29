using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Parser.Tokens
{
    public enum TokenType
    {
        LBRACE,
        RBRACE,
        LPAREN,
        RPAREN,
        LSQUARE,
        RSQUARE,
        COLON,
        COMMA,
        STRING,
        NUMBER,
        BOOLEAN,
        NULL,
        GAMMY
    }
}
