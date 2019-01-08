using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Reabastecimento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.Reabastecimento
{
    [TestFixture]
    [Category("Domain"), Category("ReturnSheet")]
    public class ReturnSheetServiceTest
    {
        [Test]
        public void PossuiExportacao_IdReturnSheet_PossuiExportacao()
        {
            var returnSheetGateway = MockRepository.GenerateMock<IReturnSheetGateway>();
            var regiaoCompraGateway = MockRepository.GenerateMock<IRegiaoCompraGateway>();
            var departamentoGateway = MockRepository.GenerateMock<IDepartamentoGateway>();
            var returnSheetItemPrincipalService = MockRepository.GenerateMock<IReturnSheetItemPrincipalService>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();

            var returnSheetService = new ReturnSheetService(returnSheetGateway, regiaoCompraGateway, departamentoGateway, returnSheetItemPrincipalService, auditServiceMock);

            returnSheetGateway.Expect(g => g.PossuiExportacao(1)).Return(true);

            returnSheetService.PodeSerEditada(1);

            returnSheetGateway.AssertWasCalled(g => g.PossuiExportacao(1));
        }

        [Test]
        public void Remover_IdReturnSheet_RealizaExclusaoLogica()
        {
            var returnSheetGateway = MockRepository.GenerateMock<IReturnSheetGateway>();
            var regiaoCompraGateway = MockRepository.GenerateMock<IRegiaoCompraGateway>();
            var departamentoGateway = MockRepository.GenerateMock<IDepartamentoGateway>();
            var returnSheetItemPrincipalService = MockRepository.GenerateMock<IReturnSheetItemPrincipalService>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();

            returnSheetItemPrincipalService.Expect(g => g.ObterPorIdReturnSheet(1)).Return(new List<ReturnSheetItemPrincipal>() 
            { 
                new ReturnSheetItemPrincipal() { Id = 1, IdReturnSheet = 1},
                new ReturnSheetItemPrincipal() { Id = 2, IdReturnSheet = 1}
            });

            var returnSheetService = new ReturnSheetService(returnSheetGateway, regiaoCompraGateway, departamentoGateway, returnSheetItemPrincipalService, auditServiceMock);

            returnSheetGateway.Expect(g => g.FindById(1)).Return(new ReturnSheet() { Id = 1 });

            returnSheetService.Remover(1);

            returnSheetGateway.AssertWasNotCalled(g => g.Delete(1));
        }

        [Test]
        public void Obter_IdReturnSheet_RetornaReturnSheet()
        {
            var returnSheetGateway = MockRepository.GenerateMock<IReturnSheetGateway>();
            var regiaoCompraGateway = MockRepository.GenerateMock<IRegiaoCompraGateway>();
            var departamentoGateway = MockRepository.GenerateMock<IDepartamentoGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();

            var sugestaoReturnSheetGateway = MockRepository.GenerateMock<ISugestaoReturnSheetGateway>();
            var sugestaoReturnSheetService = new SugestaoReturnSheetService(sugestaoReturnSheetGateway, returnSheetGateway, auditServiceMock, MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>());

            var returnSheetItemLojaGateway = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var returnSheetItemLojaService = new ReturnSheetItemLojaService(returnSheetItemLojaGateway, sugestaoReturnSheetService, auditServiceMock);

            var returnSheetItemPrincipalGateway = MockRepository.GenerateMock<IReturnSheetItemPrincipalGateway>();
            var returnSheetItemPrincipalService = new ReturnSheetItemPrincipalService(returnSheetItemPrincipalGateway, returnSheetItemLojaService, returnSheetItemLojaGateway, auditServiceMock);

            var returnSheetService = new ReturnSheetService(returnSheetGateway, regiaoCompraGateway, departamentoGateway, returnSheetItemPrincipalService, auditServiceMock);

            returnSheetGateway
                .Expect(x => x.Obter(2016))
                .Return(new ReturnSheet() { Id = 2016 });

            Assert.IsNotNull(returnSheetService.Obter(2016));
        }

        [Test]
        public void Pesquisar_Filtros_ReturnSheets()
        {
            var returnSheetGateway = MockRepository.GenerateMock<IReturnSheetGateway>();
            var regiaoCompraGateway = MockRepository.GenerateMock<IRegiaoCompraGateway>();
            var departamentoGateway = MockRepository.GenerateMock<IDepartamentoGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();

            var sugestaoReturnSheetGateway = MockRepository.GenerateMock<ISugestaoReturnSheetGateway>();
            var sugestaoReturnSheetService = new SugestaoReturnSheetService(sugestaoReturnSheetGateway, returnSheetGateway, auditServiceMock, MockRepository.GenerateMock<ILogMensagemReturnSheetVigenteService>());

            var returnSheetItemLojaGateway = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var returnSheetItemLojaService = new ReturnSheetItemLojaService(returnSheetItemLojaGateway, sugestaoReturnSheetService, auditServiceMock);

            var returnSheetItemPrincipalGateway = MockRepository.GenerateMock<IReturnSheetItemPrincipalGateway>();
            var returnSheetItemPrincipalService = new ReturnSheetItemPrincipalService(returnSheetItemPrincipalGateway, returnSheetItemLojaService, returnSheetItemLojaGateway, auditServiceMock);

            var returnSheetService = new ReturnSheetService(returnSheetGateway, regiaoCompraGateway, departamentoGateway, returnSheetItemPrincipalService, auditServiceMock);

            var paging = new Infrastructure.Framework.Domain.Paging() { Limit = 50, Offset = 0, OrderBy = null };
            var dt = DateTime.Now;

            returnSheetGateway
                .Expect(x => x.Pesquisar(dt, dt, "Teste", 1, 1, 1, paging))
                .Return(new ReturnSheet[] { new ReturnSheet() { Id = 1, Descricao = "Teste", DhInicioReturn = dt, IdRegiaoCompra = 1, DhFinalReturn = dt, idDepartamento = 1, BlAtivo = true } });

            Assert.AreEqual(1, returnSheetService.Pesquisar(dt, dt, "Teste", 1, 1, 1, paging).Count());
        }

        [Test]
        public void Salvar_ReturnSheetNova_Inclusao()
        {
            var returnSheetGateway = MockRepository.GenerateMock<IReturnSheetGateway>();
            var regiaoCompraGateway = MockRepository.GenerateMock<IRegiaoCompraGateway>();
            var departamentoGateway = MockRepository.GenerateMock<IDepartamentoGateway>();
            var returnSheetItemPrincipalService = MockRepository.GenerateMock<IReturnSheetItemPrincipalService>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var returnSheetService = new ReturnSheetService(returnSheetGateway, regiaoCompraGateway, departamentoGateway, returnSheetItemPrincipalService, auditServiceMock);

            var dhCriacao = DateTime.Now;
            var dhInicioReturn = dhCriacao.AddDays(2);
            var dhFinalReturn = dhInicioReturn.AddDays(3);
            var dhInicioEvento = dhFinalReturn.AddDays(1);
            var dhFinalEvento = dhInicioEvento.AddDays(5);

            var rsNovo = new ReturnSheet()
            {
                Id = 0,
                BlAtivo = true,
                Descricao = "teste",
                DhCriacao = dhCriacao,
                DhAtualizacao = dhCriacao,
                DhInicioReturn = dhInicioReturn,
                DhFinalReturn = dhFinalReturn,
                DhInicioEvento = dhInicioEvento,
                DhFinalEvento = dhFinalEvento,
                HoraCorte = dhCriacao.Date.AddHours(12).AddMinutes(30),
                idDepartamento = 1,
                IdRegiaoCompra = 1,
                IdUsuarioCriacao = 1
            };

            returnSheetService.Salvar(rsNovo);

            returnSheetGateway.AssertWasCalled(g => g.Insert(rsNovo));
        }

        [Test]
        public void Salvar_ReturnSheetAtualizada_Atualizar()
        {
            var returnSheetGateway = MockRepository.GenerateMock<IReturnSheetGateway>();
            var regiaoCompraGateway = MockRepository.GenerateMock<IRegiaoCompraGateway>();
            var departamentoGateway = MockRepository.GenerateMock<IDepartamentoGateway>();
            var returnSheetItemPrincipalService = MockRepository.GenerateMock<IReturnSheetItemPrincipalService>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var returnSheetService = new ReturnSheetService(returnSheetGateway, regiaoCompraGateway, departamentoGateway, returnSheetItemPrincipalService, auditServiceMock);

            var dhCriacao = DateTime.Now;
            var dhInicioReturn = dhCriacao.AddDays(2);
            var dhFinalReturn = dhInicioReturn.AddDays(3);
            var dhInicioEvento = dhFinalReturn.AddDays(1);
            var dhFinalEvento = dhInicioEvento.AddDays(5);
            var horaCorte = dhCriacao.Date.AddHours(12).AddMinutes(30);

            var rsOld = new ReturnSheet
            {
                Id = 1,
                BlAtivo = true,
                Descricao = "teste",
                DhCriacao = dhCriacao,
                DhAtualizacao = dhCriacao,
                DhInicioReturn = dhInicioReturn,
                DhFinalReturn = dhFinalReturn,
                DhInicioEvento = dhInicioEvento,
                DhFinalEvento = dhFinalEvento,
                HoraCorte = horaCorte,
                idDepartamento = 1,
                IdRegiaoCompra = 1,
                IdUsuarioCriacao = 1
            };

            var rsNew = new ReturnSheet
            {
                Id = 1,
                BlAtivo = true,
                Descricao = "teste new",
                DhCriacao = dhCriacao,
                DhAtualizacao = dhCriacao,
                DhInicioReturn = dhInicioReturn,
                DhFinalReturn = dhFinalReturn,
                DhInicioEvento = dhInicioEvento,
                DhFinalEvento = dhFinalEvento,
                HoraCorte = horaCorte,
                idDepartamento = 1,
                IdRegiaoCompra = 1,
                IdUsuarioCriacao = 1
            };

            returnSheetGateway
                .Expect(g => g.Obter(1))
                .Return(rsOld);

            returnSheetService.Salvar(rsNew);

            returnSheetGateway.AssertWasCalled(g => g.Update(rsNew));
            auditServiceMock.AssertWasCalled(x => x.LogUpdate(rsNew, "DhInicioReturn", "DhFinalReturn", "DhInicioEvento", "DhFinalEvento", "IdRegiaoCompra", "Descricao", "BlAtivo"));
        }

        [Test]
        public void Salvar_ReturnSheetComDatasAlteradasPosterioresAHoje_Atualizar()
        {
            var returnSheetGateway = MockRepository.GenerateMock<IReturnSheetGateway>();
            var regiaoCompraGateway = MockRepository.GenerateMock<IRegiaoCompraGateway>();
            var departamentoGateway = MockRepository.GenerateMock<IDepartamentoGateway>();
            var returnSheetItemPrincipalService = MockRepository.GenerateMock<IReturnSheetItemPrincipalService>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var returnSheetService = new ReturnSheetService(returnSheetGateway, regiaoCompraGateway, departamentoGateway, returnSheetItemPrincipalService, auditServiceMock);

            var dhCriacao = DateTime.Now;
            var dhInicioReturn = dhCriacao.AddDays(2);
            var dhFinalReturn = dhInicioReturn.AddDays(3);
            var dhInicioEvento = dhFinalReturn.AddDays(1);
            var dhFinalEvento = dhInicioEvento.AddDays(5);
            var horaCorte = dhCriacao.Date.AddHours(12).AddMinutes(30);

            var rsOld = new ReturnSheet
            {
                Id = 1,
                BlAtivo = true,
                Descricao = "teste",
                DhCriacao = dhCriacao,
                DhAtualizacao = dhCriacao,
                DhInicioReturn = dhInicioReturn,
                DhFinalReturn = dhFinalReturn,
                DhInicioEvento = dhInicioEvento,
                DhFinalEvento = dhFinalEvento,
                HoraCorte = horaCorte,
                idDepartamento = 1,
                IdRegiaoCompra = 1,
                IdUsuarioCriacao = 1
            };

            var rsNew = new ReturnSheet
            {
                Id = 1,
                BlAtivo = true,
                Descricao = "teste new",
                DhCriacao = dhCriacao,
                DhAtualizacao = dhCriacao,
                DhInicioReturn = dhInicioReturn.AddDays(1),
                DhFinalReturn = dhFinalReturn.AddDays(1),
                DhInicioEvento = dhInicioEvento.AddDays(1),
                DhFinalEvento = dhFinalEvento.AddDays(1),
                HoraCorte = horaCorte,
                idDepartamento = 1,
                IdRegiaoCompra = 1,
                IdUsuarioCriacao = 1
            };

            returnSheetGateway
                .Expect(g => g.Obter(1))
                .Return(rsOld);

            returnSheetService.Salvar(rsNew);

            returnSheetGateway.AssertWasCalled(g => g.Update(rsNew));
            auditServiceMock.AssertWasCalled(x => x.LogUpdate(rsNew, "DhInicioReturn", "DhFinalReturn", "DhInicioEvento", "DhFinalEvento", "IdRegiaoCompra", "Descricao", "BlAtivo"));
        }
    }
}
