using MinJSON.PDA;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MinJSON.Parser.Lexer
{
    public class StringMatcher : IDFAConfiguration<char, string>
    {
        private const int LQUOTE = 0;
        private const int CONTENT = 1;
        private const int ESCAPE_CHAR = 2;
        private const int UHEX_START = 3;
        private const int UHEX_1 = 4;
        private const int UHEX_2 = 5;
        private const int UHEX_3 = 6;
        private const int RQUOTE = 7;

        private IDFAInputConsumer<char> inputConsumer;

        private StringBuilder stringBuilder;

        private int[] accepting = new[] { RQUOTE };
        private TransitionTable<char> transitions = new TransitionTable<char>(8);

        public StringMatcher(IDFAInputConsumer<char> inputConsumer)
        {
            this.inputConsumer = inputConsumer;
            stringBuilder = new StringBuilder();
            buildTransition();
        }

        public int[] GetAcceptingStates()
        {
            return accepting;
        }

        public IDFAInputConsumer<char> GetInputConsumer()
        {
            return inputConsumer;
        }

        public string GetResult()
        {
            return Regex.Unescape(stringBuilder.ToString().Trim('"'));
        }

        public int GetStartingState()
        {
            return LQUOTE;
        }

        public TransitionTable<char> GetTransitionFunctions()
        {
		    return transitions;
        }

        public bool IsValidInput(char input)
        {
            //for all characters
            return true;
        }

        public int OnStateVisited(int currentState, int nextState, char currentInput, Stack<char> operationStack)
        {
            stringBuilder.Append(currentInput);
            return nextState;
        }

        public void Reset()
        {
            stringBuilder.Clear();
        }

        private void buildTransition()
        {
            transitions.Add(new TransitionFunction<char>(LQUOTE, CONTENT, (i, opt_stack) => i == '"'));
            transitions.Add(new TransitionFunction<char>(CONTENT, CONTENT, (i, opt_stack) => (i != '\\' && i != '"')));
            transitions.Add(new TransitionFunction<char>(CONTENT, ESCAPE_CHAR, (i, opt_stack) => i == '\\'));
            transitions.Add(new TransitionFunction<char>(ESCAPE_CHAR, CONTENT, (i, opt_stack) => {
                return  i == '"' ||
                        i == '\\' ||
                        i == '/' ||
                        i == 'b' ||
                        i == 'r' ||
                        i == 'n' ||
                        i == 'f' ||
                        i == 't';
            }));
            transitions.Add(new TransitionFunction<char>(ESCAPE_CHAR, UHEX_START, (i, opt_stack) => {
                return i == 'u';
            }));
            transitions.Add(new TransitionFunction<char>(UHEX_START, UHEX_1, (i, opt_stack) => {
                return ('0' <= i && i <= '9') || ('a' <= i && i <= 'f');
            }));
            transitions.Add(new TransitionFunction<char>(UHEX_1, UHEX_2, (i, opt_stack) => {
                return ('0' <= i && i <= '9') || ('a' <= i && i <= 'f');
            }));
            transitions.Add(new TransitionFunction<char>(UHEX_2, UHEX_3, (i, opt_stack) => {
                return ('0' <= i && i <= '9') || ('a' <= i && i <= 'f');
            }));
            transitions.Add(new TransitionFunction<char>(UHEX_3, CONTENT, (i, opt_stack) => {
                return ('0' <= i && i <= '9') || ('a' <= i && i <= 'f');
            }));
            transitions.Add(new TransitionFunction<char>(CONTENT, RQUOTE, (i, opt_stack) => i == '"'));
        }
    }
}
