using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Infrastructure.Framework.UnitTests.Specs
{
    [TestFixture]
    public class ItemDeveSerMultisourcingPossivelSpecTest
    {
        [Test]
        public void ItemIsMultisourcing()
        {
            var gw = MockRepository.GenerateMock<IItemDetalheGateway>();
            gw.Expect(g => g.ObterQuantidadeItensEntrada(1, 1, 1)).Return(2);

            var target = new ItemDeveSerMultisourcingPossivelSpec(gw.ObterQuantidadeItensEntrada);
            var actual = target.IsSatisfiedBy(new Tuple<long, long, long>(1, 1, 1));

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void ItemIsNotMultisourcing()
        {
            var gw = MockRepository.GenerateMock<IItemDetalheGateway>();
            gw.Expect(g => g.ObterQuantidadeItensEntrada(1, 1, 1)).Return(1);

            var target = new ItemDeveSerMultisourcingPossivelSpec(gw.ObterQuantidadeItensEntrada);
            var actual = target.IsSatisfiedBy(new Tuple<long, long, long>(1, 1, 1));

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.NoMultisourcingItem, actual.Reason);
        }
    }
}
