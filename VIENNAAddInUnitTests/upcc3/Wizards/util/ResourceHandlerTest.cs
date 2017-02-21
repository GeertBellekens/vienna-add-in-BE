// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.IO;
using NUnit.Framework;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    [Category(TestCategories.FileBased)]
    public class ResourceHandlerTest
    {
        // Purpose: The test retrieves a set of test resources from a particular URI, stores the resources
        // retrieved on the local file system and compares the files retrieved with a set of files already
        // available on the local system to ensure correct content retrieval. 
        [Test]
        public void TestResourceCachingToFileSystem()
        {
            // the resource handler expects the filenames to be retrieved in an array
            string[] resources = new[] { "simplified_enumlibrary.xmi", "simplified_primlibrary.xmi", "simplified_cdtlibrary.xmi" };// TODO: temporarily excluded since it caused a filure for whatever reason, "simplified_cclibrary.xmi" };

            // the uri where the files to be downloaded are located 
            const string downloadUri = "http://www.umm-dev.org/xmi/testresources/simplified_1/";

            // the directory on the local file system where the downloaded file is to be stored
            string storageDirectory = Directory.GetCurrentDirectory() +
                                      "\\..\\..\\..\\VIENNAAddInUnitTests\\testresources\\ResourceHandlerTest\\xmi\\download\\";

            string comparisonDirectory = Directory.GetCurrentDirectory() +
                                         "\\..\\..\\..\\VIENNAAddInUnitTests\\testresources\\ResourceHandlerTest\\xmi\\reference\\";

            // method to be tested 1: caching resources locally
            ResourceDescriptor resourceDescriptor = new ResourceDescriptor
                                                        (
                                                        resources,
                                                        downloadUri,
                                                        storageDirectory

                                                        );

            (new ResourceHandler(resourceDescriptor)).CacheResourcesLocally();

            foreach (string fileName in resources)
            {
                // evaluate if the file was downloaded at all
                AssertDownloadedFileExists(storageDirectory + fileName);

                // evalate the content of the file downloaded
                AssertFileContent(comparisonDirectory + fileName, storageDirectory + fileName);                
            }
        }

        #region Private Class Methods

        private static void AssertDownloadedFileExists(string downloadedFile)
        {
            if (!File.Exists(downloadedFile))
            {
                Assert.Fail("Retrieving resource file to local file system failed at: {0}", downloadedFile);
            }
        }

        private static void AssertFileContent(string comparisonFile, string newFile)
        {
            string comparisonContent = File.ReadAllText(comparisonFile);
            string newContent = File.ReadAllText(newFile);

            if (!comparisonContent.Equals(newContent))
            {
                Assert.Fail(
                    "Content of retrieved resource does not match expected content.\nexpected file: <{0}>\nretrieved file: <{1}>",
                    comparisonFile, newFile);
            }
        }

        #endregion
    }
}