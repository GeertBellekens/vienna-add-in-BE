using System.Threading;
using System.Windows;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.Wizards.dev.ui;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.ui
{
    [TestFixture]
    public class XsltGeneratorTest
    {
        [Test]
        [Ignore]
        public void ShouldOpenAndPopulateForm()
        {
            var repository = new Repository();
            repository.OpenFile(TestUtils.PathToTestResource(@"full_initial.eap"));
            var t = new Thread(() => new Application().Run(new XsltGeneratorForm(CctsRepositoryFactory.CreateCctsRepository(repository))));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}