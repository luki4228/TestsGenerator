using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

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
            var linkOptions = new DataflowLinkOptions();
            linkOptions.PropagateCompletion = true;
            var readOptions = new ExecutionDataflowBlockOptions();
            readOptions.MaxDegreeOfParallelism = maxReading;
            var processOptions = new ExecutionDataflowBlockOptions();
            processOptions.MaxDegreeOfParallelism = maxProcessing;
            var writeOptions = new ExecutionDataflowBlockOptions();
            writeOptions.MaxDegreeOfParallelism = maxWriting;

            var readBlock = new TransformBlock<string, string>(fileName => Reader.Read(fileName), readOptions);
            var processBlock = new TransformBlock<string, List<Test>>(src => codeParcer.GenerateTests(src), processOptions);
            var writeBlock = new ActionBlock<List<Test>>(output => Writer.Write(outputPath, output).Wait(), writeOptions);

            readBlock.LinkTo(processBlock, linkOptions);
            processBlock.LinkTo(writeBlock, linkOptions);

            foreach (string file in inputFiles)
            {
                readBlock.Post(file);
            }
            readBlock.Complete();
            await writeBlock.Completion;
        }
    }
}
