using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.PDA
{
    /// <summary>
    /// Represent a Pushdown Automata
    /// <para>
    /// (Q,F,Σ,Γ,δ)
    /// </para>
    /// </summary>
    /// <typeparam name="T">input stream type</typeparam>
    /// <typeparam name="R">DFA result type</typeparam>
    public class PDA<T,R>
    {
        IDFAConfiguration<T, R> configuration;
        TransitionTable<T> transitionFunctions;
        int startingState;
        int[] acceptingStates;

        Stack<T> operationStack;

        public T CurrentInput { get; private set; }

        public PDA()
        {
            operationStack = new Stack<T>();
        }

        public PDA(IDFAConfiguration<T, R> configuration) : this()
        {
            LoadConfiguration(configuration);
        }

        public void LoadConfiguration(IDFAConfiguration<T, R> configuration)
        {
            operationStack.Clear();
            if (this.configuration != null)
            {
                this.configuration.Reset();
            }
            this.configuration = configuration;
            transitionFunctions = configuration.GetTransitionFunctions();
            startingState = configuration.GetStartingState();
            acceptingStates = configuration.GetAcceptingStates();
        }

        public void Run()
        {
            IDFAInputConsumer<T> consumer = configuration.GetInputConsumer();
            int currentState = startingState;
            int exit_code = 0;
            do
            {
                CurrentInput = consumer.Peek();
                if(!configuration.IsValidInput(CurrentInput))
                {
                    exit_code = 1;
                    break;
                }
                TransitionFunction<T> function = transitionFunctions.Get(currentState, CurrentInput, operationStack);
                if(function == null)
                {
                    exit_code = 2;
                    break;
                }
                currentState = configuration.OnStateVisited(currentState, function.getNextState(), CurrentInput, operationStack);
            }
            while (consumer.Consume());

            CheckRunnerStatus(exit_code, currentState, CurrentInput);
        }

        private void CheckRunnerStatus(int code, int currentState, T lastInput)
        {
            switch(code)
            {
                case 0:
                case 1:
                    if (!shouldAccept(currentState))
                    {
                        throw new PDAException($"Unexpected input: {lastInput.ToString()}");
                    }
                    break;
                case 2:
                    if (!shouldAccept(currentState))
                    {
                        throw new PDAException($"Unable to perform transition on {lastInput.ToString()}. (State #{currentState})");
                    }
                    break;
            }
        }

        private bool shouldAccept(int state)
        {
            foreach(int s in acceptingStates)
            {
                if(s == state)
                {
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            configuration.Reset();
            operationStack.Clear();
        }

        public R Result
        {
            get
            {
                if (configuration != null)
                {
                    return configuration.GetResult();
                }
                throw new PDAException("No configuration is loaded");
            }
        }
    }
}
