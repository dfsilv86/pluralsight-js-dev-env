using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Domain.Inventarios.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class ArquivoInventarioDeveSerOMaisRecenteSpecTest
    {
        [Test]
        public void ArquivoInventarioDeveSerOMaisRecenteSpec_IsSatisfiedBy_Satisfied()
        {
            var inventario = new Inventario { dhInventarioArquivo = DateTime.Today.AddDays(-100), stInventario = InventarioStatus.Importado };
            var arquivo = new ArquivoInventario(0, null, DateTime.Today) { DataArquivo = DateTime.Today };

            var target = new ArquivoInventarioDeveSerOMaisRecenteSpec(inventario, "teste");

            Assert.IsTrue(target.IsSatisfiedBy(arquivo));

            inventario = new Inventario { dhInventarioArquivo = null, stInventario = InventarioStatus.Aberto };

            Assert.IsTrue(target.IsSatisfiedBy(arquivo));
        }

        [Test]
        public void ArquivoInventarioDeveSerOMaisRecenteSpec_IsSatisfiedBy_NotSatisfied()
        {
            var inventario = new Inventario { dhInventarioArquivo = DateTime.Today.AddDays(-1), stInventario = InventarioStatus.Importado };
            var arquivo = new ArquivoInventario(0, null, DateTime.Today) { DataArquivo = DateTime.Today.AddDays(-2) };

            var target = new ArquivoInventarioDeveSerOMaisRecenteSpec(inventario, "teste");

            Assert.IsFalse(target.IsSatisfiedBy(arquivo));
        }
    }
}
