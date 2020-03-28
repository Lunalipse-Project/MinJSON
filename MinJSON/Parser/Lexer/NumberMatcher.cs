using MinJSON.PDA;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.Parser.Lexer
{
    public class NumberMatcher : IDFAConfiguration<char, string>
    {
        private const int START = 0;
        private const int SIGN = 1;
        private const int INTEGER = 2;
        private const int DOT = 3;
        private const int FRAC_PARTS = 4;
        private const int EXPONENT_E = 5;
        private const int EXPONENT = 6;

        private IDFAInputConsumer<char> inputConsumer;

        private StringBuilder stringBuilder;

        private TransitionTable<char> transitions = new TransitionTable<char>(7);
        private int[] accepting = new [] { INTEGER, FRAC_PARTS, EXPONENT };

        public NumberMatcher(IDFAInputConsumer<char> inputConsumer)
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
            return ('0' <= input && input <= '9') ||
                    input == '+' ||
                    input == '-' ||
                    input == '.' ||
                    input == 'e' ||
                    input == 'E';
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
            transitions.Add(new TransitionFunction<char>(START, SIGN, (i, opt_stack) => {
                return i == '-';
            }));
            transitions.Add(new TransitionFunction<char>(START, INTEGER, (i, opt_stack) => {
                return '0' <= i && i <= '9';
            }));
            transitions.Add(new TransitionFunction<char>(SIGN, INTEGER, (i, opt_stack) => {
                return '0' <= i && i <= '9';
            }));
            transitions.Add(new TransitionFunction<char>(INTEGER, INTEGER, (i, opt_stack) => {
                return '0' <= i && i <= '9';
            }));
            transitions.Add(new TransitionFunction<char>(INTEGER, DOT, (i, opt_stack) => {
                return i == '.';
            }));
            transitions.Add(new TransitionFunction<char>(INTEGER, EXPONENT_E, (i, opt_stack) => {
                return i == 'e' || i == 'E';
            }));
            transitions.Add(new TransitionFunction<char>(DOT, FRAC_PARTS, (i, opt_stack) => {
                return '0' <= i && i <= '9';
            }));
            transitions.Add(new TransitionFunction<char>(FRAC_PARTS, FRAC_PARTS, (i, opt_stack) => {
                return '0' <= i && i <= '9';
            }));
            transitions.Add(new TransitionFunction<char>(FRAC_PARTS, EXPONENT_E, (i, opt_stack) => {
                return i == 'e' || i == 'E';
            }));
            transitions.Add(new TransitionFunction<char>(EXPONENT_E, EXPONENT, (i, opt_stack) => {
                return ('0' <= i && i <= '9') ||
                        i == '+' ||
                        i == '-';
            }));
            transitions.Add(new TransitionFunction<char>(EXPONENT, EXPONENT, (i, opt_stack) => {
                return '0' <= i && i <= '9';
            }));
        }
    }
}
