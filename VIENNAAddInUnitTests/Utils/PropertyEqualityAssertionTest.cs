using NUnit.Framework;

namespace VIENNAAddInUnitTests.Utils
{
    [TestFixture]
    public class PropertyEqualityAssertionTest
    {
        [Test]
        public void EqualObjectsShouldPass()
        {
            var testObject1 = new TestClass {IntProperty = 1, StringProperty = "abc"};
            var testObject2 = new TestClass {IntProperty = 1, StringProperty = "abc"};
            var assertProperties = new PropertyEqualityAssertion<ITestClass>(testObject1, testObject2, "a message");
            assertProperties.AreEqual(o => o.IntProperty);
            assertProperties.AreEqual(o => o.StringProperty);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void UnEqualObjectsShouldNotPass1()
        {
            var testObject1 = new TestClass {IntProperty = 1, StringProperty = "abc"};
            var testObject2 = new TestClass {IntProperty = 2, StringProperty = "abc"};
            var assertProperties = new PropertyEqualityAssertion<ITestClass>(testObject1, testObject2,
                                                                             "ints are not equal");
            assertProperties.AreEqual(o => o.IntProperty);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void UnEqualObjectsShouldNotPass2()
        {
            var testObject1 = new TestClass {IntProperty = 1, StringProperty = "abc"};
            var testObject2 = new TestClass {IntProperty = 1, StringProperty = "abcd"};
            var assertProperties = new PropertyEqualityAssertion<ITestClass>(testObject1, testObject2,
                                                                             "strings are not equal");
            assertProperties.AreEqual(o => o.StringProperty);
        }

        public interface ITestClass
        {
            int IntProperty { get; set; }
            string StringProperty { get; set; }
        }

        internal class TestClass : ITestClass
        {
            public int IntProperty { get; set; }
            public string StringProperty { get; set; }
        }
    }
}