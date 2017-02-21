// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System;
using System.Collections.Generic;
using System.Net;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class VersionHandlerTest
    {        
        private FileBasedVersionHandler versionHandler;

        private Mock<IVersionsFile> AWebClientMediator()
        {
            Mock<IVersionsFile> webClientMediator = new Mock<IVersionsFile>();
            string versionsString = "ccl08b|1|bla\nccl08b|2|blabla\nccl09a|1|bla";
            webClientMediator.Setup(mediator => mediator.GetContent()).Returns(versionsString);
            return webClientMediator;
        }

        private Mock<IVersionsFile> AWebClientMediatorWhichThrowsAnException()
        {
            Mock<IVersionsFile> webClientMediator = new Mock<IVersionsFile>();
            webClientMediator.Setup(mediator => mediator.GetContent()).Throws(new WebException("Der Remoteserver hat einen Fehler zurückgegeben: (404) Nicht gefunden."));
            return webClientMediator;
        }

        private Mock<IVersionsFile> AWebClientMediatorWhichReturnsAnEmptyString()
        {
            Mock<IVersionsFile> webClientMediator = new Mock<IVersionsFile>();
            string versionsString = "";
            webClientMediator.Setup(mediator => mediator.GetContent()).Returns(versionsString);
            return webClientMediator;
        }

        private Mock<IVersionsFile> AWebClientMediatorWhichReturnsAnInvalidVersionsString()
        {
            Mock<IVersionsFile> webClientMediator = new Mock<IVersionsFile>();
            string versionsString = "ccl08b|1|bla\nccl08b|2\nccl09a|1|bla";
            webClientMediator.Setup(mediator => mediator.GetContent()).Returns(versionsString);
            return webClientMediator;
        }

        [Test]
        public void ShouldDownloadOnlineVersionFileAndExtractAvailableVersions()
        {
            Mock<IVersionsFile> webClientMediator = AWebClientMediator();
            versionHandler = new FileBasedVersionHandler(webClientMediator.Object);
            versionHandler.RetrieveAvailableVersions();

            webClientMediator.VerifyAll();

            List<VersionDescriptor> expectedVersions = new List<VersionDescriptor>
                                                {
                                                    new VersionDescriptor {Major = "ccl08b", Minor = "1", Comment = "bla"},
                                                    new VersionDescriptor {Major = "ccl09a", Minor = "1", Comment = "bla"},
                                                    new VersionDescriptor {Major = "ccl08b", Minor = "2", Comment = "blabla"},                                                    
                                                };

            Assert.That(versionHandler.AvailableVersions, Is.EquivalentTo(expectedVersions));
        }

        [Test]
        public void ShouldProvideAvailableMajorVersions()
        {
            Mock<IVersionsFile> webClientMediator = AWebClientMediator();
            List<string> expectedMajorVersions = new List<string> { "ccl08b", "ccl09a" };

            versionHandler = new FileBasedVersionHandler(webClientMediator.Object);

            versionHandler.RetrieveAvailableVersions();            

            Assert.That(versionHandler.GetMajorVersions(), Is.EqualTo(expectedMajorVersions));
        }

        [Test]
        public void ShouldProvideAvailableMinorVersionsForMajorVersion()
        {
            Mock<IVersionsFile> webClientMediator = AWebClientMediator();
            List<string> expectedMinorVersions = new List<string> { "1", "2" };            
            
            versionHandler = new FileBasedVersionHandler(webClientMediator.Object);
                        
            versionHandler.RetrieveAvailableVersions();

            Assert.That(versionHandler.GetMinorVersions("ccl08b"), Is.EqualTo(expectedMinorVersions));
        }

        [Test]
        public void ShouldProvideCommentForMajorVersionAndMinorVersion()
        {
            Mock<IVersionsFile> webClientMediator = AWebClientMediator();
            versionHandler = new FileBasedVersionHandler(webClientMediator.Object);
            versionHandler.RetrieveAvailableVersions();

            Assert.That(versionHandler.GetComment("ccl09a", "1"), Is.EqualTo("bla"));
        }        

        [Test]
        [ExpectedException(typeof (WebException))]
        public void ShouldThrowExceptionIfVersionsFileIsUnavailable()
        {
            Mock<IVersionsFile> webClientMediator = AWebClientMediatorWhichThrowsAnException();
            versionHandler = new FileBasedVersionHandler(webClientMediator.Object);
            versionHandler.RetrieveAvailableVersions();                                  
        }

        [Test]        
        public void AvailableVersionsListShouldBeEmptyIfVersionsFileIsUnavailable()
        {
            Mock<IVersionsFile> webClientMediator = AWebClientMediatorWhichThrowsAnException();
            versionHandler = new FileBasedVersionHandler(webClientMediator.Object);

            try
            {
                versionHandler.RetrieveAvailableVersions();
                Assert.Fail("Expected exception \"System.Net.WebException\" not thrown.");
            }
            catch (WebException)
            {
                Assert.That(versionHandler.AvailableVersions, Is.Empty);
            }            
        }

        [Test]
        public void AvailableVersionsListShouldBeEmptyIfVersionsFileIsEmpty()
        {
            Mock<IVersionsFile> webClientMediator = AWebClientMediatorWhichReturnsAnEmptyString();
            versionHandler = new FileBasedVersionHandler(webClientMediator.Object);
            versionHandler.RetrieveAvailableVersions();

            Assert.That(versionHandler.AvailableVersions, Is.Empty);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionIfVersionsStringIsInvalid()
        {
            Mock<IVersionsFile> webClientMediator = AWebClientMediatorWhichReturnsAnInvalidVersionsString();
            versionHandler = new FileBasedVersionHandler(webClientMediator.Object);
            versionHandler.RetrieveAvailableVersions();
        }
    }

    [TestFixture]
    public class VersionHandlerIntegrationTest
    {
        [Test]
        [Category(TestCategories.FileBased)]
        public void ShouldDownloadOnlineVersionFileIntegrationTest()
        {
            FileBasedVersionHandler versionHandler = new FileBasedVersionHandler(new RemoteVersionsFile("http://www.umm-dev.org/xmi/testresources/ccl_versions.txt"));
            versionHandler.RetrieveAvailableVersions();

            List<VersionDescriptor> expectedVersions = new List<VersionDescriptor>
                                                {
                                                    new VersionDescriptor {Major = "simplified", Minor = "1", Comment = "Simplified Core Component Library v08B published by UN/CEFACT."},
                                                    new VersionDescriptor {Major = "simplified", Minor = "2", Comment = "Simplified Core Component Library v08B published by UN/CEFACT which has been adapted by Research Studios Austria to fix mistakes in the Core Component model. "},
                                                    new VersionDescriptor {Major = "complex", Minor = "1", Comment = "Complex Core Component Library v09A published by UN/CEFACT."},                                                    
                                                };

            Assert.That(versionHandler.AvailableVersions, Is.EquivalentTo(expectedVersions));
        }

        [Test]
        [Category(TestCategories.FileBased)]
        public void ShouldLoadLocalVersionFileAndExtractAvailableVersions()
        {
            FileBasedVersionHandler versionHandler = new FileBasedVersionHandler(new LocalVersionsFile("C:\\_work\\vienna-add-in\\VIENNAAddInUnitTests\\testresources\\VersionHandlerTest\\ccl_versions.txt"));
            versionHandler.RetrieveAvailableVersions();

            List<VersionDescriptor> expectedVersions = new List<VersionDescriptor>
                                                {
                                                    new VersionDescriptor {Major = "simplified", Minor = "1", Comment = "Simplified Core Component Library v08B published by UN/CEFACT.\r"},
                                                    new VersionDescriptor {Major = "simplified", Minor = "2", Comment = "Simplified Core Component Library v08B published by UN/CEFACT which has been adapted by Research Studios Austria to fix mistakes in the Core Component model. \r"},
                                                    new VersionDescriptor {Major = "complex", Minor = "1", Comment = "Complex Core Component Library v09A published by UN/CEFACT."},                                                    
                                                };         

            Assert.That(versionHandler.AvailableVersions, Is.EquivalentTo(expectedVersions));
        }
    }
}