using MinJSON.PDA;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Parser.Lexer
{
    public class BooleanMatcher : IDFAConfiguration<char, string>
    {
        private const int START = 0;
        private const int F = 1;
        private const int T = 2;
        private const int A = 3;
        private const int L = 4;
        private const int R = 5;
        private const int E = 6;
        private const int ACCEPT = 7;

        private readonly List<int> inputSet = new List<int>() { 'f', 'a', 'l', 's', 'e', 't', 'r', 'u' };

        private IDFAInputConsumer<char> inputConsumer;

        private StringBuilder stringBuilder;

        TransitionTable<char> transitions = new TransitionTable<char>(8);

        int[] accepting = new[] { ACCEPT };

        public BooleanMatcher(IDFAInputConsumer<char> inputConsumer)
        {
            this.inputConsumer = inputConsumer;
            stringBuilder = new StringBuilder();
            BuildTransition();
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
            return inputSet.Contains(input);
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

        private void BuildTransition()
        {
            transitions.Add(new TransitionFunction<char>(START, F, (i, opt_stack) => {

                return i == 'f';
            }));
            transitions.Add(new TransitionFunction<char>(F, A, (i, opt_stack) => {
                return i == 'a';
            }));
            transitions.Add(new TransitionFunction<char>(A, L, (i, opt_stack) => {
                return i == 'l';
            }));
            transitions.Add(new TransitionFunction<char>(L, E, (i, opt_stack) => {
                return i == 's';
            }));
            transitions.Add(new TransitionFunction<char>(START, T, (i, opt_stack) => {
                return i == 't';
            }));
            transitions.Add(new TransitionFunction<char>(T, R, (i, opt_stack) => {
                return i == 'r';
            }));
            transitions.Add(new TransitionFunction<char>(R, E, (i, opt_stack) => {
                return i == 'u';
            }));
            transitions.Add(new TransitionFunction<char>(E, ACCEPT, (i, opt_stack) => {
                return i == 'e';
            }));
        }
    }
}
