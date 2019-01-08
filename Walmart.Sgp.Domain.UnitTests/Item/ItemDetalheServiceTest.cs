using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Data.Memory;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;
using Is = Rhino.Mocks.Constraints.Is;
using System.Linq;
using System;
using Walmart.Sgp.Domain.Inventarios;

namespace Walmart.Sgp.Domain.UnitTests.Item
{
    [TestFixture]
    [Category("Domain")]
    [Category("Item")]
    public class ItemDetalheServiceTest
    {
        private IItemDetalheGateway m_itemDetalhe_gateway;
        private IItemDetalheHistGateway m_itemDetalheHistGateway;
        private IItemRelacionamentoGateway m_itemRelacionamentoGateway;
        private ItemDetalheService m_target;

        [SetUp]
        public void InitializeTest()
        {
            m_itemDetalhe_gateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            m_itemDetalheHistGateway = MockRepository.GenerateMock<IItemDetalheHistGateway>();
            m_itemRelacionamentoGateway = MockRepository.GenerateMock<IItemRelacionamentoGateway>();
            m_target = new ItemDetalheService(m_itemDetalhe_gateway, m_itemDetalheHistGateway, m_itemRelacionamentoGateway);
        }

        [Test]
        public void AlterarManipulado_IdETipoManipulado_Alterado()
        {
            var gateway = new MemoryItemDetalheGateway();
            var histGateway = new MemoryItemDetalheHistGateway();
            var target = new ItemDetalheService(gateway, histGateway, null);

            gateway.Insert(new ItemDetalhe { Id = 1, TpManipulado = TipoManipulado.NaoDefinido });
            gateway.Insert(new ItemDetalhe { Id = 2, TpManipulado = TipoManipulado.NaoDefinido });

            target.AlterarManipulado(1, TipoManipulado.Pai);
            Assert.AreEqual(1, gateway.Count("TpManipulado = @TipoManipulado", new { TipoManipulado = TipoManipulado.Pai }));
            Assert.AreEqual(1, gateway.Count("TpManipulado = @TipoManipulado", new { TipoManipulado = TipoManipulado.NaoDefinido }));
            Assert.AreEqual(2, gateway.Entities.Count);
            Assert.AreEqual(1, histGateway.Count("Id = @Id", new { Id = 1 }));
            Assert.AreEqual(1, histGateway.Entities.Count);

            target.AlterarManipulado(2, TipoManipulado.Derivado);
            Assert.AreEqual(1, gateway.Count("TpManipulado = @TipoManipulado", new { TipoManipulado = TipoManipulado.Pai }));
            Assert.AreEqual(1, gateway.Count("TpManipulado = @TipoManipulado", new { TipoManipulado = TipoManipulado.Derivado }));
            Assert.AreEqual(2, gateway.Entities.Count);
            Assert.AreEqual(1, histGateway.Count("Id = @Id", new { Id = 1 }));
            Assert.AreEqual(1, histGateway.Count("Id = @Id", new { Id = 2 }));
            Assert.AreEqual(2, histGateway.Entities.Count);
        }

        [Test]
        public void AlterarVinculado_IdETipoVinculado_Alterado()
        {
            var gateway = new MemoryItemDetalheGateway();
            var histGateway = new MemoryItemDetalheHistGateway();
            var target = new ItemDetalheService(gateway, histGateway, null);

            gateway.Insert(new ItemDetalhe { Id = 1, TpVinculado = TipoVinculado.NaoDefinido });
            gateway.Insert(new ItemDetalhe { Id = 2, TpVinculado = TipoVinculado.NaoDefinido });

            target.AlterarVinculado(1, TipoVinculado.Entrada);
            Assert.AreEqual(1, gateway.Count("TpVinculado = @TipoVinculado", new { TipoVinculado = TipoVinculado.Entrada }));
            Assert.AreEqual(1, gateway.Count("TpVinculado = @TipoVinculado", new { TipoVinculado = TipoVinculado.NaoDefinido }));
            Assert.AreEqual(2, gateway.Entities.Count);
            Assert.AreEqual(1, histGateway.Count("Id = @Id", new { Id = 1 }));
            Assert.AreEqual(1, histGateway.Entities.Count);

            target.AlterarVinculado(2, TipoVinculado.Saida);
            Assert.AreEqual(1, gateway.Count("TpVinculado = @TipoVinculado", new { TipoVinculado = TipoVinculado.Entrada }));
            Assert.AreEqual(1, gateway.Count("TpVinculado = @TipoVinculado", new { TipoVinculado = TipoVinculado.Saida }));
            Assert.AreEqual(2, gateway.Entities.Count);
            Assert.AreEqual(1, histGateway.Count("Id = @Id", new { Id = 1 }));
            Assert.AreEqual(1, histGateway.Count("Id = @Id", new { Id = 2 }));
            Assert.AreEqual(2, histGateway.Entities.Count);
        }

