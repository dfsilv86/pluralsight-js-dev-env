using NUnit.Framework;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests
{
    [TestFixture]
    [Category("Framework")]
    public class EntityBaseTest
    {
        [Test]
        public void EqualsOperator_NullEqualsNull_True()
        {
            EntityBase one = null;

            Assert.IsTrue(one == null);
        }

        [Test]
        public void Equals_DiffKeys_False()
        {
            var target1 = new Usuario { Id = 1 };
            var target2 = new Usuario { Id = 2 };

            Assert.IsFalse(target1 == target2);
        }
    }
}
