using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.PDA
{
    /// <summary>
    /// Represent a DFA configuration
    /// </summary>
    /// <typeparam name="T">input stream type</typeparam>
    /// <typeparam name="R">DFA result type</typeparam>
    public interface IDFAConfiguration<T,R>
    {
        R GetResult();
        IDFAInputConsumer<T> GetInputConsumer();
        int GetStartingState();
        int[] GetAcceptingStates();
        bool IsValidInput(T input);
        TransitionTable<T> GetTransitionFunctions();

        int OnStateVisited(int currentState, int nextState, T currentInput, Stack<T> operationStack);

        void Reset();
    }
}
