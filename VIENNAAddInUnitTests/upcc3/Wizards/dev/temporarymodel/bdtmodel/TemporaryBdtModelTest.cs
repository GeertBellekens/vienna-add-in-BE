// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Windows.Input;
using CctsRepository.BdtLibrary;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.Wizards.dev.temporarymodel.bdtmodel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CctsRepository;
using CctsRepository.CdtLibrary;
using System.Collections.Generic;
using VIENNAAddIn.upcc3.Wizards.dev.util;
using VIENNAAddInUnitTests.upcc3.Wizards.dev.TestRepository;
using VIENNAAddInUtils;
using Assert=NUnit.Framework.Assert;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.temporarymodel.bdtmodel
{
    /// <summary>
    ///This is a test class for TemporaryBdtModelTest and is intended
    ///to contain all TemporaryBdtModelTest Unit Tests
    ///</summary>
    [TestFixture]
    public class TemporaryBdtModelTest
    {
        private ICctsRepository cctsRepository;
        private TemporaryBdtModel target;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        
        //SetUp runs code before each test
        [SetUp]
        public void SetUp()
        {
            cctsRepository = CctsRepositoryFactory.CreateCctsRepository(new EARepositoryBdtEditor());
            target = new TemporaryBdtModel(cctsRepository);
                var candidateCdtLibraryIndex = 0;
                target.mCandidateCdtLibraries[candidateCdtLibraryIndex].mCandidateCdts = new List<CandidateCdt>();
                target.CandidateCdtNames = new List<string>();
                target.CandidateConItems = new List<CheckableItem>();
                target.CandidateSupItems = new List<CheckableItem>();
                foreach (ICdt cdt in cctsRepository.GetCdtLibraryByPath((Path)"test model"/"blib1"/"cdtlib1").Cdts)
                {
                    var candidateCdt = new CandidateCdt(cdt);
                    target.mCandidateCdtLibraries[candidateCdtLibraryIndex].mCandidateCdts.Add(candidateCdt);
                    target.CandidateCdtNames.Add(cdt.Name);

                    int candidateCdtIndex =
                        target.mCandidateCdtLibraries[candidateCdtLibraryIndex].mCandidateCdts.IndexOf(candidateCdt);
                    target.mCandidateCdtLibraries[candidateCdtLibraryIndex].mCandidateCdts[candidateCdtIndex].
                        mPotentialCon.OriginalCdtCon = cdt.Con;
                    target.CandidateConItems.Add(new CheckableItem(false,cdt.Con.Name,false,false, Cursors.Arrow));
                    target.mCandidateCdtLibraries[candidateCdtLibraryIndex].CandidateCdts[candidateCdtIndex].mPotentialSups = new List<PotentialSup>();
                    foreach (ICdtSup cdtSup in cdt.Sups)
                    {
                        target.mCandidateCdtLibraries[candidateCdtLibraryIndex].mCandidateCdts[candidateCdtIndex].mPotentialSups.Add(new PotentialSup(cdtSup));
                        target.CandidateSupItems.Add(new CheckableItem(false,cdtSup.Name,false,false,Cursors.Arrow));
                    }
                }
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Prefix
        ///</summary>
        [Test]
        public void ShouldSetPrefixInTemporaryBdtModel()
        {
            const string expected = "MyPrefix";
            target.Prefix = expected;
            var actual = target.Prefix;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [Test]
        public void ShouldSetNameInTemporaryBdtModel()
        {
            const string expected = "MyName";
            target.Name = expected;
            var actual = target.Name;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CandidateSupItems
        ///</summary>
        [Test]
        public void ShouldSetCandidateSupItemsInTemporaryBdtModel()
        {
            var expected = new List<CheckableItem> {new CheckableItem(true,"test",false,false,Cursors.Arrow)};
            target.CandidateSupItems = expected;
            var actual = target.CandidateSupItems;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CandidateConItems
        ///</summary>
        [Test]
        public void ShouldSetCandidateConItemsInTemporaryBdtModel()
        {
            var expected = new List<CheckableItem> { new CheckableItem(true, "test", false, false, Cursors.Arrow) };
            target.CandidateConItems = expected;
            var actual = target.CandidateConItems;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CandidateCdtNames
        ///</summary>
        [Test]
        public void ShouldSetCandidateCdtNamesInTemporaryBdtModel()
        {
            var expected = new List<string>{"test1","test2"};
            target.CandidateCdtNames = expected;
            var actual = target.CandidateCdtNames;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CandidateCdtLibraryNames
        ///</summary>
        [Test]
        public void ShouldSetCandidateCdtLibraryNamesInTemporaryBdtModel()
        {
            var expected = new List<string> { "test1", "test2" };
            target.CandidateCdtLibraryNames = expected;
            var actual = target.CandidateCdtLibraryNames;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for CandidateBdtLibraryNames
        ///</summary>
        [Test]
        public void ShouldSetCandidateBdtLibraryNamesInTemporaryBdtModel()
        {
            var expected = new List<string> { "test1", "test2" };
            target.CandidateBdtLibraryNames = expected;
            var actual = target.CandidateBdtLibraryNames;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for setSelectedCandidateCdtLibrary
        ///</summary>
        [Test]
        public void ShouldSelectCandidateCdtLibraryByNameInTemporaryBdtModel()
        {
            const string selectedCdtLibrary = "cdtlib1";
            target.setSelectedCandidateCdtLibrary(selectedCdtLibrary);
            Assert.IsTrue(target.mCandidateCdtLibraries[0].Selected);
        }

        /// <summary>
        ///A test for setSelectedCandidateCdt
        ///</summary>
        [Test]
        public void ShouldSelectCandidateCdtByNameInTemporaryBdtModel()
        {
            const string selectedCdtLibrary = "cdtlib1";
            const string selectedCdt = "Text";
            target.setSelectedCandidateCdtLibrary(selectedCdtLibrary);
            target.setSelectedCandidateCdt(selectedCdt);
            Assert.IsTrue(target.mCandidateCdtLibraries[0].mCandidateCdts[3].Selected);
        }

        /// <summary>
        ///A test for setSelectedCandidateBdtLibrary
        ///</summary>
        [Test]
        public void ShouldSelectCandidateBdtLibraryInTemporaryBdtModel()
        {
            const string selectedBdtLibrary = "bdtlib1";
            target.setSelectedCandidateBdtLibrary(selectedBdtLibrary);
            Assert.IsTrue(target.mCandidateBdtLibraries[0].Selected);
        }

        /// <summary>
        ///A test for setCheckedPotentialSup
        ///</summary>
        [Test]
        public void ShouldSetCheckedPotentialSupInTemporaryBdtModel()
        {
            const bool checkedValue = true;
            const string selectedSup = "Language";
            const string selectedCdtLibrary = "cdtlib1";
            const string selectedCdt = "Text";
            target.setSelectedCandidateCdtLibrary(selectedCdtLibrary);
            target.setSelectedCandidateCdt(selectedCdt);
            target.setCheckedPotentialSup(checkedValue, selectedSup);
            Assert.IsTrue(target.mCandidateCdtLibraries[0].mCandidateCdts[3].mPotentialSups[0].Checked);
        }

        /// <summary>
        ///A test for setCheckedAllPotentialSups
        ///</summary>
        [Test]
        public void ShouldSetCheckedAllPotentialSupsInTemporaryBdtModel()
        {
            const string selectedCdtLibrary = "cdtlib1";
            const string selectedCdt = "Code";
            const bool checkedValue = true;
            target.setSelectedCandidateCdtLibrary(selectedCdtLibrary);
            target.setSelectedCandidateCdt(selectedCdt);
            target.setCheckedAllPotentialSups(checkedValue);
            foreach (PotentialSup potentialSup in target.mCandidateCdtLibraries[0].mCandidateCdts[0].mPotentialSups)
            {
                Assert.IsTrue(potentialSup.Checked);
            }
        }

        /// <summary>
        ///A test for CreateBdt
        ///</summary>
        [Test]
        public void ShouldPrepareTemporaryBdtModelandCreateBdt()
        {
            const string selectedCdtLibrary = "cdtlib1";
            const string selectedCdt = "Code";
            const bool checkedValue = true;
            target.setSelectedCandidateCdtLibrary(selectedCdtLibrary);
            target.setSelectedCandidateCdt(selectedCdt);
            target.setCheckedAllPotentialSups(checkedValue);
            target.Prefix = "MyPrefix";
            target.Name = "MyName";
            target.setSelectedCandidateBdtLibrary("bdtlib1");

            target.CreateBdt();
            var testsuccess = false;
            foreach (IBdt bdt in cctsRepository.GetBdtLibraryByPath((Path) "test model"/"blib1"/"bdtlib1").Bdts)
            {
                if(bdt.Name.Equals("MyPrefixMyName"))
                {
                    testsuccess = true;
                }
            }
            Assert.IsTrue(testsuccess);
        }
    }
}