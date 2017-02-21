using System.Threading;
using System.Windows;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.Wizards.dev.ui;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.ui
{
    [TestFixture]
    public class SubSettingWizardTest
    {
        [Test]
        [Ignore]
        public void ShouldOpenAndPopulateForm()
        {
            var t = new Thread(() => new Application().Run(new SubSettingWizard(CctsRepositoryFactory.CreateCctsRepository(new TemporaryFileBasedRepository(TestUtils.PathToTestResource(@"simple_model.eap"))))));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}