using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;

namespace Walmart.Sgp.Domain.UnitTests.EstruturaMercadologica
{
    [TestFixture]
    [Category("Domain")]
    public class BandeiraServiceTest
    {
        #region Fields
        private IBandeiraGateway m_bandeiraGateway;
        private IPermissaoService m_permissaoService;
        private BandeiraService m_target;
        #endregion

        #region Initialize
        [SetUp]
        public void InitializeTest()
        {
            m_bandeiraGateway = MockRepository.GenerateMock<IBandeiraGateway>();
            m_permissaoService = MockRepository.GenerateMock<IPermissaoService>();
            m_target = new BandeiraService(m_bandeiraGateway, m_permissaoService);
        }
        #endregion

        #region Tests
        [Test]
        public void ContarPorUsuario_IdUsuario_NumeroDeBandeiras()
        {
            m_bandeiraGateway.Expect(g => g.Count(null, new { })).IgnoreArguments().Return(10).Repeat.Once();
            m_bandeiraGateway.Expect(g => g.Count(null, new { })).IgnoreArguments().Return(20).Repeat.Once();

            Assert.AreEqual(10, m_target.ContarPorUsuario(1));
            Assert.AreEqual(20, m_target.ContarPorUsuario(2));
        }

        [Test]
        public void Salvar_BandeiraValidaNova_BandeiraEDetalheERegioesEDistritosForamInseridos()
        {
            var bandeira = new Bandeira
            {
                DsBandeira = "Bandeira",
                SgBandeira = "BD",
                Detalhes = new BandeiraDetalhe[] { 
                    new BandeiraDetalhe(),
                    new BandeiraDetalhe()
                },
                Formato = new Formato(),
                Regioes = new Regiao[] { new Regiao { Distritos = new Distrito[] { new Distrito() } } }
            };

            m_bandeiraGateway.Expect(e => e.Insert(bandeira));
            m_permissaoService.Expect(e => e.InserirPermissaoBandeira(0, 0));
            m_target.Salvar(bandeira);

            Assert.IsTrue(bandeira.CdUsuarioCriacao.HasValue);
            Assert.AreNotEqual(DateTime.MinValue, bandeira.DhCriacao);
            Assert.AreEqual("U", bandeira.TpCusto);

            m_bandeiraGateway.VerifyAllExpectations();
            m_permissaoService.VerifyAllExpectations();
        }

        [Test]
        public void Salvar_BandeiraValidaJaExistente_BandeiraEDetalheERegioesEDistritosForamAtualizados()
        {
            var bandeira = new Bandeira
            {
                IDBandeira = 1,
                DsBandeira = "Bandeira",
                SgBandeira = "BD",
                Detalhes = new BandeiraDetalhe[] { 
                    new BandeiraDetalhe(),
                    new BandeiraDetalhe()
                },
                Formato = new Formato(),
                Regioes = new Regiao[] { new Regiao { Distritos = new Distrito[] { new Distrito() } } }
            };

            m_bandeiraGateway.Expect(e => e.Update(bandeira));
            m_target.Salvar(bandeira);

            Assert.IsTrue(bandeira.CdUsuarioAtualizacao.HasValue);
            Assert.IsTrue(bandeira.DhAtualizacao.HasValue);
            m_bandeiraGateway.VerifyAllExpectations();
            m_permissaoService.VerifyAllExpectations();
        }

        [Test]
        public void Remover_IDBandeira_RemoverPermissoesEBandeira()
        {
            m_permissaoService.Expect(e => e.RemoverPermissoesBandeira(1));
            m_bandeiraGateway.Expect(e => e.Delete(1));
            m_target.Remover(1);
            
            m_bandeiraGateway.VerifyAllExpectations();
            m_permissaoService.VerifyAllExpectations();
        }

        [Test]
        public void ObterPorUsuarioESistema_Formato1EUsuario1_QuantidadeBandeira()
        {
            m_bandeiraGateway.Expect(g => g.ObterPorUsuarioESistema(1, 1, null, null)).Return(new [] {
                new BandeiraResumo
                {
                    CdSistema = 1,
                    DsBandeira = "Walmart",
                    IDBandeira = 1,
                    IDFormato = 2
                }});

            var target = new BandeiraService(m_bandeiraGateway, null);

            var result = target.ObterPorUsuarioESistema(1, 1, null);

            Assert.AreEqual(result.Count(), 1);
        }

        [Test]
        public void ObterPorUsuarioERegiaoAdministrativa_RegiaoAdm1ESistema1_QuantidadeBandeira()
        {
            m_bandeiraGateway.Expect(g => g.ObterPorUsuarioESistema(1, 1, null, 1)).Return(new[] {
                new BandeiraResumo
                {
                    CdSistema = 1,
                    DsBandeira = "Walmart",
                    IDBandeira = 1,
                    IDFormato = 2
                }});

            var target = new BandeiraService(m_bandeiraGateway, null);

            var result = target.ObterPorUsuarioERegiaoAdministrativa(1, 1, 1);

            Assert.AreEqual(result.Count(), 1);
        }
        #endregion
    }
}
