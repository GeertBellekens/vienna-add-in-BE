using System;
using System.IO;

namespace VIENNAAddInUnitTests
{
    public static class TestUtils
    {
        public static string PathToTestResource(string relativePath)
        {
            return Directory.GetCurrentDirectory() + @"\..\..\testresources\" + relativePath;
        }

        /// <summary>
        /// Resolves a relative path to test resources which are co-located with the test class.
        /// 
        /// Assumes that the tests are executed in a directory three levels below the solution directory (e.g. "VIENNAAddInUnitTests\bin\debug").
        /// 
        /// Example:
        /// 
        /// Test class: VIENNAAddInUnitTests.upcc3.import.cctsndr.BDTXsdImporterTests
        /// Relative path: "BDTImporterTestResources\BusinessDataType_complex_type.xsd"
        /// Return value: "{test-project-directory}\VIENNAAddInUnitTests\upcc3\import\cctsndr\BDTXsdImporterTests\BusinessDataType_complex_type.xsd"
        /// </summary>
        /// <param name="testClass"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static string RelativePathToTestResource(Type testClass, string relativePath)
        {
            return Directory.GetCurrentDirectory() + @"\..\..\..\" + testClass.Namespace.Replace('.', '\\') + "\\" + relativePath;
        }
    }
}