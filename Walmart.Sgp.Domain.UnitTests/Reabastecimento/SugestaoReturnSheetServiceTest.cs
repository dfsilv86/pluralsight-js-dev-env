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

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain"), Category("SugestaoReturnSheet")]
    public class SugestaoReturnSheetServiceTest
    {
        [Test]
        public void SalvarSugestoesLoja_SugestaoReturnSheetComDados_ExecucaoDoMetodoUpdateDoGateway()
        {
            var sugestaoReturnSheetGatewayMock = MockRepository.GenerateMock<ISugestaoReturnSheetGateway>();
            var returnSheetGatewayMock = MockRepository.GenerateMock<IReturnSheetGateway>();
            
            var sugestaoOld = new SugestaoReturnSheet
            {
                Id = 123,
                QtdRA = null,
                QtdLoja = null,
            };

            var sugestaoNew = new SugestaoReturnSheet
            {
                Id = 123,
                QtdRA = 10,
                QtdLoja = 20,
                ItemLoja = new ReturnSheetItemLoja
                {
                    ItemPrincipal = new ReturnSheetItemPrincipal
                    {
                        ItemDetalhe = new Domain.Item.ItemDetalhe
                        {
                            TpCaixaFornecedor = "F"
                        },

                        ReturnSheet = new ReturnSheet()
                        {
                            Id = 1,
                            DhFinalReturn = DateTime.Now.AddDays(1)
                        }
                    }
                }
            };

            sugestaoReturnSheetGatewayMock.Expect(x => x.Obter(sugestaoOld.Id)).Return(sugestaoOld);
            returnSheetGatewayMock.Expect(g => g.PossuiExportacao(sugestaoNew.ItemLoja.ItemPrincipal.ReturnSheet.Id)).Return(false);

            var sugestaoReturnSheetService = new SugestaoReturnSheetService(sugestaoReturnSheetGatewayMock, returnSheetGatewayMock, MockRepository.GenerateMock<IAuditService>(), MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>());
            
            sugestaoReturnSheetService.SalvarSugestoesLoja(new [] { sugestaoNew });

            sugestaoReturnSheetGatewayMock.AssertWasCalled(g => g.Update("qtdLoja = @qtdLoja, qtdRA = @qtdRA, IdUsuarioAtualizacao = @IdUsuarioAtualizacao, DhAtualizacao = @DhAtualizacao", sugestaoOld));
            Assert.AreEqual(null, sugestaoOld.QtdRA);
            Assert.AreEqual(20, sugestaoOld.QtdLoja);
        }

        [Test]
        public void SalvarSugestoesRA_SugestaoReturnSheetComDados_ExecucaoDoMetodoUpdateDoGateway()
        {
            var sugestaoReturnSheetGatewayMock = MockRepository.GenerateMock<ISugestaoReturnSheetGateway>();
            var returnSheetGatewayMock = MockRepository.GenerateMock<IReturnSheetGateway>();

            var sugestaoOld = new SugestaoReturnSheet
            {
                Id = 123,
                QtdRA = null,
                QtdLoja = null,
            };

            var sugestaoNew = new SugestaoReturnSheet
            {
                Id = 123,
                QtdRA = 20,
                QtdLoja = 30,
                ItemLoja = new ReturnSheetItemLoja
                {
                    ItemPrincipal = new ReturnSheetItemPrincipal
                    {
                        ItemDetalhe = new Domain.Item.ItemDetalhe
                        {
                            TpCaixaFornecedor = "F"
                        },

                        ReturnSheet = new ReturnSheet()
                        {
                            Id = 1,
                            DhFinalReturn = DateTime.Now.AddDays(1)
                        }
                    }
                }
            };

            sugestaoReturnSheetGatewayMock.Expect(x => x.Obter(sugestaoOld.Id)).Return(sugestaoOld);
            returnSheetGatewayMock.Expect(g => g.PossuiExportacao(sugestaoNew.ItemLoja.ItemPrincipal.ReturnSheet.Id)).Return(false);

            var sugestaoReturnSheetService = new SugestaoReturnSheetService(sugestaoReturnSheetGatewayMock, returnSheetGatewayMock, MockRepository.GenerateMock<IAuditService>(), MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>());

            sugestaoReturnSheetService.SalvarSugestoesRA(new[] { sugestaoNew });

            sugestaoReturnSheetGatewayMock.AssertWasCalled(g => g.Update("qtdLoja = @qtdLoja, qtdRA = @qtdRA, IdUsuarioAtualizacao = @IdUsuarioAtualizacao, DhAtualizacao = @DhAtualizacao", sugestaoOld));
            Assert.AreEqual(null, sugestaoOld.QtdLoja);
            Assert.AreEqual(20, sugestaoOld.QtdRA);
        }

        [Test]
        public void AutorizarExportarPlanilhas_ConsultaReturnSheetRAParametroFiltroSemDados_ExecucaoDoMetodoAutorizarExportarPlanilhasDoGateway()
        {
            var sugestaoReturnSheetGateway = MockRepository.GenerateMock<ISugestaoReturnSheetGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoReturnSheetService = new SugestaoReturnSheetService(sugestaoReturnSheetGateway, MockRepository.GenerateMock<IReturnSheetGateway>(), auditServiceMock, MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>());

            sugestaoReturnSheetService.AutorizarExportarPlanilhas(DateTime.Today, DateTime.Today, null, null, null, 1, null, null, null, null);

            sugestaoReturnSheetGateway.AssertWasCalled(g => g.AutorizarExportarPlanilhas(DateTime.Today, DateTime.Today, null, null, null, 1, null, null, null, null));
        }

        [Test]
        public void ConsultaReturnSheetRA_ConsultaReturnSheetRAParametroFiltroComDados_ApenasUmaSugestaoReturnSheet()
        {
            var dt = DateTime.Now;
            var paging = new Infrastructure.Framework.Domain.Paging() { Limit = 50, Offset = 0, OrderBy = null };

            var sugestaoReturnSheetGateway = MockRepository.GenerateMock<ISugestaoReturnSheetGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoReturnSheetService = new SugestaoReturnSheetService(sugestaoReturnSheetGateway, MockRepository.GenerateMock<IReturnSheetGateway>(), auditServiceMock, MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>());

            sugestaoReturnSheetGateway
                .Expect(x => x.ConsultaReturnSheetLojaRA(DateTime.Today, DateTime.Today, null, null, null, 1, null, null, null, null, paging))
                .Return(new List<SugestaoReturnSheet>() { new SugestaoReturnSheet() { Id = 1 } });

            var r = sugestaoReturnSheetService.ConsultaReturnSheetLojaRA(DateTime.Today, DateTime.Today, null, null, null, 1, null, null, null, null, paging);

            Assert.AreEqual(1, r.Count());
        }

        [Test]
        public void ConsultaReturnSheetLoja_ParametrosParaABuscaDeReturnSheetLoja_ApenasUmaSugestaoReturnSheet()
        {
            var dt = DateTime.Now;
            var paging = new Infrastructure.Framework.Domain.Paging() { Limit = 50, Offset = 0, OrderBy = null };

            var sugestaoReturnSheetGateway = MockRepository.GenerateMock<ISugestaoReturnSheetGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoReturnSheetService = new SugestaoReturnSheetService(sugestaoReturnSheetGateway, MockRepository.GenerateMock<IReturnSheetGateway>(), auditServiceMock, MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>());

            sugestaoReturnSheetGateway
                .Expect(x => x.ConsultaReturnSheetLoja(1, 1, dt, "Teste", 1, 1, paging))
                .Return(new List<SugestaoReturnSheet>() { new SugestaoReturnSheet() { Id = 1 } });

            var r = sugestaoReturnSheetService.ConsultaReturnSheetLoja(1, 1, dt, "Teste", 1, 1, paging);

            Assert.AreEqual(1, r.Count());
        }

        [Test]
        public void Remover_Id_MetodoDeleteNaoFoiExecutadoReturnSheetFoiApenasInativada()
        {
            var sugestaoReturnSheetGateway = MockRepository.GenerateMock<ISugestaoReturnSheetGateway>();
            sugestaoReturnSheetGateway.Expect(g => g.FindById(1)).Return(new SugestaoReturnSheet() { Id = 1 });

            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoReturnSheetService = new SugestaoReturnSheetService(sugestaoReturnSheetGateway, MockRepository.GenerateMock<IReturnSheetGateway>(), auditServiceMock, MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>());

            sugestaoReturnSheetService.Remover(1);

            sugestaoReturnSheetGateway.AssertWasNotCalled(g => g.Delete(1));
        }

        [Test]
        public void Obter_Id_SugestaoReturnSheetComId2016()
        {
            var sugestaoReturnSheetGateway = MockRepository.GenerateMock<ISugestaoReturnSheetGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoReturnSheetService = new SugestaoReturnSheetService(sugestaoReturnSheetGateway, MockRepository.GenerateMock<IReturnSheetGateway>(), auditServiceMock, MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>());

            sugestaoReturnSheetGateway
                .Expect(x => x.Obter(2016))
                .Return(new SugestaoReturnSheet() { Id = 2016 });

            Assert.IsNotNull(sugestaoReturnSheetService.Obter(2016));
        }

        [Test]
        public void ObterPorIdReturnSheetItemLoja_IdDeReturnSheetItemLoja_ApenasUmaSugestaoReturnSheet()
        {
            var sugestaoReturnSheetGateway = MockRepository.GenerateMock<ISugestaoReturnSheetGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoReturnSheetService = new SugestaoReturnSheetService(sugestaoReturnSheetGateway, MockRepository.GenerateMock<IReturnSheetGateway>(), auditServiceMock, MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>());
            sugestaoReturnSheetGateway.Expect(g => g.ObterPorIdReturnSheetItemLoja(1)).Return(new List<SugestaoReturnSheet>()
            {
                new SugestaoReturnSheet(){Id = 1, IdReturnSheetItemLoja = 1}
            });

            var result = sugestaoReturnSheetService.ObterPorIdReturnSheetItemLoja(1);

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void PossuiReturnsVigentesQuantidadesVazias_IdLojaIdUsuario_PossuiReturnsVigentes()
        {
            var currentContext = RuntimeContext.Current;

            try
            {
                RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 2337, RoleId = 123, StoreId = 663 } };

                var sugestaoReturnSheetGatewayMock = MockRepository.GenerateMock<ISugestaoReturnSheetGateway>();
                sugestaoReturnSheetGatewayMock.Expect(x => x.PossuiReturnsVigentesQuantidadesVazias(2337, 123, 663)).Return(true);

                var sugestaoReturnSheetService = new SugestaoReturnSheetService(sugestaoReturnSheetGatewayMock, MockRepository.GenerateMock<IReturnSheetGateway>(), MockRepository.GenerateMock<IAuditService>(), MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>());

                var result = sugestaoReturnSheetService.PossuiReturnsVigentesQuantidadesVazias();
                Assert.True(result);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void PossuiReturnsVigentesQuantidadesVazias_IdLojaIdUsuario_NaoPossuiReturnsVigentes()
        {
            var currentContext = RuntimeContext.Current;

            try
            {
                RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 2337, StoreId = 663 } };

                var sugestaoReturnSheetGatewayMock = MockRepository.GenerateMock<ISugestaoReturnSheetGateway>();
                sugestaoReturnSheetGatewayMock.Expect(x => x.PossuiReturnsVigentesQuantidadesVazias(2337, 123, 663)).Return(false);

                var sugestaoReturnSheetService = new SugestaoReturnSheetService(sugestaoReturnSheetGatewayMock, MockRepository.GenerateMock<IReturnSheetGateway>(), MockRepository.GenerateMock<IAuditService>(), MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>());

                var result = sugestaoReturnSheetService.PossuiReturnsVigentesQuantidadesVazias();
                Assert.False(result);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void PossuiReturnsVigentesQuantidadesVazias_IdLojaVazioIdUsuario_NaoPossuiReturnsVigentes()
        {
            var currentContext = RuntimeContext.Current;

            try
            {
                RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 2337 } };

                var sugestaoReturnSheetService = new SugestaoReturnSheetService(MockRepository.GenerateMock<ISugestaoReturnSheetGateway>(), MockRepository.GenerateMock<IReturnSheetGateway>(), MockRepository.GenerateMock<IAuditService>(), MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>());

                var result = sugestaoReturnSheetService.PossuiReturnsVigentesQuantidadesVazias();
                Assert.False(result);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void RegistrarLogAvisoReturnSheetsVigentes_IdLojaIdUsuario_Salvar()
        {
            var currentContext = RuntimeContext.Current;

            try
            {
                var user =  new MemoryRuntimeUser { Id = 2337, StoreId = 663 };
                RuntimeContext.Current = new MemoryRuntimeContext { User = user };

                var logMensagemReturnSheetVigenteServiceMock = MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>();

                var sugestaoReturnSheetService = new SugestaoReturnSheetService(MockRepository.GenerateMock<ISugestaoReturnSheetGateway>(), MockRepository.GenerateMock<IReturnSheetGateway>(), MockRepository.GenerateMock<IAuditService>(), logMensagemReturnSheetVigenteServiceMock);
                sugestaoReturnSheetService.RegistrarLogAvisoReturnSheetsVigentes();

                var parametros = logMensagemReturnSheetVigenteServiceMock.GetArgumentsForCallsMadeOn(x => x.Salvar(new LogMensagemReturnSheetVigente()));
                var parametro = (LogMensagemReturnSheetVigente)parametros[0][0];

                Assert.AreEqual(user.Id, parametro.IDUsuario);
                Assert.AreEqual(user.StoreId, parametro.IDLoja);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }
    }
}