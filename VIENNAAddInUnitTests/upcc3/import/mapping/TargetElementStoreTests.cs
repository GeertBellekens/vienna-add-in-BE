using System.Linq;
using CctsRepository;
using CctsRepository.CcLibrary;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.import.mapping;
using VIENNAAddInUtils;

namespace VIENNAAddInUnitTests.upcc3.import.mapping
{
    [TestFixture]
    public class TargetElementStoreTests
    {
        #region Setup/Teardown

        [SetUp]
        public void CreateExpectedSourceElementTree()
        {
            ccRepository = CctsRepositoryFactory.CreateCctsRepository(new MappingTestRepository());
            ccLibrary = ccRepository.GetCcLibraryByPath((Path) "test"/"bLibrary"/"CCLibrary");
            accParty = ccLibrary.GetAccByName("Party");
            bccPartyName = accParty.Bccs.First(bcc => bcc.Name == "Name");
            asccPartyResidence = accParty.Asccs.First(ascc => ascc.Name == "Residence");
            mapForceMapping = LinqToXmlMapForceMappingImporter.ImportFromFiles(TestUtils.PathToTestResource(@"XSDImporterTest\mapping\TargetElementStoreTests\mapping.mfd"));
        }

        #endregion

        private ICcLibrary ccLibrary;
        private MapForceMapping mapForceMapping;
        private IAcc accParty;
        private IBcc bccPartyName;
        private IAscc asccPartyResidence;
        private ICctsRepository ccRepository;

        private void ShouldContainTargetACCElement(TargetElementStore targetElementStore, string name, IAcc referencedAcc)
        {
            object targetCc = ShouldContainTargetCc(name, targetElementStore, referencedAcc);
            Assert.IsTrue(targetCc is IAcc);
            Assert.That(((IAcc)targetCc).Id, Is.EqualTo(referencedAcc.Id));
        }

        private void ShouldContainTargetBccElement(TargetElementStore targetElementStore, string name, IBcc referencedBcc)
        {
            object targetCc = ShouldContainTargetCc(name, targetElementStore, referencedBcc);
            Assert.IsTrue(targetCc is IBcc);
            Assert.That(((IBcc)targetCc).Id, Is.EqualTo(referencedBcc.Id));
        }

        private void ShouldContainTargetAsccElement(TargetElementStore targetElementStore, string name, IAscc referencedAscc)
        {
            object targetCc = ShouldContainTargetCc(name, targetElementStore, referencedAscc);
            Assert.IsTrue(targetCc is IAscc);
            Assert.That(((IAscc)targetCc).Id, Is.EqualTo(referencedAscc.Id));
        }

        private object ShouldContainTargetCc(string name, TargetElementStore targetElementStore, object referencedCc)
        {
            string entryKey = GetTargetEntryKey(name);
            object targetCc = targetElementStore.GetTargetCc(entryKey);
            Assert.That(targetCc, Is.Not.Null, "Target CC '" + name + "' not found");
            Assert.That(targetCc, Is.EqualTo(referencedCc));
            return targetCc;
        }

        private string GetTargetEntryKey(string targetEntryName)
        {
            return FindTargetEntry(targetEntryName).InputOutputKey.Value;
        }

        private Entry FindTargetEntry(string name)
        {
            foreach (SchemaComponent component in mapForceMapping.GetTargetSchemaComponents())
            {
                Entry entry = FindEntryByName(component.RootEntry, name);
                if (entry != null)
                {
                    return entry;
                }
            }
            return null;
        }

        private static Entry FindEntryByName(Entry entry, string name)
        {
            if (entry.Name == name)
            {
                return entry;
            }
            foreach (Entry subEntry in entry.SubEntries)
            {
                Entry e = FindEntryByName(subEntry, name);
                if (e != null)
                {
                    return e;
                }
            }
            return null;
        }

        [Test]
        public void TestTargetElementStore()
        {
            var targetElementStore = new TargetElementStore(mapForceMapping, ccLibrary, ccRepository);
            ShouldContainTargetACCElement(targetElementStore, "Party", accParty);
            ShouldContainTargetBccElement(targetElementStore, "Name", bccPartyName);
            ShouldContainTargetAsccElement(targetElementStore, "ResidenceAddress", asccPartyResidence);
        }
    }
}