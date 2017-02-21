using VIENNAAddIn;

namespace VIENNAAddInUnitTests.TestRepository
{
    /// <summary>
    /// An empty repository. It is actually not entirely empty, because a newly created model always contains exactly one model object (named "Model"), which cannot be removed.
    /// </summary>
    internal class EmptyEARepository : EARepository
    {
        public EmptyEARepository()
        {
            this.AddModel("Model", m => { });
        }
    }
}