using NUnit.Framework;
using VIENNAAddIn.upcc3;

namespace VIENNAAddInUnitTests.upcc3.ccts.otf.validators
{
    [TestFixture]
    public class PRIMLibraryValidatorTests : ElementLibraryValidatorTests
    {
        public PRIMLibraryValidatorTests() : base(Stereotype.PRIMLibrary, Stereotype.PRIM)
        {
        }
    }
}