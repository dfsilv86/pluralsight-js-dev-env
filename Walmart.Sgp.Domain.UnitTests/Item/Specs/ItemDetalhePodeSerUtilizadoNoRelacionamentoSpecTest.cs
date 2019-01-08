using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;
using Walmart.Sgp.Domain.Acessos;
using Walmart.Sgp.Domain.Acessos.Specs;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Infrastructure.Framework.Globalization;
using Walmart.Sgp.Domain.Item.Specs;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.UnitTests.Item.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class ItemDetalhePodeSerUtilizadoNoRelacionamentoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_Deletado_False()
        {
            var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MockRepository.GenerateMock<IItemRelacionamentoService>(), new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Manipulado }, true);
            var actual = target.IsSatisfiedBy(new ItemDetalhe { TpStatus = TipoStatusItem.Deletado });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemIsNotEnabled.With(Texts.OutputItem), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_Inativo_False()
        {
			var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MockRepository.GenerateMock<IItemRelacionamentoService>(), new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Manipulado }, false);
            var actual = target.IsSatisfiedBy(new ItemDetalhe { TpStatus = TipoStatusItem.Inativo,  });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemIsNotEnabled.With(Texts.InputItem), actual.Reason);
        }
       
        [Test]
        public void IsSatisfiedBy_AtivoManipuladoNaoEhVinculadoDiffEntradaEReceituarioDiffTransformadoEManipuladoNaoDefinido_False()
        {
			var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MockRepository.GenerateMock<IItemRelacionamentoService>(), new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Manipulado }, true);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Entrada,
                TpReceituario = TipoReceituario.Insumo,
                TpManipulado = TipoManipulado.NaoDefinido
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemCannotBeUsedAsManipulated.With(Texts.OutputItem), actual.Reason);
          
            actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Saida,
                TpReceituario = TipoReceituario.Transformado,
                TpManipulado = TipoManipulado.NaoDefinido
            });
            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemCannotBeUsedAsManipulated.With(Texts.OutputItem), actual.Reason);

            actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Saida,
                TpReceituario = TipoReceituario.Insumo,
                TpManipulado = TipoManipulado.Pai
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemCannotBeUsedAsManipulated.With(Texts.OutputItem), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_AtivoManipuladoSaidaVinculadoDiffEntradaEReceituarioDiffTransformadoEManipuladoNaoDefinidoMasEstaEmOutroRelacionamento_False()
        {
            var service = MockRepository.GenerateMock<IItemRelacionamentoService>();
            service.Expect(s => s.ContarItemDetalheComoSaidaEmOutrosRelacionamentos(1, 10)).Return(1);

			var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(service, new RelacionamentoItemPrincipal { Id = 1, TipoRelacionamento = TipoRelacionamento.Manipulado }, true);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {                
                IDItemDetalhe = 10,
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Saida,
                TpReceituario = TipoReceituario.Insumo,
                TpManipulado = TipoManipulado.NaoDefinido
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemIsInAnotherRelationship.With(Texts.OutputItem), actual.Reason);
        }

        /// <summary>
        /// Bug 4698:BUG - Relac. Manipulado - Erro ao salvar item já relacionado.
        /// </summary>
        [Test]
        [Category("Bug")]
        public void IsSatisfiedBy_ManipuladoEntradaETpVinculadoSaidaETpReceituarioNaoDefinidoETpManipuladoPai_False()
        {
            var service = MockRepository.GenerateMock<IItemRelacionamentoService>();
            service.Expect(s => s.ContarItemDetalheComoSaidaEmOutrosRelacionamentos(1, 10)).Return(1);

            var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(service, new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Manipulado }, false);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                IDItemDetalhe = 10,
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Saida,
                TpReceituario = TipoReceituario.NaoDefinido,
                TpManipulado = TipoManipulado.Pai
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemCannotBeUsedAsManipulated.With(Texts.InputItem), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_AtivoManipuladoVinculadoDiffEntradaEReceituarioDiffTransformadoEManipuladoNaoDefinidoEntrada_True()
        {
			var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MockRepository.GenerateMock<IItemRelacionamentoService>(), new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Manipulado }, true);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Saida,
                TpReceituario = TipoReceituario.Insumo,
                TpManipulado = TipoManipulado.NaoDefinido
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_AtivoManipuladoVinculadoDiffEntradaEReceituarioDiffTransformadoEManipuladoNaoDefinidoSaida_True()
        {
            var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MockRepository.GenerateMock<IItemRelacionamentoService>(), new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Manipulado }, false);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.NaoDefinido,
                TpReceituario = TipoReceituario.Insumo,
                TpManipulado = TipoManipulado.NaoDefinido
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_AtivoReceituarioESaidaVinculadoDiffNaoDefinidoOuReceituarioDiffNaoDefinidoOuManipuladoNaoDefinido_False()
        {
			var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MockRepository.GenerateMock<IItemRelacionamentoService>(), new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Receituario }, true);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Entrada,
                TpReceituario = TipoReceituario.NaoDefinido,
                TpManipulado = TipoManipulado.NaoDefinido
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemIsInAnotherRelationship.With(Texts.OutputItem), actual.Reason);

            actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.NaoDefinido,
                TpReceituario = TipoReceituario.Insumo,
                TpManipulado = TipoManipulado.NaoDefinido
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemIsInAnotherRelationship.With(Texts.OutputItem), actual.Reason);

            actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.NaoDefinido,
                TpReceituario = TipoReceituario.NaoDefinido,
                TpManipulado = TipoManipulado.Pai
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemIsInAnotherRelationship.With(Texts.OutputItem), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_AtivoReceiturarioESaidaTodosNaoDefinido_True()
        {
			var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MockRepository.GenerateMock<IItemRelacionamentoService>(), new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Receituario }, true);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.NaoDefinido,
                TpReceituario = TipoReceituario.NaoDefinido,
                TpManipulado = TipoManipulado.NaoDefinido
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_AtivoReceiturarioNaoSaidaTodosNaoDefinido_True()
        {
			var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MockRepository.GenerateMock<IItemRelacionamentoService>(), new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Receituario }, false);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.NaoDefinido,
                TpReceituario = TipoReceituario.NaoDefinido,
                TpManipulado = TipoManipulado.NaoDefinido
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItemSaidaIgualAItemDetalhe_False()
        {
			var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MockRepository.GenerateMock<IItemRelacionamentoService>(), new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Receituario }, true);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.NaoDefinido,
                TpReceituario = TipoReceituario.NaoDefinido,
                TpManipulado = TipoManipulado.NaoDefinido
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_AtivoVinculadoESaidaVinculadoDifNaoDefinidoOuReceituarioDiffTransformadoOuManipuladoDerivado_False()
        {
			var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MockRepository.GenerateMock<IItemRelacionamentoService>(), new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Vinculado }, true);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Entrada,
                TpReceituario = TipoReceituario.Transformado,
                TpManipulado = TipoManipulado.Derivado
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemIsInAnotherRelationship.With(Texts.OutputItem), actual.Reason);

            actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.NaoDefinido,
                TpReceituario = TipoReceituario.Insumo,
                TpManipulado = TipoManipulado.Derivado
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemIsInAnotherRelationship.With(Texts.OutputItem), actual.Reason);

            actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.NaoDefinido,
                TpReceituario = TipoReceituario.Transformado,
                TpManipulado = TipoManipulado.Pai
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemIsInAnotherRelationship.With(Texts.OutputItem), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_AtivoVinculadoNaoESaidaAlgumDiffNaoDefinido_False()
        {
			var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MockRepository.GenerateMock<IItemRelacionamentoService>(), new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Vinculado }, false);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Entrada,
                TpReceituario = TipoReceituario.NaoDefinido,
                TpManipulado = TipoManipulado.NaoDefinido
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemIsInAnotherRelationship.With(Texts.InputItem), actual.Reason);

            actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.NaoDefinido,
                TpReceituario = TipoReceituario.Insumo,
                TpManipulado = TipoManipulado.NaoDefinido
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemIsInAnotherRelationship.With(Texts.InputItem), actual.Reason);

            actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.NaoDefinido,
                TpReceituario = TipoReceituario.NaoDefinido,
                TpManipulado = TipoManipulado.Derivado
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemIsInAnotherRelationship.With(Texts.InputItem), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_AtivoVinculadoESaidaVinculadoNaoDefinidoOuReceituarioDiffTransformadoOuManipuladoDiffDerivado_True()
        {
			var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MockRepository.GenerateMock<IItemRelacionamentoService>(), new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Vinculado }, true);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.NaoDefinido,
                TpReceituario = TipoReceituario.Insumo,
                TpManipulado = TipoManipulado.Pai
            });

            Assert.IsTrue(actual.Satisfied);
        }

        /// <summary>
        /// Bug 4700:Vinculado: Está permitindo adicionar um Item Secundário com tpVinculado = S.
        /// </summary>
        [Test]
        [Category("Bug")]
        public void IsSatisfiedBy_VinculadoSecundarioJahETpVinculadoSaida_False()
        {
            var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MockRepository.GenerateMock<IItemRelacionamentoService>(), new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Vinculado }, false);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Saida,
                TpReceituario = TipoReceituario.NaoDefinido,
                TpManipulado = TipoManipulado.NaoDefinido
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemIsInAnotherRelationship.With(Texts.InputItem), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_AtivoVinculadoNaoESaidaTudoNaoDefinido_True()
        {
			var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(MockRepository.GenerateMock<IItemRelacionamentoService>(), new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Vinculado }, false);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.NaoDefinido,
                TpReceituario = TipoReceituario.NaoDefinido,
                TpManipulado = TipoManipulado.NaoDefinido
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItemDetalheJahExisteNaLista_False()
        {
            var service = MockRepository.GenerateMock<IItemRelacionamentoService>();            

            var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(service, 
                new RelacionamentoItemPrincipal { 
                    TipoRelacionamento = TipoRelacionamento.Vinculado,
                    RelacionamentoSecundario = new RelacionamentoItemSecundario[] { 
                        new RelacionamentoItemSecundario { IDItemDetalhe = 10 }
                    }
                }, true);

            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                IDItemDetalhe = 10,
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Saida,
                TpReceituario = TipoReceituario.Insumo,
                TpManipulado = TipoManipulado.NaoDefinido
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.InputItemAlreadyAddedToOutputItem, actual.Reason);
        }

        /// <summary>
        /// Para validar o Bug 3317: Relacionamento Receituário: Não está permitindo adicionar um item com o relacionamento tpVinculado = S, tpManipulado = P e tpReceituario = Null.
        /// </summary>
        [Test]
        [Category("Bug")]
        public void IsSatisfiedBy_InsercaoEhReceituarioEVinculadoSaidaEManipuladoPaiEReceituarioNaoDefinido_True()
        {
            var service = MockRepository.GenerateMock<IItemRelacionamentoService>();            
            var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(service, new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Receituario }, false);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                IDItemDetalhe = 10,
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Saida,
                TpManipulado = TipoManipulado.Pai,
                TpReceituario = TipoReceituario.NaoDefinido
            });

            Assert.IsTrue(actual.Satisfied);
        }

        /// <summary>
        /// Para validar o Bug 3317: Relacionamento Receituário: Não está permitindo adicionar um item com o relacionamento tpVinculado = S, tpManipulado = P e tpReceituario = Null.
        /// </summary>
        [Test]
        [Category("Bug")]
        public void IsSatisfiedBy_AlteracaoEhReceituarioEVinculadoSaidaEManipuladoPaiEReceituarioNaoDefinido_True()
        {
            var service = MockRepository.GenerateMock<IItemRelacionamentoService>();
            service.Expect(s => s.ContarItemDetalheComoSaidaEmOutrosRelacionamentos(1, 10)).Return(1);
            var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(service, new RelacionamentoItemPrincipal { Id = 1, TipoRelacionamento = TipoRelacionamento.Receituario }, false);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                IDItemDetalhe = 10,
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Saida,
                TpManipulado = TipoManipulado.Pai,
                TpReceituario = TipoReceituario.Insumo
            });

            Assert.IsTrue(actual.Satisfied);
        }


        [Test]
        public void IsSatisfiedBy_InsercaoEhManipuladoEVinculadoSaidaEManipuladoNaoDefinidoEReceituarioInsumo_True()
        {
            var service = MockRepository.GenerateMock<IItemRelacionamentoService>();            
            var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(service, new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Manipulado }, false);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                IDItemDetalhe = 10,
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Saida,
                TpManipulado = TipoManipulado.NaoDefinido,
                TpReceituario = TipoReceituario.Insumo
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_AlteracaoEhManipuladoEVinculadoSaidaEManipuladoPaiEReceituarioInsumo_True()
        {
            var service = MockRepository.GenerateMock<IItemRelacionamentoService>();
            var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(service, new RelacionamentoItemPrincipal { Id = 1, TipoRelacionamento = TipoRelacionamento.Manipulado }, false);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                IDItemDetalhe = 10,
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Saida,
                TpManipulado = TipoManipulado.Pai,
                TpReceituario = TipoReceituario.Insumo
            });

            Assert.IsTrue(actual.Satisfied);
        }

        /// <summary>
        /// Bug 4668:Bug - Não está permitindo salvar relacionamento entre itens Transformados
        /// </summary>
        [Test]
        [Category("Bug")]
        public void IsSatisfiedBy_ReceituarioComSecundarioTransformado_True()
        {
            var service = MockRepository.GenerateMock<IItemRelacionamentoService>();
            var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(service, new RelacionamentoItemPrincipal { Id = 1, TipoRelacionamento = TipoRelacionamento.Receituario }, false);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {
                IDItemDetalhe = 10,
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Saida,
                TpManipulado = TipoManipulado.Pai,
                TpReceituario = TipoReceituario.Transformado
            });

            Assert.IsTrue(actual.Satisfied);
        }

        /// <summary>
        /// Bug 4660:Relac. Manipulado: Não está permitindo relacionar um Item Saída como um item Pai.
        /// </summary>
        [Test]
        [Category("Bug")]
        public void IsSatisfiedBy_ReceituarioPrincipalVpVinculadoSaidaEOutrosNaoDefinido_True()
        {
            var service = MockRepository.GenerateMock<IItemRelacionamentoService>();
            var target = new ItemDetalhePodeSerUtilizadoNoRelacionamentoSpec(service, new RelacionamentoItemPrincipal { TipoRelacionamento = TipoRelacionamento.Manipulado }, false);
            var actual = target.IsSatisfiedBy(new ItemDetalhe
            {                
                TpStatus = TipoStatusItem.Ativo,
                TpVinculado = TipoVinculado.Saida,
                TpManipulado = TipoManipulado.NaoDefinido,
                TpReceituario = TipoReceituario.NaoDefinido
            });

            Assert.IsTrue(actual.Satisfied);
        }
    }
}
