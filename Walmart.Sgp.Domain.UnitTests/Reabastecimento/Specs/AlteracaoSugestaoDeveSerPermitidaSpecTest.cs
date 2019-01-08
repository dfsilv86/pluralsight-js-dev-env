using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Specs;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class AlteracaoSugestaoDeveSerPermitidaSpecTest
    {
        [Test]
        public void IsSatisfiedBy_SugestaoPedidoModelComQtdPackCompraDiferenteDoOriginal_NotSatisfied()
        {
            var target = new SugestaoPedidoModel();
            target.blAlterarPercentual = true;
            target.vlEstoque = target.Original_vlEstoque = 1;
            target.qtdPackCompra = 2;
            target.qtdPackCompraOriginal = 1;
            target.cdOrigemCalculo = TipoOrigemCalculo.Sgp;
            target.blCDConvertido = true;
            target.TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade;
            target.vlPesoLiquido = 5;

            var actual = new AlteracaoSugestaoDeveSerPermitidaSpec().IsSatisfiedBy(target);

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_QuantidadeAlteradaEstoqueNaoAlterado_NotSatisfied()
        {
            var target = new SugestaoPedidoModel();
            target.dtPedido = DateTime.Today;
            target.qtdPackCompraAlterado = true;
            target.qtdPackCompra = 100;
            target.Original_qtdPackCompra = 30;
            target.vlEstoque = 0;
            target.Original_vlEstoque = 0;
            target.vlLimiteInferior = 0;
            target.vlLimiteSuperior = 60;
            target.cdOrigemCalculo = TipoOrigemCalculo.Sgp;
            target.blAlterarPercentual = true;
            target.TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade;
            target.blCDConvertido = true;
    
            var actual = new AlteracaoSugestaoDeveSerPermitidaSpec().IsSatisfiedBy(target);

            Assert.IsFalse(actual.Satisfied);
            Assert.IsTrue(actual.Reason.IndexOf("A quantidade solicitada não corresponde a Alçada definida para este perfil") >= 0);
        }

        [Test]
        public void IsSatisfiedBy_QuantidadeNaoAlteradaEstoqueAlterado_NotSatisfied()
        {
            var target = new SugestaoPedidoModel();
            target.dtPedido = DateTime.Today;
            target.qtdPackCompraAlterado = false;
            target.vlEstoque = 40;
            target.Original_vlEstoque = 0;
            target.qtdPackCompraOriginal = 30;
            target.blZerarItem = false;

            var actual = new AlteracaoSugestaoDeveSerPermitidaSpec().IsSatisfiedBy(target);

            Assert.AreEqual(actual.Reason, Texts.CannotZeroQtdPackCompra);
        }

        [Test]
        public void IsSatisfiedBy_QuantidadeAlteradaEstoqueAlteradoAnteriormenteNaoRecalcular_NotSatisfied()
        {
            var target = new SugestaoPedidoModel();
            target.dtPedido = DateTime.Today;
            target.qtdPackCompraAlterado = true;
            target.vlEstoque = 40;
            target.Original_vlEstoque = 0;
            target.qtdPackCompra = 100;
            target.Original_qtdPackCompra = 30;
            target.qtdPackCompraOriginal = 30;
            target.cdOrigemCalculo = TipoOrigemCalculo.Sgp;
            target.blAlterarPercentual = true;
            target.TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade;

            var actual = new AlteracaoSugestaoDeveSerPermitidaSpec().IsSatisfiedBy(target);

            Assert.IsTrue(actual.Reason.IndexOf("A quantidade solicitada não corresponde a Alçada definida para este perfil") >= 0);
        }
    }
}
