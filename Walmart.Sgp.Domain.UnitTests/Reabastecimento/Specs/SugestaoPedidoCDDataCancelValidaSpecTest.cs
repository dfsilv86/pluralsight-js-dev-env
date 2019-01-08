using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Reabastecimento")]
    public class SugestaoPedidoCDDataCancelValidaSpecTest
    {
        [Test]
        public void IsSatisfiedBy_SugestaoPedidoComCancelamentoAnteriorADataAtual_NotSatisfied()
        {
            var hj = DateTime.Now.Date;

            var target = new SugestaoPedidoCDDataCancelValidaSpec();
            var actual = target.IsSatisfiedBy(new SugestaoPedidoCD()
            {
                dtEnvioPedido = hj,
                dtCancelamentoPedido = hj.AddDays(-5)
            });

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_SugestaoPedidoComCancelamentoPosteriorADataAtual_Satisfied()
        {
            var hj = DateTime.Now.Date;

            var target = new SugestaoPedidoCDDataCancelValidaSpec();
            var actual = target.IsSatisfiedBy(new SugestaoPedidoCD()
            {
                dtEnvioPedido = hj,
                dtCancelamentoPedido = hj.AddDays(5)
            });

            Assert.IsTrue(actual.Satisfied);
        }
    }
}