        [Test]
        public void AlterarReceituario_IdETipoReceituario_Alterado()
        {
            var gateway = new MemoryItemDetalheGateway();
            var histGateway = new MemoryItemDetalheHistGateway();
            var target = new ItemDetalheService(gateway, histGateway, null);

            gateway.Insert(new ItemDetalhe { Id = 1, TpReceituario = TipoReceituario.NaoDefinido });
            gateway.Insert(new ItemDetalhe { Id = 2, TpReceituario = TipoReceituario.NaoDefinido });

            target.AlterarReceituario(1, TipoReceituario.Transformado);
            Assert.AreEqual(1, gateway.Count("TpReceituario = @TipoReceituario", new { TipoReceituario = TipoReceituario.Transformado }));
            Assert.AreEqual(1, gateway.Count("TpReceituario = @TipoReceituario", new { TipoReceituario = TipoReceituario.NaoDefinido }));
            Assert.AreEqual(2, gateway.Entities.Count);
            Assert.AreEqual(1, histGateway.Count("Id = @Id", new { Id = 1 }));
            Assert.AreEqual(1, histGateway.Entities.Count);

            target.AlterarReceituario(2, TipoReceituario.Insumo);
            Assert.AreEqual(1, gateway.Count("TpReceituario = @TipoReceituario", new { TipoReceituario = TipoReceituario.Transformado }));
            Assert.AreEqual(1, gateway.Count("TpReceituario = @TipoReceituario", new { TipoReceituario = TipoReceituario.Insumo }));
            Assert.AreEqual(2, gateway.Entities.Count);
            Assert.AreEqual(1, histGateway.Count("Id = @Id", new { Id = 1 }));
            Assert.AreEqual(1, histGateway.Count("Id = @Id", new { Id = 2 }));
            Assert.AreEqual(2, histGateway.Entities.Count);
        }

        [Test]
        public void ObterEstruturadoPorId_Id_HierarquiaSemParametro()
        {
            var gateway = MockRepository.GenerateMock<IItemDetalheGateway>();

            gateway
                .Expect(a => a.ObterEstruturadoPorId(1))
                .Return(new ItemDetalhe
                {
                    Id = 1,
                    CdItem = 123,
                    CdSistema = 12
                });

            gateway
                .Expect(a => a.ObterFornecedorParametro(1))
                .Return(null);

            var target = new ItemDetalheService(gateway, null, null);

            var result = target.ObterEstruturadoPorId(1);

            gateway.VerifyAllExpectations();

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(result.Id, result.IDItemDetalhe);
            Assert.AreEqual(123, result.CdItem);
            Assert.AreEqual(12, result.CdSistema);

            Assert.IsNull(result.FornecedorParametro);
        }

        [Test]
        public void ObterEstruturadoPorId_Id_HierarquiaEParametro()
        {
            var gateway = MockRepository.GenerateMock<IItemDetalheGateway>();

            gateway
                .Expect(a => a.ObterEstruturadoPorId(1))
                .Return(new ItemDetalhe
                {
                    Id = 1,
                    CdItem = 123,
                    CdSistema = 12
                });

            gateway
                .Expect(a => a.ObterFornecedorParametro(1))
                .Return(new FornecedorParametro
                {
                    cdV9D = 012345678,
                    IDFornecedor = 1,
                    IDFornecedorParametro = 1
                });

            var target = new ItemDetalheService(gateway, null, null);

            var result = target.ObterEstruturadoPorId(1);

            gateway.VerifyAllExpectations();

            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(result.Id, result.IDItemDetalhe);
            Assert.AreEqual(123, result.CdItem);
            Assert.AreEqual(12, result.CdSistema);

            Assert.IsNotNull(result.FornecedorParametro);
            Assert.AreEqual(012345678, result.FornecedorParametro.cdV9D);
        }

