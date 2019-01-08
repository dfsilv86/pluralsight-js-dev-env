using NUnit.Framework;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Specs
{    
    [TestFixture]
    [Category("Framework")]
    public class EntityMustBeNewSpecTest
    {
        [Test]
        public void IsSatisfiedBy_NewEntity_Satisfied()
        {
            var entity = new FakeEntity();
            Assert.IsTrue(entity.IsNew);

            var target = new EntityMustBeNewSpec();
            var actual = target.IsSatisfiedBy(entity);
            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ExistingEntity_NotSatisfied()
        {
            var entity = new FakeEntity
            {
                Id = 1
            };

            Assert.IsFalse(entity.IsNew);

            var target = new EntityMustBeNewSpec();
            var actual = target.IsSatisfiedBy(entity);
            Assert.IsFalse(actual.Satisfied);
        }

        private class FakeEntity : EntityBase
        {
        }
    }

}
