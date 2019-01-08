using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Extensions;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Is = Rhino.Mocks.Constraints.Is;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain")]
    public class SugestaoPedidoModelTest
    {
        [Test]
        public void Recalcular_SugestaoPedidoModelComDados_SugestaoPedidoModelNaoNulo()
        {
            var model = new SugestaoPedidoModel()
            {
                blCDConvertido = true,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlPesoLiquido = 10,
                qtVendorPackage = 10
            };

            var r = SugestaoPedidoModel.Recalcular(model);

            Assert.IsNotNull(r);
        }

        #region HabilitaAlteracaoEstoque

        //// Regras usadas na tela de pesquisa de sugestao pedido para habilitar o campo de edicao do vlEstoque
        [Test]
        public void HabilitaAlteracaoEstoque_OrigemSgpAlcadaPermite_True()
        {
            var target = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blAlterarInformacaoEstoque = true
            };

            Assert.IsTrue(target.HabilitaAlteracaoEstoque);
        }

        [Test]
        public void HabilitaAlteracaoEstoque_OrigemSgpAlcadaNaoPermite_False()
        {
            var target = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                blAlterarInformacaoEstoque = false
            };

            Assert.IsFalse(target.HabilitaAlteracaoEstoque);
        }

        [Test]
        public void HabilitaAlteracaoEstoque_OrigemInforemAlcadaPermite_True()
        {
            var target = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Inforem,
                blAlterarInformacaoEstoque = true
            };

            Assert.IsTrue(target.HabilitaAlteracaoEstoque);
        }

        [Test]
        public void HabilitaAlteracaoEstoque_OrigemInforemAlcadaNaoPermite_True()
        {
            var target = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Inforem,
                blAlterarInformacaoEstoque = false
            };

            Assert.IsTrue(target.HabilitaAlteracaoEstoque);
        }


        [Test]
        public void HabilitaAlteracaoEstoque_OrigemGrsAlcadaPermite_True()
        {
            var target = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Grs,
                blAlterarInformacaoEstoque = true
            };

            Assert.IsTrue(target.HabilitaAlteracaoEstoque);
        }

        [Test]
        public void HabilitaAlteracaoEstoque_OrigemGrsAlcadaNaoPermite_True()
        {
            var target = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Grs,
                blAlterarInformacaoEstoque = false
            };

            Assert.IsTrue(target.HabilitaAlteracaoEstoque);
        }

        [Test]
        public void HabilitaAlteracaoEstoque_OrigemManualAlcadaPermite_False()
        {
            var target = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Manual,
                blAlterarInformacaoEstoque = true
            };

            Assert.IsFalse(target.HabilitaAlteracaoEstoque);
        }

        [Test]
        public void HabilitaAlteracaoEstoque_OrigemManualAlcadaNaoPermite_False()
        {
            var target = new SugestaoPedidoModel
            {
                cdOrigemCalculo = TipoOrigemCalculo.Manual,
                blAlterarInformacaoEstoque = false
            };

            Assert.IsFalse(target.HabilitaAlteracaoEstoque);
        }

        [Test]
        public void Recalcular_SugestaoPedidoModel_QtdPackCompra()
        {
            var sugestaoPedido = new SugestaoPedidoModel
            {
                qtVendorPackage = 1
            };

            var actual = SugestaoPedidoModel.Recalcular(sugestaoPedido);
            Assert.AreEqual(0, actual.qtdPackCompra);

            sugestaoPedido.vlEstoque = 1;
            actual = SugestaoPedidoModel.Recalcular(sugestaoPedido);
            Assert.AreEqual(0, actual.qtdPackCompra);
            Assert.AreEqual(1, actual.vlPipeline);

            sugestaoPedido.vlForecast = 2;
            sugestaoPedido.vlModulo = 1;
            actual = SugestaoPedidoModel.Recalcular(sugestaoPedido);
            Assert.AreEqual(1, actual.qtdPackCompra);
            Assert.AreEqual(1, actual.vlPipeline);

            sugestaoPedido.vlEstoqueSegurancaQtd = 3;
            actual = SugestaoPedidoModel.Recalcular(sugestaoPedido);
            Assert.AreEqual(4, actual.qtdPackCompra);
            Assert.AreEqual(1, actual.vlPipeline);

            sugestaoPedido.qtVendorPackage = 4;
            actual = SugestaoPedidoModel.Recalcular(sugestaoPedido);
            Assert.AreEqual(1, actual.qtdPackCompra);
            Assert.AreEqual(1, actual.vlPipeline);

            sugestaoPedido.vlTotalPedidosAberto = 5;
            actual = SugestaoPedidoModel.Recalcular(sugestaoPedido);
            Assert.AreEqual(1, actual.qtdPackCompra);
            Assert.AreEqual(1, actual.vlPipeline);

            sugestaoPedido.vlFatorConversao = 6;
            actual = SugestaoPedidoModel.Recalcular(sugestaoPedido);
            Assert.AreEqual(0, actual.qtdPackCompra);
            Assert.AreEqual(121, actual.vlPipeline);

            sugestaoPedido.vlSugestaoPedido = 7;
            actual = SugestaoPedidoModel.Recalcular(sugestaoPedido);
            Assert.AreEqual(0, actual.qtdPackCompra);
            Assert.AreEqual(121, actual.vlPipeline); 
        }
        #endregion
        [Test]
        public void CalcularLimitesDisponiveis_CDConvertidoTpCaixaV_Limites()
        {
            var target = new SugestaoPedidoModel
            {
                vlModulo = 0.1m,
                blCDConvertido = true,
                vlLimiteInferior = 10,
                vlLimiteSuperior = 100,
                TpCaixaFornecedor = "V"
            };

            var actual = target.CalcularLimitesDisponiveis();
            Assert.AreEqual(10m, actual.StartValue.Value);
            Assert.AreEqual(100m, actual.EndValue.Value);
        }

        [Test]
        public void CalcularLimitesDisponiveis_CDConvertidoTpCaixaVvlPesoLiquido1_Limites()
        {
            var target = new SugestaoPedidoModel
            {
                vlModulo = 0.1m,
                blCDConvertido = true,
                vlPesoLiquido = 1,
                vlLimiteInferior = 10,
                vlLimiteSuperior = 100,
                TpCaixaFornecedor = "V"
            };

            var actual = target.CalcularLimitesDisponiveis();
            Assert.AreEqual(10m, actual.StartValue.Value);
            Assert.AreEqual(100m, actual.EndValue.Value);
        }

        [Test]
        public void CalcularLimitesDisponiveis_vlModuloInferior1_Limites()
        {
            var target = new SugestaoPedidoModel
            {
                vlModulo = 0.1m,
                vlLimiteInferior = 10,
                vlLimiteSuperior = 100
            };

            var actual = target.CalcularLimitesDisponiveis();
            Assert.AreEqual(10m, actual.StartValue.Value);
            Assert.AreEqual(100m, actual.EndValue.Value);
        }

        [Test]
        public void CalcularLimitesDisponiveis_vlModuloIqual1_Limites()
        {
            var target = new SugestaoPedidoModel
            {
                vlModulo = 1m,
                vlLimiteInferior = 10,
                vlLimiteSuperior = 100
            };

            var actual = target.CalcularLimitesDisponiveis();
            Assert.AreEqual(10m, actual.StartValue.Value);
            Assert.AreEqual(100m, actual.EndValue.Value);
        }

        [Test]
        public void CalcularLimitesDisponiveis_vlModuloMaiorQue1_Limites()
        {
            var target = new SugestaoPedidoModel
            {
                qtVendorPackage = 1,
                vlModulo = 10m,
                vlLimiteInferior = 15,
                vlLimiteSuperior = 45
            };

            var actual = target.CalcularLimitesDisponiveis();
            Assert.AreEqual(20m, actual.StartValue.Value);
            Assert.AreEqual(40m, actual.EndValue.Value);
        }

        [Test]
        public void CalcularLimitesDisponiveis_vlModuloMaiorQueLimiteInferior_Limites()
        {
            var target = new SugestaoPedidoModel
            {
                qtVendorPackage = 1,
                vlModulo = 30m,
                vlLimiteInferior = 10,
                vlLimiteSuperior = 100
            };

            var actual = target.CalcularLimitesDisponiveis();
            Assert.AreEqual(30m, actual.StartValue.Value);
            Assert.AreEqual(90m, actual.EndValue.Value);
        }

        [Test]
        public void CalcularLimitesDisponiveis_LimiteInferiorZero_Limites()
        {
            // qtdPackCompra = 1, alçada = 200%
            var target = new SugestaoPedidoModel
            {
                qtdPackCompra = 1,
                qtVendorPackage = 1,
                vlModulo = 1m,
                vlLimiteInferior = 0,
                vlLimiteSuperior = 3m
            };

            var actual = target.CalcularLimitesDisponiveis();
            Assert.AreEqual(0m, actual.StartValue.Value);
            Assert.AreEqual(3m, actual.EndValue.Value);
        }

        [Test]
        public void CalcularLimitesDisponiveis_LimiteInferiorZeroEModuloDecimal_Limites()
        {
            // qtdPackCompra = 3, alçada = 200%
            var target = new SugestaoPedidoModel
            {
                qtdPackCompra = 3,
                qtVendorPackage = 1,
                vlModulo = 1.8m,         // arredonda pra 2 - #4866
                vlLimiteInferior = 0m,
                vlLimiteSuperior = 9m
            };

            var actual = target.CalcularLimitesDisponiveis();
            Assert.AreEqual(0m, actual.StartValue.Value);
            Assert.AreEqual(8m, actual.EndValue.Value);
        }

        [Test]
        public void CalcularLimitesDisponiveis_ModuloDecimal_Limites()
        {
            // qtdPackCompra = 3, alçada = 50%
            var target = new SugestaoPedidoModel
            {
                qtdPackCompra = 3,
                qtVendorPackage = 1,
                vlModulo = 1.8m,         // arredonda pra 2 - #4866
                vlLimiteInferior = 1.5m,
                vlLimiteSuperior = 4.5m
            };

            var actual = target.CalcularLimitesDisponiveis();
            Assert.AreEqual(2m, actual.StartValue.Value);
            Assert.AreEqual(4m, actual.EndValue.Value);
        }

        [Test]
        public void CalcularLimitesDisponiveis_CaixaELimiteInferiorZero_Limites()
        {
            // qtdPackCompra = 1, alçada = 200%
            var target = new SugestaoPedidoModel
            {
                qtdPackCompra = 1,
                qtVendorPackage = 2,
                vlLimiteInferior = 0m,
                vlLimiteSuperior = 3m
            };

            var actual = target.CalcularLimitesDisponiveis();
            Assert.AreEqual(0m, actual.StartValue.Value);
            Assert.AreEqual(3m, actual.EndValue.Value);
        }

        [Test]
        public void PesoLiquidoOuBruto_CaixaCDNaoConvertidoSemReabastecimento_VlModulo()
        {
            SugestaoPedidoModel target = new SugestaoPedidoModel()
            {
                vlTipoReabastecimento = ValorTipoReabastecimento.Nenhum,
                qtVendorPackage = 2,
                vlModulo = 3
            };

            Assert.AreEqual(target.vlModulo, target.PesoLiquidoOuBruto);
        }

        [Test]
        public void PesoLiquidoOuBruto_PesoCDNaoConvertidoSemReabastecimento_VlModulo()
        {
            SugestaoPedidoModel target = new SugestaoPedidoModel()
            {
                vlTipoReabastecimento = ValorTipoReabastecimento.Nenhum,
                qtVendorPackage = 1,
                vlModulo = 3
            };

            Assert.AreEqual(target.vlModulo, target.PesoLiquidoOuBruto);
        }

        [Test]
        public void PesoLiquidoOuBruto_VariavelCDConvertidoSemReabastecimento_VlPesoLiquido()
        {
            SugestaoPedidoModel target = new SugestaoPedidoModel()
            {
                vlTipoReabastecimento = ValorTipoReabastecimento.Nenhum,
                qtVendorPackage = 2,
                blCDConvertido = true,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 3,
                vlPesoLiquido = 5
            };

            Assert.AreEqual(target.vlPesoLiquido, target.PesoLiquidoOuBruto);
        }

        [Test]
        public void PesoLiquidoOuBruto_VariavelCDNaoConvertidoReabastecimentoDsd_VlPesoLiquido()
        {
            SugestaoPedidoModel target = new SugestaoPedidoModel()
            {
                vlTipoReabastecimento = ValorTipoReabastecimento.Dsd7,
                qtVendorPackage = 2,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 3,
                vlPesoLiquido = 5
            };

            Assert.AreEqual(target.vlPesoLiquido, target.PesoLiquidoOuBruto);
        }

        [Test]
        public void PesoLiquidoOuBruto_BrutoCDNaoConvertidoSemReabastecimento_VlModulo()
        {
            SugestaoPedidoModel target = new SugestaoPedidoModel()
            {
                vlTipoReabastecimento = ValorTipoReabastecimento.Nenhum,
                qtVendorPackage = 1,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 3,
                vlPesoLiquido = 5
            };

            Assert.AreEqual(target.vlModulo, target.PesoLiquidoOuBruto);
        }

        [Test]
        public void UnidadeCompra_CaixaCDNaoConvertidoSemReabastecimento_Fixo()
        {
            SugestaoPedidoModel target = new SugestaoPedidoModel()
            {
                vlTipoReabastecimento = ValorTipoReabastecimento.Nenhum,
                qtVendorPackage = 2,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 3,
                vlPesoLiquido = 5
            };

            Assert.AreEqual(TipoCaixaFornecedor.Caixa, target.UnidadeCompra);
        }

        [Test]
        public void UnidadeCompra_PesoCDNaoConvertidoSemReabastecimento_Variavel()
        {
            SugestaoPedidoModel target = new SugestaoPedidoModel()
            {
                vlTipoReabastecimento = ValorTipoReabastecimento.Nenhum,
                qtVendorPackage = 1,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.Caixa,
                vlModulo = 3,
                vlPesoLiquido = 5
            };

            Assert.AreEqual(TipoCaixaFornecedor.KgOuUnidade, target.UnidadeCompra);
        }

        [Test]
        public void UnidadeCompra_CaixaCDConvertidoSemReabastecimento_Fixo()
        {
            SugestaoPedidoModel target = new SugestaoPedidoModel()
            {
                vlTipoReabastecimento = ValorTipoReabastecimento.Nenhum,
                qtVendorPackage = 2,
                blCDConvertido = true,
                TpCaixaFornecedor = TipoCaixaFornecedor.Caixa,
                vlModulo = 3,
                vlPesoLiquido = 5
            };

            Assert.AreEqual(TipoCaixaFornecedor.Caixa, target.UnidadeCompra);
        }

        [Test]
        public void UnidadeCompra_PesoCDConvertidoSemReabastecimento_Variavel()
        {
            SugestaoPedidoModel target = new SugestaoPedidoModel()
            {
                vlTipoReabastecimento = ValorTipoReabastecimento.Nenhum,
                qtVendorPackage = 2,
                blCDConvertido = true,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 3,
                vlPesoLiquido = 5
            };

            Assert.AreEqual(TipoCaixaFornecedor.KgOuUnidade, target.UnidadeCompra);
        }

        [Test]
        public void UnidadeCompra_CaixaCDNaoConvertidoReabastecimentoDsd_Fixo()
        {
            SugestaoPedidoModel target = new SugestaoPedidoModel()
            {
                vlTipoReabastecimento = ValorTipoReabastecimento.Dsd7,
                qtVendorPackage = 2,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.Caixa,
                vlModulo = 3,
                vlPesoLiquido = 5
            };

            Assert.AreEqual(TipoCaixaFornecedor.Caixa, target.UnidadeCompra);
        }

        [Test]
        public void UnidadeCompra_PesoCDNaoConvertidoReabastecimentoDsd_Variavel()
        {
            SugestaoPedidoModel target = new SugestaoPedidoModel()
            {
                vlTipoReabastecimento = ValorTipoReabastecimento.Dsd7,
                qtVendorPackage = 2,
                blCDConvertido = false,
                TpCaixaFornecedor = TipoCaixaFornecedor.KgOuUnidade,
                vlModulo = 3,
                vlPesoLiquido = 5
            };

            Assert.AreEqual(TipoCaixaFornecedor.KgOuUnidade, target.UnidadeCompra);
        }
    }
}
