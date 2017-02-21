namespace VIENNAAddInUnitTests.TestRepository
{
    public class IDFactory
    {
        private int nextID = 1;

        public int NextID
        {
            get
            {
                return nextID++;
            }
        }
    }
}