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
    public class ItemDetalhePodeEstarEmOutrosRelacionamentosSpecTest
    {
        [Test]
        public void IsSatisfiedBy_ItemEstaEmOutroRelacionamento_False()
        {
            var itemRelacionamentoService = MockRepository.GenerateMock<IItemRelacionamentoService>();
            itemRelacionamentoService.Expect(o => o.ContarItemDetalheComoSaidaEmOutrosRelacionamentos(1, 1)).IgnoreArguments().Return(1);

            var target = new ItemDetalhePodeEstarEmOutrosRelacionamentosSpec(itemRelacionamentoService, new RelacionamentoItemPrincipal(), true);
            var actual = target.IsSatisfiedBy(new ItemDetalhe());

            itemRelacionamentoService.VerifyAllExpectations();

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ItemIsInAnotherRelationship.With(Texts.OutputItem), actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_ItemEstaEmOutroRelacionamentoMasEhReceituarioEntrada_True()
        {
            var itemRelacionamentoService = MockRepository.GenerateMock<IItemRelacionamentoService>();

            var target = new ItemDetalhePodeEstarEmOutrosRelacionamentosSpec(itemRelacionamentoService, new RelacionamentoItemPrincipal() { TipoRelacionamento = TipoRelacionamento.Receituario }, false);
            var actual = target.IsSatisfiedBy(new ItemDetalhe());

            itemRelacionamentoService.VerifyAllExpectations();

            Assert.IsTrue(actual.Satisfied);
        }

        [Test]
        public void IsSatisfiedBy_ItemNaoEstaEmOutroRelacionamento_True()
        {
            var itemRelacionamentoService = MockRepository.GenerateMock<IItemRelacionamentoService>();
            itemRelacionamentoService.Expect(o => o.ContarItemDetalheComoSaidaEmOutrosRelacionamentos(1, 1)).IgnoreArguments().Return(0);

            var target = new ItemDetalhePodeEstarEmOutrosRelacionamentosSpec(itemRelacionamentoService, new RelacionamentoItemPrincipal(), true);
            var actual = target.IsSatisfiedBy(new ItemDetalhe());

            itemRelacionamentoService.VerifyAllExpectations();

            Assert.IsTrue(actual.Satisfied);
        }
    }
}