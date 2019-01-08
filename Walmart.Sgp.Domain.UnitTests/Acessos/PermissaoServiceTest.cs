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
using Walmart.Sgp.Infrastructure.Data.Memory;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Infrastructure.Framework.Domain;
using Walmart.Sgp.Infrastructure.Framework.Runtime;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Acessos
{
    [TestFixture]
    [Category("Domain")]
    public class PermissaoServiceTest
    {
        #region Fields
        private MemoryPermissaoGateway m_permissaoGateway;
        private IParametroService m_parametroService;
        private IPermissaoBandeiraGateway m_permissaoBandeiraGateway;
        private PermissaoService m_target;
        private IBandeiraGateway m_bandeiraGateway;
        private ILojaGateway m_lojaGateway;
        private IPermissaoLojaGateway m_permissaoLojaGateway;
        private IRuntimeUser m_usuario;
        #endregion

        #region Initialize
        [SetUp]
        public void InitializeTest()
        {
            m_permissaoGateway = new MemoryPermissaoGateway();
            m_permissaoBandeiraGateway = new MemoryPermissaoBandeiraGateway();
            m_parametroService = MockRepository.GenerateMock<IParametroService>();
            m_parametroService.Expect(p => p.Obter()).Return(new Parametro { cdUsuarioAdministrador = 0 });
            m_bandeiraGateway = MockRepository.GenerateMock<IBandeiraGateway>();
            m_lojaGateway = MockRepository.GenerateMock<ILojaGateway>();
            m_permissaoLojaGateway = MockRepository.GenerateMock<IPermissaoLojaGateway>();
            m_usuario = MockRepository.GenerateMock<IRuntimeUser>();

            m_target = new PermissaoService(m_permissaoGateway, m_parametroService, m_permissaoBandeiraGateway, m_permissaoLojaGateway, m_bandeiraGateway, m_lojaGateway);
        }
        #endregion

        #region Tests
        [Test]
        public void Salvar_PermissaoNovaEUsuarioSemPermissoes_Inserido()
        {
            var permissao = new Permissao 
            {
                IDUsuario = 1,
                Lojas =  new PermissaoLoja[] { new PermissaoLoja(), new PermissaoLoja(), new PermissaoLoja() },
                blRecebeNotificaoOperacoes = true,
                blRecebeNotificaoFinanceiro = false 
            };

            m_target.Salvar(permissao);

            var actual = m_permissaoGateway.Entities.First();
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.IDUsuario);
            Assert.AreEqual(0, actual.Bandeiras.Count());
            Assert.AreEqual(3, actual.Lojas.Count());
            Assert.AreEqual(DateTime.Today, actual.dhCriacao.Date);
            Assert.IsTrue(actual.blRecebeNotificaoOperacoes);
            Assert.IsFalse(actual.blRecebeNotificaoFinanceiro);

            Assert.AreEqual(1, m_permissaoGateway.Count("IDUsuario = @IDUsuario", actual));
        }

        [Test]
        public void Salvar_PermissaoExistente_Atualizado()
        {
            var permissaoInserida = new Permissao
            {
                IDUsuario = 1,
                Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira { IDBandeira = 1 }, new PermissaoBandeira { IDBandeira = 2 } },
                blRecebeNotificaoOperacoes = true,
                blRecebeNotificaoFinanceiro = false
            };
            m_target.Salvar(permissaoInserida);

            var outraPermissao = new Permissao
            {
                IDUsuario = 2,
                Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira { IDBandeira = 1 }, new PermissaoBandeira { IDBandeira = 2 } },
                blRecebeNotificaoOperacoes = true,
                blRecebeNotificaoFinanceiro = false
            };
            m_target.Salvar(outraPermissao);

            var permissao = new Permissao
            {
                IDPermissao = permissaoInserida.Id,
                IDUsuario = 1,
                Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira { IDBandeira = 1 }, new PermissaoBandeira { IDBandeira = 2 }, new PermissaoBandeira { IDBandeira = 3 } },
                blRecebeNotificaoOperacoes = false,
                blRecebeNotificaoFinanceiro = true
            };

            m_target.Salvar(permissao);
            var actual = m_permissaoGateway.Entities.First(f => f.IDPermissao == permissao.IDPermissao);

            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.IDUsuario);
            Assert.AreEqual(3, actual.Bandeiras.Count());
            Assert.AreEqual(0, actual.Lojas.Count());
            Assert.AreEqual(DateTime.Today, actual.dhAlteracao.Date);
            Assert.IsFalse(actual.blRecebeNotificaoOperacoes);
            Assert.IsTrue(actual.blRecebeNotificaoFinanceiro);

            actual = m_permissaoGateway.Find("IDUsuario = @IDUsuario", actual).Single();

            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.IDUsuario);

            var bandeiras = actual.Bandeiras.ToArray();
            Assert.AreEqual(3, bandeiras.Length);
            Assert.AreEqual(1, bandeiras[0].IDBandeira);
            Assert.AreNotEqual(0, bandeiras[0].Id);
            Assert.AreEqual(2, bandeiras[1].IDBandeira);
            Assert.AreNotEqual(0, bandeiras[1].Id);
            Assert.AreEqual(3, bandeiras[2].IDBandeira);
            Assert.AreNotEqual(0, bandeiras[2].Id);

            var lojas = actual.Lojas.ToArray();
            Assert.AreEqual(0, lojas.Length);
            Assert.IsFalse(actual.blRecebeNotificaoOperacoes);
            Assert.IsTrue(actual.blRecebeNotificaoFinanceiro);
        }

        [Test]
        public void InserirPermissaoBandeira_Args_Inserido()
        {
            var permissao = new Permissao { IDUsuario = 1 };
            m_permissaoGateway.Insert(permissao);

            var actual = m_target.InserirPermissaoBandeira(1, 2);

            Assert.IsNotNull(actual);
            Assert.AreEqual(permissao.Id, actual.IDPermissao);
            Assert.AreEqual(2, actual.IDBandeira);

            Assert.AreEqual(1, m_permissaoBandeiraGateway.Count("IDPermissao = @IDPermissao AND IDBandeira = @IDBandeira", new { IDPermissao = permissao.Id, IDBandeira = 2 }));
        }

        [Test]
        public void RemoverPermissoesBandeira_IdBandeira_PermissoesRemovidas()
        {
            m_permissaoGateway.Insert(new Permissao { IDUsuario = 1 });
            m_permissaoGateway.Insert(new Permissao { IDUsuario = 2 });

            m_target.InserirPermissaoBandeira(1, 1);
            m_target.InserirPermissaoBandeira(2, 1);
            m_target.InserirPermissaoBandeira(2, 3);

            Assert.AreEqual(2, m_permissaoBandeiraGateway.Count("IDBandeira = @IDBandeira", new { IDBandeira = 1 }));
            Assert.AreEqual(1, m_permissaoBandeiraGateway.Count("IDBandeira = @IDBandeira", new { IDBandeira = 3 }));

            m_target.RemoverPermissoesBandeira(1);
            Assert.AreEqual(0, m_permissaoBandeiraGateway.Count("IDBandeira = @IDBandeira", new { IDBandeira = 1 }));
            Assert.AreEqual(1, m_permissaoBandeiraGateway.Count("IDBandeira = @IDBandeira", new { IDBandeira = 3 }));

            m_target.RemoverPermissoesBandeira(3);
            Assert.AreEqual(0, m_permissaoBandeiraGateway.Count("IDBandeira = @IDBandeira", new { IDBandeira = 1 }));
            Assert.AreEqual(0, m_permissaoBandeiraGateway.Count("IDBandeira = @IDBandeira", new { IDBandeira = 3 }));
        }        

        [Test]
        public void ContarPermissoesPorUsuario_IdUsuario_QuantidadePermissoes()
        {
            m_permissaoGateway.Insert(new Permissao() { IDUsuario = 1 });
            m_permissaoGateway.Insert(new Permissao() { IDUsuario = 2 });
            m_permissaoGateway.Insert(new Permissao() { IDUsuario = 1 });

            Assert.AreEqual(2, m_target.ContarPermissoesPorUsuario(1));
            Assert.AreEqual(1, m_target.ContarPermissoesPorUsuario(2));
        }

        [Test]
        public void TemAcessoAdminMaster_IdUsuarioIgualParametrocdUsuarioAdministrador_False()
        {
            Assert.IsFalse(m_target.TemAcessoAdminMaster(1));
        }

        [Test]
        public void TemAcessoAdminMaster_IdUsuarioIgualParametrocdUsuarioAdministrador_True()
        {
            Assert.IsTrue(m_target.TemAcessoAdminMaster(0));
        }

        [Test]
        public void Pesquisar_Args_Permissoes()
        {
            m_permissaoGateway.Insert(new Permissao() { IDUsuario = 1, Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira { IDBandeira = 11 } }, Lojas = new PermissaoLoja[] { new PermissaoLoja { IDLoja = 111 } } });
            m_permissaoGateway.Insert(new Permissao() { IDUsuario = 2, Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira { IDBandeira = 22 } }, Lojas = new PermissaoLoja[] { new PermissaoLoja { IDLoja = 111 } } });
            m_permissaoGateway.Insert(new Permissao() { IDUsuario = 3, Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira { IDBandeira = 11 } }, Lojas = new PermissaoLoja[] { new PermissaoLoja { IDLoja = 333 } } });

            Assert.AreEqual(3, m_target.Pesquisar(null, null, null).Count());
            Assert.AreEqual(1, m_target.Pesquisar(1, null, null).Count());
            Assert.AreEqual(2, m_target.Pesquisar(null, 11, null).Count());
            Assert.AreEqual(2, m_target.Pesquisar(null, null, 111).Count());
            Assert.AreEqual(1, m_target.Pesquisar(null, 22, null).Count());
            Assert.AreEqual(1, m_target.Pesquisar(null, null, 333).Count());
        }

        [Test]
        public void PesquisarComFilhos_Args_Permissoes()
        {
            var paging = new Paging();

            m_permissaoGateway.Insert(new Permissao() { IDUsuario = 1, Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira { IDBandeira = 11 } }, Lojas = new PermissaoLoja[] { new PermissaoLoja { IDLoja = 111 } } });
            m_permissaoGateway.Insert(new Permissao() { IDUsuario = 2, Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira { IDBandeira = 22 } }, Lojas = new PermissaoLoja[] { new PermissaoLoja { IDLoja = 111 } } });
            m_permissaoGateway.Insert(new Permissao() { IDUsuario = 3, Bandeiras = new PermissaoBandeira[] { new PermissaoBandeira { IDBandeira = 11 } }, Lojas = new PermissaoLoja[] { new PermissaoLoja { IDLoja = 333 } } });

            Assert.AreEqual(3, m_target.PesquisarComFilhos(null, null, null, paging).Count());
            Assert.AreEqual(1, m_target.PesquisarComFilhos(1, null, null, paging).Count());
            Assert.AreEqual(2, m_target.PesquisarComFilhos(null, 11, null, paging).Count());
            Assert.AreEqual(2, m_target.PesquisarComFilhos(null, null, 111, paging).Count());
            Assert.AreEqual(1, m_target.PesquisarComFilhos(null, 22, null, paging).Count());
            Assert.AreEqual(1, m_target.PesquisarComFilhos(null, null, 333, paging).Count());
        }

        [Test]
        public void Remover_UsuarioAdmin_Removido()
        {
            var permissao = new Permissao
            {
                IDUsuario = 1,
                Lojas = new PermissaoLoja[] { new PermissaoLoja(), new PermissaoLoja(), new PermissaoLoja() },
                blRecebeNotificaoOperacoes = true,
                blRecebeNotificaoFinanceiro = false
            };
            
            RuntimeContext.Current = new MemoryRuntimeContext 
            {
                User = new MemoryRuntimeUser
                {
                    IsAdministrator = true
                }
            };

            m_target.Salvar(permissao);
            m_target.Remover(permissao.Id);
            Assert.AreEqual(0, m_permissaoGateway.Count("IDUsuario = @IDUsuario", permissao));
        }

        [Test]
        public void ValidarInclusaoBandeira_BandeiraAtiva_Valido() 
        {
            m_bandeiraGateway.Expect(x => x.Find(string.Empty, string.Empty, new object()))
                .Return(new[] { new Bandeira { BlAtivo = BandeiraStatus.Ativo } })
                .IgnoreArguments();

            m_target.ValidarInclusaoBandeira(1);
        }

        [Test]
        
        public void ValidarInclusaoBandeira_BandeiraNaoEValidaParaInclusao_NaoValido()
        {
            m_bandeiraGateway.Expect(x => x.Find(string.Empty, string.Empty, new object()))
                .Return(new[] { new Bandeira { BlAtivo = BandeiraStatus.Inativo } })
                .IgnoreArguments();

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                m_target.ValidarInclusaoBandeira(1);
            });
        }

        [Test]
        public void ValidarInclusaoLoja_BandeiraAtiva_Valido()
        {
            m_lojaGateway.Expect(x => x.Find(string.Empty, string.Empty, new object()))
                .Return(new[] { new Loja { IDBandeira = 2 } })
                .IgnoreArguments();

            m_bandeiraGateway.Expect(x => x.ObterPorIdLoja(1))
               .Return(new Bandeira { BlAtivo = BandeiraStatus.Ativo });

            m_usuario.Expect(x => x.IsAdministrator).Return(true);
            m_usuario.Expect(x => x.IsHo).Return(false);

            m_target.ValidarInclusaoLoja(m_usuario, 1);
        }

        [Test]
        public void ValidarInclusaoLoja_LojaNaoPossuiBandeira_NaoValido()
        {
            m_lojaGateway.Expect(x => x.Find(string.Empty, string.Empty, new object()))
                .Return(new[] { new Loja { IDBandeira = null } })
                .IgnoreArguments();

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                m_target.ValidarInclusaoLoja(null, 1);
            });
        }

        [Test]
        public void ValidarInclusaoLoja_BandeiraDaLojaInativa_NaoValido()
        {
            m_lojaGateway.Expect(x => x.Find(string.Empty, string.Empty, new object()))
                .Return(new[] { new Loja { IDBandeira = 2 } })
                .IgnoreArguments();

            m_bandeiraGateway.Expect(x => x.ObterPorIdLoja(1))
               .Return(new Bandeira { BlAtivo = BandeiraStatus.Inativo });

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                m_target.ValidarInclusaoLoja(null, 1);
            });
        }

        [Test]
        public void ValidarInclusaoLoja_UsuarioNaoPossuiPermissaoBandeiraLoja_NaoValido()
        {
            m_lojaGateway.Expect(x => x.Find(string.Empty, string.Empty, new object()))
                .Return(new[] { new Loja { IDBandeira = 2 } })
                .IgnoreArguments();

            m_bandeiraGateway.Expect(x => x.ObterPorIdLoja(1))
               .Return(new Bandeira { BlAtivo = BandeiraStatus.Ativo });

            m_usuario.Expect(x => x.IsAdministrator).Return(false);
            m_usuario.Expect(x => x.IsHo).Return(false);

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                m_target.ValidarInclusaoLoja(m_usuario, 1);
            });
        }

        [Test]
        public void PossuiPermissaoManutencao_UsuarioAdministrador_True()
        {
            m_usuario.Expect(x => x.IsAdministrator).Return(true);

            var result = m_target.PossuiPermissaoManutencao(m_usuario);
            Assert.IsTrue(result);
        }

        [Test]
        public void PossuiPermissaoManutencao_UsuarioNaoAdministradorENaoPossuiPermissao_False()
        {
            m_usuario.Expect(x => x.Id).Return(1);
            m_usuario.Expect(x => x.IsAdministrator).Return(false);

            var permissaoGatewayMock = MockRepository.GenerateMock<IPermissaoGateway>();
            
            permissaoGatewayMock.Expect(x => x.ObterPermissoesDoUsuario(1))
                .Throw(new InvalidOperationException());

            var target = new PermissaoService(permissaoGatewayMock, m_parametroService, m_permissaoBandeiraGateway, m_permissaoLojaGateway, m_bandeiraGateway, m_lojaGateway);

            var result = target.PossuiPermissaoManutencao(m_usuario);
            Assert.IsFalse(result);
        }
        #endregion
    }
}