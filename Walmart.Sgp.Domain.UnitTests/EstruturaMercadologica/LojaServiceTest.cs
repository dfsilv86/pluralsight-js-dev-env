using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Domain.Acessos;
using Walmart.Sgp.Infrastructure.Data.Memory;
using Is = Rhino.Mocks.Constraints.Is;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.UnitTests.EstruturaMercadologica
{
    [TestFixture]
    [Category("Domain")]
    public class LojaServiceTest
    {
        #region Fields
        private ILojaGateway m_lojaGateway;
        private LojaService m_target;
        #endregion

        #region Initialize
        [SetUp]
        public void InitializeTest()
        {
            m_lojaGateway = MockRepository.GenerateMock<ILojaGateway>();
            m_target = new LojaService(m_lojaGateway);
        }
        #endregion

        #region Tests
        [Test]
        public void ContarPorUsuario_IdUsuario_NumeroDeLojas()
        {
            m_lojaGateway.Expect(g => g.Count(null, new { })).IgnoreArguments().Return(10).Repeat.Once();
            m_lojaGateway.Expect(g => g.Count(null, new { })).IgnoreArguments().Return(20).Repeat.Once();

            Assert.AreEqual(10, m_target.ContarPorUsuario(1));
            Assert.AreEqual(20, m_target.ContarPorUsuario(2));
        }

        [Test]
        public void Pesquisar_Args_Paging()
        {
            Paging paging = new Paging();

            m_lojaGateway.Expect(g => g.Pesquisar(1, TipoPermissao.PorBandeira, 2, 3, 4, "5", paging)).Return(new Loja[] {
                new Loja(),
                new Loja()
            });

            Assert.AreEqual(2, m_target.Pesquisar(1, 2, 3, 4, "5", paging).Count());
        }

        [Test]
        public void ObterLojasPorBandeira_IdBandeira_Lojas()
        {
            var gateway = new MemoryLojaGateway();
            gateway.Insert(new Loja() { Id = 1, IDBandeira = 1, blCarregaSGP = 1 });
            gateway.Insert(new Loja() { Id = 2, IDBandeira = 1, blCarregaSGP = 1 });
            gateway.Insert(new Loja() { Id = 3, IDBandeira = 2, blCarregaSGP = 1 });

            var target = new LojaService(gateway);
            var actual = target.ObterLojasPorBandeira(1);
            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(1, actual.First().Id);
            Assert.AreEqual(2, actual.Last().Id);

            actual = target.ObterLojasPorBandeira(2);
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(3, actual.First().Id);
        }

        [Test]
        public void ObterPorCdLoja_CdLoja_Loja()
        {
            var gateway = new MemoryLojaGateway();
            gateway.Insert(new Loja() { Id = 1, cdSistema = 1, cdLoja = 1 });
            gateway.Insert(new Loja() { Id = 2, cdSistema = 1, cdLoja = 2 });
            gateway.Insert(new Loja() { Id = 3, cdSistema = 2, cdLoja = 1 });

            var target = new LojaService(gateway);
            var actual = target.ObterPorCdLoja(1, 1);
            Assert.AreEqual(1, actual.Id);

            actual = target.ObterPorCdLoja(2, 1);
            Assert.AreEqual(3, actual.Id);
        }

        [Test]
        public void AlterarLoja_Loja_Ok()
        {
            Loja loja = new Loja() { cdSistema = 1, Bandeira = new Bandeira { IDFormato = 1 }, IDBandeira = 1, Distrito = new Distrito { IDRegiao = 1 }, IDDistrito = 1, TipoArquivoInventario = TipoArquivoInventario.Final };

            var gateway = MockRepository.GenerateMock<ILojaGateway>();
            gateway.Expect(lg => lg.Update(null, loja)).IgnoreArguments().Constraints(Is.Anything(), Is.Equal(loja));

            var target = new LojaService(gateway);

            target.AlterarLoja(loja);

            gateway.VerifyAllExpectations();
        }

        [Test]
        public void AlterarLoja_ValoresFaltantes_Erro()
        {
            Loja loja = new Loja() { IDBandeira = 1, IDDistrito = 1, TipoArquivoInventario = TipoArquivoInventario.Final };

            var gateway = MockRepository.GenerateMock<ILojaGateway>();
            gateway.Expect(lg => lg.Update(null, loja)).IgnoreArguments().Throw(new InvalidOperationException());

            var target = new LojaService(gateway);

            Assert.Throws(typeof(NotSatisfiedSpecException), () =>
            {
                target.AlterarLoja(loja);
            });

            loja = new Loja() { cdSistema = 1, IDDistrito = 1, TipoArquivoInventario = TipoArquivoInventario.Final };

            Assert.Throws(typeof(NotSatisfiedSpecException), () =>
            {
                target.AlterarLoja(loja);
            });

            loja = new Loja() { cdSistema = 1, IDBandeira = 1, TipoArquivoInventario = TipoArquivoInventario.Final };

            Assert.Throws(typeof(NotSatisfiedSpecException), () =>
            {
                target.AlterarLoja(loja);
            });

            loja = new Loja() { cdSistema = 1, IDBandeira = 1, IDDistrito = 1 };

            Assert.Throws(typeof(NotSatisfiedSpecException), () =>
            {
                target.AlterarLoja(loja);
            });
        }
        #endregion
    }
}
