using System.Collections.Generic;
using NUnit.Framework;
using VIENNAAddIn.Utils;

namespace VIENNAAddInUnitTests.upcc3.export.cctsndr
{
    [TestFixture]
    public class DictionaryExtensionsTest
    {
        [Test]
        public void TestToStringDebug()
        {
            var dictionary = new Dictionary<string, string>();
            Assert.AreEqual("[]", dictionary.ToStringDebug());
            for (int i = 0; i < 3; i++)
            {
                dictionary["key " + i] = "value " + i;
            }
            Assert.AreEqual("[(key 0 => value 0)(key 1 => value 1)(key 2 => value 2)]", dictionary.ToStringDebug());
        }
    }
}