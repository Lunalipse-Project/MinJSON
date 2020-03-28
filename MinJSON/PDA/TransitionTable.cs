using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.PDA
{
    public class TransitionTable<T>
    {
        private TransitionFunction<T>[,] table;

        public TransitionTable(int statesCount)
        {
            table = new TransitionFunction<T>[statesCount,statesCount]; 
        }

        public void Add(TransitionFunction<T> function)
        {
            int org = function.getOriginState();
            int next = function.getNextState();
            if (table[org, next] == null)
            {
                table[org, next] = function;
            }
            else
            {
                throw new PDAException("Nondeterministic transition is not supported");
            }
        }

        public TransitionFunction<T> Get(int origin, T input, Stack<T> opt_stack)
        {
            for(int i = 0; i < table.GetLength(0); i++)
            {
                if (table[origin, i] != null)
                {
                    if (table[origin, i].canTransit(origin, input, opt_stack))
                    {
                        return table[origin, i];
                    }
                }
            }
            return null;
        }
    }
}
