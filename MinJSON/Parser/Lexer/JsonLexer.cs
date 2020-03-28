using MinJSON.Exception;
using MinJSON.Parser.Tokens;
using MinJSON.PDA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MinJSON.Parser.Lexer
{
    public class JsonLexer : IDFAInputConsumer<Token>, IEnumerable<Token>
    {
        IDFAInputConsumer<char> inputConsumer;

        int line = 1;
        int col = 0;

        PDA<char, string> Recognizer;

        IDFAConfiguration<char, string> stringConfig;
        IDFAConfiguration<char, string> numberConfig;
        IDFAConfiguration<char, string> booleanConfig;
        IDFAConfiguration<char, string> nullConfig;

        public void initializeConfiguration()
        {
            stringConfig = new StringMatcher(inputConsumer);
            numberConfig = new NumberMatcher(inputConsumer);
            booleanConfig = new BooleanMatcher(inputConsumer);
            nullConfig = new NullMatcher(inputConsumer);
        }

        public JsonLexer()
        {
            Recognizer = new PDA<char, string>();
        }

        public JsonLexer(string jsonText) : this()
        {
            inputConsumer = new CharacterInputConsumer(jsonText);
            initializeConfiguration();
        }

        public JsonLexer(Stream stream) : this()
        {
            inputConsumer = new CharacterStreamInputConsumer(stream, Encoding.UTF8);
            initializeConfiguration();
        }


        public Token NextToken()
        {
            char current;
            if(inputConsumer.IsEOF())
            {
                return Token.EOF;
            }
            while(char.IsWhiteSpace(current = inputConsumer.Peek()))
            {
                col++;
                if (current=='\n')
                {
                    line++;
                    col = 0;
                }
                if(!inputConsumer.Consume())
                {
                    return Token.EOF;
                }
            }
            return _getToken(current);
        }

        private Token _getToken(char current)
        {
            col++;
            TokenType tokenType = TokenType.GAMMY;
            bool needPDA = false;
            switch(current)
            {
                case '{':
                    tokenType = TokenType.LBRACE;
                    break;
                case '}':
                    tokenType = TokenType.RBRACE;
                    break;
                case '[':
                    tokenType = TokenType.LSQUARE;
                    break;
                case ']':
                    tokenType = TokenType.RSQUARE;
                    break;
                case ',':
                    tokenType = TokenType.COMMA;
                    break;
                case ':':
                    tokenType = TokenType.COLON;
                    break;
                case '"':
                    Recognizer.LoadConfiguration(stringConfig);
                    tokenType = TokenType.STRING;
                    needPDA = true;
                    break;
                case 'f':
                case 't':
                    Recognizer.LoadConfiguration(booleanConfig);
                    tokenType = TokenType.BOOLEAN;
                    needPDA = true;
                    break;
                case 'n':
                    Recognizer.LoadConfiguration(nullConfig);
                    tokenType = TokenType.NULL;
                    needPDA = true;
                    break;
                default:
                    if(numberConfig.IsValidInput(current))
                    {
                        tokenType = TokenType.NUMBER;
                        Recognizer.LoadConfiguration(numberConfig);
                        needPDA = true;
                    }
                    else
                    {
                        throw new JsonLexerException("Invalid character found.", current, line, col);
                    }
                    break;
            }
            try
            {
                Token token;
                if (needPDA)
                {
                    Recognizer.Run();
                    token = new Token(Recognizer.Result, tokenType, line, col);
                    col += Recognizer.Result.Length;
                }
                else
                {
                    token = new Token(current.ToString(), tokenType, line, col);
                    inputConsumer.Consume();
                }
                return token;
            }
            catch(PDAException pdae)
            {
                throw new JsonLexerException("Invalid character found or something went wrong.", current, line, col, pdae);
            }
        }

        private IEnumerator<Token> _tokenEnumerator()
        {
            Token token;
            while ((token = NextToken()).Type != TokenType.GAMMY)
            {
                yield return token;
            }
        }


        public IEnumerator<Token> GetEnumerator()
        {
            return _tokenEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tokenEnumerator();
        }


        Token currentToken = null;

        public Token Peek()
        {
            if(currentToken == null)
            {
                currentToken = NextToken();
            }
            return currentToken;
        }

        public bool Consume()
        {
            currentToken = NextToken();
            return !currentToken.Type.Equals(TokenType.GAMMY);
        }

        public void Reset()
        {
            inputConsumer.Reset();
            line = 1;
            col = 0;
        }

        public bool IsEOF()
        {
            return currentToken.Type.Equals(TokenType.GAMMY);
        }
    }
}
