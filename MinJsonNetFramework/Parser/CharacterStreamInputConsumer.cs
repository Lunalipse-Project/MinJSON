using MinJSON.PDA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MinJSON.Parser
{
    public class CharacterStreamInputConsumer : IDFAInputConsumer<char>
    {
        private StreamReader reader;
        public CharacterStreamInputConsumer(Stream stream, Encoding encoding)
        {
            reader = new StreamReader(stream, encoding);
        }

        public bool Consume()
        {
            return reader.Read() != -1;
        }

        public bool IsEOF()
        {
            return reader.EndOfStream;
        }

        public char Peek()
        {
            return (char)reader.Peek();
        }

        public void Reset()
        {
            if(reader.BaseStream.CanSeek)
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
            }
            reader.DiscardBufferedData();
        }
    }
}
