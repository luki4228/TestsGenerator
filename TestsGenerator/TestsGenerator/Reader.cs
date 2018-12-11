using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestsGenerator
{
    public static class Reader
    {
        public static async Task<string> Read(string filePath)
        {
            using (StreamReader strmReader = new StreamReader(filePath))
            {
                return await strmReader.ReadToEndAsync();
            }
        }
    }
}
