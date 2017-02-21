// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Collections.Generic;
using CctsRepository;
using CctsRepository.BdtLibrary;
using CctsRepository.BieLibrary;
using CctsRepository.CcLibrary;
using CctsRepository.CdtLibrary;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.Wizards.dev.cache;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.cache
{
    [TestFixture]
    public class CcCacheTest
    {
        [Test]
        public void ShouldGetAndCacheCdtLibraries()
        {
            // Setup
            var cdtLibraryMock = new Mock<ICdtLibrary>();

            var cctsRepositoryMock = new Mock<ICctsRepository>();
            var expectedCdtLibraries = new [] {cdtLibraryMock.Object};
            cctsRepositoryMock.Setup(r => r.GetAllLibraries()).Returns(expectedCdtLibraries);
            
            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<ICdtLibrary> cdtLibraries = ccCache.GetCdtLibraries();
            ccCache.GetCdtLibraries();
            
            // Assertion and Verification
            Assert.That(cdtLibraries, Is.EquivalentTo(expectedCdtLibraries));
            cctsRepositoryMock.Verify(r => r.GetAllLibraries(), Times.Exactly(1));
        }

        [Test]
        public void ShouldGetAndCacheCcLibraries()
        {
            // Setup
            var ccLibraryMock = new Mock<ICcLibrary>();
            var cctsRepositoryMock = new Mock<ICctsRepository>();
            var expectedCcLibraries = new[] { ccLibraryMock.Object };
            cctsRepositoryMock.Setup(r => r.GetAllLibraries()).Returns(expectedCcLibraries);

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<ICcLibrary> ccLibraries = ccCache.GetCcLibraries();
            ccCache.GetCcLibraries();

            // Assertion and Verification
            Assert.That(ccLibraries, Is.EquivalentTo(expectedCcLibraries));
            cctsRepositoryMock.Verify(r => r.GetAllLibraries(), Times.Exactly(1));
        }

        [Test]
        public void ShouldGetAndCacheBdtLibraries()
        {
            // Setup
            var bdtLibraryMock = new Mock<IBdtLibrary>();
            var cctsRepositoryMock = new Mock<ICctsRepository>();
            var expectedBdtLibraries = new[] { bdtLibraryMock.Object };
            cctsRepositoryMock.Setup(r => r.GetAllLibraries()).Returns(expectedBdtLibraries);

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<IBdtLibrary> bdtLibraries = ccCache.GetBdtLibraries();
            ccCache.GetBdtLibraries();

            // Assertion and Verification
            Assert.That(bdtLibraries, Is.EquivalentTo(expectedBdtLibraries));
            cctsRepositoryMock.Verify(r => r.GetAllLibraries(), Times.Exactly(1));
        }

        [Test]
        public void ShouldGetAndCacheBieLibraries()
        {
            // Setup
            var bieLibraryMock = new Mock<IBieLibrary>();
            var cctsRepositoryMock = new Mock<ICctsRepository>();
            var expectedBieLibraries = new[] { bieLibraryMock.Object };
            cctsRepositoryMock.Setup(r => r.GetAllLibraries()).Returns(expectedBieLibraries);

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<IBieLibrary> bieLibraries = ccCache.GetBieLibraries();
            ccCache.GetBieLibraries();

            // Assertion and Verification
            Assert.That(bieLibraries, Is.EquivalentTo(expectedBieLibraries));
            cctsRepositoryMock.Verify(r => r.GetAllLibraries(), Times.Exactly(1));
        }

        [Test]
        public void ShouldGetAndCacheAllCdtsFromCdtLibrary()
        {
            // Setup
            var cdtMock = new Mock<ICdt>();
            var cdtLibraryMock = new Mock<ICdtLibrary>();
            ICdt[] expectedCdts = new[] {cdtMock.Object, cdtMock.Object, cdtMock.Object};
            cdtLibraryMock.SetupGet(l => l.Name).Returns("cdtlib1");
            cdtLibraryMock.SetupGet(l => l.Cdts).Returns(expectedCdts);

            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetAllLibraries()).Returns(new[] { cdtLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<ICdt> cdts = ccCache.GetCdtsFromCdtLibrary("cdtlib1");
            ccCache.GetCdtsFromCdtLibrary("cdtlib1");

            // Assertion and Verification
            Assert.That(cdts, Is.EquivalentTo(expectedCdts));            
            cdtLibraryMock.VerifyGet(l => l.Cdts, Times.Exactly(1));            
        }

        [Test]
        public void ShouldGetAndCacheAllCcsFromCcLibrary()
        {
            // Setup
            var accMock = new Mock<IAcc>();
            var ccLibraryMock = new Mock<ICcLibrary>();
            IAcc[] expectedAccs = new[] { accMock.Object, accMock.Object};
            ccLibraryMock.SetupGet(l => l.Name).Returns("cclib1");
            ccLibraryMock.SetupGet(l => l.Accs).Returns(expectedAccs);
            
            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetAllLibraries()).Returns(new[] { ccLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<IAcc> accs = ccCache.GetCcsFromCcLibrary("cclib1");
            ccCache.GetCcsFromCcLibrary("cclib1");

            // Assertion and Verification
            Assert.That(accs, Is.EquivalentTo(expectedAccs));
            ccLibraryMock.VerifyGet(l => l.Accs, Times.Exactly(1));   
        }

        [Test]
        public void ShouldGetAndCacheParticularCdtFromCdtLibrary()
        {
            // Setup
            var cdtMockText = new Mock<ICdt>();
            cdtMockText.SetupGet(c => c.Name).Returns("Text");
            var cdtMockDate = new Mock<ICdt>();
            cdtMockDate.SetupGet(c => c.Name).Returns("Date");

            var cdtLibraryMock = new Mock<ICdtLibrary>();
            ICdt[] expectedCdts = new[] { cdtMockDate.Object, cdtMockText.Object };
            cdtLibraryMock.SetupGet(l => l.Name).Returns("cdtlib1");
            cdtLibraryMock.SetupGet(l => l.Cdts).Returns(expectedCdts);

            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetAllLibraries()).Returns(new[] { cdtLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            ICdt cdt = ccCache.GetCdtFromCdtLibrary("cdtlib1", "Text");
            ccCache.GetCdtFromCdtLibrary("cdtlib1", "Text");
                
            // Assertion and Verification
            Assert.That(cdt, Is.SameAs(cdtMockText.Object));
            cdtLibraryMock.VerifyGet(l => l.Cdts, Times.Exactly(1));   
        }

        [Test]
        public void ShouldGetAndCacheParticularCcFromCcLibrary()
        {
            // Setup
            var accMockPerson = new Mock<IAcc>();
            accMockPerson.SetupGet(a => a.Name).Returns("Person");
            var accMockAddress = new Mock<IAcc>();
            accMockAddress.SetupGet(a => a.Name).Returns("Address");

            var ccLibraryMock = new Mock<ICcLibrary>();
            IAcc[] expectedAccs = new[] { accMockAddress.Object, accMockPerson.Object };
            ccLibraryMock.SetupGet(l => l.Name).Returns("cclib1");
            ccLibraryMock.SetupGet(l => l.Accs).Returns(expectedAccs);

            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetAllLibraries()).Returns(new[] { ccLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            IAcc acc = ccCache.GetCcFromCcLibrary("cclib1", "Address");
            ccCache.GetCcFromCcLibrary("cclib1", "Address");

            // Assertion and Verification
            Assert.That(acc, Is.SameAs(accMockAddress.Object));
            ccLibraryMock.VerifyGet(l => l.Accs, Times.Exactly(1));  
        }

        [Test]
        public void ShouldGetAndCacheBdtLibraryByName()
        {
            // Setup
            var bdtLibraryMock = new Mock<IBdtLibrary>();
            bdtLibraryMock.SetupGet(l => l.Name).Returns("bdtlib1");
            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetAllLibraries()).Returns(new[] { bdtLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            IBdtLibrary bdtLibrary = ccCache.GetBdtLibraryByName("bdtlib1");
            ccCache.GetBdtLibraryByName("bdtlib1");

            // Assertion and Verification
            Assert.That(bdtLibrary, Is.SameAs(bdtLibraryMock.Object));
            cctsRepositoryMock.Verify(r => r.GetAllLibraries(), Times.Exactly(1));
        }

        [Test]
        public void ShouldGetAndCacheAllBdtsFromBdtLibrary()
        {
            // Setup
            var bdtMock = new Mock<IBdt>();
            var bdtLibraryMock = new Mock<IBdtLibrary>();
            IBdt[] expectedBdts = new[] { bdtMock.Object, bdtMock.Object, bdtMock.Object };
            bdtLibraryMock.SetupGet(l => l.Name).Returns("bdtlib1");
            bdtLibraryMock.SetupGet(l => l.Bdts).Returns(expectedBdts);

            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetAllLibraries()).Returns(new[] { bdtLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<IBdt> bdts = ccCache.GetBdtsFromBdtLibrary("bdtlib1");
            ccCache.GetBdtsFromBdtLibrary("bdtlib1");            

            // Assertion and Verification
            Assert.That(bdts, Is.EquivalentTo(expectedBdts));
            bdtLibraryMock.VerifyGet(l => l.Bdts, Times.Exactly(1));       
        }


        [Test]
        public void ShouldGetAndCacheBieLibraryByName()
        {
            // Setup
            var bieLibraryMock = new Mock<IBieLibrary>();
            bieLibraryMock.SetupGet(l => l.Name).Returns("bielib1");
            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetAllLibraries()).Returns(new[] { bieLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            IBieLibrary bieLibrary = ccCache.GetBieLibraryByName("bielib1");
            ccCache.GetBieLibraryByName("bielib1");

            // Assertion and Verification
            Assert.That(bieLibrary, Is.SameAs(bieLibraryMock.Object));
            cctsRepositoryMock.Verify(r => r.GetAllLibraries(), Times.Exactly(1));
        }       

        [Test]
        public void ShouldGetAndCacheAllBiesFromBieLibrary()
        {
            // Setup
            var bieMock = new Mock<IAbie>();
            var bieLibraryMock = new Mock<IBieLibrary>();
            IAbie[] expectedBdts = new[] { bieMock.Object, bieMock.Object, bieMock.Object };
            bieLibraryMock.SetupGet(l => l.Name).Returns("bielib1");
            bieLibraryMock.SetupGet(l => l.Abies).Returns(expectedBdts);

            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetAllLibraries()).Returns(new[] { bieLibraryMock.Object });

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<IAbie> bies = ccCache.GetBiesFromBieLibrary("bielib1");
            ccCache.GetBiesFromBieLibrary("bielib1");

            // Assertion and Verification
            Assert.That(bies, Is.EquivalentTo(expectedBdts));
            bieLibraryMock.VerifyGet(l => l.Abies, Times.Exactly(1));              
        }

        [Test]
        public void ShouldRefreshCacheToForceReloadOfBdtAndBieLibrary()
        {
            // Setup
            var bdtLibraryMock = new Mock<IBdtLibrary>();
            var bieLibraryMock = new Mock<IBieLibrary>();
            var expectedBieLibraries = new[] { bieLibraryMock.Object };
            var expectedBdtLibraries = new[] { bdtLibraryMock.Object };
            var allLibraries = new List<object>();
            allLibraries.AddRange(expectedBieLibraries);
            allLibraries.AddRange(expectedBdtLibraries);
            var cctsRepositoryMock = new Mock<ICctsRepository>();
            cctsRepositoryMock.Setup(r => r.GetAllLibraries()).Returns(allLibraries);

            // Events
            CcCache ccCache = CcCache.GetInstance(cctsRepositoryMock.Object);
            List<IBieLibrary> bieLibraries = ccCache.GetBieLibraries();
            List<IBdtLibrary> bdtLibraries = ccCache.GetBdtLibraries();
            
            ccCache.GetBieLibraries();
            ccCache.GetBdtLibraries();

            ccCache.Refresh();

            ccCache.GetBieLibraries();
            ccCache.GetBdtLibraries();
            ccCache.GetBieLibraries();
            ccCache.GetBdtLibraries();

            // Assertion and Verification
            Assert.That(bdtLibraries, Is.EquivalentTo(expectedBdtLibraries));
            Assert.That(bieLibraries, Is.EquivalentTo(expectedBieLibraries));
            cctsRepositoryMock.Verify(r => r.GetAllLibraries(), Times.Exactly(2));
        }
    }
}