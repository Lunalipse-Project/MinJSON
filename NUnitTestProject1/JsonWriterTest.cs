using MinJSON.Writer;
using NUnit.Framework;
using System;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Empty()
        {
            JSONSeqentialWriter writer = new JSONSeqentialWriter();
            writer.WriteObjectBegin();
            writer.WriteObjectEnd();
            Console.WriteLine(writer);
        }

        [Test]
        public void EmptyArray()
        {
            JSONSeqentialWriter writer = new JSONSeqentialWriter();
            writer.WriteArrayBegin();
            writer.WriteArrayEnd();
            Console.WriteLine(writer);
        }

        [Test]
        public void Test1()
        {
            JSONSeqentialWriter writer = new JSONSeqentialWriter();
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
        }
        [Test]
        public void TestNested()
        {
            JSONSeqentialWriter writer = new JSONSeqentialWriter();
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
        }

        [Test]
        public void TestNestedArrayObject()
        {
            JSONSeqentialWriter writer = new JSONSeqentialWriter();
            writer.WriteObjectBegin();
            writer.WriteProperty("a4");
            writer.WriteArrayBegin();
            writer.WriteInteger(2);
            writer.WriteInteger(3);
            writer.WriteObjectBegin();
            writer.WriteProperty("B4");
            writer.WriteInteger(23);
            writer.WriteProperty("B3");
            writer.WriteBoolean(false);
            writer.WriteObjectEnd();
            writer.WriteString("2333");
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
            Console.WriteLine(writer);
        }
    }
}