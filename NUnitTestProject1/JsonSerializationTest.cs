using MinJSON.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NUnitTestProject1
{
    class JsonSerializationTest
    {
        [Test]
        public void TestSimplePOCO()
        {
            Simple simp= new Simple();
            JsonSerializer<Simple> serializer = new JsonSerializer<Simple>();
            Console.WriteLine(serializer.Pack(simp));
        }
    }

    class Simple
    {
        public int field1 = 23;
        public string hello = "world";
        public Dictionary<string, Simple2> valuePairs = new Dictionary<string, Simple2>()
        {
            {"er",new Simple2("jdcdnd") },
            {"eb",new Simple2("scdfkds") }
        };
        [JsonProperty(PropertyName = "simpleClass")]
        public Simple2 simple = new Simple2("aanbcd");
    }

    class Simple2
    {
        public int[] abc = new[] { 1, 2, 3, 4, 5, 6 };
        public string str = "adjcdnsjc";
        public List<Simple3> sims = new List<Simple3>()
        {
            new Simple3(2),
            new Simple3(4),
            new Simple3(6),
            new Simple3(43)
        };
        public Simple2(string str)
        {
            this.str = str;
        }
    }

    class Simple3
    {
        string privateStuff = "pppp";
        [JsonProperty(PropertyName = "aa")]
        string privateButVisible = "bbb";
        public int a = 0;
        public Simple3(int b)
        {
            a = b;
        }
    }
}
