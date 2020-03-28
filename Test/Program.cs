using MinJSON.JSON;
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
                using (FileStream fs = new FileStream(@"E:\桌面工作目录\正在进行的\citylots.json", FileMode.Open))
                {
                    JsonObject jsonObject = JsonObject.LoadFrom(fs);
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"{stopwatch.ElapsedMilliseconds / 1000d}s");
            }
            Console.ReadKey();
        }
    }
}
