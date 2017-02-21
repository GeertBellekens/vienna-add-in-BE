// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using System.Threading;
using System.Windows;
using NUnit.Framework;
using VIENNAAddIn.menu;
using VIENNAAddIn.upcc3.Wizards.dev.ui;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.ui
{
    [TestFixture]
    public class UpccModelCreatorFormTest
    {
        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateUpccModelCreatorForm()
        {
            AddInContext context = new AddInContext(new EARepositoryModelCreator(), MenuLocation.MainMenu.ToString());

            var t = new Thread(() => new Application().Run(new UpccModelCreator(context)));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}