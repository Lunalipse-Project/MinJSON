using MinJSON.JSON;
using MinJSON.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                TestParse();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"{stopwatch.ElapsedMilliseconds}ms");
            }
            Console.ReadKey();
        }

        static void TestSerializer()
        {
            const string json = "{\"abc\":-2.23e3,\"ced\":\"my field\",\"list\":[{\"aa\":\"bbb\",\"a\":2},{\"aa\":\"bbb\",\"a\":4},{\"aa\":\"bbb\",\"a\":6},{\"aa\":\"bbb\",\"a\":43}]}";
            JsonDeserializer<DesSample> deserializer = new JsonDeserializer<DesSample>();
            DesSample desSample = deserializer.Unpack(JsonObject.Parse(json));
        }

        static void TestParse()
        {
            using (FileStream fs = new FileStream(@"C:\Users\minec\Desktop\p100000.json", FileMode.Open))
            {
                JsonObject jo = JsonObject.LoadFrom(fs);
            }
        }
    }

    [JsonSerializable]
    class DesSample
    {
        public int abc;
        public string ced;
        [JsonProperty("list")]
        private List<Simple3> simple3s;
    }
    [JsonSerializable(0)]
    class Simple3
    {
        string privateStuff = "pppp";
        [JsonProperty("aa")]
        string privateButVisible = "";
        public int a = 0;
        public Simple3(int b)
        {
            a = b;
        }
    }
}
