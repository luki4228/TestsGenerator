using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsGenerator
{
    public class Generator
    {
        int readingThreadAmout;
        int writingThreadAmout;
        int maxProcessingThreadAmout;

        public Generator(int readingThreadAmout, int writingThreadAmout, int maxProcessingThreadAmout)
        {
            this.readingThreadAmout = readingThreadAmout;
            this.writingThreadAmout = writingThreadAmout;
            this.maxProcessingThreadAmout = maxProcessingThreadAmout;
        }

        public async Task Generate(List<string> inputFiles, string outputPath)
        {
            var codeParcer = new CodeParcer();
         
        }
    }
}
