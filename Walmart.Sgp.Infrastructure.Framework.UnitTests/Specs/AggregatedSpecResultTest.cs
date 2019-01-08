using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Specs
{
    [TestFixture]
    [Category("Framework")]
    public class AggregatedSpecResultTest
    {
        [Test]
        public void AggregatedSpecResult_Specs_Satisfied()
        {
            var specA = new SpecResult(true);
            var specB = new SpecResult(true);

            var target = new AggregatedSpecResult(new SpecResult[] { specA, specB });

            Assert.IsTrue(target.Satisfied);
            Assert.AreEqual(2, target.Results.Count());
            Assert.IsTrue(target.Results.Any(r => r == specA));
            Assert.IsTrue(target.Results.Any(r => r == specB));
        }

        [Test]
        public void AggregatedSpecResult_Specs_NotSatisfied()
        {
            var specA = new SpecResult(true);
            var specB = new SpecResult(false);

            var target = new AggregatedSpecResult(new SpecResult[] { specA, specB });

            Assert.IsFalse(target.Satisfied);
            Assert.AreEqual(2, target.Results.Count());
            Assert.IsTrue(target.Results.Any(r => r == specA));
            Assert.IsTrue(target.Results.Any(r => r == specB));
        }

        [Test]
        public void AggregatedSpecResult_SpecsWithReasons_JoinedReasons()
        {
            var target = new AggregatedSpecResult(new SpecResult[] { new SpecResult(true, "Reason 1"), new SpecResult(false, "Reason 2") });

            Assert.AreEqual("Reason 1, Reason 2", target.Reason);
        }

        [Test]
        public void AggregatedSpecResult_AggregatedSpecs_NotSatisfied()
        {
            var specA = new SpecResult(true, "Reason 1");
            var specB = new SpecResult(false, "Reason 2");
            var specC = new SpecResult(true, "Reason 3");
            var specD = new SpecResult(true, "Reason 4");

            var aggrA = new AggregatedSpecResult(new SpecResult[] { specA, specB });
            var aggrB = new AggregatedSpecResult(new SpecResult[] { specC, specD });

            var target = new AggregatedSpecResult(new AggregatedSpecResult[] { aggrA, aggrB });

            Assert.IsFalse(target.Satisfied);
            Assert.AreEqual(4, target.Results.Count());
            Assert.IsTrue(target.Results.Any(r => r == specA));
            Assert.IsTrue(target.Results.Any(r => r == specB));
            Assert.IsTrue(target.Results.Any(r => r == specC));
            Assert.IsTrue(target.Results.Any(r => r == specD));

            Assert.AreEqual("Reason 1, Reason 2, Reason 3, Reason 4", target.Reason);
        }

        [Test]
        public void AggregatedSpecResult_AggregatedSpecs_Satisfied()
        {
            var specA = new SpecResult(true, "Reason 1");
            var specB = new SpecResult(true, "Reason 2");
            var specC = new SpecResult(true, "Reason 3");
            var specD = new SpecResult(true, "Reason 4");

            var aggrA = new AggregatedSpecResult(new SpecResult[] { specA, specB });
            var aggrB = new AggregatedSpecResult(new SpecResult[] { specC, specD });

            var target = new AggregatedSpecResult(new AggregatedSpecResult[] { aggrA, aggrB });

            Assert.IsTrue(target.Satisfied);
            Assert.AreEqual(4, target.Results.Count());
        }
    }
}
