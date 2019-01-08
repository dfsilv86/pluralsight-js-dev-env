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
    [Category("Domain"), Category("ReturnSheetItemLoja")]
    public class ReturnSheetItemLojaServiceTest
    {
        [Test]
        public void Remover_SugestoesAtivas_DeveInativar()
        {
            var sugestaoReturnSheetService = MockRepository.GenerateMock<ISugestaoReturnSheetService>();
            sugestaoReturnSheetService.Expect(g => g.ObterPorIdReturnSheetItemLoja(1)).Return(new List<SugestaoReturnSheet>() 
            { 
                new SugestaoReturnSheet() { IdSugestaoReturnSheet = 1, IdReturnSheetItemLoja = 1 },
                new SugestaoReturnSheet() { IdSugestaoReturnSheet = 2, IdReturnSheetItemLoja = 1 }
            });

            var returnSheetItemLojaGateway = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            returnSheetItemLojaGateway.Expect(g => g.FindById(1)).Return(new ReturnSheetItemLoja() { Id = 1 });

            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();

            var returnSheetItemLojaService = new ReturnSheetItemLojaService(returnSheetItemLojaGateway, sugestaoReturnSheetService, auditServiceMock);

            returnSheetItemLojaService.Remover(1);

            returnSheetItemLojaGateway.AssertWasNotCalled(g => g.Delete(1));
        }

        [Test]
        public void ObterLojasValidasItem_CdItemCdSistemaIdReturnSheet_RetornaLojasValidas()
        {
            var sugestaoReturnSheetService = MockRepository.GenerateMock<ISugestaoReturnSheetService>();

            var returnSheetItemLojaGateway = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var returnSheetItemLojaService = new ReturnSheetItemLojaService(returnSheetItemLojaGateway, sugestaoReturnSheetService, auditServiceMock);

            var paging = new Infrastructure.Framework.Domain.Paging() { Limit = 50, Offset = 0, OrderBy = null };

            returnSheetItemLojaGateway.Expect(g => g.ObterLojasValidasItem(1, 1, 1, null, paging)).Return(new ReturnSheetItemLoja[] 
            { 
                new ReturnSheetItemLoja() { IdReturnSheetItemLoja = 1, IdReturnSheetItemPrincipal = 1 } 
            });

            var x = returnSheetItemLojaService.ObterLojasValidasItem(1, 1, 1, null, paging);

            Assert.NotNull(x);
        }
        
        [Test]
        public void ObterPorIdReturnSheetItemPrincipal_IdReturnSheetItemPrincipal_RetornaReturnSheetItemLoja()
        {
            var sugestaoReturnSheetService = MockRepository.GenerateMock<ISugestaoReturnSheetService>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();

            var returnSheetItemLojaGateway = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var returnSheetItemLojaService = new ReturnSheetItemLojaService(returnSheetItemLojaGateway, sugestaoReturnSheetService, auditServiceMock);

            var idReturnSheetItemPrincipal = 1;
            returnSheetItemLojaGateway.Expect(g => g.ObterPorIdReturnSheetItemPrincipal(idReturnSheetItemPrincipal)).Return(new ReturnSheetItemLoja[] 
            {
                  new ReturnSheetItemLoja() { IdReturnSheetItemLoja = 1, IdReturnSheetItemPrincipal = 1 }
            });

            var x = returnSheetItemLojaService.ObterPorIdReturnSheetItemPrincipal(1);

            Assert.NotNull(x);
        }

        [Test]
        public void ObterEstadosLojasValidasItem_CdItemCdSistema_RetornaEstadosLojasValidas() 
        {
            var sugestaoReturnSheetServiceMock = MockRepository.GenerateMock<ISugestaoReturnSheetService>();
            var returnSheetItemLojaGatewayMock = MockRepository.GenerateMock<IReturnSheetItemLojaGateway>();
            var auditServiceMock = MockRepository.GenerateMock<IAuditService>();
            var returnSheetItemLojaService = new ReturnSheetItemLojaService(returnSheetItemLojaGatewayMock, sugestaoReturnSheetServiceMock, auditServiceMock);

            returnSheetItemLojaService.ObterEstadosLojasValidasItem(123, 1);

            returnSheetItemLojaGatewayMock.AssertWasCalled(x => x.ObterEstadosLojasValidasItem(123, 1));
        }
    }
}