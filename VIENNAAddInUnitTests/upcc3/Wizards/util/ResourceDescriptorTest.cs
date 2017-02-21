// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class ResourceDescriptorTest
    {
        [Test]
        public void ShouldAppendRelativePathForMajorAndMinorVersionForRemoteVersions()
        {
            ResourceDescriptor defaultDescriptor = new ResourceDescriptor();
            ResourceDescriptor customDescriptor = new ResourceDescriptor("http://xmi/", "major", "minor");

            Assert.That(customDescriptor.DownloadUri, Is.EqualTo("http://xmi/" + "major_minor/"));
            Assert.That(customDescriptor.StorageDirectory, Is.EqualTo(defaultDescriptor.StorageDirectory+ "major_minor\\"));
        }

        [Test]
        public void ShouldAppendRelativePathForMajorAndMinorVersionForLocalVersions()
        {
            ResourceDescriptor defaultDescriptor = new ResourceDescriptor();
            ResourceDescriptor customDescriptor = new ResourceDescriptor("X:\\arbitrary\\path\\", "major", "minor");

            Assert.That(customDescriptor.DownloadUri, Is.EqualTo("X:\\arbitrary\\path\\" + "major_minor\\"));
            Assert.That(customDescriptor.StorageDirectory, Is.EqualTo(defaultDescriptor.StorageDirectory+ "major_minor\\"));
        }

    }
}