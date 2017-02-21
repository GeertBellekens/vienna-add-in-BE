using NUnit.Framework;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards
{
    [TestFixture]
    public class UPCCModelWizardFormTest
    {
        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateUPCCModelWizardForm()
        {
            UpccModelWizardForm.ShowForm(new AddInContext(new EARepositoryModelCreator(), MenuLocation.MainMenu.ToString()));
        }        
    }
}