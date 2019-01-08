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
using Walmart.Sgp.Infrastructure.Data.Memory;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Extensions;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Is = Rhino.Mocks.Constraints.Is;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain")]
    public class SugestaoPedidoServiceTest
    {
        [Test]
        public void PesquisarPorFiltro_Alcadas_Ok()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var request = new SugestaoPedidoFiltro
                {
                    cdDepartamento = 1,
                    cdLoja = 1,
                    cdSistema = 1,
                    dtPedido = DateTime.Today,
                    IDUsuario = 1,
                };

                var paging = new Paging();

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

                gateway.Expect(g => g.PesquisarPorFiltros(request, paging)).Return(new SugestaoPedidoModel[]
                {
                    new SugestaoPedidoModel 
                    {
                        IDSugestaoPedido = 1,
                        FornecedorParametro = new FornecedorParametro {
                            Fornecedor = new Fornecedor {
                                nmFornecedor = "abc"
                            },
                            cdV9D = 123456789,
                            cdTipo = TipoCodigoReabastecimento.Dao
                        },
                        ItemDetalheSugestao = new ItemDetalhe {
                            FineLine = new FineLine {
                                cdFineLine = 123,
                                dsFineLine = "fineline 123"
                            },
                            CdItem = 1234,
                            DsItem = "item 1234",
                        },
                        ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                        Loja = new Loja { IDLoja = 1, IDBandeira = 1 },
                        IdLoja = 1,
                        cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                        vlForecastMedio = 12.345m,
                        dtInicioForecast = DateTime.Today.AddDays(-3),
                        dtFimForecast = DateTime.Today,
                        qtVendorPackage = 1,
                        vlModulo = 2m,
                        vlEstoqueOriginal = -2,
                        qtdPackCompraOriginal = 24,
                        vlEstoque = 12.789m,
                        qtdPackCompra = 3,
                    },
                    new SugestaoPedidoModel 
                    {
                        IDSugestaoPedido = 2,
                        FornecedorParametro = new FornecedorParametro {
                            Fornecedor = new Fornecedor {
                                nmFornecedor = "abc"
                            },
                            cdV9D = 123456789,
                            cdTipo = TipoCodigoReabastecimento.Dao
                        },
                        ItemDetalheSugestao = new ItemDetalhe {
                            FineLine = new FineLine {
                                cdFineLine = 123,
                                dsFineLine = "fineline 123"
                            },
                            CdItem = 12345,
                            DsItem = "item 12345",
                        },
                        ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                        Loja = new Loja { IDLoja = 1, IDBandeira = 1 },
                        IdLoja = 1,
                        cdOrigemCalculo = TipoOrigemCalculo.Manual,
                        vlForecastMedio = 12.345m,
                        dtInicioForecast = DateTime.Today.AddDays(-3),
                        dtFimForecast = DateTime.Today,
                        qtVendorPackage = 2,
                        vlModulo = 2m,
                        vlEstoqueOriginal = 0,
                        qtdPackCompraOriginal = 12,
                        vlEstoque = 12.789m,
                        qtdPackCompra = 3,
                    dtPedido = DateTime.Today
                    }
                });

                gateway.Stub(g => g.PesquisarPorFiltros(null, null)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaGateway = new MemoryAlcadaGateway();
                alcadaGateway.Insert(new Alcada { blAlterarInformacaoEstoque = true, vlPercentualAlterado = 50, IDAlcada = 1, IDPerfil = 1 });

                var alcadaService = new AlcadaService(alcadaGateway, null);

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                //gradeSugestaoService.Expect(s => s.ObterGradeSugestoesSeFechada(1, 1, 1, 1, DateTime.Now.ToMilitaryTime())).Return(new GradeSugestao { IDGradeSugestao = 1, cdSistema = 1, IDDepartamento = 1, IDLoja = 1, IDBandeira = 1, vlHoraInicial = DateTime.Now.AddMinutes(-10).ToMilitaryTime(), vlHoraFinal = DateTime.Now.AddMinutes(10).ToMilitaryTime() });
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway, alcadaService, gradeSugestaoService, null, null, null, alcadaDetalheService);

                var result = target.PesquisarPorFiltros(request, paging).ToList();

                Assert.AreEqual(2, result.Count);

                var item = result[0];

                Assert.AreEqual(1, item.IDSugestaoPedido);
                Assert.IsTrue(item.blAlterarInformacaoEstoque);
                Assert.AreEqual(12, item.vlLimiteInferior);
                Assert.AreEqual(36, item.vlLimiteSuperior);

                item = result[1];

                Assert.AreEqual(2, item.IDSugestaoPedido);
                Assert.IsTrue(item.blAlterarInformacaoEstoque);
                Assert.AreEqual(6, item.vlLimiteInferior);
                Assert.AreEqual(18, item.vlLimiteSuperior);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void ObterEstruturado_Codigo_Ok()
        {
            var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

            gateway.Expect(g => g.ObterEstruturado(2)).Return(new SugestaoPedido { IDSugestaoPedido = 2, Loja = new Loja { IDLoja = 3 }, IdLoja = 3 });
            gateway.Stub(g => g.ObterEstruturado(2)).IgnoreArguments().Throw(new InvalidOperationException());

            var target = new SugestaoPedidoService(gateway, null, null, null, null, null, null);

            var result = target.ObterEstruturado(2);

            gateway.VerifyAllExpectations();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.IDSugestaoPedido);
            Assert.AreEqual(3, result.Loja.IDLoja);
        }

        [Test]
        public void ObterEstruturadoComAlcada_Codigo_Ok()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();

                gateway.Expect(g => g.ObterEstruturado(2)).Return(new SugestaoPedido { IDSugestaoPedido = 2, Loja = new Loja { IDLoja = 3 }, IdLoja = 3 });
                gateway.Stub(g => g.ObterEstruturado(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaGateway = new MemoryAlcadaGateway();
                alcadaGateway.Insert(new Alcada { blAlterarInformacaoEstoque = true, vlPercentualAlterado = 50, IDAlcada = 1, IDPerfil = 1 });

                var alcadaService = new AlcadaService(alcadaGateway, null);

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway, alcadaService, null, null, null, null, alcadaDetalheService);

                var result = target.ObterEstruturadoComAlcada(2);

                gateway.VerifyAllExpectations();

                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.IDSugestaoPedido);
                Assert.AreEqual(3, result.Loja.IDLoja);
                Assert.IsTrue(result.blAlterarInformacaoEstoque);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void ValidarAlteracaoSugestao_GradeFechada_NotSatisfied()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 24,
                    vlEstoque = 12.789m,
                    vlFatorConversao = 1,
                    qtdPackCompra = 3,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                });
                gateway.Stub(s => s.ObterEstruturado(2)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(null);
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                //gradeSugestaoService.Expect(s => s.ObterGradeSugestoesSeFechada(1, 1, 1, 1, DateTime.Now.ToMilitaryTime())).Return(new GradeSugestao { IDGradeSugestao = 1, cdSistema = 1, IDDepartamento = 1, IDLoja = 1, IDBandeira = 1, vlHoraInicial = DateTime.Now.AddMinutes(-10).ToMilitaryTime(), vlHoraFinal = DateTime.Now.AddMinutes(10).ToMilitaryTime() });
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(false);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    null,
                    alcadaDetalheService);

                var response = target.ValidarAlteracaoSugestao(new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 1, qtdPackCompra = 1, IDFornecedorParametro = 1 }, 1, 1, DateTime.Today);

                Assert.IsFalse(response);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void ValidarAlteracaoSugestao_GradeAberta_NotSatisfied()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 24,
                    vlEstoque = 12.789m,
                    vlFatorConversao = 1,
                    qtdPackCompra = 3,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                });
                gateway.Stub(s => s.ObterEstruturado(2)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(null);
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                //gradeSugestaoService.Expect(s => s.ObterGradeSugestoesSeFechada(1, 1, 1, 1, DateTime.Now.ToMilitaryTime())).Return(new GradeSugestao { IDGradeSugestao = 1, cdSistema = 1, IDDepartamento = 1, IDLoja = 1, IDBandeira = 1, vlHoraInicial = DateTime.Now.AddMinutes(-10).ToMilitaryTime(), vlHoraFinal = DateTime.Now.AddMinutes(10).ToMilitaryTime() });
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    null,
                    alcadaDetalheService);

                var response = target.ValidarAlteracaoSugestao(new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 1, qtdPackCompra = 1, IDFornecedorParametro = 1 }, 1, 1, DateTime.Today);

                Assert.IsFalse(response);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void ValidarAlteracaoSugestao_GradeAberta_Satisfied()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var sugestaoPedido1 = new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 3m,
                    vlForecast = 3m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 1m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 1,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                };

                var sugestaoPedido2 = new SugestaoPedido
                {
                    IDSugestaoPedido = 2,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 3m,
                    vlForecast = 3m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 2,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 2, CdSistema = 1 },
                    IDItemDetalheSugestao = 2,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 2, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                };

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(sugestaoPedido1);
                //gateway.Expect(g => g.ObterEstruturado(2)).Return(sugestaoPedido2);
                gateway.Stub(s => s.ObterEstruturado(3)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.PesquisarPorFornecedorParametroELoja(1, 1)).Return(new SugestaoPedidoModel[] { new SugestaoPedidoModel(sugestaoPedido1), new SugestaoPedidoModel(sugestaoPedido2) });
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(new Alcada { IDAlcada = 1, blAlterarInformacaoEstoque = true, blAlterarSugestao = true, blZerarItem = true, IDPerfil = 1, vlPercentualAlterado = 50, blAlterarPercentual = true });
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 2, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(2), Is.Equal(1), Is.Anything()).Return(false);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    null,
                    alcadaDetalheService);

                var response = target.ValidarAlteracaoSugestao(new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 2, qtdPackCompra = 4, IDFornecedorParametro = 1 }, 1, 1, DateTime.Today);

                Assert.IsTrue(response);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AlterarSugestoesPedido_NaoEncontrado_Inexistente()
        {
            var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
            gateway.Expect(g => g.ObterEstruturado(1)).Return(null);
            gateway.Stub(s => s.ObterEstruturado(2)).IgnoreArguments().Throw(new InvalidOperationException());
            gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
            //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
            //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
            gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

            var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
            alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Return(new Alcada { blAlterarSugestao = true });

            var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
            //gradeSugestaoService.Stub(s => s.ObterGradeSugestoesSeFechada(1, 1, 1, 1, DateTime.Now.ToMilitaryTime())).IgnoreArguments().Throw(new InvalidOperationException());
            gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

            var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
            alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

            var target = new SugestaoPedidoService(gateway,
                alcadaService,
                gradeSugestaoService,
                null,
                null,
                null,
                alcadaDetalheService);

            var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 1, qtdPackCompra = 1, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

            gateway.VerifyAllExpectations();
            alcadaService.VerifyAllExpectations();
            gradeSugestaoService.VerifyAllExpectations();

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Total);
            Assert.AreEqual(1, response.Inexistentes);
            Assert.AreEqual(0, response.Sucesso);
            Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
            Assert.AreEqual(0, response.NaoSalvaPercentualAlteracao);
        }

        [Test]
        public void AlterarSugestoesPedido_SugestaoPedidoItensDiferentesTpPedidoMinimoMoney_NaoSalvaGradeSugestao()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 24,
                    vlEstoque = 12.789m,
                    vlFatorConversao = 1,
                    qtdPackCompra = 3,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                });
                gateway.Stub(s => s.ObterEstruturado(2)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(null);
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Return(new Alcada { blAlterarSugestao = true });

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                //gradeSugestaoService.Expect(s => s.ObterGradeSugestoesSeFechada(1, 1, 1, 1, DateTime.Now.ToMilitaryTime())).Return(new GradeSugestao { IDGradeSugestao = 1, cdSistema = 1, IDDepartamento = 1, IDLoja = 1, IDBandeira = 1, vlHoraInicial = DateTime.Now.AddMinutes(-10).ToMilitaryTime(), vlHoraFinal = DateTime.Now.AddMinutes(10).ToMilitaryTime() });
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(false);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    null,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 1, qtdPackCompra = 1, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                gateway.VerifyAllExpectations();
                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(0, response.Sucesso);
                Assert.AreEqual(1, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(0, response.NaoSalvaPercentualAlteracao);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AlterarSugestoesPedido_DataDiferenteDeHoje_NaoSalva()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 24,
                    vlEstoque = 12.789m,
                    vlFatorConversao = 1,
                    qtdPackCompra = 3,
                    dtPedido = DateTime.Today.AddDays(-1),
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                });

                gateway.Stub(s => s.ObterEstruturado(2)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(null);
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    null,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 1, qtdPackCompra = 1, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                gateway.VerifyAllExpectations();
                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(0, response.Sucesso);
                Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(1, response.NaoSalvaPercentualAlteracao);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AlterarSugestoesPedido_AlteracaoVlEstoqueForaDaAlcada_NaoSalvaPercentualAlteracao()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var sugestaoPedido1 = new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    vlForecast = 10,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 24,
                    vlEstoque = 12.1m,
                    vlFatorConversao = 1,
                    qtdPackCompra = 3,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 1,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                };

                var sugestaoPedido2 = new SugestaoPedido
                {
                    IDSugestaoPedido = 2,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 24,
                    vlEstoque = 12.789m,
                    vlFatorConversao = 1,
                    qtdPackCompra = 3,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 2,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 2,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                };

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(sugestaoPedido1);
                gateway.Stub(s => s.ObterEstruturado(3)).IgnoreArguments().Throw(new InvalidOperationException());
                //gateway.Expect(g => g.PesquisarPorFornecedorParametroELoja(1, 1)).Return(new SugestaoPedidoModel[] { new SugestaoPedidoModel(sugestaoPedido1), new SugestaoPedidoModel(sugestaoPedido2) });
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(new Alcada { IDAlcada = 1, blAlterarInformacaoEstoque = true, blAlterarSugestao = true, blZerarItem = true, IDPerfil = 1, vlPercentualAlterado = 1 });
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    null,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 6, qtdPackCompra = 1, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(0, response.Sucesso);
                Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(1, response.NaoSalvaPercentualAlteracao);

                gateway.VerifyAllExpectations();
                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AlterarSugestoesPedido_AlteracaoQtdPackCompraForaDaAlcada_NaoSalvaPercentualAlteracao()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var sugestaoPedido1 = new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 24,
                    vlEstoque = 12m,
                    vlFatorConversao = 1,
                    qtdPackCompra = 3,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 1,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                };

                var sugestaoPedido2 = new SugestaoPedido
                {
                    IDSugestaoPedido = 2,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 24,
                    vlEstoque = 12.789m,
                    vlFatorConversao = 1,
                    qtdPackCompra = 3,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 2,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 2,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                };

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(sugestaoPedido1);
                //gateway.Expect(g => g.ObterEstruturado(2)).Return(sugestaoPedido2);
                gateway.Stub(s => s.ObterEstruturado(3)).IgnoreArguments().Throw(new InvalidOperationException());
                //gateway.Expect(g => g.PesquisarPorFornecedorParametroELoja(1, 1)).Return(new SugestaoPedidoModel[] { new SugestaoPedidoModel(sugestaoPedido1), new SugestaoPedidoModel(sugestaoPedido2) });
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(new Alcada { IDAlcada = 1, blAlterarInformacaoEstoque = true, blAlterarSugestao = true, blZerarItem = true, IDPerfil = 1, vlPercentualAlterado = 1 });
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    null,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 12, qtdPackCompra = 6, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(0, response.Sucesso);
                Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(1, response.NaoSalvaPercentualAlteracao);

                gateway.VerifyAllExpectations();
                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AlterarSugestoesPedido_AlteracaoQtdPackCompraForaDaAlcadaPegandoAlcadaDetalhe_NaoSalvaPercentualAlteracao()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var sugestaoPedido1 = new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 24,
                    vlEstoque = 12m,
                    vlFatorConversao = 1,
                    qtdPackCompra = 3,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 1,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1, IdRegiaoAdministrativa = 1 }
                };

                var sugestaoPedido2 = new SugestaoPedido
                {
                    IDSugestaoPedido = 2,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 24,
                    vlEstoque = 12.789m,
                    vlFatorConversao = 1,
                    qtdPackCompra = 3,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 2,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 2,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1, IdRegiaoAdministrativa = 1 }
                };

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(sugestaoPedido1);
                //gateway.Expect(g => g.ObterEstruturado(2)).Return(sugestaoPedido2);
                gateway.Stub(s => s.ObterEstruturado(3)).IgnoreArguments().Throw(new InvalidOperationException());
                //gateway.Expect(g => g.PesquisarPorFornecedorParametroELoja(1, 1)).Return(new SugestaoPedidoModel[] { new SugestaoPedidoModel(sugestaoPedido1), new SugestaoPedidoModel(sugestaoPedido2) });
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(new Alcada { IDAlcada = 1, blAlterarInformacaoEstoque = true, blAlterarSugestao = true, blZerarItem = true, IDPerfil = 1, vlPercentualAlterado = 20 });
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadasDetalhesReturn = new List<AlcadaDetalhe>();
                alcadasDetalhesReturn.Add(new AlcadaDetalhe() { IDAlcada = 1, IDBandeira = 1, IDDepartamento = 1, IDRegiaoAdministrativa = 1, vlPercentualAlterado = 2 });

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(alcadasDetalhesReturn);

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    null,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 12, qtdPackCompra = 6, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(0, response.Sucesso);
                Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(1, response.NaoSalvaPercentualAlteracao);

                gateway.VerifyAllExpectations();
                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AlterarSugestoesPedido_QtdPackCompraAlterado_Sucesso()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var sugestaoPedido1 = new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 1,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                };

                var sugestaoPedido2 = new SugestaoPedido
                {
                    IDSugestaoPedido = 2,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 2,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 2,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                };

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(sugestaoPedido1);
                //gateway.Expect(g => g.ObterEstruturado(2)).Return(sugestaoPedido2);
                gateway.Stub(s => s.ObterEstruturado(3)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.PesquisarPorFornecedorParametroELoja(1, 1)).Return(new SugestaoPedidoModel[] { new SugestaoPedidoModel(sugestaoPedido1), new SugestaoPedidoModel(sugestaoPedido2) });
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(new Alcada { IDAlcada = 1, blAlterarInformacaoEstoque = true, blAlterarSugestao = true, blZerarItem = true, IDPerfil = 1, vlPercentualAlterado = 50, blAlterarPercentual = true });
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var auditService = MockRepository.GenerateMock<IAuditService>();
                auditService.Expect(e => e.LogUpdate<SugestaoPedido>(null, "qtdPackCompra", "vlEstoque")).IgnoreArguments();

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    auditService,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 12, qtdPackCompra = 4, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();
                auditService.VerifyAllExpectations();

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(1, response.Sucesso);
                Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(0, response.NaoSalvaPercentualAlteracao);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AlterarSugestoesPedido_VlEstoqueAlterado_Sucesso()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var sugestaoPedido1 = new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 3m,
                    vlForecast = 3m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 1m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 1,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                };

                var sugestaoPedido2 = new SugestaoPedido
                {
                    IDSugestaoPedido = 2,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 3m,
                    vlForecast = 3m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 2,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 2, CdSistema = 1 },
                    IDItemDetalheSugestao = 2,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 2, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                };

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(sugestaoPedido1);
                //gateway.Expect(g => g.ObterEstruturado(2)).Return(sugestaoPedido2);
                gateway.Stub(s => s.ObterEstruturado(3)).IgnoreArguments().Throw(new InvalidOperationException());
                //gateway.Expect(g => g.PesquisarPorFornecedorParametroELoja(1, 1)).Return(new SugestaoPedidoModel[] { new SugestaoPedidoModel(sugestaoPedido1), new SugestaoPedidoModel(sugestaoPedido2) });
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(new Alcada { IDAlcada = 1, blAlterarInformacaoEstoque = true, blAlterarSugestao = true, blZerarItem = true, IDPerfil = 1, vlPercentualAlterado = 50, blAlterarPercentual = true });
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                //gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 2, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(2), Is.Equal(1), Is.Anything()).Return(false);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var auditService = MockRepository.GenerateMock<IAuditService>();
                auditService.Expect(e => e.LogUpdate<SugestaoPedido>(null, "qtdPackCompra", "vlEstoque")).IgnoreArguments();

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    auditService,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 2, qtdPackCompra = 4, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(1, response.Sucesso);
                Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(0, response.NaoSalvaPercentualAlteracao);

                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();
                auditService.VerifyAllExpectations();
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AlterarSugestoesPedido_QtdPackCompraAlteradoPegandoAlcadaDetalhe_Sucesso()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var sugestaoPedido1 = new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 1,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1, IdRegiaoAdministrativa = 1 }
                };

                var sugestaoPedido2 = new SugestaoPedido
                {
                    IDSugestaoPedido = 2,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 2,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 2,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1, IdRegiaoAdministrativa = 1 }
                };

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(sugestaoPedido1);
                //gateway.Expect(g => g.ObterEstruturado(2)).Return(sugestaoPedido2);
                gateway.Stub(s => s.ObterEstruturado(3)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.PesquisarPorFornecedorParametroELoja(1, 1)).Return(new SugestaoPedidoModel[] { new SugestaoPedidoModel(sugestaoPedido1), new SugestaoPedidoModel(sugestaoPedido2) });
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(new Alcada { IDAlcada = 1, blAlterarInformacaoEstoque = true, blAlterarSugestao = true, blZerarItem = true, IDPerfil = 1, vlPercentualAlterado = 2, blAlterarPercentual = true });
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var auditService = MockRepository.GenerateMock<IAuditService>();
                auditService.Expect(e => e.LogUpdate<SugestaoPedido>(null, "qtdPackCompra", "vlEstoque")).IgnoreArguments();

                var alcadasDetalhesReturn = new List<AlcadaDetalhe>();
                alcadasDetalhesReturn.Add(new AlcadaDetalhe() { IDAlcada = 1, IDBandeira = 1, IDDepartamento = 1, IDRegiaoAdministrativa = 1, vlPercentualAlterado = 50 });
                
                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(alcadasDetalhesReturn);
                
                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    auditService,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 12, qtdPackCompra = 4, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();
                auditService.VerifyAllExpectations();

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(1, response.Sucesso);
                Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(0, response.NaoSalvaPercentualAlteracao);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AlterarSugestoesPedido_QtdPackCompraAlteradoIgnorandoAlcadaDetalheComIDRegiaoAdministrativaDiferente_Sucesso()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var sugestaoPedido1 = new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 1,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1, IdRegiaoAdministrativa = 1 }
                };

                var sugestaoPedido2 = new SugestaoPedido
                {
                    IDSugestaoPedido = 2,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 2,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 2,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1, IdRegiaoAdministrativa = 1 }
                };

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(sugestaoPedido1);
                //gateway.Expect(g => g.ObterEstruturado(2)).Return(sugestaoPedido2);
                gateway.Stub(s => s.ObterEstruturado(3)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.PesquisarPorFornecedorParametroELoja(1, 1)).Return(new SugestaoPedidoModel[] { new SugestaoPedidoModel(sugestaoPedido1), new SugestaoPedidoModel(sugestaoPedido2) });
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(new Alcada { IDAlcada = 1, blAlterarInformacaoEstoque = true, blAlterarSugestao = true, blZerarItem = true, IDPerfil = 1, vlPercentualAlterado = 50, blAlterarPercentual = true });
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var auditService = MockRepository.GenerateMock<IAuditService>();
                auditService.Expect(e => e.LogUpdate<SugestaoPedido>(null, "qtdPackCompra", "vlEstoque")).IgnoreArguments();

                var alcadasDetalhesReturn = new List<AlcadaDetalhe>();
                alcadasDetalhesReturn.Add(new AlcadaDetalhe() { IDAlcada = 1, IDBandeira = 1, IDDepartamento = 1, IDRegiaoAdministrativa = 2, vlPercentualAlterado = 2 });

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(alcadasDetalhesReturn);

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    auditService,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 12, qtdPackCompra = 4, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();
                auditService.VerifyAllExpectations();

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(1, response.Sucesso);
                Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(0, response.NaoSalvaPercentualAlteracao);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AlterarSugestoesPedido_QtdPackCompraAlteradoIgnorandoAlcadaDetalheComIDBandeiraDiferente_Sucesso()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var sugestaoPedido1 = new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 1,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1, IdRegiaoAdministrativa = 1 }
                };

                var sugestaoPedido2 = new SugestaoPedido
                {
                    IDSugestaoPedido = 2,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 2,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 2,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1, IdRegiaoAdministrativa = 1 }
                };

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(sugestaoPedido1);
                //gateway.Expect(g => g.ObterEstruturado(2)).Return(sugestaoPedido2);
                gateway.Stub(s => s.ObterEstruturado(3)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.PesquisarPorFornecedorParametroELoja(1, 1)).Return(new SugestaoPedidoModel[] { new SugestaoPedidoModel(sugestaoPedido1), new SugestaoPedidoModel(sugestaoPedido2) });
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(new Alcada { IDAlcada = 1, blAlterarInformacaoEstoque = true, blAlterarSugestao = true, blZerarItem = true, IDPerfil = 1, vlPercentualAlterado = 50, blAlterarPercentual = true });
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var auditService = MockRepository.GenerateMock<IAuditService>();
                auditService.Expect(e => e.LogUpdate<SugestaoPedido>(null, "qtdPackCompra", "vlEstoque")).IgnoreArguments();

                var alcadasDetalhesReturn = new List<AlcadaDetalhe>();
                alcadasDetalhesReturn.Add(new AlcadaDetalhe() { IDAlcada = 1, IDBandeira = 2, IDDepartamento = 1, IDRegiaoAdministrativa = 1, vlPercentualAlterado = 2 });

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(alcadasDetalhesReturn);

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    auditService,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 12, qtdPackCompra = 4, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();
                auditService.VerifyAllExpectations();

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(1, response.Sucesso);
                Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(0, response.NaoSalvaPercentualAlteracao);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AlterarSugestoesPedido_QtdPackCompraAlteradoIgnorandoAlcadaDetalheComIDDepartamentoDiferente_Sucesso()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var sugestaoPedido1 = new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 1,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1, IdRegiaoAdministrativa = 1 }
                };

                var sugestaoPedido2 = new SugestaoPedido
                {
                    IDSugestaoPedido = 2,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 2,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 2,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1, IdRegiaoAdministrativa = 1 }
                };

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(sugestaoPedido1);
                //gateway.Expect(g => g.ObterEstruturado(2)).Return(sugestaoPedido2);
                gateway.Stub(s => s.ObterEstruturado(3)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.PesquisarPorFornecedorParametroELoja(1, 1)).Return(new SugestaoPedidoModel[] { new SugestaoPedidoModel(sugestaoPedido1), new SugestaoPedidoModel(sugestaoPedido2) });
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(new Alcada { IDAlcada = 1, blAlterarInformacaoEstoque = true, blAlterarSugestao = true, blZerarItem = true, IDPerfil = 1, vlPercentualAlterado = 50, blAlterarPercentual = true });
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var auditService = MockRepository.GenerateMock<IAuditService>();
                auditService.Expect(e => e.LogUpdate<SugestaoPedido>(null, "qtdPackCompra", "vlEstoque")).IgnoreArguments();

                var alcadasDetalhesReturn = new List<AlcadaDetalhe>();
                alcadasDetalhesReturn.Add(new AlcadaDetalhe() { IDAlcada = 1, IDBandeira = 1, IDDepartamento = 2, IDRegiaoAdministrativa = 1, vlPercentualAlterado = 2 });

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(alcadasDetalhesReturn);

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    auditService,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 12, qtdPackCompra = 4, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();
                auditService.VerifyAllExpectations();

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(1, response.Sucesso);
                Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(0, response.NaoSalvaPercentualAlteracao);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AlterarSugestoesPedido_QtdPackCompraAlteradoSemAlcadaDetalheCompativelComAlcada_Sucesso()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var sugestaoPedido1 = new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 1,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1, IdRegiaoAdministrativa = 1 }
                };

                var sugestaoPedido2 = new SugestaoPedido
                {
                    IDSugestaoPedido = 2,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 2,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 2,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1, IdRegiaoAdministrativa = 1 }
                };

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(sugestaoPedido1);
                //gateway.Expect(g => g.ObterEstruturado(2)).Return(sugestaoPedido2);
                gateway.Stub(s => s.ObterEstruturado(3)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.PesquisarPorFornecedorParametroELoja(1, 1)).Return(new SugestaoPedidoModel[] { new SugestaoPedidoModel(sugestaoPedido1), new SugestaoPedidoModel(sugestaoPedido2) });
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(new Alcada { IDAlcada = 1, blAlterarInformacaoEstoque = true, blAlterarSugestao = true, blZerarItem = true, IDPerfil = 1, vlPercentualAlterado = 50, blAlterarPercentual = true });
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var auditService = MockRepository.GenerateMock<IAuditService>();
                auditService.Expect(e => e.LogUpdate<SugestaoPedido>(null, "qtdPackCompra", "vlEstoque")).IgnoreArguments();

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(2, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    auditService,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 12, qtdPackCompra = 4, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();
                auditService.VerifyAllExpectations();

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(1, response.Sucesso);
                Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(0, response.NaoSalvaPercentualAlteracao);
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }


        [Test]
        public void AlterarSugestoesPedido_VlEstoqueAlteradoOrigemDiferenteDeSgp_Sucesso()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var sugestaoPedido1 = new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Grs,
                    vlForecastMedio = 3m,
                    vlForecast = 3m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 1m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 1,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                };

                var sugestaoPedido2 = new SugestaoPedido
                {
                    IDSugestaoPedido = 2,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 3m,
                    vlForecast = 3m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 2,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 2, CdSistema = 1 },
                    IDItemDetalheSugestao = 2,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 2, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                };

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(sugestaoPedido1);
                //gateway.Expect(g => g.ObterEstruturado(2)).Return(sugestaoPedido2);
                gateway.Stub(s => s.ObterEstruturado(3)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.PesquisarPorFornecedorParametroELoja(1, 1)).Return(new SugestaoPedidoModel[] { new SugestaoPedidoModel(sugestaoPedido1), new SugestaoPedidoModel(sugestaoPedido2) });
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(new Alcada { IDAlcada = 1, blAlterarInformacaoEstoque = true, blAlterarSugestao = true, blZerarItem = true, IDPerfil = 1, vlPercentualAlterado = 50, blAlterarPercentual = true });
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                //gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 2, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(2), Is.Equal(1), Is.Anything()).Return(false);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var auditService = MockRepository.GenerateMock<IAuditService>();
                auditService.Expect(e => e.LogUpdate<SugestaoPedido>(null, "qtdPackCompra", "vlEstoque")).IgnoreArguments();

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    auditService,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 2, qtdPackCompra = 4, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(1, response.Sucesso);
                Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(0, response.NaoSalvaPercentualAlteracao);

                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();
                auditService.VerifyAllExpectations();
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void ObterStatusAutorizarPedido_LojaNaoPermite_L()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();

            lojaService.Expect(s => s.ObterPorId(1)).Return(new Loja { blAutorizaPedido = false });

            var target = new SugestaoPedidoService(null, null, null, lojaService, null, null, null);

            var result = target.ObterStatusAutorizarPedido(DateTime.Today, 1, 1, 1);

            lojaService.VerifyAllExpectations();

            Assert.AreEqual("L", result);
        }

        [Test]
        public void ObterStatusAutorizarPedido_DataDiferenteHoje_D()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();

            lojaService.Expect(s => s.ObterPorId(1)).Return(new Loja { blAutorizaPedido = true });

            var autorizaPedidoService = MockRepository.GenerateMock<IAutorizaPedidoService>();
            autorizaPedidoService.Expect(s => s.ExisteAutorizacaoPedido(DateTime.Today, 1, 1)).Return(false);

            var target = new SugestaoPedidoService(null, null, null, lojaService, autorizaPedidoService, null, null);

            var result = target.ObterStatusAutorizarPedido(DateTime.Today.AddDays(-3), 1, 1, 1);

            lojaService.VerifyAllExpectations();

            Assert.AreEqual("D", result);
        }

        [Test]
        public void ObterStatusAutorizarPedido_GradeFechada_D()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();

            lojaService.Expect(s => s.ObterPorId(1)).Return(new Loja { blAutorizaPedido = true, IDLoja = 1, IDBandeira = 1 });

            var autorizaPedidoService = MockRepository.GenerateMock<IAutorizaPedidoService>();
            autorizaPedidoService.Expect(s => s.ExisteAutorizacaoPedido(DateTime.Today, 1, 1)).Return(false);

            var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
            gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(false);
            gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

            var target = new SugestaoPedidoService(null, null, gradeSugestaoService, lojaService, autorizaPedidoService, null, null);

            var result = target.ObterStatusAutorizarPedido(DateTime.Today, 1, 1, 1);

            gradeSugestaoService.VerifyAllExpectations();
            lojaService.VerifyAllExpectations();

            Assert.AreEqual("D", result);
        }

        [Test]
        public void ObterStatusAutorizarPedido_AutorizacaoExiste_A()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();

            lojaService.Expect(s => s.ObterPorId(1)).Return(new Loja { blAutorizaPedido = true, IDLoja = 1, IDBandeira = 1 });

            var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
            gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

            var autorizaPedidoService = MockRepository.GenerateMock<IAutorizaPedidoService>();
            autorizaPedidoService.Expect(s => s.ExisteAutorizacaoPedido(DateTime.Today, 1, 1)).Return(true);

            var target = new SugestaoPedidoService(null, null, gradeSugestaoService, lojaService, autorizaPedidoService, null, null);

            var result = target.ObterStatusAutorizarPedido(DateTime.Today, 1, 1, 1);

            autorizaPedidoService.VerifyAllExpectations();
            gradeSugestaoService.VerifyAllExpectations();
            lojaService.VerifyAllExpectations();

            Assert.AreEqual("A", result);
        }

        [Test]
        public void ObterStatusAutorizarPedido_NaoExisteAutorizacao_P()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();

            lojaService.Expect(s => s.ObterPorId(1)).Return(new Loja { blAutorizaPedido = true, IDLoja = 1, IDBandeira = 1 });

            var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
            gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
            gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

            var autorizaPedidoService = MockRepository.GenerateMock<IAutorizaPedidoService>();
            autorizaPedidoService.Expect(s => s.ExisteAutorizacaoPedido(DateTime.Today, 1, 1)).Return(false);

            var target = new SugestaoPedidoService(null, null, gradeSugestaoService, lojaService, autorizaPedidoService, null, null);

            var result = target.ObterStatusAutorizarPedido(DateTime.Today, 1, 1, 1);

            autorizaPedidoService.VerifyAllExpectations();
            gradeSugestaoService.VerifyAllExpectations();
            lojaService.VerifyAllExpectations();

            Assert.AreEqual("P", result);
        }

        [Test]
        public void AutorizarPedido_NaoExisteAutorizacao_A()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();

            lojaService.Expect(s => s.ObterPorId(1)).Return(new Loja { blAutorizaPedido = true, IDLoja = 1, IDBandeira = 1 });

            var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
            gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
            gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

            var autorizaPedidoService = MockRepository.GenerateMock<IAutorizaPedidoService>();
            autorizaPedidoService.Expect(s => s.ExisteAutorizacaoPedido(DateTime.Today, 1, 1)).Return(false);

            autorizaPedidoService.Expect(s => s.AutorizarPedido(DateTime.Today, 1, 1));
            autorizaPedidoService.Stub(s => s.AutorizarPedido(DateTime.Today, 1, 1)).Throw(new InvalidOperationException());

            var target = new SugestaoPedidoService(null, null, gradeSugestaoService, lojaService, autorizaPedidoService, null, null);

            var result = target.AutorizarPedido(DateTime.Today, 1, 1, 1);

            autorizaPedidoService.VerifyAllExpectations();
            gradeSugestaoService.VerifyAllExpectations();
            lojaService.VerifyAllExpectations();

            Assert.AreEqual("A", result);
        }

        [Test]
        public void AutorizarPedido_LojaNaoPermite_L()
        {
            var lojaService = MockRepository.GenerateMock<ILojaService>();

            lojaService.Expect(s => s.ObterPorId(1)).Return(new Loja { blAutorizaPedido = false, IDLoja = 1, IDBandeira = 1 });

            var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
            gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

            var autorizaPedidoService = MockRepository.GenerateMock<IAutorizaPedidoService>();
            autorizaPedidoService.Stub(s => s.ExisteAutorizacaoPedido(DateTime.Today, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

            ////autorizaPedidoService.Expect(s => s.AutorizarPedido(DateTime.Today, 1, 1));
            autorizaPedidoService.Stub(s => s.AutorizarPedido(DateTime.Today, 1, 1)).Throw(new InvalidOperationException());

            var target = new SugestaoPedidoService(null, null, gradeSugestaoService, lojaService, autorizaPedidoService, null, null);

            var result = target.AutorizarPedido(DateTime.Today, 1, 1, 1);

            autorizaPedidoService.VerifyAllExpectations();
            gradeSugestaoService.VerifyAllExpectations();
            lojaService.VerifyAllExpectations();

            Assert.AreEqual("L", result);
        }

        [Test]
        public void ObterLogs_Filtro_Logs()
        {
            var paging = new Paging();
            var auditService = MockRepository.GenerateMock<IAuditService>();
            auditService.Expect(e => e.ObterRelatorio<SugestaoPedido>(new String[] { "vlEstoque", "qtdPackCompra", "qtdSugestaoRoteiroRA" }, null, 1, null, null, paging)).Return(
                new AuditRecord<SugestaoPedido>[] {
                    new AuditRecord<SugestaoPedido>(null, AuditKind.Update),
                    new AuditRecord<SugestaoPedido>(null, AuditKind.Update),
                });

            var target = new SugestaoPedidoService(null, null, null, null, null, auditService, null);

            var result = target.ObterLogs(new AuditFilter { IdEntidade = 1 }, paging);
            Assert.AreEqual(2, result.Count());
            auditService.VerifyAllExpectations();
        }

        [Test]
        public void ObterQuantidade_SugestoesPedido_Quantidades()
        {
            var repository = new MemorySugestaoPedidoGateway();
            repository.Insert(new SugestaoPedido { dtPedido = DateTime.Today, cdOrigemCalculo = TipoOrigemCalculo.Manual });
            repository.Insert(new SugestaoPedido { dtPedido = DateTime.Today, cdOrigemCalculo = TipoOrigemCalculo.Manual });
            repository.Insert(new SugestaoPedido { dtPedido = DateTime.Today.AddDays(-1), cdOrigemCalculo = TipoOrigemCalculo.Manual });
            repository.Insert(new SugestaoPedido { dtPedido = DateTime.Today, cdOrigemCalculo = TipoOrigemCalculo.Sgp });
            repository.Insert(new SugestaoPedido { dtPedido = DateTime.Today, cdOrigemCalculo = TipoOrigemCalculo.Sgp });
            repository.Insert(new SugestaoPedido { dtPedido = DateTime.Today, cdOrigemCalculo = TipoOrigemCalculo.Sgp });
            repository.Insert(new SugestaoPedido { dtPedido = DateTime.Today.AddDays(-1), cdOrigemCalculo = TipoOrigemCalculo.Sgp });
            repository.Insert(new SugestaoPedido { dtPedido = DateTime.Today, cdOrigemCalculo = TipoOrigemCalculo.Grs });
            var target = new SugestaoPedidoService(repository, null, null, null, null, null, null);
            var actual = target.ObterQuantidade(DateTime.Now);
            Assert.AreEqual(2, actual.TotalOrigemCalculoManual);
            Assert.AreEqual(4, actual.TotalOrigemCalculoNaoManual);
            Assert.AreEqual(6, actual.Total);
        }

        [Test]
        public void AlterarSugestoesPedido_ItemXDock_Sucesso()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var sugestaoPedido1 = new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 1,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 },
                    idCD = 9,
                    vlTipoReabastecimento = ValorTipoReabastecimento.CrossDocking3
                };

                var sugestaoPedido2 = new SugestaoPedido
                {
                    IDSugestaoPedido = 2,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 2,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 2,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                };

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(sugestaoPedido1);
                //gateway.Expect(g => g.ObterEstruturado(2)).Return(sugestaoPedido2);
                gateway.Stub(s => s.ObterEstruturado(3)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.PesquisarPorFornecedorParametroELoja(1, 1)).Return(new SugestaoPedidoModel[] { new SugestaoPedidoModel(sugestaoPedido1), new SugestaoPedidoModel(sugestaoPedido2) });
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                gateway.Expect(g => g.ConsolidarPedidoMinimo(1, 1, DateTime.Today, true));
                gateway.Expect(g => g.ConsolidarPedidoMinimoXDoc(DateTime.Today, 9, 1));

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(new Alcada { IDAlcada = 1, blAlterarInformacaoEstoque = true, blAlterarSugestao = true, blZerarItem = true, IDPerfil = 1, vlPercentualAlterado = 50, blAlterarPercentual = true });
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var fornecedorParametroService = MockRepository.GenerateMock<IFornecedorParametroService>();
                fornecedorParametroService.Stub(s => s.ObterPorId(1)).IgnoreArguments().Throw(new InvalidOperationException());

                var auditService = MockRepository.GenerateMock<IAuditService>();
                auditService.Expect(e => e.LogUpdate<SugestaoPedido>(null, "qtdPackCompra", "vlEstoque")).IgnoreArguments();

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    auditService,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 12, qtdPackCompra = 4, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(1, response.Sucesso);
                Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(0, response.NaoSalvaPercentualAlteracao);

                // TODO: Corrigir as expectativas.
                // gateway.VerifyAllExpectations(); 
                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();
                fornecedorParametroService.VerifyAllExpectations();
                auditService.VerifyAllExpectations();
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }

        [Test]
        public void AlterarSugestoesPedido_ItemDiferenteXDock_Sucesso()
        {
            var currentContext = RuntimeContext.Current;

            RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 1, RoleName = "Guest", RoleId = 1 } };

            try
            {
                var sugestaoPedido1 = new SugestaoPedido
                {
                    IDSugestaoPedido = 1,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 1,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 1,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 1, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 },
                    idCD = 9,
                    vlTipoReabastecimento = ValorTipoReabastecimento.Dsd37
                };

                var sugestaoPedido2 = new SugestaoPedido
                {
                    IDSugestaoPedido = 2,
                    IDFornecedorParametro = 1,
                    cdOrigemCalculo = TipoOrigemCalculo.Sgp,
                    vlForecastMedio = 12.345m,
                    dtInicioForecast = DateTime.Today.AddDays(-3),
                    dtFimForecast = DateTime.Today,
                    qtVendorPackage = 1,
                    vlModulo = 2m,
                    vlEstoqueOriginal = -2,
                    qtdPackCompraOriginal = 3,
                    vlEstoque = 12m,
                    qtdPackCompra = 3,
                    vlFatorConversao = 1,
                    dtPedido = DateTime.Today,
                    IDItemDetalhePedido = 2,
                    ItemDetalhePedido = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IDItemDetalheSugestao = 2,
                    ItemDetalheSugestao = new ItemDetalhe { IDItemDetalhe = 2, IDDepartamento = 1, CdSistema = 1 },
                    IdLoja = 1,
                    Loja = new Loja { IDLoja = 1, IDBandeira = 1 }
                };

                var gateway = MockRepository.GenerateMock<ISugestaoPedidoGateway>();
                gateway.Expect(g => g.ObterEstruturado(1)).Return(sugestaoPedido1);
                //gateway.Expect(g => g.ObterEstruturado(2)).Return(sugestaoPedido2);
                gateway.Stub(s => s.ObterEstruturado(3)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.PesquisarPorFornecedorParametroELoja(1, 1)).Return(new SugestaoPedidoModel[] { new SugestaoPedidoModel(sugestaoPedido1), new SugestaoPedidoModel(sugestaoPedido2) });
                gateway.Stub(s => s.PesquisarPorFornecedorParametroELoja(1, 1)).IgnoreArguments().Throw(new InvalidOperationException());
                gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).Return(1);
                //gateway.Expect(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 2)).Return(1);
                gateway.Stub(g => g.ContarPorDataPedidoLojaEDepartamento(DateTime.Today, 1, 1, 1)).IgnoreArguments().Throw(new InvalidOperationException());

                gateway.Expect(g => g.ConsolidarPedidoMinimo(1, 1, DateTime.Today, true));
                gateway.Expect(g => g.ConsolidarPedidoMinimoXDoc(DateTime.Today, 9, 1));

                var alcadaService = MockRepository.GenerateMock<IAlcadaService>();
                alcadaService.Expect(s => s.ObterPorPerfil(1)).Return(new Alcada { IDAlcada = 1, blAlterarInformacaoEstoque = true, blAlterarSugestao = true, blZerarItem = true, IDPerfil = 1, vlPercentualAlterado = 50, blAlterarPercentual = true });
                alcadaService.Stub(s => s.ObterPorPerfil(2)).IgnoreArguments().Throw(new InvalidOperationException());

                var gradeSugestaoService = MockRepository.GenerateMock<IGradeSugestaoService>();
                gradeSugestaoService.Expect(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).Constraints(Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Equal(1), Is.Anything()).Return(true);
                gradeSugestaoService.Stub(s => s.ExisteGradeSugestaoAberta(1, 1, 1, 1, 0)).IgnoreArguments().Throw(new InvalidOperationException());

                var fornecedorParametroService = MockRepository.GenerateMock<IFornecedorParametroService>();
                fornecedorParametroService.Stub(s => s.ObterPorId(1)).IgnoreArguments().Throw(new InvalidOperationException());

                var auditService = MockRepository.GenerateMock<IAuditService>();
                auditService.Expect(e => e.LogUpdate<SugestaoPedido>(null, "qtdPackCompra", "vlEstoque")).IgnoreArguments();

                var alcadaDetalheService = MockRepository.GenerateMock<IAlcadaDetalheService>();
                alcadaDetalheService.Expect(s => s.ObterPorIdAlcada(1, null)).Return(new List<AlcadaDetalhe>());

                var target = new SugestaoPedidoService(gateway,
                    alcadaService,
                    gradeSugestaoService,
                    null,
                    null,
                    auditService,
                    alcadaDetalheService);

                var response = target.AlterarSugestoes(new SugestaoPedidoModel[] { new SugestaoPedidoModel { IDSugestaoPedido = 1, vlEstoque = 12, qtdPackCompra = 4, IDFornecedorParametro = 1 } }, 1, DateTime.Today);

                Assert.IsNotNull(response);
                Assert.AreEqual(1, response.Total);
                Assert.AreEqual(0, response.Inexistentes);
                Assert.AreEqual(1, response.Sucesso);
                Assert.AreEqual(0, response.NaoSalvaGradeSugestao);
                Assert.AreEqual(0, response.NaoSalvaPercentualAlteracao);

                // TODO: Corrigir as expectativas.
                // gateway.VerifyAllExpectations(); 
                alcadaService.VerifyAllExpectations();
                gradeSugestaoService.VerifyAllExpectations();
                fornecedorParametroService.VerifyAllExpectations();
                auditService.VerifyAllExpectations();
            }
            finally
            {
                RuntimeContext.Current = currentContext;
            }
        }
    }
}