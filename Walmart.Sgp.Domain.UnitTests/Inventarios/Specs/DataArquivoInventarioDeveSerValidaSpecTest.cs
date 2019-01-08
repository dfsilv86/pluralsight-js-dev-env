using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walmart.Sgp.Domain.Inventarios;
using Walmart.Sgp.Infrastructure.IO.Importing.Inventario.Specs;

namespace Walmart.Sgp.Domain.UnitTests.Inventarios.Specs
{
    [TestFixture]
    [Category("Domain")]
    public class DataArquivoInventarioDeveSerValidaSpecTest
    {
        [Test]
        public void DataArquivoInventarioDeveSerValidaSpec_IsSatisfiedBy_Satisfied()
        {
            var target = new DataArquivoInventarioDeveSerValidaSpec(DateTime.Today, 1, "teste");

            Assert.IsTrue(target.IsSatisfiedBy(new ArquivoInventario(0, "teste", DateTime.Today) { DataArquivo = DateTime.Now }));
            Assert.IsTrue(target.IsSatisfiedBy(new ArquivoInventario(0, "teste", DateTime.Today) { DataArquivo = DateTime.Today }));
            Assert.IsTrue(target.IsSatisfiedBy(new ArquivoInventario(0, "teste", DateTime.Today) { DataArquivo = DateTime.Now.AddDays(-1) }));
            Assert.IsTrue(target.IsSatisfiedBy(new ArquivoInventario(0, "teste", DateTime.Today) { DataArquivo = DateTime.Today.AddDays(-1) }));
            Assert.IsTrue(target.IsSatisfiedBy(new ArquivoInventario(0, "teste", DateTime.Today) { DataArquivo = DateTime.Now.AddDays(1) }));
            Assert.IsTrue(target.IsSatisfiedBy(new ArquivoInventario(0, "teste", DateTime.Today) { DataArquivo = DateTime.Today.AddDays(1) }));
            Assert.IsTrue(target.IsSatisfiedBy(new ArquivoInventario(0, "teste", DateTime.Today)));
            Assert.IsTrue(target.IsSatisfiedBy(new ArquivoInventario(0, "teste", DateTime.Today) { DataArquivo = new DateTime(9999, 12, 31) }));
        }

        [Test]
        public void DataArquivoInventarioDeveSerValidaSpec_IsSatisfiedBy_NotSatisfied()
        {
            var target = new DataArquivoInventarioDeveSerValidaSpec(DateTime.Today, 1, "teste");

            Assert.IsFalse(target.IsSatisfiedBy(new ArquivoInventario(0, "teste", DateTime.Today) { DataArquivo = DateTime.Now.AddDays(-2) }));
            Assert.IsFalse(target.IsSatisfiedBy(new ArquivoInventario(0, "teste", DateTime.Today) { DataArquivo = DateTime.Today.AddDays(-2) }));
            Assert.IsFalse(target.IsSatisfiedBy(new ArquivoInventario(0, "teste", DateTime.Today) { DataArquivo = DateTime.Now.AddDays(2) }));
            Assert.IsFalse(target.IsSatisfiedBy(new ArquivoInventario(0, "teste", DateTime.Today) { DataArquivo = DateTime.Today.AddDays(2) }));
        }
    }
}
