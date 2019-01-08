using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Rhino.Mocks;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Specs
{
    [TestFixture]
    [Category("Framework")]
    public class SpecServiceTest
    {
        #region Tests
        [Test]
        public void ThrowIfAnySpecificationIsNotSatisfiedBy_WithSecondNotSatisfied_ThrowsExceptionWithSecondSpecification()
        {
            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                SpecService.Assert<object>(new object(), CreateSpecifications());
            });
        }

        [Test]
        public void Assert_OneNotSatisfiedBy_Throw()
        {
            var spec1 = MockRepository.GenerateMock<ISpec<int>>();
            spec1.Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            spec1.Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(true));
            spec1.Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(true));

            var spec2 = MockRepository.GenerateMock<ISpec<int>>();
            spec2.Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            spec2.Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(false, "2 é inválido"));

            var spec3 = MockRepository.GenerateMock<ISpec<int>>();
            spec3.Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            spec3.Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(true));
            spec3.Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(false, "3 é inválido"));

            Assert.Catch<NotSatisfiedSpecException>(() =>
            {
                SpecService.Assert(new int[] { 1, 2, 3 }, spec1, spec2, spec3);
            }, "2 é inválido");

            Assert.Catch<NotSatisfiedSpecException>(() =>
            {
                SpecService.Assert(new int[] { 1, 2, 3 }, spec1, spec3);
            }, "3 é inválido");
        }

        [Test]
        public void Assert_AllSatisfiedBy_NotThrow()
        {
            var spec1 = MockRepository.GenerateMock<ISpec<int>>();
            spec1.Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            spec1.Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(true));
            spec1.Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(true));

            var spec2 = MockRepository.GenerateMock<ISpec<int>>();
            spec2.Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            spec2.Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(true));
            spec2.Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(true));

            var spec3 = MockRepository.GenerateMock<ISpec<int>>();
            spec3.Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            spec3.Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(true));
            spec3.Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(true));

            SpecService.Assert(new int[] { 1, 2, 3 }, spec1, spec2, spec3);
        }

        [Test]
        public void AssertAll_TwoNotSatisfiedBy_Throw()
        {
            var spec1 = MockRepository.GenerateMock<ISpec<int>>();
            spec1.Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            spec1.Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(true));
            spec1.Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(true));

            var spec3 = MockRepository.GenerateMock<ISpec<int>>();
            spec3.Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            spec3.Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(false, "2 é inválido"));
            spec3.Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(false, "3 é inválido"));
         
            Assert.Catch<NotSatisfiedSpecException>(() =>
            {
                SpecService.AssertAll(new int[] { 1, 2, 3 }, spec1, spec3);
            }, "2 é inválido\n3 é inválido");
        }

        [Test]
        public void GetNotSatisfiedBy_OneNotSatisfiedBy_Result()
        {
            var spec1 = MockRepository.GenerateMock<ISpec<int>>();
            spec1.Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            spec1.Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(true));
            spec1.Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(true));

            var spec3 = MockRepository.GenerateMock<ISpec<int>>();
            spec3.Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            spec3.Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(false, "2 é inválido"));
            spec3.Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(false, "3 é inválido"));

            var actual = SpecService.GetNotSatisfiedBy(new int[] { 1, 2, 3 }, spec1, spec3);
            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual("2 é inválido", actual.First().Reason);
            Assert.AreEqual("3 é inválido", actual.Last().Reason);
        }

        [Test]
        public void IsSatisfiedByAll_Specs_AggregatedSpecResult()
        {
            var spec1 = MockRepository.GenerateMock<ISpec<int>>();
            spec1.Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            spec1.Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(true));
            spec1.Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(true));

            var spec2 = MockRepository.GenerateMock<ISpec<int>>();
            spec2.Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            spec2.Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(false, "Reason 2/2"));
            spec2.Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(false, "Reason 2/3"));

            var spec3 = MockRepository.GenerateMock<ISpec<int>>();
            spec3.Expect(s => s.IsSatisfiedBy(1)).Return(new SpecResult(true));
            spec3.Expect(s => s.IsSatisfiedBy(2)).Return(new SpecResult(true));
            spec3.Expect(s => s.IsSatisfiedBy(3)).Return(new SpecResult(false, "Reason 3/3"));

            AggregatedSpecResult target = SpecService.IsSatisfiedByAll(1, spec1, spec2, spec3);

            Assert.IsTrue(target.Satisfied);
            Assert.AreEqual(string.Empty, target.Reason);

            target = SpecService.IsSatisfiedByAll(2, spec1, spec2, spec3);

            Assert.IsFalse(target.Satisfied);
            Assert.AreEqual("Reason 2/2", target.Reason);

            target = SpecService.IsSatisfiedByAll(3, spec1, spec2, spec3);

            Assert.IsFalse(target.Satisfied);
            Assert.AreEqual("Reason 2/3, Reason 3/3", target.Reason);
        }

        [Test]
        public void IsSatisfiedByAll_AggregatedAndSpecs_AggregatedSpecResult()
        {
            int[] tmp = new int[] { 1, 2, 3 };

            var spec1 = MockRepository.GenerateMock<ISpec<IEnumerable<int>>>();
            spec1.Expect(s => s.IsSatisfiedBy(tmp)).Return(new AggregatedSpecResult(new SpecResult[] { new SpecResult(true), new SpecResult(false, "Reason 1"), new SpecResult(true) }));

            var spec2 = MockRepository.GenerateMock<ISpec<IEnumerable<int>>>();
            spec2.Expect(s => s.IsSatisfiedBy(tmp)).Return(new SpecResult(false, "Reason 2"));

            AggregatedSpecResult target = SpecService.IsSatisfiedByAll(tmp, spec1, spec2);

            Assert.IsFalse(target.Satisfied);
            Assert.AreEqual(4, target.Results.Count());
            Assert.AreEqual("Reason 1, Reason 2", target.Reason);
        }
        #endregion

        #region Helpers

        private ISpec<object>[] CreateSpecifications()
        {
            var spec1 = MockRepository.GenerateMock<ISpec<object>>();
            spec1.Expect(s => s.IsSatisfiedBy(null)).IgnoreArguments().Return(new SpecResult(true));

            var spec2 = MockRepository.GenerateMock<ISpec<object>>();
            spec2.Expect(s => s.IsSatisfiedBy(null)).IgnoreArguments().Return(new SpecResult(false, "Reason 2"));

            var specs = new List<ISpec<object>>();
            specs.Add(spec1);
            specs.Add(spec2);

            return specs.ToArray();
        }

        #endregion
    }
}
