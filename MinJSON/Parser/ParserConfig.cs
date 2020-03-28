using MinJSON.JSON;
using MinJSON.Parser.Tokens;
using MinJSON.PDA;
using MinJSON.Writer;
using System.Collections.Generic;

/* 
 * BNF for JSON
 * 
 * field ::= STRING COLON value;
 * 
 * fields ::= field
 * 			| fields COMMA fields;
 * 
 * array ::= LSQUARE array_content RSQUARE;
 * 
 * array_content ::=  value 
 * 					| array_content COMMA array_content;
 * 
 * object ::= LBRACE fields RBRACE;
 * 
 * value ::=  object 
 * 			| array 
 * 			| p_value;
 * 
 * p_value ::= STRING 
 * 			| NUMBER 
 * 			| BOOL
 * 			| NULL;
 * 
 */

namespace MinJSON.Parser
{
    public class ParserConfig : IDFAConfiguration<Token, JsonValue>
    {
        JSONSeqentialWriter writer;

        private const int START = 0;
        private const int OBJECT = 1;
        private const int JNAME = 2;
        private const int JVALUE = 3;       // Json values (primitives, object, array)
        private const int CLOSE = 4;
        private const int PVALUE = 5;       // Primitive values and null value
        private const int ACCEPT = 6;

        private IDFAInputConsumer<Token> inputConsumer;

        private TransitionTable<Token> transitions = new TransitionTable<Token>(7);
        private int[] accepting = new[] { ACCEPT };

        public ParserConfig(IDFAInputConsumer<Token> inputConsumer)
        {
            writer = new JSONSeqentialWriter();
            this.inputConsumer = inputConsumer;
            buildTransitions();
        }

        public int[] GetAcceptingStates()
        {
            return accepting;
        }

        public IDFAInputConsumer<Token> GetInputConsumer()
        {
            return inputConsumer;
        }

        public JsonValue GetResult()
        {
            return writer.ToJsonValue();
        }

        public int GetStartingState()
        {
            return START;
        }

        public TransitionTable<Token> GetTransitionFunctions()
        {
		    return transitions;
        }

        private bool getValueTransition(Token i)
        {
            TokenType type = i.Type;
            return type == TokenType.BOOLEAN ||
                   type == TokenType.NULL ||
                   type == TokenType.NUMBER ||
                   type == TokenType.STRING;
        }

        private bool isStackTopEquals(TokenType type, Stack<Token> stk)
        {
            if (stk.Count == 0)
            {
                return false;
            }
            return stk.Peek().Type.Equals(type);
        }

        private bool doBracePush(Token token, Stack<Token> stk)
        {
            if (token.Type == TokenType.LBRACE)
            {
                stk.Push(token);
                writer.WriteObjectBegin();
                return true;
            }
            return false;
        }

        private bool doSquareBracketPush(Token token, Stack<Token> stk)
        {
            if (token.Type == TokenType.LSQUARE)
            {
                stk.Push(token);
                writer.WriteArrayBegin();
                return true;
            }
            return false;
        }

        private bool doSquareBracketPop(Token token, Stack<Token> stk)
        {
            if (token.Type == TokenType.RSQUARE && stk.Count != 0)
            {
                if (stk.Peek().Type == TokenType.LSQUARE)
                {
                    stk.Pop();
                    return true;
                }
            }
            return false;
        }

        private bool doBracePop(Token token, Stack<Token> stk)
        {
            if (token.Type == TokenType.RBRACE && stk.Count != 0)
            {
                if (stk.Peek().Type == TokenType.LBRACE)
                {
                    stk.Pop();
                    return true;
                }
            }
            return false;
        }

        public bool IsValidInput(Token input)
        {
            // For all token types except EOF
            if (input.Type.Equals(TokenType.GAMMY))
            {
                return false;
            }
            return true;
        }

        public int OnStateVisited(int currentState, int nextState, Token currentInput, Stack<Token> operationStack)
        {
            TokenType currentType = currentInput.Type;
            string currentTokenContent = currentInput.Value;
            int next = nextState;
            switch (next)
            {
                case CLOSE:
                    if (currentType.Equals(TokenType.RBRACE))
                    {
                        writer.WriteObjectEnd();
                    }
                    else
                    {
                        writer.WriteArrayEnd();
                    }
                    if (operationStack.Count == 0)
                    {
                        next = ACCEPT;
                    }
                    else
                    {
                        next = currentState;
                    }
                    break;
                case JNAME:
                    writer.WriteProperty(currentTokenContent);
                    break;
                case PVALUE:
                    switch (currentType)
                    {
                        case TokenType.NUMBER:
                            writer.WriteDecimal(decimal.Parse(currentTokenContent));
                            break;
                        case TokenType.BOOLEAN:
                            writer.WriteBoolean(bool.Parse(currentTokenContent));
                            break;
                        case TokenType.STRING:
                            writer.WriteString(currentTokenContent);
                            break;
                        default:
                            writer.WriteNull();
                            break;
                    }
                    break;
            }
            return next;
        }

        public void Reset()
        {
            writer.Reset();
        }

        private void buildTransitions()
        {

            transitions.Add(new TransitionFunction<Token>(START, OBJECT, (i, s) => {
                return doBracePush(i, s);
            }));

            transitions.Add(new TransitionFunction<Token>(START, JVALUE, (i, s) => {
                return doSquareBracketPush(i, s);
            }));

            transitions.Add(new TransitionFunction<Token>(OBJECT, JNAME, (i, s) => {
                return i.Type.Equals(TokenType.STRING);
            }));

            transitions.Add(new TransitionFunction<Token>(OBJECT, CLOSE, (i, s) => {
                return doBracePop(i, s);
            }));

            transitions.Add(new TransitionFunction<Token>(JNAME, JVALUE, (i, s) => {
                return i.Type.Equals(TokenType.COLON);
            }));

            transitions.Add(new TransitionFunction<Token>(JVALUE, JVALUE, (i, s) => {
                return doSquareBracketPush(i, s);
            }));

            transitions.Add(new TransitionFunction<Token>(JVALUE, OBJECT, (i, s) => {
                return doBracePush(i, s);
            }));

            transitions.Add(new TransitionFunction<Token>(JVALUE, CLOSE, (i, s) => {
                return doSquareBracketPop(i, s);
            }));

            transitions.Add(new TransitionFunction<Token>(JVALUE, PVALUE, (i, s) => {
                return getValueTransition(i);
            }));

            transitions.Add(new TransitionFunction<Token>(PVALUE, JVALUE, (i, s) => {
                return i.Type.Equals(TokenType.COMMA) &&
                       isStackTopEquals(TokenType.LSQUARE, s);
            }));

            transitions.Add(new TransitionFunction<Token>(PVALUE, OBJECT, (i, s) => {
                return i.Type.Equals(TokenType.COMMA) &&
                       isStackTopEquals(TokenType.LBRACE, s);
            }));

            transitions.Add(new TransitionFunction<Token>(PVALUE, CLOSE, (i, s) => {
                if (!doBracePop(i, s))
                {
                    return doSquareBracketPop(i, s);
                }
                return true;
            }));
        }
    }
}
