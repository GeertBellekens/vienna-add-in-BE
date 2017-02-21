using System.Runtime.InteropServices;
using NUnit.Framework;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.ea
{
    [TestFixture]
    public class EaCollectionTests
    {
        [Test]
        [Category(TestCategories.FileBased)]
        [ExpectedException(typeof (COMException))]
        public void ShouldThrowExceptionIfElementNotFound()
        {
            var eaRepository = new TemporaryFileBasedRepository();
            eaRepository.Models.GetByName("foobar");
        }
    }
}