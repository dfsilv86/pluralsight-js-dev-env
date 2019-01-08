using NUnit.Framework;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Item.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Item.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class RelacionamentoItemReceituarioPesoPodeSerSalvoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_PesoAcabadoMaiorQuePesoBruto_False()
        {
            var target = new RelacionamentoItemReceituarioPesoPodeSerSalvoSpec();
            
            var actual = target.IsSatisfiedBy(new RelacionamentoItemPrincipal
            {
                TipoRelacionamento = TipoRelacionamento.Receituario,
                QtProdutoAcabado = 200,
                QtProdutoBruto = 100
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.FinishedProductWeightMustBeLessOrEqualToWeightGrossProduct, actual.Reason);
        }
    }
}
