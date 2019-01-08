using NUnit.Framework;
using Walmart.Sgp.Domain.Inventarios.Specs;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class ItemNaoDeveSerVinculadoDeEntradaSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItemVinculadoEntrada_NotSatisfied()
        {
            var itemDetalhe = new ItemDetalhe
            {
                TpVinculado = TipoVinculado.Entrada
            };

            var target = new ItemNaoDeveSerVinculadoDeEntradaSpec();
            var actual = target.IsSatisfiedBy(itemDetalhe);
            Assert.IsFalse(actual);
        }

        [Test]
        public void IsSatisfiedBy_ItemVinculadoSaida_Satisfied()
        {
            var itemDetalhe = new ItemDetalhe
            {
                TpVinculado = TipoVinculado.Saida
            };

            var target = new ItemNaoDeveSerVinculadoDeEntradaSpec();
            var actual = target.IsSatisfiedBy(itemDetalhe);
            Assert.IsTrue(actual);
        }
    }
}