using System;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Specs
{
    [TestFixture]
    [Category("Framework")]
    public class AllMustBeEqualSpecTests
    {
        [Test]
        public void IsSatisfiedBy_AllPropertiesEqual_Satisfied()
        {
            var target = new AllMustBeEqualSpec();
            var actual = target.IsSatisfiedBy(new { NewPassword = "teste1", ConfirmPassword = "teste1" });
            Assert.IsTrue(actual);
        }

        [Test]
        public void IsSatisfiedBy_NotAllPropertiesEqual_NotSatisfied()
        {
            var target = new AllMustBeEqualSpec();
            var actual = target.IsSatisfiedBy(new { NewPassword = "teste1", ConfirmPassword = "teste2" });
            Assert.IsFalse(actual);
        }

        [Test]
        public void IsSatisfiedBy_NonCompartableValue_NotSatisfied()
        {
            var target = new AllMustBeEqualSpec();
            var actual = target.IsSatisfiedBy(new { NewPassword = "teste1", ConfirmPassword = new NonComparableText("teste1") });
            Assert.IsFalse(actual);
        }

        [Test]
        public void IsSatisfiedBy_CompartableValue_Satisfied()
        {
            var target = new AllMustBeEqualSpec();
            var actual = target.IsSatisfiedBy(new { NewPassword = "teste1", ConfirmPassword = new ComparableText("teste1") });
            Assert.IsTrue(actual);
        }
    }

    public class NonComparableText
    {
        public NonComparableText(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }

    public class ComparableText : IComparable, IComparable<string>
    {
        public ComparableText(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as string);
        }

        public int CompareTo(string other)
        {
            return ReferenceEquals(Value, other) 
                ? 0 
                : string.Compare(Value, other, StringComparison.Ordinal);
        }
    }
}
