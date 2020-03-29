using System;
using System.Collections.Generic;
using System.Text;

namespace MinJSON.PDA
{
    public interface IDFAInputConsumer<T>
    {
        /// <summary>
        /// Peek current input
        /// </summary>
        /// <returns></returns>
        T Peek();

        /// <summary>
        /// Consume current input and get next.
        /// Return false if it has consumed last input.
        /// </summary>
        /// <returns></returns>
        bool Consume();

        /// <summary>
        /// Reset the consumer
        /// </summary>
        void Reset();

        bool IsEOF();
    }
}
