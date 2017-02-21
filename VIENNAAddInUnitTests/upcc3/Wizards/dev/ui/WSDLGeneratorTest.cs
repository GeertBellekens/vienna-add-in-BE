using System.Threading;
using System.Windows;
using EA;
using NUnit.Framework;
using VIENNAAddIn.upcc3.Wizards.dev.ui;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.ui
{
    [TestFixture]
    public class WSDLGeneratorTest
    {
        [Test]
        [Ignore]
        public void ShouldOpenAndPopulateForm()
        {
            var repository = new Repository();
            repository.OpenFile(TestUtils.PathToTestResource(@"umm2-example-orderfromquote-20080911.eap"));
            var t = new Thread(() => new Application().Run(new WSDLGenerator(repository)));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}