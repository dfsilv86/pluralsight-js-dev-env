using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Reabastecimento;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain")]
    public class SugestaoPedidoTest
    {
        [Test]
        public void CalcularValorOriginal_SugestaoPedidoComCDConvertido_10()
        {
            var sp = new SugestaoPedido()
            {
                qtdPackCompra = 0,
                blCDConvertido = true,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlPesoLiquido = 10
            };

            Assert.AreEqual(10, sp.CalcularValorOriginal());
        }

        [Test]
        public void CalcularValorOriginal_SugestaoPedidoSemCDConvertido_2()
        {
            var sp = new SugestaoPedido()
            {
                qtdPackCompra = 0,
                blCDConvertido = false,
                qtVendorPackage = 1,
                vlModulo = 2
            };

            Assert.AreEqual(2, sp.CalcularValorOriginal());
        }

        [Test]
        public void QtdPackCompraToCaixa_SugestaoPedicoComTpCaixaFornecedorCaixa_4()
        {
            var sugestao = new SugestaoPedido();

            sugestao.qtdPackCompra = 4;
            sugestao.vlPesoLiquido = 2;

            sugestao.TpCaixaFornecedor = "F";
            Assert.AreEqual(4, sugestao.QtdPackCompraToCaixa());
        }

        [Test]
        public void QtdPackCompraToCaixa_SugestaoPedicoComTpCaixaFornecedorKgOuUnidade_2()
        {
            var sugestao = new SugestaoPedido();

            sugestao.qtdPackCompra = 4;
            sugestao.vlPesoLiquido = 2;

            sugestao.TpCaixaFornecedor = "V";
            Assert.AreEqual(2, sugestao.QtdPackCompraToCaixa());
        }

        [Test]
        public void QtdPackCompraToCaixa_SugestaoPedicoSemTpCaixaFornecedor_0()
        {
            var sugestao = new SugestaoPedido();

            sugestao.qtdPackCompra = 4;
            sugestao.vlPesoLiquido = 2;

            sugestao.TpCaixaFornecedor = null;
            Assert.AreEqual(0, sugestao.QtdPackCompraToCaixa());
        }

        [Test]
        public void QtdSugestaoRoteiroRAToCaixa_SugestaoPedidoComTpCaixaFornecedorCaixa_4()
        {
            var sugestao = new SugestaoPedido();

            sugestao.qtdSugestaoRoteiroRA = 4;
            sugestao.vlPesoLiquido = 2;

            sugestao.TpCaixaFornecedor = "F";
            Assert.AreEqual(4, sugestao.QtdSugestaoRoteiroRAToCaixa());
        }

        [Test]
        public void QtdSugestaoRoteiroRAToCaixa_SugestaoPedidoComTpCaixaFornecedorKgOuUnidade_2()
        {
            var sugestao = new SugestaoPedido();

            sugestao.qtdSugestaoRoteiroRA = 4;
            sugestao.vlPesoLiquido = 2;

            sugestao.TpCaixaFornecedor = "V";
            Assert.AreEqual(2, sugestao.QtdSugestaoRoteiroRAToCaixa());
        }

        [Test]
        public void QtdSugestaoRoteiroRAToCaixa_SugestaoPedidoSemTpCaixaFornecedor_0()
        {
            var sugestao = new SugestaoPedido();

            sugestao.qtdSugestaoRoteiroRA = 4;
            sugestao.vlPesoLiquido = 2;

            sugestao.TpCaixaFornecedor = null;
            Assert.AreEqual(0, sugestao.QtdSugestaoRoteiroRAToCaixa());
        }

        [Test]
        public void QtdPackCompraToQuilo_SugestaoPedidoComTpCaixaFornecedorCaixa_8()
        {
            var sugestao = new SugestaoPedido();

            sugestao.qtdPackCompra = 4;
            sugestao.qtVendorPackage = 2;

            sugestao.TpCaixaFornecedor = "F";
            Assert.AreEqual(8, sugestao.QtdPackCompraToQuilo());
        }

        [Test]
        public void QtdPackCompraToQuilo_SugestaoPedidoComTpCaixaFornecedorKgOuUnidade_4()
        {
            var sugestao = new SugestaoPedido();

            sugestao.qtdPackCompra = 4;
            sugestao.qtVendorPackage = 2;

            sugestao.TpCaixaFornecedor = "V";
            Assert.AreEqual(4, sugestao.QtdPackCompraToQuilo());
        }

        [Test]
        public void QtdPackCompraToQuilo_SugestaoPedidoSemTpCaixaFornecedor_0()
        {
            var sugestao = new SugestaoPedido();

            sugestao.qtdPackCompra = 4;
            sugestao.qtVendorPackage = 2;

            sugestao.TpCaixaFornecedor = null;
            Assert.AreEqual(0, sugestao.QtdPackCompraToQuilo());
        }

        [Test]
        public void QtdSugestaoRoteiroRAToQuilo_SugestaoPedidoComTpCaixaFornecedorCaixa_8()
        {
            var sugestao = new SugestaoPedido();

            sugestao.qtVendorPackage = 2;
            sugestao.qtdSugestaoRoteiroRA = 4;

            sugestao.TpCaixaFornecedor = "F";
            Assert.AreEqual(8, sugestao.QtdSugestaoRoteiroRAToQuilo());
        }

        [Test]
        public void QtdSugestaoRoteiroRAToQuilo_SugestaoPedidoComTpCaixaFornecedorKgOuUnidade_4()
        {
            var sugestao = new SugestaoPedido();

            sugestao.qtVendorPackage = 2;
            sugestao.qtdSugestaoRoteiroRA = 4;

            sugestao.TpCaixaFornecedor = "V";
            Assert.AreEqual(4, sugestao.QtdSugestaoRoteiroRAToQuilo());
        }

        [Test]
        public void QtdSugestaoRoteiroRAToQuilo_SugestaoPedidoSemTpCaixaFornecedor_0()
        {
            var sugestao = new SugestaoPedido();

            sugestao.qtVendorPackage = 2;
            sugestao.qtdSugestaoRoteiroRA = 4;

            sugestao.TpCaixaFornecedor = null;
            Assert.AreEqual(0, sugestao.QtdSugestaoRoteiroRAToQuilo());
        }
    }
}
