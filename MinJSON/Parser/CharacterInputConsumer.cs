using MinJSON.PDA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MinJSON.Parser
{
    public class CharacterInputConsumer : IDFAInputConsumer<char>
    {
        private CharEnumerator stringIterator;
        private bool Noteof = true;
        public CharacterInputConsumer(string inputStr)
        {
            stringIterator = inputStr.GetEnumerator();
            stringIterator.MoveNext();
        }

        public bool Consume()
        {
            return Noteof = stringIterator.MoveNext();
        }

        public bool IsEOF()
        {
            return !Noteof;
        }

        public char Peek()
        {
            return stringIterator.Current;
        }

        public void Reset()
        {
            stringIterator.Reset();
            stringIterator.MoveNext();
        }
    }
}
