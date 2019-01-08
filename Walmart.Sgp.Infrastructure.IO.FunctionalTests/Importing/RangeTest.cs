using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.IO.Importing;

namespace Walmart.Sgp.Infrastructure.IO.FunctionalTests.Importing
{
    [TestFixture]
    public class RangeTest
    {
        [Test]
        [Category("ImportacaoInventario")]
        public void Range_Constructor_Range()
        {
            Range target = new Range(10, 11);

            Assert.AreEqual(10, target.Offset);
            Assert.AreEqual(11, target.Length);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void Range_Equals_True()
        {
            Range target = new Range(10, 11);

            Range other = new Range(10, 11);

            Assert.IsTrue(target.Equals(other));
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void Range_Equals_False()
        {
            Range target = new Range(10, 11);

            Assert.IsFalse(target.Equals(null));

            Range other = new Range(10, 12);

            Assert.IsFalse(target.Equals(other));
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void Range_GetHashcode_Hashcode()
        {
            Range target = new Range(10, 11);

            Assert.AreEqual(target.GetHashCode(), 10.GetHashCode() ^ 11.GetHashCode());
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void Range_OperatorEquality_True()
        {
            Range target = new Range(10, 11);

            Range other = new Range(10, 11);

            Assert.IsTrue(target == other);
        }

        [Test]
        [Category("ImportacaoInventario")]
        public void Range_OperatorInequality_True()
        {
            Range target = new Range(10, 11);

            Range other = new Range(10, 12);

            Assert.IsTrue(target != other);
        }
    }
}
