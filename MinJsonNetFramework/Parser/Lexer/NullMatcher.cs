using MinJSON.PDA;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Parser.Lexer
{
    public class NullMatcher : IDFAConfiguration<char, string>
    {

        private const int START = 0;
        private const int N = 1;
        private const int U = 2;
        private const int L = 3;
        private const int ACCEPT = 4;

        private IDFAInputConsumer<char> inputConsumer;

        private StringBuilder stringBuilder;

        private TransitionTable<char> transitions = new TransitionTable<char>(5);

        private int[] accepting = new[] { ACCEPT };

        public NullMatcher(IDFAInputConsumer<char> inputConsumer)
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
            return stringBuilder.ToString();
        }

        public int GetStartingState()
        {
            return START;
        }

        public TransitionTable<char> GetTransitionFunctions()
        {            
		    return transitions;
        }

        public bool IsValidInput(char input)
        {
            return input == 'n' ||
                   input == 'l' ||
                   input == 'u';
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
            transitions.Add(new TransitionFunction<char>(START, N, (i, opt_stack) => {
                return i == 'n';
            }));
            transitions.Add(new TransitionFunction<char>(N, U, (i, opt_stack) => {
                return i == 'u';
            }));
            transitions.Add(new TransitionFunction<char>(U, L, (i, opt_stack) => {
                return i == 'l';
            }));
            transitions.Add(new TransitionFunction<char>(L, ACCEPT, (i, opt_stack) => {
                return i == 'l';
            }));
        }
    }
}
