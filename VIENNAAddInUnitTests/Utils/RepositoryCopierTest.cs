using EA;
using NUnit.Framework;
using VIENNAAddIn.Utils;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests.Utils
{
    [TestFixture]
    public class RepositoryCopierTest
    {
        private static void CopyAndAssertTo(Repository copy)
        {
            var original = new EARepository1();
            RepositoryCopier.CopyRepository(original, copy);
            EAAssert.RepositoriesAreEqual(original, copy);
        }

        [Test]
        public void TestCopyRepository()
        {
            CopyAndAssertTo(new EmptyEARepository());
        }

        [Test]
        [Category(TestCategories.FileBased)]
        public void TestCopyRepositoryFileBased()
        {
            using (var temporaryFileBasedRepository = new TemporaryFileBasedRepository(new EmptyEARepository()))
            {
                CopyAndAssertTo(temporaryFileBasedRepository);
            }
        }
    }
}