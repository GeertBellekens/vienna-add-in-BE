using System;
using NUnit.Framework;
using VIENNAAddIn.upcc3.ea;

namespace VIENNAAddInUnitTests.upcc3.ccts.util
{
    [TestFixture]
    public class AggregationKindTest
    {
        [Test]
        public void TestEnumFromInt()
        {
            Assert.AreEqual(EaAggregationKind.None, (EaAggregationKind) Enum.ToObject(typeof (EaAggregationKind), 0));
            Assert.AreEqual(EaAggregationKind.Shared, Enum.ToObject(typeof (EaAggregationKind), 1));
            Assert.AreEqual(EaAggregationKind.Composite, Enum.ToObject(typeof (EaAggregationKind), 2));
            Assert.AreEqual(false, Enum.IsDefined(typeof (EaAggregationKind), -1));
            Assert.AreEqual(false, Enum.IsDefined(typeof (EaAggregationKind), 3));
        }

        [Test]
        public void TestEnumToInt()
        {
            Assert.AreEqual(0, (int) EaAggregationKind.None);
            Assert.AreEqual(1, (int) EaAggregationKind.Shared);
            Assert.AreEqual(2, (int) EaAggregationKind.Composite);
        }
    }
}