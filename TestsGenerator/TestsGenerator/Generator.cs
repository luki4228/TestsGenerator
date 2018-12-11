using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsGenerator
{
    public class Generator
    {
        int maxReading;
        int maxWriting;
        int maxProcessing;

        public Generator(int _maxReading, int _maxWriting, int _maxProcessing)
        {
            maxReading = _maxReading;
            maxWriting = _maxWriting;
            maxProcessing = _maxProcessing;
        }

        public async Task Generate(List<string> inputFiles, string outputPath)
        {
            var codeParcer = new CodeParcer();
         
        }
    }
}
