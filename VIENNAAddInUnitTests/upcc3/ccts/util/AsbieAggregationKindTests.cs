using System;
using CctsRepository;
using CctsRepository.BieLibrary;
using NUnit.Framework;

namespace VIENNAAddInUnitTests.upcc3.ccts.util
{
    [TestFixture]
    public class AsbieAggregationKindTests
    {
        [Test]
        public void MatchesIntValuesUsedByEA()
        {
            Assert.AreEqual(AggregationKind.Shared, Enum.ToObject(typeof (AggregationKind), 1));
            Assert.AreEqual(AggregationKind.Composite, Enum.ToObject(typeof (AggregationKind), 2));

            Assert.AreEqual(false, Enum.IsDefined(typeof (AggregationKind), -1));
            Assert.AreEqual(false, Enum.IsDefined(typeof (AggregationKind), 0));
            Assert.AreEqual(false, Enum.IsDefined(typeof (AggregationKind), 3));

            Assert.AreEqual(1, (int) AggregationKind.Shared);
            Assert.AreEqual(2, (int) AggregationKind.Composite);
        }
    }
}