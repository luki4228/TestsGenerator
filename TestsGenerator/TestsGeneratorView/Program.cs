using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsGenerator;

namespace TestsGeneratorView
{
    class Program
    {
        static void Main(string[] args)
        {
            var srcFiles = new List<string>();
            srcFiles.Add(@"D:\Test\input\Class1.cs");
            srcFiles.Add(@"D:\Test\input\ConsoleWriter.cs");
            string output = @"D:\Test\output";

            int maxReading = 2;
            int maxWriting = 3;
            int maxProcessing = 4;

            var generator = new Generator(maxReading, maxWriting, maxProcessing);
            generator.Generate(srcFiles, output).Wait();

            Console.WriteLine("Success");
            Console.ReadKey();
        }
    }
}
