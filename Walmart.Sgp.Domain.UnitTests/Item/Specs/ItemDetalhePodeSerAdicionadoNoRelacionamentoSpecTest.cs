﻿using System;
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
    public class ItemDetalhePodeSerAdicionadoNoRelacionamentoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItemSaidaIgualItemEntrada_False()
        {
            var principal = new RelacionamentoItemPrincipal
            {
                IDItemDetalhe = 111
            };
            var target = new ItemDetalhePodeSerAdicionadoNoRelacionamentoSpec(principal);
            var actual = target.IsSatisfiedBy(new ItemDetalhe { IDItemDetalhe = 111, CdItem = 999 });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.SecondaryItemShouldBeDiffThanMainItem.With(999), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_ItemSaidaJaExisteNoRelacionamentoSecundario_False()
        {
            var principal = new RelacionamentoItemPrincipal
            {
                IDItemDetalhe = 111,
                ItemDetalhe = new ItemDetalhe { IDItemDetalhe = 111, IDDepartamento = 1 },
                RelacionamentoSecundario = new RelacionamentoItemSecundario[]
                {
                    new RelacionamentoItemSecundario { IDItemDetalhe = 112 },
                    new RelacionamentoItemSecundario { IDItemDetalhe = 113 }
                }
            };
            var target = new ItemDetalhePodeSerAdicionadoNoRelacionamentoSpec(principal);
            var actual = target.IsSatisfiedBy(new ItemDetalhe { IDItemDetalhe = 113, IDDepartamento = 1, CdItem = 999 });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.SecondaryItemAlreadyInRelationship.With(999), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_ItemSaidaComDepartamentoDiferenteDoPrincipalEDiffVinculado_False()
        {
            var principal = new RelacionamentoItemPrincipal
            {
                IDItemDetalhe = 111,
                ItemDetalhe = new ItemDetalhe { IDItemDetalhe = 111, IDDepartamento = 1 },
                TipoRelacionamento = TipoRelacionamento.Receituario,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[]
                {
                    new RelacionamentoItemSecundario { IDItemDetalhe = 112 },
                    new RelacionamentoItemSecundario { IDItemDetalhe = 113 }
                }
            };

            // Receituario.
            var target = new ItemDetalhePodeSerAdicionadoNoRelacionamentoSpec(principal);
            var actual = target.IsSatisfiedBy(new ItemDetalhe { IDItemDetalhe = 114, IDDepartamento = 2 });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.SecondaryItemWithDepartmentDiffThanMainItem, actual.Reason);

            // Manipulado.
            principal.TipoRelacionamento = TipoRelacionamento.Manipulado;
            actual = target.IsSatisfiedBy(new ItemDetalhe { IDItemDetalhe = 114, IDDepartamento = 2 });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.SecondaryItemWithDepartmentDiffThanMainItem, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_ItemSaidaComDepartamentoDiferenteDoPrincipalEIgualVinculado_True()
        {
            var principal = new RelacionamentoItemPrincipal
            {
                IDItemDetalhe = 111,
                ItemDetalhe = new ItemDetalhe { IDItemDetalhe = 111, IDDepartamento = 1 },
                TipoRelacionamento = TipoRelacionamento.Vinculado,
                RelacionamentoSecundario = new RelacionamentoItemSecundario[]
                {
                    new RelacionamentoItemSecundario { IDItemDetalhe = 112 },
                    new RelacionamentoItemSecundario { IDItemDetalhe = 113 }
                }
            };
            var target = new ItemDetalhePodeSerAdicionadoNoRelacionamentoSpec(principal);
            var actual = target.IsSatisfiedBy(new ItemDetalhe { IDItemDetalhe = 114, IDDepartamento = 2 });

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItemSaidaDiffItemEntradaENaoExisteNoRelacionamentoSecundario_True()
        {
            var principal = new RelacionamentoItemPrincipal
            {
                IDItemDetalhe = 111,
                ItemDetalhe = new ItemDetalhe { IDItemDetalhe = 111, IDDepartamento = 1 },
                RelacionamentoSecundario = new RelacionamentoItemSecundario[]
                {
                    new RelacionamentoItemSecundario { IDItemDetalhe = 112 },
                    new RelacionamentoItemSecundario { IDItemDetalhe = 113 }
                }
            };
            var target = new ItemDetalhePodeSerAdicionadoNoRelacionamentoSpec(principal);
            var actual = target.IsSatisfiedBy(new ItemDetalhe { IDItemDetalhe = 114, IDDepartamento = 1 });

            Assert.IsTrue(actual.Satisfied);
        }
    }

}