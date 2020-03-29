using MinJSON.Writer;
using NUnit.Framework;
using System;

namespace Tests
{
    public class Tests
    {
        ISeqentialWriter writer;
        [SetUp]
        public void Setup()
        {
            writer = new JsonTextWriter();
        }

        [Test]
        public void Empty()
        {
            writer.WriteObjectBegin();
            writer.WriteObjectEnd();
            Console.WriteLine(writer);
            writer.WriterReset();
        }

        [Test]
        public void EmptyArray()
        {
            writer.WriteArrayBegin();
            writer.WriteArrayEnd();
            Console.WriteLine(writer);
            writer.WriterReset();
        }

        [Test]
        public void Test1()
        {
            writer.WriteObjectBegin();
            writer.WriteProperty("a1");
            writer.WriteLong(1232231L);
            writer.WriteProperty("a2");
            writer.WriteNull();
            writer.WriteProperty("a3");
            writer.WriteString("hello world");
            writer.WriteProperty("a4");
            writer.WriteBoolean(false);
            writer.WriteObjectEnd();
            Console.WriteLine(writer);
            writer.WriterReset();
        }
        [Test]
        public void TestNested()
        {
            writer.WriteObjectBegin();
            writer.WriteProperty("a1");
            writer.WriteLong(1232231L);
            writer.WriteProperty("a4");
            writer.WriteArrayBegin();
            writer.WriteInteger(2);
            writer.WriteInteger(3);
            writer.WriteString("2333");
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
            Console.WriteLine(writer);
            writer.WriterReset();
        }

        [Test]
        public void TestNestedArrayObject()
        {
            writer.WriteObjectBegin();
            writer.WriteProperty("a4");
            writer.WriteArrayBegin();
            writer.WriteObjectBegin();
            writer.WriteProperty("B4");
            writer.WriteInteger(23);
            writer.WriteProperty("B3");
            writer.WriteBoolean(false);
            writer.WriteObjectEnd();
            writer.WriteObjectBegin();
            writer.WriteProperty("B4");
            writer.WriteInteger(23);
            writer.WriteProperty("B3");
            writer.WriteBoolean(false);
            writer.WriteObjectEnd();
            writer.WriteObjectBegin();
            writer.WriteProperty("B4");
            writer.WriteArrayBegin();
            writer.WriteSingle(2);
            writer.WriteString("e\\b\\c");
            writer.WriteArrayEnd();
            writer.WriteProperty("B3");
            writer.WriteString(@"e\\v\\s");
            writer.WriteObjectEnd();
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
            Console.WriteLine(writer);
            writer.WriterReset();
        }
    }
}