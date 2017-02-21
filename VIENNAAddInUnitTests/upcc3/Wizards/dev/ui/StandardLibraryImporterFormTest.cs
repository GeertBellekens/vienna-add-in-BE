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
using VIENNAAddIn.upcc3.Wizards.dev.ui;
using VIENNAAddInUnitTests.upcc3.Wizards.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.ui
{
    [TestFixture]
    public class StandardLibraryImporterFormTest
    {
        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateStandardLibraryImporterForm()
        {
            var t = new Thread(() => new Application().Run(new StandardLibraryImporter(new EARepositoryLibraryImporter())));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}