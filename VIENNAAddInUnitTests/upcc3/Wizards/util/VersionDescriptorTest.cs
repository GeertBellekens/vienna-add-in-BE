using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.Wizards.util;

namespace VIENNAAddInUnitTests.upcc3.Wizards.util
{
    [TestFixture]
    public class VersionDescriptorTest
    {
        [Test]
        public void ValidateGeneratedDownloadDirectory()
        {
            VersionDescriptor versionDescriptor = new VersionDescriptor {Major = "ccl08b", Minor = "1", Comment = "bla"};

            string downloadUri = versionDescriptor.ResourceDirectory;
            Assert.That(downloadUri, Is.EqualTo("ccl08b_1"));            
        }
    }
}