using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using VIENNAAddIn.upcc3.uml;

namespace VIENNAAddInUnitTests.upcc3.ccts.util
{
    [TestFixture]
    public class MultiPartTaggedValueTests
    {
        [Test]
        public void ShouldHandleEmptyTaggedValues()
        {
            Assert.That(MultiPartTaggedValue.Split(null), Is.EqualTo(new string[0]));
            Assert.That(MultiPartTaggedValue.Split(""), Is.EqualTo(new string[0]));
            Assert.That(MultiPartTaggedValue.Merge(new string[0]), Is.EqualTo(""));
            Assert.That(MultiPartTaggedValue.Merge(null), Is.EqualTo(""));

            Assert.That(MultiPartTaggedValue.Split("value1|value2||value4"), Is.EqualTo(new[] { "value1", "value2", "", "value4" }));
            Assert.That(MultiPartTaggedValue.Merge(new[] { "value1", "value2", null, "value4" }), Is.EqualTo("value1|value2||value4"));
        }

        [Test]
        public void ShouldHandleEmptyValues()
        {
            Assert.That(MultiPartTaggedValue.Split("|abc||"), Is.EqualTo(new[] { "", "abc", "", "" }));
            Assert.That(MultiPartTaggedValue.Merge(new[] { "", "abc", "", "" }), Is.EqualTo("|abc||"));
        }

        [Test]
        public void ShouldHandleASingleValue()
        {
            Assert.That(MultiPartTaggedValue.Split("value1"), Is.EqualTo(new[] { "value1" }));
            Assert.That(MultiPartTaggedValue.Merge(new[] { "value1" }), Is.EqualTo("value1"));
        }

        [Test]
        public void ShouldHandleMultipleValues()
        {
            Assert.That(MultiPartTaggedValue.Split("value1|value2|value3|value4"), Is.EqualTo(new[] { "value1", "value2", "value3", "value4" }));
            Assert.That(MultiPartTaggedValue.Merge(new[] { "value1", "value2", "value3", "value4" }), Is.EqualTo("value1|value2|value3|value4"));
        }

    }
}