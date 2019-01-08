using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Processing;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain")]
    public class RelacaoItemLojaCDServiceTest
    {
        [Test]
        public void Desvincular_ItemEntradaVinculado_VinculosRemovidos()
        {
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var gw = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            gwLCP.Expect(g => g.FindById(1)).IgnoreArguments().Return(new LojaCdParametro() { IDLoja = 1, IDCD = 1 });
            var svc = new RelacaoItemLojaCDService(gw, gwLCP);

            var vinculo = new RelacaoItemLojaCDVinculo()
            {
                CdLoja = 1,
                CdItemDetalheSaida = 1
            };

            var vinculos = new List<RelacaoItemLojaCDVinculo>()
            {
                vinculo
            };

            var relacao = new RelacaoItemLojaCD()
            {
                IDItem = 1,
                IdItemEntrada = 2016
            };

            gw.Expect(g => g.ObterPorVinculo(1, 1, 1)).Return(new RelacaoItemLojaCD[] { relacao });

            gw.Expect(g => g.FindById(1)).IgnoreArguments().Return(new RelacaoItemLojaCD() { blAtivo = true, IDItem = 1, IdItemEntrada = 1 });

            svc.Desvincular(vinculos, 1, 1);

            Assert.IsNull(relacao.IdItemEntrada);
        }

        [Test]
        public void RemoverRelacionamentoPorItemEntrada_ItemEntrada_RelacionamentoRemovido()
        {
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            gwLCP.Expect(g => g.FindById(0)).IgnoreArguments().Return(new LojaCdParametro() { IDLojaCDParametro = 0, IDLoja = 1, IDCD = 1 });
            var gw = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var svc = new RelacaoItemLojaCDService(gw, gwLCP);

            var idItem = 1;
            var relacionamento = new RelacaoItemLojaCD() { IdItemEntrada = idItem };

            gw.Expect(g => g.Find(null, null)).Return(new[] { relacionamento }).IgnoreArguments();

            gw.Expect(g => g.Find("idItemEntrada = @idItemEntrada", new { idItemEntrada = 1 })).IgnoreArguments()
               .Return(new[] { new RelacaoItemLojaCD() { blAtivo = true, IDItem = 1, IdItemEntrada = 1 } });

            gw.Expect(g => g.FindById(1)).IgnoreArguments().Return(new RelacaoItemLojaCD() { blAtivo = true, IDItem = 1, IdItemEntrada = 1 });

            svc.RemoverRelacionamentoPorItemEntrada(idItem, false);

            Assert.IsNull(relacionamento.IdItemEntrada);
        }

        [Test]
        public void ObterPoFiltro_Filtro_Itens()
        {
            var request = new RelacaoItemLojaCDFiltro
                {
                    blVinculado = false,
                    cdItemSaida = 9459505,
                    dsEstado = null,
                    idBandeira = null,
                    idRegiaoCompra = null
                };

            var paging = new Paging();

            var gateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();

            gateway.Expect(g => g.ObterPorFiltro(request, paging)).Return(new RelacaoItemLojaCDConsolidado[] { new RelacaoItemLojaCDConsolidado() });

            RelacaoItemLojaCDService target = new RelacaoItemLojaCDService(gateway, gwLCP);

            Assert.AreEqual(1, target.ObterPorFiltro(request, paging).Count());
        }

        [Test]
        public void Salvar_JaExiste_Atualiza()
        {
            var gateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            gwLCP.Expect(g => g.FindById(0)).IgnoreArguments().Return(new LojaCdParametro() { IDLojaCDParametro = 0, IDLoja = 1, IDCD = 1 });
            var target = new RelacaoItemLojaCDService(gateway, gwLCP);

            var entity = new RelacaoItemLojaCD
            {
                Id = 7,
                IdItemEntrada = 423217
            };

            gateway.Expect(g => g.Find("idItemEntrada = @idItemEntrada", new { idItemEntrada = 1 })).IgnoreArguments()
               .Return(new[] { new RelacaoItemLojaCD() { blAtivo = true, IDItem = 1, IdItemEntrada = 1 } });

            gateway.Expect(g => g.FindById(1)).IgnoreArguments().Return(new RelacaoItemLojaCD() { blAtivo = true, IDItem = 1, IdItemEntrada = 1 });

            target.Salvar(entity);
            Assert.IsTrue(entity.CdUsuarioAtualizacao.HasValue);
            Assert.IsTrue(entity.DhAtualizacao.HasValue);
        }

        [Test]
        public void SalvarVinculos_Vinculos_Salva()
        {
            var gateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var updateFields = @"idItemEntrada = @IdItemEntrada, vlTipoReabastecimento = @VlTipoReabastecimento, cdCrossRef = @CdCrossRef, dhAtualizacao = @DhAtualizacao, cdUsuarioAtualizacao = @CdUsuarioAtualizacao, blAtivo = @blAtivo";
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            gwLCP.Expect(g => g.FindById(1)).IgnoreArguments().Return(new LojaCdParametro() { IDLoja = 1, IDCD = 1 });
            var target = new RelacaoItemLojaCDService(gateway, gwLCP);

            var vinculos = new RelacaoItemLojaCDVinculo[]
            {
                new RelacaoItemLojaCDVinculo()
                {
                    CdCD = 1,
                    CdLoja = 1,
                    CdItemDetalheSaida = 1,
                    CdItemDetalheEntrada = 1
                },
                new RelacaoItemLojaCDVinculo()
                {
                    CdCD = 2,
                    CdLoja = 2,
                    CdItemDetalheSaida = 2,
                    CdItemDetalheEntrada = 2
                }
            };

            var relacaoItemLojaCD1 = new RelacaoItemLojaCD() { IDRelacaoItemLojaCD = 1, IDItem = 1000, IdItemEntrada = 1001 };
            var relacaoItemLojaCD2 = new RelacaoItemLojaCD() { IDRelacaoItemLojaCD = 2, IDItem = 2000, IdItemEntrada = 2001 };

            gateway.Expect(g => g.ObterPorFiltroConsiderandoXRef(1, 1, 1, 1))
                .Return(relacaoItemLojaCD1);

            gateway.Expect(g => g.ObterPorFiltroConsiderandoXRef(2, 2, 2, 2))
                .Return(relacaoItemLojaCD2);

            gateway.Expect(g => g.FindById(1)).IgnoreArguments().Return(new RelacaoItemLojaCD() { CdCrossRef = 1, VlTipoReabastecimento = 1, IdItemEntrada = 1 });

            target.SalvarVinculos(vinculos, 1);

            gateway.AssertWasCalled(x => x.Update(updateFields, relacaoItemLojaCD1));
            gateway.AssertWasCalled(x => x.Update(updateFields, relacaoItemLojaCD2));
        }

        [Test]
        public void SalvarVinculos_Vinculos_NaoSalvaUmDosVinculos()
        {
            var gateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var updateFields = @"idItemEntrada = @IdItemEntrada, vlTipoReabastecimento = @VlTipoReabastecimento, cdCrossRef = @CdCrossRef, dhAtualizacao = @DhAtualizacao, cdUsuarioAtualizacao = @CdUsuarioAtualizacao, blAtivo = @blAtivo";
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            gwLCP.Expect(g => g.FindById(1)).IgnoreArguments().Return(new LojaCdParametro() { IDLoja = 1, IDCD = 1 });
            var target = new RelacaoItemLojaCDService(gateway, gwLCP);

            var vinculos = new RelacaoItemLojaCDVinculo[]
            {
                new RelacaoItemLojaCDVinculo()
                {
                    CdCD = 1,
                    CdLoja = 1,
                    CdItemDetalheSaida = 1,
                    CdItemDetalheEntrada = 1
                },
                new RelacaoItemLojaCDVinculo()
                {
                    CdCD = 2,
                    CdLoja = 2,
                    CdItemDetalheSaida = 2,
                    CdItemDetalheEntrada = 2
                }
            };

            RelacaoItemLojaCD relacaoItemLojaCD1 = null;
            var relacaoItemLojaCD2 = new RelacaoItemLojaCD() { IDRelacaoItemLojaCD = 1, IDItem = 2000, IdItemEntrada = 2001 };

            gateway.Expect(g => g.ObterPorFiltroConsiderandoXRef(1, 1, 1, 1))
                .Return(relacaoItemLojaCD1);

            gateway.Expect(g => g.ObterPorFiltroConsiderandoXRef(2, 2, 2, 2))
                .Return(relacaoItemLojaCD2);

            gateway.Expect(g => g.FindById(1)).IgnoreArguments().Return(new RelacaoItemLojaCD() { CdCrossRef = 1, VlTipoReabastecimento = 1, IdItemEntrada = 1 });

            target.SalvarVinculos(vinculos, 1);

            gateway.AssertWasNotCalled(x => x.Update(updateFields, relacaoItemLojaCD1));
            gateway.AssertWasCalled(x => x.Update(updateFields, relacaoItemLojaCD2));
        }

        [Test]
        public void ObterProcessamentosImportacao_Null_Exception()
        {
            var gateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();
            var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
            var target = new RelacaoItemLojaCDService(gateway, gwLCP);

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                var result = target.ObterProcessamentosImportacao(null, null, null, null);
            });
        }

        [Test]
        public void ObterProcessamentosImportacao_Filter_Result()
        {
            var current = RuntimeContext.Current;

            try
            {
                RuntimeContext.Current = new MemoryRuntimeContext { User = new MemoryRuntimeUser { Id = 3, IsAdministrator = false } };

                var gateway = MockRepository.GenerateMock<IRelacaoItemLojaCDGateway>();

                gateway.Expect(g => g.ObterProcessamentosImportacao(3, false, null, "Imp", null, null)).Return(new ProcessOrderModel[] { new ProcessOrderModel() { Id = 2 } });
                var gwLCP = MockRepository.GenerateMock<ILojaCdParametroGateway>();
                var target = new RelacaoItemLojaCDService(gateway, gwLCP);

                var result = target.ObterProcessamentosImportacao(null, "Imp", null, null);

                Assert.AreEqual(1, result.Count());
                Assert.AreEqual(2, result.First().Id);

                gateway.VerifyAllExpectations();
            }
            finally
            {
                RuntimeContext.Current = current;
            }
        }
    }
}
