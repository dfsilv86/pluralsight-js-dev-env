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
using Walmart.Sgp.Domain.Inventarios.Specs;
using Walmart.Sgp.Infrastructure.Framework.Commons;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Gerenciamento;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class InventarioPodeSerPreparadoSpecTest
    {
        [Test]
        public void IsSatisfiedBy_NaoExistemInventariosParaImportacao_False()
        {
            var inventarioService = MockRepository.GenerateMock<IInventarioService>();
            inventarioService.Expect(e => e.ObterInventariosAbertosParaImportacao(1, null, 2, null)).IgnoreArguments().Return(new List<Inventario>());

            var target = new InventarioPodeSerPreparadoSpec(inventarioService);
            var actual = target.IsSatisfiedBy(new Inventario
            {
                IDLoja = 1,
                IDDepartamento = 2
            });

            Assert.IsFalse(actual.Satisfied);
            Assert.AreEqual(Texts.ThereAreNoOpenInventoryToPrepareInventory, actual.Reason);
        }

        [Test]
        public void IsSatisfiedBy_ExistemInventariosParaImportacao_True()
        {
            var inventarioService = MockRepository.GenerateMock<IInventarioService>();
            inventarioService.Expect(e => e.ObterInventariosAbertosParaImportacao(1, null, 2, null)).IgnoreArguments().Return(new List<Inventario> 
            { 
                new Inventario
                {
                    IDInventario = 1,
                    IDDepartamento = 2
                }
            });

            var target = new InventarioPodeSerPreparadoSpec(inventarioService);
            var actual = target.IsSatisfiedBy(new Inventario
            {
                IDLoja = 1,                
                IDDepartamento = 2
            });

            Assert.IsTrue(actual.Satisfied);
        }
    }
}