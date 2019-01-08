using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.EstruturaMercadologica;
using Walmart.Sgp.Domain.Gerenciamento;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Item;
using Walmart.Sgp.Domain.MultisourcingVendor;
using Walmart.Sgp.Domain.MultisourcingVendor.Specs;

namespace Walmart.Sgp.Domain.UnitTests.MultisourcingVendor.Specs
{
    [TestFixture]
    [Category("Domain"), Category("Multisourcing")]
    public class ItemEntradaModalidadeXDockSpecTest
    {
        [Test]
        public void IsSatisfiedBy_NaoPossuiParametrosParaValidacao1_True()
        {
            var multisourcing = new Multisourcing()
            {
                ItemDetalheEntrada = null,
                IDCD = 0
            };

            var target = new ItemEntradaModalidadeXDockSpec(m => null);
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_NaoPossuiParametrosParaValidacao2_True()
        {
            var multisourcing = new Multisourcing()
            {
                ItemDetalheEntrada = new ItemDetalhe
                {
                    IDItemDetalhe = 0
                },
                IDCD = 0
            };

            var target = new ItemEntradaModalidadeXDockSpec(m => null);
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ValorTipoReabastecimentoCrossDocking3_True()
        {
            var multisourcing = new Multisourcing()
            {
                ItemDetalheEntrada = new ItemDetalhe
                {
                    IDItemDetalhe = 1
                },
                IDCD = 1
            };

            var target = new ItemEntradaModalidadeXDockSpec(m => ValorTipoReabastecimento.CrossDocking3);
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ValorTipoReabastecimentoCrossDocking33_True()
        {
            var multisourcing = new Multisourcing()
            {
                ItemDetalheEntrada = new ItemDetalhe
                {
                    IDItemDetalhe = 1
                },
                IDCD = 1
            };

            var target = new ItemEntradaModalidadeXDockSpec(m => ValorTipoReabastecimento.CrossDocking33);
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ValorTipoReabastecimentoCrossDocking94_True()
        {
            var multisourcing = new Multisourcing()
            {
                ItemDetalheEntrada = new ItemDetalhe
                {
                    IDItemDetalhe = 1
                },
                IDCD = 1
            };

            var target = new ItemEntradaModalidadeXDockSpec(m => ValorTipoReabastecimento.CrossDocking94);
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ValorTipoReabastecimentoInvalido1_False()
        {
            var multisourcing = new Multisourcing()
            {
                ItemDetalheEntrada = new ItemDetalhe
                {
                    IDItemDetalhe = 1
                },
                IDCD = 1
            };

            var target = new ItemEntradaModalidadeXDockSpec(m => null);
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsFalse(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ValorTipoReabastecimentoInvalido2_False()
        {
            var multisourcing = new Multisourcing()
            {
                ItemDetalheEntrada = new ItemDetalhe
                {
                    IDItemDetalhe = 1
                },
                IDCD = 1
            };

            var target = new ItemEntradaModalidadeXDockSpec(m => ValorTipoReabastecimento.Dsd37);
            var actual = target.IsSatisfiedBy(new[] { multisourcing });

            Assert.IsFalse(actual.Satisfied);
        }
    }
}