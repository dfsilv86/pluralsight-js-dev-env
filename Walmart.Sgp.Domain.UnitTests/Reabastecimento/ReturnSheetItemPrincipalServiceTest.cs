using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain"), Category("ReturnSheetItemPrincipal")]
    public class ReturnSheetItemPrincipalServiceTest
    {
        [Test]
        public void ObterPorIdReturnSheet_IdReturnSheet_RetornaReturnSheetItemPrincipal()
        {
            var returnSheetItemLojaService = MockRepository.GenerateMock<IReturnSheetItemLojaService>();
            var returnSheetItemPrincipalGateway = MockRepository.GenerateMock<IReturnSheetItemPrincipalGateway>();
            var returnSheetItemLojaGateway = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var returnSheetItemPrincipalService = new ReturnSheetItemPrincipalService(returnSheetItemPrincipalGateway, returnSheetItemLojaService, returnSheetItemLojaGateway, auditServiceMock);

            returnSheetItemPrincipalGateway.Expect(g => g.ObterPorIdReturnSheet(1)).Return(new List<ReturnSheetItemPrincipal>() 
            { 
                new ReturnSheetItemPrincipal() { Id = 1, IdReturnSheet = 1 },
                new ReturnSheetItemPrincipal() { Id = 2, IdReturnSheet = 1 }
            });

            var retorno = returnSheetItemPrincipalService.ObterPorIdReturnSheet(1);

            Assert.AreEqual(2, retorno.Count());
        }

        [Test]
        public void Remover_IdReturnSheetItemPrincipal_RealizaExclusaoLogica()
        {
            var returnSheetItemLojaService = MockRepository.GenerateMock<IReturnSheetItemLojaService>();
            returnSheetItemLojaService.Expect(g => g.ObterPorIdReturnSheetItemPrincipal(1)).Return(new List<ReturnSheetItemLoja>() 
            { 
                new ReturnSheetItemLoja(){ Id = 1, IdReturnSheetItemPrincipal = 1 },
                new ReturnSheetItemLoja(){ Id = 2, IdReturnSheetItemPrincipal = 1 }
            });

            var returnSheetItemPrincipalGateway = MockRepository.GenerateMock<IReturnSheetItemPrincipalGateway>();
            var returnSheetItemLojaGateway = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var returnSheetItemPrincipalService = new ReturnSheetItemPrincipalService(returnSheetItemPrincipalGateway, returnSheetItemLojaService, returnSheetItemLojaGateway, auditServiceMock);

            returnSheetItemPrincipalGateway.Expect(g => g.FindById(1)).Return(new ReturnSheetItemPrincipal() { Id = 1 });

            returnSheetItemPrincipalService.Remover(1);

            returnSheetItemPrincipalGateway.AssertWasNotCalled(g => g.Delete(1));
        }

        [Test]
        public void PesquisarPorIdReturnSheet_IdReturnSheetPaging_RetornaReturnSheetItemPrincipal()
        {
            var sugestaoReturnSheetGateway = MockRepository.GenerateMock<ISugestaoReturnSheetGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var sugestaoReturnSheetService = new SugestaoReturnSheetService(sugestaoReturnSheetGateway, MockRepository.GenerateMock<IReturnSheetGateway>(), auditServiceMock, MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>());

            var returnSheetItemLojaGateway = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var returnSheetItemLojaService = new ReturnSheetItemLojaService(returnSheetItemLojaGateway, sugestaoReturnSheetService, auditServiceMock);

            var returnSheetItemPrincipalGateway = MockRepository.GenerateMock<IReturnSheetItemPrincipalGateway>();
            var returnSheetItemPrincipalService = new ReturnSheetItemPrincipalService(returnSheetItemPrincipalGateway, returnSheetItemLojaService, returnSheetItemLojaGateway, auditServiceMock);

            var paging = new Infrastructure.Framework.Domain.Paging() { Limit = 50, Offset = 0, OrderBy = null };
            var dt = DateTime.Now;

            returnSheetItemPrincipalGateway
                .Expect(x => x.PesquisarPorIdReturnSheet(1, paging))
                .Return(new ReturnSheetItemPrincipal[] { new ReturnSheetItemPrincipal() { Id = 1 } });

            Assert.AreEqual(1, returnSheetItemPrincipalService.PesquisarPorIdReturnSheet(1, paging).Count());
        }

        [Test]
        public void Remover_IdReturnSheetCdItem_RealizaExclusaoLogica()
        {
            var returnSheetItemLojaGatewayMock = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var returnSheetItemLojaServiceMock = MockRepository.GenerateMock<IReturnSheetItemLojaService>();
            var returnSheetItemPrincipalGatewayMock = MockRepository.GenerateMock<IReturnSheetItemPrincipalGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();

            var returnSheetItemPrincipalService = new ReturnSheetItemPrincipalService(returnSheetItemPrincipalGatewayMock, returnSheetItemLojaServiceMock, returnSheetItemLojaGatewayMock, auditServiceMock);

            var idReturnSheetItemPrincipal = 1;
            var returnSheetItemPrincipal = new ReturnSheetItemPrincipal { Id = idReturnSheetItemPrincipal, blAtivo = true };

            returnSheetItemPrincipalGatewayMock
                .Expect(x => x.ObterPorReturnSheetEItemDetalheSaida(33, 500051463))
                .Return(idReturnSheetItemPrincipal);

            returnSheetItemPrincipalGatewayMock
                .Expect(x => x.FindById(1))
                .Return(returnSheetItemPrincipal);

            returnSheetItemPrincipalGatewayMock
                .Expect(x => x.Update(returnSheetItemPrincipal));

            returnSheetItemPrincipalService.Remover(33, 500051463);

            Assert.IsFalse(returnSheetItemPrincipal.blAtivo);
            returnSheetItemPrincipalGatewayMock.AssertWasCalled(x => x.Update(returnSheetItemPrincipal));
        }

        [Test]
        public void SalvarLojas_NovaLoja_DeveInserirLoja()
        {
            var returnSheetItemPrincipalGatewayMock = MockRepository.GenerateMock<IReturnSheetItemPrincipalGateway>();
            var returnSheetItemLojaServiceMock = MockRepository.GenerateMock<IReturnSheetItemLojaService>();
            var returnSheetItemLojaGatewayMock = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();

            var returnSheetItemPrincipalService = new ReturnSheetItemPrincipalService(returnSheetItemPrincipalGatewayMock, returnSheetItemLojaServiceMock, returnSheetItemLojaGatewayMock, auditServiceMock);

            var idReturnSheet = 33;
            var idItemDetalheSaida = 1;
            var idReturnSheetItemPrincipal = 321;
            var returnSheetItemPrincipal = new ReturnSheetItemPrincipal { IdReturnSheetItemPrincipal = idReturnSheetItemPrincipal };

            var lojaAlteradaInserir = new ReturnSheetItemLoja
            {
                IdItemDetalheSaida = idItemDetalheSaida,
                IdItemDetalheEntrada = 123,
                IdLoja = 12,
                selecionado = true,
                PrecoVenda = 100.00M,
                dsEstado = "RS"
            };

            returnSheetItemPrincipalGatewayMock
                .Expect(x => x.Insert(idReturnSheet, idItemDetalheSaida))
                .Return(returnSheetItemPrincipal);

            returnSheetItemLojaGatewayMock
                .Expect(x => x.ObterLojasPorReturnSheetEItemDetalheSaida(idReturnSheet, idItemDetalheSaida))
                .Return(new ReturnSheetItemLoja[0]);

            returnSheetItemLojaGatewayMock
                .Expect(x => x.Insert(lojaAlteradaInserir));

            returnSheetItemPrincipalService.SalvarLojas(new[] { lojaAlteradaInserir }, idReturnSheet, null);

            Assert.AreEqual(lojaAlteradaInserir.IdReturnSheetItemPrincipal, idReturnSheetItemPrincipal);
            Assert.AreEqual(lojaAlteradaInserir.blAtivo, lojaAlteradaInserir.selecionado);
            Assert.AreEqual(lojaAlteradaInserir.IdItemDetalhe, lojaAlteradaInserir.IdItemDetalheEntrada);
            returnSheetItemLojaGatewayMock.AssertWasCalled(x => x.Insert(lojaAlteradaInserir));
        }

        [Test]
        public void SalvarLojas_AtualizarLoja_LojaAlteradaSelecionada()
        {
            var returnSheetItemPrincipalGatewayMock = MockRepository.GenerateMock<IReturnSheetItemPrincipalGateway>();
            var returnSheetItemLojaServiceMock = MockRepository.GenerateMock<IReturnSheetItemLojaService>();
            var returnSheetItemLojaGatewayMock = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();

            var returnSheetItemPrincipalService = new ReturnSheetItemPrincipalService(returnSheetItemPrincipalGatewayMock, returnSheetItemLojaServiceMock, returnSheetItemLojaGatewayMock, auditServiceMock);

            var idReturnSheet = 33;
            var idItemDetalheSaida = 1;
            var idItemDetalheEntrada = 222;
            var idLoja = 333;
            var returnSheetItemPrincipal = new ReturnSheetItemPrincipal();

            var lojaAlteradaAtualizar = new ReturnSheetItemLoja
            {
                IdItemDetalheSaida = idItemDetalheSaida,
                IdItemDetalheEntrada = idItemDetalheEntrada,
                IdLoja = idLoja,
                selecionado = true,
                PrecoVenda = 200.00M,
                dsEstado = "RS"
            };

            var lojaPersistida = new ReturnSheetItemLoja
            {
                IdItemDetalheSaida = idItemDetalheSaida,
                IdItemDetalhe = idItemDetalheEntrada,
                IdLoja = idLoja,
                blAtivo = false,
                PrecoVenda = 100.00M,
                dsEstado = "RS"
            };

            returnSheetItemPrincipalGatewayMock
                .Expect(x => x.Insert(idReturnSheet, idItemDetalheSaida))
                .Return(returnSheetItemPrincipal);

            returnSheetItemLojaGatewayMock
                .Expect(x => x.ObterLojasPorReturnSheetEItemDetalheSaida(idReturnSheet, idItemDetalheSaida))
                .Return(new[] { lojaPersistida });

            returnSheetItemLojaGatewayMock
                .Expect(x => x.Update(lojaPersistida));

            returnSheetItemPrincipalService.SalvarLojas(new[] { lojaAlteradaAtualizar }, idReturnSheet, null);

            Assert.AreEqual(lojaPersistida.blAtivo, lojaAlteradaAtualizar.selecionado);
            Assert.AreEqual(lojaPersistida.PrecoVenda, lojaAlteradaAtualizar.PrecoVenda);
            returnSheetItemLojaGatewayMock.AssertWasCalled(x => x.Update(lojaPersistida));
        }

        [Test]
        public void SalvarLojas_AtualizarLoja_LojaAlteraNaoSelecionadaELojaPersistidaInativa()
        {
            var returnSheetItemPrincipalGatewayMock = MockRepository.GenerateMock<IReturnSheetItemPrincipalGateway>();
            var returnSheetItemLojaServiceMock = MockRepository.GenerateMock<IReturnSheetItemLojaService>();
            var returnSheetItemLojaGatewayMock = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();

            var returnSheetItemPrincipalService = new ReturnSheetItemPrincipalService(returnSheetItemPrincipalGatewayMock, returnSheetItemLojaServiceMock, returnSheetItemLojaGatewayMock, auditServiceMock);

            var idReturnSheet = 33;
            var idItemDetalheSaida = 1;
            var idItemDetalheEntrada = 222;
            var idLoja = 333;
            var returnSheetItemPrincipal = new ReturnSheetItemPrincipal();

            var lojaAlteradaAtualizar = new ReturnSheetItemLoja
            {
                IdItemDetalheSaida = idItemDetalheSaida,
                IdItemDetalheEntrada = idItemDetalheEntrada,
                IdLoja = idLoja,
                selecionado = false,
                PrecoVenda = 200.00M,
                dsEstado = "RS"
            };

            var lojasAlteradas = new[] 
            { 
                lojaAlteradaAtualizar,
                new ReturnSheetItemLoja
                {
                    IdItemDetalheSaida = idItemDetalheSaida,
                    IdItemDetalheEntrada = 123,
                    IdLoja = 444,
                    selecionado = true,
                    PrecoVenda = 200.00M,
                    dsEstado = "RS"
                }
            };

            var lojaPersistida = new ReturnSheetItemLoja
            {
                IdItemDetalheSaida = idItemDetalheSaida,
                IdItemDetalhe = idItemDetalheEntrada,
                IdLoja = idLoja,
                blAtivo = false,
                PrecoVenda = 100.00M,
                dsEstado = "RS"
            };

            returnSheetItemPrincipalGatewayMock
                .Expect(x => x.Insert(idReturnSheet, idItemDetalheSaida))
                .Return(returnSheetItemPrincipal);

            returnSheetItemLojaGatewayMock
                .Expect(x => x.ObterLojasPorReturnSheetEItemDetalheSaida(idReturnSheet, idItemDetalheSaida))
                .Return(new[] { lojaPersistida });

            returnSheetItemLojaGatewayMock
                .Expect(x => x.Update(lojaPersistida));

            returnSheetItemPrincipalService.SalvarLojas(lojasAlteradas, idReturnSheet, null);
            returnSheetItemLojaGatewayMock.AssertWasNotCalled(x => x.Update(lojaPersistida));
        }

        [Test]
        public void SalvarLojas_LojasAtualizadas_AtualizarPrecoVendaLojasPersistidas()
        {
            var returnSheetItemPrincipalGatewayMock = MockRepository.GenerateMock<IReturnSheetItemPrincipalGateway>();
            var returnSheetItemLojaServiceMock = MockRepository.GenerateMock<IReturnSheetItemLojaService>();
            var returnSheetItemLojaGatewayMock = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();

            var returnSheetItemPrincipalService = new ReturnSheetItemPrincipalService(returnSheetItemPrincipalGatewayMock, returnSheetItemLojaServiceMock, returnSheetItemLojaGatewayMock, auditServiceMock);

            var idReturnSheet = 33;
            var idItemDetalheSaida = 1;
            var precoVenda = 300.00M;
            var returnSheetItemPrincipal = new ReturnSheetItemPrincipal();

            var lojasAlteradas = new ReturnSheetItemLoja
            {
                IdItemDetalheSaida = idItemDetalheSaida,
                IdItemDetalheEntrada = 123,
                IdLoja = 12,
                selecionado = true,
                PrecoVenda = 200.00M,
                dsEstado = "RS"
            };

            var lojaPersistida = new ReturnSheetItemLoja
            {
                IdItemDetalheSaida = idItemDetalheSaida,
                IdItemDetalhe = 234,
                IdLoja = 13,
                blAtivo = true,
                PrecoVenda = 100.00M,
                dsEstado = "RS"
            };

            returnSheetItemPrincipalGatewayMock
                .Expect(x => x.Insert(idReturnSheet, idItemDetalheSaida))
                .Return(returnSheetItemPrincipal);

            returnSheetItemLojaGatewayMock
                .Expect(x => x.ObterLojasPorReturnSheetEItemDetalheSaida(idReturnSheet, idItemDetalheSaida))
                .Return(new[] { lojaPersistida });

            returnSheetItemLojaGatewayMock
                .Expect(x => x.Update(lojaPersistida));

            returnSheetItemPrincipalService.SalvarLojas(new[] { lojasAlteradas }, idReturnSheet, precoVenda);

            Assert.AreEqual(lojaPersistida.PrecoVenda, precoVenda);
            returnSheetItemLojaGatewayMock.AssertWasCalled(x => x.Update(lojaPersistida));
        }

        [Test]
        public void SalvarLojas_ReativarReturnSheetItemPrincipal_ItemPrincipalInativo()
        {
            var returnSheetItemPrincipalGatewayMock = MockRepository.GenerateMock<IReturnSheetItemPrincipalGateway>();
            var returnSheetItemLojaServiceMock = MockRepository.GenerateMock<IReturnSheetItemLojaService>();
            var returnSheetItemLojaGatewayMock = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();

            var returnSheetItemPrincipalService = new ReturnSheetItemPrincipalService(returnSheetItemPrincipalGatewayMock, returnSheetItemLojaServiceMock, returnSheetItemLojaGatewayMock, auditServiceMock);

            var idReturnSheet = 33;
            var idItemDetalheSaida = 1;
            var returnSheetItemPrincipal = new ReturnSheetItemPrincipal { blAtivo = false };

            var lojasAlteradas = new ReturnSheetItemLoja
            {
                IdItemDetalheSaida = idItemDetalheSaida,
                IdItemDetalheEntrada = 123,
                IdLoja = 12,
                selecionado = true,
                PrecoVenda = 200.00M,
                dsEstado = "RS"
            };

            returnSheetItemPrincipalGatewayMock
                .Expect(x => x.Insert(idReturnSheet, idItemDetalheSaida))
                .Return(returnSheetItemPrincipal);

            returnSheetItemLojaGatewayMock
                .Expect(x => x.ObterLojasPorReturnSheetEItemDetalheSaida(idReturnSheet, idItemDetalheSaida))
                .Return(new ReturnSheetItemLoja[0]);

            returnSheetItemPrincipalGatewayMock
                .Expect(x => x.Update(returnSheetItemPrincipal));

            returnSheetItemPrincipalService.SalvarLojas(new[] { lojasAlteradas }, idReturnSheet, null);

            Assert.IsTrue(returnSheetItemPrincipal.blAtivo);
            returnSheetItemPrincipalGatewayMock.AssertWasCalled(x => x.Update(returnSheetItemPrincipal));
        }

        [Test]
        public void SalvarLojas_ReativarReturnSheetItemPrincipal_ItemPrincipalAtivo()
        {
            var returnSheetItemPrincipalGatewayMock = MockRepository.GenerateMock<IReturnSheetItemPrincipalGateway>();
            var returnSheetItemLojaServiceMock = MockRepository.GenerateMock<IReturnSheetItemLojaService>();
            var returnSheetItemLojaGatewayMock = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();

            var returnSheetItemPrincipalService = new ReturnSheetItemPrincipalService(returnSheetItemPrincipalGatewayMock, returnSheetItemLojaServiceMock, returnSheetItemLojaGatewayMock, auditServiceMock);

            var idReturnSheet = 33;
            var idItemDetalheSaida = 1;
            var returnSheetItemPrincipal = new ReturnSheetItemPrincipal { blAtivo = true };

            var lojasAlteradas = new ReturnSheetItemLoja
            {
                IdItemDetalheSaida = idItemDetalheSaida,
                IdItemDetalheEntrada = 123,
                IdLoja = 12,
                selecionado = true,
                PrecoVenda = 200.00M,
                dsEstado = "RS"
            };

            returnSheetItemPrincipalGatewayMock
                .Expect(x => x.Insert(idReturnSheet, idItemDetalheSaida))
                .Return(returnSheetItemPrincipal);

            returnSheetItemLojaGatewayMock
                .Expect(x => x.ObterLojasPorReturnSheetEItemDetalheSaida(idReturnSheet, idItemDetalheSaida))
                .Return(new ReturnSheetItemLoja[0]);

            returnSheetItemPrincipalGatewayMock
                .Expect(x => x.Update(returnSheetItemPrincipal));

            returnSheetItemPrincipalService.SalvarLojas(new[] { lojasAlteradas }, idReturnSheet, null);

            Assert.IsTrue(returnSheetItemPrincipal.blAtivo);
            returnSheetItemPrincipalGatewayMock.AssertWasNotCalled(x => x.Update(returnSheetItemPrincipal));
        }
    }
}