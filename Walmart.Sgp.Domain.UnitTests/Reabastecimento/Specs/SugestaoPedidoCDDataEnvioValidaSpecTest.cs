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
    public class SugestaoPedidoCDDataEnvioValidaSpecTest
    {
        [Test]
        public void IsSatisfiedBy_SugestaoPedidoCDComEnvioPosteriorAoCancelamento_NotSatisfied()
        {
            var hj = DateTime.Now.Date;

            var target = new SugestaoPedidoCDDataEnvioValidaSpec();
            var actual = target.IsSatisfiedBy(new SugestaoPedidoCD()
            {
                dtEnvioPedido = hj,
                dtCancelamentoPedido = hj.AddDays(-5)
            });

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_SugestaoPedidoCDComEnvioAnteriorAoCancelamento_Satisfied()
        {
            var hj = DateTime.Now.Date;

            var target = new SugestaoPedidoCDDataEnvioValidaSpec();
            var actual = target.IsSatisfiedBy(new SugestaoPedidoCD()
            {
                dtEnvioPedido = hj,
                dtCancelamentoPedido = hj.AddDays(5)
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_SugestaoPedidoComEnvioAnterioADataAtual_NotSatisfied(){

            var hj = DateTime.Now.Date;
            var dtEnvio = hj.AddDays(-2).Date;  

            var target = new SugestaoPedidoCDDataEnvioValidaSpec();
            var actual = target.IsSatisfiedBy(new SugestaoPedidoCD()
            {
                dtEnvioPedido = dtEnvio,
                dtCancelamentoPedido = hj.AddDays(5)
            });

            Assert.IsFalse(actual.Satisfied);
        }
    }
}
