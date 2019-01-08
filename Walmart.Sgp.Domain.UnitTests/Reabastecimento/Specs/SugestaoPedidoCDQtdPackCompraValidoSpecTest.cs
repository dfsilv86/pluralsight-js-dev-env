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
    public class SugestaoPedidoCDQtdPackCompraValidoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_SugestaoPedidoCDComPackCompraNegativo_NotSatisfied()
        {
            var hj = DateTime.Now.Date;

            var target = new SugestaoPedidoCDQtdPackCompraValidoSpec();
            var actual = target.IsSatisfiedBy(new SugestaoPedidoCD()
            {
                qtdPackCompra = -1
            });

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_SugestaoPedidoCDComPackCompraPositivo_Satisfied()
        {
            var hj = DateTime.Now.Date;

            var target = new SugestaoPedidoCDQtdPackCompraValidoSpec();
            var actual = target.IsSatisfiedBy(new SugestaoPedidoCD()
            {
                qtdPackCompra = 10
            });

            Assert.IsTrue(actual.Satisfied);
        }
    }
}
