using System;
using System.Collections.Generic;
using System.Text;
using MinJSON.Exception;
using MinJSON.JSON;
using MinJSON.Parser.Lexer;
using MinJSON.Parser.Tokens;
using MinJSON.PDA;

namespace MinJSON.Parser
{

    public class JsonParser
    {
        PDA<Token, JsonValue> Recoginzer;
        ParserConfig grammerConfig;

        public JsonParser(JsonLexer lexer) : this(lexer as IDFAInputConsumer<Token>)
        {
            
        }

        public JsonParser(IDFAInputConsumer<Token> inputConsumer)
        {
            grammerConfig = new ParserConfig(inputConsumer);
            Recoginzer = new PDA<Token, JsonValue>(grammerConfig);
        }

        public JsonValue Parse()
        {
            try
            {
                Recoginzer.Run();
                JsonValue jvalue = Recoginzer.Result;
                Recoginzer.Reset();
                return jvalue;
            }
            catch(PDAException pdae)
            {
                throw new JsonParserException(string.Empty, Recoginzer.CurrentInput, pdae);
            }
        }
    }
}
