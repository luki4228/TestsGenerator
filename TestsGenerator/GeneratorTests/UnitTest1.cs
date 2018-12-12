using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;
using TestsGenerator;

namespace GeneratorTests
{
    [TestClass]
    public class UnitTest1
    {
        private List<CInfo> parcedClasses;
        private List<Test> generatedTests;

        [TestInitialize]
        public void Initialize()
        {
            string path = "Class1.cs";

            int maxReading = 2;
            int maxWriting = 3;
            int maxProcessing = 4;

            var generator = new Generator(maxReading, maxWriting, maxProcessing);

            string sourceCode;
            using (StreamReader strmReader = new StreamReader(path))
            {
                sourceCode = strmReader.ReadToEnd();
            }

            var parcer = new CodeParcer();
            parcedClasses = parcer.Parce(sourceCode);
            generatedTests = generator.GenerateTests(sourceCode);
        }

        [TestMethod]
        public void ClassCountTest()
        {
            Assert.AreEqual(parcedClasses.Count, 2);
        }

        [TestMethod]
        public void ClassEqualNamespacesTest()
        {
            Assert.AreEqual(parcedClasses[0].Namespace, parcedClasses[1].Namespace);
        }

        [TestMethod]
        public void ClassInfoTest()
        {
            Assert.AreEqual(parcedClasses[0].Namespace, "UnitTests");
            Assert.AreEqual(parcedClasses[1].Namespace, "UnitTests");
            Assert.AreEqual(parcedClasses[0].Name, "Class1");
            Assert.AreEqual(parcedClasses[1].Name, "Class2");
            Assert.AreEqual(parcedClasses[0].Methods[0], "Method1");
            Assert.AreEqual(parcedClasses[1].Methods[0], "Method2");
        }

        [TestMethod]
        public void TestClassesAmountTest()
        {
            Assert.AreEqual(generatedTests.Count, 2);
        }

        [TestMethod]
        public void TestNameTest()
        {
            Assert.AreEqual(generatedTests[0].Name, "Class1Test.cs");
            Assert.AreEqual(generatedTests[1].Name, "Class2Test.cs");
        }

        [TestMethod]
        public void TestContentTest()
        {
            generatedTests[0].Content.Contains("GeneratorTests.Tests");
            generatedTests[1].Content.Contains("GeneratorTests.Tests");
            generatedTests[0].Content.Contains("Class1Test");
            generatedTests[1].Content.Contains("Class2Test");
            generatedTests[0].Content.Contains("Method1Test");
            generatedTests[1].Content.Contains("Method2Test");
        }
    }
}
