using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestsGenerator
{
    public static class Writer
    {
        public static async Task Write(string outputPath, List<TestInfo> tests)
        {
            string path;
            if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);
            foreach (Test test in tests)
            {
                path = outputPath + "\\" + test.TestName;
                using (StreamWriter writer = new StreamWriter(path))
                {
                    await writer.WriteAsync(test.TestContent);
                }
            }
        }
    }
}
