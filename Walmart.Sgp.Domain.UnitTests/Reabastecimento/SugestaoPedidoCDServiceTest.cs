using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Globalization;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain")]
    public class SugestaoPedidoCDServiceTest
    {
        [Test]
        public void ValidarDataEnvio_DataEnvioVMInvalida_SpecResultNotSatisfied()
        {
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoPedidoCDService = new SugestaoPedidoCDService(sugestaoPedidoCDGateway, auditServiceMock);

            var sg = new SugestaoPedidoCD()
            {
                Id = 1,
                dtEnvioPedidoSerialized = "1/13/1900"
            };

            var result = sugestaoPedidoCDService.ValidarDataEnvio(sg);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void ValidarDataCancelamento_DataCancelamentoVMInvalida_SpecResultNotSatisfied()
        {
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoPedidoCDService = new SugestaoPedidoCDService(sugestaoPedidoCDGateway, auditServiceMock);

            var sg = new SugestaoPedidoCD()
            {
                Id = 1,
                dtCancelamentoPedidoSerialized = "1/13/1900"
            };

            var result = sugestaoPedidoCDService.ValidarDataCancelamento(sg);

            Assert.IsFalse(result.Satisfied);
        }

        [Test]
        public void Salvar_SugestaoPedidoCD_DeveLancarExceptionDataCancelamentoVMInvalida()
        {
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoPedidoCDService = new SugestaoPedidoCDService(sugestaoPedidoCDGateway, auditServiceMock);

            var hj = DateTime.Now.Date;

            var sg = new SugestaoPedidoCD()
            {
                Id = 1,
                dtEnvioPedidoSerialized = hj.ToString("dd/MM/yyyy", RuntimeContext.Current.Culture),
                dtCancelamentoPedidoSerialized = "2/13/1900",
                qtdPackCompra = 1,
                blFinalizado = true
            };

            Assert.Catch(() =>
            {
                sugestaoPedidoCDService.SalvarVarios(new[] { sg });
            }, Texts.InvalidDateFormatException);
        }

        [Test]
        public void Salvar_SugestaoPedidoCD_DeveLancarExceptionDataEnvioVMInvalida()
        {
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoPedidoCDService = new SugestaoPedidoCDService(sugestaoPedidoCDGateway, auditServiceMock);

            var hj = DateTime.Now.Date;

            var sg = new SugestaoPedidoCD()
            {
                Id = 1,
                dtEnvioPedidoSerialized = "2/13/1900",
                dtCancelamentoPedidoSerialized = hj.AddDays(5).ToString("dd/MM/yyyy", RuntimeContext.Current.Culture),
                qtdPackCompra = 1,
                blFinalizado = true
            };

            Assert.Catch(() =>
            {
                sugestaoPedidoCDService.SalvarVarios(new[] { sg });
            }, Texts.InvalidDateFormatException);
        }

        [Test]
        public void Salvar_SugestaoPedidoCD_DeveInvocarUpdate()
        {
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoPedidoCDService = new SugestaoPedidoCDService(sugestaoPedidoCDGateway, auditServiceMock);

            var hj = DateTime.Now.Date;

            var sg = new SugestaoPedidoCD()
            {
                Id = 1,
                dtEnvioPedido = hj,
                dtCancelamentoPedido = hj.AddDays(5),
                qtdPackCompra = 1,
                blFinalizado = true
            };

            var sg2 = new SugestaoPedidoCD()
            {
                Id = 1,
                dtEnvioPedido = hj,
                dtCancelamentoPedido = hj.AddDays(5),
                qtdPackCompra = 1,
                blFinalizado = true,
                dtEnvioPedidoSerialized = hj.ToString("dd/MM/yyyy", RuntimeContext.Current.Culture),
                dtCancelamentoPedidoSerialized = hj.AddDays(5).ToString("dd/MM/yyyy", RuntimeContext.Current.Culture)
            };

            sugestaoPedidoCDService.SalvarVarios(new[] { sg, sg2 });

            sugestaoPedidoCDGateway.AssertWasCalled(g => g.Update("qtdPackCompra=@qtdPackCompra,dtEnvioPedido=@dtEnvioPedido,dtCancelamentoPedido=@dtCancelamentoPedido,blFinalizado=@blFinalizado", sg));
        }

        [Test]
        public void ValidarDataCancelamento_SugestaoPedidoCDSemDatas_SpecResultNaoNulo()
        {
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoPedidoCDService = new SugestaoPedidoCDService(sugestaoPedidoCDGateway, auditServiceMock);

            var sg = new SugestaoPedidoCD()
            {
                Id = 1
            };

            var result = sugestaoPedidoCDService.ValidarDataCancelamento(sg);
            Assert.IsNotNull(result);
        }

        [Test]
        public void ValidarDataEnvio_SugestaoPedidoCDSemDatas_SpecResultNaoNulo()
        {
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoPedidoCDService = new SugestaoPedidoCDService(sugestaoPedidoCDGateway, auditServiceMock);

            var sg = new SugestaoPedidoCD()
            {
                Id = 1
            };

            var result = sugestaoPedidoCDService.ValidarDataEnvio(sg);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Pesquisar_DadosDePesquisa_SomenteUmaSugestaoPedidoCD()
        {
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoPedidoCDService = new SugestaoPedidoCDService(sugestaoPedidoCDGateway, auditServiceMock);

            var dt = DateTime.Now;
            var pg = new Paging();

            var filtro = new SugestaoPedidoCDFiltro()
            {
                DtSolicitacao = dt,
                IdCD = 1,
                IdDepartamento = 1,
                IdFornecedorParametro = 1,
                IdItem = 1,
                ItemPesoVariavel = 1,
                StatusPedido = 2
            };

            sugestaoPedidoCDGateway.Expect(g => g.Pesquisar(filtro, pg)).Return(new[] {new SugestaoPedidoCD()
            {
                Id = 1
            }});

            var result = sugestaoPedidoCDService.Pesquisar(filtro, pg);

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void Pesquisar_DadosDePesquisa_VariasSugestaoPedidoCD()
        {
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoPedidoCDService = new SugestaoPedidoCDService(sugestaoPedidoCDGateway, auditServiceMock);

            var dt = DateTime.Now;
            var pg = new Paging();

            var filtro = new SugestaoPedidoCDFiltro()
            {
                DtSolicitacao = dt,
                IdCD = 1,
                IdDepartamento = 1,
                IdFornecedorParametro = 1,
                IdItem = 1,
                ItemPesoVariavel = 1,
                StatusPedido = 2
            };

            sugestaoPedidoCDGateway.Expect(g => g.Pesquisar(filtro, pg)).Return(new[] {new SugestaoPedidoCD()
            {
                Id = 1
            },new SugestaoPedidoCD()
            {
                Id = 2
            }});

            var result = sugestaoPedidoCDService.Pesquisar(filtro, pg);

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void ObterPorId_IdDeUmaSugestaoPedidoCD_SugestaoPedidoCDComIdUm()
        {
            var sugestaoPedidoCDGateway = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoPedidoCDService = new SugestaoPedidoCDService(sugestaoPedidoCDGateway, auditServiceMock);

            sugestaoPedidoCDGateway.Expect(g => g.FindById(1)).Return(new SugestaoPedidoCD()
            {
                Id = 1
            });

            var result = sugestaoPedidoCDService.ObterPorId(1);

            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public void FinalizarPedidos_SugestaoPedidoCDsComDados_ASugestoesPassadasParaOUpdateDoGatewaySaoOsMesmosDaEntrada()
        {
            var sugestaoPedidoCDGatewayMock = MockRepository.GenerateMock<ISugestaoPedidoCDGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();

            var sugestoesParaFinalizar = new[]
            {
                new SugestaoPedidoCD
                {
                    IDSugestaoPedidoCD = 111,
                    idItemDetalhePedido = 123,
                    idItemDetalheSugestao = 444,
                    dtEnvioPedido = DateTime.Today,
                    dtCancelamentoPedido = DateTime.Today.AddDays(1)
                },

                new SugestaoPedidoCD
                {
                    IDSugestaoPedidoCD = 222,
                    idItemDetalhePedido = 112,
                    idItemDetalheSugestao = 321,
                    dtEnvioPedido = DateTime.Today,
                    dtCancelamentoPedido = DateTime.Today.AddDays(1)
                }
            };

            var target = new SugestaoPedidoCDService(sugestaoPedidoCDGatewayMock, auditServiceMock);
            target.FinalizarPedidos(sugestoesParaFinalizar);

            var arguments = sugestaoPedidoCDGatewayMock.GetArgumentsForCallsMadeOn(x => x.Update(null, new SugestaoPedidoCD()));

            var sugestaoPedidoCD1 = (SugestaoPedidoCD)arguments[0][1];
            var sugestaoPedidoCD2 = (SugestaoPedidoCD)arguments[1][1];

            Assert.AreEqual(sugestoesParaFinalizar[0], sugestaoPedidoCD1);
            Assert.AreEqual(sugestoesParaFinalizar[1], sugestaoPedidoCD2);
        }
    }
}
