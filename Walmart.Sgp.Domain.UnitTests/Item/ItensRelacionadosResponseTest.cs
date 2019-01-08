using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Item;

namespace Walmart.Sgp.Domain.UnitTests.Item
{
    [TestFixture]
    [Category("Domain")]
    public class ItensRelacionadosResponseTest
    {
        [Test]
        public void Constructor_Args_ItensRelacionadosResponse()
        {
            var entrada = new dynamic[0];
            var saida = new dynamic[0];
            var derivado = new dynamic[0];
            var insumo = new dynamic[0];
            var transformado = new dynamic[0];

            ItensRelacionadosResponse target = new ItensRelacionadosResponse(entrada, derivado, insumo, saida, transformado);

            Assert.IsNotNull(target);
            Assert.AreEqual(entrada, target.Entrada);
            Assert.AreEqual(saida, target.Saida);
            Assert.AreEqual(derivado, target.Derivado);
            Assert.AreEqual(insumo, target.Insumo);
            Assert.AreEqual(transformado, target.Transformado);
        }
    }
}
