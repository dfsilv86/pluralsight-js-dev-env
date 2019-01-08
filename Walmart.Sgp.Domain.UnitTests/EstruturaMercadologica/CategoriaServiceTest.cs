using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Data.Memory;
using Walmart.Sgp.Infrastructure.Framework.Domain;

namespace Walmart.Sgp.Domain.UnitTests.EstruturaMercadologica
{
    [TestFixture]
    [Category("Domain")]
    public class CategoriaServiceTest
    {
        #region Fields
        private ICategoriaGateway m_categoriaGateway;
        private CategoriaService m_target;
        #endregion

        #region Initialize
        [SetUp]
        public void InitializeTest()
        {
            m_categoriaGateway = new MemoryCategoriaGateway();
            m_target = new CategoriaService(m_categoriaGateway);
        }
        #endregion

        #region Tests
        [Test]
        public void ObterIDCategoria_CdSistemaECdCategoria_CategoriaAtiva()
        {
            m_categoriaGateway.Insert(new Categoria { Id = 1, cdSistema = 1, cdCategoria = 11, dsCategoria = "11 desativada", blAtivo = false });
            m_categoriaGateway.Insert(new Categoria { Id = 2, cdSistema = 1, cdCategoria = 11, dsCategoria = "11 ativada sistema 1", blAtivo = true });
            m_categoriaGateway.Insert(new Categoria { Id = 3, cdSistema = 2, cdCategoria = 11, dsCategoria = "11 ativada sistema 2", blAtivo = true });

            var actual = m_target.ObterIDCategoria(1, 11);
            Assert.AreEqual(2, actual);
        }

        [Test]
        public void ObterPorSistema_CdSistema_CategoriasAtivas()
        {
            m_categoriaGateway.Insert(new Categoria { Id = 1, cdSistema = 1, cdCategoria = 11, dsCategoria = "11 desativada", blAtivo = false });
            m_categoriaGateway.Insert(new Categoria { Id = 2, cdSistema = 1, cdCategoria = 11, dsCategoria = "11 ativada sistema 1", blAtivo = true });
            m_categoriaGateway.Insert(new Categoria { Id = 3, cdSistema = 2, cdCategoria = 11, dsCategoria = "11 ativada sistema 2", blAtivo = true });
            m_categoriaGateway.Insert(new Categoria { Id = 4, cdSistema = 2, cdCategoria = 12, dsCategoria = "12 ativada sistema 2", blAtivo = true });

            var actual = m_target.ObterPorSistema(1);
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(2, actual.First().Id);

            actual = m_target.ObterPorSistema(2);
            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(3, actual.First().Id);
            Assert.AreEqual(4, actual.Last().Id);
        }
        #endregion
    }
}
