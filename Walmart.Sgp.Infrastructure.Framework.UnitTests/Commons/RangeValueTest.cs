using NUnit.Framework;
using System;
using Walmart.Sgp.Infrastructure.Framework.Commons;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Commons
{
    [TestFixture]
    [Category("Framework")]
    public class RangeValueTest
    {
        [Test]
        public void IsEmpty_True()
        {
            var target = new RangeValue<DateTime>();
            Assert.IsTrue(target.IsEmpty);
        }

        [Test]
        public void IsIncomplete_True()
        {
            var target = new RangeValue<DateTime>();
            target.StartValue = DateTime.UtcNow;
            Assert.IsTrue(target.IsIncomplete);
        }

        [Test]
        public void StartValue_GreaterThanEndValue_Exception()
        {
            var target = new RangeValue<DateTime>();
            target.EndValue = DateTime.UtcNow;

            Assert.That(() => target.StartValue = target.EndValue.Value.AddDays(1),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void EndValue_LesserThanStartValue_Exception()
        {
            var target = new RangeValue<DateTime>();
            target.StartValue = DateTime.UtcNow;

            Assert.That(() => target.EndValue = target.StartValue.Value.AddDays(-1),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void OperatorEquals_RangeValue_True()
        {
            var date = DateTime.UtcNow.Date;
            var target = new RangeValue<DateTime>();
            target.StartValue = date;
            target.EndValue = date.AddDays(1);

            var other = new RangeValue<DateTime>();
            other.StartValue = date;
            other.EndValue = date.AddDays(1);

            Assert.IsTrue(target == other);
        }

        [Test]
        public void OperatorEquals_NullableRangeValue_True()
        {
            var target = new RangeValue<DateTime>?();
            var other = new RangeValue<DateTime>?();

            Assert.IsTrue(target == other);
        }

        [Test]
        public void OperatorEquals_Nullable_NonNulableRangeValue_False()
        {
            var target = new RangeValue<DateTime>();
            RangeValue<DateTime>? other = null;

            Assert.IsFalse(target == other);
        }

        [Test]
        public void OperatorEquals_RangeValue_False()
        {
            var date = DateTime.UtcNow.Date;
            var target = new RangeValue<DateTime>();
            target.StartValue = date;
            target.EndValue = date.AddDays(1);

            var other = new RangeValue<DateTime>();
            other.StartValue = date;
            other.EndValue = date.AddDays(2);

            Assert.IsTrue(target != other);
        }

        [Test]
        public void Equals_Null_False()
        {
            var target = new RangeValue<DateTime>();

            Assert.IsFalse(target.Equals(null));
        }

        [Test]
        public void Equals_DifferentType_False()
        {
            var target = new RangeValue<DateTime>();

            var other = new RangeValue<int>();

            Assert.IsFalse(target.Equals(other));
        }

        [Test]
        public void GetHashCode_XorTwoDateTimesHashCode()
        {
            var target = new RangeValue<DateTime>();
            DateTime? startValue = DateTime.UtcNow.Date;
            DateTime? endValue = startValue.Value.AddDays(10);

            target.StartValue = startValue;
            target.EndValue = endValue;

            var expected = startValue.GetHashCode() ^ endValue.GetHashCode();

            var actual = target.GetHashCode();

            Assert.AreEqual(expected, actual);
        }
    }
}
