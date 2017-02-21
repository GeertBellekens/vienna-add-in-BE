using System.Threading;
using System.Windows;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.Wizards.dev.ui;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.ui
{
    [TestFixture]
    public class XsdImporterFormTest
    {
        [Test]
        [Ignore]
        public void ShouldOpenAndPopulateForm()
        {
            var t = new Thread(() => new Application().Run(new XsdImporterForm(CctsRepositoryFactory.CreateCctsRepository(new EARepositoryModelCreator()))));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}