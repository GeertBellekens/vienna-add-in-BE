using CctsRepository;
using CctsRepository.CdtLibrary;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3;

namespace VIENNAAddInUnitTests.upcc3.ccts.dra
{
    [TestFixture]
    public class CdtLibraryTests
    {
        [Test]
        public void ShouldAutoGenerateTaggedValues()
        {
            ICctsRepository ccRepository = CctsRepositoryFactory.CreateCctsRepository(new CdtLibraryTestRepository());
            ICdtLibrary cdtLibrary = ccRepository.GetCdtLibraryByPath(CdtLibraryTestRepository.PathToCdtLibrary());

            CdtSpec cdtSpec = new CdtSpec {Name = "cdt1"};
            ICdt cdt = cdtLibrary.CreateCdt(cdtSpec);

            Assert.That(cdt.UniqueIdentifier, Is.Not.Null);
            Assert.That(cdt.UniqueIdentifier, Is.Not.Empty);
            Assert.That(cdt.DictionaryEntryName, Is.EqualTo("cdt1. Type"));
        }

        [Test]
        public void ShouldOnlyAutoGenerateUnspecifiedTaggedValues()
        {
            ICctsRepository ccRepository = CctsRepositoryFactory.CreateCctsRepository(new CdtLibraryTestRepository());
            ICdtLibrary cdtLibrary = ccRepository.GetCdtLibraryByPath(CdtLibraryTestRepository.PathToCdtLibrary());

            CdtSpec cdtSpec = new CdtSpec { Name = "cdt1", UniqueIdentifier = "{x-123-456-789-x}", DictionaryEntryName = "shouldNotBeReplaced" };
            ICdt cdt = cdtLibrary.CreateCdt(cdtSpec);

            Assert.That(cdt.UniqueIdentifier, Is.EqualTo("{x-123-456-789-x}"));
            Assert.That(cdt.DictionaryEntryName, Is.EqualTo("shouldNotBeReplaced"));
        }

        [Test]
        public void ShouldAutoGenerateClonedTaggedValues()
        {
            ICctsRepository ccRepository = CctsRepositoryFactory.CreateCctsRepository(new CdtLibraryTestRepository());
            ICdtLibrary cdtLibrary = ccRepository.GetCdtLibraryByPath(CdtLibraryTestRepository.PathToCdtLibrary());
            ICdt cdtText = ccRepository.GetCdtByPath(CdtLibraryTestRepository.PathToCdtText());

            CdtSpec cdtSpec = CdtSpec.CloneCdt(cdtText);
            cdtSpec.Name = "cdt1";

            ICdt cdt = cdtLibrary.CreateCdt(cdtSpec);

            Assert.That(cdt.UniqueIdentifier, Is.Not.Null);
            Assert.That(cdt.UniqueIdentifier, Is.Not.Empty);
            Assert.That(cdt.UniqueIdentifier, Is.Not.EqualTo(cdtText.UniqueIdentifier));
            Assert.That(cdt.DictionaryEntryName, Is.EqualTo("cdt1. Type"));
        }

        [Test]
        public void ShouldNotUpdateAutoGeneratedTaggedValues()
        {
            ICctsRepository ccRepository = CctsRepositoryFactory.CreateCctsRepository(new CdtLibraryTestRepository());
            ICdtLibrary cdtLibrary = ccRepository.GetCdtLibraryByPath(CdtLibraryTestRepository.PathToCdtLibrary());
            
            ICdt cdtText = ccRepository.GetCdtByPath(CdtLibraryTestRepository.PathToCdtText());
            string cdtTextUniqueIdentifier = cdtText.UniqueIdentifier;
            string cdtTextDictionaryEntryName = cdtText.DictionaryEntryName;

            CdtSpec cdtSpec = CdtSpec.CloneCdt(cdtText);

            cdtSpec.Name = "cdt1";

            ICdt updatedCdt = cdtLibrary.UpdateCdt(cdtText, cdtSpec);
            
            Assert.That(updatedCdt.UniqueIdentifier, Is.EqualTo(cdtTextUniqueIdentifier));
            Assert.That(updatedCdt.DictionaryEntryName, Is.EqualTo(cdtTextDictionaryEntryName));
        }

        [Test]
        public void ShouldNotAutoGenerateSpecifiedTaggedValues()
        {
            ICctsRepository ccRepository = CctsRepositoryFactory.CreateCctsRepository(new CdtLibraryTestRepository());
            ICdtLibrary cdtLibrary = ccRepository.GetCdtLibraryByPath(CdtLibraryTestRepository.PathToCdtLibrary());

            ICdt cdtText = ccRepository.GetCdtByPath(CdtLibraryTestRepository.PathToCdtText());            
            
            CdtSpec cdtSpec = CdtSpec.CloneCdt(cdtText);

            cdtSpec.DictionaryEntryName = "cdt1";
            cdtSpec.UniqueIdentifier = "{1-2-3}";
            
            ICdt updatedCdt = cdtLibrary.UpdateCdt(cdtText, cdtSpec);

            Assert.That(updatedCdt.UniqueIdentifier, Is.EqualTo("{1-2-3}"));
            Assert.That(updatedCdt.DictionaryEntryName, Is.EqualTo("cdt1"));
        }
    }
}