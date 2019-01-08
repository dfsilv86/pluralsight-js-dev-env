using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.Gerenciamento
{
    [TestFixture]
    [Category("Domain")]
    public class FornecedorServiceTest
    {
        #region Fields
        private IFornecedorGateway m_lojaGateway;
        private FornecedorService m_target;
        #endregion

        #region Initialize
        [SetUp]
        public void InitializeTest()
        {
            m_lojaGateway = MockRepository.GenerateMock<IFornecedorGateway>();
            m_target = new FornecedorService(m_lojaGateway);
        }
        #endregion

        #region Tests
        [Test]
        public void ObterPorSistemaCodigo_SitemaECodigo_Fornecedor()
        {
            m_lojaGateway.Expect(g => g.ObterPorSistemaCodigo(1, 128703)).Return(new Fornecedor());

            Assert.NotNull(m_target.ObterPorSistemaCodigo(1, 128703));
        }

        [Test]
        public void ObterListaPorSistemaCodigoNome_SistemaCodigoNuloENome_Fornecedores()
        {
            Paging paging = new Paging();

            m_lojaGateway.Expect(g => g.ObterListaPorSistemaCodigoNome(1, null, "WALM", paging)).Return(new Fornecedor[] {
                new Fornecedor(),
                new Fornecedor()
            });

            Assert.AreEqual(2, m_target.ObterListaPorSistemaCodigoNome(1, null, "WALM", paging).Count());
        }
        #endregion
    }
}
