using MinJSON.PDA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MinJSON.Parser
{
    public class CharacterInputConsumer : IDFAInputConsumer<char>
    {
        private string stringIterator;
        private int index;
        private bool Noteof = true;
        public CharacterInputConsumer(string inputStr)
        {
            stringIterator = inputStr;
            index = 0;
        }

        public bool Consume()
        {
            return Noteof = (++index) < stringIterator.Length;
        }

        public bool IsEOF()
        {
            return !Noteof;
        }

        public char Peek()
        {
            return stringIterator[index];
        }

        public void Reset()
        {
            index = 0;
        }
    }
}
