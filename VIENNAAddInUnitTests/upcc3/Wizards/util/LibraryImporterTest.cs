// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.IO;
using EA;
using NUnit.Framework;
using VIENNAAddIn;
using VIENNAAddIn.upcc3.Wizards.util;
using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;
using Path=VIENNAAddInUtils.Path;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class LibraryImporterTest
    {
        private readonly ResourceDescriptor resourceDescriptor = new ResourceDescriptor
                                                                     (
                                                                      new[]
                                                                                         {
                                                                                             "simplified_enumlibrary.xmi"
                                                                                             ,
                                                                                             "simplified_primlibrary.xmi"
                                                                                             ,
                                                                                             "simplified_cdtlibrary.xmi"
                                                                                             ,
                                                                                             "simplified_cclibrary.xmi"
                                                                                         },
                                                                      
                                                                             "http://www.umm-dev.org/xmi/testresources/simplified_1/",
                                                                      
                                                                             Directory.GetCurrentDirectory() +
                                                                             "\\..\\..\\..\\VIENNAAddInUnitTests\\testresources\\ModelCreatorTest\\download\\"
                                                                     );

        // Purpose: Test the import capability of the model creator to import the standard CC libraries
        // into a particular model which contains an empty business library named bLibrary. 

        private static void AssertEmptyStandardCcLibrariesInBusinessLibrary(Repository repository, string modelName,
                                                                            string bLibraryName)
        {
            Assert.IsNotNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"ENUMLibrary"),
                             "The business library \"{0}\" is missing an ENUM library named \"ENUMLibrary\".",
                             bLibraryName);
            Assert.IsNotNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"PRIMLibrary"),
                             "The business library \"{0}\" is missing a PRIM library named \"PRIMLibrary\".",
                             bLibraryName);
            Assert.IsNotNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"CDTLibrary"),
                             "The business library \"{0}\" is missing a CDT library named \"CDTLibrary\".", bLibraryName);
            Assert.IsNotNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"CCLibrary"),
                             "The business library \"{0}\" is missing an CC library named \"CCLibrary\".", bLibraryName);
        }

        private static void AssertStandardCcLibrariesContentInBusinessLibrary(Repository repository, string modelName,
                                                                              string bLibraryName)
        {
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"PRIMLibrary"/"Boolean"),
                             "The PRIM library \"PRIMLibrary\" is missing a primitive type named \"Boolean\".");
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"PRIMLibrary"/"Date"),
                             "The PRIM library \"PRIMLibrary\" is missing a primitive type named \"Date\".");
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"PRIMLibrary"/"Decimal"),
                             "The PRIM library \"PRIMLibrary\" is missing a primitive type named \"Decimal\".");
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"PRIMLibrary"/"String"),
                             "The PRIM library \"PRIMLibrary\" is missing a primitive type named \"String\".");

            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"CDTLibrary"/"DateTime"),
                             "The CDT library \"CDTLibrary\" is missing a primitive type named \"DateTime\".");
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"CDTLibrary"/"Name"),
                             "The CDT library \"CDTLibrary\" is missing a primitive type named \"Name\".");
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"CDTLibrary"/"Text"),
                             "The CDT library \"CDTLibrary\" is missing a primitive type named \"Text\".");
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"CDTLibrary"/"Time"),
                             "The CDT library \"CDTLibrary\" is missing a primitive type named \"Time\".");

            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"CCLibrary"/"Address"),
                             "The CC library \"CCLibrary\" is missing a primitive type named \"Address\".");
            Assert.IsNotNull(repository.Resolve<Element>((Path) modelName/bLibraryName/"CCLibrary"/"Person"),
                             "The CC library \"CCLibrary\" is missing a primitive type named \"Person\".");
        }

        private static void AssertNonExistenceOfBieLibrariesinBusinessLibrary(Repository repository, string modelName,
                                                                              string bLibraryName)
        {
            Assert.IsNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"BDTLibrary"),
                          "The business library \"{0}\" contains a BDT library named \"BDTLibrary\" which is not supposed to be part of the business library.",
                          bLibraryName);
            Assert.IsNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"BIELibrary"),
                          "The business library \"{0}\" contains a BIE library named \"BIELibrary\" which is not supposed to be part of the business library.",
                          bLibraryName);
            Assert.IsNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"DOCLibrary"),
                          "The business library \"{0}\" contains a DOC library named \"DOCLibrary\" which is not supposed to be part of the business library.",
                          bLibraryName);
        }

        private static void AssertEmptyBieLibrariesinBusinessLibrary(Repository repository, string modelName,
                                                                     string bLibraryName)
        {
            Assert.IsNotNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"BDTLibrary"),
                             "The business library \"{0}\" is missing an BDT library named \"BDTLibrary\".",
                             bLibraryName);
            Assert.IsNotNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"BIELibrary"),
                             "The business library \"{0}\" is missing a BIE library named \"BIELibrary\".", bLibraryName);
            Assert.IsNotNull(repository.Resolve<Package>((Path) modelName/bLibraryName/"DOCLibrary"),
                             "The business library \"{0}\" is missing a DOC library named \"DOCLibrary\".", bLibraryName);
        }

        [Test]
        [Category("file-based")]
        public void TestImportStandardCcLibrariesIntoEmptyBusinessLibrary()
        {
            Repository eaRepository = new TemporaryFileBasedRepository(new EARepositoryLibraryImporter());
            var bLibrary = eaRepository.Resolve<Package>((Path) "Test Model 1"/"bLibrary");

            DeleteDownloadDirectory();

            var importer = new LibraryImporter(eaRepository, resourceDescriptor);
            importer.ImportStandardCcLibraries(bLibrary);

            AssertEmptyStandardCcLibrariesInBusinessLibrary(eaRepository, "Test Model 1", "bLibrary");
            AssertStandardCcLibrariesContentInBusinessLibrary(eaRepository, "Test Model 1", "bLibrary");
            AssertNonExistenceOfBieLibrariesinBusinessLibrary(eaRepository, "Test Model 1", "bLibrary");
        }

        private void DeleteDownloadDirectory()
        {
            if (Directory.Exists(resourceDescriptor.StorageDirectory))
            {
                Directory.Delete(resourceDescriptor.StorageDirectory, true);    
            }
        }

        // Purpose: Test the import capability of the model creator to import the standard CC libraries
        // into a particular model which already contains CC libraries. The test therefore checks if the 
        // existing CC libraries are deleted/replaced by the standard CC libraries. 
        [Test]
        [Category("file-based")]
        public void TestImportStandardCcLibrariesIntoFullBusinessLibrary()
        {
            Repository eaRepository = new TemporaryFileBasedRepository(new EARepositoryLibraryImporter());

            var bLibrary = eaRepository.Resolve<Package>((Path) "Test Model 2"/"bLibrary");

            DeleteDownloadDirectory();

            var importer = new LibraryImporter(eaRepository, resourceDescriptor);
            importer.ImportStandardCcLibraries(bLibrary);

            AssertEmptyStandardCcLibrariesInBusinessLibrary(eaRepository, "Test Model 2", "bLibrary");
            AssertStandardCcLibrariesContentInBusinessLibrary(eaRepository, "Test Model 2", "bLibrary");
            AssertEmptyBieLibrariesinBusinessLibrary(eaRepository, "Test Model 2", "bLibrary");
        }
    }
}