using MinJSON.JSON;
using MinJSON.Parser;
using MinJSON.Parser.Lexer;
using MinJSON.Parser.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NUnitTestProject1
{
    class JsonParserTest
    {
        [Test]
        public void TestLexer()
        {
            using(FileStream fs = new FileStream(@"E:\Lunalipse\Lunalipse\Resources\Themes+StarlightGlimmer.json", FileMode.Open))
            {
                JsonLexer lexer = new JsonLexer(fs);
                foreach(Token token in lexer)
                {
                    Console.WriteLine(token);
                }
            }
        }
        [Test]
        public void TestParser()
        {
            using (FileStream fs = new FileStream(@"E:\Workspace\cw2p2\src\test\resources\json\test4.json", FileMode.Open))
            {
                JsonLexer lexer = new JsonLexer(fs);
                JsonParser jsonParser = new JsonParser(lexer);
                JsonValue jv= jsonParser.Parse();
                //Console.WriteLine(jv);
                Assert.NotNull(jv);
            }
        }

        [Test]
        public void TestParserLarge()
        {
            using (FileStream fs = new FileStream(@"C:\Users\minec\Desktop\p100000.json", FileMode.Open))
            {
                JsonLexer lexer = new JsonLexer(fs);
                JsonParser jsonParser = new JsonParser(lexer);
                JsonValue jv = jsonParser.Parse();
                //Console.WriteLine(jv);
                Assert.NotNull(jv);
            }
        }

        [Test]
        public void TestParserNested()
        {
            const string json = "{\"a\":\"Alpha\",\"b\":true,\"c\":12345,\"d\":[true,[false,[-123456789,null],3.9676,[\"Something else.\",false],null]],\"e\":{\"zero\":null,\"one\":1,\"two\":2,\"three\":[3],\"four\":[0,1,2,3,4]},\"f\":null,\"h\":{\"a\":{\"b\":{\"c\":{\"d\":{\"e\":{\"f\":{\"g\":null}}}}}}},\"i\":[[[[[[[null]]]]]]]}";
            JsonLexer lexer = new JsonLexer(json);
            JsonParser jsonParser = new JsonParser(lexer);
            JsonValue jv = jsonParser.Parse();
            //Console.WriteLine(jv);
            Assert.NotNull(jv);
        }

        [Test]
        public void TestParserDNetNested()
        {
            const string json = "{\"a\":\"Alpha\",\"b\":true,\"c\":12345,\"d\":[true,[false,[-123456789,null],3.9676,[\"Something else.\",false],null]],\"e\":{\"zero\":null,\"one\":1,\"two\":2,\"three\":[3],\"four\":[0,1,2,3,4]},\"f\":null,\"h\":{\"a\":{\"b\":{\"c\":{\"d\":{\"e\":{\"f\":{\"g\":null}}}}}}},\"i\":[[[[[[[null]]]]]]]}";
            Assert.NotNull(JObject.Parse(json));
        }

        [Test]
        public void TestJsonDNetLarge()
        {
            using (FileStream fs = new FileStream(@"C:\Users\minec\Desktop\p100000.json", FileMode.Open))
            {
                JObject jObject = JObject.Load(new JsonTextReader(new StreamReader(fs)));
                Assert.NotNull(jObject);
            }
        }
    }
}
