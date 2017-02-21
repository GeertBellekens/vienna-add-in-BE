using System.Threading;
using System.Windows;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.Wizards.dev.ui;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.ui
{
    [TestFixture]
    public class TransformerWizardTest
    {
        [Test]
        [Ignore]
        public void ShouldOpenAndPopulateForm()
        {
            var t = new Thread(() => new Application().Run(new TransformerWizard(CctsRepositoryFactory.CreateCctsRepository(new TemporaryFileBasedRepository(TestUtils.PathToTestResource(@"TranformerTest\\repository_containing_ubl_and_ebinterface_invoice.eap"))))));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}