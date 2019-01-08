using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Domain
{
    [TestFixture]
    [Category("Framework")]
    public class DomainServiceBaseTest
    {
        [Test]
        public void Assert_Target_SpecsVerified()
        {
            var target = new DomainServiceStub(0);
            var specs = new ISpec<int>[]
            {
                MockRepository.GenerateMock<ISpec<int>>(),
                MockRepository.GenerateMock<ISpec<int>>()
            };

            specs[0].Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            specs[0].Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(true));
            specs[0].Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(true));


            specs[1].Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            specs[1].Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(true));
            specs[1].Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(false));

            // Um target.
            target.TestAssert(1, specs);
            target.TestAssert(2, specs);

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                target.TestAssert(3, specs);
            });

            // Vários targets.
            target.TestAssert(new int[] { 1, 2 }, specs);

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                target.TestAssert(new int[] { 1, 2, 3 }, specs);
            });
        }

        [Test]
        public void AssertAll_Target_SpecsVerified()
        {
            var target = new DomainServiceStub(0);
            var specs = new ISpec<int>[]
            {
                MockRepository.GenerateMock<ISpec<int>>(),
                MockRepository.GenerateMock<ISpec<int>>()
            };

            specs[0].Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            specs[0].Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(true));
            specs[0].Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(true));


            specs[1].Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            specs[1].Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(false));
            specs[1].Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(true));

            target.TestAssertAll(new int[] { 1, 2 }, new ISpec<int>[] { specs[0] });

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                target.TestAssert(new int[] { 1, 2, 3 }, specs);
            });
        }
    }
}
