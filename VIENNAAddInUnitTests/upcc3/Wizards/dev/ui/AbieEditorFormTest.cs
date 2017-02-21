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
using CctsRepository;
using CctsRepository.BieLibrary;
using NUnit.Framework;
using VIENNAAddIn.upcc3;
using VIENNAAddIn.upcc3.Wizards.dev.ui;
using VIENNAAddInUnitTests.upcc3.Wizards.dev.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.dev.ui
{
    [TestFixture]
    public class AbieEditorFormTest
    {
        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateAbieEditorForm()
        {
            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(new EARepositoryAbieEditor());

            var t = new Thread(() => new Application().Run(new AbieEditor(cctsRepository)));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }

        [Test]
        [Ignore]
        public void ShouldLaunchAndPopulateAbieEditorFormForParticularAbie()
        {
            ICctsRepository cctsRepository = CctsRepositoryFactory.CreateCctsRepository(new EARepositoryAbieEditor());           
            IAbie abieToBeUpdated = cctsRepository.GetAbieByPath(EARepositoryAbieEditor.PathToBIEPerson());
            //IAbie abieToBeUpdated = cctsRepository.GetAbieByPath(EARepositoryAbieEditor.PathToBIEAddress());
            
            var t = new Thread(() => new Application().Run(new AbieEditor(cctsRepository, abieToBeUpdated.Id)));                        
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
        }
    }
}