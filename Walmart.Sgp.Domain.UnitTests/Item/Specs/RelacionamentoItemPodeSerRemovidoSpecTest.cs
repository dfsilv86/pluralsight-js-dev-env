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
    public class RelacionamentoItemPodeSerRemovidoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_VinculadoItemDetalhePrincipalEhReceituarioInsumoOuManipuladoPai_False()
        {
            var target = new RelacionamentoItemPodeSerRemovidoSpec((s) => { return false; });
            var actual = target.IsSatisfiedBy(new RelacionamentoItemPrincipal
            {
                TipoRelacionamento = TipoRelacionamento.Vinculado,
                ItemDetalhe = new ItemDetalhe
                {
                    TpReceituario = TipoReceituario.Insumo,
                    TpManipulado = TipoManipulado.Derivado
                }
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.CannotDeleteWhenParentManipulatedOrInputRecipe, actual.Reason);

            actual = target.IsSatisfiedBy(new RelacionamentoItemPrincipal
            {
                TipoRelacionamento = TipoRelacionamento.Vinculado,
                ItemDetalhe = new ItemDetalhe
                {
                    TpReceituario = TipoReceituario.Transformado,
                    TpManipulado = TipoManipulado.Pai
                }
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.CannotDeleteWhenParentManipulatedOrInputRecipe, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_NaoEhVinculadoItemDetalhePrincipalEhReceituarioInsumoOuManipuladoPai_False()
        {
            var target = new RelacionamentoItemPodeSerRemovidoSpec((s) => { return false; });
            var actual = target.IsSatisfiedBy(new RelacionamentoItemPrincipal
            {
                TipoRelacionamento = TipoRelacionamento.Manipulado,
                ItemDetalhe = new ItemDetalhe
                {
                    TpReceituario = TipoReceituario.Insumo,
                    TpManipulado = TipoManipulado.Derivado
                }
            });

            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new RelacionamentoItemPrincipal
            {
                TipoRelacionamento = TipoRelacionamento.Receituario,
                ItemDetalhe = new ItemDetalhe
                {
                    TpReceituario = TipoReceituario.Transformado,
                    TpManipulado = TipoManipulado.Pai
                }
            });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_VinculadoItemDetalhePrincipalNaoEhReceituarioInsumoEManipuladoPai_True()
        {
            var target = new RelacionamentoItemPodeSerRemovidoSpec((s) => { return false; });
            var actual = target.IsSatisfiedBy(new RelacionamentoItemPrincipal
            {
                TipoRelacionamento = TipoRelacionamento.Vinculado,
                ItemDetalhe = new ItemDetalhe
                {
                    TpReceituario = TipoReceituario.NaoDefinido,
                    TpManipulado = TipoManipulado.NaoDefinido
                }
            });
            Assert.IsTrue(actual.Satisfied);

            actual = target.IsSatisfiedBy(new RelacionamentoItemPrincipal
            {
                TipoRelacionamento = TipoRelacionamento.Vinculado,
                ItemDetalhe = new ItemDetalhe
                {
                    TpReceituario = TipoReceituario.Transformado,
                    TpManipulado = TipoManipulado.Derivado
                }
            });
            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItemDetalheComMultisourcing_False()
        {
            var target = new RelacionamentoItemPodeSerRemovidoSpec((s) => { return true; });
            var actual = target.IsSatisfiedBy(new RelacionamentoItemPrincipal
            {
                TipoRelacionamento = TipoRelacionamento.Receituario,
                ItemDetalhe = new ItemDetalhe
                {
                    TpReceituario = TipoReceituario.NaoDefinido,
                    TpManipulado = TipoManipulado.NaoDefinido
                }
            });

            Assert.IsFalse(actual.Satisfied);

            Assert.AreEqual(Texts.CantDeleteBecauseItemHaveMultisourcing, actual.Reason);
        }
    }
}
