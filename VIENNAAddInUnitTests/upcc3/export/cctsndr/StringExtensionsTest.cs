// *******************************************************************************
// This file is part of the VIENNAAddIn project
// 
// Licensed under GNU General Public License V3 http://gplv3.fsf.org/
// 
// For further information on the VIENNAAddIn project please visit 
// http://vienna-add-in.googlecode.com
// *******************************************************************************
using System;
using NUnit.Framework;
using VIENNAAddIn.upcc3.export.cctsndr;
using VIENNAAddInUtils;

namespace VIENNAAddInUnitTests.upcc3.export.cctsndr
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void TestStringEqualityOperators()
        {
            AssertStringsAreEqual("abc", "abc");
            AssertStringsAreEqual("", "");
            AssertStringsAreEqual(null, null);
            AssertStringsAreNotEqual("abc", "abd");
            AssertStringsAreNotEqual("", "b");
            AssertStringsAreNotEqual("a", "");
            AssertStringsAreNotEqual(null, "");
            AssertStringsAreNotEqual(null, "b");
            AssertStringsAreNotEqual("", null);
            AssertStringsAreNotEqual("a", null);
        }

        private static void AssertStringsAreEqual(string s1, string s2)
        {
            Assert.IsTrue(s1 == s2);
        }

        private static void AssertStringsAreNotEqual(string s1, string s2)
        {
            Assert.IsTrue(s1 != s2);
        }

        [Test]
        public void TestStringDefault()
        {
            Assert.AreEqual("abc" + null, "abc");
            Assert.AreEqual("abc" + "", "abc");
            Assert.AreEqual("abc" + String.Empty, "abc");

            Assert.AreEqual(String.Empty.DefaultTo("abc"), "abc");
            Assert.AreEqual("".DefaultTo("abc"), "abc");

            const string nullString = null;
            Assert.AreEqual(nullString.DefaultTo("abc"), "abc");

            Assert.AreEqual("foo".DefaultTo("bar"), "foo");
        }
    }
}