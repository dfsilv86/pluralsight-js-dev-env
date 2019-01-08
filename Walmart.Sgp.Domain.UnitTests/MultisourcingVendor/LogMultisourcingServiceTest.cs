using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor
{
    [TestFixture]
    [Category("Domain"), Category("LogMultiSourcing")]
    public class LogMultisourcingServiceTest
    {
        [Test]
        public void Logar_MultisourcingAnteriorEPosteriorAAlteracao_EfetuarLogSemBuscarDetalhesDeMultisourcing()
        {
            var multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
            var logMultiSourcingGateway = MockRepository.GenerateMock<ILogMultiSourcingGateway>();
            var logMultisourcingService = new LogMultiSourcingService(logMultiSourcingGateway, multisourcingGateway);

            var ide = new ItemDetalhe()
            {
                IDItemDetalhe = 1
            };

            var ids = new ItemDetalhe()
            {
                IDItemDetalhe = 2
            };

            var f = new Fornecedor();
            var fp = new FornecedorParametro() { IDFornecedorParametro = 1 };
            f.Parametros.Add(fp);

            var msAnterior = new Multisourcing()
            {
                IDMultisourcing = 1,
                vlPercentual = 25,
                IDCD = 1,
                cdUsuarioAlteracao = 1
            };

            var msPosterior = new Multisourcing()
            {
                IDMultisourcing = 1,
                vlPercentual = 100,
                IDCD = 1,
                cdUsuarioAlteracao = 1,
                Fornecedor = f,
                ItemDetalheEntrada = ide,
                ItemDetalheSaida = ids
            };

            logMultisourcingService.Logar(TpOperacao.Inclusao, msAnterior, msPosterior, "Teste");

            Assert.IsNotNull(msPosterior.ItemDetalheEntrada);

            multisourcingGateway.AssertWasNotCalled(g => g.ObterComItemDetalhesEFp(msPosterior.IDMultisourcing));

            logMultisourcingService.Logar(TpOperacao.Inclusao, msPosterior, null, "Teste");

            Assert.IsNotNull(msPosterior.ItemDetalheEntrada);

            multisourcingGateway.AssertWasNotCalled(g => g.ObterComItemDetalhesEFp(msPosterior.IDMultisourcing));
        }

        [Test]
        public void Logar_MultisourcingAnteriorEPosteriorAAlteracao_EfetuarLogObtendoDetalhesDoMultisourcing()
        {
            var multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
            var logMultiSourcingGateway = MockRepository.GenerateMock<ILogMultiSourcingGateway>();
            var logMultisourcingService = new LogMultiSourcingService(logMultiSourcingGateway, multisourcingGateway);

            var ide = new ItemDetalhe()
            {
                IDItemDetalhe = 1
            };

            var ids = new ItemDetalhe()
            {
                IDItemDetalhe = 2
            };

            var f = new Fornecedor();
            var fp = new FornecedorParametro() { IDFornecedorParametro = 1 };
            f.Parametros.Add(fp);

            var msAnterior = new Multisourcing()
            {
                IDMultisourcing = 1,
                vlPercentual = 25,
                IDCD = 1,
                cdUsuarioAlteracao = 1
            };

            var msPosterior = new Multisourcing()
            {
                IDMultisourcing = 1,
                vlPercentual = 100,
                IDCD = 1,
                cdUsuarioAlteracao = 1
            };

            multisourcingGateway.Expect(g => g.ObterComItemDetalhesEFp(1)).Return(new Multisourcing()
            {
                IDMultisourcing = 1,
                Fornecedor = f,
                ItemDetalheEntrada = ide,
                ItemDetalheSaida = ids
            });

            logMultisourcingService.Logar(TpOperacao.Inclusao, msAnterior, msPosterior, "Teste");

            Assert.IsNull(msPosterior.ItemDetalheEntrada);

            multisourcingGateway.AssertWasCalled(g => g.ObterComItemDetalhesEFp(msAnterior.IDMultisourcing));
        }
    }
}
