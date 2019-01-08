using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Infrastructure.Data.Memory;
using Walmart.Sgp.Infrastructure.Framework.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Item
{
    [TestFixture]
    [Category("Domain")]
    [Category("Item")]
    public class ItemRelacionamentoServiceTest
    {
        #region Fields
        private MemoryItemRelacionamentoGateway m_relacionamentoItemGateway;
        private MemoryRelacionamentoItemPrincipalHistGateway m_histPrincipalGateway;
        private MemoryRelacionamentoItemSecundarioHistGateway m_histSecundarioGateway;
        private IMultisourcingGateway m_multisourcingGateway;
        private IItemDetalheService m_itemDetalheService;
        #endregion

        #region Initialize
        [SetUp]
        public void InitializeTest()
        {
            m_relacionamentoItemGateway = new MemoryItemRelacionamentoGateway();
            m_histPrincipalGateway = new MemoryRelacionamentoItemPrincipalHistGateway();
            m_histSecundarioGateway = new MemoryRelacionamentoItemSecundarioHistGateway();
            m_itemDetalheService = MockRepository.GenerateMock<IItemDetalheService>();
            m_multisourcingGateway = MockRepository.GenerateMock<IMultisourcingGateway>();
        }
        #endregion

        #region Tests
        [Test]
        public void Salvar_Novo_InsereManipulado()
        {
            m_itemDetalheService.Expect(i => i.AlterarManipulado(111, TipoManipulado.Pai));
            m_itemDetalheService.Expect(i => i.AlterarManipulado(112, TipoManipulado.Derivado));
            m_itemDetalheService.Expect(i => i.AlterarManipulado(113, TipoManipulado.Derivado));
            m_itemDetalheService.Expect(i => i.ObterPorId(111)).Return(new ItemDetalhe { Id = 111, TpVinculado = TipoVinculado.Saida, TpReceituario = TipoReceituario.Insumo, TpManipulado = TipoManipulado.NaoDefinido });
            m_itemDetalheService.Expect(i => i.ObterPorId(112)).Return(new ItemDetalhe { Id = 112, TpVinculado = TipoVinculado.Saida, TpReceituario = TipoReceituario.Insumo, TpManipulado = TipoManipulado.NaoDefinido });
            m_itemDetalheService.Expect(i => i.ObterPorId(113)).Return(new ItemDetalhe { Id = 113, TpVinculado = TipoVinculado.Saida, TpReceituario = TipoReceituario.Insumo, TpManipulado = TipoManipulado.NaoDefinido });

            var target = new ItemRelacionamentoService(m_relacionamentoItemGateway, m_histPrincipalGateway, m_histSecundarioGateway, m_itemDetalheService, m_multisourcingGateway);
            var entidade = new RelacionamentoItemPrincipal
            {
                TipoRelacionamento = TipoRelacionamento.Manipulado,
                IDItemDetalhe = 111,
                ItemDetalhe = new ItemDetalhe
                {
                    Id = 111,
                    TpVinculado = TipoVinculado.Saida,
                    TpReceituario = TipoReceituario.Insumo,
                    TpManipulado = TipoManipulado.NaoDefinido
                },
                StatusReprocessamentoCusto = StatusReprocessamentoCusto.Concluido,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[] {
                    new RelacionamentoItemSecundario {
                        IDItemDetalhe = 112,
                        ItemDetalhe = new ItemDetalhe
                        {
                            Id = 112,
                            TpVinculado = TipoVinculado.Saida,
                            TpReceituario = TipoReceituario.Insumo,
                            TpManipulado = TipoManipulado.NaoDefinido
                        }
                    },
                    new RelacionamentoItemSecundario {
                        IDItemDetalhe = 113,
                        ItemDetalhe = new ItemDetalhe
                        {
                            Id = 113,
                            TpVinculado = TipoVinculado.Saida,
                            TpReceituario = TipoReceituario.Insumo,
                            TpManipulado = TipoManipulado.NaoDefinido
                        }
                    }
                }
            };

            m_multisourcingGateway.Expect(g => g.VerificaRelacionamentoExistente(entidade)).Return(false);

            target.Salvar(entidade);
            Assert.AreNotEqual(0, entidade.Id);
            Assert.AreEqual(StatusReprocessamentoCusto.Agendado, entidade.StatusReprocessamentoCusto);
            Assert.IsTrue(entidade.DhCadastro.HasValue);
            Assert.IsFalse(entidade.DhAlteracao.HasValue);
            Assert.AreEqual(1, m_relacionamentoItemGateway.Entities.Count);
            Assert.AreEqual(1, m_histPrincipalGateway.Entities.Count);
            Assert.AreEqual(TipoRelacionamento.Manipulado.Value, m_histPrincipalGateway.Entities.First().IDTipoRelacionamento);
            Assert.AreEqual(2, m_histSecundarioGateway.Entities.Count);

            m_itemDetalheService.VerifyAllExpectations();
        }

        [Test]
        public void Salvar_Novo_InsereVinculado()
        {
            m_itemDetalheService.Expect(i => i.AlterarVinculado(111, TipoVinculado.Saida));
            m_itemDetalheService.Expect(i => i.AlterarVinculado(112, TipoVinculado.Entrada));
            m_itemDetalheService.Expect(i => i.AlterarVinculado(113, TipoVinculado.Entrada));
            m_itemDetalheService.Expect(i => i.ObterPorId(111)).Return(new ItemDetalhe { Id = 111, TpVinculado = TipoVinculado.NaoDefinido, TpReceituario = TipoReceituario.NaoDefinido, TpManipulado = TipoManipulado.NaoDefinido });
            m_itemDetalheService.Expect(i => i.ObterPorId(112)).Return(new ItemDetalhe { Id = 112, TpVinculado = TipoVinculado.NaoDefinido, TpReceituario = TipoReceituario.NaoDefinido, TpManipulado = TipoManipulado.NaoDefinido });
            m_itemDetalheService.Expect(i => i.ObterPorId(113)).Return(new ItemDetalhe { Id = 113, TpVinculado = TipoVinculado.NaoDefinido, TpReceituario = TipoReceituario.NaoDefinido, TpManipulado = TipoManipulado.NaoDefinido });

            var target = new ItemRelacionamentoService(m_relacionamentoItemGateway, m_histPrincipalGateway, m_histSecundarioGateway, m_itemDetalheService, m_multisourcingGateway);
            var entidade = new RelacionamentoItemPrincipal
            {
                TipoRelacionamento = TipoRelacionamento.Vinculado,
                IDItemDetalhe = 111,
                ItemDetalhe = new ItemDetalhe
                {
                    Id = 111,
                    TpVinculado = TipoVinculado.NaoDefinido,
                    TpReceituario = TipoReceituario.NaoDefinido,
                    TpManipulado = TipoManipulado.NaoDefinido
                },
                StatusReprocessamentoCusto = StatusReprocessamentoCusto.Concluido,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[] {
                    new RelacionamentoItemSecundario {
                        IDItemDetalhe = 112,
                        TpItem = TipoItemEntrada.Insumo,
                        ItemDetalhe = new ItemDetalhe
                        {
                            Id = 112,
                            TpVinculado = TipoVinculado.NaoDefinido,
                            TpReceituario = TipoReceituario.NaoDefinido,
                            TpManipulado = TipoManipulado.NaoDefinido
                        }
                    },
                    new RelacionamentoItemSecundario {
                        IDItemDetalhe = 113,
                        TpItem = TipoItemEntrada.Insumo,
                        ItemDetalhe = new ItemDetalhe
                        {
                            Id = 113,
                            TpVinculado = TipoVinculado.NaoDefinido,
                            TpReceituario = TipoReceituario.NaoDefinido,
                            TpManipulado = TipoManipulado.NaoDefinido
                        }
                    }
                }
            };

            m_multisourcingGateway.Expect(g => g.VerificaRelacionamentoExistente(entidade)).Return(false);

            target.Salvar(entidade);
            Assert.AreNotEqual(0, entidade.Id);
            Assert.AreEqual(StatusReprocessamentoCusto.Agendado, entidade.StatusReprocessamentoCusto);
            Assert.IsTrue(entidade.DhCadastro.HasValue);
            Assert.IsFalse(entidade.DhAlteracao.HasValue);
            Assert.AreEqual(1, m_relacionamentoItemGateway.Entities.Count);
            Assert.AreEqual(1, m_histPrincipalGateway.Entities.Count);
            Assert.AreEqual(TipoRelacionamento.Vinculado.Value, m_histPrincipalGateway.Entities.First().IDTipoRelacionamento);
            Assert.AreEqual(2, m_histSecundarioGateway.Entities.Count);

            m_itemDetalheService.VerifyAllExpectations();
        }

        [Test]
        public void Salvar_NovoReceituarioSemInsumo_NaoSalva()
        {
            var target = new ItemRelacionamentoService(m_relacionamentoItemGateway, m_histPrincipalGateway, m_histSecundarioGateway, m_itemDetalheService, m_multisourcingGateway);
            var entidade = new RelacionamentoItemPrincipal
            {
                TipoRelacionamento = TipoRelacionamento.Receituario,
                ItemDetalhe = new ItemDetalhe
                {
                    Id = 111,
                    TpVinculado = TipoVinculado.NaoDefinido,
                    TpReceituario = TipoReceituario.Insumo,
                    TpManipulado = TipoManipulado.NaoDefinido
                },
                StatusReprocessamentoCusto = StatusReprocessamentoCusto.Concluido,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[] {
                    new RelacionamentoItemSecundario {
                        IDItemDetalhe = 112,
                        TpItem = TipoItemEntrada.Embalagem,
                        ItemDetalhe = new ItemDetalhe
                        {
                            Id = 112,
                            TpVinculado = TipoVinculado.NaoDefinido,
                            TpReceituario = TipoReceituario.NaoDefinido,
                            TpManipulado = TipoManipulado.NaoDefinido
                        }
                    },
                    new RelacionamentoItemSecundario {
                        IDItemDetalhe = 113,
                        TpItem = TipoItemEntrada.Embalagem,
                        ItemDetalhe = new ItemDetalhe
                        {
                            Id = 113,
                            TpVinculado = TipoVinculado.NaoDefinido,
                            TpReceituario = TipoReceituario.NaoDefinido,                            
                            TpManipulado = TipoManipulado.NaoDefinido
                        }
                    }
                }
            };

            m_multisourcingGateway.Expect(g => g.VerificaRelacionamentoExistente(entidade)).Return(false);

            Assert.Throws<NotSatisfiedSpecException>(() =>
            {
                target.Salvar(entidade);
            });
        }

        [Test]
        [Category("Bug")]
        public void Salvar_Novo_InsereReceituario()
        {
            m_itemDetalheService.Expect(i => i.ObterPorId(111)).Return(new ItemDetalhe { Id = 111, TpVinculado = TipoVinculado.NaoDefinido, TpReceituario = TipoReceituario.NaoDefinido, TpManipulado = TipoManipulado.NaoDefinido });
            m_itemDetalheService.Expect(i => i.ObterPorId(112)).Return(new ItemDetalhe { Id = 112, TpVinculado = TipoVinculado.NaoDefinido, TpReceituario = TipoReceituario.NaoDefinido, TpManipulado = TipoManipulado.NaoDefinido });

            // Bug 4699:Receituario: Ao Vincular um Item Transformado como Insumo, está setando tpReceituario = I
            var itemDetalheTpReceituarioTransformado = new ItemDetalhe { Id = 113, TpVinculado = TipoVinculado.NaoDefinido, TpReceituario = TipoReceituario.Transformado, TpManipulado = TipoManipulado.NaoDefinido };
            m_itemDetalheService.Expect(i => i.ObterPorId(113)).Return(itemDetalheTpReceituarioTransformado);            
            m_itemDetalheService.Expect(e => e.AlterarReceituario(112, TipoReceituario.Insumo));
            m_itemDetalheService.Expect(e => e.AlterarReceituario(113, TipoReceituario.Insumo)).Repeat.Never();
            //

            var target = new ItemRelacionamentoService(m_relacionamentoItemGateway, m_histPrincipalGateway, m_histSecundarioGateway, m_itemDetalheService, m_multisourcingGateway);
            var entidade = new RelacionamentoItemPrincipal
            {
                TipoRelacionamento = TipoRelacionamento.Receituario,
                IDItemDetalhe = 111,
                ItemDetalhe = new ItemDetalhe
                {
                    Id = 111,
                    TpVinculado = TipoVinculado.NaoDefinido,
                    TpReceituario = TipoReceituario.NaoDefinido,
                    TpManipulado = TipoManipulado.NaoDefinido
                },
                StatusReprocessamentoCusto = StatusReprocessamentoCusto.Concluido,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[] {
                    new RelacionamentoItemSecundario {
                        IDItemDetalhe = 112,
                        TpItem = TipoItemEntrada.Insumo,
                        ItemDetalhe = new ItemDetalhe
                        {
                            Id = 112,
                            TpVinculado = TipoVinculado.NaoDefinido,
                            TpReceituario = TipoReceituario.NaoDefinido,
                            TpManipulado = TipoManipulado.NaoDefinido,
                        }
                    },
                    new RelacionamentoItemSecundario {
                        IDItemDetalhe = 113,
                        TpItem = TipoItemEntrada.Insumo,
                        ItemDetalhe = new ItemDetalhe
                        {
                            Id = 113,
                            TpVinculado = TipoVinculado.NaoDefinido,
                            TpReceituario = TipoReceituario.NaoDefinido,                            
                            TpManipulado = TipoManipulado.NaoDefinido
                        }
                    }
                }
            };            

            m_multisourcingGateway.Expect(g => g.VerificaRelacionamentoExistente(entidade)).Return(false);

            target.Salvar(entidade);
            Assert.AreNotEqual(0, entidade.Id);
            Assert.AreEqual(StatusReprocessamentoCusto.Agendado, entidade.StatusReprocessamentoCusto);
            Assert.IsTrue(entidade.DhCadastro.HasValue);
            Assert.IsFalse(entidade.DhAlteracao.HasValue);
            Assert.AreEqual(1, m_relacionamentoItemGateway.Entities.Count);
            Assert.AreEqual(1, m_histPrincipalGateway.Entities.Count);
            Assert.AreEqual(TipoRelacionamento.Receituario.Value, m_histPrincipalGateway.Entities.First().IDTipoRelacionamento);
            Assert.AreEqual(2, m_histSecundarioGateway.Entities.Count);            

            m_itemDetalheService.VerifyAllExpectations();
        }

        [Test]
        public void Salvar_Existe_Atualiza()
        {
            var existente = new RelacionamentoItemPrincipal
            {
                TipoRelacionamento = TipoRelacionamento.Manipulado,
                ItemDetalhe = new ItemDetalhe
                {
                    Id = 111,
                    TpVinculado = TipoVinculado.Saida,
                    TpReceituario = TipoReceituario.Insumo,
                    TpManipulado = TipoManipulado.NaoDefinido
                },
                StatusReprocessamentoCusto = StatusReprocessamentoCusto.Concluido,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[] { 
                    new RelacionamentoItemSecundario {
                        Id = 2,
                        IDItemDetalhe = 112,
                        ItemDetalhe = new ItemDetalhe
                        {
                            Id = 112,
                            TpVinculado = TipoVinculado.Saida,
                            TpReceituario = TipoReceituario.Insumo,
                            TpManipulado = TipoManipulado.NaoDefinido
                        }
                    },
                    new RelacionamentoItemSecundario {
                        Id = 3,
                        IDItemDetalhe = 113,
                        ItemDetalhe = new ItemDetalhe
                        {
                            Id = 113,
                            TpVinculado = TipoVinculado.Saida,
                            TpReceituario = TipoReceituario.Insumo,
                            TpManipulado = TipoManipulado.NaoDefinido
                        }
                    }
                }
            };

            m_relacionamentoItemGateway.Insert(existente);

            m_itemDetalheService.Expect(i => i.AlterarManipulado(111, TipoManipulado.Pai));
            m_itemDetalheService.Expect(i => i.AlterarManipulado(112, TipoManipulado.Derivado));
            m_itemDetalheService.Expect(i => i.AlterarManipulado(113, TipoManipulado.Derivado));
            m_itemDetalheService.Expect(i => i.ObterPorId(111)).Return(new ItemDetalhe { Id = 111, TpVinculado = TipoVinculado.NaoDefinido, TpReceituario = TipoReceituario.NaoDefinido, TpManipulado = TipoManipulado.Pai });
            m_itemDetalheService.Expect(i => i.ObterPorId(112)).Return(new ItemDetalhe { Id = 112, TpVinculado = TipoVinculado.Saida, TpReceituario = TipoReceituario.Insumo, TpManipulado = TipoManipulado.NaoDefinido });
            m_itemDetalheService.Expect(i => i.ObterPorId(113)).Return(new ItemDetalhe { Id = 113, TpVinculado = TipoVinculado.Saida, TpReceituario = TipoReceituario.Insumo, TpManipulado = TipoManipulado.NaoDefinido });

            var target = new ItemRelacionamentoService(m_relacionamentoItemGateway, m_histPrincipalGateway, m_histSecundarioGateway, m_itemDetalheService, m_multisourcingGateway);
            var entidade = new RelacionamentoItemPrincipal
            {
                TipoRelacionamento = TipoRelacionamento.Manipulado,
                Id = existente.Id,
                IDItemDetalhe = 111,
                ItemDetalhe = new ItemDetalhe
                {
                    Id = 111,
                    TpVinculado = TipoVinculado.NaoDefinido,
                    TpReceituario = TipoReceituario.NaoDefinido,
                    TpManipulado = TipoManipulado.Pai
                },
                StatusReprocessamentoCusto = StatusReprocessamentoCusto.Concluido,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[] { 
                    new RelacionamentoItemSecundario
                    {
                        Id = existente.RelacionamentoSecundario.First().Id,
                        IDItemDetalhe = 112,
                        ItemDetalhe = new ItemDetalhe
                        {
                            Id = 112,
                            TpVinculado = TipoVinculado.Saida,
                            TpReceituario = TipoReceituario.Insumo,
                            TpManipulado = TipoManipulado.NaoDefinido
                        }
                    },
                    new RelacionamentoItemSecundario {
                        Id = existente.RelacionamentoSecundario.Last().Id,
                        IDItemDetalhe = 113,
                        ItemDetalhe = new ItemDetalhe
                        {
                            Id = 113,
                            TpVinculado = TipoVinculado.Saida,
                            TpReceituario = TipoReceituario.Insumo,
                            TpManipulado = TipoManipulado.NaoDefinido
                        }
                    }
                }
            };

            m_multisourcingGateway.Expect(g => g.VerificaRelacionamentoExistente(entidade)).Return(false);

            target.Salvar(entidade);
            Assert.AreEqual(existente.Id, entidade.Id);
            Assert.AreEqual(StatusReprocessamentoCusto.Agendado, entidade.StatusReprocessamentoCusto);
            Assert.IsTrue(entidade.DhAlteracao.HasValue);
            Assert.AreEqual(1, m_relacionamentoItemGateway.Entities.Count);
            Assert.AreEqual(1, m_histPrincipalGateway.Entities.Count);
            Assert.AreEqual(2, m_histSecundarioGateway.Entities.Count);

            m_itemDetalheService.VerifyAllExpectations();
        }

        [Test]
        public void Salvar_ExisteMasEstaRemovendoUmRelacionamentoSecundario_AtualizaERemoveRelacionamento()
        {
            var existente = new RelacionamentoItemPrincipal
            {
                ItemDetalhe = new ItemDetalhe { Id = 111 },
                StatusReprocessamentoCusto = StatusReprocessamentoCusto.Concluido,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[] { 
                    new RelacionamentoItemSecundario { IDItemDetalhe = 112, TpItem = TipoItemEntrada.Insumo },
                    new RelacionamentoItemSecundario { IDItemDetalhe = 113, TpItem = TipoItemEntrada.Insumo }
                }
            };

            m_relacionamentoItemGateway.Insert(existente);

            m_itemDetalheService.Expect(i => i.AlterarManipulado(111, TipoManipulado.Pai));
            m_itemDetalheService.Expect(i => i.AlterarManipulado(112, TipoManipulado.Derivado));
            m_itemDetalheService.Expect(i => i.AlterarManipulado(113, TipoManipulado.NaoDefinido));
            m_itemDetalheService.Expect(i => i.ObterPorId(111)).Return(new ItemDetalhe { Id = 111, TpVinculado = TipoVinculado.NaoDefinido, TpReceituario = TipoReceituario.NaoDefinido, TpManipulado = TipoManipulado.Pai });
            m_itemDetalheService.Expect(i => i.ObterPorId(112)).Return(new ItemDetalhe { Id = 112, TpVinculado = TipoVinculado.Saida, TpReceituario = TipoReceituario.Insumo, TpManipulado = TipoManipulado.NaoDefinido });

            var target = new ItemRelacionamentoService(m_relacionamentoItemGateway, m_histPrincipalGateway, m_histSecundarioGateway, m_itemDetalheService, m_multisourcingGateway);
            var entidade = new RelacionamentoItemPrincipal
            {
                Id = existente.Id,
                IDItemDetalhe = 111,
                TipoRelacionamento = TipoRelacionamento.Manipulado,
                ItemDetalhe = new ItemDetalhe
                {
                    Id = 111,
                    TpVinculado = TipoVinculado.NaoDefinido,
                    TpReceituario = TipoReceituario.NaoDefinido,
                    TpManipulado = TipoManipulado.Pai
                },
                StatusReprocessamentoCusto = StatusReprocessamentoCusto.Concluido,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[] {
                     new RelacionamentoItemSecundario
                    {
                        IDItemDetalhe = 112,
                        ItemDetalhe = new ItemDetalhe
                        {
                            Id = 112,
                            TpVinculado = TipoVinculado.Saida,
                            TpReceituario = TipoReceituario.Insumo,
                            TpManipulado = TipoManipulado.NaoDefinido
                        }
                    },
                }
            };

            m_multisourcingGateway.Expect(g => g.VerificaRelacionamentoExistente(entidade)).Return(false);

            target.Salvar(entidade);
            Assert.AreEqual(existente.Id, entidade.Id);
            Assert.AreEqual(StatusReprocessamentoCusto.Agendado, entidade.StatusReprocessamentoCusto);
            Assert.IsTrue(entidade.DhAlteracao.HasValue);
            Assert.AreEqual(1, m_relacionamentoItemGateway.Entities.Count);
            Assert.AreEqual(1, m_histPrincipalGateway.Entities.Count);
            Assert.AreEqual(1, m_histSecundarioGateway.Entities.Count(e => e.TpAcao == TipoAcao.Alteracao && e.IDItemDetalhe == 112));
            Assert.AreEqual(1, m_histSecundarioGateway.Entities.Count(e => e.TpAcao == TipoAcao.Exclusao && e.IDItemDetalhe == 113));

            m_itemDetalheService.VerifyAllExpectations();
        }

        [Test]
        public void Remover_PrincipalESecundariosManipulados_RemovePrincipalSecundariosERegistraHistorico()
        {
            var existente = new RelacionamentoItemPrincipal
            {
                ItemDetalhe = new ItemDetalhe { Id = 111 },
                TipoRelacionamento = TipoRelacionamento.Manipulado,
                StatusReprocessamentoCusto = StatusReprocessamentoCusto.Concluido,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[] { 
					new RelacionamentoItemSecundario { IDItemDetalhe = 112, TpItem = TipoItemEntrada.Insumo },
					new RelacionamentoItemSecundario { IDItemDetalhe = 113, TpItem = TipoItemEntrada.Insumo }
				}
            };

            m_relacionamentoItemGateway.Insert(existente);

            m_itemDetalheService.Expect(i => i.AlterarManipulado(111, TipoManipulado.NaoDefinido));
            m_itemDetalheService.Expect(i => i.AlterarManipulado(112, TipoManipulado.NaoDefinido));
            m_itemDetalheService.Expect(i => i.AlterarManipulado(113, TipoManipulado.NaoDefinido));

            var target = new ItemRelacionamentoService(m_relacionamentoItemGateway, m_histPrincipalGateway, m_histSecundarioGateway, m_itemDetalheService, m_multisourcingGateway);

            m_multisourcingGateway.Expect(g => g.VerificaRelacionamentoExistente(existente)).Return(false);

            target.Remover(existente.Id);
            Assert.AreEqual(0, m_relacionamentoItemGateway.Entities.Count);
            Assert.AreEqual(1, m_histPrincipalGateway.Entities.Count(e => e.TpAcao == TipoAcao.Exclusao && e.IDItemDetalhe == 111));
            Assert.AreEqual(1, m_histPrincipalGateway.Entities.Count);
            Assert.AreEqual(1, m_histSecundarioGateway.Entities.Count(e => e.TpAcao == TipoAcao.Exclusao && e.IDItemDetalhe == 112));
            Assert.AreEqual(1, m_histSecundarioGateway.Entities.Count(e => e.TpAcao == TipoAcao.Exclusao && e.IDItemDetalhe == 113));
            Assert.AreEqual(2, m_histSecundarioGateway.Entities.Count);

            m_itemDetalheService.VerifyAllExpectations();
        }

        [Test]
        public void Remover_PrincipalESecundariosVinculados_RemovePrincipalSecundariosERegistraHistorico()
        {
            var existente = new RelacionamentoItemPrincipal
            {
                ItemDetalhe = new ItemDetalhe { Id = 111 },
                TipoRelacionamento = TipoRelacionamento.Vinculado,
                StatusReprocessamentoCusto = StatusReprocessamentoCusto.Concluido,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[] { 
					new RelacionamentoItemSecundario { IDItemDetalhe = 112, TpItem = TipoItemEntrada.Insumo },
					new RelacionamentoItemSecundario { IDItemDetalhe = 113, TpItem = TipoItemEntrada.Insumo }
				}
            };

            m_relacionamentoItemGateway.Insert(existente);

            m_itemDetalheService.Expect(i => i.AlterarVinculado(111, TipoVinculado.NaoDefinido));
            m_itemDetalheService.Expect(i => i.AlterarVinculado(112, TipoVinculado.NaoDefinido));
            m_itemDetalheService.Expect(i => i.AlterarVinculado(113, TipoVinculado.NaoDefinido));

            var target = new ItemRelacionamentoService(m_relacionamentoItemGateway, m_histPrincipalGateway, m_histSecundarioGateway, m_itemDetalheService, m_multisourcingGateway);

            m_multisourcingGateway.Expect(g => g.VerificaRelacionamentoExistente(existente)).Return(false);

            target.Remover(existente.Id);
            Assert.AreEqual(0, m_relacionamentoItemGateway.Entities.Count);
            Assert.AreEqual(1, m_histPrincipalGateway.Entities.Count(e => e.TpAcao == TipoAcao.Exclusao && e.IDItemDetalhe == 111));
            Assert.AreEqual(1, m_histPrincipalGateway.Entities.Count);
            Assert.AreEqual(1, m_histSecundarioGateway.Entities.Count(e => e.TpAcao == TipoAcao.Exclusao && e.IDItemDetalhe == 112));
            Assert.AreEqual(1, m_histSecundarioGateway.Entities.Count(e => e.TpAcao == TipoAcao.Exclusao && e.IDItemDetalhe == 113));
            Assert.AreEqual(2, m_histSecundarioGateway.Entities.Count);

            m_itemDetalheService.VerifyAllExpectations();
        }

        [Test]
        public void Remover_PrincipalESecundariosReceituarios_RemovePrincipalSecundariosERegistraHistorico()
        {
            var existente = new RelacionamentoItemPrincipal
            {
                ItemDetalhe = new ItemDetalhe { Id = 111 },
                TipoRelacionamento = TipoRelacionamento.Receituario,
                StatusReprocessamentoCusto = StatusReprocessamentoCusto.Concluido,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[] { 
					new RelacionamentoItemSecundario { IDItemDetalhe = 112, TpItem = TipoItemEntrada.Insumo },
					new RelacionamentoItemSecundario { IDItemDetalhe = 113, TpItem = TipoItemEntrada.Insumo }
				}
            };

            m_relacionamentoItemGateway.Insert(existente);

            m_itemDetalheService.Expect(i => i.AlterarReceituario(111, TipoReceituario.NaoDefinido));
            m_itemDetalheService.Expect(i => i.AlterarReceituario(112, TipoReceituario.NaoDefinido));
            m_itemDetalheService.Expect(i => i.AlterarReceituario(113, TipoReceituario.NaoDefinido));

            var target = new ItemRelacionamentoService(m_relacionamentoItemGateway, m_histPrincipalGateway, m_histSecundarioGateway, m_itemDetalheService, m_multisourcingGateway);

            m_multisourcingGateway.Expect(g => g.VerificaRelacionamentoExistente(existente)).Return(false);

            target.Remover(existente.Id);
            Assert.AreEqual(0, m_relacionamentoItemGateway.Entities.Count);
            Assert.AreEqual(1, m_histPrincipalGateway.Entities.Count(e => e.TpAcao == TipoAcao.Exclusao && e.IDItemDetalhe == 111));
            Assert.AreEqual(1, m_histPrincipalGateway.Entities.Count);
            Assert.AreEqual(1, m_histSecundarioGateway.Entities.Count(e => e.TpAcao == TipoAcao.Exclusao && e.IDItemDetalhe == 112));
            Assert.AreEqual(1, m_histSecundarioGateway.Entities.Count(e => e.TpAcao == TipoAcao.Exclusao && e.IDItemDetalhe == 113));
            Assert.AreEqual(2, m_histSecundarioGateway.Entities.Count);

            m_itemDetalheService.VerifyAllExpectations();
        }

        [Test]
        public void PesquisarPorTipoRelacionamento_CdItemZero_NotSatisfied()
        {
            var target = new ItemRelacionamentoService(null, null, null, null, null);

            Assert.Throws(typeof(NotSatisfiedSpecException), () =>
            {
                target.PesquisarPorTipoRelacionamento(TipoRelacionamento.Vinculado, null, 0, null, null, null, null, 1, null, new Infrastructure.Framework.Domain.Paging());
            });
        }
        #endregion
    }
}
