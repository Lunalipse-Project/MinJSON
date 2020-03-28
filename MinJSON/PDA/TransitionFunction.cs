using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.PDA
{
    /// <summary>
    /// Represent a transition function
    /// <para>
    /// δ:(q,s,g)→{(q2,g2)|q2∈Q ∧ g2∈Γ} ∀.(q∈Q∧s∈Σ∧g∈Γ)
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TransitionFunction<T>
    {
        private int currentState;
        private int nextState;
        private Func<T, Stack<T>, bool> predicate;
        public TransitionFunction(int currentState, int nextState, Func<T, Stack<T>, bool> predicate)
        {
            this.currentState = currentState;
            this.nextState = nextState;
            this.predicate = predicate;
        }

        public bool canTransit(int currentState, T input, Stack<T> operatingStack)
        {
            if(this.currentState != currentState)
            {
                return false;
            }
            return predicate.Invoke(input, operatingStack);
        }

        public int getNextState()
        {
            return nextState;
        }

        public int getOriginState()
        {
            return currentState;
        }
    }
}
