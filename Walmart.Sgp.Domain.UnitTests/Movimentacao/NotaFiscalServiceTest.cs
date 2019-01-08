using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Movimentacao;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Data.Memory;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Infrastructure.Framework.Commons;

namespace Walmart.Sgp.Domain.UnitTests.Movimentacao
{
    [TestFixture]
    [Category("Domain")]
    public class NotaFiscalServiceTest
    {
        [Test]
        public void ObterEstruturadoPorId_Id_NotaFiscal()
        {
            var gateway = MockRepository.GenerateMock<INotaFiscalGateway>();

            gateway.Expect(o => o.ObterEstruturadoPorId(1)).IgnoreArguments().Return(new NotaFiscal
            {
                IDNotaFiscal = 1,
                Itens = new NotaFiscalItem[] 
                {
                    new NotaFiscalItem
                    {
                        Id = 1,
                        vlCusto = 5,
                        qtItem = 5
                    }
                }
            });

            var target = new NotaFiscalService(gateway, null);
            var actual = target.ObterEstruturadoPorId(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.Itens.Count);
            Assert.AreEqual(25, actual.Itens[0].vlTotal);
            gateway.VerifyAllExpectations();
        }

        [Test]
        public void PesquisarUltimasEntradasPorFiltros_Filro_NotaFiscal()
        {
            var gateway = MockRepository.GenerateMock<INotaFiscalGateway>();
            var dtSolicitacao = DateTime.Parse("2015-12-21");
            var paging = new Paging { Offset = 0, Limit = 10, OrderBy = "dtRecebimento DESC" };

            gateway.Expect(o => o.PesquisarUltimasEntradasPorFiltro(4661, 243, dtSolicitacao, paging)).Return(new NotaFiscalConsolidado[]
            {
                new NotaFiscalConsolidado(),
                new NotaFiscalConsolidado(),
                new NotaFiscalConsolidado()
            });

            var target = new NotaFiscalService(gateway, null);            
            Assert.AreEqual(3, target.PesquisarUltimasEntradasPorFiltros(4661, 243, dtSolicitacao, paging).Count());
        }

        [Test]
        public void ObterCustosPorItem_Args_NotaFiscal()
        {
            var gateway = MockRepository.GenerateMock<INotaFiscalGateway>();
            var dtSolicitacao = DateTime.Parse("2015-12-21");
            gateway.Expect(o => o.ObterCustosPorItem(1005, 8022604, dtSolicitacao)).IgnoreArguments().Return(new NotaFiscalItemCustosConsolidado 
            { 
                
            });

            var target = new NotaFiscalService(gateway, null);
            var actual = target.ObterCustosPorItem(1005, 8022604, dtSolicitacao);
            Assert.IsNotNull(actual);
            gateway.VerifyAllExpectations();
        }

        [Test]
        public void ExisteNotasPendentesPorItem_Args_NotaFiscal()
        {
            var gateway = MockRepository.GenerateMock<INotaFiscalGateway>();
            var dtSolicitacao = DateTime.Parse("2015-12-21");
            gateway.Expect(_ => _.ExisteNotasPendentesPorItem(1005, 8022604, dtSolicitacao)).IgnoreArguments().Return(false);
            var target = new NotaFiscalService(gateway, null);
            var actual = target.ExisteNotasPendentesPorItem(1005, 8022604, dtSolicitacao);
            Assert.AreEqual(false, actual);
            gateway.VerifyAllExpectations();
        }

        [Test]
        public void CorrigirCustos_Custos_NotaFiscalItemAtualizado()
        {
            var notaFiscalGateway = MockRepository.GenerateMock<INotaFiscalGateway>();
            var notaFiscalItemGateway = MockRepository.GenerateMock<MemoryNotaFiscalItemGateway>();

            var notasFiscaisItemExistentes = new List<NotaFiscalItem> 
            {
                new NotaFiscalItem
                {
                    IDNotaFiscalItem = 1,
                    dtLiberacao = null,
                    qtItemAnterior = 0,
                    qtItem = 1,
                    vlCusto = 10,
                    vlMercadoria = 100
                },
                new NotaFiscalItem
                {
                    IDNotaFiscalItem = 2,
                    dtLiberacao = null,
                    qtItemAnterior = 0,
                    qtItem = 1,
                    vlCusto = 10,
                    vlMercadoria = 100
                }
            };

            notaFiscalItemGateway.Insert(notasFiscaisItemExistentes);

            var custos = new List<CustoNotaFiscal> 
            {
                new CustoNotaFiscal
                {
                    blLiberar = true,
                    IDNotaFiscalItem = 1,
                    IDBandeira = 1,
                    qtAjustada = 2
                },
                new CustoNotaFiscal
                {
                    blLiberar = true,
                    IDNotaFiscalItem = 2,
                    IDBandeira = 2,
                    qtAjustada = 3
                }
            };

            notaFiscalItemGateway.Update(new NotaFiscalItem
            {
                IDNotaFiscalItem = 1,
                dtLiberacao = DateTime.UtcNow,
                qtItemAnterior = 1,
                qtItem = 2,
                vlCusto = 50,
                vlMercadoria = 100
            });

            notaFiscalItemGateway.Update(new NotaFiscalItem
            {
                IDNotaFiscalItem = 2,
                dtLiberacao = DateTime.UtcNow,
                qtItemAnterior = 1,
                qtItem = 4,
                vlCusto = 100,
                vlMercadoria = 300
            });

            var target = new NotaFiscalService(notaFiscalGateway, notaFiscalItemGateway);

            target.CorrigirCustos(custos);

            var actual = notaFiscalItemGateway.FindById(1);
            Assert.AreEqual(50, actual.vlCusto);
            Assert.IsNotNull(actual.dtLiberacao);
            Assert.AreEqual(2, actual.qtItemAnterior);
            Assert.AreEqual(2, actual.qtItem);
            Assert.AreEqual(100, actual.vlMercadoria);

            actual = notaFiscalItemGateway.FindById(2);
            Assert.AreEqual(100, actual.vlCusto);
            Assert.IsNotNull(actual.dtLiberacao);
            Assert.AreEqual(4, actual.qtItemAnterior);
            Assert.AreEqual(3, actual.qtItem);
            Assert.AreEqual(300, actual.vlMercadoria);

            notaFiscalGateway.VerifyAllExpectations();
        }
    }
}
