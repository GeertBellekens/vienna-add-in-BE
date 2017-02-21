// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************

using NUnit.Framework;
using VIENNAAddIn;
using VIENNAAddInUnitTests.TestRepository;

namespace VIENNAAddInUnitTests.upcc3.Wizards.TestRepository
{
    [TestFixture]
    public class EARepositoryResourceHandler : EARepository
    {
        [Test]
        public void Test1()
        {
            this.AddModel("Test Model", m => { });
        }
    }
}