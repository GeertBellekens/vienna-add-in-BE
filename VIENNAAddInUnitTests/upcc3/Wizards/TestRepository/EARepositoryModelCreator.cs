// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using VIENNAAddInUnitTests.TestRepository;
using VIENNAAddIn;

namespace VIENNAAddInUnitTests.upcc3.Wizards.TestRepository
{
    public class EARepositoryModelCreator : EARepository
    {
        public EARepositoryModelCreator()
        {
            this.AddModel("Test Model 1", m => { });
        }
    }
}