using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Domain.Gerenciamento;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain")]
    public class RoteiroServiceTest
    {
        [Test]
        public void ObterRoteirosPorFornecedor_Fornecedor_RoteirosDoFornecedor()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroLojaGatewayMock = MockRepository.GenerateMock<IRoteiroLojaService>();
            var sugestaoPedidoGatewayMock = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
            var fornecedorParametroGatewayMock = MockRepository.GenerateMock<IFornecedorParametroGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroService = new RoteiroService(roteiroGatewayMock, roteiroLojaGatewayMock, sugestaoPedidoGatewayMock, auditServiceMock, fornecedorParametroGatewayMock);

            var paging = new Paging();

            roteiroGatewayMock
                .Expect(x => x.ObterRoteirosPorFornecedor(1, 2, 3, null, paging))
                .Return(new[] { new Roteiro() });

            var actual = roteiroService.ObterRoteirosPorFornecedor(1, 2, 3, null, paging);

            roteiroGatewayMock.AssertWasCalled(x => x.ObterRoteirosPorFornecedor(1, 2, 3, null, paging));
            Assert.AreEqual(1, actual.Count());
        }
        
        [Test]
        public void Salvar_Roteiro_ExecucaoDoMetodoInsertDoGateway()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroLojaService = MockRepository.GenerateMock<IRoteiroLojaService>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
            var fornecedorParametroGatewayMock = MockRepository.GenerateMock<IFornecedorParametroGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroService = new RoteiroService(roteiroGatewayMock, roteiroLojaService, sugestaoPedidoGateway, auditServiceMock, fornecedorParametroGatewayMock);

            var entidade = new Roteiro()
            {
                Id = 0,
                cdV9D = 1,
                Descricao = "Teste",
                vlCargaMinima = 1,
                Lojas = new List<RoteiroLoja>() { new RoteiroLoja() { Id = 1, idRoteiro = 1, blativo = true, blQuinta = true } }
            };

            fornecedorParametroGatewayMock.Expect(x => x.PossuiItensDSD(1)).Return(true);

            roteiroService.Salvar(entidade);

            roteiroGatewayMock.AssertWasCalled(g => g.Insert(entidade));
        }

        [Test]
        public void ObterSugestaoPedidoLoja_IdDeUmaLoja_SugestaoPedido()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroLojaGatewayMock = MockRepository.GenerateMock<IRoteiroLojaService>();
            var sugestaoPedidoGatewayMock = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
            var fornecedorParametroGatewayMock = MockRepository.GenerateMock<IFornecedorParametroGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroService = new RoteiroService(roteiroGatewayMock, roteiroLojaGatewayMock, sugestaoPedidoGatewayMock, auditServiceMock, fornecedorParametroGatewayMock);

            var paging = new Paging();
            var dt = DateTime.Now;
            var r = new Roteiro() { IDRoteiro = 1 };

            roteiroGatewayMock.Expect(g => g.FindById(1)).Return(r);

            roteiroGatewayMock
                .Expect(x => x.ObterSugestaoPedidoLoja(1, dt, 1, paging))
                .Return(new[] { new SugestaoPedido() { Id = 1, IDItemDetalhePedido = 1 } });

            var actual = roteiroService.ObterSugestaoPedidoLoja(1, dt, 1, paging);

            Assert.AreEqual(1, actual.Count());
        }

        [Test]
        public void SalvarSugestaoPedidoConvertidoCaixa_RoteiroComDadosESugestaoPedidoComTpCaixaFornecedorV_AtualizarQtdSugestaoRoteiroRADaSugestaoPedido()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroLojaService = MockRepository.GenerateMock<IRoteiroLojaService>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
            var fornecedorParametroGatewayMock = MockRepository.GenerateMock<IFornecedorParametroGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroService = new RoteiroService(roteiroGatewayMock, roteiroLojaService, sugestaoPedidoGateway, auditServiceMock, fornecedorParametroGatewayMock);

            var roteiro = new Roteiro()
            {
                Id = 0,
                cdV9D = 1,
                Descricao = "Teste",
                vlCargaMinima = 1,
                blKgCx = true,
                Lojas = new List<RoteiroLoja>() { new RoteiroLoja() { Id = 1, idRoteiro = 1, blativo = true, blQuinta = true } }
            };

            roteiroGatewayMock.Expect(g => g.FindById(1)).Return(roteiro);

            var su = new SugestaoPedido() { qtVendorPackage = 1, vlPesoLiquido = 1, ValorEmCaixa = 1, ValorEmCaixaRA = 1, TpCaixaFornecedor = "V", Id = 1 };
            var sugestoes = new[] { su };

            sugestaoPedidoGateway.Expect(g => g.FindById(1)).Return(su);

            roteiroService.SalvarSugestaoPedidoConvertidoCaixa(sugestoes, 1);

            sugestaoPedidoGateway.AssertWasCalled(g => g.Update("qtdSugestaoRoteiroRA = @qtdSugestaoRoteiroRA", su));
        }

        [Test]
        public void SalvarSugestaoPedidoConvertidoCaixa_RoteiroComBlKgCxFalseESugestaoPedidoComTpCaixaFornecedorF_AtualizarQtdSugestaoRoteiroRADaSugestaoPedido()
        {
            var roteiroGatewayMock = MockRepository.GenerateMock<IRoteiroGateway>();
            var roteiroLojaService = MockRepository.GenerateMock<IRoteiroLojaService>();
            var sugestaoPedidoGateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
            var fornecedorParametroGatewayMock = MockRepository.GenerateMock<IFornecedorParametroGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var roteiroService = new RoteiroService(roteiroGatewayMock, roteiroLojaService, sugestaoPedidoGateway, auditServiceMock,fornecedorParametroGatewayMock);

            var roteiro = new Roteiro()
            {
                Id = 0,
                cdV9D = 1,
                Descricao = "Teste",
                vlCargaMinima = 1,
                blKgCx = false,
                Lojas = new List<RoteiroLoja>() { new RoteiroLoja() { Id = 1, idRoteiro = 1, blativo = true, blQuinta = true } }
            };

            roteiroGatewayMock.Expect(g => g.FindById(1)).Return(roteiro);

            var su = new SugestaoPedido() { qtVendorPackage = 1, vlPesoLiquido = 1, ValorEmCaixa = 1, ValorEmCaixaRA = 1, TpCaixaFornecedor = "F", Id = 1 };
            var sugestoes = new[] { su };

            sugestaoPedidoGateway.Expect(g => g.FindById(1)).Return(su);

            roteiroService.SalvarSugestaoPedidoConvertidoCaixa(sugestoes, 1);

            sugestaoPedidoGateway.AssertWasCalled(g => g.Update("qtdSugestaoRoteiroRA = @qtdSugestaoRoteiroRA", su));
        }
    }
}