        [Test]
        public void ObterEstruturadoPorId_IdInvalido_Hierarquia()
        {
            var gateway = MockRepository.GenerateMock<IItemDetalheGateway>();

            gateway.Expect(a => a.ObterEstruturadoPorId(1)).Return(null);

            var target = new ItemDetalheService(gateway, null, null);

            var result = target.ObterEstruturadoPorId(1);

            gateway.VerifyAllExpectations();

            Assert.IsNull(result);
        }

        [Test]
        public void AtualizarDadosCustos_ItemDetalheComDados_Atualizado()
        {
            var itemDetalhe = new ItemDetalhe { IDItemDetalhe = 1, CdSistema = 1, VlCustoUnitario = 2, TpUnidadeMedida = TipoUnidadeMedida.Quilo, VlFatorConversao = 1 };

            var gateway = MockRepository.GenerateMock<IItemDetalheGateway>();

            gateway.Expect(a => a.AlterarDadosCustos(itemDetalhe));

            var target = new ItemDetalheService(gateway, null, null);

            target.AlterarDadosCustos(itemDetalhe);

            gateway.VerifyAllExpectations();
        }

        [Test]
        public void AtualizarDadosCustos_ItemDetalheSemId_Assert()
        {
            var itemDetalhe = new ItemDetalhe { IDItemDetalhe = 0, CdSistema = 0, VlCustoUnitario = 2, TpUnidadeMedida = TipoUnidadeMedida.Quilo };

            var gateway = MockRepository.GenerateMock<IItemDetalheGateway>();

            var target = new ItemDetalheService(null, null, null);

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                target.AlterarDadosCustos(itemDetalhe);
            });
        }

        [Test]
        public void AtualizarInformacoesCadastrais_ItemRecebeNotaFatorConversaoIgualUm_Atualizado()
        {
            var gateway = new MemoryItemDetalheGateway();
            var histGateway = new MemoryItemDetalheHistGateway();
            var inserido = new ItemDetalhe
            {
                CdItem = 2,
                CdSistema = 1,
                VlFatorConversao = 1,
                BlItemTransferencia = false,
                TpUnidadeMedida = TipoUnidadeMedida.Quilo,
                TpManipulado = TipoManipulado.NaoDefinido,
                TpReceituario = TipoReceituario.NaoDefinido,
                TpVinculado = TipoVinculado.NaoDefinido
            };

            Assert.AreEqual(true, inserido.RecebeNota);

            gateway.Insert(inserido);
            var itemRelacionamentoGateway = MockRepository.GeneratePartialMock<MemoryItemRelacionamentoGateway>();
            var itemRelacionamento = new RelacionamentoItemPrincipal
            {
                cdSistema = 1,
                IDItemDetalhe = inserido.IDItemDetalhe,
                StatusReprocessamentoCusto = StatusReprocessamentoCusto.Nenhum
            };

            itemRelacionamentoGateway.Stub(t => t.ObterPrincipaisPorItem(1)).Return(new RelacionamentoItemPrincipal[] { itemRelacionamento });

            var target = new ItemDetalheService(gateway, histGateway, itemRelacionamentoGateway);
            target.AlterarInformacoesCadastrais(new ItemDetalhe
            {
                TpUnidadeMedida = TipoUnidadeMedida.Unidade,
                BlItemTransferencia = true,
                CdItem = 2,
                IDItemDetalhe = inserido.IDItemDetalhe,
                VlFatorConversao = 1,
                CdSistema = 1
            });

            Assert.AreEqual(1, gateway.Entities.Count);

            var atualizado = gateway.Entities[0];

            Assert.AreEqual(inserido.IDItemDetalhe, atualizado.IDItemDetalhe);
            Assert.AreEqual(true, atualizado.BlItemTransferencia);
            Assert.AreEqual(TipoUnidadeMedida.Unidade, atualizado.TpUnidadeMedida);
            Assert.AreEqual(1, atualizado.VlFatorConversao);

            Assert.AreEqual(StatusReprocessamentoCusto.Agendado, itemRelacionamento.StatusReprocessamentoCusto);
            Assert.AreEqual(false, itemRelacionamento.BlReprocessamentoManual);
            Assert.AreEqual(string.Empty, itemRelacionamento.DescErroReprocessamento);
            Assert.IsNull(itemRelacionamento.DtInicioReprocessamentoCusto);
            Assert.IsNull(itemRelacionamento.DtFinalReprocessamentoCusto);
            Assert.AreEqual(RuntimeContext.Current.User.Id, itemRelacionamento.IdUsuarioAlteracao);
        }

        [Test]
        public void AtualizarInformacoesCadastrais_ItemNaoRecebeNotaFatorConversaoIgualUm_Atualizado()
        {
            var gateway = new MemoryItemDetalheGateway();
            var histGateway = new MemoryItemDetalheHistGateway();
            var inserido = new ItemDetalhe
            {
                CdItem = 2,
                CdSistema = 1,
                VlFatorConversao = 1,
                BlItemTransferencia = false,
                TpUnidadeMedida = TipoUnidadeMedida.Quilo,
                TpManipulado = TipoManipulado.Derivado,
                TpReceituario = TipoReceituario.NaoDefinido,
                TpVinculado = TipoVinculado.NaoDefinido
            };

            Assert.AreEqual(false, inserido.RecebeNota);

            gateway.Insert(inserido);
            var itemRelacionamentoGateway = MockRepository.GeneratePartialMock<MemoryItemRelacionamentoGateway>();

            itemRelacionamentoGateway.AssertWasNotCalled(t => t.ObterPrincipaisPorItem(0));

            var target = new ItemDetalheService(gateway, histGateway, itemRelacionamentoGateway);
            target.AlterarInformacoesCadastrais(new ItemDetalhe
            {
                TpUnidadeMedida = TipoUnidadeMedida.Unidade,
                BlItemTransferencia = true,
                CdItem = 2,
                IDItemDetalhe = inserido.IDItemDetalhe,
                VlFatorConversao = 1,
                CdSistema = 1
            });

            Assert.AreEqual(1, gateway.Entities.Count);

            var atualizado = gateway.Entities[0];

            Assert.AreEqual(inserido.IDItemDetalhe, atualizado.IDItemDetalhe);
            Assert.AreEqual(true, atualizado.BlItemTransferencia);
            Assert.AreEqual(TipoUnidadeMedida.Unidade, atualizado.TpUnidadeMedida);
            Assert.AreEqual(1, atualizado.VlFatorConversao);

            itemRelacionamentoGateway.VerifyAllExpectations();
        }

        [Test]
        public void AtualizarInformacoesCadastrais_ItemRecebeNotaFatorConversaoVazio_Exception()
        {
            var gateway = new MemoryItemDetalheGateway();
            var inserido = new ItemDetalhe
            {
                CdItem = 2,
                CdSistema = 1,
                VlFatorConversao = 1,
                BlItemTransferencia = false,
                TpUnidadeMedida = TipoUnidadeMedida.Quilo,
                TpManipulado = TipoManipulado.NaoDefinido,
                TpReceituario = TipoReceituario.NaoDefinido,
                TpVinculado = TipoVinculado.NaoDefinido
            };

            Assert.AreEqual(true, inserido.RecebeNota);

            gateway.Insert(inserido);

            var target = new ItemDetalheService(gateway, null, null);
            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                target.AlterarInformacoesCadastrais(new ItemDetalhe
                {
                    TpUnidadeMedida = TipoUnidadeMedida.Unidade,
                    BlItemTransferencia = true,
                    CdItem = 2,
                    IDItemDetalhe = inserido.IDItemDetalhe,
                    VlFatorConversao = null,
                    CdSistema = 1
                });
            });


            Assert.AreEqual(1, gateway.Entities.Count);

            var naoAtualizado = gateway.Entities[0];

            Assert.AreEqual(inserido.IDItemDetalhe, naoAtualizado.IDItemDetalhe);
            Assert.AreEqual(inserido.BlItemTransferencia, naoAtualizado.BlItemTransferencia);
            Assert.AreEqual(inserido.TpUnidadeMedida, naoAtualizado.TpUnidadeMedida);
        }

        [Test]
        public void AtualizarInformacoesCadastrais_ItemNaoRecebeNotaFatorConversaoMaiorZero_Exception()
        {
            var gateway = new MemoryItemDetalheGateway();
            var inserido = new ItemDetalhe
            {
                CdItem = 2,
                CdSistema = 1,
                VlFatorConversao = 1,
                BlItemTransferencia = false,
                TpUnidadeMedida = TipoUnidadeMedida.Quilo,
                TpManipulado = TipoManipulado.Derivado,
                TpReceituario = TipoReceituario.NaoDefinido,
                TpVinculado = TipoVinculado.NaoDefinido
            };

            Assert.AreEqual(false, inserido.RecebeNota);

            gateway.Insert(inserido);

            var target = new ItemDetalheService(gateway, null, null);
            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                target.AlterarInformacoesCadastrais(new ItemDetalhe
                {
                    TpUnidadeMedida = TipoUnidadeMedida.Unidade,
                    BlItemTransferencia = true,
                    CdItem = 2,
                    IDItemDetalhe = inserido.IDItemDetalhe,
                    VlFatorConversao = 2,
                    CdSistema = 1
                });
            });


            Assert.AreEqual(1, gateway.Entities.Count);

            var naoAtualizado = gateway.Entities[0];

            Assert.AreEqual(inserido.IDItemDetalhe, naoAtualizado.IDItemDetalhe);
            Assert.AreEqual(inserido.BlItemTransferencia, naoAtualizado.BlItemTransferencia);
            Assert.AreEqual(inserido.TpUnidadeMedida, naoAtualizado.TpUnidadeMedida);
        }

        [Test]
        public void ObterListaItemSaidaPorFornecedorItemEntrada_CdItemPaging_RetornaItemDetalhe()
        {
            var gateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var histgw = MockRepository.GenerateMock<IItemDetalheHistGateway>();
            var itemRelacionamentoGateway = MockRepository.GenerateMock<IItemRelacionamentoGateway>();

            var itemDetalheService = new ItemDetalheService(gateway, histgw, itemRelacionamentoGateway);

            var paging = new Paging();
            var filtro = new ItemDetalheFiltro() { CdItem = 1, CdSistema = 1 };

            gateway.Expect(g => g.ObterListaItemSaidaPorFornecedorItemEntrada(filtro, paging)).Return(new[] { new ItemDetalhe() { Id = 1 } });

            var r = itemDetalheService.ObterListaItemSaidaPorFornecedorItemEntrada(filtro, paging);

            Assert.AreEqual(1, r.Count());
        }

        [Test]
        public void PesquisarItensSaidaComCDConvertido_ItensDetalhe_RetornaItensMultivendorPaginado()
        {
            Paging paging = new Paging();

            m_itemDetalhe_gateway.Expect(g => g.PesquisarItemCD(1, 1, 1, 1, 2, 1, paging)).Return(
                new[] { 
                    new ItemDetalheCD(){ QtdItensEntrada = 3 },
                    new ItemDetalheCD(){ QtdItensEntrada = 4 },
                    new ItemDetalheCD(){ QtdItensEntrada = 1 }
                });

            Assert.AreEqual(2, m_target.PesquisarItensSaidaComCDConvertido(1, 1, 1, 1, 2, 1, paging).Where(q => q.Multivendor).Count());
        }

        [Test]
        public void PesquisarItensEntradaPorSaidaCD_ItensDetalhe_RetornaItensDetalhePaginadoPorCodigoSaida()
        {
            Paging paging = new Paging();

            m_itemDetalhe_gateway.Expect(g => g.PesquisarItensEntradaPorSaidaCD(2016, 1, 1, paging)).Return(
                new[] { 
                    new ItemDetalheCD(){ cdItemSaida = 2016 },
                    new ItemDetalheCD(){ cdItemSaida = 2016 },
                    new ItemDetalheCD(){ cdItemSaida = 1 }
                });

            m_itemDetalhe_gateway.Expect(g => g.ObterQuantidadeItensEntrada(2016, 1, 1)).Return(2);

            Assert.AreEqual(2, m_target.PesquisarItensEntradaPorSaidaCD(2016, 1, 1, paging).Where(q => q.cdItemSaida == 2016).Count());
        }

        [Test]
        public void PesquisarItensEntradaPorSaidaCD_ItensDetalhe_ContaItensDetalhePorCodigoSaida()
        {
            m_itemDetalhe_gateway.Expect(g => g.PesquisarItensEntradaPorSaidaCD(2016, 1, 1)).Return(
                new[] { 
                    new ItemDetalheCD(){ cdItemSaida = 2016 },
                    new ItemDetalheCD(){ cdItemSaida = 2016 },
                    new ItemDetalheCD(){ cdItemSaida = 1 }
                });

            m_itemDetalhe_gateway.Expect(g => g.ObterQuantidadeItensEntrada(2016, 1, 1)).Return(2);

            Assert.AreEqual(2, m_target.PesquisarItensEntradaPorSaidaCD(2016, 1, 1).Where(q => q.cdItemSaida == 2016).Count());
        }

        [Test]
        public void ObterEstruturadoPorId_RegiaoCompraEAreaCD_CamposPreenchidos()
        {
            var gateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var itemRelacionamentoGateway = MockRepository.GenerateMock<IItemRelacionamentoGateway>();
            var service = new ItemDetalheService(gateway, null, itemRelacionamentoGateway);

            gateway.Expect(g => g.ObterEstruturadoPorId(1)).Return(new ItemDetalhe()
            {
                Id = 1
            });

            gateway.Expect(g => g.ObterEstruturadoExtraPorId(1)).Return(new ItemDetalhe()
            {
                Id = 1,
                AreaCD = new AreaCD() { IdAreaCD = 1, dsAreaCD = "Area Teste" },
                RegiaoCompra = new RegiaoCompra() { IdRegiaoCompra = 1, dsRegiaoCompra = "Regiao Teste" }
            });

            var result = service.ObterEstruturadoPorId(1);

            Assert.IsNotNull(result.AreaCD);
            Assert.IsNotNull(result.RegiaoCompra);
        }

        [Test]
        public void ObterTraitsPorItem_IdItemDetalhe_DuasLojas()
        {
            var gateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var service = new ItemDetalheService(gateway, null, null);
            Paging paging = new Paging();

            gateway.Expect(g => g.ObterTraitsPorItem(1, 1, paging)).Return(new[] { 
                new Loja() { IDLoja = 1 }, 
                new Loja() { IDLoja = 2 } 
            });

            var result = service.ObterTraitsPorItem(1, 1, paging);

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void ObterTraitsPorItem_IdItemDetalhe_NenhumaLoja()
        {
            var gateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var service = new ItemDetalheService(gateway, null, null);
            Paging paging = new Paging();

            gateway.Expect(g => g.ObterTraitsPorItem(1, 1, paging)).Return(null);

            var result = service.ObterTraitsPorItem(1, 1, paging);

            Assert.Null(result);
        }

        [Test]
        public void EstaVinculadoCompraCasada_CdItemDetalheEntradaCdSistema_EstaVinculado()
        {
            var gateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var service = new ItemDetalheService(gateway, null, null);

            gateway.Expect(g => g.EstaVinculadoCompraCasada(123, 1)).Return(true);

            var result = service.EstaVinculadoCompraCasada(123, 1);

            Assert.IsTrue(result);
        }

        [Test]
        public void EstaVinculadoCompraCasada_CdItemDetalheEntradaCdSistema_NaoEstaVinculado()
        {
            var gateway = MockRepository.GenerateMock<IItemDetalheGateway>();
            var service = new ItemDetalheService(gateway, null, null);

            gateway.Expect(g => g.EstaVinculadoCompraCasada(123, 1)).Return(false);

            var result = service.EstaVinculadoCompraCasada(123, 1);

            Assert.IsFalse(result);
        }
    }
}
