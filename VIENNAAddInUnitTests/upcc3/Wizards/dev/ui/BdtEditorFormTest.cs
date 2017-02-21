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
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.Wizards.dev.ui;
using VIENNAAddInUnitTests.upcc3.Wizards.dev.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.ui
{
    [TestFixture]
    public class BdtEditorFormTest
    {
        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateBdtModelerForm()
        {
            var t = new Thread(() => new Application().Run(new BdtEditor(CctsRepositoryFactory.CreateCctsRepository(new EARepositoryBdtEditor()))));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}