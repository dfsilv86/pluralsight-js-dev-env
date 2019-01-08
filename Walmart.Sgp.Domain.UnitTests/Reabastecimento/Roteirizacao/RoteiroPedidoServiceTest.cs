using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Domain.Reabastecimento.Roteirizacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento.Roteirizacao
{
    [TestFixture]
    [Category("Domain")]
    class RoteiroPedidoServiceTest
    {
        [Test]
        public void ObterPedidosRoteirizados_Filtros_RetornaPedidoRoteirizadoConsolidado()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroPedidoGatewayMock = MockRepository.GenerateMock<IRoteiroPedidoGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroPedidoService = new RoteiroPedidoService(roteiroPedidoGatewayMock, roteiroGatewayMock, auditServiceMock);

            var dt = DateTime.Now;
            var paging = new Infrastructure.Framework.Domain.Paging() { Limit = 50, Offset = 0, OrderBy = null };

            roteiroPedidoGatewayMock
                .Expect(x => x.ObterPedidosRoteirizados(dt, 1, 1, null, null, paging))
                .Return(new List<PedidoRoteirizadoConsolidado>() { new PedidoRoteirizadoConsolidado() { idRoteiro = 1 } });

            var actual = roteiroPedidoService.ObterPedidosRoteirizados(dt, 1, 1, null, null, paging);

            Assert.AreEqual(1, actual.Count());
        }

        [Test]
        public void AutorizarPedidos_IdRoteiroDtPedido_NaoDeveFalharAutorizacaoPedidos1()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroPedidoGatewayMock = MockRepository.GenerateMock<IRoteiroPedidoGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroPedidoService = new RoteiroPedidoService(roteiroPedidoGatewayMock, roteiroGatewayMock, auditServiceMock);

            var dt = DateTime.Now;
            
            try
            {
                roteiroPedidoService.AutorizarPedidos(1, dt);
            }
            catch
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void AutorizarPedidos_IdRoteiroDtPedido_NaoDeveFalharAutorizacaoPedidos2()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroPedidoGatewayMock = MockRepository.GenerateMock<IRoteiroPedidoGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroPedidoService = new RoteiroPedidoService(roteiroPedidoGatewayMock, roteiroGatewayMock, auditServiceMock);

            var dt = DateTime.Now;
            var rp = new RoteiroPedido() { Id = 1 };

            roteiroPedidoGatewayMock.Expect(g => g.ObterRoteirosPedidosParaAutorizar(1, dt)).Return(new[] { rp });
            roteiroPedidoGatewayMock.Expect(g => g.FindById(1)).Return(rp);

            try
            {
                roteiroPedidoService.AutorizarPedidos(1, dt);
            }
            catch
            {
                Assert.Fail();
            }

            Assert.Pass();
        }

        [Test]
        public void ObterRoteirosPedidosPorRoteiroEdtPedido_IdRoteiroDtPedidoPaging_RetornaRoteiroPedido()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroPedidoGatewayMock = MockRepository.GenerateMock<IRoteiroPedidoGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroPedidoService = new RoteiroPedidoService(roteiroPedidoGatewayMock, roteiroGatewayMock, auditServiceMock);

            var dt = DateTime.Now;
            var paging = new Infrastructure.Framework.Domain.Paging() { Limit = 50, Offset = 0, OrderBy = null };

            roteiroGatewayMock.Expect(g => g.FindById(1)).Return(new Roteiro() { blKgCx = true });

            roteiroPedidoGatewayMock
                .Expect(x => x.ObterRoteirosPedidosPorRoteiroEdtPedido(1, dt, paging))
                .Return(new List<RoteiroPedido>() { new RoteiroPedido() { Id = 1, idRoteiro = 1, ItemDetalhe = new Domain.Item.ItemDetalhe() { Id = 1 } } });

            var actual = roteiroPedidoService.ObterRoteirosPedidosPorRoteiroEdtPedido(1, dt, paging);

            Assert.AreEqual(1, actual.Count());
        }

        [Test]
        public void CalcularTotalRoteiroItem_UsarQtdRoteiroRATrueBlKgCxTrue_CalcularQtdRoteiroRA()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroPedidoGatewayMock = MockRepository.GenerateMock<IRoteiroPedidoGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroPedidoService = new RoteiroPedidoService(roteiroPedidoGatewayMock, roteiroGatewayMock, auditServiceMock);

            var dt = DateTime.Now;

            roteiroPedidoGatewayMock.Expect(g => g.BuscarSugestaoPedidoPorRoteiroItem(1, dt, 1)).Return(new List<SugestaoPedido>()
                {
                    new SugestaoPedido()
                    {  
                        TpCaixaFornecedor = "F",
                        qtdPackCompra = 1,
                        vlPesoLiquido = 1,
                        qtdSugestaoRoteiroRA = 1
                    },

                    new SugestaoPedido()
                    {  
                        TpCaixaFornecedor = "V",
                        qtdPackCompra = 1,
                        vlPesoLiquido = 1,
                        qtdSugestaoRoteiroRA = 1
                    }
                });

            roteiroGatewayMock.Expect(g => g.FindById(1)).Return(new Roteiro() { Id = 1, blKgCx = true });

            var rRA = roteiroPedidoService.CalcularTotalRoteiroItem(1, dt, true, 1);
            Assert.AreEqual(2, rRA);
        }

        [Test]
        public void CalcularTotalRoteiroItem_UsarQtdRoteiroRAFalseBlKgCxTrue_CalcularQtdRoteiro()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroPedidoGatewayMock = MockRepository.GenerateMock<IRoteiroPedidoGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroPedidoService = new RoteiroPedidoService(roteiroPedidoGatewayMock, roteiroGatewayMock, auditServiceMock);

            var dt = DateTime.Now;

            roteiroPedidoGatewayMock.Expect(g => g.BuscarSugestaoPedidoPorRoteiroItem(1, dt, 1)).Return(new List<SugestaoPedido>()
                {
                    new SugestaoPedido()
                    {  
                        TpCaixaFornecedor = "F",
                        qtdPackCompra = 1,
                        vlPesoLiquido = 1,
                        qtdSugestaoRoteiroRA = 1
                    },

                    new SugestaoPedido()
                    {  
                        TpCaixaFornecedor = "V",
                        qtdPackCompra = 1,
                        vlPesoLiquido = 1,
                        qtdSugestaoRoteiroRA = 1
                    }
                });

            roteiroGatewayMock.Expect(g => g.FindById(1)).Return(new Roteiro() { Id = 1, blKgCx = true });

            var r = roteiroPedidoService.CalcularTotalRoteiroItem(1, dt, false, 1);
            Assert.AreEqual(2, r);
        }

        [Test]
        public void CalcularTotalRoteiroItem_UsarQtdRoteiroRATrueBlKgCxFalse_CalcularQtdRoteiroRA()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroPedidoGatewayMock = MockRepository.GenerateMock<IRoteiroPedidoGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroPedidoService = new RoteiroPedidoService(roteiroPedidoGatewayMock, roteiroGatewayMock, auditServiceMock);

            var dt = DateTime.Now;

            roteiroPedidoGatewayMock.Expect(g => g.BuscarSugestaoPedidoPorRoteiroItem(1, dt, 1)).Return(new List<SugestaoPedido>()
                {
                    new SugestaoPedido()
                    {  
                        TpCaixaFornecedor = "F",
                        qtdPackCompra = 1,
                        vlPesoLiquido = 1,
                        qtdSugestaoRoteiroRA = 1
                    },

                    new SugestaoPedido()
                    {  
                        TpCaixaFornecedor = "V",
                        qtdPackCompra = 1,
                        vlPesoLiquido = 1,
                        qtdSugestaoRoteiroRA = 1
                    }
                });

            roteiroGatewayMock.Expect(g => g.FindById(1)).Return(new Roteiro() { Id = 1, blKgCx = false });

            var rRA = roteiroPedidoService.CalcularTotalRoteiroItem(1, dt, true, 1);
            Assert.AreEqual(1, rRA);
        }

        [Test]
        public void CalcularTotalRoteiroItem_UsarQtdRoteiroRAFalseBlKgCxFalse_CalcularQtdRoteiro()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroPedidoGatewayMock = MockRepository.GenerateMock<IRoteiroPedidoGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroPedidoService = new RoteiroPedidoService(roteiroPedidoGatewayMock, roteiroGatewayMock, auditServiceMock);

            var dt = DateTime.Now;

            roteiroPedidoGatewayMock.Expect(g => g.BuscarSugestaoPedidoPorRoteiroItem(1, dt, 1)).Return(new List<SugestaoPedido>()
                {
                    new SugestaoPedido()
                    {  
                        TpCaixaFornecedor = "F",
                        qtdPackCompra = 1,
                        vlPesoLiquido = 1,
                        qtdSugestaoRoteiroRA = 1
                    },

                    new SugestaoPedido()
                    {  
                        TpCaixaFornecedor = "V",
                        qtdPackCompra = 1,
                        vlPesoLiquido = 1,
                        qtdSugestaoRoteiroRA = 1
                    }
                });

            roteiroGatewayMock.Expect(g => g.FindById(1)).Return(new Roteiro() { Id = 1, blKgCx = false });

            var r = roteiroPedidoService.CalcularTotalRoteiroItem(1, dt, false, 1);
            Assert.AreEqual(1, r);
        }

        [Test]
        public void QtdPedidosNaoAutorizadosParaDataCorrente_IdRoteiro_RetornaQuantidadeDePedidosNaoAutorizados()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroPedidoGatewayMock = MockRepository.GenerateMock<IRoteiroPedidoGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroPedidoService = new RoteiroPedidoService(roteiroPedidoGatewayMock, roteiroGatewayMock, auditServiceMock);

            roteiroPedidoGatewayMock.Expect(g => g.QtdPedidosNaoAutorizadosParaDataCorrente(1)).Return(1);

            var r = roteiroPedidoService.QtdPedidosNaoAutorizadosParaDataCorrente(1);

            Assert.AreEqual(1, r);
        }

        [Test]
        public void ObterDadosAutorizacaoRoteiro_IdRoteiroDtPedido_RetornaDadosAutorizacaoRoteiro()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroPedidoGatewayMock = MockRepository.GenerateMock<IRoteiroPedidoGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroPedidoService = new RoteiroPedidoService(roteiroPedidoGatewayMock, roteiroGatewayMock, auditServiceMock);

            var dt = DateTime.Now;

            roteiroPedidoGatewayMock.Expect(g => g.ObterDadosAutorizacao(1, dt)).Return(new RoteiroPedido() { Id = 1 });

            var r = roteiroPedidoService.ObterDadosAutorizacaoRoteiro(1, dt);

            Assert.NotNull(r);
        }
    }
}
