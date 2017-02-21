using System.Collections.Generic;
using EA;
using NUnit.Framework;
using VIENNAAddInUnitTests.TestRepository;
using Constraint=NUnit.Framework.Constraints.Constraint;

namespace VIENNAAddInUnitTests.SynchTaggedValuesTest
{
    public class SynchTaggedValuesTestsBase
    {
        private TemporaryFileBasedRepository temporaryFileBasedRepository;

        [SetUp]
        public void Setup()
        {
//            temporaryFileBasedRepository = new TemporaryFileBasedRepository(TestUtils.RelativePathToTestResource(GetType(), "resources\\RepositoryWithoutTaggedValues.eap"));
        }

        [TearDown]
        public void CleanUp()
        {
            if (temporaryFileBasedRepository != null)
            {
                temporaryFileBasedRepository.Dispose();
            }
        }

        protected Repository CreateRepositoryWithoutTaggedValues()
        {

            return (Repository) temporaryFileBasedRepository ?? new SynchTaggedValuesTestRepository();
        }

        protected static Constraint HasTaggedValues(IEnumerable<string> taggedValues)
        {
            return new HasTaggedValuesConstraint(taggedValues);
        }
    }
}