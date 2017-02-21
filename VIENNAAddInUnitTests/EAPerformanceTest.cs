using System;
using CctsRepository;
using CctsRepository.CcLibrary;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests
{
    [TestFixture]
    public class EAPerformanceTest
    {
        [Test]
        [Ignore("Depends on test file C:/Temp/test.eap")]
        public void AccessEARepositoryDirectlyAndLoadNamesOfAllElements()
        {
            Repository repository = new Repository();
            repository.OpenFile("C:/Temp/test.eap");
            Package model = (Package) repository.Models.GetAt(0);
            Package package = (Package) model.Packages.GetAt(0);
            Package Cclib = (Package) package.Packages.GetAt(2);

            foreach (Element element in Cclib.Elements)
            {
                Console.WriteLine(element.Name);
            }
        }
        [Test]
        [Ignore("Depends on test file C:/Temp/test.eap")]
        public void AccessCctsRepositoryAndLoadNamesOfAllElements()
        {
            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(new TemporaryFileBasedRepository("C:/Temp/test.eap"));
            foreach (ICcLibrary ccLibrary in cctsRepository.GetCcLibraries())
            {
                foreach (IAcc acc in ccLibrary.Accs)
                {
                    Console.WriteLine(acc.Name);
                }   
            }
        }
    }

}