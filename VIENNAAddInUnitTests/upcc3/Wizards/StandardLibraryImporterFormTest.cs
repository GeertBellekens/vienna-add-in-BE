using NUnit.Framework;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards
{
    [TestFixture]
    public class StandardLibraryImporterFormTest
    {
        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateStandardLibraryImporterForm()
        {
            AddInContext context = new AddInContext(new EARepositoryLibraryImporter(), MenuLocation.MainMenu.ToString());

            StandardLibraryImporterForm.ShowForm(context);    
        }        
    }
}